﻿using System;
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
        private const int gameModeOffset = 0x008;
        private const int saveNumberOffset = 0x00C;
        private const int levelIndexOffset = 0x628;

        // Iterators
        private const int BASE_SAVEGAME_OFFSET_TR2 = 0x72000;
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

        private void WriteByte(string path, int offset, byte value)
        {
            using (FileStream saveFile = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                saveFile.Seek(offset, SeekOrigin.Begin);
                byte[] byteData = { value };
                saveFile.Write(byteData, 0, byteData.Length);
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
            {  1,  "The Great Wall"             },
            {  2,  "Venice"                     },
            {  3,  "Bartoli's Hideout"          },
            {  4,  "Opera House"                },
            {  5,  "Offshore Rig"               },
            {  6,  "Diving Area"                },
            {  7,  "40 Fathoms"                 },
            {  8,  "Wreck of the Maria Doria"   },
            {  9,  "Living Quarters"            },
            { 10,  "The Deck"                   },
            { 11,  "Tibetan Foothills"          },
            { 12,  "Barkhang Monastery"         },
            { 13,  "Catacombs of the Talion"    },
            { 14,  "Ice Palace"                 },
            { 15,  "Temple of Xian"             },
            { 16,  "Floating Islands"           },
            { 17,  "The Dragon's Lair"          },
            { 18,  "Home Sweet Home"            },
            { 19,  "The Cold War"               },
            { 20,  "Fool's Gold"                },
            { 21,  "Furnace of the Gods"        },
            { 22,  "Kingdom"                    },
            { 23,  "Nightmare in Vegas"         },
        };

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            try
            {
                for (int i = 0; i < 32; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_ITERATOR);

                    byte levelIndex = GetLevelIndex(savegameSourcePath, currentSavegameOffset);

                    if (levelIndex >= 1 && levelIndex <= 23)
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
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_ITERATOR);

                    byte levelIndex = GetLevelIndex(savegameDestinationPath, currentSavegameOffset);

                    if (levelIndex >= 1 && levelIndex <= 23)
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

                if (levelIndex >= 1 && levelIndex <= 23)
                {
                    numOverwrites++;
                }
            }

            return numOverwrites;
        }

        public void WriteSavegamesToDestination(List<Savegame> savegames, CheckedListBox cklSourceSavegamesTR1,
            CheckedListBox cklSourceSavegamesTR2, CheckedListBox cklSourceSavegamesTR3, Button btnExtractTR1,
            Button btnExtractTR2, Button btnExtractTR3, Button btnSelectAllTR1, Button btnSelectAllTR2, Button btnSelectAllTR3,
            Button btnBrowseSourceFile, Button btnBrowseDestinationFile, CheckBox chkBackupOnWrite, ListBox lstDestinationSavegamesTR2,
            ToolStripMenuItem tsmiBrowseSourceFile, ToolStripMenuItem tsmiBrowseDestinationFile, ToolStripStatusLabel slblStatus,
            ToolStripMenuItem tsmiExtract, ComboBox cmbConversionTR1, ComboBox cmbConversionTR2, ComboBox cmbConversionTR3,
            Button btnManageSlotsTR1, Button btnManageSlotsTR2, Button btnManageSlotsTR3, ToolStripMenuItem tsmiBackupDestinationFile)
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

            int conversion = cmbConversionTR2.SelectedIndex;

            if (conversion == 0)
            {
                NO_CONVERT = true;
            }
            else if (conversion == 1)
            {
                PC_TO_PS4 = true;
            }
            else if (conversion == 2)
            {
                PS4_TO_PC = true;
            }
            else if (conversion == 3)
            {
                SWITCH_TO_PC = true;
            }
            else if (conversion == 4)
            {
                PC_TO_SWITCH = true;
            }
            else if (conversion == 5)
            {
                PS4_TO_SWITCH = true;
            }
            else if (conversion == 6)
            {
                SWITCH_TO_PS4 = true;
            }

            totalSavegames = savegames.Count;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += WriteSavegamesBackground;

            bgWorker.RunWorkerCompleted += (sender, e) => bgWorker_RunWorkerCompleted(sender, e, cklSourceSavegamesTR1, cklSourceSavegamesTR2,
                cklSourceSavegamesTR3, btnExtractTR1, btnExtractTR2, btnExtractTR3, btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3,
                btnBrowseSourceFile, btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR2, tsmiBrowseSourceFile,
                tsmiBrowseDestinationFile, slblStatus, tsmiExtract, cmbConversionTR1, cmbConversionTR2, cmbConversionTR3, btnManageSlotsTR1,
                btnManageSlotsTR2, btnManageSlotsTR3, tsmiBackupDestinationFile);

            bgWorker.ProgressChanged += UpdateProgressBar;

            string operation = NO_CONVERT ? "Extracting" : "Converting";
            slblStatus.Text = $"{operation} savegames...";

            bgWorker.RunWorkerAsync(savegames);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, CheckedListBox cklSourceSavegamesTR1,
            CheckedListBox cklSourceSavegamesTR2, CheckedListBox cklSourceSavegamesTR3, Button btnExtractTR1, Button btnExtractTR2,
            Button btnExtractTR3, Button btnSelectAllTR1, Button btnSelectAllTR2, Button btnSelectAllTR3, Button btnBrowseSourceFile,
            Button btnBrowseDestinationFile, CheckBox chkBackupOnWrite, ListBox lstDestinationSavegamesTR2,
            ToolStripMenuItem tsmiBrowseSourceFile, ToolStripMenuItem tsmiBrowseDestinationFile, ToolStripStatusLabel slblStatus,
            ToolStripMenuItem tsmiExtract, ComboBox cmbConversionTR1, ComboBox cmbConversionTR2, ComboBox cmbConversionTR3,
            Button btnManageSlotsTR1, Button btnManageSlotsTR2, Button btnManageSlotsTR3, ToolStripMenuItem tsmiBackupDestinationFile)
        {
            progressForm.Close();

            if (e.Error != null || (e.Result != null && e.Result is Exception))
            {
                Exception exception = e.Error as Exception ?? e.Result as Exception;
                string errorMessage = e.Error != null ? e.Error.Message : exception.Message;

                string operation;
                if (NO_CONVERT) operation = "transferred";
                else operation = "converting";

                slblStatus.Text = $"Error {operation} savegames.";
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                slblStatus.Text = $"Transfer canceled.";
                MessageBox.Show("Operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string operation;
                if (NO_CONVERT) operation = "transferred";
                else operation = "converted and transferred";
                string savegamesText = totalSavegames == 1 ? "savegame" : "savegames";

                slblStatus.Text = $"Successfully {operation} {totalSavegames} {savegamesText} to destination file.";

                MessageBox.Show($"Successfully {operation} {totalSavegames} Tomb Raider II {savegamesText} to destination file.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            isWriting = false;

            cklSourceSavegamesTR1.Enabled = true;
            cklSourceSavegamesTR2.Enabled = true;
            cklSourceSavegamesTR3.Enabled = true;

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

            PopulateDestinationSavegames(lstDestinationSavegamesTR2);
        }


        private void WriteSavegamesBackground(object sender, DoWorkEventArgs e)
        {
            List<Savegame> savegames = e.Argument as List<Savegame>;

            try
            {
                File.SetAttributes(savegameDestinationPath, File.GetAttributes(savegameDestinationPath) & ~FileAttributes.ReadOnly);

                for (int i = 0; i < savegames.Count; i++)
                {
                    progressForm.UpdateStatusMessage($"Copying '{savegames[i]}'...");

                    int currentSavegameOffset = savegames[i].Offset;

                    byte[] savegameBytes = new byte[SAVEGAME_ITERATOR];

                    for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                    {
                        byte currentByte = ReadByte(savegameSourcePath, offset);
                        savegameBytes[j] = currentByte;
                    }

                    if (PS4_TO_PC)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PC...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x690)
                            {
                                WriteByte(savegameDestinationPath, (offset + 4), savegameBytes[j]);
                            }
                            else
                            {
                                WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                            }
                        }
                    }
                    else if (PC_TO_PS4)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to PS4...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x690)
                            {
                                WriteByte(savegameDestinationPath, (offset - 4), savegameBytes[j]);
                            }
                            else
                            {
                                WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                            }
                        }
                    }
                    else if (SWITCH_TO_PC)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to PC...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x690)
                            {
                                WriteByte(savegameDestinationPath, (offset + 4), savegameBytes[j]);
                            }
                            else
                            {
                                WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                            }
                        }
                    }
                    else if (PC_TO_SWITCH)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to to Nintendo Switch...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x690)
                            {
                                WriteByte(savegameDestinationPath, (offset - 4), savegameBytes[j]);
                            }
                            else
                            {
                                WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                            }
                        }
                    }
                    else if (PS4_TO_SWITCH)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to Nintendo Switch...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                        }
                    }
                    else if (SWITCH_TO_PS4)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to PS4...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                        }
                    }
                    else if (NO_CONVERT)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                        }
                    }

                    int progressPercentage = (i + 1) * 100 / totalSavegames;
                    bgWorker.ReportProgress(progressPercentage);
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
