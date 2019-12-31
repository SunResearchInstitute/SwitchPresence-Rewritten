using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            largeImageKey.Text = config.BigKey;
            largeImageText.Text = config.BigText;
            smallImageKey.Text = config.SmallKey;
            showTimer.Checked = config.DisplayTimer;
            shrinkToTray.Checked = config.AllowTray;
            mainMenuStatus.Checked = config.DisplayMainMenu;
            autoToMac.Checked = config.AutoToMac;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            config.BigKey = largeImageKey.Text;
            config.BigText = largeImageText.Text;
            config.SmallKey = smallImageKey.Text;
            config.DisplayTimer = showTimer.Checked;
            config.AllowTray = shrinkToTray.Checked;
            config.DisplayMainMenu = mainMenuStatus.Checked;
            config.AutoToMac = autoToMac.Checked;
            config.saveConfig();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
