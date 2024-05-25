
namespace TombExtract
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblVersionNumber = new System.Windows.Forms.Label();
            this.lblAuthorName = new System.Windows.Forms.Label();
            this.lblMore = new System.Windows.Forms.Label();
            this.llbGitHub = new System.Windows.Forms.LinkLabel();
            this.picTombExtract = new System.Windows.Forms.PictureBox();
            this.grpInfo = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTombExtract)).BeginInit();
            this.grpInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(134, 349);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(17, 68);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(57, 16);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version:";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.Location = new System.Drawing.Point(17, 33);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(49, 16);
            this.lblAuthor.TabIndex = 3;
            this.lblAuthor.Text = "Author:";
            // 
            // lblVersionNumber
            // 
            this.lblVersionNumber.AutoSize = true;
            this.lblVersionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionNumber.Location = new System.Drawing.Point(277, 68);
            this.lblVersionNumber.Name = "lblVersionNumber";
            this.lblVersionNumber.Size = new System.Drawing.Size(39, 16);
            this.lblVersionNumber.TabIndex = 4;
            this.lblVersionNumber.Text = "v2.02";
            // 
            // lblAuthorName
            // 
            this.lblAuthorName.AutoSize = true;
            this.lblAuthorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthorName.Location = new System.Drawing.Point(207, 33);
            this.lblAuthorName.Name = "lblAuthorName";
            this.lblAuthorName.Size = new System.Drawing.Size(109, 16);
            this.lblAuthorName.TabIndex = 5;
            this.lblAuthorName.Text = "Julian Ozel Rose";
            // 
            // lblMore
            // 
            this.lblMore.AutoSize = true;
            this.lblMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMore.Location = new System.Drawing.Point(17, 103);
            this.lblMore.Name = "lblMore";
            this.lblMore.Size = new System.Drawing.Size(42, 16);
            this.lblMore.TabIndex = 6;
            this.lblMore.Text = "More:";
            // 
            // llbGitHub
            // 
            this.llbGitHub.AutoSize = true;
            this.llbGitHub.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llbGitHub.Location = new System.Drawing.Point(267, 103);
            this.llbGitHub.Name = "llbGitHub";
            this.llbGitHub.Size = new System.Drawing.Size(49, 16);
            this.llbGitHub.TabIndex = 7;
            this.llbGitHub.TabStop = true;
            this.llbGitHub.Text = "GitHub";
            this.llbGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbGitHub_LinkClicked);
            this.llbGitHub.MouseHover += new System.EventHandler(this.llbGitHub_MouseHover);
            // 
            // picTombExtract
            // 
            this.picTombExtract.Image = global::TombExtract.Properties.Resources.AboutImage;
            this.picTombExtract.Location = new System.Drawing.Point(12, 12);
            this.picTombExtract.Name = "picTombExtract";
            this.picTombExtract.Size = new System.Drawing.Size(332, 176);
            this.picTombExtract.TabIndex = 0;
            this.picTombExtract.TabStop = false;
            // 
            // grpInfo
            // 
            this.grpInfo.Controls.Add(this.lblAuthor);
            this.grpInfo.Controls.Add(this.llbGitHub);
            this.grpInfo.Controls.Add(this.lblVersion);
            this.grpInfo.Controls.Add(this.lblVersionNumber);
            this.grpInfo.Controls.Add(this.lblAuthorName);
            this.grpInfo.Controls.Add(this.lblMore);
            this.grpInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpInfo.Location = new System.Drawing.Point(12, 190);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(332, 150);
            this.grpInfo.TabIndex = 8;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "Info";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 390);
            this.Controls.Add(this.grpInfo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.picTombExtract);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.picTombExtract)).EndInit();
            this.grpInfo.ResumeLayout(false);
            this.grpInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picTombExtract;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblVersionNumber;
        private System.Windows.Forms.Label lblAuthorName;
        private System.Windows.Forms.Label lblMore;
        private System.Windows.Forms.LinkLabel llbGitHub;
        private System.Windows.Forms.GroupBox grpInfo;
    }
}