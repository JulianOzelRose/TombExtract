using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    class TR6Utilities
    {
        // Paths
        private string savegameSourcePath;
        private string savegameDestinationPath;

        // Offsets
        private const int SLOT_STATUS_OFFSET = 0x004;
        private const int DISPLAY_NAME_OFFSET = 0x124;

        // Savegame constants
        private const int BASE_SAVEGAME_OFFSET_TR6 = 0x293C00;
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

        private bool IsSavegamePresent(string path, int savegameOffset)
        {
            return ReadByte(path, savegameOffset + SLOT_STATUS_OFFSET) != 0;
        }

        //private GameMode GetGameMode(string path, int savegameOffset)
        //{
        //    int gameMode = ReadByte(path, savegameOffset + GAME_MODE_OFFSET);
        //    return gameMode == 0 ? GameMode.Normal : GameMode.Plus;
        //}

        private string ReadString(string path, int offset, int maxLength = 256)
        {
            using (FileStream saveFile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                saveFile.Seek(offset, SeekOrigin.Begin);

                List<byte> stringBytes = new List<byte>();

                for (int i = 0; i < maxLength; i++)
                {
                    int readByte = saveFile.ReadByte();

                    if (readByte == -1 || readByte == 0)
                        break;

                    stringBytes.Add((byte)readByte);
                }

                return System.Text.Encoding.ASCII.GetString(stringBytes.ToArray());
            }
        }

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            try
            {
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR6 + (i * SAVEGAME_SIZE);

                    bool savegamePresent = IsSavegamePresent(savegameSourcePath, currentSavegameOffset);

                    if (savegamePresent)
                    {
                        //GameMode gameMode = GetGameMode(savegameSourcePath, currentSavegameOffset);
                        string savegameDisplayString = ReadString(savegameSourcePath, currentSavegameOffset + DISPLAY_NAME_OFFSET);
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, savegameDisplayString, GameMode.Normal, true);
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
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR6 + (i * SAVEGAME_SIZE);

                    bool savegamePresent = IsSavegamePresent(savegameDestinationPath, currentSavegameOffset);

                    if (savegamePresent)
                    {
                        //GameMode gameMode = GetGameMode(savegameDestinationPath, currentSavegameOffset);
                        string savegameDisplayString = ReadString(savegameDestinationPath, currentSavegameOffset + DISPLAY_NAME_OFFSET);
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, savegameDisplayString, GameMode.Normal, true);
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

                bool savegamePresent = IsSavegamePresent(savegameDestinationPath, currentSavegameOffset);

                if (savegamePresent)
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

        public void WriteSavegamesToDestination(List<Savegame> savegames, ListBox lstDestinationSavegamesTR6, ToolStripStatusLabel slblStatus)
        {
            isWriting = true;


            totalSavegames = savegames.Count;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += WriteSavegamesBackground;

            bgWorker.RunWorkerCompleted += (sender, e) => bgWorker_RunWorkerCompleted(sender, e, lstDestinationSavegamesTR6, slblStatus);

            bgWorker.ProgressChanged += UpdateProgressBar;

            slblStatus.Text = $"Extracting savegame(s)...";

            bgWorker.RunWorkerAsync(savegames);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, ListBox lstDestinationSavegamesTR6, ToolStripStatusLabel slblStatus)
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

            PopulateDestinationSavegames(lstDestinationSavegamesTR6);
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