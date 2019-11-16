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
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using Timer = System.Timers.Timer;

namespace SwitchPresence_Rewritten
{
    public partial class MainForm : Form
    {
        private Thread listenThread;
        static Socket client;
        static DiscordRpcClient rpc;
        private IPAddress ipAddress;
        private PhysicalAddress macAddress;
        bool ManualUpdate = false;
        string LastGame = "";
        private Timestamps time = null;
        private Timer timer;

        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }

        public MainForm()
        {
            InitializeComponent();
            listenThread = new Thread(TryConnect);
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
                        IPAddress.TryParse(Utils.GetIpByMac(addressBox.Text), out ipAddress);
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
                if (rpc != null && !rpc.IsDisposed)
                {
                    rpc.ClearPresence();
                    rpc.Dispose();
                }

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

            if (rpc != null && !rpc.IsDisposed)
            {
                rpc.ClearPresence();
                rpc.Dispose();
            }

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
                ToggleMacButton(false);

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
                    if (rpc != null && !rpc.IsDisposed) rpc.ClearPresence();
                }
            }
        }

        private void DataListen()
        {
            while (true)
            {
                try
                {
                    byte[] bytes = new byte[600];
                    int cnt = client.Receive(bytes);
                    UpdateStatus("Connected to the server!", Color.Green);
                    trayIcon.Icon = Resources.Connected;
                    trayIcon.Text = "SwitchPresence (Connected)";
                    ToggleMacButton(true);
                    Title title = new Title(bytes);
                    if (title.Magic == 0xffaadd23)
                    {
                        if (LastGame != title.Name)
                        {
                            time = Timestamps.Now;
                        }
                        if ((rpc != null && rpc.CurrentPresence == null) || LastGame != title.Name || ManualUpdate)
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

                            if (title.Name == "NULL")
                            {
                                ass.LargeImageText = "Home Menu";
                                ass.LargeImageText = !string.IsNullOrWhiteSpace(bigTextBox.Text) ? bigTextBox.Text : "Home Menu";
                                ass.LargeImageKey = !string.IsNullOrWhiteSpace(bigKeyBox.Text) ? bigKeyBox.Text : string.Format("0{0:x}", 0x0100000000001000);
                                presence.Details = "In the home menu";
                            }
                            else
                            {
                                ass.LargeImageText = !string.IsNullOrWhiteSpace(bigTextBox.Text) ? bigTextBox.Text : title.Name;
                                ass.LargeImageKey = !string.IsNullOrWhiteSpace(bigKeyBox.Text) ? bigKeyBox.Text : string.Format("0{0:x}", title.Tid);
                                presence.Details = $"Playing {title.Name}";
                            }

                            presence.Assets = ass;
                            if (checkTime.Checked) presence.Timestamps = time;
                            rpc.SetPresence(presence);

                            ManualUpdate = false;
                            LastGame = title.Name;
                        }
                    }
                    else
                    {
                        if (rpc != null && !rpc.IsDisposed) rpc.ClearPresence();
                        client.Close();
                        return;
                    }
                }
                catch (SocketException)
                {
                    if (rpc != null && !rpc.IsDisposed) rpc.ClearPresence();
                    client.Close();
                    return;
                }
            }
        }

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
            if (rpc != null && !rpc.IsDisposed)
            {
                rpc.ClearPresence();
                rpc.Dispose();
            }

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

        private void MacButton_Click(object sender, EventArgs e)
        {
            string macAddress = Utils.GetMacByIp(ipAddress.ToString());
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

        private void UpdateStatus(string text, Color color)
        {
            MethodInvoker inv = () =>
            {
                statusLabel.Text = text;
                statusLabel.ForeColor = color;
            };
            Invoke(inv);
        }

        private void ToggleMacButton(bool enable)
        {
            MethodInvoker inv = () =>
            {
                macButton.Enabled = enable;
            };
            Invoke(inv);
        }

        private void CheckTime_CheckedChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void BigKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void SKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void StateBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void BigTextBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void TrayExitMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start($"https://discordapp.com/developers/applications/{clientBox.Text}");
    }
}
