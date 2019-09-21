using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static SwitchPresence_Rewritten.MainForm;

namespace SwitchPresence_Rewritten
{
    public static class Utils
    {
        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            T stuff;
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }

        public static string GetMacByIp(string ip)
        {
            List<MacIpPair> macIpPairs = GetAllMacAddressesAndIPPairs();
            MacIpPair pair = macIpPairs.FirstOrDefault(x => x.IpAddress == ip);

            return pair.MacAddress ?? "";
        }

        public static string GetIpByMac(string mac)
        {
            mac = mac.ToLower();
            List<MacIpPair> macIpPairs = GetAllMacAddressesAndIPPairs();
            MacIpPair pair = macIpPairs.FirstOrDefault(x => x.MacAddress == mac);

            return pair.IpAddress ?? "";
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
