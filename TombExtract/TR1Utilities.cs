using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        private const int NEW_GAME_PLUS_OFFSET = 0x008;
        private const int SAVE_NUMBER_OFFSET = 0x00C;
        private const int LEVEL_INDEX_OFFSET_PREPATCH = 0x62C;

        // Platform or patch-dependent offsets
        private int SOURCE_LEVEL_INDEX_OFFSET;
        private int DESTINATION_LEVEL_INDEX_OFFSET;
        private int SOURCE_CHALLENGE_MODE_OFFSET;
        private int DESTINATION_CHALLENGE_MODE_OFFSET;
        private int SOURCE_SAVEGAME_VERSION_OFFSET;

        // PC offsets
        private const int LEVEL_INDEX_OFFSET_PC = 0x62C;
        private const int SAVEGAME_VERSION_OFFSET_PC = 0x6E4;
        private const int CHALLENGE_MODE_OFFSET_PC = 0x6EC;

        // Android offsets
        private const int LEVEL_INDEX_OFFSET_ANDROID = 0x65C;
        private const int SAVEGAME_VERSION_OFFSET_ANDROID = 0x710;
        private const int CHALLENGE_MODE_OFFSET_ANDROID = 0x718;

        // PS4 offsets
        private const int LEVEL_INDEX_OFFSET_PS4 = 0x62C;
        private const int SAVEGAME_VERSION_OFFSET_PS4 = 0x6E0;
        private const int CHALLENGE_MODE_OFFSET_PS4 = 0x6E8;

        // Savegame constants
        private int SOURCE_BASE_SAVEGAME_OFFSET_TR1;
        private int DESTINATION_BASE_SAVEGAME_OFFSET_TR1;
        private int SOURCE_SAVEGAME_SIZE;
        private int DESTINATION_SAVEGAME_SIZE;

        // Patch-specific
        private const int BASE_SAVEGAME_OFFSET_TR1_PREPATCH = 0x2000;
        private const int BASE_SAVEGAME_OFFSET_TR1_PATCH5 = 0x2000;

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
                    else if (sourcePlatform == Platform.Android)
                    {
                        SOURCE_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_ANDROID;
                        SOURCE_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_ANDROID;
                        SOURCE_SAVEGAME_VERSION_OFFSET = SAVEGAME_VERSION_OFFSET_ANDROID;
                    }
                    else if (sourcePlatform == Platform.PlayStation4)
                    {
                        SOURCE_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_PS4;
                        SOURCE_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_PS4;
                        SOURCE_SAVEGAME_VERSION_OFFSET = SAVEGAME_VERSION_OFFSET_PS4;
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

                    byte levelIndex = fileData[currentSavegameOffset + SOURCE_LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        bool isChallengeMode = fileData[currentSavegameOffset + SOURCE_CHALLENGE_MODE_OFFSET] == 1 && isPatch5;
                        string levelName = LevelNames.TR1[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, false, isChallengeMode);
                        cklSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

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
                    else if (destinationPlatform == Platform.Android)
                    {
                        DESTINATION_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_ANDROID;
                        DESTINATION_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_ANDROID;
                    }
                    else if (destinationPlatform == Platform.PlayStation4)
                    {
                        DESTINATION_LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_PS4;
                        DESTINATION_CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_PS4;
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

                    byte levelIndex = fileData[currentSavegameOffset + DESTINATION_LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        bool isChallengeMode = fileData[currentSavegameOffset + DESTINATION_CHALLENGE_MODE_OFFSET] == 1 && isPatch5;
                        string levelName = LevelNames.TR1[levelIndex];

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
                System.Media.SystemSounds.Hand.Play();

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

                    byte levelIndex = fileData[currentSavegameOffset + DESTINATION_LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        numOverwrites++;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

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

                System.Media.SystemSounds.Hand.Play();

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

                System.Media.SystemSounds.Asterisk.Play();

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

                System.Media.SystemSounds.Asterisk.Play();

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

                        if (sourcePlatform == Platform.PC && destinationPlatform == Platform.PC)
                        {
                            if (isSourcePrepatch && isDestinationPatch5)    // PRE-PATCH -> PATCH 5
                            {
                                progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                                byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                                destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                                destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6E0 && currentRelativeOffset <= Globals.SAVEGAME_SIZE_TRX_PREPATCH)
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

                                byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                                destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                                destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6F3 && currentRelativeOffset <= Globals.SAVEGAME_SIZE_TRX_PREPATCH)
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
                        else if (sourcePlatform == Platform.PC && destinationPlatform == Platform.PlayStation4)     // PC -> PS4
                        {
                            if (isSourcePrepatch && !isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PS4...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                                    {
                                        destinationFile.Seek(offset - 1, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x6B0)
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
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PS4...");

                                byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                                destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                                destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6B0)
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
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PS4...");

                                byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                                destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                                destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                                // INTERMEDIATE PATCH 5 PC BUFFER
                                byte[] migratedPatch5Buffer = new byte[Globals.SAVEGAME_SIZE_TRX_PATCH5];

                                // PREPATCH -> PATCH 5 PC MIGRATION
                                for (int j = 0; j < Globals.SAVEGAME_SIZE_TRX_PREPATCH; j++)
                                {
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;

                                    if (j >= 0x6E0)
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

                                    if (currentRelativeOffset >= 0x6B0)
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
                        else if (sourcePlatform == Platform.PC && destinationPlatform == Platform.NintendoSwitch)  // PC -> Switch
                        {
                            progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to Nintendo Switch...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                                {
                                    destinationFile.Seek(offset - 1, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else if (currentRelativeOffset >= 0x6B0)
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
                        else if (sourcePlatform == Platform.PlayStation4 && destinationPlatform == Platform.PC)     // PS4 -> PC
                        {
                            if (isSourcePrepatch && !isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PC...");

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                                    {
                                        destinationFile.Seek(offset + 1, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x6B0)
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

                                byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                                destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                                destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x6B0)
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
                        }
                        else if (sourcePlatform == Platform.Android && destinationPlatform == Platform.PC)     // Android -> PC
                        {
                            progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PC...");

                            // ZERO BUFFER
                            byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                            destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                            destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x610 && currentRelativeOffset < 0x696)
                                {
                                    destinationFile.Seek(offset - 0x30, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else if (currentRelativeOffset >= 0x696 && currentRelativeOffset < 0x72B)
                                {
                                    destinationFile.Seek(offset - 0x2C, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else if (currentRelativeOffset >= 0x72B && currentRelativeOffset < Globals.SAVEGAME_SIZE_TRX_PATCH5)
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
                            destinationFile.Seek(currentSavegameOffset + LEVEL_INDEX_OFFSET_PC, SeekOrigin.Begin);
                            destinationFile.WriteByte(savegameBytes[LEVEL_INDEX_OFFSET_ANDROID]);
                        }
                        else if (sourcePlatform == Platform.PC && destinationPlatform == Platform.Android)     // PC -> Android
                        {
                            if (isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to Android...");

                                // ZERO BUFFER
                                byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                                destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                                destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                                // INTERMEDIATE PATCH 5 PC BUFFER
                                byte[] migratedPatch5Buffer = new byte[Globals.SAVEGAME_SIZE_TRX_PATCH5];

                                // PREPATCH -> PATCH 5 PC MIGRATION
                                for (int j = 0; j < Globals.SAVEGAME_SIZE_TRX_PREPATCH; j++)
                                {
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;

                                    if (j >= 0x6E0)
                                    {
                                        migratedPatch5Buffer[j + 0x13] = value;
                                    }
                                    else
                                    {
                                        migratedPatch5Buffer[j] = value;
                                    }
                                }

                                // PATCH 5 PC -> ANDROID
                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;

                                    byte value = j < migratedPatch5Buffer.Length ? migratedPatch5Buffer[j] : (byte)0;

                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x610 && currentRelativeOffset < 0x696)
                                    {
                                        destinationFile.Seek(offset + 0x30, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x696 && currentRelativeOffset < 0x700)
                                    {
                                        destinationFile.Seek(offset + 0x2C, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x700 && currentRelativeOffset < Globals.SAVEGAME_SIZE_TRX_PATCH5)
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
                                destinationFile.Seek(currentSavegameOffset + LEVEL_INDEX_OFFSET_ANDROID, SeekOrigin.Begin);
                                destinationFile.WriteByte(migratedPatch5Buffer[LEVEL_INDEX_OFFSET_PC]);
                            }
                            else if (!isSourcePrepatch && isDestinationPatch5)
                            {
                                progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to Android...");

                                // ZERO BUFFER
                                byte[] zeroBuffer = new byte[DESTINATION_SAVEGAME_SIZE];
                                destinationFile.Seek(currentSavegameOffset, SeekOrigin.Begin);
                                destinationFile.Write(zeroBuffer, 0, zeroBuffer.Length);

                                for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                                {
                                    int currentRelativeOffset = offset - currentSavegameOffset;
                                    byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                    byte[] currentByte = { value };

                                    if (currentRelativeOffset >= 0x610 && currentRelativeOffset < 0x696)
                                    {
                                        destinationFile.Seek(offset + 0x30, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x696 && currentRelativeOffset < 0x700)
                                    {
                                        destinationFile.Seek(offset + 0x2C, SeekOrigin.Begin);
                                        destinationFile.Write(currentByte, 0, currentByte.Length);
                                    }
                                    else if (currentRelativeOffset >= 0x700 && currentRelativeOffset < Globals.SAVEGAME_SIZE_TRX_PATCH5)
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
                                destinationFile.Seek(currentSavegameOffset + LEVEL_INDEX_OFFSET_ANDROID, SeekOrigin.Begin);
                                destinationFile.WriteByte(savegameBytes[LEVEL_INDEX_OFFSET_PC]);
                            }
                        }
                        else if (sourcePlatform == Platform.NintendoSwitch && destinationPlatform == Platform.PC)  // NS -> PC
                        {
                            progressForm.UpdateStatusMessage($"Converting '{savegames[i]}' to PC...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                                {
                                    destinationFile.Seek(offset + 1, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else if (currentRelativeOffset >= 0x6B0)
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

        private bool IsPrepatchSavegameFile(byte[] fileData)
        {
            return fileData[Globals.SAVEFILE_VERSION_OFFSET] == Globals.SAVEFILE_TRX_PREPATCH;
        }

        private bool IsPatch5SavegameFile(byte[] fileData)
        {
            return fileData[Globals.SAVEFILE_VERSION_OFFSET] == Globals.SAVEFILE_TRX_PATCH5;
        }

        public bool IsNativePatch5Savegame(byte[] fileData, Savegame savegame)
        {
            Int32 savegameVersion = BitConverter.ToInt32(fileData, savegame.Offset + SOURCE_SAVEGAME_VERSION_OFFSET);
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
