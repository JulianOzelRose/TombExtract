using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Forms;
using static TombExtract.MainForm;

namespace TombExtract
{
    class TR1Utilities
    {
        // Paths
        private string savegameSourcePath;
        private string savegameDestinationPath;

        // Offsets
        private const int SLOT_STATUS_OFFSET = 0x000;
        private const int NEW_GAME_PLUS_OFFSET = 0x004;
        private const int SAVE_NUMBER_OFFSET = 0x008;
        private const int LEVEL_INDEX_OFFSET_PREPATCH = 0x628;

        // Platform or patch-dependent offsets
        private int SOURCE_LEVEL_INDEX_OFFSET;
        private int DESTINATION_LEVEL_INDEX_OFFSET;
        private int SOURCE_CHALLENGE_MODE_OFFSET;
        private int DESTINATION_CHALLENGE_MODE_OFFSET;
        private int SOURCE_SAVEGAME_VERSION_OFFSET;

        // PC offsets
        private const int LEVEL_INDEX_OFFSET_PC = 0x628;
        private const int SAVEGAME_VERSION_OFFSET_PC = 0x6E0;
        private const int CHALLENGE_MODE_OFFSET_PC = 0x6E8;

        // Mobile offsets
        private const int LEVEL_INDEX_OFFSET_MOBILE = 0x658;
        private const int SAVEGAME_VERSION_OFFSET_MOBILE = 0x70C;
        private const int CHALLENGE_MODE_OFFSET_MOBILE = 0x714;

        // Console offsets
        private const int LEVEL_INDEX_OFFSET_CONSOLE = 0x628;
        private const int SAVEGAME_VERSION_OFFSET_CONSOLE = 0x6DC;
        private const int CHALLENGE_MODE_OFFSET_CONSOLE = 0x6E4;

        // Savegame constants
        private int SOURCE_BASE_SAVEGAME_OFFSET_TR1;
        private int DESTINATION_BASE_SAVEGAME_OFFSET_TR1;
        private int SOURCE_SAVEGAME_SIZE;
        private int DESTINATION_SAVEGAME_SIZE;

        // Patch-specific
        private const int BASE_SAVEGAME_OFFSET_TR1_PREPATCH = 0x2004;
        private const int BASE_SAVEGAME_OFFSET_TR1_PATCH5 = 0x2004;

        // Entity block
        private const int ENTITY_BLOCK_START_PC = 0x6F0;
        private const int ENTITY_BLOCK_START_MOBILE = 0x72B;
        private const int ENTITY_BLOCK_START_CONSOLE = 0x6EC;

        // Misc
        private int totalSavegames = 0;
        private BackgroundWorker bgWorker;
        private ProgressForm progressForm;
        private bool isWriting = false;
        private bool isSourcePrepatch;
        private bool isDestinationPatch5;
        private bool NO_CONVERT = false;
        private readonly IWin32Window owner;

        // Platform
        Platform sourcePlatform;
        Platform destinationPlatform;

