namespace SwitchPresence_Rewritten_GUI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.connectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.stateBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAppStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.trayExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addressBox = new System.Windows.Forms.TextBox();
            this.settingsButton = new System.Windows.Forms.Button();
            this.largeImageKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.largeImageText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.trayContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(41, 172);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 13;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 19);
            this.label1.MinimumSize = new System.Drawing.Size(100, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP or MAC Address";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 63);
            this.label2.MinimumSize = new System.Drawing.Size(100, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "State Text";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateBox
            // 
            this.stateBox.Location = new System.Drawing.Point(18, 79);
            this.stateBox.MaxLength = 128;
            this.stateBox.Name = "stateBox";
            this.stateBox.Size = new System.Drawing.Size(100, 20);
            this.stateBox.TabIndex = 11;
            this.stateBox.TextChanged += new System.EventHandler(this.StateBox_TextChanged);
            // 
            // statusLabel
            // 
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.Red;
            this.statusLabel.Location = new System.Drawing.Point(25, 116);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(200, 40);
            this.statusLabel.TabIndex = 12;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayContextMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "SwitchPresence (Disconnected)";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // trayContextMenu
            // 
            this.trayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.showAppStrip,
            this.trayExitMenuItem});
            this.trayContextMenu.Name = "trayContextMenu";
            this.trayContextMenu.Size = new System.Drawing.Size(129, 70);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // showAppStrip
            // 
            this.showAppStrip.Name = "showAppStrip";
            this.showAppStrip.Size = new System.Drawing.Size(128, 22);
            this.showAppStrip.Text = "Show App";
            this.showAppStrip.Click += new System.EventHandler(this.toolStripShowApp_Click);
            // 
            // trayExitMenuItem
            // 
            this.trayExitMenuItem.Name = "trayExitMenuItem";
            this.trayExitMenuItem.Size = new System.Drawing.Size(128, 22);
            this.trayExitMenuItem.Text = "Exit";
            this.trayExitMenuItem.Click += new System.EventHandler(this.TrayExitMenuItem_Click);
            // 
            // addressBox
            // 
            this.addressBox.Location = new System.Drawing.Point(18, 35);
            this.addressBox.Name = "addressBox";
            this.addressBox.Size = new System.Drawing.Size(100, 20);
            this.addressBox.TabIndex = 1;
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(136, 172);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(75, 23);
            this.settingsButton.TabIndex = 20;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // largeImageKey
            // 
            this.largeImageKey.Location = new System.Drawing.Point(134, 35);
            this.largeImageKey.MaxLength = 32;
            this.largeImageKey.Name = "largeImageKey";
            this.largeImageKey.Size = new System.Drawing.Size(100, 20);
            this.largeImageKey.TabIndex = 5;
            this.largeImageKey.TextChanged += new System.EventHandler(this.largeImageKey_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 63);
            this.label3.MinimumSize = new System.Drawing.Size(100, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Large Image Text";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // largeImageText
            // 
            this.largeImageText.Location = new System.Drawing.Point(134, 79);
            this.largeImageText.MaxLength = 128;
            this.largeImageText.Name = "largeImageText";
            this.largeImageText.Size = new System.Drawing.Size(100, 20);
            this.largeImageText.TabIndex = 7;
            this.largeImageText.TextChanged += new System.EventHandler(this.largeImageText_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(134, 19);
            this.label4.MinimumSize = new System.Drawing.Size(100, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Large Image Key";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 208);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stateBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.largeImageKey);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.addressBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.largeImageText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SwitchPresence-Rewritten";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.trayContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox stateBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayContextMenu;
        private System.Windows.Forms.ToolStripMenuItem trayExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.TextBox addressBox;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.ToolStripMenuItem showAppStrip;
        private System.Windows.Forms.TextBox largeImageKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox largeImageText;
        private System.Windows.Forms.Label label4;
    }
}

