using CommandLine;
using System.Net;

namespace SwitchPresence_CLI
{
    public class ConsoleOptions
    {
        [Option('m', "ignore-home-screen", Required = false, Default = false, HelpText = "Don't display the home screen")]
        public bool IgnoreHomeScreen { get; set; }

        [Value(0, MetaName = "IP", Required = true, HelpText = "The IP address of your Switch")]
        public string IP { get; set; }
        public IPAddress ParsedIP { get; set; }

        [Value(1, MetaName = "Client ID", Required = true, HelpText = "The Client ID of your Discord Rich Presence application")]
        public ulong ClientID { get; set; }
    }
}
