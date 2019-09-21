using DiscordRPC;
#if DEBUG
using DiscordRPC.Logging;
#endif
using Newtonsoft.Json;
using SwitchPresence_Rewritten.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net;
using System.Timers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Timer = System.Timers.Timer;

namespace SwitchPresence_Rewritten
{
    public partial class MainForm : Form
    {
        Thread listenThread;
        static Socket client;
        static DiscordRpcClient rpc;
        IPAddress ipAddress;
        PhysicalAddress macAddress;
        bool ManualUpdate = false;
        string LastGame = "";
        Timestamps time = null;
        Timer timer;
        public MainForm()
        {
            InitializeComponent();
            listenThread = new Thread(TryConnect);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TitlePacket
        {
            [MarshalAs(UnmanagedType.U8)]
            public ulong magic;
            [MarshalAs(UnmanagedType.U8)]
            public ulong tid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string name;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (connectButton.Text == "Connect")
            {

                if (string.IsNullOrWhiteSpace(clientBox.Text))
                {
                    Show();
                    Activate();
                    UpdateStatus("Client ID cannot be empty", Color.DarkRed);
                    SystemSounds.Exclamation.Play();
                    return;
                }


                if (!IPAddress.TryParse(addressBox.Text, out ipAddress))
                {
                    try
                    {
                        macAddress = PhysicalAddress.Parse(addressBox.Text.ToUpper());
                        ipAddress = IPAddress.Parse(GetIpByMac(addressBox.Text));
                    }
                    catch (FormatException)
                    {
                        Show();
                        Activate();
                        UpdateStatus("Invalid IP or MAC Address", Color.DarkRed);
                        SystemSounds.Exclamation.Play();
                        return;
                    }
                }

                listenThread.Start();

                connectButton.Text = "Disconnect";
                connectToolStripMenuItem.Text = "Disconnect";

                macButton.Visible = (macAddress == null);
                macButton.Enabled = false;
                addressBox.Enabled = false;
                clientBox.Enabled = false;
            }
            else
            {
                listenThread.Abort();
                if (rpc != null && !rpc.IsDisposed) rpc.Dispose();
                if (client != null) client.Close();
                if (timer != null) timer.Dispose();
                listenThread = new Thread(TryConnect);
                UpdateStatus("", Color.Gray);
                connectButton.Text = "Connect";
                connectToolStripMenuItem.Text = "Connect";
                trayIcon.Icon = Resources.Disconnected;
                trayIcon.Text = "SwitchPresence (Disconnected)";

                macAddress = null;
                ipAddress = null;
                macButton.Visible = false;
                macButton.Enabled = false;
                addressBox.Enabled = true;
                clientBox.Enabled = true;
                LastGame = "";
                time = null;
            }
        }

        private void OnConnectTimeout(object source, ElapsedEventArgs e)
        {
            LastGame = "";
            time = null;
        }

        private void TryConnect()
        {
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 0xCAFE);

            if (rpc != null && !rpc.IsDisposed) rpc.Dispose();
            rpc = new DiscordRpcClient(clientBox.Text);
            rpc.Initialize();

            timer = new Timer()
            {
                Interval = 15000,
                SynchronizingObject = this,
                Enabled = false,
            };
            timer.Elapsed += new ElapsedEventHandler(OnConnectTimeout);

#if DEBUG
            rpc.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            //Subscribe to events
            rpc.OnReady += (s, obj) =>
            {
                Console.WriteLine("Received Ready from user {0}", obj.User.Username);
            };

            rpc.OnPresenceUpdate += (s, obj) =>
            {
                Console.WriteLine("Received Update! {0}", obj.Presence);
            };
#endif

            while (true)
            {
                client = new Socket(SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveTimeout = 5500,
                    SendTimeout = 5500,
                };

                UpdateStatus("Attemping to connect to server...", Color.Gray);
                trayIcon.Icon = Resources.Disconnected;
                trayIcon.Text = "SwitchPresence (Connecting...)";
                timer.Enabled = true;
                EnableMacButton(false);

                try
                {
                    IAsyncResult result = client.BeginConnect(localEndPoint, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(2000, true);
                    if (!success)
                    {
                        //UpdateStatus("Could not connect to Server! Retrying...", Color.DarkRed);
                        client.Close();
                    }
                    else
                    {
                        client.EndConnect(result);
                        timer.Enabled = false;

                        DataListen();
                    }
                }
                catch (SocketException)
                {
                    client.Close();
                    if (rpc != null && !rpc.IsDisposed) rpc.SetPresence(null);
                }
            }
        }

        private void UpdateStatus(string text, Color color)
        {
            MethodInvoker inv = () =>
            {
                statusLabel.Text = text;
                statusLabel.ForeColor = color;
            };
            Invoke(inv);
        }

        private void EnableMacButton(bool enable)
        {
            MethodInvoker inv = () =>
            {
                macButton.Enabled = enable;
            };
            Invoke(inv);
        }

        private void DataListen()
        {
            while (true)
            {
                try
                {
                    byte[] bytes = new byte[800];
                    int cnt = client.Receive(bytes);
                    UpdateStatus("Connected to the server!", Color.Green);
                    trayIcon.Icon = Resources.Connected;
                    trayIcon.Text = "SwitchPresence (Connected)";
                    EnableMacButton(true);
                    TitlePacket title = Utils.ByteArrayToStructure<TitlePacket>(bytes);
                    if (title.magic == 0xffaadd23)
                    {
                        if (LastGame != title.name)
                        {
                            time = Timestamps.Now;
                        }
                        if ((rpc != null && rpc.CurrentPresence == null) || LastGame != title.name || ManualUpdate)
                        {
                            Assets ass = new Assets
                            {
                                SmallImageKey = smallKeyBox.Text,
                                SmallImageText = "Switch-Presence Rewritten"
                            };
                            RichPresence presence = new RichPresence
                            {
                                State = stateBox.Text
                            };


                            if (title.name == "NULL")
                            {
                                ass.LargeImageText = "Home Menu";
                                ass.LargeImageText = !string.IsNullOrWhiteSpace(bigTextBox.Text) ? bigTextBox.Text : "Home Menu";
                                ass.LargeImageKey = !string.IsNullOrWhiteSpace(bigKeyBox.Text) ? bigKeyBox.Text : string.Format("0{0:x}", 0x0100000000001000);
                                presence.Details = "In the home menu";
                            }
                            else
                            {

                                ass.LargeImageText = !string.IsNullOrWhiteSpace(bigTextBox.Text) ? bigTextBox.Text : title.name;
                                ass.LargeImageKey = !string.IsNullOrWhiteSpace(bigKeyBox.Text) ? bigKeyBox.Text : string.Format("0{0:x}", title.tid);
                                presence.Details = $"Playing {title.name}";

                            }

                            presence.Assets = ass;
                            if (checkTime.Checked) presence.Timestamps = time;
                            rpc.SetPresence(presence);

                            ManualUpdate = false;
                            LastGame = title.name;
                        }
                    }
                    else
                    {
                        if (rpc != null && !rpc.IsDisposed) rpc.SetPresence(null);
                        client.Close();
                        return;
                    }
                }
                catch (SocketException)
                {
                    if (rpc != null && !rpc.IsDisposed) rpc.SetPresence(null);
                    client.Close();
                    return;
                }
            }
        }

        public string GetMacByIp(string ip)
        {
            var macIpPairs = GetAllMacAddressesAndIppairs();
            int index = macIpPairs.FindIndex(x => x.IpAddress == ip);
            if (index >= 0)
            {
                return macIpPairs[index].MacAddress.ToUpper();
            }
            else
            {
                return null;
            }
        }

        public string GetIpByMac(string mac)
        {
            mac = mac.ToLower();
            var macIpPairs = GetAllMacAddressesAndIppairs();
            int index = macIpPairs.FindIndex(x => x.MacAddress == mac);
            if (index >= 0)
            {
                return macIpPairs[index].IpAddress;
            }
            else
            {
                return "";
            }
        }

        public List<MacIpPair> GetAllMacAddressesAndIppairs()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            Process pProcess = new Process();
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
            pProcess.Dispose();
            return mip;
        }
        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }


