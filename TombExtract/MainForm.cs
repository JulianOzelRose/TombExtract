using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TombExtract
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            tr1Utilities = new TR1Utilities(this);
            tr2Utilities = new TR2Utilities(this);
            tr3Utilities = new TR3Utilities(this);
            tr4Utilities = new TR4Utilities(this);
            tr5Utilities = new TR5Utilities(this);
            tr6Utilities = new TR6Utilities(this);
        }

        // Utils
        private readonly TR1Utilities tr1Utilities;
        private readonly TR2Utilities tr2Utilities;
        private readonly TR3Utilities tr3Utilities;
        private readonly TR4Utilities tr4Utilities;
        private readonly TR5Utilities tr5Utilities;
        private readonly TR6Utilities tr6Utilities;

        // Tabs
        private const int TAB_TR1 = 0;
        private const int TAB_TR2 = 1;
        private const int TAB_TR3 = 2;
        private const int TAB_TR4 = 3;
        private const int TAB_TR5 = 4;
        private const int TAB_TR6 = 5;

        // Progress form
        private static ProgressForm progressForm;

        // Savegame paths (TRX)
        private string savegameDestinationPathTRX;
        private string savegameSourcePathTRX;

        // Savegame paths (TRX2)
        private string savegameDestinationPathTRX2;
        private string savegameSourcePathTRX2;

        // Savegame versioning
        private const int SAVEFILE_SIZE_TRX_PREPATCH = 0x152004;
        private const int SAVEFILE_SIZE_TRX_PATCH5 = 0x272004;
        private const int SAVEFILE_SIZE_TRX2 = 0x3DCA04;
        private const int SAVEFILE_VERSION_OFFSET = 0x000;

        private const byte SAVEFILE_TRX_PREPATCH = 0x3B;
        private const byte SAVEFILE_TRX_PATCH5 = 0x3C;
        private const byte SAVEFILE_TRX2_FORMAT = 0x28;

        // Config
        private const string CONFIG_FILE_NAME = "TombExtract.ini";

        // Misc
        private bool isSyncingPlatformComboBoxes = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            ReadConfigFile();
            InitializePlatformComboBoxes();
        }

        private void InitializePlatformComboBoxes()
        {
            InitializePlatformComboBox(cmbSourceFormatTR1);
            InitializePlatformComboBox(cmbDestinationFormatTR1);

            InitializePlatformComboBox(cmbSourceFormatTR2);
            InitializePlatformComboBox(cmbDestinationFormatTR2);

            InitializePlatformComboBox(cmbSourceFormatTR3);
            InitializePlatformComboBox(cmbDestinationFormatTR3);
        }

        private void InitializePlatformComboBox(ComboBox comboBox)
        {
            comboBox.DataSource = Enum.GetValues(typeof(Platform)).Cast<Platform>().ToList();

            comboBox.Format += (s, e) =>
            {
                if (e.ListItem is Platform platform)
                {
                    e.Value = platform.ToFriendlyString();
                }
            };

            comboBox.SelectedItem = Platform.PC;
        }

        public static class ThemedMessageBox
        {
            public static DialogResult Show(
                IWin32Window owner,
                string message,
                string title = "",
                MessageBoxButtons buttons = MessageBoxButtons.OK,
                MessageBoxIcon icon = MessageBoxIcon.None)
            {
                using (var dlg = new ThemedDialog(message, title, buttons, icon))
                {
                    return dlg.ShowDialog(owner);
                }
            }
        }

        private void ApplyDarkMode()
        {
            ThemeUtilities.ApplyDarkMode(this);
            ThemeUtilities.ApplyDarkTitleBar(this);

            tabGame.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabGame.DrawItem -= tabGame_DrawItem;
            tabGame.DrawItem += tabGame_DrawItem;

            ThemeUtilities.DARK_MODE_ENABLED = true;
        }

        private void ApplyLightMode()
        {
            ThemeUtilities.ApplyLightMode(this);

            tabGame.DrawItem -= tabGame_DrawItem;
            tabGame.DrawMode = TabDrawMode.Normal;
            this.BackColor = SystemColors.Control;

            ThemeUtilities.ApplyLightTitleBar(this);
            ThemeUtilities.DARK_MODE_ENABLED = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_COMPOSITED = 0x02000000;

                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_COMPOSITED;
                return cp;
            }
        }

        private void tabGame_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage page = tabControl.TabPages[e.Index];

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color backColor = selected ? ThemeUtilities.Surface : ThemeUtilities.Background;

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                page.Font,
                e.Bounds,
                ThemeUtilities.Text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }

        private void ReadConfigFile()
        {
            string rootFolder = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(rootFolder, CONFIG_FILE_NAME);

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.StartsWith("StatusBar"))
                    {
                        if (bool.TryParse(line.Substring("StatusBar=".Length), out bool statusBar))
                        {
                            tsmiStatusBar.Checked = statusBar;
                            ssrStatusStrip.Visible = statusBar;
                            slblStatus.Visible = statusBar;

                            if (!statusBar)
                            {
                                this.Height -= ssrStatusStrip.Height;
                            }
                        }
                    }
                    else if (line.StartsWith("DarkMode="))
                    {
                        if (bool.TryParse(line.Substring("DarkMode=".Length), out bool darkMode) && darkMode)
                        {
                            ApplyDarkMode();
                            tsmiDarkMode.Checked = true;
                        }
                    }
                }
            }
            else
            {
                UpdateConfigFile();
            }
        }

        private void UpdateConfigFile()
        {
            string rootFolder = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(rootFolder, CONFIG_FILE_NAME);

            string content = $"StatusBar={tsmiStatusBar.Checked}\n";
            content += $"DarkMode={tsmiDarkMode.Checked}";

            File.WriteAllText(filePath, content);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    "Exiting in the middle of a write operation could result in a corrupted savegame file. Are you sure you wish to exit?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            UpdateConfigFile();
        }

        private bool IsAnyWriting()
        {
            return tr1Utilities.IsWriting() || tr2Utilities.IsWriting() || tr3Utilities.IsWriting() || tr4Utilities.IsWriting() || tr5Utilities.IsWriting() || tr6Utilities.IsWriting();
        }

        private bool IsTRXTabSelected()
        {
            return tabGame.SelectedIndex == TAB_TR1 || tabGame.SelectedIndex == TAB_TR2 || tabGame.SelectedIndex == TAB_TR3;
        }

        private void CreateBackup()
        {
            string savegameDestinationPath = IsTRXTabSelected() ? savegameDestinationPathTRX : savegameDestinationPathTRX2;

            if (!string.IsNullOrEmpty(savegameDestinationPath) && File.Exists(savegameDestinationPath))
            {
                string directory = Path.GetDirectoryName(savegameDestinationPath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(savegameDestinationPath);
                string fileExtension = Path.GetExtension(savegameDestinationPath);

                string backupFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}{fileExtension}.bak");

                if (File.Exists(backupFilePath))
                {
                    File.SetAttributes(backupFilePath, File.GetAttributes(backupFilePath) & ~FileAttributes.ReadOnly);
                }

                File.Copy(savegameDestinationPath, backupFilePath, true);

                slblStatus.Text = $"Created backup: \"{backupFilePath}\"";
            }
        }

        private void EnableButtonsConditionally()
        {
            string savegameDestinationPath = IsTRXTabSelected() ? savegameDestinationPathTRX : savegameDestinationPathTRX2;
            bool isDestinationFilePresent = !string.IsNullOrEmpty(savegameDestinationPath) && File.Exists(savegameDestinationPath);

            // Group TRX and TRX2 elements
            CheckedListBox[] sourceSavegameLists = IsTRXTabSelected()
                ? new[] { cklSourceSavegamesTR1, cklSourceSavegamesTR2, cklSourceSavegamesTR3 }
                : new[] { cklSourceSavegamesTR4, cklSourceSavegamesTR5, cklSourceSavegamesTR6 };

            Button[] extractButtons = IsTRXTabSelected()
                ? new[] { btnExtractTR1, btnExtractTR2, btnExtractTR3 }
                : new[] { btnExtractTR4, btnExtractTR5, btnExtractTR6 };

            Button[] selectAllButtons = IsTRXTabSelected()
                ? new[] { btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3 }
                : new[] { btnSelectAllTR4, btnSelectAllTR5, btnSelectAllTR6 };

            Button[] manageSlotsButtons = IsTRXTabSelected()
                ? new[] { btnManageSlotsTR1, btnManageSlotsTR2, btnManageSlotsTR3 }
                : new[] { btnManageSlotsTR4, btnManageSlotsTR5, btnManageSlotsTR6 };

            bool hasSourceSavegames = false;

            // Enable/Disable relevant buttons
            for (int i = 0; i < sourceSavegameLists.Length; i++)
            {
                bool hasSaves = sourceSavegameLists[i].Items.Count > 0;
                extractButtons[i].Enabled = hasSaves && isDestinationFilePresent;
                selectAllButtons[i].Enabled = hasSaves && isDestinationFilePresent;
                manageSlotsButtons[i].Enabled = isDestinationFilePresent;

                if (hasSaves)
                {
                    hasSourceSavegames = true;
                }
            }

            // Enable extraction only if there are source savegames
            tsmiExtract.Enabled = hasSourceSavegames && isDestinationFilePresent;

            // Enable backup option only if a destination file exists
            tsmiBackupDestinationFile.Enabled = isDestinationFilePresent;
        }

        private bool IsValidSavegameFileTRX(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            byte[] fileData = File.ReadAllBytes(path);

            long saveFileSize = fileInfo.Length;
            byte saveFileVersion = GetSaveFileVersion(fileData);

            if (saveFileVersion == SAVEFILE_TRX_PREPATCH && saveFileSize >= SAVEFILE_SIZE_TRX_PREPATCH)
            {
                return true;
            }

            if (saveFileVersion == SAVEFILE_TRX_PATCH5 && saveFileSize >= SAVEFILE_SIZE_TRX_PATCH5)
            {
                return true;
            }

            return false;
        }

        private bool IsValidSavegameFileTRX2(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            byte[] fileData = File.ReadAllBytes(path);

            long saveFileSize = fileInfo.Length;
            byte saveFileVersion = GetSaveFileVersion(fileData);

            if (saveFileVersion == SAVEFILE_TRX2_FORMAT && saveFileSize >= SAVEFILE_SIZE_TRX2)
            {
                return true;
            }

            return false;
        }

        public byte GetSaveFileVersion(byte[] fileData)
        {
            return fileData[SAVEFILE_VERSION_OFFSET];
        }

        public bool IsPatch5SavegameFileTRX(byte[] fileData)
        {
            return fileData[SAVEFILE_VERSION_OFFSET] == SAVEFILE_TRX_PATCH5;
        }

        public bool IsPrepatchSavegameFileTRX(byte[] fileData)
        {
            return fileData[SAVEFILE_VERSION_OFFSET] == SAVEFILE_TRX_PREPATCH;
        }

        private void SetSourceFileTRX(string path)
        {
            if (!IsValidSavegameFileTRX(path))
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Not a valid Tomb Raider I-III Remastered savegame file.",
                    "Invalid Savegame File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            txtSourceFilePath.Text = path;
            savegameSourcePathTRX = path;

            tr1Utilities.SetSavegameSourcePath(path);
            tr1Utilities.SetSourceFormat((Platform)cmbSourceFormatTR1.SelectedItem);
            tr1Utilities.PopulateSourceSavegames(cklSourceSavegamesTR1);

            tr2Utilities.SetSavegameSourcePath(path);
            tr2Utilities.SetSourceFormat((Platform)cmbSourceFormatTR2.SelectedItem);
            tr2Utilities.PopulateSourceSavegames(cklSourceSavegamesTR2);

            tr3Utilities.SetSavegameSourcePath(path);
            tr3Utilities.SetSourceFormat((Platform)cmbSourceFormatTR3.SelectedItem);
            tr3Utilities.PopulateSourceSavegames(cklSourceSavegamesTR3);

            EnableButtonsConditionally();

            int numSaves = cklSourceSavegamesTR1.Items.Count +
                           cklSourceSavegamesTR2.Items.Count +
                           cklSourceSavegamesTR3.Items.Count;

            slblStatus.Text = $"{numSaves} savegame(s) found in \"{path}\"";
        }

        private void SetSourceFileTRX2(string path)
        {
            if (!IsValidSavegameFileTRX2(path))
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Not a valid Tomb Raider IV-VI Remastered savegame file.",
                    "Invalid Savegame File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            txtSourceFilePath.Text = path;
            savegameSourcePathTRX2 = path;

            tr4Utilities.SetSavegameSourcePath(path);
            tr4Utilities.PopulateSourceSavegames(cklSourceSavegamesTR4);

            tr5Utilities.SetSavegameSourcePath(path);
            tr5Utilities.PopulateSourceSavegames(cklSourceSavegamesTR5);

            tr6Utilities.SetSavegameSourcePath(path);
            tr6Utilities.PopulateSourceSavegames(cklSourceSavegamesTR6);

            EnableButtonsConditionally();

            int numSaves = cklSourceSavegamesTR4.Items.Count +
                           cklSourceSavegamesTR5.Items.Count +
                           cklSourceSavegamesTR6.Items.Count;

            slblStatus.Text = $"{numSaves} savegame(s) found in \"{path}\"";
        }

        private void SetDestinationFileTRX(string path)
        {
            if (!IsValidSavegameFileTRX(path))
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Not a valid Tomb Raider I-III Remastered savegame file.",
                    "Invalid Savegame File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            txtDestinationFilePath.Text = path;
            savegameDestinationPathTRX = path;

            tr1Utilities.SetSavegameDestinationPath(path);
            tr1Utilities.SetDestinationFormat((Platform)cmbDestinationFormatTR1.SelectedItem);
            tr1Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR1);

            tr2Utilities.SetSavegameDestinationPath(path);
            tr2Utilities.SetDestinationFormat((Platform)cmbDestinationFormatTR2.SelectedItem);
            tr2Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR2);

            tr3Utilities.SetSavegameDestinationPath(path);
            tr3Utilities.SetDestinationFormat((Platform)cmbDestinationFormatTR3.SelectedItem);
            tr3Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR3);

            EnableButtonsConditionally();

            slblStatus.Text = $"Opened destination file: \"{path}\"";
        }

        private void SetDestinationFileTRX2(string path)
        {
            if (!IsValidSavegameFileTRX2(path))
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Not a valid Tomb Raider IV-VI Remastered savegame file.",
                    "Invalid Savegame File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            txtDestinationFilePath.Text = path;
            savegameDestinationPathTRX2 = path;

            tr4Utilities.SetSavegameDestinationPath(path);
            tr4Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR4);

            tr5Utilities.SetSavegameDestinationPath(path);
            tr5Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR5);

            tr6Utilities.SetSavegameDestinationPath(path);
            tr6Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR6);

            EnableButtonsConditionally();

            slblStatus.Text = $"Opened destination file: \"{path}\"";
        }

        private void cklSourceSavegamesTR1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetSourceFileTRX(filePath);
            }
        }

        private void cklSourceSavegamesTR1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstDestinationSavegamesTR1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetDestinationFileTRX(filePath);
            }
        }

        private void lstDestinationSavegamesTR1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void cklSourceSavegamesTR2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetSourceFileTRX(filePath);
            }
        }

        private void cklSourceSavegamesTR2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstDestinationSavegamesTR2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetDestinationFileTRX(filePath);
            }
        }

        private void lstDestinationSavegamesTR2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void cklSourceSavegamesTR3_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetSourceFileTRX(filePath);
            }
        }

        private void cklSourceSavegamesTR3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstDestinationSavegamesTR3_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetDestinationFileTRX(filePath);
            }
        }

        private void lstDestinationSavegamesTR3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void cklSourceSavegamesTR4_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetSourceFileTRX2(filePath);
            }
        }

        private void cklSourceSavegamesTR4_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void cklSourceSavegamesTR5_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetSourceFileTRX2(filePath);
            }
        }

        private void cklSourceSavegamesTR5_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void cklSourceSavegamesTR6_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetSourceFileTRX2(filePath);
            }
        }

        private void cklSourceSavegamesTR6_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstDestinationSavegamesTR4_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetDestinationFileTRX2(filePath);
            }
        }

        private void lstDestinationSavegamesTR4_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstDestinationSavegamesTR5_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetDestinationFileTRX2(filePath);
            }
        }

        private void lstDestinationSavegamesTR5_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstDestinationSavegamesTR6_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetDestinationFileTRX2(filePath);
            }
        }

        private void lstDestinationSavegamesTR6_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void BrowseSourceFileTRX()
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = "C:\\";
                fileBrowserDialog.Title = "Select Tomb Raider I-III source file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    SetSourceFileTRX(fileBrowserDialog.FileName);
                }
            }
        }

        private void BrowseSourceFileTRX2()
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = "C:\\";
                fileBrowserDialog.Title = "Select Tomb Raider IV-VI source file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    SetSourceFileTRX2(fileBrowserDialog.FileName);
                }
            }
        }

        private void BrowseDestinationFileTRX()
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TRX");
                fileBrowserDialog.Title = "Select Tomb Raider I-III destination file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    SetDestinationFileTRX(fileBrowserDialog.FileName);
                }
            }
        }

        private void BrowseDestinationFileTRX2()
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TRX2");
                fileBrowserDialog.Title = "Select Tomb Raider IV-VI destination file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    SetDestinationFileTRX2(fileBrowserDialog.FileName);
                }
            }
        }

        private void btnBrowseSourceFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (IsTRXTabSelected())
            {
                BrowseSourceFileTRX();
            }
            else
            {
                BrowseSourceFileTRX2();
            }
        }

        private void btnBrowseDestinationFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (IsTRXTabSelected())
            {
                BrowseDestinationFileTRX();
            }
            else
            {
                BrowseDestinationFileTRX2();
            }
        }

        private void btnExitTR1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitTR2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitTR3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitTR4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitTR5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitTR6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExtractSavegamesTR1()
        {
            List<Savegame> selectedSavegames = new List<Savegame>();

            for (int i = 0; i < cklSourceSavegamesTR1.Items.Count; i++)
            {
                if (cklSourceSavegamesTR1.GetItemChecked(i))
                {
                    Savegame savegame = cklSourceSavegamesTR1.Items[i] as Savegame;

                    if (savegame != null)
                    {
                        selectedSavegames.Add(savegame);
                    }
                }
            }

            if (selectedSavegames.Count == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Please select at least one savegame to convert.",
                    "No Savegames Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame source file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame destination file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int numOverwrites = tr1Utilities.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                System.Media.SystemSounds.Asterisk.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    $"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.TopMost = tsmiAlwaysOnTop.Checked;
            progressForm.Show();

            tr1Utilities.SetProgressForm(progressForm);
            tr1Utilities.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR1, slblStatus);
        }

        private void ExtractSavegamesTR2()
        {
            List<Savegame> selectedSavegames = new List<Savegame>();

            for (int i = 0; i < cklSourceSavegamesTR2.Items.Count; i++)
            {
                if (cklSourceSavegamesTR2.GetItemChecked(i))
                {
                    Savegame savegame = cklSourceSavegamesTR2.Items[i] as Savegame;

                    if (savegame != null)
                    {
                        selectedSavegames.Add(savegame);
                    }
                }
            }

            if (selectedSavegames.Count == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Please select at least one savegame to convert.",
                    "No Savegames Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame source file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame destination file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int numOverwrites = tr2Utilities.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                System.Media.SystemSounds.Asterisk.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    $"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.TopMost = tsmiAlwaysOnTop.Checked;
            progressForm.Show();

            tr2Utilities.SetProgressForm(progressForm);
            tr2Utilities.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR2, slblStatus);
        }

        private void ExtractSavegamesTR3()
        {
            List<Savegame> selectedSavegames = new List<Savegame>();

            for (int i = 0; i < cklSourceSavegamesTR3.Items.Count; i++)
            {
                if (cklSourceSavegamesTR3.GetItemChecked(i))
                {
                    Savegame savegame = cklSourceSavegamesTR3.Items[i] as Savegame;

                    if (savegame != null)
                    {
                        selectedSavegames.Add(savegame);
                    }
                }
            }

            if (selectedSavegames.Count == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Please select at least one savegame to convert.",
                    "No Savegames Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame source file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame destination file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int numOverwrites = tr3Utilities.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                System.Media.SystemSounds.Asterisk.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    $"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.TopMost = tsmiAlwaysOnTop.Checked;
            progressForm.Show();

            tr3Utilities.SetProgressForm(progressForm);
            tr3Utilities.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR3, slblStatus);
        }

        private void ExtractSavegamesTR4()
        {
            List<Savegame> selectedSavegames = new List<Savegame>();

            for (int i = 0; i < cklSourceSavegamesTR4.Items.Count; i++)
            {
                if (cklSourceSavegamesTR4.GetItemChecked(i))
                {
                    Savegame savegame = cklSourceSavegamesTR4.Items[i] as Savegame;

                    if (savegame != null)
                    {
                        selectedSavegames.Add(savegame);
                    }
                }
            }

            if (selectedSavegames.Count == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Please select at least one savegame to convert.",
                    "No Savegames Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame source file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame destination file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int numOverwrites = tr4Utilities.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                System.Media.SystemSounds.Asterisk.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    $"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.TopMost = tsmiAlwaysOnTop.Checked;
            progressForm.Show();

            tr4Utilities.SetProgressForm(progressForm);
            tr4Utilities.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR4, slblStatus);
        }

        private void ExtractSavegamesTR5()
        {
            List<Savegame> selectedSavegames = new List<Savegame>();

            for (int i = 0; i < cklSourceSavegamesTR5.Items.Count; i++)
            {
                if (cklSourceSavegamesTR5.GetItemChecked(i))
                {
                    Savegame savegame = cklSourceSavegamesTR5.Items[i] as Savegame;

                    if (savegame != null)
                    {
                        selectedSavegames.Add(savegame);
                    }
                }
            }

            if (selectedSavegames.Count == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Please select at least one savegame to convert.",
                    "No Savegames Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame source file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame destination file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int numOverwrites = tr5Utilities.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                System.Media.SystemSounds.Asterisk.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    $"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.TopMost = tsmiAlwaysOnTop.Checked;
            progressForm.Show();

            tr5Utilities.SetProgressForm(progressForm);
            tr5Utilities.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR5, slblStatus);
        }

        private void ExtractSavegamesTR6()
        {
            List<Savegame> selectedSavegames = new List<Savegame>();

            for (int i = 0; i < cklSourceSavegamesTR6.Items.Count; i++)
            {
                if (cklSourceSavegamesTR6.GetItemChecked(i))
                {
                    Savegame savegame = cklSourceSavegamesTR6.Items[i] as Savegame;

                    if (savegame != null)
                    {
                        selectedSavegames.Add(savegame);
                    }
                }
            }

            if (selectedSavegames.Count == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Please select at least one savegame to convert.",
                    "No Savegames Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame source file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                System.Media.SystemSounds.Hand.Play();

                ThemedMessageBox.Show(
                    this,
                    "Could not find savegame destination file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int numOverwrites = tr6Utilities.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                System.Media.SystemSounds.Asterisk.Play();

                DialogResult result = ThemedMessageBox.Show(
                    this,
                    $"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.TopMost = tsmiAlwaysOnTop.Checked;
            progressForm.Show();

            tr6Utilities.SetProgressForm(progressForm);
            tr6Utilities.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR6, slblStatus);
        }

        private bool IsAnyChallengeModeSavegameChecked(CheckedListBox cklSavegames)
        {
            for (int i = 0; i < cklSavegames.Items.Count; i++)
            {
                if (!cklSavegames.GetItemChecked(i))
                {
                    continue;
                }

                if (cklSavegames.Items[i] is Savegame savegame && savegame.IsChallengeMode)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnExtractTR1_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            byte[] sourceFileData = File.ReadAllBytes(savegameSourcePathTRX);
            byte[] destinationFileData = File.ReadAllBytes(savegameDestinationPathTRX);

            bool isSourcePatch5 = IsPatch5SavegameFileTRX(sourceFileData);
            bool isDestinationPrepatch = !IsPatch5SavegameFileTRX(destinationFileData);

            if (isSourcePatch5 && isDestinationPrepatch)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Unable to convert Patch 5 to pre-patch savegames.",
                    "Unable to Convert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (isSourcePatch5 && !isDestinationPrepatch)
            {
                Platform sourcePlatform = (Platform)cmbSourceFormatTR1.SelectedItem;
                Platform destinationPlatform = (Platform)cmbDestinationFormatTR1.SelectedItem;

                if (sourcePlatform == Platform.PC && destinationPlatform == Platform.Android)
                {
                    bool isAnyChallengeModeSavegameChecked = IsAnyChallengeModeSavegameChecked(cklSourceSavegamesTR1);

                    if (isAnyChallengeModeSavegameChecked)
                    {
                        System.Media.SystemSounds.Exclamation.Play();

                        string warningMessage = $"Challenge Mode savegames may not convert correctly from PC to Android. Proceed anyway?";

                        DialogResult result = ThemedMessageBox.Show(
                            this,
                            warningMessage,
                            "Conversion Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                if (sourcePlatform == Platform.NintendoSwitch && destinationPlatform != Platform.NintendoSwitch)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    string warningMessage = $"Unable to convert {sourcePlatform.ToFriendlyString()} savegames to {destinationPlatform.ToFriendlyString()} for Patch 5.";

                    ThemedMessageBox.Show(
                        this,
                        warningMessage,
                        "Unable to Convert",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            if (!isSourcePatch5 && !isDestinationPrepatch)
            {
                Platform sourcePlatform = (Platform)cmbSourceFormatTR1.SelectedItem;
                Platform destinationPlatform = (Platform)cmbDestinationFormatTR1.SelectedItem;

                if (sourcePlatform != Platform.PC)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    string warningMessage = $"Unable to convert pre-patch savegames from {sourcePlatform.ToFriendlyString()} to {destinationPlatform.ToFriendlyString()}.";

                    ThemedMessageBox.Show(
                        this,
                        warningMessage,
                        "Unable to Convert",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            ExtractSavegamesTR1();
        }

        private void btnExtractTR2_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            byte[] sourceFileData = File.ReadAllBytes(savegameSourcePathTRX);
            byte[] destinationFileData = File.ReadAllBytes(savegameDestinationPathTRX);

            bool isSourcePatch5 = IsPatch5SavegameFileTRX(sourceFileData);
            bool isDestinationPrepatch = !IsPatch5SavegameFileTRX(destinationFileData);

            if (isSourcePatch5 && isDestinationPrepatch)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Unable to convert Patch 5 to pre-patch savegames.",
                    "Unable to Convert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (isSourcePatch5 && !isDestinationPrepatch)
            {
                Platform sourcePlatform = (Platform)cmbSourceFormatTR2.SelectedItem;
                Platform destinationPlatform = (Platform)cmbDestinationFormatTR2.SelectedItem;

                if (sourcePlatform == Platform.PC && destinationPlatform == Platform.Android)
                {
                    bool isAnyChallengeModeSavegameChecked = IsAnyChallengeModeSavegameChecked(cklSourceSavegamesTR2);

                    if (isAnyChallengeModeSavegameChecked)
                    {
                        System.Media.SystemSounds.Exclamation.Play();

                        string warningMessage = $"Challenge Mode savegames may not convert correctly from PC to Android. Proceed anyway?";

                        DialogResult result = ThemedMessageBox.Show(
                            this,
                            warningMessage,
                            "Conversion Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                if (sourcePlatform == Platform.NintendoSwitch && destinationPlatform != Platform.NintendoSwitch)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    string warningMessage = $"Unable to convert {sourcePlatform.ToFriendlyString()} savegames to {destinationPlatform.ToFriendlyString()} for Patch 5.";

                    ThemedMessageBox.Show(
                        this,
                        warningMessage,
                        "Unable to Convert",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            if (!isSourcePatch5 && !isDestinationPrepatch)
            {
                Platform sourcePlatform = (Platform)cmbSourceFormatTR2.SelectedItem;
                Platform destinationPlatform = (Platform)cmbDestinationFormatTR2.SelectedItem;

                if (sourcePlatform != Platform.PC)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    string warningMessage = $"Unable to convert pre-patch savegames from {sourcePlatform.ToFriendlyString()} to {destinationPlatform.ToFriendlyString()}.";

                    ThemedMessageBox.Show(
                        this,
                        warningMessage,
                        "Unable to Convert",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            ExtractSavegamesTR2();
        }

        private void btnExtractTR3_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            byte[] sourceFileData = File.ReadAllBytes(savegameSourcePathTRX);
            byte[] destinationFileData = File.ReadAllBytes(savegameDestinationPathTRX);

            bool isSourcePatch5 = IsPatch5SavegameFileTRX(sourceFileData);
            bool isDestinationPrepatch = !IsPatch5SavegameFileTRX(destinationFileData);

            if (isSourcePatch5 && isDestinationPrepatch)
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "Unable to convert Patch 5 to pre-patch savegames.",
                    "Unable to Convert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (isSourcePatch5 && !isDestinationPrepatch)
            {
                Platform sourcePlatform = (Platform)cmbSourceFormatTR3.SelectedItem;
                Platform destinationPlatform = (Platform)cmbDestinationFormatTR3.SelectedItem;

                if (sourcePlatform == Platform.PC && destinationPlatform == Platform.Android)
                {
                    bool isAnyChallengeModeSavegameChecked = IsAnyChallengeModeSavegameChecked(cklSourceSavegamesTR3);

                    if (isAnyChallengeModeSavegameChecked)
                    {
                        System.Media.SystemSounds.Exclamation.Play();

                        string warningMessage = $"Challenge Mode savegames may not convert correctly from PC to Android. Proceed anyway?";

                        DialogResult result = ThemedMessageBox.Show(
                            this,
                            warningMessage,
                            "Conversion Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                if (sourcePlatform == Platform.NintendoSwitch && destinationPlatform != Platform.NintendoSwitch)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    string warningMessage = $"Unable to convert {sourcePlatform.ToFriendlyString()} savegames to {destinationPlatform.ToFriendlyString()} for Patch 5.";

                    ThemedMessageBox.Show(
                        this,
                        warningMessage,
                        "Unable to Convert",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            if (!isSourcePatch5 && !isDestinationPrepatch)
            {
                Platform sourcePlatform = (Platform)cmbSourceFormatTR3.SelectedItem;
                Platform destinationPlatform = (Platform)cmbDestinationFormatTR3.SelectedItem;

                if (sourcePlatform != Platform.PC)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    string warningMessage = $"Unable to convert pre-patch savegames from {sourcePlatform.ToFriendlyString()} to {destinationPlatform.ToFriendlyString()}.";

                    ThemedMessageBox.Show(
                        this,
                        warningMessage,
                        "Unable to Convert",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            ExtractSavegamesTR3();
        }

        private void btnExtractTR4_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR4();
        }

        private void btnExtractTR5_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR5();
        }

        private void btnExtractTR6_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR6();
        }

        private void btnSelectAllTR1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklSourceSavegamesTR1.Items.Count; i++)
            {
                cklSourceSavegamesTR1.SetItemChecked(i, true);
            }
        }

        private void btnSelectAllTR2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklSourceSavegamesTR2.Items.Count; i++)
            {
                cklSourceSavegamesTR2.SetItemChecked(i, true);
            }
        }

        private void btnSelectAllTR3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklSourceSavegamesTR3.Items.Count; i++)
            {
                cklSourceSavegamesTR3.SetItemChecked(i, true);
            }
        }

        private void btnSelectAllTR4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklSourceSavegamesTR4.Items.Count; i++)
            {
                cklSourceSavegamesTR4.SetItemChecked(i, true);
            }
        }

        private void btnSelectAllTR5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklSourceSavegamesTR5.Items.Count; i++)
            {
                cklSourceSavegamesTR5.SetItemChecked(i, true);
            }
        }

        private void btnSelectAllTR6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklSourceSavegamesTR6.Items.Count; i++)
            {
                cklSourceSavegamesTR6.SetItemChecked(i, true);
            }
        }

        private void btnAboutTR1_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.TopMost = TopMost;
            aboutForm.ShowDialog();
        }

        private void btnAboutTR2_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.TopMost = TopMost;
            aboutForm.ShowDialog();
        }

        private void btnAboutTR3_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.TopMost = TopMost;
            aboutForm.ShowDialog();
        }

        private void btnAboutTR4_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.TopMost = TopMost;
            aboutForm.ShowDialog();
        }

        private void btnAboutTR5_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.TopMost = TopMost;
            aboutForm.ShowDialog();
        }

        private void btnAboutTR6_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.TopMost = TopMost;
            aboutForm.ShowDialog();
        }

        private void tsmiBrowseTRXSourceFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            BrowseSourceFileTRX();
        }

        private void tsmiBrowseTRXDestinationFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            BrowseDestinationFileTRX();
        }

        private void tsmiBrowseTRX2SourceFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            BrowseSourceFileTRX2();
        }

        private void tsmiBrowseTRX2DestinationFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            BrowseDestinationFileTRX2();
        }

        private void tsmiBackupDestinationFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            CreateBackup();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiViewReadme_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/JulianOzelRose/TombExtract/blob/master/README.md");
        }

        private void tsmiReportBug_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/JulianOzelRose/TombExtract/issues");
        }

        private void tsmiAlwaysOnTop_Click(object sender, EventArgs e)
        {
            this.TopMost = tsmiAlwaysOnTop.Checked;
        }

        private void tsmiStatusBar_Click(object sender, EventArgs e)
        {
            if (tsmiStatusBar.Checked)
            {
                ssrStatusStrip.Visible = true;
                slblStatus.Visible = true;
                this.Height += ssrStatusStrip.Height;
            }
            else
            {
                ssrStatusStrip.Visible = false;
                slblStatus.Visible = false;
                this.Height -= ssrStatusStrip.Height;
            }
        }

        private void tsmiDarkMode_Click(object sender, EventArgs e)
        {
            if (tsmiDarkMode.Checked)
            {
                ApplyDarkMode();
            }
            else
            {
                ApplyLightMode();
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.TopMost = TopMost;
            aboutForm.ShowDialog();
        }

        private void tsmiExtract_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (tabGame.SelectedIndex == TAB_TR1)
            {
                ExtractSavegamesTR1();
            }
            else if (tabGame.SelectedIndex == TAB_TR2)
            {
                ExtractSavegamesTR2();
            }
            else if (tabGame.SelectedIndex == TAB_TR3)
            {
                ExtractSavegamesTR3();
            }
            else if (tabGame.SelectedIndex == TAB_TR4)
            {
                ExtractSavegamesTR4();
            }
            else if (tabGame.SelectedIndex == TAB_TR5)
            {
                ExtractSavegamesTR5();
            }
            else if (tabGame.SelectedIndex == TAB_TR6)
            {
                ExtractSavegamesTR6();
            }
        }

        private void btnManageSlotsTR1_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked, (Platform)cmbDestinationFormatTR1.SelectedItem);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            tr1Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR1);
        }

        private void btnManageSlotsTR2_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked, (Platform)cmbDestinationFormatTR2.SelectedItem);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            tr2Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR2);
        }

        private void btnManageSlotsTR3_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked, (Platform)cmbDestinationFormatTR3.SelectedItem);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            tr3Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR3);
        }

        private void btnManageSlotsTR4_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX2, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked, Platform.PC);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            tr4Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR4);
        }

        private void btnManageSlotsTR5_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX2, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked, Platform.PC);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            tr5Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR5);
        }

        private void btnManageSlotsTR6_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                System.Media.SystemSounds.Exclamation.Play();

                ThemedMessageBox.Show(
                    this,
                    "A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX2, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked, Platform.PC);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            tr6Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR6);
        }

        private void tabGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsTRXTabSelected())
            {
                lblSourceFile.Text = "Tomb Raider I-III source file:";
                lblDestinationFile.Text = "Tomb Raider I-III destination file:";
                txtDestinationFilePath.Text = savegameDestinationPathTRX;
                txtSourceFilePath.Text = savegameSourcePathTRX;
            }
            else
            {
                lblSourceFile.Text = "Tomb Raider IV-VI source file:";
                lblDestinationFile.Text = "Tomb Raider IV-VI destination file:";
                txtDestinationFilePath.Text = savegameDestinationPathTRX2;
                txtSourceFilePath.Text = savegameSourcePathTRX2;
            }
        }

        private void UpdateExtractButtonText()
        {
            if (cmbSourceFormatTR1.SelectedItem == null || cmbDestinationFormatTR1.SelectedItem == null)
            {
                return;
            }

            Platform sourcePlatform = (Platform)cmbSourceFormatTR1.SelectedItem;
            Platform destinationPlatform = (Platform)cmbDestinationFormatTR1.SelectedItem;

            bool isConvert = sourcePlatform != destinationPlatform;

            string buttonText = isConvert ? "Convert" : "Extract";

            btnExtractTR1.Text = buttonText;
            btnExtractTR2.Text = buttonText;
            btnExtractTR3.Text = buttonText;
        }

        private void SyncSourcePlatforms(Platform platform)
        {
            if (isSyncingPlatformComboBoxes)
            {
                return;
            }

            isSyncingPlatformComboBoxes = true;

            cmbSourceFormatTR1.SelectedItem = platform;
            cmbSourceFormatTR2.SelectedItem = platform;
            cmbSourceFormatTR3.SelectedItem = platform;

            tr1Utilities.SetSourceFormat(platform);
            tr2Utilities.SetSourceFormat(platform);
            tr3Utilities.SetSourceFormat(platform);

            tr1Utilities.PopulateSourceSavegames(cklSourceSavegamesTR1);
            tr2Utilities.PopulateSourceSavegames(cklSourceSavegamesTR2);
            tr3Utilities.PopulateSourceSavegames(cklSourceSavegamesTR3);

            isSyncingPlatformComboBoxes = false;

            EnableButtonsConditionally();
            UpdateExtractButtonText();
        }

        private void SyncDestinationPlatforms(Platform platform)
        {
            if (isSyncingPlatformComboBoxes)
            {
                return;
            }

            isSyncingPlatformComboBoxes = true;

            cmbDestinationFormatTR1.SelectedItem = platform;
            cmbDestinationFormatTR2.SelectedItem = platform;
            cmbDestinationFormatTR3.SelectedItem = platform;

            tr1Utilities.SetDestinationFormat(platform);
            tr2Utilities.SetDestinationFormat(platform);
            tr3Utilities.SetDestinationFormat(platform);

            tr1Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR1);
            tr2Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR2);
            tr3Utilities.PopulateDestinationSavegames(lstDestinationSavegamesTR3);

            isSyncingPlatformComboBoxes = false;

            UpdateExtractButtonText();
            EnableButtonsConditionally();
        }

        private void cmbSourceFormatTR1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncSourcePlatforms((Platform)cmbSourceFormatTR1.SelectedItem);
        }

        private void cmbSourceFormatTR2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncSourcePlatforms((Platform)cmbSourceFormatTR2.SelectedItem);
        }

        private void cmbSourceFormatTR3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncSourcePlatforms((Platform)cmbSourceFormatTR3.SelectedItem);
        }

        private void cmbDestinationFormatTR1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncDestinationPlatforms((Platform)cmbDestinationFormatTR1.SelectedItem);
        }

        private void cmbDestinationFormatTR2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncDestinationPlatforms((Platform)cmbDestinationFormatTR2.SelectedItem);
        }

        private void cmbDestinationFormatTR3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncDestinationPlatforms((Platform)cmbDestinationFormatTR3.SelectedItem);
        }
    }
}