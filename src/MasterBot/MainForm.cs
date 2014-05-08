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

        private void button1_Click(object sender, EventArgs e)
        {
            RtbConsole.WriteLine("%chello! %cHello!", Color.Red, Color.Green);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RtbConsole_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

    }
}
