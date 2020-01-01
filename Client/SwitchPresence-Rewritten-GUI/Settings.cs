using System;
using System.Diagnostics;
using System.Windows.Forms;

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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UseMacDefault_CheckedChanged(object sender, EventArgs e) => config.SeenAutoMacPrompt = true;

        private void ClientIDLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start($"https://discordapp.com/developers/applications/{config.Client}");
    }
}
