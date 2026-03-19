using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    class TR2Utilities
    {
        // Paths
        private string savegameSourcePath;
        private string savegameDestinationPath;

        // Offsets
        private const int SAVEGAME_VERSION_OFFSET = 0x000;
        private const int SLOT_STATUS_OFFSET = 0x004;
        private const int GAME_MODE_OFFSET = 0x008;
        private const int SAVE_NUMBER_OFFSET = 0x00C;
        private const int LEVEL_INDEX_OFFSET = 0x628;
        private const int CHALLENGE_MODE_OFFSET = 0x6B0;

        // Savegame constants
        private const int MAX_SAVEGAMES = 32;
        private int SOURCE_BASE_SAVEGAME_OFFSET_TR2;
        private int DESTINATION_BASE_SAVEGAME_OFFSET_TR2;
        private int SOURCE_SAVEGAME_SIZE;
        private int DESTINATION_SAVEGAME_SIZE;

        // Patch-specific
        private const byte PATCH5_SIGNATURE = 0x3C;
        private const int BASE_SAVEGAME_OFFSET_TR2_PREPATCH = 0x72000;
        private const int BASE_SAVEGAME_OFFSET_TR2_PATCH5 = 0xD2000;
        private const int SAVEGAME_SIZE_PREPATCH = 0x3800;
        private const int SAVEGAME_SIZE_PATCH5 = 0x6800;

        // Conversion
        private bool PS4_TO_PC = false;
        private bool PC_TO_PS4 = false;
        private bool SWITCH_TO_PC = false;
        private bool NO_CONVERT = false;
        private bool PC_TO_SWITCH = false;
        private bool PS4_TO_SWITCH = false;
        private bool SWITCH_TO_PS4 = false;
        private bool PREPATCH_TO_PATCH5 = false;

        // Misc
        private int totalSavegames = 0;
        private BackgroundWorker bgWorker;
        private ProgressForm progressForm;
        private bool isWriting = false;

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegameSourcePath);

                bool isPatch5 = IsPatch5Savegame(fileData);

                if (isPatch5)
                {
                    SOURCE_BASE_SAVEGAME_OFFSET_TR2 = BASE_SAVEGAME_OFFSET_TR2_PATCH5;
                    SOURCE_SAVEGAME_SIZE = SAVEGAME_SIZE_PATCH5;
                }
                else
                {
                    SOURCE_BASE_SAVEGAME_OFFSET_TR2 = BASE_SAVEGAME_OFFSET_TR2_PREPATCH;
                    SOURCE_SAVEGAME_SIZE = SAVEGAME_SIZE_PREPATCH;
                }

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = SOURCE_BASE_SAVEGAME_OFFSET_TR2 + (i * SOURCE_SAVEGAME_SIZE);

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR2.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;
                        bool isChallengeMode = fileData[currentSavegameOffset + CHALLENGE_MODE_OFFSET] == 1 && isPatch5;

                        string levelName = LevelNames.TR2[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode, false, isChallengeMode);
                        cklSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PopulateDestinationSavegames(ListBox lstSavegames)
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegameDestinationPath);

                bool isPatch5 = IsPatch5Savegame(fileData);

                if (isPatch5)
                {
                    DESTINATION_BASE_SAVEGAME_OFFSET_TR2 = BASE_SAVEGAME_OFFSET_TR2_PATCH5;
                    DESTINATION_SAVEGAME_SIZE = SAVEGAME_SIZE_PATCH5;
                }
                else
                {
                    DESTINATION_BASE_SAVEGAME_OFFSET_TR2 = BASE_SAVEGAME_OFFSET_TR2_PREPATCH;
                    DESTINATION_SAVEGAME_SIZE = SAVEGAME_SIZE_PREPATCH;
                }

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = DESTINATION_BASE_SAVEGAME_OFFSET_TR2 + (i * DESTINATION_SAVEGAME_SIZE);

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR2.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;
                        bool isChallengeMode = fileData[currentSavegameOffset + CHALLENGE_MODE_OFFSET] == 1 && isPatch5;

                        string levelName = LevelNames.TR2[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode, false, isChallengeMode);
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        lstSavegames.Items.Add("Empty Slot");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    int slotIndex = (savegames[i].Offset - SOURCE_BASE_SAVEGAME_OFFSET_TR2) / SOURCE_SAVEGAME_SIZE;
                    int currentSavegameOffset = DESTINATION_BASE_SAVEGAME_OFFSET_TR2 + (slotIndex * DESTINATION_SAVEGAME_SIZE);

                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];
                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR2.ContainsKey(levelIndex))
                    {
                        numOverwrites++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            return numOverwrites;
        }

        public void WriteSavegamesToDestination(List<Savegame> savegames, ListBox lstDestinationSavegames, ToolStripStatusLabel slblStatus, ComboBox cmbConversion)
        {
            isWriting = true;

            // Reset conversion flags
            PS4_TO_PC = false;
            PC_TO_PS4 = false;
            SWITCH_TO_PC = false;
            NO_CONVERT = false;
            PC_TO_SWITCH = false;
            PS4_TO_SWITCH = false;
            SWITCH_TO_PS4 = false;
            PREPATCH_TO_PATCH5 = false;

            int conversionNumber = cmbConversion.SelectedIndex;

            if (conversionNumber == 0)
            {
                byte[] sourceFileData = File.ReadAllBytes(savegameSourcePath);
                bool isSourcePatch5 = IsPatch5Savegame(sourceFileData);

                byte[] destinationFileData = File.ReadAllBytes(savegameDestinationPath);
                bool isDestinationPatch5 = IsPatch5Savegame(destinationFileData);

                if (!isSourcePatch5 && isDestinationPatch5)
                {
                    PREPATCH_TO_PATCH5 = true;
                }
                else
                {
                    NO_CONVERT = true;
                }
            }
            else if (conversionNumber == 1)
            {
                PC_TO_PS4 = true;
            }
            else if (conversionNumber == 2)
            {
                PC_TO_SWITCH = true;
            }
            else if (conversionNumber == 3)
            {
                PS4_TO_PC = true;
            }
            else if (conversionNumber == 4)
            {
                PS4_TO_SWITCH = true;
            }
            else if (conversionNumber == 5)
            {
                SWITCH_TO_PC = true;
            }
            else if (conversionNumber == 6)
            {
                SWITCH_TO_PS4 = true;
            }

            totalSavegames = savegames.Count;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += WriteSavegamesBackground;

            bgWorker.RunWorkerCompleted += (sender, e) => bgWorker_RunWorkerCompleted(sender, e, lstDestinationSavegames, slblStatus);

            bgWorker.ProgressChanged += UpdateProgressBar;

            slblStatus.Text = $"{(NO_CONVERT ? "Extracting" : "Converting")} savegame(s)...";

            bgWorker.RunWorkerAsync(savegames);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, ListBox lstDestinationSavegames, ToolStripStatusLabel slblStatus)
        {
            progressForm.Close();
            isWriting = false;

            if (e.Error != null || (e.Result != null && e.Result is Exception))
            {
                Exception exception = e.Error as Exception ?? e.Result as Exception;
                string errorMessage = e.Error != null ? e.Error.Message : exception.Message;

                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                slblStatus.Text = $"Error {(NO_CONVERT ? "transferring" : "converting")} savegame(s)";
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                slblStatus.Text = $"{(NO_CONVERT ? "Transfer" : "Conversion")} canceled";
            }
            else
            {
                MessageBox.Show($"Successfully {(NO_CONVERT ? "transferred " : "converted and transferred ")}" +
                    $"{totalSavegames} savegame(s) to destination file.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                slblStatus.Text = $"Successfully {(NO_CONVERT ? "transferred " : "converted and transferred ")}" +
                    $"{totalSavegames} savegame(s) to destination file";

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

                        int slotIndex = (savegames[i].Offset - SOURCE_BASE_SAVEGAME_OFFSET_TR2) / SOURCE_SAVEGAME_SIZE;
                        int currentSavegameOffset = DESTINATION_BASE_SAVEGAME_OFFSET_TR2 + (slotIndex * DESTINATION_SAVEGAME_SIZE);
                        byte[] savegameBytes = savegames[i].SavegameBytes;

                        if (NO_CONVERT)
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
                        else if (PREPATCH_TO_PATCH5)
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

                                if (currentRelativeOffset >= 0x6A0)
                                {
                                    destinationFile.Seek(offset + 0x1A, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                                else
                                {
                                    destinationFile.Seek(offset, SeekOrigin.Begin);
                                    destinationFile.Write(currentByte, 0, currentByte.Length);
                                }
                            }
                        }
                        else if (PC_TO_PS4)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PS4...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x690)
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
                        else if (PC_TO_SWITCH)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to Nintendo Switch...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x690)
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
                        else if (PS4_TO_PC)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PC...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x690)
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
                        else if (PS4_TO_SWITCH)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to Nintendo Switch...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }
                        else if (SWITCH_TO_PC)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PC...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + DESTINATION_SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte value = j < savegameBytes.Length ? savegameBytes[j] : (byte)0;
                                byte[] currentByte = { value };

                                if (currentRelativeOffset >= 0x690)
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
                        else if (SWITCH_TO_PS4)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PS4...");

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

        private bool IsPatch5Savegame(byte[] fileData)
        {
            return fileData[SAVEGAME_VERSION_OFFSET] >= PATCH5_SIGNATURE;
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
    }
}
