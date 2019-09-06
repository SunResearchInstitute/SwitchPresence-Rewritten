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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SwitchPresence_Rewritten
{
    public partial class MainForm : Form
    {
        Thread listenThread;
        static Socket client;
        static DiscordRpcClient rpc;
        bool ManualUpdate = false;
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
                if (!IPAddress.TryParse(ipBox.Text, out IPAddress ip))
                {
                    Show();
                    Activate();
                    UpdateStatus("Invalid IP", Color.DarkRed);
                    SystemSounds.Exclamation.Play();
                    return;
                }
                if (string.IsNullOrWhiteSpace(clientBox.Text))
                {
                    Show();
                    Activate();
                    UpdateStatus("Client ID cannot be empty", Color.DarkRed);
                    SystemSounds.Exclamation.Play();
                    return;
                }

                listenThread.Start();

                connectButton.Text = "Disconnect";
                connectToolStripMenuItem.Text = "Disconnect";
                ipBox.Enabled = false;
                clientBox.Enabled = false;
            }
            else
            {
                if (rpc != null && !rpc.IsDisposed)
                {
                    rpc.SetPresence(null);
                    rpc.Dispose();
                }

                if (client != null) client.Close();
                listenThread.Abort();
                listenThread = new Thread(TryConnect);
                UpdateStatus("", Color.Gray);
                connectButton.Text = "Connect";
                connectToolStripMenuItem.Text = "Connect";
                trayIcon.Icon = Resources.Disconnected;
                trayIcon.Text = "SwitchPresence (Disconnected)";
                ipBox.Enabled = true;
                clientBox.Enabled = true;
            }
        }

        private void TryConnect()
        {
            IPAddress.TryParse(ipBox.Text, out IPAddress ip);
            IPEndPoint localEndPoint = new IPEndPoint(ip, 0xCAFE);
            while (true)
            {
                client = new Socket(SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveTimeout = 5500
                };
                IAsyncResult result = client.BeginConnect(localEndPoint, null, null);

                UpdateStatus("Attemping to connect to server...", Color.Gray);
                trayIcon.Text = "SwitchPresence (Connecting...)";

                bool success = result.AsyncWaitHandle.WaitOne(2000, true);
                if (!success)
                {
                    //UpdateStatus("Could not connect to Server! Retrying...", Color.DarkRed);
                    client.Close();
                }
                else
                {
                    client.EndConnect(result);
                    try
                    {
                        StartListening();
                    }
                    catch (SocketException)
                    {
                        client.Close();
                        if (rpc != null && !rpc.IsDisposed) rpc.Dispose();
                    }
                }
            }
        }

        private void StartListening()
        {
            rpc = new DiscordRpcClient(clientBox.Text);
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
            rpc.Initialize();
            DataListen();
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

        private void DataListen()
        {
            string LastGame = "";
            Timestamps time = null;
            while (true)
            {
                try
                {
                    byte[] bytes = new byte[800];
                    int cnt = client.Receive(bytes);
                    UpdateStatus("Connected to the server!", Color.Green);
                    trayIcon.Icon = Resources.Connected;
                    trayIcon.Text = "SwitchPresence (Connected)";
                    TitlePacket title = Utils.ByteArrayToStructure<TitlePacket>(bytes);
                    if (title.magic == 0xffaadd23)
                    {
                        if ((rpc != null && rpc.CurrentPresence == null) || LastGame != title.name)
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
                }
                catch (SocketException)
                {
                    rpc.Dispose();
                    client.Close();
                    return;
                }
            }
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
                ipBox.Text = cfg.IP;
                stateBox.Text = cfg.State;
                clientBox.Text = cfg.Client;
                checkTray.Checked = cfg.AllowTray;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            listenThread.Abort();
            try
            {
                client.Close();
            }
            catch { }
            if (rpc != null && !rpc.IsDisposed)
                rpc.Dispose();
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
                    IP = ipBox.Text,
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
    }

    public class Config
    {
        public string IP, Client, BigKey, BigText, SmallKey, State;
        public bool DisplayTimer, AllowTray;

        public Config() { }
    }
}
