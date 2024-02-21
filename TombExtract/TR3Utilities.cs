using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    class TR3Utilities
    {
        private string savegameSourcePath;
        private string savegameDestinationPath;

        private const int saveNumberOffset = 0x00C;
        private const int levelIndexOffset = 0x8D6;

        private const int BASE_SAVEGAME_OFFSET_TR3 = 0xE2000;
        private const int SAVEGAME_ITERATOR = 0x3800;

        private bool toPC = false;
        private bool toPS4 = false;
        private bool noConvert = false;

        private int totalSavegames = 0;

        private BackgroundWorker bgWorker;
        private ToolStripProgressBar prgProgress;

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

        private UInt16 ReadUInt16(string path, int offset)
        {
            byte lowerByte = ReadByte(path, offset);
            byte upperByte = ReadByte(path, offset + 1);

            return (UInt16)(lowerByte + (upperByte << 8));
        }

        private UInt16 GetSaveNumber(string path, int savegameOffset)
        {
            return ReadUInt16(path, savegameOffset + saveNumberOffset);
        }

        private byte GetLevelIndex(string path, int savegameOffset, int levelIndexOffset)
        {
            return ReadByte(path, savegameOffset + levelIndexOffset);
        }

        private readonly Dictionary<byte, string> levelNames = new Dictionary<byte, string>()
        {
            {  1, "Jungle"                      },
            {  2, "Temple Ruins"                },
            {  3, "The River Ganges"            },
            {  4, "Caves of Kaliya"             },
            {  5, "Coastal Village"             },
            {  6, "Crash Site"                  },
            {  7, "Madubu Gorge"                },
            {  8, "Temple Of Puna"              },
            {  9, "Thames Wharf"                },
            { 10, "Aldwych"                     },
            { 11, "Lud's Gate"                  },
            { 12, "City"                        },
            { 13, "Nevada Desert"               },
            { 14, "High Security Compound"      },
            { 15, "Area 51"                     },
            { 16, "Antarctica"                  },
            { 17, "RX-Tech Mines"               },
            { 18, "Lost City Of Tinnos"         },
            { 19, "Meteorite Cavern"            },
            { 20, "All Hallows"                 },
            { 21, "Highland Fling"              },
            { 22, "Willard's Lair"              },
            { 23, "Shakespeare Cliff"           },
            { 24, "Sleeping with the Fishes"    },
            { 25, "It's a Madhouse!"            },
            { 26, "Reunion"                     },
        };

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            int currentSavegameOffset;

            for (int i = 0; i < 32; i++)
            {
                currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_ITERATOR);

                UInt16 saveNumber = GetSaveNumber(savegameSourcePath, currentSavegameOffset);
                byte levelIndex = GetLevelIndex(savegameSourcePath, currentSavegameOffset, levelIndexOffset);

                if (saveNumber != 0 && levelIndex >= 1 && levelIndex <= 22)
                {
                    string levelName = levelNames[levelIndex];
                    Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName);
                    cklSavegames.Items.Add(savegame);
                }
            }
        }

        public void PopulateDestinationSavegames(ListBox lstSavegames)
        {
            lstSavegames.Items.Clear();

            int currentSavegameOffset;

            for (int i = 0; i < 32; i++)
            {
                currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_ITERATOR);

                UInt16 saveNumber = GetSaveNumber(savegameDestinationPath, currentSavegameOffset);
                byte levelIndex = GetLevelIndex(savegameDestinationPath, currentSavegameOffset, levelIndexOffset);

                if (saveNumber != 0 && levelIndex >= 1 && levelIndex <= 22)
                {
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

        public void WriteSavegamesToDestination(List<Savegame> savegames, ToolStripProgressBar prgProgress,
            RadioButton rdoNoConvert, RadioButton rdoToPC, RadioButton rdoToPS4, CheckedListBox cklSourceSavegamesTR1,
            CheckedListBox cklSourceSavegamesTR2, CheckedListBox cklSourceSavegamesTR3, Button btnExtractTR1,
            Button btnExtractTR2, Button btnExtractTR3, Button btnSelectAllTR1, Button btnSelectAllTR2, Button btnSelectAllTR3,
            GroupBox grpConvertTR1, GroupBox grpConvertTR2, GroupBox grpConvertTR3, Button btnBrowseSourceFile,
            Button btnBrowseDestinationFile, CheckBox chkBackupOnWrite, ListBox lstDestinationSavegamesTR3)
        {
            isWriting = true;

            noConvert = rdoNoConvert.Checked;
            toPC = rdoToPC.Checked;
            toPS4 = rdoToPS4.Checked;

            totalSavegames = savegames.Count;

            this.prgProgress = prgProgress;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += WriteSavegamesBackground;
            bgWorker.RunWorkerCompleted += (sender, e) => bgWorker_RunWorkerCompleted(sender, e, cklSourceSavegamesTR1, cklSourceSavegamesTR2,
                cklSourceSavegamesTR3, btnExtractTR1, btnExtractTR2, btnExtractTR3, btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3,
                grpConvertTR1, grpConvertTR2, grpConvertTR3, btnBrowseSourceFile, btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR3);
            bgWorker.ProgressChanged += UpdateProgress;

            try
            {
                bgWorker.RunWorkerAsync(savegames);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, CheckedListBox cklSourceSavegamesTR1,
            CheckedListBox cklSourceSavegamesTR2, CheckedListBox cklSourceSavegamesTR3, Button btnExtractTR1, Button btnExtractTR2,
            Button btnExtractTR3, Button btnSelectAllTR1, Button btnSelectAllTR2, Button btnSelectAllTR3, GroupBox grpConvertTR1,
            GroupBox grpConvertTR2, GroupBox grpConvertTR3, Button btnBrowseSourceFile, Button btnBrowseDestinationFile, CheckBox chkBackupOnWrite,
            ListBox lstDestinationSavegamesTR3)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string operation = "";

                if (noConvert) operation = "extracted";
                else if (toPC || toPS4) operation = "converted";

                string savegamesText = totalSavegames == 1 ? "savegame" : "savegames";

                MessageBox.Show($"Successfully {operation} {totalSavegames} Tomb Raider III {savegamesText} to destination file.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            isWriting = false;

            cklSourceSavegamesTR1.Enabled = true;
            cklSourceSavegamesTR2.Enabled = true;
            cklSourceSavegamesTR3.Enabled = true;

            grpConvertTR1.Enabled = true;
            grpConvertTR2.Enabled = true;
            grpConvertTR3.Enabled = true;

            btnSelectAllTR1.Enabled = true;
            btnSelectAllTR2.Enabled = true;
            btnSelectAllTR3.Enabled = true;

            btnExtractTR1.Enabled = true;
            btnExtractTR2.Enabled = true;
            btnExtractTR3.Enabled = true;

            btnBrowseSourceFile.Enabled = true;
            btnBrowseDestinationFile.Enabled = true;

            chkBackupOnWrite.Enabled = true;

            PopulateDestinationSavegames(lstDestinationSavegamesTR3);
        }


        private void WriteSavegamesBackground(object sender, DoWorkEventArgs e)
        {
            List<Savegame> savegames = e.Argument as List<Savegame>;

            try
            {
                for (int i = 0; i < savegames.Count; i++)
                {
                    int currentSavegameOffset = savegames[i].Offset;

                    byte[] savegameBytes = new byte[SAVEGAME_ITERATOR];

                    for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                    {
                        byte currentByte = ReadByte(savegameSourcePath, offset);
                        savegameBytes[j] = currentByte;
                    }

                    if (toPC)
                    {
                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0xAAA)
                            {
                                WriteByte(savegameDestinationPath, (offset + 2), savegameBytes[j]);
                            }
                            else
                            {
                                WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                            }
                        }
                    }
                    else if (toPS4)
                    {
                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_ITERATOR; offset++, j++)
                        {
                            int currentRelativeOffset = offset - currentSavegameOffset;

                            if (currentRelativeOffset >= 0xAAA)
                            {
                                WriteByte(savegameDestinationPath, (offset - 2), savegameBytes[j]);
                            }
                            else
                            {
                                WriteByte(savegameDestinationPath, offset, savegameBytes[j]);
                            }
                        }
                    }
                    else if (noConvert)
                    {
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

        private void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            prgProgress.Value = e.ProgressPercentage;
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
