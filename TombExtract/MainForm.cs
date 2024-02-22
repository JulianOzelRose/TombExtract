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

        // Progress form
        private static ProgressForm progressForm;

        // Destination path
        private string savegameDestinationPath;

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

            grpConvertTR1.Enabled = false;
            grpConvertTR2.Enabled = false;
            grpConvertTR3.Enabled = false;

            btnBrowseDestinationFile.Enabled = false;
            btnBrowseSourceFile.Enabled = false;

            chkBackupOnWrite.Enabled = false;
        }

        private void btnBrowseSourceFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = "C:\\";
                fileBrowserDialog.Title = "Select source savegame file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourceFilePath.Text = fileBrowserDialog.FileName;

                    TR1.SetSavegameSourcePath(fileBrowserDialog.FileName);
                    TR1.PopulateSourceSavegames(cklSourceSavegamesTR1);

                    TR2.SetSavegameSourcePath(fileBrowserDialog.FileName);
                    TR2.PopulateSourceSavegames(cklSourceSavegamesTR2);

                    TR3.SetSavegameSourcePath(fileBrowserDialog.FileName);
                    TR3.PopulateSourceSavegames(cklSourceSavegamesTR3);
                }
            }

            btnExtractTR1.Enabled = cklSourceSavegamesTR1.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnExtractTR2.Enabled = cklSourceSavegamesTR2.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnExtractTR3.Enabled = cklSourceSavegamesTR3.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);

            btnSelectAllTR1.Enabled = cklSourceSavegamesTR1.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnSelectAllTR2.Enabled = cklSourceSavegamesTR2.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnSelectAllTR3.Enabled = cklSourceSavegamesTR3.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);

            chkBackupOnWrite.Enabled = !string.IsNullOrEmpty(txtSourceFilePath.Text) && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
        }

        private void btnBrowseDestinationFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileBrowserDialog = new OpenFileDialog())
            {
                fileBrowserDialog.InitialDirectory = "C:\\";
                fileBrowserDialog.Title = "Select destination savegame file";
                fileBrowserDialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

                if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDestinationFilePath.Text = fileBrowserDialog.FileName;
                    savegameDestinationPath = fileBrowserDialog.FileName;

                    TR1.SetSavegameDestinationPath(fileBrowserDialog.FileName);
                    TR1.PopulateDestinationSavegames(lstDestinationSavegamesTR1);

                    TR2.SetSavegameDestinationPath(fileBrowserDialog.FileName);
                    TR2.PopulateDestinationSavegames(lstDestinationSavegamesTR2);

                    TR3.SetSavegameDestinationPath(fileBrowserDialog.FileName);
                    TR3.PopulateDestinationSavegames(lstDestinationSavegamesTR3);
                }
            }

            btnExtractTR1.Enabled = cklSourceSavegamesTR1.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnExtractTR2.Enabled = cklSourceSavegamesTR2.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnExtractTR3.Enabled = cklSourceSavegamesTR3.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);

            btnSelectAllTR1.Enabled = cklSourceSavegamesTR1.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnSelectAllTR2.Enabled = cklSourceSavegamesTR2.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
            btnSelectAllTR3.Enabled = cklSourceSavegamesTR3.Items.Count > 0 && !string.IsNullOrEmpty(txtDestinationFilePath.Text);

            chkBackupOnWrite.Enabled = !string.IsNullOrEmpty(txtSourceFilePath.Text) && !string.IsNullOrEmpty(txtDestinationFilePath.Text);
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

        private void btnExtractTR1_Click(object sender, EventArgs e)
        {
            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

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
            }
            else
            {
                DisableButtons();

                progressForm = new ProgressForm();
                progressForm.Owner = this;
                progressForm.Show();

                TR1.SetProgressForm(progressForm);

                TR1.WriteSavegamesToDestination(selectedSavegames, rdoNoConvertTR1, rdoToPCTR1, rdoToPS4TR1,
                    cklSourceSavegamesTR1, cklSourceSavegamesTR2, cklSourceSavegamesTR3, btnExtractTR1, btnExtractTR2, btnExtractTR3,
                    btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3, grpConvertTR1, grpConvertTR2, grpConvertTR3, btnBrowseSourceFile,
                    btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR1);
            }
        }

        private void btnExtractTR2_Click(object sender, EventArgs e)
        {
            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

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
            }
            else
            {
                DisableButtons();

                progressForm = new ProgressForm();
                progressForm.Owner = this;
                progressForm.Show();

                TR2.SetProgressForm(progressForm);

                TR2.WriteSavegamesToDestination(selectedSavegames, rdoNoConvertTR2, rdoToPCTR2, rdoToPS4TR2,
                    cklSourceSavegamesTR1, cklSourceSavegamesTR2, cklSourceSavegamesTR3, btnExtractTR1, btnExtractTR2, btnExtractTR3,
                    btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3, grpConvertTR1, grpConvertTR2, grpConvertTR3, btnBrowseSourceFile,
                    btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR2);
            }
        }

        private void btnExtractTR3_Click(object sender, EventArgs e)
        {
            if (chkBackupOnWrite.Checked)
            {
                CreateBackup();
            }

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
            }
            else
            {
                DisableButtons();

                progressForm = new ProgressForm();
                progressForm.Owner = this;
                progressForm.Show();

                TR3.SetProgressForm(progressForm);

                TR3.WriteSavegamesToDestination(selectedSavegames, rdoNoConvertTR3, rdoToPCTR3, rdoToPS4TR3,
                    cklSourceSavegamesTR1, cklSourceSavegamesTR2, cklSourceSavegamesTR3, btnExtractTR1, btnExtractTR2, btnExtractTR3,
                    btnSelectAllTR1, btnSelectAllTR2, btnSelectAllTR3, grpConvertTR1, grpConvertTR2, grpConvertTR3, btnBrowseSourceFile,
                    btnBrowseDestinationFile, chkBackupOnWrite, lstDestinationSavegamesTR3);
            }
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

        private void rdoNoConvertTR1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR1.Checked)
            {
                btnExtractTR1.Text = "Extract";
            }
            else
            {
                btnExtractTR1.Text = "Convert";
            }

            rdoNoConvertTR2.Checked = rdoNoConvertTR1.Checked;
            rdoNoConvertTR3.Checked = rdoNoConvertTR1.Checked;
        }

        private void rdoToPCTR1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR1.Checked)
            {
                btnExtractTR1.Text = "Extract";
            }
            else
            {
                btnExtractTR1.Text = "Convert";
            }

            rdoToPCTR2.Checked = rdoToPCTR1.Checked;
            rdoToPCTR3.Checked = rdoToPCTR1.Checked;
        }

        private void rdoToPS4TR1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR1.Checked)
            {
                btnExtractTR1.Text = "Extract";
            }
            else
            {
                btnExtractTR1.Text = "Convert";
            }

            rdoToPS4TR2.Checked = rdoToPS4TR1.Checked;
            rdoToPS4TR3.Checked = rdoToPS4TR1.Checked;
        }

        private void rdoNoConvertTR2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR2.Checked)
            {
                btnExtractTR2.Text = "Extract";
            }
            else
            {
                btnExtractTR2.Text = "Convert";
            }

            rdoNoConvertTR1.Checked = rdoNoConvertTR2.Checked;
            rdoNoConvertTR3.Checked = rdoNoConvertTR2.Checked;
        }

        private void rdoToPCTR2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR2.Checked)
            {
                btnExtractTR2.Text = "Extract";
            }
            else
            {
                btnExtractTR2.Text = "Convert";
            }

            rdoToPCTR1.Checked = rdoToPCTR2.Checked;
            rdoToPCTR3.Checked = rdoToPCTR2.Checked;
        }

        private void rdoToPS4TR2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR2.Checked)
            {
                btnExtractTR2.Text = "Extract";
            }
            else
            {
                btnExtractTR2.Text = "Convert";
            }

            rdoToPS4TR1.Checked = rdoToPS4TR2.Checked;
            rdoToPS4TR3.Checked = rdoToPS4TR2.Checked;
        }

        private void rdoNoConvertTR3_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR3.Checked)
            {
                btnExtractTR3.Text = "Extract";
            }
            else
            {
                btnExtractTR3.Text = "Convert";
            }

            rdoNoConvertTR1.Checked = rdoNoConvertTR3.Checked;
            rdoNoConvertTR2.Checked = rdoNoConvertTR3.Checked;
        }

        private void rdoToPCTR3_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR3.Checked)
            {
                btnExtractTR3.Text = "Extract";
            }
            else
            {
                btnExtractTR3.Text = "Convert";
            }

            rdoToPCTR1.Checked = rdoToPCTR3.Checked;
            rdoToPCTR2.Checked = rdoToPCTR3.Checked;
        }

        private void rdoToPS4TR3_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoConvertTR3.Checked)
            {
                btnExtractTR3.Text = "Extract";
            }
            else
            {
                btnExtractTR3.Text = "Convert";
            }

            rdoToPS4TR1.Checked = rdoToPS4TR3.Checked;
            rdoToPS4TR2.Checked = rdoToPS4TR3.Checked;
        }
    }
}