        public TR1Utilities(IWin32Window owner)
        {
            this.owner = owner;
        }

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            try
            {
                if (string.IsNullOrEmpty(savegameSourcePath) || !File.Exists(savegameSourcePath))
                {
                    return;
                }

                byte[] fileData = File.ReadAllBytes(savegameSourcePath);

                bool isPatch5 = IsPatch5SavegameFile(fileData);

                if (isPatch5)
                {
                    SOURCE_SAVEGAME_SIZE = Globals.SAVEGAME_SIZE_TRX_PATCH5;
                    SOURCE_BASE_SAVEGAME_OFFSET_TR1 = BASE_SAVEGAME_OFFSET_TR1_PATCH5;

                    if (sourcePlatform == Platform.PC)
                    {
                        SOURCE_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_PC;
                        SOURCE_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_PC;
                        SOURCE_SAVEGAME_VERSION_OFFSET = SAVEGAME_VERSION_OFFSET_PC;
                    }
                    else if (sourcePlatform.IsMobile())
                    {
                        SOURCE_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_MOBILE;
                        SOURCE_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_MOBILE;
                        SOURCE_SAVEGAME_VERSION_OFFSET = SAVEGAME_VERSION_OFFSET_MOBILE;
                    }
                    else if (sourcePlatform.IsConsole())
                    {
                        SOURCE_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_CONSOLE;
                        SOURCE_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_CONSOLE;
                        SOURCE_SAVEGAME_VERSION_OFFSET = SAVEGAME_VERSION_OFFSET_CONSOLE;
                    }
                }
                else
                {
                    SOURCE_SAVEGAME_SIZE = Globals.SAVEGAME_SIZE_TRX_PREPATCH;
                    SOURCE_BASE_SAVEGAME_OFFSET_TR1 = BASE_SAVEGAME_OFFSET_TR1_PREPATCH;
                    SOURCE_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_PREPATCH;
                }

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = SOURCE_BASE_SAVEGAME_OFFSET_TR1 + (i * SOURCE_SAVEGAME_SIZE);

                    Int16 levelIndex = BitConverter.ToInt16(fileData, currentSavegameOffset + SOURCE_LEVEL_INDEX_OFFSET);
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR1.TryGetValue(levelIndex, out string levelName))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        bool isChallengeMode = fileData[currentSavegameOffset + SOURCE_CHALLENGE_MODE_OFFSET] == 1 && isPatch5;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, false, isChallengeMode);
                        cklSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    owner,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void PopulateDestinationSavegames(ListBox lstSavegames)
        {
            lstSavegames.Items.Clear();

            try
            {
                if (string.IsNullOrEmpty(savegameDestinationPath) || !File.Exists(savegameDestinationPath))
                {
                    return;
                }

                byte[] fileData = File.ReadAllBytes(savegameDestinationPath);

                bool isPatch5 = IsPatch5SavegameFile(fileData);

                if (isPatch5)
                {
                    DESTINATION_SAVEGAME_SIZE = Globals.SAVEGAME_SIZE_TRX_PATCH5;
                    DESTINATION_BASE_SAVEGAME_OFFSET_TR1 = BASE_SAVEGAME_OFFSET_TR1_PATCH5;

                    if (destinationPlatform == Platform.PC)
                    {
                        DESTINATION_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_PC;
                        DESTINATION_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_PC;
                    }
                    else if (destinationPlatform.IsMobile())
                    {
                        DESTINATION_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_MOBILE;
                        DESTINATION_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_MOBILE;
                    }
                    else if (destinationPlatform.IsConsole())
                    {
                        DESTINATION_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_CONSOLE;
                        DESTINATION_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_CONSOLE;
                    }
                }
                else
                {
                    DESTINATION_SAVEGAME_SIZE = Globals.SAVEGAME_SIZE_TRX_PREPATCH;
                    DESTINATION_BASE_SAVEGAME_OFFSET_TR1 = BASE_SAVEGAME_OFFSET_TR1_PREPATCH;
                    DESTINATION_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_PREPATCH;
                }

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = DESTINATION_BASE_SAVEGAME_OFFSET_TR1 + (i * DESTINATION_SAVEGAME_SIZE);

                    Int16 levelIndex = BitConverter.ToInt16(fileData, currentSavegameOffset + DESTINATION_LEVEL_INDEX_OFFSET);
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR1.TryGetValue(levelIndex, out string levelName))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        bool isChallengeMode = fileData[currentSavegameOffset + DESTINATION_CHALLENGE_MODE_OFFSET] == 1 && isPatch5;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, false, isChallengeMode);
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        lstSavegames.Items.Add(Globals.EMPTY_SLOT_TEXT);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    owner,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public int GetNumOverwrites(List<Savegame> savegames)
        {
            int numOverwrites = 0;

            try
            {
                byte[] fileData = File.ReadAllBytes(savegameDestinationPath);

                for (int i = 0; i < savegames.Count; i++)
                {
                    int slotIndex = (savegames[i].Offset - SOURCE_BASE_SAVEGAME_OFFSET_TR1) / SOURCE_SAVEGAME_SIZE;
                    int currentSavegameOffset = DESTINATION_BASE_SAVEGAME_OFFSET_TR1 + (slotIndex * DESTINATION_SAVEGAME_SIZE);

                    Int16 levelIndex = BitConverter.ToInt16(fileData, currentSavegameOffset + DESTINATION_LEVEL_INDEX_OFFSET);
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        numOverwrites++;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    owner,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return 0;
            }

            return numOverwrites;
        }

        public void WriteSavegamesToDestination(List<Savegame> savegames, ListBox lstDestinationSavegames, ToolStripStatusLabel slblStatus)
        {
            isWriting = true;

            byte[] sourceFileData = File.ReadAllBytes(savegameSourcePath);
            isSourcePrepatch = IsPrepatchSavegameFile(sourceFileData);

            byte[] destinationFileData = File.ReadAllBytes(savegameDestinationPath);
            isDestinationPatch5 = IsPatch5SavegameFile(destinationFileData);

            if (isSourcePrepatch && isDestinationPatch5)
            {
                NO_CONVERT = false;
            }
            else if (sourcePlatform == destinationPlatform)
            {
                NO_CONVERT = true;
            }
            else
            {
                NO_CONVERT = false;
            }

            totalSavegames = savegames.Count;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += WriteSavegamesBackground;

            bgWorker.RunWorkerCompleted += (sender, e) => bgWorker_RunWorkerCompleted(sender, e, lstDestinationSavegames, slblStatus);

            bgWorker.ProgressChanged += UpdateProgressBar;

            slblStatus.Text = NO_CONVERT ? Globals.STATUS_MSG_TRANSFER_IN_PROGRESS : Globals.STATUS_MSG_CONVERSION_IN_PROGRESS;

            bgWorker.RunWorkerAsync(savegames);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, ListBox lstDestinationSavegames, ToolStripStatusLabel slblStatus)
        {
            progressForm.Close();
            isWriting = false;

            if (e.Error != null || (e.Result != null && e.Result is Exception))
            {
                slblStatus.Text = NO_CONVERT ? Globals.STATUS_MSG_TRANSFER_ERROR : Globals.STATUS_MSG_CONVERSION_ERROR;

                Exception exception = e.Error as Exception ?? e.Result as Exception;
                string errorMessage = e.Error != null ? e.Error.Message : exception.Message;

                SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    owner,
                    errorMessage,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                slblStatus.Text = NO_CONVERT ? Globals.STATUS_MSG_TRANSFER_CANCELED : Globals.STATUS_MSG_CONVERSION_CANCELED;

                SystemSounds.Asterisk.Play();

                ThemedMessageBox.Show(
                    owner,
                    Globals.DIALOG_MSG_OPERATION_CANCELED,
                    Globals.DIALOG_TITLE_CANCELED,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                slblStatus.Text = $"Successfully {(NO_CONVERT ? "transferred " : "converted and transferred ")}" +
                    $"{totalSavegames} savegame(s) to destination file";

                SystemSounds.Asterisk.Play();

                string dialogMessage = $"Successfully {(NO_CONVERT ? "transferred " : "converted and transferred ")}" +
                    $"{totalSavegames} savegame(s) to destination file.";

                ThemedMessageBox.Show(
                    owner,
                    dialogMessage,
                    Globals.DIALOG_TITLE_SUCCESS,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            PopulateDestinationSavegames(lstDestinationSavegames);
        }

        private void WriteSavegamesBackground(object sender, DoWorkEventArgs e)
        {
            List<Savegame> savegames = e.Argument as List<Savegame>;

            try
            {
                using (FileStream sourceFile = new FileStream(savegameSourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    int savegamesCopied = 0;

                    for (int i = 0; i < savegames.Count; i++)
                    {
                        progressForm.UpdateStatusMessage($"Copying '{savegames[i]}'...");

                        int currentSavegameOffset = savegames[i].Offset;
                        byte[] savegameBytes = new byte[SOURCE_SAVEGAME_SIZE];

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SOURCE_SAVEGAME_SIZE; offset++, j++)
                        {
                            sourceFile.Seek(offset, SeekOrigin.Begin);
                            byte currentByte = (byte)sourceFile.ReadByte();
                            savegameBytes[j] = currentByte;
                        }

                        savegames[i].SavegameBytes = savegameBytes;

                        savegamesCopied++;

                        int copyProgress = (int)((double)savegamesCopied / totalSavegames * 50);
                        bgWorker.ReportProgress(copyProgress);
                    }
                }

                File.SetAttributes(savegameDestinationPath, File.GetAttributes(savegameDestinationPath) & ~FileAttributes.ReadOnly);

                using (FileStream destinationFile = new FileStream(savegameDestinationPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    int savegamesWritten = 0;

                    for (int i = 0; i < savegames.Count; i++)
                    {
                        progressForm.UpdateStatusMessage($"Copying '{savegames[i]}'...");

                        int slotIndex = (savegames[i].Offset - SOURCE_BASE_SAVEGAME_OFFSET_TR1) / SOURCE_SAVEGAME_SIZE;
                        int currentSavegameOffset = DESTINATION_BASE_SAVEGAME_OFFSET_TR1 + (slotIndex * DESTINATION_SAVEGAME_SIZE);
                        byte[] savegameBytes = savegames[i].SavegameBytes;

                        // Clear destination savegame slot before writing
                        byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                        destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                        destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                        if (sourcePlatform == Platform.PC && destinationPlatform == Platform.PC)
                        {
                            if (isSourcePrepatch && isDestinationPatch5)    // PRE-PATCH -> PATCH 5
                            {
                                progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6DD && currentRelativeOffset <= Globals.SAVEGAME_SIZE_TRX_PREPATCH)
                                    {
                                        destinationFile.Seek(offset + 0x13, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                            else if (!isSourcePrepatch && !isDestinationPatch5)  // PATCH 5 -> PRE-PATCH
                            {
                                progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                                bool isNativePatch5Savegame = BitConverter.ToUInt32(savegameBytes, SOURCE_SAVEGAME_VERSION_OFFSET) >= 2;

                                if (isNativePatch5Savegame)
                                {
                                    savegameBytes = ConvertNativePatch5EntityBlockToPrepatchFormat(savegameBytes);
                                }

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6F0 && currentRelativeOffset <= Globals.SAVEGAME_SIZE_TRX_PREPATCH)
                                    {
                                        destinationFile.Seek(offset - 0x13, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                            else    // NO CONVERT
                            {
                                progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    destinationFile.Seek(offset, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                            }
                        }
                        else if (sourcePlatform == Platform.PC && destinationPlatform.IsConsole())     // PC -> Console
                        {
                            if (isSourcePrepatch && !isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to {destinationPlatform.ToFriendlyString()}...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x64A && currentRelativeOffset < 0x6AC)
                                    {
                                        destinationFile.Seek(offset - 1, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x6AC)
                                    {
                                        destinationFile.Seek(offset - 4, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                            else if (!isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to {destinationPlatform.ToFriendlyString()}...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6AC)
                                    {
                                        destinationFile.Seek(offset - 4, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                            else if (isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to {destinationPlatform.ToFriendlyString()}...");

                                // INTERMEDIATE PATCH 5 PC BUFFER
                                byte[] migratedPatch5Buffer = new byte[Globals.SAVEGAME_SIZE_TRX_PATCH5];

                                // PREPATCH -> PATCH 5 PC MIGRATION
                                for (int j = 0; j < Globals.SAVEGAME_SIZE_TRX_PREPATCH; j++)
                                {
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;

                                    if (j >= 0x6DD)
                                    {
                                        migratedPatch5Buffer[j + 0x13] = value;
                                    }
                                    else
                                    {
                                        migratedPatch5Buffer[j] = value;
                                    }
                                }

                                // PATCH 5 PC -> PS4
                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < migratedPatch5Buffer.Length ? migratedPatch5Buffer[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6AC)
                                    {
                                        destinationFile.Seek(offset - 4, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                        }
                        else if (sourcePlatform.IsConsole() && destinationPlatform == Platform.PC)     // Console -> PC
                        {
                            if (isSourcePrepatch && !isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PC...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x64A && currentRelativeOffset < 0x6AC)
                                    {
                                        destinationFile.Seek(offset + 1, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x6AC && currentRelativeOffset < DESTINATION_SAVEGAME_SIZE - 4)
                                    {
                                        destinationFile.Seek(offset + 4, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                            else if (!isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PC...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6AC && currentRelativeOffset < DESTINATION_SAVEGAME_SIZE - 4)
                                    {
                                        destinationFile.Seek(offset + 4, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                            else if (isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PC...");

                                // INTERMEDIATE PREPATCH PC BUFFER
                                byte[] migratedPrepatchBuffer = new byte[Globals.SAVEGAME_SIZE_TRX_PREPATCH];

                                // CONSOLE -> PREPATCH PC
                                for (int j = 0; j < Globals.SAVEGAME_SIZE_TRX_PREPATCH; j++)
                                {
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;

                                    if (j >= 0x64A && j < 0x6AC)
                                    {
                                        migratedPrepatchBuffer[j + 1] = value;
                                    }
                                    else if (j >= 0x6AC && j < Globals.SAVEGAME_SIZE_TRX_PREPATCH - 4)
                                    {
                                        migratedPrepatchBuffer[j + 4] = value;
                                    }
                                    else
                                    {
                                        migratedPrepatchBuffer[j] = value;
                                    }
                                }

                                // PREPATCH PC -> PATCH 5 PC
                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < migratedPrepatchBuffer.Length ? migratedPrepatchBuffer[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6DD && currentRelativeOffset <= Globals.SAVEGAME_SIZE_TRX_PREPATCH)
                                    {
                                        destinationFile.Seek(offset + 0x13, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }
                            }
                        }
                        else if (sourcePlatform.IsMobile() && destinationPlatform == Platform.PC)     // Mobile -> PC
                        {
                            progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PC...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x60C && currentRelativeOffset < 0x692)
                                {
                                    destinationFile.Seek(offset - 0x30, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else if (currentRelativeOffset >= 0x692 && currentRelativeOffset < 0x727)
                                {
                                    destinationFile.Seek(offset - 0x2C, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else if (currentRelativeOffset >= 0x727 && currentRelativeOffset < Globals.SAVEGAME_SIZE_TRX_PATCH5)
                                {
                                    destinationFile.Seek(offset - 0x3B, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else
                                {
                                    destinationFile.Seek(offset, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                            }

                            // Force correct level index
                            Int16 levelIndex = BitConverter.ToInt16(savegameBytes, LEVEL_INDEX_OFFSET_MOBILE);

                            destinationFile.Seek(currentSavegameOffset + LEVEL_INDEX_OFFSET_PC, SeekOrigin.Begin);
                            destinationFile.Write(BitConverter.GetBytes(levelIndex), 0, sizeof(Int16));
                        }
                        else if (sourcePlatform.IsMobile() && destinationPlatform.IsMobile())  // Mobile -> Mobile
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to {destinationPlatform.ToFriendlyString()}...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }
                        else if (sourcePlatform.IsConsole() && destinationPlatform.IsConsole()) // Console -> Console
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to {destinationPlatform.ToFriendlyString()}...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }
                        else if (sourcePlatform == Platform.PC && destinationPlatform.IsMobile())     // PC -> Mobile
                        {
                            if (isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to {destinationPlatform.ToFriendlyString()}...");

                                // INTERMEDIATE PATCH 5 PC BUFFER
                                byte[] migratedPatch5Buffer = new byte[Globals.SAVEGAME_SIZE_TRX_PATCH5];

                                // PREPATCH -> PATCH 5 PC MIGRATION
                                for (int j = 0; j < Globals.SAVEGAME_SIZE_TRX_PREPATCH; j++)
                                {
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;

                                    if (j >= 0x6DD)
                                    {
                                        migratedPatch5Buffer[j + 0x13] = value;
                                    }
                                    else
                                    {
                                        migratedPatch5Buffer[j] = value;
                                    }
                                }

                                // PATCH 5 PC -> MOBILE
                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;

                                    byte value = j < migratedPatch5Buffer.Length ? migratedPatch5Buffer[j] : (byte)0;

                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x60C && currentRelativeOffset < 0x692)
                                    {
                                        destinationFile.Seek(offset + 0x30, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x692 && currentRelativeOffset < 0x6FC)
                                    {
                                        destinationFile.Seek(offset + 0x2C, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x6FC && currentRelativeOffset < DESTINATION_SAVEGAME_SIZE - 0x3B)
                                    {
                                        destinationFile.Seek(offset + 0x3B, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }

                                // Force correct level index
                                Int16 levelIndex = BitConverter.ToInt16(migratedPatch5Buffer, LEVEL_INDEX_OFFSET_PC);

                                destinationFile.Seek(currentSavegameOffset + LEVEL_INDEX_OFFSET_MOBILE, SeekOrigin.Begin);
                                destinationFile.Write(BitConverter.GetBytes(levelIndex), 0, sizeof(Int16));
                            }
                            else if (!isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to {destinationPlatform.ToFriendlyString()}...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x60C && currentRelativeOffset < 0x692)
                                    {
                                        destinationFile.Seek(offset + 0x30, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x692 && currentRelativeOffset < 0x6FC)
                                    {
                                        destinationFile.Seek(offset + 0x2C, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x6FC && currentRelativeOffset < DESTINATION_SAVEGAME_SIZE - 0x3B)
                                    {
                                        destinationFile.Seek(offset + 0x3B, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else
                                    {
                                        destinationFile.Seek(offset, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                }

                                // Force correct level index
                                Int16 levelIndex = BitConverter.ToInt16(savegameBytes, LEVEL_INDEX_OFFSET_PC);

                                destinationFile.Seek(currentSavegameOffset + LEVEL_INDEX_OFFSET_MOBILE, SeekOrigin.Begin);
                                destinationFile.Write(BitConverter.GetBytes(levelIndex), 0, sizeof(Int16));
                            }
                        }
                        else    // NO CONVERT
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }

                        savegamesWritten++;

                        int writeProgress = (int)((double)savegamesWritten / totalSavegames * 50);
                        bgWorker.ReportProgress(50 + writeProgress);
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private static void CopyBytes(byte[] source, byte[] destination, ref int sourceCursor, ref int destinationCursor, int length)
        {
            Array.Copy(source, sourceCursor, destination, destinationCursor, length);

            sourceCursor += length;
            destinationCursor += length;
        }

        private int GetEntityBlockStart()
        {
            if (sourcePlatform == Platform.PC)
            {
                return ENTITY_BLOCK_START_PC;
            }
            else if (sourcePlatform.IsMobile())
            {
                return ENTITY_BLOCK_START_MOBILE;
            }
            else if (sourcePlatform.IsConsole())
            {
                return ENTITY_BLOCK_START_CONSOLE;
            }

            return ENTITY_BLOCK_START_PC;
        }

        private byte[] ConvertNativePatch5EntityBlockToPrepatchFormat(byte[] source)
        {
            byte[] destination = new byte[Globals.SAVEGAME_SIZE_TRX_PATCH5];

            int entityBlockStart = GetEntityBlockStart();

            Array.Copy(source, destination, entityBlockStart);

            Int16 levelIndex = BitConverter.ToInt16(source, SOURCE_LEVEL_INDEX_OFFSET);

            var levelObjectIds = new List<int>(TR1EntityCache.LevelObjectIdsByLevel[levelIndex]);

            if (!TR1EntityCache.TR1ObjectsByLevel.TryGetValue(levelIndex, out var levelObjects))
            {
                throw new Exception($"{Globals.ERROR_MSG_MISSING_LEVEL_DEFINITION} {levelIndex}.");
            }

            int sourceCursor = entityBlockStart;
            int destinationCursor = entityBlockStart;

            CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, 4);
            CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, 0x118);

            int stateCount = TR1EntityCache.LevelStateEntryCounts[levelIndex];

            CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, stateCount * 2);

            sourceCursor += 4;

            foreach (int objectId in levelObjectIds)
            {
                sourceCursor += 4;

                if (!levelObjects.TryGetValue(objectId, out var tr1Object))
                {
                    throw new Exception($"{Globals.ERROR_MSG_MISSING_OBJECT_DEFINITION} (object ID: 0x{objectId:X}).");
                }

                if ((tr1Object.Flags00 & 0x08) != 0)
                {
                    CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, 0x1A);
                }

                if ((tr1Object.Flags00 & 0x40) != 0)
                {
                    CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, 0x0A);
                }

                if ((tr1Object.Flags00 & 0x10) != 0)
                {
                    CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, 0x02);
                }

                if ((tr1Object.Flags00 & 0x20) != 0)
                {
                    bool has02 = (tr1Object.Flags00 & 0x02) != 0;

                    CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, has02 ? 0x10 : 0x04);
                    CopyBytes(source, destination, ref sourceCursor, ref destinationCursor, 0x10);
                }
            }

            int remainingBytes = source.Length - sourceCursor;

            Array.Copy(source, sourceCursor, destination, destinationCursor, remainingBytes);

            return destination;
        }

        private bool IsPrepatchSavegameFile(byte[] fileData)
        {
            return BitConverter.ToUInt32(fileData, Globals.SAVEFILE_VERSION_OFFSET) == Globals.SAVEFILE_TRX_PREPATCH;
        }

        private bool IsPatch5SavegameFile(byte[] fileData)
        {
            return BitConverter.ToUInt32(fileData, Globals.SAVEFILE_VERSION_OFFSET) == Globals.SAVEFILE_TRX_PATCH5;
        }

        public bool IsNativePatch5Savegame(byte[] fileData, Savegame savegame)
        {
            UInt32 savegameVersion = BitConverter.ToUInt32(fileData, savegame.Offset + SOURCE_SAVEGAME_VERSION_OFFSET);
            return savegameVersion >= 2;
        }

        public bool IsWriting()
        {
            return isWriting;
        }

        private void UpdateProgressBar(object sender, ProgressChangedEventArgs e)
        {
            progressForm.UpdateProgressBar(e.ProgressPercentage);
            progressForm.UpdatePercentage(e.ProgressPercentage);
        }

        public void SetProgressForm(ProgressForm progressForm)
        {
            this.progressForm = progressForm;
        }

        public void SetSavegameSourcePath(string path)
        {
            savegameSourcePath = path;
        }

        public void SetSavegameDestinationPath(string path)
        {
            savegameDestinationPath = path;
        }

        public void SetSourceFormat(Platform platform)
        {
            sourcePlatform = platform;
        }

        public void SetDestinationFormat(Platform platform)
        {
            destinationPlatform = platform;
        }
    }
}
