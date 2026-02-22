using System;
using System.Windows.Forms;

namespace TombExtract
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            if (ThemeUtilities.DARK_MODE_ENABLED)
            {
                ThemeUtilities.ApplyDarkMode(this);
                ThemeUtilities.ApplyDarkTitleBar(this);
            }
        }

        public void UpdateProgressBar(int newValue)
        {
            prgOverall.Value = newValue;
        }

        public void UpdateStatusMessage(string status)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action<string>(UpdateStatusMessage), status);
            }
            else
            {
                lblStatus.Text = status;
            }
        }

        public void UpdatePercentage(int newPercentage)
        {
            if (lblPercentage.InvokeRequired)
            {
                lblPercentage.Invoke((MethodInvoker)(() =>
                {
                    lblPercentage.Text = $"{newPercentage}%";
                }));
            }
            else
            {
                lblPercentage.Text = $"{newPercentage}%";
            }
        }
    }
}
