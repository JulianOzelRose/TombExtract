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
        private const int TAB_TR4 = 3;
        private const int TAB_TR5 = 4;
        private const int TAB_TR6 = 5;
        private int CURRENT_TAB;

        // Path
        private string savegamePath;

        // Offsets
        private const int SLOT_STATUS_OFFSET = 0x004;
        private int GAME_MODE_OFFSET;
        private int LEVEL_INDEX_OFFSET;
        private int SAVE_NUMBER_OFFSET;

        // Savegame constants
        private const int BASE_SAVEGAME_OFFSET_TR1 = 0x2000;
        private const int BASE_SAVEGAME_OFFSET_TR2 = 0x72000;
        private const int BASE_SAVEGAME_OFFSET_TR3 = 0xE2000;
        private const int BASE_SAVEGAME_OFFSET_TR4 = 0x2000;
        private const int BASE_SAVEGAME_OFFSET_TR5 = 0x14AE00;
        private const int BASE_SAVEGAME_OFFSET_TR6 = 0x293C00;
        private const int SAVEGAME_SIZE_TRX = 0x3800;
        private const int SAVEGAME_SIZE_TRX2 = 0xA470;
        private const int MAX_SAVEGAMES = 32;
        private const int SLOT_NUMBER_OFFSET_TR6 = 0x15;

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
            else if (CURRENT_TAB == TAB_TR4)
            {
                gameSuffix = "Tomb Raider IV";
            }
            else if (CURRENT_TAB == TAB_TR5)
            {
                gameSuffix = "Tomb Raider V";
            }
            else if (CURRENT_TAB == TAB_TR6)
            {
                gameSuffix = "Tomb Raider VI";
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

        private bool IsTRXSavegame()
        {
            return (CURRENT_TAB == TAB_TR1 || CURRENT_TAB == TAB_TR2 || CURRENT_TAB == TAB_TR3);
        }

        private void DetermineOffsets()
        {
            if (CURRENT_TAB == TAB_TR1)
            {
                LEVEL_INDEX_OFFSET = 0x62C;
                SAVE_NUMBER_OFFSET = 0x00C;
                GAME_MODE_OFFSET = 0x008;
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                LEVEL_INDEX_OFFSET = 0x628;
                SAVE_NUMBER_OFFSET = 0x00C;
                GAME_MODE_OFFSET = 0x008;
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                LEVEL_INDEX_OFFSET = 0x8D6;
                SAVE_NUMBER_OFFSET = 0x00C;
                GAME_MODE_OFFSET = 0x008;
            }
            else if (CURRENT_TAB == TAB_TR4)
            {
                LEVEL_INDEX_OFFSET = 0x26F;
                SAVE_NUMBER_OFFSET = 0x008;
                GAME_MODE_OFFSET = 0x01C;
            }
            else if (CURRENT_TAB == TAB_TR5)
            {
                LEVEL_INDEX_OFFSET = 0x26F;
                SAVE_NUMBER_OFFSET = 0x008;
                GAME_MODE_OFFSET = 0x01C;
            }
            else if (CURRENT_TAB == TAB_TR6)
            {
                LEVEL_INDEX_OFFSET = 0x14;
                SAVE_NUMBER_OFFSET = 0x11C;
                GAME_MODE_OFFSET = 0x35C;
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
            else if (CURRENT_TAB == TAB_TR4)
            {
                PopulateSavegamesTR4();
            }
            else if (CURRENT_TAB == TAB_TR5)
            {
                PopulateSavegamesTR5();
            }
            else if (CURRENT_TAB == TAB_TR6)
            {
                PopulateSavegamesTR6();
            }

            btnCancel.Enabled = false;
            btnReorder.Enabled = false;
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

        private void DisableButtons()
        {
            btnMoveUp.Enabled = false;
            btnMoveDown.Enabled = false;
            btnDelete.Enabled = false;
            btnReorder.Enabled = false;
            btnClose.Enabled = false;
            btnNew.Enabled = false;
        }

        private void EnableButtons()
        {
            btnClose.Enabled = true;
        }

        private void PopulateSavegamesTR1()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_SIZE_TRX);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR1) / SAVEGAME_SIZE_TRX;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR1[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);

                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);

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
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_SIZE_TRX);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR2) / SAVEGAME_SIZE_TRX;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR2.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR2[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);

                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);

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
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_SIZE_TRX);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR3) / SAVEGAME_SIZE_TRX;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR3.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR3[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);

                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);

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

        private void PopulateSavegamesTR4()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR4 + (i * SAVEGAME_SIZE_TRX2);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR4) / SAVEGAME_SIZE_TRX2;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR4.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR4[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);

                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);

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

        private void PopulateSavegamesTR5()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR5 + (i * SAVEGAME_SIZE_TRX2);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR5) / SAVEGAME_SIZE_TRX2;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR5.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR5[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);

                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);

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

        private void PopulateSavegamesTR6()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR6 + (i * SAVEGAME_SIZE_TRX2);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR6) / SAVEGAME_SIZE_TRX2;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    byte slotStatus = fileData[currentSavegameOffset + SLOT_STATUS_OFFSET];

                    bool savegamePresent = slotStatus != 0;

                    if (savegamePresent && LevelNames.TR6.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        GameMode gameMode = fileData[currentSavegameOffset + GAME_MODE_OFFSET] == 0 ? GameMode.Normal : GameMode.Plus;

                        string levelName = LevelNames.TR6[levelIndex];
                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode, true);

                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);

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

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (lstSavegames.SelectedItem == null)
            {
                return;
            }

            if (CURRENT_TAB == TAB_TR6)
            {
                string warningMessage = $"This feature is under construction for Tomb Raider VI.";
                MessageBox.Show(warningMessage, "Feature Under Construction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lstSavegames.SelectedItem.ToString() != "Empty Slot")
            {
                DialogResult result = MessageBox.Show($"Are you sure you wish to overwrite '{(Savegame)lstSavegames.SelectedItem}'?",
                    "Create Savegame", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            CreateSavegameForm createSavegameForm = new CreateSavegameForm(CURRENT_TAB, savegamePath, (lstSavegames.SelectedItem as Savegame).Slot, (lstSavegames.SelectedItem as Savegame).Offset, slblStatus);
            createSavegameForm.ShowDialog();

            PopulateSavegamesConditionaly();
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

                btnReorder.Enabled = true;
                btnCancel.Enabled = true;
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

                btnReorder.Enabled = true;
                btnCancel.Enabled = true;
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

            int SAVEGAME_SIZE = IsTRXSavegame() ? SAVEGAME_SIZE_TRX : SAVEGAME_SIZE_TRX2;

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

                    using (FileStream savegameStream = new FileStream(savegamePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                    {
                        for (int offset = selectedSavegame.Offset; offset < (selectedSavegame.Offset + SAVEGAME_SIZE); offset++)
                        {
                            savegameStream.Seek(offset, SeekOrigin.Begin);
                            savegameStream.WriteByte(0);
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

                    slblStatus.Text = $"Successfully deleted savegame: '{deletedSavegameString}'.";

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

                    using (FileStream savegameStream = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            if (currentSavegame.Slot != i)
                            {
                                progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_SIZE_TRX);

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

                                savegameStream.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                savegameStream.Write(currentByte, 0, currentByte.Length);
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

                    using (FileStream savegameStream = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                            if (currentSavegame.Slot != i)
                            {
                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_SIZE_TRX);

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

                                savegameStream.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                savegameStream.Write(currentByte, 0, currentByte.Length);
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

                    using (FileStream savegameStream = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                            if (currentSavegame.Slot != i)
                            {
                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_SIZE_TRX);

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

                                savegameStream.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                savegameStream.Write(currentByte, 0, currentByte.Length);
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

        private void ReorderSavegamesTR4(List<Savegame> savegamesToMove)
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

                    using (FileStream savegameStream = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                            if (currentSavegame.Slot != i)
                            {
                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX2];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR4 + (i * SAVEGAME_SIZE_TRX2);

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

                                savegameStream.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                savegameStream.Write(currentByte, 0, currentByte.Length);
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

                PopulateSavegamesTR4();
                EnableButtons();
            };

            bgWorker.RunWorkerAsync();
        }

        private void ReorderSavegamesTR5(List<Savegame> savegamesToMove)
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

                    using (FileStream savegameStream = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                            if (currentSavegame.Slot != i)
                            {
                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX2];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR5 + (i * SAVEGAME_SIZE_TRX2);

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

                                savegameStream.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                savegameStream.Write(currentByte, 0, currentByte.Length);
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

                PopulateSavegamesTR5();
                EnableButtons();
            };

            bgWorker.RunWorkerAsync();
        }

        private void ReorderSavegamesTR6(List<Savegame> savegamesToMove)
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

                    using (FileStream savegameStream = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame currentSavegame = (Savegame)lstSavegames.Items[i];

                            progressForm.UpdateStatusMessage($"Copying '{currentSavegame}'...");

                            if (currentSavegame.Slot != i)
                            {
                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX2];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR6 + (i * SAVEGAME_SIZE_TRX2);

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

                                savegameStream.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                savegameStream.Write(currentByte, 0, currentByte.Length);
                            }

                            savegameStream.Seek(savegame.Offset + SLOT_NUMBER_OFFSET_TR6, SeekOrigin.Begin);
                            savegameStream.WriteByte((byte)savegame.Slot);

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

                PopulateSavegamesTR6();
                EnableButtons();
            };

            bgWorker.RunWorkerAsync();
        }

        private void btnReorder_Click(object sender, EventArgs e)
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
            else if (CURRENT_TAB == TAB_TR4)
            {
                ReorderSavegamesTR4(savegamesToMove);
            }
            else if (CURRENT_TAB == TAB_TR5)
            {
                ReorderSavegamesTR5(savegamesToMove);
            }
            else if (CURRENT_TAB == TAB_TR6)
            {
                ReorderSavegamesTR6(savegamesToMove);
            }

            btnCancel.Enabled = false;
            btnReorder.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PopulateSavegamesConditionaly();
        }

        private void lstSavegames_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = lstSavegames.SelectedIndex;

            if (!isWriting)
            {
                btnMoveUp.Enabled = (selectedIndex != -1 && selectedIndex >= 1);
                btnMoveDown.Enabled = (selectedIndex != -1 && selectedIndex < 31);
                btnDelete.Enabled = (selectedIndex != -1 && lstSavegames.SelectedItem.ToString() != "Empty Slot");
                btnNew.Enabled = (selectedIndex != -1);
            }
        }
    }
}
