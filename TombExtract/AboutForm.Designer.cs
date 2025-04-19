﻿
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
            this.lblAbout1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.llbGitHub = new System.Windows.Forms.LinkLabel();
            this.picTitleText = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblAbout2 = new System.Windows.Forms.Label();
            this.lblAbout3 = new System.Windows.Forms.Label();
            this.lblAbout4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picTitleText)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAbout1
            // 
            this.lblAbout1.AutoSize = true;
            this.lblAbout1.BackColor = System.Drawing.Color.Transparent;
            this.lblAbout1.Font = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout1.ForeColor = System.Drawing.Color.White;
            this.lblAbout1.Location = new System.Drawing.Point(12, 183);
            this.lblAbout1.Name = "lblAbout1";
            this.lblAbout1.Size = new System.Drawing.Size(117, 21);
            this.lblAbout1.TabIndex = 3;
            this.lblAbout1.Text = "Check out my";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(114, 66);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(118, 26);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version 4.00";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthor.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.ForeColor = System.Drawing.Color.White;
            this.lblAuthor.Location = new System.Drawing.Point(89, 101);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(171, 26);
            this.lblAuthor.TabIndex = 5;
            this.lblAuthor.Text = "by Julian Ozel Rose";
            // 
            // llbGitHub
            // 
            this.llbGitHub.AutoSize = true;
            this.llbGitHub.BackColor = System.Drawing.Color.Transparent;
            this.llbGitHub.Font = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llbGitHub.LinkColor = System.Drawing.SystemColors.MenuHighlight;
            this.llbGitHub.Location = new System.Drawing.Point(128, 183);
            this.llbGitHub.Name = "llbGitHub";
            this.llbGitHub.Size = new System.Drawing.Size(65, 21);
            this.llbGitHub.TabIndex = 7;
            this.llbGitHub.TabStop = true;
            this.llbGitHub.Text = "GitHub";
            this.llbGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbGitHub_LinkClicked);
            this.llbGitHub.MouseHover += new System.EventHandler(this.llbGitHub_MouseHover);
            // 
            // picTitleText
            // 
            this.picTitleText.BackColor = System.Drawing.Color.Transparent;
            this.picTitleText.Image = global::TombExtract.Properties.Resources.AboutImage;
            this.picTitleText.Location = new System.Drawing.Point(12, 12);
            this.picTitleText.Name = "picTitleText";
            this.picTitleText.Size = new System.Drawing.Size(328, 41);
            this.picTitleText.TabIndex = 0;
            this.picTitleText.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.BorderSize = 2;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Impact", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.Yellow;
            this.btnOK.Location = new System.Drawing.Point(106, 327);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(136, 39);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblAbout2
            // 
            this.lblAbout2.AutoSize = true;
            this.lblAbout2.BackColor = System.Drawing.Color.Transparent;
            this.lblAbout2.Font = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout2.ForeColor = System.Drawing.Color.White;
            this.lblAbout2.Location = new System.Drawing.Point(191, 183);
            this.lblAbout2.Name = "lblAbout2";
            this.lblAbout2.Size = new System.Drawing.Size(110, 21);
            this.lblAbout2.TabIndex = 25;
            this.lblAbout2.Text = "for the latest";
            // 
            // lblAbout3
            // 
            this.lblAbout3.AutoSize = true;
            this.lblAbout3.BackColor = System.Drawing.Color.Transparent;
            this.lblAbout3.Font = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout3.ForeColor = System.Drawing.Color.White;
            this.lblAbout3.Location = new System.Drawing.Point(12, 213);
            this.lblAbout3.Name = "lblAbout3";
            this.lblAbout3.Size = new System.Drawing.Size(329, 21);
            this.lblAbout3.TabIndex = 26;
            this.lblAbout3.Text = "version and for more reverse engineering";
            // 
            // lblAbout4
            // 
            this.lblAbout4.AutoSize = true;
            this.lblAbout4.BackColor = System.Drawing.Color.Transparent;
            this.lblAbout4.Font = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout4.ForeColor = System.Drawing.Color.White;
            this.lblAbout4.Location = new System.Drawing.Point(12, 245);
            this.lblAbout4.Name = "lblAbout4";
            this.lblAbout4.Size = new System.Drawing.Size(180, 21);
            this.lblAbout4.TabIndex = 27;
            this.lblAbout4.Text = "writeups and content.";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::TombExtract.Properties.Resources.AboutBackground;
            this.ClientSize = new System.Drawing.Size(357, 390);
            this.Controls.Add(this.lblAbout4);
            this.Controls.Add(this.lblAbout3);
            this.Controls.Add(this.lblAbout2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.llbGitHub);
            this.Controls.Add(this.lblAbout1);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.picTitleText);
            this.Controls.Add(this.lblAuthor);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.picTitleText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picTitleText;
        private System.Windows.Forms.Label lblAbout1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.LinkLabel llbGitHub;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblAbout2;
        private System.Windows.Forms.Label lblAbout3;
        private System.Windows.Forms.Label lblAbout4;
    }
}