using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using static TombExtract.MainForm;

namespace TombExtract
{
    class TR6Utilities
    {
        // Paths
        private string savegameSourcePath;
        private string savegameDestinationPath;

        // Offsets
        private const int NEW_GAME_PLUS_OFFSET = 0x35C;
        private const int SAVE_NUMBER_OFFSET = 0x11C;
        private const int LEVEL_INDEX_OFFSET = 0x14;
        private const int BASE_SAVEGAME_OFFSET_TR6 = 0x293C00;

        // Misc
        private int totalSavegames = 0;
        private BackgroundWorker bgWorker;
        private ProgressForm progressForm;
        private bool isWriting = false;
        private readonly IWin32Window owner;

        public TR6Utilities(IWin32Window owner)
        {
            this.owner = owner;
        }

        public void PopulateSourceSavegames(CheckedListBox cklSavegames)
        {
            cklSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegameSourcePath);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR6 + (i * Globals.SAVEGAME_SIZE_TRX2);

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR6.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        string levelName = LevelNames.TR6[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, true);
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
                byte[] fileData = File.ReadAllBytes(savegameDestinationPath);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR6 + (i * Globals.SAVEGAME_SIZE_TRX2);

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR6.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        string levelName = LevelNames.TR6[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, true);
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
                    int currentSavegameOffset = savegames[i].Offset;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR6.ContainsKey(levelIndex))
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
                        byte[] savegameBytes = new byte[Globals.SAVEGAME_SIZE_TRX2];

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + Globals.SAVEGAME_SIZE_TRX2; offset++, j++)
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

                        progressForm.UpdateStatusMessage($"Transferring '{savegames[i]}' to destination...");

                        for (int offset = currentSavegameOffset, j = 0; offset < currentSavegameOffset + Globals.SAVEGAME_SIZE_TRX2; offset++, j++)
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

            slblStatus.Text = Globals.STATUS_MSG_TRANSFER_IN_PROGRESS;

            bgWorker.RunWorkerAsync(savegames);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, ListBox lstDestinationSavegamesTR6, ToolStripStatusLabel slblStatus)
        {
            progressForm.Close();
            isWriting = false;

            if (e.Error != null || (e.Result != null && e.Result is Exception))
            {
                slblStatus.Text = Globals.STATUS_MSG_TRANSFER_ERROR;

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
                slblStatus.Text = Globals.STATUS_MSG_TRANSFER_CANCELED;

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
                slblStatus.Text = $"Successfully transferred {totalSavegames} savegame(s) to destination file";

                System.Media.SystemSounds.Asterisk.Play();

                string dialogMessage = $"Successfully transferred {totalSavegames} savegame(s) to destination file.";

                ThemedMessageBox.Show(
                    owner,
                    dialogMessage,
                    Globals.DIALOG_TITLE_SUCCESS,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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