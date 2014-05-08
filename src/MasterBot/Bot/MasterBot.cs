using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasterBot.SubBot;
using MasterBot.Room;
using PlayerIOClient;
using MasterBot.Movement;

namespace MasterBot
{
    public class MasterBot : IBot
    {
        private Timer updateTimer = new Timer();
        private MainForm mainForm;
        private SubBotHandler subBotHandler;
        private Client client;
        private Connection connection;
        private Room.IRoom room;

        public bool LoggedIn { get { return client != null; } }
        public bool Connected { get { return connection != null && connection.Connected; } }
        public ISubBotHandler SubBotHandler
        {
            get { return subBotHandler; }
        }

        public MainForm MainForm
        {
            get { return mainForm; }
        }

        public Client Client
        {
            get { return client; }
        }

        public Connection Connection
        {
            get { return connection; }
        }

        public IRoom Room
        {
            get { return room; }
        }

        public MasterBot()
        {
            subBotHandler = new SubBotHandler();
            subBotHandler.AddSubBot("Room", (ISubBot)(room = new Room.Room(this)));
            Application.Run(mainForm = new MainForm(this));

            updateTimer.Interval = 50;
            updateTimer.Tick += updateTimer_Tick;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            subBotHandler.Update(this);
        }

        public void onMessage(object sender, PlayerIOClient.Message m)
        {
            subBotHandler.onMessage(this, m);
        }

        public void onDisconnect(object sender, string reason)
        {
            Disconnect(reason);
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
                    mainForm.Invoke(new Action(() => { mainForm.onLoginFinished(true); }));
                },
                delegate(PlayerIOError tempError)
                {
                    mainForm.Invoke(new Action(() => { mainForm.onLoginFinished(false); }));
                    MessageBox.Show(tempError.ToString());
                });
        }

        public bool Connect(string roomId)
        {
            if (LoggedIn)
            {
                client.Multiplayer.JoinRoom(
                    roomId,
                    null,
                    delegate(PlayerIOClient.Connection tempConnection)
                    {
                        connection = tempConnection;
                        connection.Send("init");
                        connection.AddOnDisconnect(new DisconnectEventHandler(onDisconnect));
                        connection.AddOnMessage(new MessageReceivedEventHandler(onMessage));
                        mainForm.Invoke(new Action(() => { mainForm.onConnectFinished(true); }));
                    },
                    delegate(PlayerIOError tempError)
                    {
                        mainForm.Invoke(new Action(() => { mainForm.onConnectFinished(false); }));
                        MessageBox.Show(tempError.ToString());
                    });
                return true;
            }
            return false;
        }

        public void Disconnect(string reason)
        {
            if (Connected)
                connection.Disconnect();
            subBotHandler.onDisconnect(this, reason);
        }
    }
}
