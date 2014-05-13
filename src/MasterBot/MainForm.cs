using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot
{
    public partial class MainForm : Form, IMainForm
    {
        private bool loggingIn = false;
        private bool connecting = false;

        public IBot bot;

        public MainForm(IBot bot)
        {
            this.bot = bot;
            InitializeComponent();
            System.Timers.Timer roomUpdateTimer = new System.Timers.Timer();
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
            roomUpdateTimer.Start();
        }
        #region Connect stuff
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (!loggingIn)
            {
                loggingIn = true;
                bot.Login("everybody-edits-su9rn58o40itdbnw69plyw", textBoxEmail.Text, textBoxPassword.Text);
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
                    if (bot.Connect(textBoxRoomId.Text))
                    {
                        connecting = true;
                        buttonConnect.Enabled = false;
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
                buttonLogin.Invoke(new Action(() =>
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
                    buttonConnect.Invoke(new Action(() =>
                    {
                        buttonConnect.Text = "Connect";
                        buttonConnect.Enabled = true;
                        timer.Stop();
                    }));
                }));
                timer.Start();
                loggingIn = false;
            }
            connecting = false;
        }
        #endregion

        public void UpdateSubbotsDatasource(Dictionary<string, ASubBot> source)
        {
            this.Invoke(new Action(() =>
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = source.Keys;
                this.checkedListBoxSubBots.DataSource = bs;
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
                    this.Invoke(new Action(() =>
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
                    Array.Copy(input, 1, args, 0, input.Length - 1);
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

    }
}
