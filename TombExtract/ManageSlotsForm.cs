using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using static TombExtract.MainForm;

namespace TombExtract
{
    public partial class ManageSlotsForm : Form
    {
        // Tab
        private int CURRENT_TAB;

        // Path
        private string savegamePath;

        // Platform or patch-dependent offsets
        private int NEW_GAME_PLUS_OFFSET;
        private int LEVEL_INDEX_OFFSET;
        private int SAVE_NUMBER_OFFSET;
        private int CHALLENGE_MODE_OFFSET;

        // TR1 offsets (universal)
        private const int SAVE_NUMBER_OFFSET_TR1 = 0x00C;
        private const int NEW_GAME_PLUS_OFFSET_TR1 = 0x008;

        // TR1 offsets (PC)
        private const int LEVEL_INDEX_OFFSET_TR1_PC = 0x62C;
        private const int CHALLENGE_MODE_OFFSET_TR1_PC = 0x6EC;

        // TR1 offsets (Android)
        private const int LEVEL_INDEX_OFFSET_TR1_ANDROID = 0x65C;
        private const int CHALLENGE_MODE_OFFSET_TR1_ANDROID = 0x718;

        // TR1 offsets (PS4)
        private const int LEVEL_INDEX_OFFSET_TR1_PS4 = 0x62C;
        private const int CHALLENGE_MODE_OFFSET_TR1_PS4 = 0x6E8;

        // TR2 offsets (universal)
        private const int SAVE_NUMBER_OFFSET_TR2 = 0x00C;
        private const int NEW_GAME_PLUS_OFFSET_TR2 = 0x008;

        // TR2 offsets (PC)
        private const int LEVEL_INDEX_OFFSET_TR2_PC = 0x628;
        private const int CHALLENGE_MODE_OFFSET_TR2_PC = 0x6B0;

        // TR2 offsets (Android)
        private const int LEVEL_INDEX_OFFSET_TR2_ANDROID = 0x658;
        private const int CHALLENGE_MODE_OFFSET_TR2_ANDROID = 0x6DC;

        // TR2 offsets (PS4)
        private const int LEVEL_INDEX_OFFSET_TR2_PS4 = 0x628;
        private const int CHALLENGE_MODE_OFFSET_TR2_PS4 = 0x6AC;

        // TR3 offsets (universal)
        private const int SAVE_NUMBER_OFFSET_TR3 = 0x00C;
        private const int NEW_GAME_PLUS_OFFSET_TR3 = 0x008;

        // TR3 offsets (PC)
        private const int LEVEL_INDEX_OFFSET_TR3_PC = 0x8D6;
        private const int CHALLENGE_MODE_OFFSET_TR3_PC = 0x990;

        // TR3 offsets (Android)
        private const int LEVEL_INDEX_OFFSET_TR3_ANDROID = 0x916;
        private const int CHALLENGE_MODE_OFFSET_TR3_ANDROID = 0x9D0;

        // TR3 offsets (PS4)
        private const int LEVEL_INDEX_OFFSET_TR3_PS4 = 0x8D6;
        private const int CHALLENGE_MODE_OFFSET_TR3_PS4 = 0x990;

        // TR4 offsets
        private const int LEVEL_INDEX_OFFSET_TR4 = 0x26F;
        private const int SAVE_NUMBER_OFFSET_TR4 = 0x008;
        private const int NEW_GAME_PLUS_OFFSET_TR4 = 0x01C;

        // TR5 offsets
        private const int LEVEL_INDEX_OFFSET_TR5 = 0x26F;
        private const int SAVE_NUMBER_OFFSET_TR5 = 0x008;
        private const int NEW_GAME_PLUS_OFFSET_TR5 = 0x01C;

        // TR6 offsets
        private const int LEVEL_INDEX_OFFSET_TR6 = 0x14;
        private const int SAVE_NUMBER_OFFSET_TR6 = 0x11C;
        private const int NEW_GAME_PLUS_OFFSET_TR6 = 0x35C;
        private const int SLOT_NUMBER_OFFSET_TR6 = 0x015;

        // Savegame constants
        private int BASE_SAVEGAME_OFFSET_TR1;
        private int BASE_SAVEGAME_OFFSET_TR2;
        private int BASE_SAVEGAME_OFFSET_TR3;
        private const int BASE_SAVEGAME_OFFSET_TR4 = 0x2000;
        private const int BASE_SAVEGAME_OFFSET_TR5 = 0x14AE00;
        private const int BASE_SAVEGAME_OFFSET_TR6 = 0x293C00;
        private int SAVEGAME_SIZE_TRX;

