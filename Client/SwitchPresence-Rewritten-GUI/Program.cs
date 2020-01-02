using System;
using System.Windows.Forms;

namespace SwitchPresence_Rewritten_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // We'll load the config here to make it accessible by the whole application
            Config cfg = new Config();
            cfg.LoadConfig();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(cfg));
        }
    }
}
