namespace KMS_Helper
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.NetworkListBox = new System.Windows.Forms.ListBox();
            this.CurrentWlanTextBox = new System.Windows.Forms.TextBox();
            this.SettingsPanel = new System.Windows.Forms.Panel();
            this.LoadingSpinnerPictureBox = new System.Windows.Forms.PictureBox();
            this.SettingsRunBgOnStartCheckBox = new System.Windows.Forms.CheckBox();
            this.SettingsSaveButton = new System.Windows.Forms.Button();
            this.SettingsResetButton = new System.Windows.Forms.Button();
            this.SettingsCheckWlanOnStartCheckBox = new System.Windows.Forms.CheckBox();
            this.SettingsAddToListButton = new System.Windows.Forms.Button();
            this.SettingsProxyPortLabel = new System.Windows.Forms.Label();
            this.SettingsProxyHostLabel = new System.Windows.Forms.Label();
            this.SettingsProxyPortTextBox = new System.Windows.Forms.TextBox();
            this.SettingsProxyHostTextBox = new System.Windows.Forms.TextBox();
            this.SettingsStandardWlanLabel = new System.Windows.Forms.Label();
            this.SettingsLabel = new System.Windows.Forms.Label();
            this.SettingsStandardWlanTextBox = new System.Windows.Forms.TextBox();
            this.SettingsSavedLabel = new System.Windows.Forms.Label();
            this.SetStandardWlanButton = new System.Windows.Forms.Button();
            this.CurrentWlanLabel = new System.Windows.Forms.Label();
            this.RemoveProxyButton = new System.Windows.Forms.Button();
            this.SetProxyButton = new System.Windows.Forms.Button();
            this.ProxyListBox = new System.Windows.Forms.ListBox();
            this.RemoveProxyListButton = new System.Windows.Forms.Button();
            this.SettingsDeleteConfigButton = new System.Windows.Forms.Button();
            this.SettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingSpinnerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NetworkListBox
            // 
            this.NetworkListBox.FormattingEnabled = true;
            this.NetworkListBox.ItemHeight = 15;
            this.NetworkListBox.Location = new System.Drawing.Point(12, 13);
            this.NetworkListBox.Name = "NetworkListBox";
            this.NetworkListBox.Size = new System.Drawing.Size(189, 214);
            this.NetworkListBox.TabIndex = 0;
            this.NetworkListBox.SelectedIndexChanged += new System.EventHandler(this.NetworkListBox_SelectedIndexChanged);
            // 
            // CurrentWlanTextBox
            // 
            this.CurrentWlanTextBox.Location = new System.Drawing.Point(218, 80);
            this.CurrentWlanTextBox.Name = "CurrentWlanTextBox";
            this.CurrentWlanTextBox.ReadOnly = true;
            this.CurrentWlanTextBox.Size = new System.Drawing.Size(141, 23);
            this.CurrentWlanTextBox.TabIndex = 1;
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SettingsPanel.Controls.Add(this.SettingsDeleteConfigButton);
            this.SettingsPanel.Controls.Add(this.LoadingSpinnerPictureBox);
            this.SettingsPanel.Controls.Add(this.SettingsRunBgOnStartCheckBox);
            this.SettingsPanel.Controls.Add(this.SettingsSaveButton);
            this.SettingsPanel.Controls.Add(this.SettingsResetButton);
            this.SettingsPanel.Controls.Add(this.SettingsCheckWlanOnStartCheckBox);
            this.SettingsPanel.Controls.Add(this.SettingsAddToListButton);
            this.SettingsPanel.Controls.Add(this.SettingsProxyPortLabel);
            this.SettingsPanel.Controls.Add(this.SettingsProxyHostLabel);
            this.SettingsPanel.Controls.Add(this.SettingsProxyPortTextBox);
            this.SettingsPanel.Controls.Add(this.SettingsProxyHostTextBox);
            this.SettingsPanel.Controls.Add(this.SettingsStandardWlanLabel);
            this.SettingsPanel.Controls.Add(this.SettingsLabel);
            this.SettingsPanel.Controls.Add(this.SettingsStandardWlanTextBox);
            this.SettingsPanel.Controls.Add(this.SettingsSavedLabel);
            this.SettingsPanel.Location = new System.Drawing.Point(395, 12);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Size = new System.Drawing.Size(393, 354);
            this.SettingsPanel.TabIndex = 2;
            // 
            // LoadingSpinnerPictureBox
            // 
            this.LoadingSpinnerPictureBox.Image = global::KMS_Helper.Properties.Resources.loading_spinner;
            this.LoadingSpinnerPictureBox.Location = new System.Drawing.Point(169, 326);
            this.LoadingSpinnerPictureBox.Name = "LoadingSpinnerPictureBox";
            this.LoadingSpinnerPictureBox.Size = new System.Drawing.Size(23, 23);
            this.LoadingSpinnerPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LoadingSpinnerPictureBox.TabIndex = 8;
            this.LoadingSpinnerPictureBox.TabStop = false;
            this.LoadingSpinnerPictureBox.Visible = false;
            // 
            // SettingsRunBgOnStartCheckBox
            // 
            this.SettingsRunBgOnStartCheckBox.AutoSize = true;
            this.SettingsRunBgOnStartCheckBox.Location = new System.Drawing.Point(153, 83);
            this.SettingsRunBgOnStartCheckBox.Name = "SettingsRunBgOnStartCheckBox";
            this.SettingsRunBgOnStartCheckBox.Size = new System.Drawing.Size(196, 19);
            this.SettingsRunBgOnStartCheckBox.TabIndex = 14;
            this.SettingsRunBgOnStartCheckBox.Text = "On auto start run in background";
            this.SettingsRunBgOnStartCheckBox.UseVisualStyleBackColor = true;
            this.SettingsRunBgOnStartCheckBox.CheckedChanged += new System.EventHandler(this.SettingsRunBgOnStartCheckBox_CheckedChanged);
            // 
            // SettingsSaveButton
            // 
            this.SettingsSaveButton.Location = new System.Drawing.Point(206, 326);
            this.SettingsSaveButton.Name = "SettingsSaveButton";
            this.SettingsSaveButton.Size = new System.Drawing.Size(57, 23);
            this.SettingsSaveButton.TabIndex = 13;
            this.SettingsSaveButton.Text = "Save";
            this.SettingsSaveButton.UseVisualStyleBackColor = true;
            this.SettingsSaveButton.Click += new System.EventHandler(this.SettingsSaveButton_Click);
            // 
            // SettingsResetButton
            // 
            this.SettingsResetButton.Location = new System.Drawing.Point(269, 326);
            this.SettingsResetButton.Name = "SettingsResetButton";
            this.SettingsResetButton.Size = new System.Drawing.Size(119, 23);
            this.SettingsResetButton.TabIndex = 12;
            this.SettingsResetButton.Text = "Reset to Default";
            this.SettingsResetButton.UseVisualStyleBackColor = true;
            this.SettingsResetButton.Click += new System.EventHandler(this.SettingsResetButton_Click);
            // 
            // SettingsCheckWlanOnStartCheckBox
            // 
            this.SettingsCheckWlanOnStartCheckBox.AutoSize = true;
            this.SettingsCheckWlanOnStartCheckBox.Location = new System.Drawing.Point(153, 58);
            this.SettingsCheckWlanOnStartCheckBox.Name = "SettingsCheckWlanOnStartCheckBox";
            this.SettingsCheckWlanOnStartCheckBox.Size = new System.Drawing.Size(184, 19);
            this.SettingsCheckWlanOnStartCheckBox.TabIndex = 11;
            this.SettingsCheckWlanOnStartCheckBox.Text = "Check Wlan on Windows start";
            this.SettingsCheckWlanOnStartCheckBox.UseVisualStyleBackColor = true;
            this.SettingsCheckWlanOnStartCheckBox.CheckedChanged += new System.EventHandler(this.SettingsCheckWlanOnStartCheckBox_CheckedChanged);
            // 
            // SettingsAddToListButton
            // 
            this.SettingsAddToListButton.Location = new System.Drawing.Point(17, 275);
            this.SettingsAddToListButton.Name = "SettingsAddToListButton";
            this.SettingsAddToListButton.Size = new System.Drawing.Size(120, 23);
            this.SettingsAddToListButton.TabIndex = 8;
            this.SettingsAddToListButton.Text = "Add to Proxy List";
            this.SettingsAddToListButton.UseVisualStyleBackColor = true;
            this.SettingsAddToListButton.Click += new System.EventHandler(this.SettingsKMSStandardButton_Click);
            // 
            // SettingsProxyPortLabel
            // 
            this.SettingsProxyPortLabel.AutoSize = true;
            this.SettingsProxyPortLabel.Location = new System.Drawing.Point(17, 228);
            this.SettingsProxyPortLabel.Name = "SettingsProxyPortLabel";
            this.SettingsProxyPortLabel.Size = new System.Drawing.Size(62, 15);
            this.SettingsProxyPortLabel.TabIndex = 10;
            this.SettingsProxyPortLabel.Text = "Proxy Port";
            // 
            // SettingsProxyHostLabel
            // 
            this.SettingsProxyHostLabel.AutoSize = true;
            this.SettingsProxyHostLabel.Location = new System.Drawing.Point(17, 182);
            this.SettingsProxyHostLabel.Name = "SettingsProxyHostLabel";
            this.SettingsProxyHostLabel.Size = new System.Drawing.Size(65, 15);
            this.SettingsProxyHostLabel.TabIndex = 9;
            this.SettingsProxyHostLabel.Text = "Proxy Host";
            // 
            // SettingsProxyPortTextBox
            // 
            this.SettingsProxyPortTextBox.Location = new System.Drawing.Point(17, 246);
            this.SettingsProxyPortTextBox.Name = "SettingsProxyPortTextBox";
            this.SettingsProxyPortTextBox.Size = new System.Drawing.Size(100, 23);
            this.SettingsProxyPortTextBox.TabIndex = 8;
            this.SettingsProxyPortTextBox.TextChanged += new System.EventHandler(this.SettingsProxyPortTextBox_TextChanged);
            this.SettingsProxyPortTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SettingsProxyPortTextBox_KeyPress);
            // 
            // SettingsProxyHostTextBox
            // 
            this.SettingsProxyHostTextBox.Location = new System.Drawing.Point(17, 200);
            this.SettingsProxyHostTextBox.Name = "SettingsProxyHostTextBox";
            this.SettingsProxyHostTextBox.Size = new System.Drawing.Size(160, 23);
            this.SettingsProxyHostTextBox.TabIndex = 7;
            this.SettingsProxyHostTextBox.TextChanged += new System.EventHandler(this.SettingsProxyHostTextBox_TextChanged);
            // 
            // SettingsStandardWlanLabel
            // 
            this.SettingsStandardWlanLabel.AutoSize = true;
            this.SettingsStandardWlanLabel.Location = new System.Drawing.Point(17, 40);
            this.SettingsStandardWlanLabel.Name = "SettingsStandardWlanLabel";
            this.SettingsStandardWlanLabel.Size = new System.Drawing.Size(117, 15);
            this.SettingsStandardWlanLabel.TabIndex = 6;
            this.SettingsStandardWlanLabel.Text = "Standard Proxy Wlan";
            // 
            // SettingsLabel
            // 
            this.SettingsLabel.AutoSize = true;
            this.SettingsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SettingsLabel.Location = new System.Drawing.Point(-1, 0);
            this.SettingsLabel.Name = "SettingsLabel";
            this.SettingsLabel.Size = new System.Drawing.Size(66, 21);
            this.SettingsLabel.TabIndex = 4;
            this.SettingsLabel.Text = "Settings";
            // 
            // SettingsStandardWlanTextBox
            // 
            this.SettingsStandardWlanTextBox.Location = new System.Drawing.Point(17, 58);
            this.SettingsStandardWlanTextBox.Name = "SettingsStandardWlanTextBox";
            this.SettingsStandardWlanTextBox.ReadOnly = true;
            this.SettingsStandardWlanTextBox.Size = new System.Drawing.Size(100, 23);
            this.SettingsStandardWlanTextBox.TabIndex = 3;
            // 
            // SettingsSavedLabel
            // 
            this.SettingsSavedLabel.AutoSize = true;
            this.SettingsSavedLabel.Location = new System.Drawing.Point(162, 330);
            this.SettingsSavedLabel.Name = "SettingsSavedLabel";
            this.SettingsSavedLabel.Size = new System.Drawing.Size(38, 15);
            this.SettingsSavedLabel.TabIndex = 15;
            this.SettingsSavedLabel.Text = "Saved";
            this.SettingsSavedLabel.Visible = false;
            // 
            // SetStandardWlanButton
            // 
            this.SetStandardWlanButton.Location = new System.Drawing.Point(218, 122);
            this.SetStandardWlanButton.Name = "SetStandardWlanButton";
            this.SetStandardWlanButton.Size = new System.Drawing.Size(141, 44);
            this.SetStandardWlanButton.TabIndex = 3;
            this.SetStandardWlanButton.Text = "Use as standard Wlan";
            this.SetStandardWlanButton.UseVisualStyleBackColor = true;
            this.SetStandardWlanButton.Click += new System.EventHandler(this.SetStandardWlanButton_Click);
            // 
            // CurrentWlanLabel
            // 
            this.CurrentWlanLabel.AutoSize = true;
            this.CurrentWlanLabel.Location = new System.Drawing.Point(218, 62);
            this.CurrentWlanLabel.Name = "CurrentWlanLabel";
            this.CurrentWlanLabel.Size = new System.Drawing.Size(81, 15);
            this.CurrentWlanLabel.TabIndex = 5;
            this.CurrentWlanLabel.Text = "Selected Wlan";
            // 
            // RemoveProxyButton
            // 
            this.RemoveProxyButton.Location = new System.Drawing.Point(665, 407);
            this.RemoveProxyButton.Name = "RemoveProxyButton";
            this.RemoveProxyButton.Size = new System.Drawing.Size(123, 31);
            this.RemoveProxyButton.TabIndex = 6;
            this.RemoveProxyButton.Text = "Remove Proxy";
            this.RemoveProxyButton.UseVisualStyleBackColor = true;
            this.RemoveProxyButton.Click += new System.EventHandler(this.RemoveProxyButton_Click);
            // 
            // SetProxyButton
            // 
            this.SetProxyButton.Location = new System.Drawing.Point(536, 407);
            this.SetProxyButton.Name = "SetProxyButton";
            this.SetProxyButton.Size = new System.Drawing.Size(123, 31);
            this.SetProxyButton.TabIndex = 7;
            this.SetProxyButton.Text = "Set Proxy";
            this.SetProxyButton.UseVisualStyleBackColor = true;
            this.SetProxyButton.Click += new System.EventHandler(this.SetProxyButton_Click);
            // 
            // ProxyListBox
            // 
            this.ProxyListBox.FormattingEnabled = true;
            this.ProxyListBox.ItemHeight = 15;
            this.ProxyListBox.Location = new System.Drawing.Point(12, 248);
            this.ProxyListBox.Name = "ProxyListBox";
            this.ProxyListBox.Size = new System.Drawing.Size(189, 184);
            this.ProxyListBox.TabIndex = 8;
            this.ProxyListBox.SelectedIndexChanged += new System.EventHandler(this.ProxyListBox_SelectedIndexChanged);
            // 
            // RemoveProxyListButton
            // 
            this.RemoveProxyListButton.Location = new System.Drawing.Point(207, 391);
            this.RemoveProxyListButton.Name = "RemoveProxyListButton";
            this.RemoveProxyListButton.Size = new System.Drawing.Size(152, 23);
            this.RemoveProxyListButton.TabIndex = 16;
            this.RemoveProxyListButton.Text = "Remove Proxy from List";
            this.RemoveProxyListButton.UseVisualStyleBackColor = true;
            this.RemoveProxyListButton.Click += new System.EventHandler(this.RemoveProxyListButton_Click);
            // 
            // SettingsDeleteConfigButton
            // 
            this.SettingsDeleteConfigButton.Location = new System.Drawing.Point(269, 3);
            this.SettingsDeleteConfigButton.Name = "SettingsDeleteConfigButton";
            this.SettingsDeleteConfigButton.Size = new System.Drawing.Size(119, 23);
            this.SettingsDeleteConfigButton.TabIndex = 16;
            this.SettingsDeleteConfigButton.Text = "Delete Config File";
            this.SettingsDeleteConfigButton.UseVisualStyleBackColor = true;
            this.SettingsDeleteConfigButton.Click += new System.EventHandler(this.SettingsDeleteConfigButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RemoveProxyListButton);
            this.Controls.Add(this.ProxyListBox);
            this.Controls.Add(this.SetProxyButton);
            this.Controls.Add(this.RemoveProxyButton);
            this.Controls.Add(this.CurrentWlanLabel);
            this.Controls.Add(this.SetStandardWlanButton);
            this.Controls.Add(this.SettingsPanel);
            this.Controls.Add(this.CurrentWlanTextBox);
            this.Controls.Add(this.NetworkListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "KMS Helper 2.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingSpinnerPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox NetworkListBox;
        private TextBox CurrentWlanTextBox;
        private Panel SettingsPanel;
        private Label SettingsLabel;
        private TextBox SettingsStandardWlanTextBox;
        private Button SetStandardWlanButton;
        private Label CurrentWlanLabel;
        private Label SettingsStandardWlanLabel;
        private Button SettingsResetButton;
        private CheckBox SettingsCheckWlanOnStartCheckBox;
        private Button SettingsAddToListButton;
        private Label SettingsProxyPortLabel;
        private Label SettingsProxyHostLabel;
        private TextBox SettingsProxyPortTextBox;
        private TextBox SettingsProxyHostTextBox;
        private Button RemoveProxyButton;
        private Button SetProxyButton;
        private Button SettingsSaveButton;
        private CheckBox SettingsRunBgOnStartCheckBox;
        private PictureBox LoadingSpinnerPictureBox;
        private Label SettingsSavedLabel;
        private ListBox ProxyListBox;
        private Button RemoveProxyListButton;
        private Button SettingsDeleteConfigButton;
    }
}