        private void CheckTime_CheckedChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start($"https://discordapp.com/developers/applications/{clientBox.Text}");

        private void BigKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void SKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void StateBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void BigTextBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists("Config.json"))
            {
                Config cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
                checkTime.Checked = cfg.DisplayTimer;
                bigKeyBox.Text = cfg.BigKey;
                bigTextBox.Text = cfg.BigText;
                smallKeyBox.Text = cfg.SmallKey;
                addressBox.Text = cfg.IP;
                stateBox.Text = cfg.State;
                clientBox.Text = cfg.Client;
                checkTray.Checked = cfg.AllowTray;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            listenThread.Abort();
            if (rpc != null && !rpc.IsDisposed) rpc.Dispose();
            if (client != null) client.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && checkTray.Checked)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                Config cfg = new Config()
                {
                    IP = addressBox.Text,
                    Client = clientBox.Text,
                    BigKey = bigKeyBox.Text,
                    SmallKey = smallKeyBox.Text,
                    State = stateBox.Text,
                    BigText = bigTextBox.Text,
                    DisplayTimer = checkTime.Checked,
                    AllowTray = checkTray.Checked
                };
                File.WriteAllText("Config.json", JsonConvert.SerializeObject(cfg));
            }
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            Activate();
        }

        private void TrayExitMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void MacButton_Click(object sender, EventArgs e)
        {
            string macAddress = GetMacByIp(ipAddress.ToString());
            if (macAddress != null)
            {
                addressBox.Text = macAddress;
                macButton.Visible = false;
            }
            else
            {
                MessageBox.Show("Can't convert to MAC Address! Sorry!");
            }
        }
    }

    public class Config
    {
        public string IP, Client, BigKey, BigText, SmallKey, State;
        public bool DisplayTimer, AllowTray;

        public Config() { }
    }
}
