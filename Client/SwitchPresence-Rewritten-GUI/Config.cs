using Newtonsoft.Json;
using System.IO;

namespace SwitchPresence_Rewritten_GUI
{
    public class Config : ConfigData
    {
        public void LoadConfig()
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
                AllowCustomKeyText = true;
            }
        }

        public void SaveConfig()
        {
            ConfigData cfg = new ConfigData()
            {
                IP = IP,
                Client = Client,
                BigKey = BigKey,
                SmallKey = SmallKey,
                State = State,
                BigText = BigText,
                DisplayTimer = DisplayTimer,
                AllowTray = AllowTray,
                DisplayMainMenu = DisplayMainMenu,
                SeenAutoMacPrompt = SeenAutoMacPrompt,
                AutoToMac = AutoToMac,
                IsFirstRun = IsFirstRun,
                AllowCustomKeyText = AllowCustomKeyText
            };
            File.WriteAllText("Config.json", JsonConvert.SerializeObject(cfg, Formatting.Indented));
        }
    }
}
