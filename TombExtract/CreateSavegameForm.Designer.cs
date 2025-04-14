﻿namespace TombExtract
{
    partial class CreateSavegameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateSavegameForm));
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblSaveNumber = new System.Windows.Forms.Label();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.nudSaveNumber = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.cmbPlatform = new System.Windows.Forms.ComboBox();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.lblSeparator = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSaveNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(16, 17);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(36, 13);
            this.lblLevel.TabIndex = 0;
            this.lblLevel.Text = "Level:";
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(16, 43);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(37, 13);
            this.lblMode.TabIndex = 1;
            this.lblMode.Text = "Mode:";
            // 
            // lblSaveNumber
            // 
            this.lblSaveNumber.AutoSize = true;
            this.lblSaveNumber.Location = new System.Drawing.Point(16, 95);
            this.lblSaveNumber.Name = "lblSaveNumber";
            this.lblSaveNumber.Size = new System.Drawing.Size(75, 13);
            this.lblSaveNumber.TabIndex = 2;
            this.lblSaveNumber.Text = "Save Number:";
            // 
            // cmbLevel
            // 
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Location = new System.Drawing.Point(175, 17);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(183, 21);
            this.cmbLevel.TabIndex = 3;
            this.cmbLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLevel_SelectedIndexChanged);
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Items.AddRange(new object[] {
            "New Game",
            "New Game+"});
            this.cmbMode.Location = new System.Drawing.Point(250, 43);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(108, 21);
            this.cmbMode.TabIndex = 4;
            this.cmbMode.SelectedIndexChanged += new System.EventHandler(this.cmbMode_SelectedIndexChanged);
            // 
            // nudSaveNumber
            // 
            this.nudSaveNumber.Location = new System.Drawing.Point(305, 95);
            this.nudSaveNumber.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudSaveNumber.Name = "nudSaveNumber";
            this.nudSaveNumber.Size = new System.Drawing.Size(53, 20);
            this.nudSaveNumber.TabIndex = 5;
            this.nudSaveNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(109, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(190, 143);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 17;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // cmbPlatform
            // 
            this.cmbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlatform.FormattingEnabled = true;
            this.cmbPlatform.Items.AddRange(new object[] {
            "PC",
            "PS4",
            "Nintendo Switch"});
            this.cmbPlatform.Location = new System.Drawing.Point(250, 69);
            this.cmbPlatform.Name = "cmbPlatform";
            this.cmbPlatform.Size = new System.Drawing.Size(108, 21);
            this.cmbPlatform.TabIndex = 18;
            // 
            // lblPlatform
            // 
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point(16, 69);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(48, 13);
            this.lblPlatform.TabIndex = 19;
            this.lblPlatform.Text = "Platform:";
            // 
            // lblSeparator
            // 
            this.lblSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator.Location = new System.Drawing.Point(16, 132);
            this.lblSeparator.Name = "lblSeparator";
            this.lblSeparator.Size = new System.Drawing.Size(344, 2);
            this.lblSeparator.TabIndex = 20;
            // 
            // CreateSavegameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 179);
            this.Controls.Add(this.lblSeparator);
            this.Controls.Add(this.cmbLevel);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.cmbMode);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lblSaveNumber);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.cmbPlatform);
            this.Controls.Add(this.nudSaveNumber);
            this.Controls.Add(this.lblMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateSavegameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Savegame";
            ((System.ComponentModel.ISupportInitialize)(this.nudSaveNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblSaveNumber;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.NumericUpDown nudSaveNumber;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.ComboBox cmbPlatform;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.Label lblSeparator;
    }
}