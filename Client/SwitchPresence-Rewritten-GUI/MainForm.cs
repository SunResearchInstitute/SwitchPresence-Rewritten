using DiscordRPC;
#if DEBUG
using DiscordRPC.Logging;
#endif
using Newtonsoft.Json;
using PresenceCommon.Types;
using SwitchPresence_Rewritten_GUI.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace SwitchPresence_Rewritten_GUI
{
    public partial class MainForm : Form
    {
        private Thread listenThread;
        private static Socket client;
        private static DiscordRpcClient rpc;
        private IPAddress ipAddress;
        private bool ManualUpdate = false;
        private string LastGame = "";
        private Timestamps time = null;
        private static Timer timer;
        private Config config;

        public MainForm(Config cfg)
        {
            InitializeComponent();
            listenThread = new Thread(TryConnect);
            config = cfg;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (connectButton.Text == "Connect")
            {
                // Check and see if ClientID is empty
                if (string.IsNullOrWhiteSpace(clientBox.Text))
                {
                    Show();
                    Activate();
                    UpdateStatus("Client ID cannot be empty", Color.DarkRed);
                    SystemSounds.Exclamation.Play();
                    return;
                }

                // Check and see if we have an IP
                // If we have an IP, prompt to swap to MAC Address
                if (IPAddress.TryParse(addressBox.Text, out ipAddress))
                {
                    if (!config.SeenAutoMacPrompt)
                    {
                        config.SeenAutoMacPrompt = true;

                        string message = "We've detected that you're using an IP to connect to your Switch. Connecting via MAC address may make it easier to reconnect to your device in case the IP changes." +
                                         "\n\nWould you like to swap to connecting via MAC address? \n(We'll only ask this once.)";

                        if (MessageBox.Show(message, "IP Detected", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            UseMacDefault.Checked = true;
                            IpToMac();
                        }
                        else
                            UseMacDefault.Checked = false;
                    }
                    else if (UseMacDefault.Checked == true)
                        IpToMac();
                }
                else
                {
                    // If in this block, means we dont have a valid IP.
                    // Check and see if it's a MAC Address
                    try
                    {
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

                // Check to see if we obtained a valid IP
                if (ipAddress == null)
                {
                    Show();
                    Activate();
                    UpdateStatus("Device with MAC wasn't found!", Color.DarkRed);
                    SystemSounds.Exclamation.Play();
                    return;
                }

                listenThread.Start();

                connectButton.Text = "Disconnect";
                connectToolStripMenuItem.Text = "Disconnect";

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

                ipAddress = null;
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

                    Title title = new Title(bytes);
                    if (title.Magic == 0xffaadd23)
                    {
                        if (LastGame != title.Name)
                        {
                            time = Timestamps.Now;
                        }
                        if ((LastGame != title.Name) || ManualUpdate)
                        {
                            if (rpc != null)
                            {
                                if (checkMainMenu.Checked == false && title.Name == "NULL")
                                    rpc.ClearPresence();
                                else
                                    rpc.SetPresence(PresenceCommon.Utils.CreateDiscordPresence(title, time, bigKeyBox.Text, bigTextBox.Text, smallKeyBox.Text, stateBox.Text));
                            }
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

        private void IpToMac()
        {
            string macAddress = Utils.GetMacByIp(ipAddress.ToString());
            if (macAddress != null)
                addressBox.Text = macAddress;
            else
                MessageBox.Show("Can't convert to MAC Address! Sorry!");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // load data from config
            addressBox.Text = config.IP;
            clientBox.Text = config.Client;
            bigKeyBox.Text = config.BigKey;
            bigTextBox.Text = config.BigText;
            smallKeyBox.Text = config.SmallKey;
            stateBox.Text = config.State;
            checkTime.Checked = config.DisplayTimer;
            checkTray.Checked = config.AllowTray;
            checkMainMenu.Checked = config.DisplayMainMenu;
            UseMacDefault.Checked = config.AutoToMac;
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
                if (timer != null) timer.Dispose();

                // make sure config is saved
                config.IP = addressBox.Text;
                config.Client = clientBox.Text;
                config.BigKey = bigKeyBox.Text;
                config.BigText = bigTextBox.Text;
                config.SmallKey = smallKeyBox.Text;
                config.State = stateBox.Text;
                config.DisplayTimer = checkTime.Checked;
                config.AllowTray = checkTray.Checked;
                config.DisplayMainMenu = checkMainMenu.Checked;
                config.AutoToMac = UseMacDefault.Checked;
                config.saveConfig();
            }
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            Activate();
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

        private void CheckTime_CheckedChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void BigKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void SKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void StateBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void BigTextBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void TrayExitMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start($"https://discordapp.com/developers/applications/{clientBox.Text}");

        private void CheckMainMenu_CheckedChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void UseMacDefault_CheckedChanged(object sender, EventArgs e) => config.SeenAutoMacPrompt = true;
    }
}
