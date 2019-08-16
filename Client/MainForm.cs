using DiscordRPC;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SwitchPresence_Rewritten
{
    public partial class MainForm : Form
    {
        Thread t;
        static Socket client;
        static DiscordRpcClient rpc;
        bool ManualUpdate = false;
        public MainForm()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            t = new Thread(DataListen);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            if (rpc != null)
                if (!rpc.IsDisposed)
                    rpc.Dispose();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Connect")
            {
                if (!IPAddress.TryParse(ipBox.Text, out IPAddress ip))
                {
                    MessageBox.Show("Invalid IP", "IP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrWhiteSpace(clientBox.Text))
                {
                    MessageBox.Show("Client ID cannot be empty", "Client ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                client = new Socket(SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localEndPoint = new IPEndPoint(ip, 0xCAFE);
                IAsyncResult result = client.BeginConnect(localEndPoint, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(2000, true);
                if (!success)
                {
                    MessageBox.Show("Could not connect to Server!", "Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    client.EndConnect(result);
                }
                button1.Text = "Disconnect";

                rpc = new DiscordRpcClient(clientBox.Text);
                rpc.Initialize();
                t.Start();
            }
            else
            {
                rpc.Dispose();
                t.Abort();
                t = new Thread(DataListen);
                client.Close();
                //small workaround for race condition on reconnect :(
                Thread.Sleep(800);
                button1.Text = "Connect";
            }
        }

        private void DataListen()
        {
            string LastGame= "";
            Timestamps time = null;
            while (true)
            {
                try
                {
                    byte[] bytes = new byte[800];
                    int cnt = client.Receive(bytes);
                    TitlePacket title = Utils.ByteArrayToStructure<TitlePacket>(bytes);
                    if (rpc.CurrentPresence == null || LastGame != title.name)
                    {
                        time = Timestamps.Now;
                    }
                    if (title.magic == 0xffaadd23 && (rpc.CurrentPresence == null || LastGame != title.name) || ManualUpdate)
                    {
                        
                        //ManualUpdate needs to be put somewhere
                        if (title.name == "NULL")
                        {
                            Assets ass = new Assets()
                            {
                                LargeImageText = "Home Menu",
                                SmallImageKey = smallKeyBox.Text,
                                SmallImageText = "Switch-Presence Rewritten"
                            };

                            if (!string.IsNullOrWhiteSpace(bigTextBox.Text))
                                ass.LargeImageText = bigTextBox.Text;
                            else
                                ass.LargeImageText = "Home Menu";

                            if (!string.IsNullOrWhiteSpace(bigKeyBox.Text))
                                ass.LargeImageKey = bigKeyBox.Text;
                            else
                                ass.LargeImageKey = string.Format("0{0:x}", 0x0100000000001000);

                            RichPresence presence = new RichPresence()
                            {

                                Details = "In the home menu",
                                State = stateBox.Text,
                                Assets = ass
                            };

                            if (checkTime.Checked) presence.Timestamps = time;
                            rpc.SetPresence(presence);
                            ManualUpdate = false;
                        }
                        else
                        {
                            Assets ass = new Assets()
                            {
                                SmallImageKey = smallKeyBox.Text,
                                SmallImageText = "Switch-Presence Rewritten"
                            };

                            if (!string.IsNullOrWhiteSpace(bigTextBox.Text))
                                ass.LargeImageText = bigTextBox.Text;
                            else
                                ass.LargeImageText = title.name;

                            if (!string.IsNullOrWhiteSpace(bigKeyBox.Text))
                                ass.LargeImageKey = bigKeyBox.Text;
                            else
                                ass.LargeImageKey = string.Format("0{0:x}", title.tid);

                            RichPresence presence = new RichPresence()
                            {

                                Details = $"Playing {title.name}",
                                State = stateBox.Text,
                                Assets = ass
                            };

                            if (checkTime.Checked) presence.Timestamps = time;
                            rpc.SetPresence(presence);
                            ManualUpdate = false;
                        }
                        LastGame = title.name;
                        Config cfg = new Config()
                        {
                            ip = ipBox.Text,
                            client = clientBox.Text,
                            bigKey = bigKeyBox.Text,
                            smallKey = smallKeyBox.Text,
                            state = stateBox.Text,
                            bigText = bigTextBox.Text,
                            timer = checkTime.Checked
                        };
                        File.WriteAllText("Config.json", JsonConvert.SerializeObject(cfg));
                    }
                }
                catch (SocketException)
                {
                    client.Close();
                    rpc.Dispose();
                    t = new Thread(DataListen);
                    MessageBox.Show("A socket error occured please try again.", "Socket", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MethodInvoker inv = () => button1.Text = "Connect";
                    Invoke(inv);
                    return;
                }
            }
        }

        private void CheckTime_CheckedChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start($"https://discordapp.com/developers/applications/{clientBox.Text}");

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

        private void BigKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void SKeyBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void StateBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void BigTextBox_TextChanged(object sender, EventArgs e) => ManualUpdate = true;

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists("Config.json"))
            {
                Config cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
                checkTime.Checked = cfg.timer;
                bigKeyBox.Text = cfg.bigKey;
                bigTextBox.Text = cfg.bigText;
                smallKeyBox.Text = cfg.smallKey;
                ipBox.Text = cfg.ip;
                stateBox.Text = cfg.state;
                clientBox.Text = cfg.client;
            }
        }
    }

    public class Config
    {
        public string ip;
        public string client;
        public string bigKey;
        public string bigText;
        public string smallKey;
        public string state;
        public bool timer;

        public Config() { }
    }
}
