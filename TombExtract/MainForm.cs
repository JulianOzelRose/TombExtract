using System;
using System.Collections.Generic;
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
        readonly TR1Utilities TR1 = new TR1Utilities();
        readonly TR2Utilities TR2 = new TR2Utilities();
        readonly TR3Utilities TR3 = new TR3Utilities();
        readonly TR4Utilities TR4 = new TR4Utilities();
        readonly TR5Utilities TR5 = new TR5Utilities();
        readonly TR6Utilities TR6 = new TR6Utilities();

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

        // Savegame file sizes
        private const int SAVEGAME_FILE_SIZE_TRX = 0x152004;
        private const int SAVEGAME_FILE_SIZE_TRX2 = 0x3DCA04;

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbConversionTR1.SelectedIndex = 0;
            cmbConversionTR2.SelectedIndex = 0;
            cmbConversionTR3.SelectedIndex = 0;
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
        }

        private bool IsAnyWriting()
        {
            return TR1.IsWriting() || TR2.IsWriting() || TR3.IsWriting() || TR4.IsWriting() || TR5.IsWriting() || TR6.IsWriting();
        }

        private void CreateBackup()
        {
            string savegameDestinationPath = "";

            if (IsTRXTabSelected())
            {
                savegameDestinationPath = savegameDestinationPathTRX;
            }
            else
            {
                savegameDestinationPath = savegameDestinationPathTRX2;
            }

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

            if (fileInfo.Extension.ToLower() != ".dat")
            {
                return false;
            }

            return fileInfo.Length >= SAVEGAME_FILE_SIZE_TRX;
        }

        private bool IsValidSavegameTRX2(string path)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Extension.ToLower() != ".dat")
            {
                return false;
            }

            return fileInfo.Length >= SAVEGAME_FILE_SIZE_TRX2;
        }

        private void SetSourceFileTRX(string path)
        {
            if (!IsValidSavegameTRX(path))
            {
                MessageBox.Show("Invalid savegame file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            slblStatus.Text = $"{numSaves} savegames found in \"{path}\"";
        }

        private void SetSourceFileTRX2(string path)
        {
            if (!IsValidSavegameTRX2(path))
            {
                MessageBox.Show("Invalid savegame file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            slblStatus.Text = $"{numSaves} savegames found in \"{path}\"";
        }

        private void SetDestinationFileTRX(string path)
        {
            if (!IsValidSavegameTRX(path))
            {
                MessageBox.Show("Invalid savegame file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            slblStatus.Text = $"Opened destination file: \"{path}\"";
        }

        private void SetDestinationFileTRX2(string path)
        {
            if (!IsValidSavegameTRX2(path))
            {
                MessageBox.Show("Invalid savegame file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No savegames selected to convert!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No savegames selected to convert!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No savegames selected to convert!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No savegames selected to convert!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No savegames selected to convert!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No savegames selected to convert!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameSourcePathTRX2))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPathTRX2))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            aboutForm.ShowDialog();
        }

        private void btnAboutTR2_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void btnAboutTR3_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void btnAboutTR4_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void btnAboutTR5_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void btnAboutTR6_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
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

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
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

            manageSlotsForm.ShowDialog();

            TR6.PopulateDestinationSavegames(lstDestinationSavegamesTR6);
        }

        private bool IsTRXTabSelected()
        {
            return tabGame.SelectedIndex == TAB_TR1 || tabGame.SelectedIndex == TAB_TR2 || tabGame.SelectedIndex == TAB_TR3;
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