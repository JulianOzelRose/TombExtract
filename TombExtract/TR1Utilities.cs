using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    class TR1Utilities
    {
        // Paths
        private string savegameSourcePath;
        private string savegameDestinationPath;

        // Offsets
        private const int SLOT_STATUS_OFFSET = 0x004;
        private const int GAME_MODE_OFFSET = 0x008;
        private const int SAVE_NUMBER_OFFSET = 0x00C;
        private const int LEVEL_INDEX_OFFSET = 0x62C;

        // Savegame constants
        private const int BASE_SAVEGAME_OFFSET_TR1 = 0x2000;
        private const int SAVEGAME_SIZE = 0x3800;
        private const int MAX_SAVEGAMES = 32;

        // Conversion
        private bool PS4_TO_PC = false;
        private bool PC_TO_PS4 = false;
        private bool SWITCH_TO_PC = false;
        private bool NO_CONVERT = false;
        private bool PC_TO_SWITCH = false;
        private bool PS4_TO_SWITCH = false;
        private bool SWITCH_TO_PS4 = false;

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

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_SIZE);

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR1[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
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

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_SIZE);

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR1[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
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
                    int currentSavegameOffset = savegames[i].Offset;

                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];
                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
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

            int conversionNumber = cmbConversion.SelectedIndex;

            if (conversionNumber == 0)
            {
                NO_CONVERT = true;
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

                slblStatus.Text = $"Error {(NO_CONVERT ? "transferring" : "converting")} savegame(s).";
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                slblStatus.Text = $"{(NO_CONVERT ? "Transfer" : "Conversion")} canceled.";
            }
            else
            {
                MessageBox.Show($"Successfully {(NO_CONVERT ? "transferred " : "converted and transferred ")}" +
                    $"{totalSavegames} savegame(s) to destination file.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                slblStatus.Text = $"Successfully {(NO_CONVERT ? "transferred " : "converted and transferred ")}" +
                    $"{totalSavegames} savegame(s) to destination file.";

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
                        byte[] savegameBytes = new byte[SAVEGAME_SIZE];

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
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

                        int currentSavegameOffset = savegames[i].Offset;
                        byte[] savegameBytes = savegames[i].SavegameBytes;

                        if (NO_CONVERT)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                            {
                                byte[] currentByte = { savegameBytes[j] };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }
                        else if (PC_TO_PS4)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to PS4...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte[] currentByte = { savegameBytes[j] };

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
                        else if (PC_TO_SWITCH)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to Nintendo Switch...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte[] currentByte = { savegameBytes[j] };

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
                        else if (PS4_TO_PC)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PC...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte[] currentByte = { savegameBytes[j] };

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
                        else if (PS4_TO_SWITCH)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to Nintendo Switch...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                            {
                                byte[] currentByte = { savegameBytes[j] };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }
                        else if (SWITCH_TO_PC)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PC...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                            {
                                int currentRelativeOffset = offset - currentSavegameOffset;
                                byte[] currentByte = { savegameBytes[j] };

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
                        else if (SWITCH_TO_PS4)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to PS4...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                            {
                                byte[] currentByte = { savegameBytes[j] };

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
