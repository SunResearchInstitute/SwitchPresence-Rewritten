using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SwitchPresence_Rewritten_GUI
{
    public partial class Settings : Form
    {
        private Config config;

        public Settings(Config cfg)
        {
            InitializeComponent();
            config = cfg;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            clientBox.Text = config.Client;
            smallImageKey.Text = config.SmallKey;
            showTimer.Checked = config.DisplayTimer;
            shrinkToTray.Checked = config.AllowTray;
            mainMenuStatus.Checked = config.DisplayMainMenu;
            autoToMac.Checked = config.AutoToMac;
            useCustomTextBox.Checked = config.AllowCustomKeyText;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // check and see if client ID is valid
            Match match = Regex.Match(clientBox.Text, @"^\d{18}$");
            if (match.Success)
            {
                config.Client = clientBox.Text;
                config.SmallKey = smallImageKey.Text;
                config.DisplayTimer = showTimer.Checked;
                config.AllowTray = shrinkToTray.Checked;
                config.DisplayMainMenu = mainMenuStatus.Checked;
                config.AutoToMac = autoToMac.Checked;
                config.AllowCustomKeyText = useCustomTextBox.Checked;
                config.saveConfig();
                this.Close();
            } 
            else
            {
                string alert = "The Client ID provided is not valid. Please input a valid Discord Client ID.";
                MessageBox.Show(alert, "Invalid Client ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clientBox_TextChanged(object sender, EventArgs e)
        {
            Match match = Regex.Match(clientBox.Text, @"^\d{18}$");
            if (match.Success)
            {
                clientBox.ForeColor = Color.FromName("Black");
            }
            else
            {
                clientBox.ForeColor = Color.FromName("Red");
            }
        }

        private void UseMacDefault_CheckedChanged(object sender, EventArgs e) => config.SeenAutoMacPrompt = true;

        private void ClientIDLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start($"https://discordapp.com/developers/applications/{config.Client}");
    }
}
