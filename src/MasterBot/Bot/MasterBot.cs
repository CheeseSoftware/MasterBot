using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasterBot.SubBot;
using PlayerIOClient;

namespace MasterBot
{
    class MasterBot : IBot
    {
        private Timer updateTimer = new Timer();

        public MainForm mainForm;
        public SubBotHandler subBotHandler;
        public Client client;
        public Connection connection;

        public bool LoggedIn { get { return client != null; } }
        public bool Connected { get { return connection != null && connection.Connected; } }

        public MasterBot()
        {
            subBotHandler = new SubBotHandler();
            Application.Run(mainForm = new MainForm());

            updateTimer.Interval = 50;
            updateTimer.Tick += updateTimer_Tick;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            subBotHandler.Update(this);
        }

        public void Login(string game, string email, string password)
        {
            PlayerIO.QuickConnect.SimpleConnect(
                "everybody-edits-su9rn58o40itdbnw69plyw",
                email,
                password,
                delegate(Client tempClient)
                {
                    client = tempClient;
                },
                delegate(PlayerIOError tempError)
                {
                    MessageBox.Show(tempError.ToString());
                });
        }

        public void Connect(string roomId)
        {
            client.Multiplayer.JoinRoom(
                roomId, 
                null,
                delegate(PlayerIOClient.Connection tempConnection)
                {
                    connection = tempConnection;
                    connection.AddOnDisconnect(new DisconnectEventHandler(onDisconnect));
                    connection.AddOnMessage(new MessageReceivedEventHandler(onMessage));
                },
                delegate(PlayerIOError tempError)
                {
                    MessageBox.Show(tempError.ToString());
                });
        }

        public void Disconnect(string reason)
        {
            if (Connected)
            {
                subBotHandler.onDisconnect(this, reason);
                connection.Disconnect();
            }
        }

        public void onMessage(object sender, PlayerIOClient.Message m)
        {
            subBotHandler.onMessage(this, m);
        }

        public void onDisconnect(object sender, string reason)
        {
            Disconnect(reason);
        }
    }
}
