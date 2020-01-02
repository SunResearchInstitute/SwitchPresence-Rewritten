using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SwitchPresence_Rewritten_GUI
{
    public partial class SettingsForm : Form
    {
        private readonly Config config;

        public SettingsForm(Config cfg)
        {
            InitializeComponent();
            config = cfg;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            clientBox.Text = config.Client;
            showTimer.Checked = config.DisplayTimer;
            shrinkToTray.Checked = config.AllowTray;
            mainMenuStatus.Checked = config.DisplayMainMenu;
            autoToMac.Checked = config.AutoToMac;
            useCustomTextBox.Checked = config.AllowCustomKeyText;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // check and see if client ID is valid
            if (Regex.Match(clientBox.Text, @"^\d{18}$").Success)
            {
                config.Client = clientBox.Text;
                config.DisplayTimer = showTimer.Checked;
                config.AllowTray = shrinkToTray.Checked;
                config.DisplayMainMenu = mainMenuStatus.Checked;
                config.AutoToMac = autoToMac.Checked;
                config.AllowCustomKeyText = useCustomTextBox.Checked;
                config.SaveConfig();
                Close();
            }
            else
            {
                string alert = "The Client ID provided is not valid. Please input a valid Discord Client ID.";
                MessageBox.Show(alert, "Invalid Client ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClientBox_TextChanged(object sender, EventArgs e) => clientBox.ForeColor = Regex.Match(clientBox.Text, @"^\d{18}$").Success ? Color.FromName("Black") : Color.FromName("Red");

        private void CancelButton_Click(object sender, EventArgs e) => Close();

        private void UseMacDefault_CheckedChanged(object sender, EventArgs e) => config.SeenAutoMacPrompt = true;

        private void ClientIDLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start($"https://discordapp.com/developers/applications/{config.Client}");
    }
}
