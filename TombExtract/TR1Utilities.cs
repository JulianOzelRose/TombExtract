﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    class TR1Utilities
    {
        private string savegameSourcePath;
        private string savegameDestinationPath;

        private const int saveNumberOffset = 0x00C;
        private const int levelIndexOffset = 0x62C;

        private const int BASE_SAVEGAME_OFFSET_TR1 = 0x2000;
        private const int SAVEGAME_ITERATOR = 0x3800;

        private bool PS4_TO_PC = false;
        private bool PC_TO_PS4 = false;
        private bool SWITCH_TO_PC = false;
        private bool NO_CONVERT = false;
        private bool PC_TO_SWITCH = false;
        private bool PS4_TO_SWITCH = false;
        private bool SWITCH_TO_PS4 = false;

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

        private Int32 GetSaveNumber(string path, int savegameOffset)
        {
            return ReadInt32(path, savegameOffset + saveNumberOffset);
        }

        private byte GetLevelIndex(string path, int savegameOffset, int levelIndexOffset)
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
            { 16,  "Return to Egypt"        },
            { 17,  "Temple of the Cat"      },
            { 18,  "Atlantean Stronghold"   },
            { 19,  "The Hive"               },
        };

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            for (int i = 0; i < 32; i++)
            {
                int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_ITERATOR);

                byte levelIndex = GetLevelIndex(savegameSourcePath, currentSavegameOffset, levelIndexOffset);

                if (levelIndex >= 1 && levelIndex <= 22)
                {
                    Int32 saveNumber = GetSaveNumber(savegameSourcePath, currentSavegameOffset);
                    string levelName = levelNames[levelIndex];

                    Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName);
                    cklSavegames.Items.Add(savegame);
                }
            }
        }

        public void PopulateDestinationSavegames(ListBox lstSavegames)
        {
            lstSavegames.Items.Clear();

            for (int i = 0; i < 32; i++)
            {
                int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_ITERATOR);

                byte levelIndex = GetLevelIndex(savegameDestinationPath, currentSavegameOffset, levelIndexOffset);

                if (levelIndex >= 1 && levelIndex <= 22)
                {
                    Int32 saveNumber = GetSaveNumber(savegameDestinationPath, currentSavegameOffset);
                    string levelName = levelNames[levelIndex];

                    Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName);
                    lstSavegames.Items.Add(savegame);
                }
                else
                {
                    lstSavegames.Items.Add("Empty Slot");
                }
            }
        }

        public int GetNumOverwrites(List<Savegame> savegames)
        {
            int numOverwrites = 0;

            for (int i = 0; i < savegames.Count; i++)
            {
                int currentSavegameOffset = savegames[i].Offset;

                if (ReadByte(savegameDestinationPath, currentSavegameOffset + saveNumberOffset) != 0)
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
            ToolStripMenuItem tsmiBrowseSourceFile, ToolStripMenuItem tsmiBrowseDestinationFile, ToolStripStatusLabel slblStatus,
            ToolStripMenuItem tsmiExtract, ComboBox cmbConversionTR1, ComboBox cmbConversionTR2, ComboBox cmbConversionTR3)
        {
            isWriting = true;

            int conversion = cmbConversionTR1.SelectedIndex;

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
                btnBrowseSourceFile, btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR1, tsmiBrowseSourceFile,
                tsmiBrowseDestinationFile, slblStatus, tsmiExtract, cmbConversionTR1,
                cmbConversionTR2, cmbConversionTR3);

            bgWorker.ProgressChanged += UpdateProgressBar;

            try
            {
                string operation = NO_CONVERT ? "Extracting" : "Converting";
                slblStatus.Text = $"{operation} savegames...";
                bgWorker.RunWorkerAsync(savegames);
            }
            catch (Exception ex)
            {
                string operation = NO_CONVERT ? "extracting" : "converting";
                slblStatus.Text = $"Error {operation} savegames.";
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, CheckedListBox cklSourceSavegamesTR1,
            CheckedListBox cklSourceSavegamesTR2, CheckedListBox cklSourceSavegamesTR3, Button btnExtractTR1, Button btnExtractTR2,
            Button btnExtractTR3, Button btnSelectAllTR1, Button btnSelectAllTR2, Button btnSelectAllTR3,
            Button btnBrowseSourceFile,
            Button btnBrowseDestinationFile, CheckBox chkBackupOnWrite, ListBox lstDestinationSavegamesTR1,
            ToolStripMenuItem tsmiBrowseSourceFile, ToolStripMenuItem tsmiBrowseDestinationFile, ToolStripStatusLabel slblStatus,
            ToolStripMenuItem tsmiExtract, ComboBox cmbConversionTR1, ComboBox cmbConversionTR2, ComboBox cmbConversionTR3)
        {
            progressForm.Close();

            if (e.Error != null)
            {
                slblStatus.Text = $"Error transferring savegames.";
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show($"Successfully {operation} {totalSavegames} Tomb Raider I {savegamesText} to destination file.",
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

            btnBrowseSourceFile.Enabled = true;
            btnBrowseDestinationFile.Enabled = true;

            tsmiBrowseSourceFile.Enabled = true;
            tsmiBrowseDestinationFile.Enabled = true;
            tsmiExtract.Enabled = true;

            chkBackupOnWrite.Enabled = true;

            PopulateDestinationSavegames(lstDestinationSavegamesTR1);
        }

        private void WriteSavegamesBackground(object sender, DoWorkEventArgs e)
        {
            List<Savegame> savegames = e.Argument as List<Savegame>;

            try
            {
                for (int i = 0; i < savegames.Count; i++)
                {
                    progressForm.UpdateStatusMessage($"Copying '{savegames[i].Name} - {savegames[i].Number}'...");

                    int currentSavegameOffset = savegames[i].Offset;

                    byte[] savegameBytes = new byte[SAVEGAME_ITERATOR];

                    for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                    {
                        byte currentByte = ReadByte(savegameSourcePath, offset);
                        savegameBytes[j] = currentByte;
                    }

                    if (PS4_TO_PC)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i].Name} - {savegames[i].Number}' to PC...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                            {
                                WriteByte(savegameDestinationPath, (offset + 1), savegameBytes[j]);
                            }
                            else if (currentRelativeOffset >= 0x6B0)
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
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i].Name} - {savegames[i].Number}' to to PS4...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                            {
                                WriteByte(savegameDestinationPath, (offset - 1), savegameBytes[j]);
                            }
                            else if (currentRelativeOffset >= 0x6B0)
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
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i].Name} - {savegames[i].Number}' to to PC...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                            {
                                WriteByte(savegameDestinationPath, (offset + 1), savegameBytes[j]);
                            }
                            else if (currentRelativeOffset >= 0x6B0)
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
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i].Name} - {savegames[i].Number}' to to Switch...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0x64E && currentRelativeOffset < 0x6B0)
                            {
                                WriteByte(savegameDestinationPath, (offset - 1), savegameBytes[j]);
                            }
                            else if (currentRelativeOffset >= 0x6B0)
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
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i].Name} - {savegames[i].Number}' to Switch...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                        }
                    }
                    else if (SWITCH_TO_PS4)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i].Name} - {savegames[i].Number}' to PS4...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                        }
                    }
                    else if (NO_CONVERT)
                    {
                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i].Name} - {savegames[i].Number}' to destination...");

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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