        // Patch-specific
        private const int BASE_SAVEGAME_OFFSET_TR1_PREPATCH = 0x2000;
        private const int BASE_SAVEGAME_OFFSET_TR1_PATCH5 = 0x2000;
        private const int BASE_SAVEGAME_OFFSET_TR2_PREPATCH = 0x72000;
        private const int BASE_SAVEGAME_OFFSET_TR2_PATCH5 = 0xD2000;
        private const int BASE_SAVEGAME_OFFSET_TR3_PREPATCH = 0xE2000;
        private const int BASE_SAVEGAME_OFFSET_TR3_PATCH5 = 0x1A2000;

        // Misc
        private ProgressForm progressForm;
        private ToolStripStatusLabel slblStatus;
        private bool isWriting = false;
        private bool backupOnWrite = false;
        Platform platform;

        public ManageSlotsForm(string path, int CURRENT_TAB, ToolStripStatusLabel slblStatus, bool backupOnWrite, Platform platform)
        {
            InitializeComponent();

            savegamePath = path;
            this.CURRENT_TAB = CURRENT_TAB;
            this.slblStatus = slblStatus;
            this.backupOnWrite = backupOnWrite;

            string gameSuffix = "";

            if (CURRENT_TAB == Globals.TAB_TR1)
            {
                gameSuffix = "Tomb Raider I";
            }
            else if (CURRENT_TAB == Globals.TAB_TR2)
            {
                gameSuffix = "Tomb Raider II";
            }
            else if (CURRENT_TAB == Globals.TAB_TR3)
            {
                gameSuffix = "Tomb Raider III";
            }
            else if (CURRENT_TAB == Globals.TAB_TR4)
            {
                gameSuffix = "Tomb Raider IV";
            }
            else if (CURRENT_TAB == Globals.TAB_TR5)
            {
                gameSuffix = "Tomb Raider V";
            }
            else if (CURRENT_TAB == Globals.TAB_TR6)
            {
                gameSuffix = "Tomb Raider VI";
            }

            this.Text = $"{Globals.WINDOW_TITLE_MANAGE_SLOTS} - {gameSuffix}";
            this.platform = platform;
        }

        private void ManageSlotsForm_Load(object sender, EventArgs e)
        {
            if (ThemeUtilities.DARK_MODE_ENABLED)
            {
                ThemeUtilities.ApplyDarkMode(this);
                ThemeUtilities.ApplyDarkTitleBar(this);
            }

            DetermineOffsets();
            PopulateSavegamesConditionaly();
        }

        private void ManageSlotsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isWriting)
            {
                System.Media.SystemSounds.Exclamation.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    Globals.DIALOG_MSG_WRITE_IN_PROGRESS_EXIT_CONFIRM,
                    Globals.DIALOG_TITLE_CONFIRMATION,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private bool IsTRXSavegame()
        {
            return (CURRENT_TAB == Globals.TAB_TR1 || CURRENT_TAB == Globals.TAB_TR2 || CURRENT_TAB == Globals.TAB_TR3);
        }

        private void DetermineOffsets()
        {
            bool isPatch5 = false;

            if (IsTRXSavegame())
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);
                isPatch5 = IsPatch5SavegameFile(fileData);

                if (isPatch5)
                {
                    SAVEGAME_SIZE_TRX = Globals.SAVEGAME_SIZE_TRX_PATCH5;
                    BASE_SAVEGAME_OFFSET_TR1 = BASE_SAVEGAME_OFFSET_TR1_PATCH5;
                    BASE_SAVEGAME_OFFSET_TR2 = BASE_SAVEGAME_OFFSET_TR2_PATCH5;
                    BASE_SAVEGAME_OFFSET_TR3 = BASE_SAVEGAME_OFFSET_TR3_PATCH5;
                }
                else
                {
                    SAVEGAME_SIZE_TRX = Globals.SAVEGAME_SIZE_TRX_PREPATCH;
                    BASE_SAVEGAME_OFFSET_TR1 = BASE_SAVEGAME_OFFSET_TR1_PREPATCH;
                    BASE_SAVEGAME_OFFSET_TR2 = BASE_SAVEGAME_OFFSET_TR2_PREPATCH;
                    BASE_SAVEGAME_OFFSET_TR3 = BASE_SAVEGAME_OFFSET_TR3_PREPATCH;
                }
            }

