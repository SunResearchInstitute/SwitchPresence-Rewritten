using Newtonsoft.Json;
using System.IO;

namespace SwitchPresence_Rewritten_GUI
{
    public class Config : ConfigData
    {
        public void loadConfig()
        {
            if (File.Exists("Config.json"))
            {
                ConfigData save = JsonConvert.DeserializeObject<ConfigData>(File.ReadAllText("Config.json"));
                IP = save.IP;
                Client = save.Client;
                BigKey = save.BigKey;
                BigText = save.BigText;
                SmallKey = save.SmallKey;
                State = save.State;
                DisplayTimer = save.DisplayTimer;
                AllowTray = save.AllowTray;
                DisplayMainMenu = save.DisplayMainMenu;
                SeenAutoMacPrompt = save.SeenAutoMacPrompt;
                AutoToMac = save.AutoToMac;
                IsFirstRun = save.IsFirstRun;
                AllowCustomKeyText = save.AllowCustomKeyText;
            }
            else
            {
                // A few defaults we want to set on first run go here.
                DisplayTimer = true;
                AllowTray = true;
                SeenAutoMacPrompt = false;
                IsFirstRun = true;
                AllowCustomKeyText = false;
            }
        }

        public void saveConfig()
        {
            ConfigData cfg = new ConfigData()
            {
                IP = this.IP,
                Client = this.Client,
                BigKey = this.BigKey,
                SmallKey = this.SmallKey,
                State = this.State,
                BigText = this.BigText,
                DisplayTimer = this.DisplayTimer,
                AllowTray = this.AllowTray,
                DisplayMainMenu = this.DisplayMainMenu,
                SeenAutoMacPrompt = this.SeenAutoMacPrompt,
                AutoToMac = this.AutoToMac,
                IsFirstRun = this.IsFirstRun,
                AllowCustomKeyText = this.AllowCustomKeyText
        };
            File.WriteAllText("Config.json", JsonConvert.SerializeObject(cfg));
        }
    }
}
