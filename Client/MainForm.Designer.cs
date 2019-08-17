namespace SwitchPresence_Rewritten
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ipBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clientBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkTime = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.stateBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.smallKeyBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bigKeyBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bigTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(107, 61);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(100, 20);
            this.ipBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(120, 358);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // clientBox
            // 
            this.clientBox.Location = new System.Drawing.Point(107, 114);
            this.clientBox.MaxLength = 18;
            this.clientBox.Name = "clientBox";
            this.clientBox.Size = new System.Drawing.Size(100, 20);
            this.clientBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(134, 91);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(47, 13);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Client ID";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked_1);
            // 
            // checkTime
            // 
            this.checkTime.AutoSize = true;
            this.checkTime.Checked = true;
            this.checkTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkTime.Location = new System.Drawing.Point(100, 393);
            this.checkTime.Name = "checkTime";
            this.checkTime.Size = new System.Drawing.Size(115, 17);
            this.checkTime.TabIndex = 5;
            this.checkTime.Text = "Show time elapsed";
            this.checkTime.UseVisualStyleBackColor = true;
            this.checkTime.CheckedChanged += new System.EventHandler(this.CheckTime_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "State Text";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stateBox
            // 
            this.stateBox.Location = new System.Drawing.Point(107, 291);
            this.stateBox.MaxLength = 128;
            this.stateBox.Name = "stateBox";
            this.stateBox.Size = new System.Drawing.Size(100, 20);
            this.stateBox.TabIndex = 6;
            this.stateBox.TextChanged += new System.EventHandler(this.StateBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Small Image Key";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // smallKeyBox
            // 
            this.smallKeyBox.Location = new System.Drawing.Point(107, 242);
            this.smallKeyBox.MaxLength = 32;
            this.smallKeyBox.Name = "smallKeyBox";
            this.smallKeyBox.Size = new System.Drawing.Size(100, 20);
            this.smallKeyBox.TabIndex = 8;
            this.smallKeyBox.TextChanged += new System.EventHandler(this.SKeyBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(114, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Large Image Key";
            // 
            // bigKeyBox
            // 
            this.bigKeyBox.Location = new System.Drawing.Point(107, 158);
            this.bigKeyBox.MaxLength = 32;
            this.bigKeyBox.Name = "bigKeyBox";
            this.bigKeyBox.Size = new System.Drawing.Size(100, 20);
            this.bigKeyBox.TabIndex = 10;
            this.bigKeyBox.TextChanged += new System.EventHandler(this.BigKeyBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(112, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Large Image Text";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bigTextBox
            // 
            this.bigTextBox.Location = new System.Drawing.Point(107, 202);
            this.bigTextBox.MaxLength = 128;
            this.bigTextBox.Name = "bigTextBox";
            this.bigTextBox.Size = new System.Drawing.Size(100, 20);
            this.bigTextBox.TabIndex = 12;
            this.bigTextBox.TextChanged += new System.EventHandler(this.BigTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(54, 323);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(203, 26);
            this.label6.TabIndex = 14;
            this.label6.Text = "If you go to sleep while connected\r\n you will have to reconnect!";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 455);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bigTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bigKeyBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.smallKeyBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stateBox);
            this.Controls.Add(this.checkTime);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clientBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ipBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SwitchPresence-Rewritten";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox clientBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox stateBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox smallKeyBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox bigKeyBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox bigTextBox;
        private System.Windows.Forms.Label label6;
    }
}

