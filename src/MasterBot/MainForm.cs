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
    public partial class MainForm : Form
    {
        private bool loggingIn = false;
        private bool connecting = false;

        public MasterBot masterBot;

        public MainForm(MasterBot masterBot)
        {
            this.masterBot = masterBot;
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (!loggingIn)
            {
                loggingIn = true;
                masterBot.Login("everybody-edits-su9rn58o40itdbnw69plyw", textBoxEmail.Text, textBoxPassword.Text);
                buttonLogin.Enabled = false;
                buttonLogin.Text = "Logging in..";
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!connecting)
            {
                if (!masterBot.Connected)
                {
                    connecting = true;
                    masterBot.Connect(textBoxRoomId.Text);
                    buttonConnect.Enabled = false;
                    buttonConnect.Text = "Connecting..";
                }
                else
                {
                    masterBot.Disconnect("Exited");
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
            if(success)
            {
                buttonConnect.Text = "Connected";
            }
            else
            {
                buttonConnect.Text = "Failed";
            }
            connecting = false;
        }

    }
}
