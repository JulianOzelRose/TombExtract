using System;
using System.Windows.Forms;

namespace TombExtract
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void llbGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/JulianOzelRose");
        }

        private void llbGitHub_MouseHover(object sender, EventArgs e)
        {
            if (sender is LinkLabel linkLabel)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.InitialDelay = 500;
                toolTip.SetToolTip(linkLabel, "https://github.com/JulianOzelRose");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
