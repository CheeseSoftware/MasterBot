using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot
{
    public partial class MainForm : Form, IMainForm
    {
        private Dictionary<string, KeyValuePair<string, Queue<string>>> accounts = new Dictionary<string, KeyValuePair<string, Queue<string>>>();
        private bool loggingIn = false;
        private bool connecting = false;
        private System.Timers.Timer connectTimeoutTimer = new System.Timers.Timer();

        public IBot bot;

        public MainForm(IBot bot)
        {
            InitializeComponent();
            this.bot = bot;
            LoadLogin("login.txt");
            /*System.Timers.Timer roomUpdateTimer = new System.Timers.Timer();
            roomUpdateTimer.Interval = 50;
            roomUpdateTimer.Elapsed += delegate
            {
                try
                {
                    if (bot.Room != null)
                    {
                        this.Invoke(new Action(() =>
                        {
                            blocksSentNumericUpDown.Value = bot.Room.BlocksSentSize;
                            blocksToSendNumericUpDown.Value = bot.Room.BlocksToSendSize;
                        }));
                    }
                }
                catch { }
            };
            roomUpdateTimer.Start();*/
        }

        private void LoadLogin(string file)
        {
            comboBoxEmail.Items.Clear();
            comboBoxEmail.Text = "";
            textBoxPassword.Text = "";
            comboBoxRoomId.Items.Clear();
            comboBoxRoomId.Text = "";
            if (accounts.Count > 0)
                accounts.Clear();
            string email = "";
            string password = "";
            Queue<string> roomIds = new Queue<string>();

            try
            {
                if (!File.Exists(file))
                    File.Create(file);

                StreamReader reader = new StreamReader(file);
                string line = "";
                string lineParsed = "";
                while ((line = reader.ReadLine()) != null)
                {
                    lineParsed = line.Replace("\t", "");
                    if (!line.StartsWith("\t"))
                    {
                        if (email != "")
                        {
                            accounts.Add(email, new KeyValuePair<string, Queue<string>>(password, new Queue<string>(roomIds)));
                            email = "";
                            password = "";
                            roomIds.Clear();
                        }
                        comboBoxEmail.Items.Add(lineParsed);
                        email = lineParsed;
                    }
                    else if (line.StartsWith("\t") && !line.StartsWith("\t\t"))
                        password = lineParsed;
                    else if (line.StartsWith("\t\t") && !line.StartsWith("\t\t\t"))
                        roomIds.Enqueue(lineParsed);
                }
                if (email != "")
                {
                    accounts.Add(email, new KeyValuePair<string, Queue<string>>(password, new Queue<string>(roomIds)));
                    email = "";
                    password = "";
                    roomIds.Clear();
                }

            }
            catch { Console("Could not load login data."); }
        }

        #region Connect stuff
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (!loggingIn)
            {
                loggingIn = true;
                bot.Login("everybody-edits-su9rn58o40itdbnw69plyw", comboBoxEmail.Text, textBoxPassword.Text);
                buttonLogin.Enabled = false;
                buttonLogin.Text = "Logging in..";
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!connecting)
            {
                if (!bot.Connected)
                {
                    if (bot.Connect(comboBoxRoomId.Text))
                    {
                        connecting = true;
                        buttonConnect.Enabled = false;
                        connectTimeoutTimer.Interval = 5000;
                        connectTimeoutTimer.Elapsed += delegate { SafeInvoke.Invoke(this, (new Action(() => { onConnectFinished(false); }))); };
                        connectTimeoutTimer.Start();
                        buttonConnect.Text = "Connecting..";
                    }
                }
                else
                {
                    bot.Disconnect("Exited");
                    buttonConnect.Text = "Connect";
                }
            }
        }

        public void onLoginFinished(bool success)
        {
            if (success)
            {
                buttonLogin.Enabled = true;
                buttonLogin.Text = "Success!";
            }
            else
            {
                buttonLogin.Enabled = true;
            }
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(new Action<object, EventArgs>((object sender, EventArgs e) =>
            {
                SafeInvoke.Invoke(buttonLogin, new Action(() =>
                {
                    buttonLogin.Text = "Login";
                    timer.Stop();
                }));
            }));
            timer.Start();
            loggingIn = false;
        }

        public void onConnectFinished(bool success)
        {
            connectTimeoutTimer.Stop();
            buttonConnect.Enabled = true;
            if (success)
            {
                buttonConnect.Text = "Disconnect";
            }
            else
            {
                buttonConnect.Text = "Failed";
                Timer timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += new EventHandler(new Action<object, EventArgs>((object sender, EventArgs e) =>
                {
                    SafeInvoke.Invoke(buttonConnect, (new Action(() =>
                    {
                        buttonConnect.Text = "Connect";
                        buttonConnect.Enabled = true;
                        timer.Stop();
                    })));
                }));
                timer.Start();
                loggingIn = false;
            }
            connecting = false;
        }
        #endregion

        public void UpdateSubbotsDatasource(Dictionary<string, ASubBot> source)
        {
            SafeInvoke.Invoke(checkedListBoxSubBots, new Action(() =>
            {
                checkedListBoxSubBots.Items.Clear();
                foreach (ASubBot subBot in source.Values)
                {
                    checkedListBoxSubBots.Items.Add(subBot.Name, subBot.Enabled);
                }
            }));
        }

        public void Console(string text, params Color[] color)
        {
            RtbConsole.WriteLine(text, color);
        }

        public void UpdateMinimap(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                try
                {
                    Bitmap temp = new Bitmap(bitmap);
                    SafeInvoke.Invoke(pictureBoxMinimap, new Action(() =>
                    {
                        pictureBoxMinimap.Size = temp.Size;
                        pictureBoxMinimap.Image = temp;
                    }));
                }
                catch { }

            }
        }

        private void RtbConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && RtbConsole.Text.Length > 0)
            {
                RtbConsole.WriteLine("%B> %b" + RtbConsoleInput.Text.Replace("%", "%%"));
                string[] input = RtbConsoleInput.Text.Split(' ');
                string cmd = input[0];
                string[] args = new string[0];
                if (input.Length >= 2)
                {
                    args = new string[input.Length - 1];
                    Array.Copy(input, 1, args, 0, input.Length - 1);
                }
                bot.SubBotHandler.onCommand(cmd, args, (ICmdSource)(new ConsoleCmdSource(bot)));
                RtbConsoleInput.Clear();
            }
        }

        private void buttonSendCode_Click(object sender, EventArgs e)
        {
            bot.Connection.Send("access", textBoxCode.Text);
        }

        public TabControl BotTabPage { get { return this.tabControlSubBots; } }

        private void checkedListBoxSubBots_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue != e.CurrentValue)
            {
                if (!(checkedListBoxSubBots.SelectedItem is string))
                    return;
                string name = (string)checkedListBoxSubBots.SelectedItem;
                ASubBot subBot = bot.SubBotHandler.GetSubBot(name);
                if (subBot != null)
                    subBot.Enabled = (e.NewValue == CheckState.Checked) ? true : false;
                else
                    Console("ERROR! subbots checkedlistbox has bugged out.");
            }
        }

        private void comboBoxEmail_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selected = (string)comboBoxEmail.SelectedItem;
            if (accounts.ContainsKey(selected))
            {
                textBoxPassword.Text = accounts[selected].Key;
                if (accounts[selected].Value.Count > 0)
                {
                    foreach (string roomId in accounts[selected].Value)
                        comboBoxRoomId.Items.Add(roomId);
                    comboBoxRoomId.Text = accounts[selected].Value.Dequeue();
                }
            }
        }

    }
}