            if (CURRENT_TAB == Globals.TAB_TR1)
            {
                if (isPatch5)
                {
                    SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR1;
                    NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR1;

                    if (platform == Platform.PC)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR1_PC;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR1_PC;
                    }
                    else if (platform == Platform.Android)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR1_ANDROID;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR1_ANDROID;
                    }
                    else if (platform == Platform.PlayStation4)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR1_PS4;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR1_PS4;
                    }
                }
                else
                {
                    LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR1_PC;
                    SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR1;
                    NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR1;
                }
            }
            else if (CURRENT_TAB == Globals.TAB_TR2)
            {
                if (isPatch5)
                {
                    SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR2;
                    NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR2;

                    if (platform == Platform.PC)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR2_PC;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR2_PC;
                    }
                    else if (platform == Platform.Android)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR2_ANDROID;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR2_ANDROID;
                    }
                    else if (platform == Platform.PlayStation4)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR2_PS4;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR2_PS4;
                    }
                }
                else
                {
                    LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR2_PC;
                    SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR2;
                    NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR2;
                }
            }
            else if (CURRENT_TAB == Globals.TAB_TR3)
            {
                if (isPatch5)
                {
                    SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR3;
                    NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR3;

                    if (platform == Platform.PC)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR3_PC;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR3_PC;
                    }
                    else if (platform == Platform.Android)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR3_ANDROID;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR3_ANDROID;
                    }
                    else if (platform == Platform.PlayStation4)
                    {
                        LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR3_PS4;
                        CHALLENGE_MODE_OFFSET = CHALLENGE_MODE_OFFSET_TR3_PS4;
                    }
                }
                else
                {
                    LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR3_PC;
                    SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR3;
                    NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR3;
                }
            }
            else if (CURRENT_TAB == Globals.TAB_TR4)
            {
                LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR4;
                SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR4;
                NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR4;
            }
            else if (CURRENT_TAB == Globals.TAB_TR5)
            {
                LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR5;
                SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR5;
                NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR5;
            }
            else if (CURRENT_TAB == Globals.TAB_TR6)
            {
                LEVEL_INDEX_OFFSET = LEVEL_INDEX_OFFSET_TR6;
                SAVE_NUMBER_OFFSET = SAVE_NUMBER_OFFSET_TR6;
                NEW_GAME_PLUS_OFFSET = NEW_GAME_PLUS_OFFSET_TR6;
            }
        }

        private void PopulateSavegamesConditionaly()
        {
            if (CURRENT_TAB == Globals.TAB_TR1)
            {
                PopulateSavegamesTR1();
            }
            else if (CURRENT_TAB == Globals.TAB_TR2)
            {
                PopulateSavegamesTR2();
            }
            else if (CURRENT_TAB == Globals.TAB_TR3)
            {
                PopulateSavegamesTR3();
            }
            else if (CURRENT_TAB == Globals.TAB_TR4)
            {
                PopulateSavegamesTR4();
            }
            else if (CURRENT_TAB == Globals.TAB_TR5)
            {
                PopulateSavegamesTR5();
            }
            else if (CURRENT_TAB == Globals.TAB_TR6)
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

                bool isPatch5 = IsPatch5SavegameFile(fileData);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR1 + (i * SAVEGAME_SIZE_TRX);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR1) / SAVEGAME_SIZE_TRX;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR1.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        bool isChallengeMode = fileData[currentSavegameOffset + CHALLENGE_MODE_OFFSET] == 1 && isPatch5;
                        string levelName = LevelNames.TR1[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, false, isChallengeMode);
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, false);
                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PopulateSavegamesTR2()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                bool isPatch5 = IsPatch5SavegameFile(fileData);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR2 + (i * SAVEGAME_SIZE_TRX);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR2) / SAVEGAME_SIZE_TRX;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR2.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        bool isChallengeMode = fileData[currentSavegameOffset + CHALLENGE_MODE_OFFSET] == 1 && isPatch5;
                        string levelName = LevelNames.TR2[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, false, isChallengeMode);
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, false);
                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PopulateSavegamesTR3()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                bool isPatch5 = IsPatch5SavegameFile(fileData);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR3 + (i * SAVEGAME_SIZE_TRX);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR3) / SAVEGAME_SIZE_TRX;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR3.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        bool isChallengeMode = fileData[currentSavegameOffset + CHALLENGE_MODE_OFFSET] == 1 && isPatch5;
                        string levelName = LevelNames.TR3[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, false, isChallengeMode);
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, false);
                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PopulateSavegamesTR4()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR4 + (i * Globals.SAVEGAME_SIZE_TRX2);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR4) / Globals.SAVEGAME_SIZE_TRX2;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR4.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        string levelName = LevelNames.TR4[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus);
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, false);
                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PopulateSavegamesTR5()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR5 + (i * Globals.SAVEGAME_SIZE_TRX2);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR5) / Globals.SAVEGAME_SIZE_TRX2;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR5.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        string levelName = LevelNames.TR5[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus);
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, false);
                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PopulateSavegamesTR6()
        {
            lstSavegames.Items.Clear();

            try
            {
                byte[] fileData = File.ReadAllBytes(savegamePath);

                for (int i = 0; i < Globals.MAX_SAVEGAMES; i++)
                {
                    int currentSavegameOffset = BASE_SAVEGAME_OFFSET_TR6 + (i * Globals.SAVEGAME_SIZE_TRX2);
                    int slot = (currentSavegameOffset - BASE_SAVEGAME_OFFSET_TR6) / Globals.SAVEGAME_SIZE_TRX2;

                    byte levelIndex = fileData[currentSavegameOffset + LEVEL_INDEX_OFFSET];
                    bool isSavegamePresent = BitConverter.ToInt32(fileData, currentSavegameOffset + Globals.SLOT_STATUS_OFFSET) != 0;

                    if (isSavegamePresent && LevelNames.TR6.ContainsKey(levelIndex))
                    {
                        Int32 saveNumber = BitConverter.ToInt32(fileData, currentSavegameOffset + SAVE_NUMBER_OFFSET);
                        bool isNewGamePlus = BitConverter.ToInt32(fileData, currentSavegameOffset + NEW_GAME_PLUS_OFFSET) != 0;
                        string levelName = LevelNames.TR6[levelIndex];

                        Savegame savegame = new Savegame(currentSavegameOffset, saveNumber, levelName, isNewGamePlus, true);
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                    else
                    {
                        Savegame savegame = new Savegame(currentSavegameOffset, 0, null, false);
                        savegame.IsEmptySlot = true;
                        savegame.Slot = slot;
                        lstSavegames.Items.Add(savegame);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    ex.Message,
                    Globals.DIALOG_TITLE_ERROR,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

            if (platform != Platform.PC && IsTRXSavegame())
            {
                System.Media.SystemSounds.Exclamation.Play();

                string warningMessage = $"Savegame creation is not currently supported for {platform.ToFriendlyString()}.";

                ThemedMessageBox.Show(
                    this,
                    warningMessage,
                    Globals.DIALOG_TITLE_PLATFORM_NOT_SUPPORTED,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (lstSavegames.SelectedItem.ToString() != Globals.EMPTY_SLOT_TEXT)
            {
                System.Media.SystemSounds.Asterisk.Play();

                string warningMessage = $"Are you sure you wish to overwrite '{(Savegame)lstSavegames.SelectedItem}'?";

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    warningMessage,
                    Globals.DIALOG_TITLE_CREATE_SAVEGAME,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            CreateSavegameForm createSavegameForm = new CreateSavegameForm(CURRENT_TAB, savegamePath, (lstSavegames.SelectedItem as Savegame).Slot, (lstSavegames.SelectedItem as Savegame).Offset, slblStatus);
            createSavegameForm.TopMost = TopMost;
            createSavegameForm.ShowDialog(this);

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
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    Globals.DIALOG_MSG_NO_SAVEGAME_SELECTED,
                    Globals.DIALOG_TITLE_NO_SAVEGAME_SELECTED,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (lstSavegames.SelectedItem != null && lstSavegames.SelectedItem.ToString() == Globals.EMPTY_SLOT_TEXT)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    Globals.DIALOG_MSG_CANNOT_DELETE_EMPTY_SLOTS,
                    Globals.DIALOG_TITLE_INVALID_ACTION,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            System.Media.SystemSounds.Exclamation.Play();

            string warningMessage = $"Are you sure you wish to delete '{(Savegame)lstSavegames.SelectedItem}'?";

            DialogResult result = ThemedMessageBox.Show(
                this,
                warningMessage,
                Globals.DIALOG_TITLE_CONFIRMATION,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            slblStatus.Text = Globals.STATUS_MSG_DELETION_IN_PROGRESS;

            int SAVEGAME_SIZE = IsTRXSavegame() ? SAVEGAME_SIZE_TRX : Globals.SAVEGAME_SIZE_TRX2;

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
                    slblStatus.Text = Globals.STATUS_MSG_DELETION_ERROR;

                    System.Media.SystemSounds.Hand.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_SAVEGAME_DELETION_ERROR,
                        Globals.DIALOG_TITLE_ERROR,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (args.Cancelled)
                {
                    slblStatus.Text = Globals.STATUS_MSG_DELETION_CANCEL;

                    System.Media.SystemSounds.Asterisk.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_OPERATION_CANCELED,
                        Globals.DIALOG_TITLE_CANCELED,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    string deletedSavegameString = (string)args.Result;

                    slblStatus.Text = $"{Globals.STATUS_MSG_DELETION_SUCCESS} '{deletedSavegameString}'";

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

            slblStatus.Text = Globals.STATUS_MSG_REORDER_IN_PROGRESS;

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
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_ERROR;

                    System.Media.SystemSounds.Hand.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_REORDER_ERROR,
                        Globals.DIALOG_TITLE_ERROR,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (e.Cancelled)
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_CANCELED;

                    System.Media.SystemSounds.Asterisk.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_OPERATION_CANCELED,
                        Globals.DIALOG_TITLE_CANCELED,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_SUCCESS;

                    System.Media.SystemSounds.Asterisk.Play();

                    string dialogMessage = $"Successfully reordered {savegamesToMove.Count} savegames.";

                    ThemedMessageBox.Show(
                        this,
                        dialogMessage,
                        Globals.DIALOG_TITLE_SUCCESS,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            slblStatus.Text = Globals.STATUS_MSG_REORDER_IN_PROGRESS;

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
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_ERROR;

                    System.Media.SystemSounds.Hand.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_REORDER_ERROR,
                        Globals.DIALOG_TITLE_ERROR,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (e.Cancelled)
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_CANCELED;

                    System.Media.SystemSounds.Asterisk.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_OPERATION_CANCELED,
                        Globals.DIALOG_TITLE_CANCELED,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_SUCCESS;

                    System.Media.SystemSounds.Asterisk.Play();

                    string dialogMessage = $"Successfully reordered {savegamesToMove.Count} savegames.";

                    ThemedMessageBox.Show(
                        this,
                        dialogMessage,
                        Globals.DIALOG_TITLE_SUCCESS,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            slblStatus.Text = Globals.STATUS_MSG_REORDER_IN_PROGRESS;

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
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_ERROR;

                    System.Media.SystemSounds.Hand.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_REORDER_ERROR,
                        Globals.DIALOG_TITLE_ERROR,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (e.Cancelled)
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_CANCELED;

                    System.Media.SystemSounds.Asterisk.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_OPERATION_CANCELED,
                        Globals.DIALOG_TITLE_CANCELED,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_SUCCESS;

                    System.Media.SystemSounds.Asterisk.Play();

                    string dialogMessage = $"Successfully reordered {savegamesToMove.Count} savegames.";

                    ThemedMessageBox.Show(
                        this,
                        dialogMessage,
                        Globals.DIALOG_TITLE_SUCCESS,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            slblStatus.Text = Globals.STATUS_MSG_REORDER_IN_PROGRESS;

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
                                byte[] savegameBytes = new byte[Globals.SAVEGAME_SIZE_TRX2];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR4 + (i * Globals.SAVEGAME_SIZE_TRX2);

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
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_ERROR;

                    System.Media.SystemSounds.Hand.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_REORDER_ERROR,
                        Globals.DIALOG_TITLE_ERROR,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (e.Cancelled)
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_CANCELED;

                    System.Media.SystemSounds.Asterisk.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_OPERATION_CANCELED,
                        Globals.DIALOG_TITLE_CANCELED,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_SUCCESS;

                    System.Media.SystemSounds.Asterisk.Play();

                    string dialogMessage = $"Successfully reordered {savegamesToMove.Count} savegames.";

                    ThemedMessageBox.Show(
                        this,
                        dialogMessage,
                        Globals.DIALOG_TITLE_SUCCESS,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            slblStatus.Text = Globals.STATUS_MSG_REORDER_IN_PROGRESS;

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
                                byte[] savegameBytes = new byte[Globals.SAVEGAME_SIZE_TRX2];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR5 + (i * Globals.SAVEGAME_SIZE_TRX2);

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
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_ERROR;

                    System.Media.SystemSounds.Hand.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_REORDER_ERROR,
                        Globals.DIALOG_TITLE_ERROR,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (e.Cancelled)
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_CANCELED;

                    System.Media.SystemSounds.Asterisk.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_OPERATION_CANCELED,
                        Globals.DIALOG_TITLE_CANCELED,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_SUCCESS;

                    System.Media.SystemSounds.Asterisk.Play();

                    string dialogMessage = $"Successfully reordered {savegamesToMove.Count} savegames.";

                    ThemedMessageBox.Show(
                        this,
                        dialogMessage,
                        Globals.DIALOG_TITLE_SUCCESS,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            slblStatus.Text = Globals.STATUS_MSG_REORDER_IN_PROGRESS;

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
                                byte[] savegameBytes = new byte[Globals.SAVEGAME_SIZE_TRX2];

                                for (int offset = 0; offset < savegameBytes.Length; offset++)
                                {
                                    savegameStream.Seek(currentSavegame.Offset + offset, SeekOrigin.Begin);
                                    byte currentByte = (byte)savegameStream.ReadByte();

                                    savegameBytes[offset] = currentByte;
                                }

                                currentSavegame.SavegameBytes = savegameBytes;

                                currentSavegame.Slot = i;
                                currentSavegame.Offset = BASE_SAVEGAME_OFFSET_TR6 + (i * Globals.SAVEGAME_SIZE_TRX2);

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
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_ERROR;

                    System.Media.SystemSounds.Hand.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_REORDER_ERROR,
                        Globals.DIALOG_TITLE_ERROR,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (e.Cancelled)
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_CANCELED;

                    System.Media.SystemSounds.Asterisk.Play();

                    ThemedMessageBox.Show(
                        this,
                        Globals.DIALOG_MSG_OPERATION_CANCELED,
                        Globals.DIALOG_TITLE_CANCELED,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    slblStatus.Text = Globals.STATUS_MSG_REORDER_SUCCESS;

                    System.Media.SystemSounds.Asterisk.Play();

                    string dialogMessage = $"Successfully reordered {savegamesToMove.Count} savegames.";

                    ThemedMessageBox.Show(
                        this,
                        dialogMessage,
                        Globals.DIALOG_TITLE_SUCCESS,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                PopulateSavegamesTR6();
                EnableButtons();
            };

            bgWorker.RunWorkerAsync();
        }

        private bool IsPatch5SavegameFile(byte[] fileData)
        {
            return fileData[Globals.SAVEFILE_VERSION_OFFSET] == Globals.SAVEFILE_TRX_PATCH5;
        }

        private void btnReorder_Click(object sender, EventArgs e)
        {
            DisableButtons();

            List<Savegame> savegamesToMove = new List<Savegame>();

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.Show();

            isWriting = true;

            if (CURRENT_TAB == Globals.TAB_TR1)
            {
                ReorderSavegamesTR1(savegamesToMove);
            }
            else if (CURRENT_TAB == Globals.TAB_TR2)
            {
                ReorderSavegamesTR2(savegamesToMove);
            }
            else if (CURRENT_TAB == Globals.TAB_TR3)
            {
                ReorderSavegamesTR3(savegamesToMove);
            }
            else if (CURRENT_TAB == Globals.TAB_TR4)
            {
                ReorderSavegamesTR4(savegamesToMove);
            }
            else if (CURRENT_TAB == Globals.TAB_TR5)
            {
                ReorderSavegamesTR5(savegamesToMove);
            }
            else if (CURRENT_TAB == Globals.TAB_TR6)
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
                btnDelete.Enabled = (selectedIndex != -1 && lstSavegames.SelectedItem.ToString() != Globals.EMPTY_SLOT_TEXT);
                btnNew.Enabled = (selectedIndex != -1);
            }
        }

        private void lstSavegames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete &&
                lstSavegames.SelectedIndex >= 0 &&
                lstSavegames.SelectedItem?.ToString() != Globals.EMPTY_SLOT_TEXT)
            {
                btnDelete_Click(btnDelete, EventArgs.Empty);
            }
        }
    }
}
