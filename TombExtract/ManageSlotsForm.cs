using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    public partial class ManageSlotsForm : Form
    {
        // Tabs
        private const int TAB_TR1 = 0;
        private const int TAB_TR2 = 1;
        private const int TAB_TR3 = 2;
        private int CURRENT_TAB;

        // Path
        private string savegamePath;

        // Offsets
        private const int slotStatusOffset = 0x004;
        private const int gameModeOffset = 0x008;
        private const int saveNumberOffset = 0x00C;
        private int levelIndexOffset;

        // Iterators
        private const int BASE_SAVEGAME_OFFSET_TR1 = 0x2000;
        private const int BASE_SAVEGAME_OFFSET_TR2 = 0x72000;
        private const int BASE_SAVEGAME_OFFSET_TR3 = 0xE2000;
        private const int SAVEGAME_ITERATOR = 0x3800;

        // Misc
        private ProgressForm progressForm;
        private ToolStripStatusLabel slblStatus;
        private bool isWriting = false;
        private bool backupOnWrite = false;

        public ManageSlotsForm(string path, int CURRENT_TAB, ToolStripStatusLabel slblStatus, bool backupOnWrite)
        {
            InitializeComponent();

            savegamePath = path;
            this.CURRENT_TAB = CURRENT_TAB;
            this.slblStatus = slblStatus;
            this.backupOnWrite = backupOnWrite;

            string gameSuffix = "";

            if (CURRENT_TAB == TAB_TR1)
            {
                gameSuffix = "Tomb Raider I";
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                gameSuffix = "Tomb Raider II";
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                gameSuffix = "Tomb Raider III";
            }

            this.Text = $"Manage Slots - {gameSuffix}";
        }

        private void ManageSlotsForm_Load(object sender, EventArgs e)
        {
            DetermineOffsets();
            PopulateSavegamesConditionaly();
        }

        private void ManageSlotsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isWriting)
            {
                DialogResult result = MessageBox.Show(
                    "Exiting in the middle of a write operation could result in a corrupted savegame file. Are you sure you wish to exit?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void DetermineOffsets()
        {
            if (CURRENT_TAB == TAB_TR1)
            {
                levelIndexOffset = 0x62C;
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                levelIndexOffset = 0x628;
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                levelIndexOffset = 0x8D6;
            }
        }

        private void PopulateSavegamesConditionaly()
        {
            if (CURRENT_TAB == TAB_TR1)
            {
                PopulateSavegamesTR1();
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                PopulateSavegamesTR2();
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                PopulateSavegamesTR3();
            }
        }

        private void CreateBackup()
        {
            if (!string.IsNullOrEmpty(savegamePath) && File.Exists(savegamePath))
            {
                string directory = Path.GetDirectoryName(savegamePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(savegamePath);
                string fileExtension = Path.GetExtension(savegamePath);

                string backupFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}{fileExtension}.bak");

                if (File.Exists(backupFilePath))
                {
                    File.SetAttributes(backupFilePath, File.GetAttributes(backupFilePath) & ~FileAttributes.ReadOnly);
                }

                File.Copy(savegamePath, backupFilePath, true);
            }
        }

        private byte ReadByte(int offset)
        {
            using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                saveFile.Seek(offset, SeekOrigin.Begin);
                return (byte)saveFile.ReadByte();
            }
        }

        private Int32 ReadInt32(int offset)
        {
            byte byte1 = ReadByte(offset);
            byte byte2 = ReadByte(offset + 1);
            byte byte3 = ReadByte(offset + 2);
            byte byte4 = ReadByte(offset + 3);

            return (Int32)(byte1 + (byte2 << 8) + (byte3 << 16) + (byte4 << 24));
        }

        private bool IsSavegamePresent(int savegameOffset)
        {
            return ReadByte(savegameOffset + slotStatusOffset) != 0;
        }

        private byte GetLevelIndex(int savegameOffset)
        {
            return ReadByte(savegameOffset + levelIndexOffset);
        }

        private Int32 GetSaveNumber(int savegameOffset)
        {
            return ReadInt32(savegameOffset + saveNumberOffset);
        }

        private GameMode GetGameMode(int savegameOffset)
        {
            int gameMode = ReadByte(savegameOffset + gameModeOffset);
            return gameMode == 0 ? GameMode.Normal : GameMode.Plus;
        }

        private void DisableButtons()
        {
            btnMoveUp.Enabled = false;
            btnMoveDown.Enabled = false;
            btnDelete.Enabled = false;
            btnApply.Enabled = false;
            btnClose.Enabled = false;
            btnRefresh.Enabled = false;
        }

        private void EnableButtons()
        {
            btnApply.Enabled = true;
            btnClose.Enabled = true;
            btnRefresh.Enabled = true;
        }

        private void PopulateSavegamesTR1()
        {
            lstSavegames.Items.Clear();

            try
            {
                for (int i = 0; i < 32; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_ITERATOR);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR1[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR1) / SAVEGAME_ITERATOR;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR1) / SAVEGAME_ITERATOR;

                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateSavegamesTR2()
        {
            lstSavegames.Items.Clear();

            try
            {
                for (int i = 0; i < 32; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_ITERATOR);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR2.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR2[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR2) / SAVEGAME_ITERATOR;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR2) / SAVEGAME_ITERATOR;

                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateSavegamesTR3()
        {
            lstSavegames.Items.Clear();

            try
            {
                for (int i = 0; i < 32; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_ITERATOR);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR3.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR3[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR3) / SAVEGAME_ITERATOR;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR3) / SAVEGAME_ITERATOR;

                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateSavegamesConditionaly();

            btnMoveUp.Enabled = false;
            btnMoveDown.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lstSavegames.SelectedIndex > 0)
            {
                int selectedIndex = lstSavegames.SelectedIndex;
                Savegame selectedSavegame = (Savegame)lstSavegames.SelectedItem;

                lstSavegames.Items.RemoveAt(selectedIndex);
                lstSavegames.Items.Insert(selectedIndex - 1, selectedSavegame);
                lstSavegames.SelectedIndex = selectedIndex - 1;
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lstSavegames.SelectedIndex < lstSavegames.Items.Count - 1 && lstSavegames.SelectedIndex != -1)
            {
                int selectedIndex = lstSavegames.SelectedIndex;
                Savegame selectedSavegame = (Savegame)lstSavegames.SelectedItem;

                lstSavegames.Items.RemoveAt(selectedIndex);
                lstSavegames.Items.Insert(selectedIndex + 1, selectedSavegame);
                lstSavegames.SelectedIndex = selectedIndex + 1;
            }
        }

        private void UpdateProgressBar(object sender, ProgressChangedEventArgs e)
        {
            progressForm.UpdateProgressBar(e.ProgressPercentage);
            progressForm.UpdatePercentage(e.ProgressPercentage);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstSavegames.SelectedItem == null)
            {
                MessageBox.Show("Invalid selection!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lstSavegames.SelectedItem != null && lstSavegames.SelectedItem.ToString() == "Empty Slot")
            {
                MessageBox.Show("You cannot delete an empty slot!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure you wish to delete '{(Savegame)lstSavegames.SelectedItem}'?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.No)
            {
                return;
            }

            slblStatus.Text = $"Deleting savegame...";

            BackgroundWorker bgWorker = new BackgroundWorker();

            bgWorker.DoWork += (s, args) =>
            {
                isWriting = true;

                if (backupOnWrite)
                {
                    CreateBackup();
                }

                try
                {
                    Savegame selectedSavegame = (Savegame)args.Argument;
                    string deletedSavegameString = selectedSavegame.ToString();

                    File.SetAttributes(savegamePath, File.GetAttributes(savegamePath) & ~FileAttributes.ReadOnly);

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                    {
                        for (int offset = selectedSavegame.Offset; offset < (selectedSavegame.Offset + SAVEGAME_ITERATOR); offset++)
                        {
                            saveFile.Seek(offset, SeekOrigin.Begin);
                            saveFile.WriteByte(0);
                        }
                    }

                    args.Result = deletedSavegameString;
                }
                catch (Exception ex)
                {
                    args.Result = ex;
                }
            };

            bgWorker.RunWorkerCompleted += (s, args) =>
            {
                EnableButtons();
                isWriting = false;

                if (args.Error != null || args.Result is Exception)
                {
                    MessageBox.Show($"Error occurred while deleting savegame.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    slblStatus.Text = $"Error occurred while deleting savegame.";
                }
                else if (args.Cancelled)
                {
                    MessageBox.Show($"Operation cancelled.",
                        "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Savegame deletion cancelled.";
                }
                else
                {
                    string deletedSavegameString = (string)args.Result;

                    MessageBox.Show($"Successfully deleted '{deletedSavegameString}'.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Successfully deleted savegame.";

                    PopulateSavegamesConditionaly();
                }
            };

            DisableButtons();
            bgWorker.RunWorkerAsync(lstSavegames.SelectedItem);
        }

        private void ReorderSavegamesTR1(List<Savegame> savegamesToMove)
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.ProgressChanged += UpdateProgressBar;

            if (backupOnWrite)
            {
                CreateBackup();
            }

            slblStatus.Text = $"Reordering savegames...";

            bgWorker.DoWork += (sender, e) =>
            {
                try
                {
                    File.SetAttributes(savegamePath, File.GetAttributes(savegamePath) & ~FileAttributes.ReadOnly);

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            if (currentSavegame.Slot != i)
                            {
                                progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                                byte[] savegameBytes = new byte[SAVEGAME_ITERATOR];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_ITERATOR);

                                savegamesToMove.Add(currentSavegame);
                            }
                        }

                        for (int i = 0; i < savegamesToMove.Count; i++)
                        {
                            Savegame savegame = savegamesToMove[i];

                            progressForm.UpdateStatusMessage($"Moving '{savegame}' to Slot {savegame.Slot + 1}...");

                            for (int offset = 0; offset < savegame.SavegameBytes.Length; offset++)
                            {
                                byte[] currentByte = { savegame.SavegameBytes[offset] };

                                saveFile.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                saveFile.Write(currentByte, 0, currentByte.Length);
                            }

                            int progressPercentage = 50 + (i * 50) / savegamesToMove.Count;
                            bgWorker.ReportProgress(progressPercentage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            };

            bgWorker.ProgressChanged += (sender, e) =>
            {
                progressForm.UpdateProgressBar(e.ProgressPercentage);
            };

            bgWorker.RunWorkerCompleted += (sender, e) =>
            {
                progressForm.Close();
                isWriting = false;

                if (e.Error != null || e.Result is Exception)
                {
                    MessageBox.Show($"Error occurred while reordering savegames.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    slblStatus.Text = $"Error occurred while reordering savegames.";
                }
                else if (e.Cancelled)
                {
                    MessageBox.Show($"Operation was cancelled.",
                        "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Savegame reordering cancelled.";
                }
                else
                {
                    MessageBox.Show($"Successfully reordered {savegamesToMove.Count} savegames.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Successfully reordered savegames.";
                }

                PopulateSavegamesTR1();
                EnableButtons();
            };

            bgWorker.RunWorkerAsync();
        }

        private void ReorderSavegamesTR2(List<Savegame> savegamesToMove)
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.ProgressChanged += UpdateProgressBar;

            if (backupOnWrite)
            {
                CreateBackup();
            }

            slblStatus.Text = $"Reordering savegames...";

            bgWorker.DoWork += (sender, e) =>
            {
                try
                {
                    File.SetAttributes(savegamePath, File.GetAttributes(savegamePath) & ~FileAttributes.ReadOnly);

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                            if (currentSavegame.Slot != i)
                            {
                                byte[] savegameBytes = new byte[SAVEGAME_ITERATOR];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_ITERATOR);

                                savegamesToMove.Add(currentSavegame);
                            }

                            int progressPercentage = (i * 50) / lstSavegames.Items.Count;
                            bgWorker.ReportProgress(progressPercentage);
                        }

                        for (int i = 0; i < savegamesToMove.Count; i++)
                        {
                            Savegame savegame = savegamesToMove[i];

                            progressForm.UpdateStatusMessage($"Moving '{savegame}' to Slot {savegame.Slot + 1}...");

                            for (int offset = 0; offset < savegame.SavegameBytes.Length; offset++)
                            {
                                byte[] currentByte = { savegame.SavegameBytes[offset] };

                                saveFile.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                saveFile.Write(currentByte, 0, currentByte.Length);
                            }

                            int progressPercentage = 50 + (i * 50) / savegamesToMove.Count;
                            bgWorker.ReportProgress(progressPercentage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            };

            bgWorker.ProgressChanged += (sender, e) =>
            {
                progressForm.UpdateProgressBar(e.ProgressPercentage);
            };

            bgWorker.RunWorkerCompleted += (sender, e) =>
            {
                progressForm.Close();
                isWriting = false;

                if (e.Error != null || e.Result is Exception)
                {
                    MessageBox.Show($"Error occurred while reordering savegames.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    slblStatus.Text = $"Error occurred while reordering savegames.";
                }
                else if (e.Cancelled)
                {
                    MessageBox.Show($"Operation was cancelled.",
                        "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Savegame reordering cancelled.";
                }
                else
                {
                    MessageBox.Show($"Successfully reordered {savegamesToMove.Count} savegames.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Successfully reordered savegames.";
                }

                PopulateSavegamesTR2();
                EnableButtons();
            };

            bgWorker.RunWorkerAsync();
        }

        private void ReorderSavegamesTR3(List<Savegame> savegamesToMove)
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.ProgressChanged += UpdateProgressBar;

            if (backupOnWrite)
            {
                CreateBackup();
            }

            slblStatus.Text = $"Reordering savegames...";

            bgWorker.DoWork += (sender, e) =>
            {
                try
                {
                    File.SetAttributes(savegamePath, File.GetAttributes(savegamePath) & ~FileAttributes.ReadOnly);

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                            if (currentSavegame.Slot != i)
                            {
                                byte[] savegameBytes = new byte[SAVEGAME_ITERATOR];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_ITERATOR);

                                savegamesToMove.Add(currentSavegame);
                            }

                            int progressPercentage = (i * 50) / lstSavegames.Items.Count;
                            bgWorker.ReportProgress(progressPercentage);
                        }

                        for (int i = 0; i < savegamesToMove.Count; i++)
                        {
                            Savegame savegame = savegamesToMove[i];

                            progressForm.UpdateStatusMessage($"Moving '{savegame}' to Slot {savegame.Slot + 1}...");

                            for (int offset = 0; offset < savegame.SavegameBytes.Length; offset++)
                            {
                                byte[] currentByte = { savegame.SavegameBytes[offset] };

                                saveFile.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                saveFile.Write(currentByte, 0, currentByte.Length);
                            }

                            int progressPercentage = 50 + (i * 50) / savegamesToMove.Count;
                            bgWorker.ReportProgress(progressPercentage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            };

            bgWorker.ProgressChanged += (sender, e) =>
            {
                progressForm.UpdateProgressBar(e.ProgressPercentage);
            };

            bgWorker.RunWorkerCompleted += (sender, e) =>
            {
                progressForm.Close();
                isWriting = false;

                if (e.Error != null || e.Result is Exception)
                {
                    MessageBox.Show($"Error occurred while reordering savegames.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    slblStatus.Text = $"Error occurred while reordering savegames.";
                }
                else if (e.Cancelled)
                {
                    MessageBox.Show($"Operation was cancelled.",
                        "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Savegame reordering cancelled.";
                }
                else
                {
                    MessageBox.Show($"Successfully reordered {savegamesToMove.Count} savegames.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    slblStatus.Text = $"Successfully reordered savegames.";
                }

                PopulateSavegamesTR3();
                EnableButtons();
            };

            bgWorker.RunWorkerAsync();
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            DisableButtons();

            List<Savegame> savegamesToMove = new List<Savegame>();

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.Show();

            isWriting = true;

            if (CURRENT_TAB == TAB_TR1)
            {
                ReorderSavegamesTR1(savegamesToMove);
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                ReorderSavegamesTR2(savegamesToMove);
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                ReorderSavegamesTR3(savegamesToMove);
            }
        }

        private void lstSavegames_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = lstSavegames.SelectedIndex;

            if (!isWriting)
            {
                btnMoveUp.Enabled = (selectedIndex != -1 && selectedIndex >= 1);
                btnMoveDown.Enabled = (selectedIndex != -1 && selectedIndex < 31);
                btnDelete.Enabled = (selectedIndex != -1 && lstSavegames.SelectedItem.ToString() != "Empty Slot");
            }
        }

        private readonly Dictionary<byte, string> levelNamesTR1 = new Dictionary<byte, string>()
        {
            { 1,  "Caves"                       },
            { 2,  "City of Vilcabamba"          },
            { 3,  "Lost Valley"                 },
            { 4,  "Tomb of Qualopec"            },
            { 5,  "St. Francis' Folly"          },
            { 6,  "Colosseum"                   },
            { 7,  "Palace Midas"                },
            { 8,  "The Cistern"                 },
            { 9,  "Tomb of Tihocan"             },
            { 10, "City of Khamoon"             },
            { 11, "Obelisk of Khamoon"          },
            { 12, "Sanctuary of the Scion"      },
            { 13, "Natla's Mines"               },
            { 14, "Atlantis"                    },
            { 15, "The Great Pyramid"           },
            { 16, "Return to Egypt"             },
            { 17, "Temple of the Cat"           },
            { 18, "Atlantean Stronghold"        },
            { 19, "The Hive"                    },
        };

        private readonly Dictionary<byte, string> levelNamesTR2 = new Dictionary<byte, string>()
        {
            {  1, "The Great Wall"              },
            {  2, "Venice"                      },
            {  3, "Bartoli's Hideout"           },
            {  4, "Opera House"                 },
            {  5, "Offshore Rig"                },
            {  6, "Diving Area"                 },
            {  7, "40 Fathoms"                  },
            {  8, "Wreck of the Maria Doria"    },
            {  9, "Living Quarters"             },
            { 10, "The Deck"                    },
            { 11, "Tibetan Foothills"           },
            { 12, "Barkhang Monastery"          },
            { 13, "Catacombs of the Talion"     },
            { 14, "Ice Palace"                  },
            { 15, "Temple of Xian"              },
            { 16, "Floating Islands"            },
            { 17, "The Dragon's Lair"           },
            { 18, "Home Sweet Home"             },
            { 19, "The Cold War"                },
            { 20, "Fool's Gold"                 },
            { 21, "Furnace of the Gods"         },
            { 22, "Kingdom"                     },
            { 23, "Nightmare in Vegas"          },
        };

        private readonly Dictionary<byte, string> levelNamesTR3 = new Dictionary<byte, string>()
        {
            {  1, "Jungle"                      },
            {  2, "Temple Ruins"                },
            {  3, "The River Ganges"            },
            {  4, "Caves of Kaliya"             },
            {  5, "Coastal Village"             },
            {  6, "Crash Site"                  },
            {  7, "Madubu Gorge"                },
            {  8, "Temple of Puna"              },
            {  9, "Thames Wharf"                },
            { 10, "Aldwych"                     },
            { 11, "Lud's Gate"                  },
            { 12, "City"                        },
            { 13, "Nevada Desert"               },
            { 14, "High Security Compound"      },
            { 15, "Area 51"                     },
            { 16, "Antarctica"                  },
            { 17, "RX-Tech Mines"               },
            { 18, "Lost City of Tinnos"         },
            { 19, "Meteorite Cavern"            },
            { 20, "All Hallows"                 },
            { 21, "Highland Fling"              },
            { 22, "Willard's Lair"              },
            { 23, "Shakespeare Cliff"           },
            { 24, "Sleeping with the Fishes"    },
            { 25, "It's a Madhouse!"            },
            { 26, "Reunion"                     },
        };
    }
}
