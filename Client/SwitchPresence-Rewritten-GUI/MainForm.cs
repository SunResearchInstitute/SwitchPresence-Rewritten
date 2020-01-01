using DiscordRPC;
#if DEBUG
using DiscordRPC.Logging;
#endif
using PresenceCommon.Types;
using SwitchPresence_Rewritten_GUI.Properties;
using System;
using System.Drawing;
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
        private delegate void SafeCallDelegate(TextBox inbox, string text);
        private Thread listenThread;
        private static Socket client;
        private static DiscordRpcClient rpc;
        private IPAddress ipAddress;
        private bool ManualUpdate = false;
        private string LastGame = "";
        private Timestamps time = null;
        private static Timer timer;
        private Config config;
        private bool hasSeenTrayPrompt = false;
        private Settings settingsMenu;

        public MainForm(Config cfg)
        {
            InitializeComponent();
            listenThread = new Thread(TryConnect);
            config = cfg;
            settingsMenu = new Settings(config);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (connectButton.Text == "Connect")
            {
                // Check and see if ClientID is empty
                if (string.IsNullOrWhiteSpace(config.Client))
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
                                         "\n\nWould you us to automatically convert IPs into MAC addresses for you? (We'll only ask this once.)";

                        if (MessageBox.Show(message, "IP Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            config.AutoToMac = true;
                            IpToMac();
                        }
                        else
                            config.AutoToMac = false;
                    }
                    else if (config.AutoToMac == true)
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
                //largeImageKey.Enabled = false;
                //largeImageText.Enabled = false;
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
                //largeImageKey.Enabled = true;
                //largeImageText.Enabled = true;
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

            rpc = new DiscordRpcClient(config.Client);
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
                                if (!config.DisplayMainMenu && title.Name == "NULL")
                                {
                                    rpc.ClearPresence();
                                }
                                else
                                {
                                    if (config.AllowCustomKeyText)
                                    {
                                        rpc.SetPresence(PresenceCommon.Utils.CreateDiscordPresence(title, config.DisplayTimer ? time : null, largeImageKey.Text, largeImageText.Text, config.SmallKey, stateBox.Text));
                                    } 
                                    else
                                    {
                                        // because of multithreading, we need to use a wrapper to change data on the main form
                                        changeTextFromThread(largeImageKey, $"0{title.Tid:x}");
                                        changeTextFromThread(largeImageText, title.Name);
                                        rpc.SetPresence(PresenceCommon.Utils.CreateDiscordPresence(title, config.DisplayTimer ? time : null, config.BigKey, config.BigText, config.SmallKey, stateBox.Text));
                                    }
                                    
                                }
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

        private void changeTextFromThread(TextBox inbox, string text)
        {
            if (inbox.InvokeRequired)
            {
                var d = new SafeCallDelegate(changeTextFromThread);
                inbox.Invoke(d, new object[] { inbox, text });
            } 
            else
            {
                inbox.Text = text;
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
            stateBox.Text = config.State;
            if (config.AllowCustomKeyText)
            {
                largeImageKey.Text = config.BigKey;
                largeImageText.Text = config.BigText;
            } 
            else
            {
                largeImageKey.Text = largeImageText.Text = "";
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
            if (e.CloseReason == CloseReason.UserClosing && config.AllowTray)
            {
                e.Cancel = true;
                Hide();

                // Prompt user to let them know we're hidden.
                if (!hasSeenTrayPrompt)
                {
                    hasSeenTrayPrompt = true;
                    string message = "SwitchPresence-Rewritten will now run in the tray.\nIn order to quit the app, right-click on the icon in the tray and click Exit.\n\n(You can turn this off in settings.)";
                    MessageBox.Show(message, "Shrinking to Tray", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (timer != null) timer.Dispose();

                // make sure config is saved
                config.IP = addressBox.Text;
                config.State = stateBox.Text;
                if (config.AllowCustomKeyText)
                {
                    config.BigKey = largeImageKey.Text;
                    config.BigText = largeImageText.Text;
                }
                config.saveConfig();
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            ManualUpdate = true;

            if (config.IsFirstRun)
            {
                config.IsFirstRun = false;

                // NOTE: I'm debating making a "First Run" webpage to help guide users to set things up. May improve 
                //       user experience. If that gets set up, we'll probably link that here. -Azure
                string message = "Thanks for using SwitchPresence-Rewritten!\nTo get started, make sure to add your Discord app's Client ID in settings and your Switch's IP is set in the main menu. Then, hit connect and watch your status pop up!";
                if (MessageBox.Show(message, "SwitchPresence-Rewritten", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    // Open settings on "OK" press.
                    settingsMenu.ShowDialog();
                }
            }

            if (config.AllowCustomKeyText)
                largeImageKey.Enabled = largeImageText.Enabled = true;
            else
                largeImageKey.Enabled = largeImageText.Enabled = false;
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            Activate();
        }

        private void toolStripShowApp_Click(object sender, EventArgs e)
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

        private void settingsButton_Click(object sender, EventArgs e)
        {
            settingsMenu.ShowDialog();
        }

        private void StateBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void largeImageText_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void largeImageKey_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void TrayExitMenuItem_Click(object sender, EventArgs e) => Application.Exit();
    }
}
