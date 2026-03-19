using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TombExtract
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // Utils
        private readonly TR1Utilities TR1 = new TR1Utilities();
        private readonly TR2Utilities TR2 = new TR2Utilities();
        private readonly TR3Utilities TR3 = new TR3Utilities();
        private readonly TR4Utilities TR4 = new TR4Utilities();
        private readonly TR5Utilities TR5 = new TR5Utilities();
        private readonly TR6Utilities TR6 = new TR6Utilities();

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
        private const int SAVEGAME_FILE_SIZE_TRX_PREPATCH = 0x152004;
        private const int SAVEGAME_FILE_SIZE_TRX_PATCH5 = 0x272004;
        private const int SAVEGAME_FILE_SIZE_TRX2 = 0x3DCA04;
        private const int SAVEGAME_VERSION_OFFSET = 0x000;
        private const byte TRX_PREPATCH_SIGNATURE = 0x3B;
        private const byte TRX_PATCH5_SIGNATURE = 0x3C;
        private const byte TRX2_SAVEGAME_SIGNATURE = 0x28;

        // Config
        private const string CONFIG_FILE_NAME = "TombExtract.ini";

        private void MainForm_Load(object sender, EventArgs e)
        {
            ReadConfigFile();

            cmbConversionTR1.SelectedIndex = 0;
            cmbConversionTR2.SelectedIndex = 0;
            cmbConversionTR3.SelectedIndex = 0;
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
                DialogResult result = MessageBox.Show(
                    "Exiting in the middle of a write operation could result in a corrupted savegame file. Are you sure you wish to exit?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            UpdateConfigFile();
        }

        private bool IsAnyWriting()
        {
            return TR1.IsWriting() || TR2.IsWriting() || TR3.IsWriting() || TR4.IsWriting() || TR5.IsWriting() || TR6.IsWriting();
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

            // Enable backup options only if a destination file exists
            chkBackupOnWrite.Enabled = isDestinationFilePresent;
            tsmiBackupDestinationFile.Enabled = isDestinationFilePresent;
        }

        private bool IsValidSavegameTRX(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            byte[] fileData = File.ReadAllBytes(path);

            long savegameFileSize = fileInfo.Length;
            byte savegameVersion = GetSavegameVersion(fileData);

            if ((savegameVersion == TRX_PREPATCH_SIGNATURE || savegameVersion == TRX_PATCH5_SIGNATURE)
                && (savegameFileSize >= SAVEGAME_FILE_SIZE_TRX_PREPATCH || savegameFileSize >= SAVEGAME_FILE_SIZE_TRX_PATCH5))
            {
                return true;
            }

            return false;
        }

        private bool IsValidSavegameTRX2(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            byte[] fileData = File.ReadAllBytes(path);

            long savegameFileSize = fileInfo.Length;
            byte savegameVersion = GetSavegameVersion(fileData);

            if (savegameVersion == TRX2_SAVEGAME_SIGNATURE && savegameFileSize >= SAVEGAME_FILE_SIZE_TRX2)
            {
                return true;
            }

            return false;
        }

        public byte GetSavegameVersion(byte[] fileData)
        {
            return fileData[SAVEGAME_VERSION_OFFSET];
        }

        public bool IsPatch5Savegame(byte[] fileData)
        {
            return fileData[SAVEGAME_VERSION_OFFSET] == TRX_PATCH5_SIGNATURE;
        }

        private void SetSourceFileTRX(string path)
        {
            if (!IsValidSavegameTRX(path))
            {
                MessageBox.Show("Not a valid Tomb Raider I-III Remastered savegame file.", "Invalid Savegame File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtSourceFilePath.Text = path;
            savegameSourcePathTRX = path;

            TR1.SetSavegameSourcePath(path);
            TR1.PopulateSourceSavegames(cklSourceSavegamesTR1);

            TR2.SetSavegameSourcePath(path);
            TR2.PopulateSourceSavegames(cklSourceSavegamesTR2);

            TR3.SetSavegameSourcePath(path);
            TR3.PopulateSourceSavegames(cklSourceSavegamesTR3);

            EnableButtonsConditionally();

            int numSaves = cklSourceSavegamesTR1.Items.Count +
                           cklSourceSavegamesTR2.Items.Count +
                           cklSourceSavegamesTR3.Items.Count;

            byte[] fileData = File.ReadAllBytes(path);
            bool isPatch5 = IsPatch5Savegame(fileData);

            if (isPatch5)
            {
                cmbConversionTR1.SelectedIndex = 0;
                cmbConversionTR2.SelectedIndex = 0;
                cmbConversionTR3.SelectedIndex = 0;

                cmbConversionTR1.Enabled = false;
                cmbConversionTR2.Enabled = false;
                cmbConversionTR3.Enabled = false;
            }

            slblStatus.Text = $"{numSaves} savegame(s) found in \"{path}\"";
        }

        private void SetSourceFileTRX2(string path)
        {
            if (!IsValidSavegameTRX2(path))
            {
                MessageBox.Show("Not a valid Tomb Raider IV-VI Remastered savegame file.", "Invalid Savegame File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtSourceFilePath.Text = path;
            savegameSourcePathTRX2 = path;

            TR4.SetSavegameSourcePath(path);
            TR4.PopulateSourceSavegames(cklSourceSavegamesTR4);

            TR5.SetSavegameSourcePath(path);
            TR5.PopulateSourceSavegames(cklSourceSavegamesTR5);

            TR6.SetSavegameSourcePath(path);
            TR6.PopulateSourceSavegames(cklSourceSavegamesTR6);

            EnableButtonsConditionally();

            int numSaves = cklSourceSavegamesTR4.Items.Count +
                           cklSourceSavegamesTR5.Items.Count +
                           cklSourceSavegamesTR6.Items.Count;

            slblStatus.Text = $"{numSaves} savegame(s) found in \"{path}\"";
        }

        private void SetDestinationFileTRX(string path)
        {
            if (!IsValidSavegameTRX(path))
            {
                MessageBox.Show("Not a valid Tomb Raider I-III Remastered savegame file.", "Invalid Savegame File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtDestinationFilePath.Text = path;
            savegameDestinationPathTRX = path;

            TR1.SetSavegameDestinationPath(path);
            TR1.PopulateDestinationSavegames(lstDestinationSavegamesTR1);

            TR2.SetSavegameDestinationPath(path);
            TR2.PopulateDestinationSavegames(lstDestinationSavegamesTR2);

            TR3.SetSavegameDestinationPath(path);
            TR3.PopulateDestinationSavegames(lstDestinationSavegamesTR3);

            EnableButtonsConditionally();

            byte[] fileData = File.ReadAllBytes(path);
            bool isPatch5 = IsPatch5Savegame(fileData);

            if (isPatch5)
            {
                cmbConversionTR1.SelectedIndex = 0;
                cmbConversionTR2.SelectedIndex = 0;
                cmbConversionTR3.SelectedIndex = 0;

                cmbConversionTR1.Enabled = false;
                cmbConversionTR2.Enabled = false;
                cmbConversionTR3.Enabled = false;
            }

            slblStatus.Text = $"Opened destination file: \"{path}\"";
        }

        private void SetDestinationFileTRX2(string path)
        {
            if (!IsValidSavegameTRX2(path))
            {
                MessageBox.Show("Not a valid Tomb Raider IV-VI Remastered savegame file.", "Invalid Savegame File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtDestinationFilePath.Text = path;
            savegameDestinationPathTRX2 = path;

            TR4.SetSavegameDestinationPath(path);
            TR4.PopulateDestinationSavegames(lstDestinationSavegamesTR4);

            TR5.SetSavegameDestinationPath(path);
            TR5.PopulateDestinationSavegames(lstDestinationSavegamesTR5);

            TR6.SetSavegameDestinationPath(path);
            TR6.PopulateDestinationSavegames(lstDestinationSavegamesTR6);

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
                fileBrowserDialog.InitialDirectory = "C:\\";
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
                fileBrowserDialog.InitialDirectory = "C:\\";
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
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
            Application.Exit();
        }

        private void btnExitTR2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExitTR3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExitTR4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExitTR5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExitTR6_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                MessageBox.Show("Please select at least one savegame to convert.", "No Savegames Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                MessageBox.Show("Could not find savegame source file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                MessageBox.Show("Could not find savegame destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR1.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

            TR1.SetProgressForm(progressForm);
            TR1.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR1, slblStatus, cmbConversionTR1);
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
                MessageBox.Show("Please select at least one savegame to convert.", "No Savegames Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                MessageBox.Show("Could not find savegame source file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                MessageBox.Show("Could not find savegame destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR2.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

            TR2.SetProgressForm(progressForm);
            TR2.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR2, slblStatus, cmbConversionTR2);
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
                MessageBox.Show("Please select at least one savegame to convert.", "No Savegames Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                MessageBox.Show("Could not find savegame source file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                MessageBox.Show("Could not find savegame destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR3.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

            TR3.SetProgressForm(progressForm);
            TR3.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR3, slblStatus, cmbConversionTR3);
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
                MessageBox.Show("Please select at least one savegame to convert.", "No Savegames Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                MessageBox.Show("Could not find savegame source file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                MessageBox.Show("Could not find savegame destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR4.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

            TR4.SetProgressForm(progressForm);
            TR4.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR4, slblStatus);
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
                MessageBox.Show("Please select at least one savegame to convert.", "No Savegames Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                MessageBox.Show("Could not find savegame source file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                MessageBox.Show("Could not find savegame destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR5.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

            TR5.SetProgressForm(progressForm);
            TR5.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR5, slblStatus);
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
                MessageBox.Show("Please select at least one savegame to convert.", "No Savegames Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                MessageBox.Show("Could not find savegame source file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                MessageBox.Show("Could not find savegame destination file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR6.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegame(s). Are you sure you wish to proceed?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

            TR6.SetProgressForm(progressForm);
            TR6.WriteSavegamesToDestination(selectedSavegames, lstDestinationSavegamesTR6, slblStatus);
        }

        private void btnExtractTR1_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR1();
        }

        private void btnExtractTR2_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR2();
        }

        private void btnExtractTR3_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR3();
        }

        private void btnExtractTR4_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR4();
        }

        private void btnExtractTR5_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ExtractSavegamesTR5();
        }

        private void btnExtractTR6_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            BrowseSourceFileTRX();
        }

        private void tsmiBrowseTRXDestinationFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            BrowseDestinationFileTRX();
        }

        private void tsmiBrowseTRX2SourceFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            BrowseSourceFileTRX2();
        }

        private void tsmiBrowseTRX2DestinationFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            BrowseDestinationFileTRX2();
        }

        private void tsmiBackupDestinationFile_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            CreateBackup();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

        private void cmbConversionTR1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbConversionTR2.SelectedIndex = cmbConversionTR1.SelectedIndex;
            cmbConversionTR3.SelectedIndex = cmbConversionTR1.SelectedIndex;

            if (cmbConversionTR1.SelectedIndex == 0)
            {
                btnExtractTR1.Text = "Extract";
                btnExtractTR2.Text = "Extract";
                btnExtractTR3.Text = "Extract";
                tsmiExtract.Text = "Extract";
            }
            else
            {
                btnExtractTR1.Text = "Convert";
                btnExtractTR2.Text = "Convert";
                btnExtractTR3.Text = "Convert";
                tsmiExtract.Text = "Convert";
            }
        }

        private void cmbConversionTR2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbConversionTR1.SelectedIndex = cmbConversionTR2.SelectedIndex;
            cmbConversionTR3.SelectedIndex = cmbConversionTR2.SelectedIndex;

            if (cmbConversionTR2.SelectedIndex == 0)
            {
                btnExtractTR1.Text = "Extract";
                btnExtractTR2.Text = "Extract";
                btnExtractTR3.Text = "Extract";
                tsmiExtract.Text = "Extract";
            }
            else
            {
                btnExtractTR1.Text = "Convert";
                btnExtractTR2.Text = "Convert";
                btnExtractTR3.Text = "Convert";
                tsmiExtract.Text = "Convert";
            }
        }

        private void cmbConversionTR3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbConversionTR1.SelectedIndex = cmbConversionTR3.SelectedIndex;
            cmbConversionTR2.SelectedIndex = cmbConversionTR3.SelectedIndex;

            if (cmbConversionTR3.SelectedIndex == 0)
            {
                btnExtractTR1.Text = "Extract";
                btnExtractTR2.Text = "Extract";
                btnExtractTR3.Text = "Extract";
                tsmiExtract.Text = "Extract";
            }
            else
            {
                btnExtractTR1.Text = "Convert";
                btnExtractTR2.Text = "Convert";
                btnExtractTR3.Text = "Convert";
                tsmiExtract.Text = "Convert";
            }
        }

        private void btnManageSlotsTR1_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            TR1.PopulateDestinationSavegames(lstDestinationSavegamesTR1);
        }

        private void btnManageSlotsTR2_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            TR2.PopulateDestinationSavegames(lstDestinationSavegamesTR2);
        }

        private void btnManageSlotsTR3_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            TR3.PopulateDestinationSavegames(lstDestinationSavegamesTR3);
        }

        private void btnManageSlotsTR4_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX2, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            TR4.PopulateDestinationSavegames(lstDestinationSavegamesTR4);
        }

        private void btnManageSlotsTR5_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX2, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            TR5.PopulateDestinationSavegames(lstDestinationSavegamesTR5);
        }

        private void btnManageSlotsTR6_Click(object sender, EventArgs e)
        {
            if (IsAnyWriting())
            {
                MessageBox.Show("A savegame write operation is in progress. Please wait until it completes.",
                    "Write In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPathTRX2, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.TopMost = TopMost;
            manageSlotsForm.ShowDialog();

            TR6.PopulateDestinationSavegames(lstDestinationSavegamesTR6);
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
    }
}