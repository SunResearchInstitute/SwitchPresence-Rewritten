namespace SwitchPresence_Rewritten_GUI
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.label1 = new System.Windows.Forms.Label();
            this.smallImageKey = new System.Windows.Forms.TextBox();
            this.largeImageKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.largeImageText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.showTimer = new System.Windows.Forms.CheckBox();
            this.shrinkToTray = new System.Windows.Forms.CheckBox();
            this.mainMenuStatus = new System.Windows.Forms.CheckBox();
            this.autoToMac = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 104);
            this.label1.MinimumSize = new System.Drawing.Size(100, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Small Image Key";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // smallImageKey
            // 
            this.smallImageKey.Location = new System.Drawing.Point(70, 120);
            this.smallImageKey.MaxLength = 32;
            this.smallImageKey.Name = "smallImageKey";
            this.smallImageKey.Size = new System.Drawing.Size(100, 20);
            this.smallImageKey.TabIndex = 9;
            // 
            // largeImageKey
            // 
            this.largeImageKey.Location = new System.Drawing.Point(70, 36);
            this.largeImageKey.MaxLength = 32;
            this.largeImageKey.Name = "largeImageKey";
            this.largeImageKey.Size = new System.Drawing.Size(100, 20);
            this.largeImageKey.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 20);
            this.label2.MinimumSize = new System.Drawing.Size(100, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Large Image Key";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // largeImageText
            // 
            this.largeImageText.Location = new System.Drawing.Point(70, 80);
            this.largeImageText.MaxLength = 128;
            this.largeImageText.Name = "largeImageText";
            this.largeImageText.Size = new System.Drawing.Size(100, 20);
            this.largeImageText.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(70, 64);
            this.label3.MinimumSize = new System.Drawing.Size(100, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Large Image Text";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showTimer
            // 
            this.showTimer.AutoSize = true;
            this.showTimer.Checked = true;
            this.showTimer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTimer.Location = new System.Drawing.Point(39, 146);
            this.showTimer.Name = "showTimer";
            this.showTimer.Size = new System.Drawing.Size(117, 17);
            this.showTimer.TabIndex = 14;
            this.showTimer.Text = "Show Time Lapsed";
            this.showTimer.UseVisualStyleBackColor = true;
            // 
            // shrinkToTray
            // 
            this.shrinkToTray.AutoSize = true;
            this.shrinkToTray.Checked = true;
            this.shrinkToTray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shrinkToTray.Location = new System.Drawing.Point(39, 169);
            this.shrinkToTray.Name = "shrinkToTray";
            this.shrinkToTray.Size = new System.Drawing.Size(102, 17);
            this.shrinkToTray.TabIndex = 15;
            this.shrinkToTray.Text = "Minimize to Tray";
            this.shrinkToTray.UseVisualStyleBackColor = true;
            // 
            // mainMenuStatus
            // 
            this.mainMenuStatus.AutoSize = true;
            this.mainMenuStatus.Checked = true;
            this.mainMenuStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mainMenuStatus.Location = new System.Drawing.Point(39, 192);
            this.mainMenuStatus.Name = "mainMenuStatus";
            this.mainMenuStatus.Size = new System.Drawing.Size(170, 17);
            this.mainMenuStatus.TabIndex = 18;
            this.mainMenuStatus.Text = "Display Main Menu as a status";
            this.mainMenuStatus.UseVisualStyleBackColor = true;
            // 
            // autoToMac
            // 
            this.autoToMac.AutoSize = true;
            this.autoToMac.Location = new System.Drawing.Point(39, 215);
            this.autoToMac.Name = "autoToMac";
            this.autoToMac.Size = new System.Drawing.Size(178, 17);
            this.autoToMac.TabIndex = 19;
            this.autoToMac.Text = "Automatically convert IP to MAC";
            this.autoToMac.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(39, 238);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(134, 238);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 21;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // Settings
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(244, 281);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.smallImageKey);
            this.Controls.Add(this.autoToMac);
            this.Controls.Add(this.largeImageKey);
            this.Controls.Add(this.mainMenuStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.shrinkToTray);
            this.Controls.Add(this.largeImageText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.showTimer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox smallImageKey;
        private System.Windows.Forms.TextBox largeImageKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox largeImageText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox showTimer;
        private System.Windows.Forms.CheckBox shrinkToTray;
        private System.Windows.Forms.CheckBox mainMenuStatus;
        private System.Windows.Forms.CheckBox autoToMac;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
    }
}