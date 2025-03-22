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
        private const int TR6_DISPLAY_NAME_OFFSET = 0x124;
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
        private const string EMPTY_SLOT_STRING_TR6 = "< Empty Slot >";

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

        private void WriteString(string path, int offset, string value, int maxLength = 256)
        {
            using (FileStream saveFile = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                saveFile.Seek(offset, SeekOrigin.Begin);

                byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(value);

                if (stringBytes.Length > maxLength)
                {
                    Array.Resize(ref stringBytes, maxLength);
                }

                saveFile.Write(stringBytes, 0, stringBytes.Length);

                if (stringBytes.Length < maxLength)
                {
                    saveFile.WriteByte(0);
                }
            }
        }

        private bool IsSavegamePresent(int savegameOffset)
        {
            return ReadByte(savegameOffset + SLOT_STATUS_OFFSET) != 0;
        }

        private byte GetLevelIndex(int savegameOffset)
        {
            return ReadByte(savegameOffset + LEVEL_INDEX_OFFSET);
        }

        private Int32 GetSaveNumber(int savegameOffset)
        {
            return ReadInt32(savegameOffset + SAVE_NUMBER_OFFSET);
        }

        private GameMode GetGameMode(int savegameOffset)
        {
            int gameMode = ReadByte(savegameOffset + GAME_MODE_OFFSET);
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
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_SIZE_TRX);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR1[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR1) / SAVEGAME_SIZE_TRX;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR1) / SAVEGAME_SIZE_TRX;

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
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_SIZE_TRX);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR2.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR2[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR2) / SAVEGAME_SIZE_TRX;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR2) / SAVEGAME_SIZE_TRX;

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
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_SIZE_TRX);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR3.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR3[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR3) / SAVEGAME_SIZE_TRX;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR3) / SAVEGAME_SIZE_TRX;

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
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR4 + (i * SAVEGAME_SIZE_TRX2);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR4.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR4[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR4) / SAVEGAME_SIZE_TRX2;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR4) / SAVEGAME_SIZE_TRX2;

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
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR5 + (i * SAVEGAME_SIZE_TRX2);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR5.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR5[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR5) / SAVEGAME_SIZE_TRX2;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR5) / SAVEGAME_SIZE_TRX2;

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
                for (int i = 0; i < MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR6 + (i * SAVEGAME_SIZE_TRX2);

                    byte levelIndex = GetLevelIndex(currentSavegameOffset);
                    bool savegamePresent = IsSavegamePresent(currentSavegameOffset);

                    if (savegamePresent && levelNamesTR6.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = GetSaveNumber(currentSavegameOffset);
                        string levelName = levelNamesTR6[levelIndex];
                        GameMode gameMode = GetGameMode(currentSavegameOffset);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR6) / SAVEGAME_SIZE_TRX2;

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, gameMode, true);
                        savegame.Slot = slot;

                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, GameMode.None);
                        int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR6) / SAVEGAME_SIZE_TRX2;

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

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                    {
                        for (int offset = selectedSavegame.Offset; offset < (selectedSavegame.Offset + SAVEGAME_SIZE); offset++)
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

                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

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
                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

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
                                byte[] savegameBytes = new byte[SAVEGAME_SIZE_TRX];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

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

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

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

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

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

                    using (FileStream saveFile = new FileStream(savegamePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                                    saveFile.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)saveFile.ReadByte();

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

                                saveFile.Seek(savegame.Offset + offset, SeekOrigin.Begin);
                                saveFile.Write(currentByte, 0, currentByte.Length);
                            }

                            saveFile.Seek(savegame.Offset + SLOT_NUMBER_OFFSET_TR6, SeekOrigin.Begin);
                            saveFile.WriteByte((byte)savegame.Slot);

                            int progressPercentage = 50 + (i * 50) / savegamesToMove.Count;
                            bgWorker.ReportProgress(progressPercentage);
                        }

                        // Ensure that any empty slots have the empty slot name written
                        for (int i = 0; i < lstSavegames.Items.Count; i++)
                        {
                            Savegame savegame = (Savegame)lstSavegames.Items[i];

                            if (savegame.IsEmptySlot)
                            {
                                WriteString(savegamePath, savegame.Offset + TR6_DISPLAY_NAME_OFFSET, EMPTY_SLOT_STRING_TR6);
                            }
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

        private readonly Dictionary<byte, string> levelNamesTR4 = new Dictionary<byte, string>()
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

        private readonly Dictionary<byte, string> levelNamesTR5 = new Dictionary<byte, string>()
        {
            {  1, "Streets of Rome"                      },
            {  2, "Trajan's Markets"                     },
            {  3, "The Colosseum"                        },
            {  4, "The Base"                             },
            {  5, "The Submarine"                        },
            {  6, "Deepsea Dive"                         },
            {  7, "Sinking Submarine"                    },
            {  8, "Gallows Tree"                         },
            {  9, "Labyrinth"                            },
            { 10, "Old Mill"                             },
            { 11, "The 13th Floor"                       },
            { 12, "Escape with the Iris"                 },
            { 14, "Red Alert!"                           },
        };

        private readonly Dictionary<byte, string> levelNamesTR6 = new Dictionary<byte, string>()
        {
            {  0, "Parisian Back Streets"       },
            {  1, "Derelict Apartment Block"    },
            {  2, "Margot Carvier's Apartment"  },
            {  3, "Industrial Roof Tops"        },
            {  4, "Parisian Ghetto"             },
            {  5, "Parisian Ghetto"             },
            {  6, "Parisian Ghetto"             },
            {  7, "The Serpent Rouge"           },
            {  8, "Rennes' Pawnshop"            },
            {  9, "Willowtree Herbalist"        },
            { 10, "St. Aicard's Church"         },
            { 11, "Café Metro"                  },
            { 12, "St. Aicard's Graveyard"      },
            { 13, "Bouchard's Hideout"          },
            { 14, "Louvre Storm Drains"         },
            { 15, "Louvre Galleries"            },
            { 16, "Galleries Under Siege"       },
            { 17, "Tomb of Ancients"            },
            { 18, "The Archaeological Dig"      },
            { 19, "Von Croy's Apartment"        },
            { 20, "The Monstrum Crimescene"     },
            { 21, "The Strahov Fortress"        },
            { 22, "The Bio-Research Facility"   },
            { 23, "Aquatic Research Area"       },
            { 24, "The Sanitarium"              },
            { 25, "Maximum Containment Area"    },
            { 26, "The Vault of Trophies"       },
            { 27, "Boaz Returns"                },
            { 28, "Eckhardt's Lab"              },
            { 29, "The Lost Domain"             },
            { 30, "The Hall of Seasons"         },
            { 31, "Neptune's Hall"              },
            { 32, "Wrath of the Beast"          },
            { 33, "The Sanctuary of Flame"      },
            { 34, "The Breath of Hades"         },
        };
    }
}
