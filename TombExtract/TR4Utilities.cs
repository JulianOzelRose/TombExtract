using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    class TR4Utilities
    {
        // Paths
        private string savegameSourcePath;
        private string savegameDestinationPath;

        // Offsets
        private const int SLOT_STATUS_OFFSET = 0x004;
        private const int GAME_MODE_OFFSET = 0x01C;
        private const int SAVE_NUMBER_OFFSET = 0x008;
        private const int LEVEL_INDEX_OFFSET = 0x26F;

        // Savegame constants
        private const int BASE_SAVEGAME_OFFSET_TR4 = 0x2000;
        private const int SAVEGAME_SIZE = 0xA470;
        private const int MAX_SAVEGAMES = 32;

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
            return ReadByte(path, savegameOffset + SLOT_STATUS_OFFSET) != 0;
        }

        private GameMode GetGameMode(string path, int savegameOffset)
        {
            int gameMode = ReadByte(path, savegameOffset + GAME_MODE_OFFSET);
            return gameMode == 0 ? GameMode.Normal : GameMode.Plus;
        }

        private Int32 GetSaveNumber(string path, int savegameOffset)
        {
            return ReadInt32(path, savegameOffset + SAVE_NUMBER_OFFSET);
        }

        private byte GetLevelIndex(string path, int savegameOffset)
        {
            return ReadByte(path, savegameOffset + LEVEL_INDEX_OFFSET);
        }

        private readonly Dictionary<byte, string> levelNames = new Dictionary<byte, string>()
        {
            {  1, "Angkor Wat"                      },
            {  2, "Race for the Iris"               },
            {  3, "The Tomb of Seth"                },
            {  4, "Burial Chambers"                 },
            {  5, "Valley of the Kings"             },
            {  6, "KV5"                             },
            {  7, "Temple of Karnak"                },
            {  8, "The Great Hypostyle Hall"        },
            {  9, "Sacred Lake"                     },
            { 11, "Tomb of Semerkhet"               },
            { 12, "Guardian of Semerkhet"           },
            { 13, "Desert Railroad"                 },
            { 14, "Alexandria"                      },
            { 15, "Coastal Ruins"                   },
            { 16, "Pharos, Temple of Isis"          },
            { 17, "Cleopatra's Palaces"             },
            { 18, "Catacombs"                       },
            { 19, "Temple of Poseidon"              },
            { 20, "The Lost Library"                },
            { 21, "Hall of Demetrius"               },
            { 22, "City of the Dead"                },
            { 23, "Trenches"                        },
            { 24, "Chambers of Tulun"               },
            { 25, "Street Bazaar"                   },
            { 26, "Citadel Gate"                    },
            { 27, "Citadel"                         },
            { 28, "The Sphinx Complex"              },
            { 30, "Underneath the Sphinx"           },
            { 31, "Menkaure's Pyramid"              },
            { 32, "Inside Menkaure's Pyramid"       },
            { 33, "The Mastabas"                    },
            { 34, "The Great Pyramid"               },
            { 35, "Khufu's Queens Pyramids"         },
            { 36, "Inside the Great Pyramid"        },
            { 37, "Temple of Horus"                 },
            { 38, "Temple of Horus"                 },
            { 39, "The Times Office"                },
            { 40, "The Times Exclusive"             },
        };

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            try
            {
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR4 + (i * SAVEGAME_SIZE);

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
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR4 + (i * SAVEGAME_SIZE);

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

        private void WriteSavegamesBackground(object sender, DoWorkEventArgs e)
        {
            List<Savegame> savegames = e.Argument as List<Savegame>;

            try
            {
                using (FileStream saveFile = new FileStream(savegameSourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    int savegamesCopied = 0;

                    for (int i = 0; i < savegames.Count; i++)
                    {
                        progressForm.UpdateStatusMessage($"Copying '{savegames[i]}'...");

                        int currentSavegameOffset = savegames[i].Offset;
                        byte[] savegameBytes = new byte[SAVEGAME_SIZE];

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                        {
                            saveFile.Seek(offset, SeekOrigin.Begin);
                            byte currentByte = (byte)saveFile.ReadByte();
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

                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + SAVEGAME_SIZE; offset++, j++)
                        {
                            byte[] currentByte = { savegameBytes[j] };

                            destinationFile.Seek(offset, SeekOrigin.Begin);
                            destinationFile.Write(currentByte, 0, currentByte.Length);
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

        public void WriteSavegamesToDestination(List<Savegame> savegames, ListBox lstDestinationSavegamesTR4, ToolStripStatusLabel slblStatus)
        {
            isWriting = true;


            totalSavegames = savegames.Count;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += WriteSavegamesBackground;

            bgWorker.RunWorkerCompleted += (sender, e) => bgWorker_RunWorkerCompleted(sender, e, lstDestinationSavegamesTR4, slblStatus);

            bgWorker.ProgressChanged += UpdateProgressBar;

            slblStatus.Text = $"Extracting savegame(s)...";

            bgWorker.RunWorkerAsync(savegames);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, ListBox lstDestinationSavegamesTR4, ToolStripStatusLabel slblStatus)
        {
            progressForm.Close();
            isWriting = false;

            if (e.Error != null || (e.Result != null && e.Result is Exception))
            {
                Exception exception = e.Error as Exception ?? e.Result as Exception;
                string errorMessage = e.Error != null ? e.Error.Message : exception.Message;

                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                slblStatus.Text = $"Error transferring savegame(s).";
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                slblStatus.Text = $"Conversion canceled.";
            }
            else
            {
                MessageBox.Show($"Successfully transferred {totalSavegames} savegame(s) to destination file.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                slblStatus.Text = $"Successfully transferred {totalSavegames} savegame(s) to destination file.";
            }

            PopulateDestinationSavegames(lstDestinationSavegamesTR4);
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