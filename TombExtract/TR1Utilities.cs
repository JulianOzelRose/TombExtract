﻿using System;
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
        private const int slotStatusOffset = 0x004;
        private const int gameModeOffset = 0x008;
        private const int saveNumberOffset = 0x00C;
        private const int levelIndexOffset = 0x62C;

        // Iterators
        private const int BASE_SAVEGAME_OFFSET_TR1 = 0x2000;
        private const int SAVEGAME_ITERATOR = 0x3800;

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

        private byte ReadByte(string path, int offset)
        {
            using (FileStream saveFile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                saveFile.Seek(offset, SeekOrigin.Begin);
                return (byte)saveFile.ReadByte();
            }
        }

        private Int32 ReadInt32(string path, int offset)
        {
            byte byte1 = ReadByte(path, offset);
            byte byte2 = ReadByte(path, offset + 1);
            byte byte3 = ReadByte(path, offset + 2);
            byte byte4 = ReadByte(path, offset + 3);

            return (Int32)(byte1 + (byte2 << 8) + (byte3 << 16) + (byte4 << 24));
        }

        private bool IsSavegamePresent(string path, int savegameOffset)
        {
            return ReadByte(path, savegameOffset + slotStatusOffset) != 0;
        }

        private GameMode GetGameMode(string path, int savegameOffset)
        {
            int gameMode = ReadByte(path, savegameOffset + gameModeOffset);
            return gameMode == 0 ? GameMode.Normal : GameMode.Plus;
        }

        private Int32 GetSaveNumber(string path, int savegameOffset)
        {
            return ReadInt32(path, savegameOffset + saveNumberOffset);
        }

        private byte GetLevelIndex(string path, int savegameOffset)
        {
            return ReadByte(path, savegameOffset + levelIndexOffset);
        }

        private readonly Dictionary<byte, string> levelNames = new Dictionary<byte, string>()
        {
            { 1,  "Caves"                   },
            { 2,  "City of Vilcabamba"      },
            { 3,  "Lost Valley"             },
            { 4,  "Tomb of Qualopec"        },
            { 5,  "St. Francis' Folly"      },
            { 6,  "Colosseum"               },
            { 7,  "Palace Midas"            },
            { 8,  "The Cistern"             },
            { 9,  "Tomb of Tihocan"         },
            { 10, "City of Khamoon"         },
            { 11, "Obelisk of Khamoon"      },
            { 12, "Sanctuary of the Scion"  },
            { 13, "Natla's Mines"           },
            { 14, "Atlantis"                },
            { 15, "The Great Pyramid"       },
            { 16, "Return to Egypt"         },
            { 17, "Temple of the Cat"       },
            { 18, "Atlantean Stronghold"    },
            { 19, "The Hive"                },
        };

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            try
            {
                for (int i = 0; i < 32; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_ITERATOR);

                    byte levelIndex = GetLevelIndex(savegameSourcePath, currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(savegameSourcePath, currentSavegameOffset);

                    if (savegamePresent && levelNames.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(savegameSourcePath, currentSavegameOffset);
                        string levelName = levelNames[levelIndex];
                        GameMode gameMode = GetGameMode(savegameSourcePath, currentSavegameOffset);

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
                for (int i = 0; i < 32; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_ITERATOR);

                    byte levelIndex = GetLevelIndex(savegameDestinationPath, currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(savegameDestinationPath, currentSavegameOffset);

                    if (savegamePresent && levelNames.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(savegameDestinationPath, currentSavegameOffset);
                        string levelName = levelNames[levelIndex];
                        GameMode gameMode = GetGameMode(savegameDestinationPath, currentSavegameOffset);

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

            for (int i = 0; i < savegames.Count; i++)
            {
                int currentSavegameOffset = savegames[i].Offset;

                byte levelIndex = GetLevelIndex(savegameDestinationPath, currentSavegameOffset);
                bool savegamePresent = IsSavegamePresent(savegameDestinationPath, currentSavegameOffset);

                if (savegamePresent && levelNames.ContainsKey(levelIndex))
                {
                    numOverwrites++;
                }
            }

            return numOverwrites;
        }

        public void WriteSavegamesToDestination(List<Savegame> savegames, CheckedListBox cklSourceSavegamesTR1,
            CheckedListBox cklSourceSavegamesTR2, CheckedListBox cklSourceSavegamesTR3, Button btnExtractTR1,
            Button btnExtractTR2, Button btnExtractTR3, Button btnSelectAllTR1, Button btnSelectAllTR2, Button btnSelectAllTR3,
            Button btnBrowseSourceFile, Button btnBrowseDestinationFile, CheckBox chkBackupOnWrite, ListBox lstDestinationSavegamesTR1,
            ListBox lstDestinationSavegamesTR2, ListBox lstDestinationSavegamesTR3, ToolStripMenuItem tsmiBrowseSourceFile,
            ToolStripMenuItem tsmiBrowseDestinationFile, ToolStripStatusLabel slblStatus, ToolStripMenuItem tsmiExtract,
            ComboBox cmbConversionTR1, ComboBox cmbConversionTR2, ComboBox cmbConversionTR3, Button btnManageSlotsTR1,
            Button btnManageSlotsTR2, Button btnManageSlotsTR3, ToolStripMenuItem tsmiBackupDestinationFile)
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

            int conversionNumber = cmbConversionTR1.SelectedIndex;

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

            bgWorker.RunWorkerCompleted += (sender, e) => bgWorker_RunWorkerCompleted(sender, e, cklSourceSavegamesTR1, cklSourceSavegamesTR2,
                cklSourceSavegamesTR3, btnExtractTR1, btnExtractTR2, btnExtractTR3, btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3,
                btnBrowseSourceFile, btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR1, lstDestinationSavegamesTR2,
                lstDestinationSavegamesTR3, tsmiBrowseSourceFile, tsmiBrowseDestinationFile, slblStatus, tsmiExtract,
                cmbConversionTR1, cmbConversionTR2, cmbConversionTR3, btnManageSlotsTR1, btnManageSlotsTR2, btnManageSlotsTR3,
                tsmiBackupDestinationFile);

            bgWorker.ProgressChanged += UpdateProgressBar;

            slblStatus.Text = $"{(NO_CONVERT ? "Extracting" : "Converting")} savegame(s)...";

            bgWorker.RunWorkerAsync(savegames);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, CheckedListBox cklSourceSavegamesTR1,
            CheckedListBox cklSourceSavegamesTR2, CheckedListBox cklSourceSavegamesTR3, Button btnExtractTR1, Button btnExtractTR2,
            Button btnExtractTR3, Button btnSelectAllTR1, Button btnSelectAllTR2, Button btnSelectAllTR3,
            Button btnBrowseSourceFile, Button btnBrowseDestinationFile, CheckBox chkBackupOnWrite, ListBox lstDestinationSavegamesTR1,
            ListBox lstDestinationSavegamesTR2, ListBox lstDestinationSavegamesTR3, ToolStripMenuItem tsmiBrowseSourceFile,
            ToolStripMenuItem tsmiBrowseDestinationFile, ToolStripStatusLabel slblStatus, ToolStripMenuItem tsmiExtract,
            ComboBox cmbConversionTR1, ComboBox cmbConversionTR2, ComboBox cmbConversionTR3, Button btnManageSlotsTR1,
            Button btnManageSlotsTR2, Button btnManageSlotsTR3, ToolStripMenuItem tsmiBackupDestinationFile)
        {
            progressForm.Close();

            if (e.Error != null || (e.Result != null && e.Result is Exception))
            {
                Exception exception = e.Error as Exception ?? e.Result as Exception;
                string errorMessage = e.Error != null ? e.Error.Message : exception.Message;

                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                slblStatus.Text = $"Error {(NO_CONVERT ? "transferred" : "converting")} savegame(s).";
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

            isWriting = false;

            cklSourceSavegamesTR1.Enabled = true;
            cklSourceSavegamesTR2.Enabled = true;
            cklSourceSavegamesTR3.Enabled = true;

            lstDestinationSavegamesTR1.Enabled = true;
            lstDestinationSavegamesTR2.Enabled = true;
            lstDestinationSavegamesTR3.Enabled = true;

            cmbConversionTR1.Enabled = true;
            cmbConversionTR2.Enabled = true;
            cmbConversionTR3.Enabled = true;

            btnSelectAllTR1.Enabled = true;
            btnSelectAllTR2.Enabled = true;
            btnSelectAllTR3.Enabled = true;

            btnExtractTR1.Enabled = true;
            btnExtractTR2.Enabled = true;
            btnExtractTR3.Enabled = true;

            btnManageSlotsTR1.Enabled = true;
            btnManageSlotsTR2.Enabled = true;
            btnManageSlotsTR3.Enabled = true;

            btnBrowseSourceFile.Enabled = true;
            btnBrowseDestinationFile.Enabled = true;

            tsmiBrowseSourceFile.Enabled = true;
            tsmiBrowseDestinationFile.Enabled = true;
            tsmiExtract.Enabled = true;
            tsmiBackupDestinationFile.Enabled = true;

            chkBackupOnWrite.Enabled = true;

            PopulateDestinationSavegames(lstDestinationSavegamesTR1);
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
                        byte[] savegameBytes = new byte[SAVEGAME_ITERATOR];

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
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

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                            {
                                byte[] currentByte = { savegameBytes[j] };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }
                        else if (PC_TO_PS4)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to PS4...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
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

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
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

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
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

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                            {
                                byte[] currentByte = { savegameBytes[j] };

                                destinationFile.Seek(offset, SeekOrigin.Begin);
                                destinationFile.Write(currentByte, 0, currentByte.Length);
                            }
                        }
                        else if (SWITCH_TO_PC)
                        {
                            progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PC...");

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
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

                            for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
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
