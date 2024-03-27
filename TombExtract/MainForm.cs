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

        // Tabs
        private const int TAB_TR1 = 0;
        private const int TAB_TR2 = 1;
        private const int TAB_TR3 = 2;

        // Progress form
        private static ProgressForm progressForm;

        // Savegame paths
        private string savegameDestinationPath;
        private string savegameSourcePath;

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbConversionTR1.SelectedIndex = 0;
            cmbConversionTR2.SelectedIndex = 0;
            cmbConversionTR3.SelectedIndex = 0;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TR1.IsWriting() || TR2.IsWriting() || TR3.IsWriting())
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

        private void CreateBackup()
        {
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

        private void DisableButtons()
        {
            btnExtractTR1.Enabled = false;
            btnExtractTR2.Enabled = false;
            btnExtractTR3.Enabled = false;

            btnSelectAllTR1.Enabled = false;
            btnSelectAllTR2.Enabled = false;
            btnSelectAllTR3.Enabled = false;

            cklSourceSavegamesTR1.Enabled = false;
            cklSourceSavegamesTR2.Enabled = false;
            cklSourceSavegamesTR3.Enabled = false;

            cmbConversionTR1.Enabled = false;
            cmbConversionTR2.Enabled = false;
            cmbConversionTR3.Enabled = false;

            btnManageSlotsTR1.Enabled = false;
            btnManageSlotsTR2.Enabled = false;
            btnManageSlotsTR3.Enabled = false;

            btnBrowseDestinationFile.Enabled = false;
            btnBrowseSourceFile.Enabled = false;

            tsmiBrowseSourceFile.Enabled = false;
            tsmiBrowseDestinationFile.Enabled = false;
            tsmiExtract.Enabled = false;
            tsmiBackupDestinationFile.Enabled = false;

            chkBackupOnWrite.Enabled = false;
        }

        private void EnableButtonsConditionally()
        {
            bool isDestinationFilePresent = !string.IsNullOrEmpty(savegameDestinationPath) && File.Exists(savegameDestinationPath);

            btnExtractTR1.Enabled = cklSourceSavegamesTR1.Items.Count > 0 && isDestinationFilePresent;
            btnExtractTR2.Enabled = cklSourceSavegamesTR2.Items.Count > 0 && isDestinationFilePresent;
            btnExtractTR3.Enabled = cklSourceSavegamesTR3.Items.Count > 0 && isDestinationFilePresent;

            btnSelectAllTR1.Enabled = cklSourceSavegamesTR1.Items.Count > 0 && isDestinationFilePresent;
            btnSelectAllTR2.Enabled = cklSourceSavegamesTR2.Items.Count > 0 && isDestinationFilePresent;
            btnSelectAllTR3.Enabled = cklSourceSavegamesTR3.Items.Count > 0 && isDestinationFilePresent;

            tsmiExtract.Enabled = ((cklSourceSavegamesTR1.Items.Count > 0 ||
                                    cklSourceSavegamesTR1.Items.Count > 0 ||
                                    cklSourceSavegamesTR1.Items.Count > 0) && isDestinationFilePresent);

            chkBackupOnWrite.Enabled = isDestinationFilePresent;
            tsmiBackupDestinationFile.Enabled = isDestinationFilePresent;

            btnManageSlotsTR1.Enabled = isDestinationFilePresent;
            btnManageSlotsTR2.Enabled = isDestinationFilePresent;
            btnManageSlotsTR3.Enabled = isDestinationFilePresent;
        }

        private bool IsValidSavegame(string path)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Extension.ToLower() != ".dat")
            {
                return false;
            }

            return fileInfo.Length >= 0x152004;
        }

        private void SetSourceFile(string path)
        {
            if (!IsValidSavegame(path))
            {
                MessageBox.Show("Invalid savegame file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtSourceFilePath.Text = path;
            savegameSourcePath = path;

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

        private void SetDestinationFile(string path)
        {
            if (!IsValidSavegame(path))
            {
                MessageBox.Show("Invalid savegame file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtDestinationFilePath.Text = path;
            savegameDestinationPath = path;

            TR1.SetSavegameDestinationPath(path);
            TR1.PopulateDestinationSavegames(lstDestinationSavegamesTR1);

            TR2.SetSavegameDestinationPath(path);
            TR2.PopulateDestinationSavegames(lstDestinationSavegamesTR2);

            TR3.SetSavegameDestinationPath(path);
            TR3.PopulateDestinationSavegames(lstDestinationSavegamesTR3);

            EnableButtonsConditionally();

            slblStatus.Text = $"Opened destination file: \"{path}\"";
        }

        private void cklSourceSavegamesTR1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                SetSourceFile(filePath);
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

                SetDestinationFile(filePath);
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

                SetSourceFile(filePath);
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

                SetDestinationFile(filePath);
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

                SetSourceFile(filePath);
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

                SetDestinationFile(filePath);
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

        private void BrowseSourceFile()
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = "C:\\";
                fileBrowserDialog.Title = "Select source savegame file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    SetSourceFile(fileBrowserDialog.FileName);
                }
            }
        }

        private void BrowseDestinationFile()
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = "C:\\";
                fileBrowserDialog.Title = "Select destination savegame file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    SetDestinationFile(fileBrowserDialog.FileName);
                }
            }
        }

        private void btnBrowseSourceFile_Click(object sender, EventArgs e)
        {
            BrowseSourceFile();
        }

        private void btnBrowseDestinationFile_Click(object sender, EventArgs e)
        {
            BrowseDestinationFile();
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

            if (!File.Exists(savegameSourcePath))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPath))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR1.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegames. Are you sure you wish to proceed?",
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

            DisableButtons();

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.Show();

            TR1.SetProgressForm(progressForm);

            TR1.WriteSavegamesToDestination(selectedSavegames, cklSourceSavegamesTR1, cklSourceSavegamesTR2, cklSourceSavegamesTR3,
                btnExtractTR1, btnExtractTR2, btnExtractTR3, btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3, btnBrowseSourceFile,
                btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR1, tsmiBrowseSourceFile, tsmiBrowseDestinationFile,
                slblStatus, tsmiExtract, cmbConversionTR1, cmbConversionTR2, cmbConversionTR3, btnManageSlotsTR1, btnManageSlotsTR2,
                btnManageSlotsTR3, tsmiBackupDestinationFile);
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

            if (!File.Exists(savegameSourcePath))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPath))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR2.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegames. Are you sure you wish to proceed?",
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

            DisableButtons();

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.Show();

            TR2.SetProgressForm(progressForm);

            TR2.WriteSavegamesToDestination(selectedSavegames, cklSourceSavegamesTR1, cklSourceSavegamesTR2, cklSourceSavegamesTR3,
                btnExtractTR1, btnExtractTR2, btnExtractTR3, btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3, btnBrowseSourceFile,
                btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR2, tsmiBrowseSourceFile, tsmiBrowseDestinationFile,
                slblStatus, tsmiExtract, cmbConversionTR1, cmbConversionTR2, cmbConversionTR3, btnManageSlotsTR1, btnManageSlotsTR2,
                btnManageSlotsTR3, tsmiBackupDestinationFile);
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

            if (!File.Exists(savegameSourcePath))
            {
                MessageBox.Show("Could not find savegame source file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(savegameDestinationPath))
            {
                MessageBox.Show("Could not find savegame destination file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numOverwrites = TR3.GetNumOverwrites(selectedSavegames);

            if (numOverwrites > 0)
            {
                DialogResult result = MessageBox.Show($"This will overwrite {numOverwrites} savegames. Are you sure you wish to proceed?",
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

            DisableButtons();

            progressForm = new ProgressForm();
            progressForm.Owner = this;
            progressForm.Show();

            TR3.SetProgressForm(progressForm);

            TR3.WriteSavegamesToDestination(selectedSavegames, cklSourceSavegamesTR1, cklSourceSavegamesTR2, cklSourceSavegamesTR3,
                btnExtractTR1, btnExtractTR2, btnExtractTR3, btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3, btnBrowseSourceFile,
                btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR3, tsmiBrowseSourceFile, tsmiBrowseDestinationFile,
                slblStatus, tsmiExtract, cmbConversionTR1, cmbConversionTR2, cmbConversionTR3, btnManageSlotsTR1, btnManageSlotsTR2,
                btnManageSlotsTR3, tsmiBackupDestinationFile);
        }

        private void btnExtractTR1_Click(object sender, EventArgs e)
        {
            ExtractSavegamesTR1();
        }

        private void btnExtractTR2_Click(object sender, EventArgs e)
        {
            ExtractSavegamesTR2();
        }

        private void btnExtractTR3_Click(object sender, EventArgs e)
        {
            ExtractSavegamesTR3();
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

        private void tsmiBrowseSourceFile_Click(object sender, EventArgs e)
        {
            BrowseSourceFile();
        }

        private void tsmiBrowseDestinationFile_Click(object sender, EventArgs e)
        {
            BrowseDestinationFile();
        }

        private void tsmiBackupDestinationFile_Click(object sender, EventArgs e)
        {
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

        private void tsmiSendFeedback_Click(object sender, EventArgs e)
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
            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPath, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.ShowDialog();

            TR1.PopulateDestinationSavegames(lstDestinationSavegamesTR1);
        }

        private void btnManageSlotsTR2_Click(object sender, EventArgs e)
        {
            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPath, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.ShowDialog();

            TR2.PopulateDestinationSavegames(lstDestinationSavegamesTR2);
        }

        private void btnManageSlotsTR3_Click(object sender, EventArgs e)
        {
            ManageSlotsForm manageSlotsForm = new ManageSlotsForm(savegameDestinationPath, tabGame.SelectedIndex,
                slblStatus, chkBackupOnWrite.Checked);

            manageSlotsForm.ShowDialog();

            TR3.PopulateDestinationSavegames(lstDestinationSavegamesTR3);
        }
    }
}
