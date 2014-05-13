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
using MasterBot.Minimap;
using MasterBot.SubBot.WorldEdit;
using System.Threading;

namespace MasterBot
{
    public class MasterBot : IBot
    {
        private MainForm mainForm;
        private SubBotHandler subBotHandler;
        private Client client;
        private Connection connection;
        private Room.IRoom room;

        public bool LoggedIn { get { return client != null; } }
        public bool Connected { get { return connection != null && connection.Connected; } }

        public MasterBot()
        {
            MinimapColors.CreateColorCodes();

            mainForm = new MainForm(this);
            mainForm.FormClosing += delegate
            {
                Disconnect("Form Closing");
            };
            new Thread(() => { Application.Run(mainForm); }).Start();

<<<<<<< HEAD
            subBotHandler = new SubBotHandler(this, mainForm.BotTabPage);
            subBotHandler.AddSubBot((ASubBot)(room = new Room.Room(this)));
            subBotHandler.AddSubBot(new BlockPlaceTest(this));
            subBotHandler.AddSubBot(new Commands(this));
            subBotHandler.AddSubBot(new WorldEdit(this));
=======
            MinimapColors.CreateColorCodes();
            subBotHandler = new SubBotHandler(mainForm.BotTabPage);
            subBotHandler.AddSubBot("Room", (ASubBot)(room = new Room.Room(this)));
            subBotHandler.AddSubBot("BlockPlaceTest", new BlockPlaceTest());
            subBotHandler.AddSubBot("Commands", new Commands());
            subBotHandler.AddSubBot("WorldEdit", new WorldEdit(this));

            updateTimer.Interval = 50;
            updateTimer.Tick += updateTimer_Tick;
            Application.Run(mainForm);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            subBotHandler.Update(this);
>>>>>>> origin/Ost
        }

        public void onMessage(object sender, PlayerIOClient.Message m)
        {
            subBotHandler.onMessage(m);
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
                        subBotHandler.onConnect();
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
            subBotHandler.onDisconnect(reason);
        }

        public SubBotHandler SubBotHandler
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
    }
}
