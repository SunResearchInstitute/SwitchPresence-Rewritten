using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwitchPresence_Rewritten_GUI
{
    public static class Utils
    {
        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }

        //This will only work on Windows(?)
        public static string GetMacByIp(string ip)
        {
            List<MacIpPair> macIpPairs = GetAllMacAddressesAndIPPairs();

            return macIpPairs.FirstOrDefault(x => x.IpAddress == ip).MacAddress ?? "";
        }

        public static string GetIpByMac(string mac)
        {
            mac = mac.ToLower();
            List<MacIpPair> macIpPairs = GetAllMacAddressesAndIPPairs();

            return macIpPairs.FirstOrDefault(x => x.MacAddress == mac).IpAddress ?? "";
        }
        public static List<MacIpPair> GetAllMacAddressesAndIPPairs()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            using (Process pProcess = new Process())
            {
                pProcess.StartInfo.FileName = "arp";
                pProcess.StartInfo.Arguments = "-a ";
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.Start();
                string cmdOutput = pProcess.StandardOutput.ReadToEnd();
                string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

                foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
                {
                    mip.Add(new MacIpPair()
                    {
                        MacAddress = m.Groups["mac"].Value,
                        IpAddress = m.Groups["ip"].Value
                    });
                }
            }
            return mip;
        }
    }
}
