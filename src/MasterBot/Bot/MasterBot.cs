﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasterBot.SubBot;
using MasterBot.Room;
using PlayerIOClient;

namespace MasterBot
{
    public class MasterBot : IBot
    {
        private Timer updateTimer = new Timer();

        public MainForm mainForm;
        public SubBotHandler subBotHandler;
        public Client client;
        public Connection connection;
        public Room.Room room;

        public bool LoggedIn { get { return client != null; } }
        public bool Connected { get { return connection != null && connection.Connected; } }

        public MasterBot()
        {
            subBotHandler = new SubBotHandler();
            subBotHandler.AddSubBot("Room", room = new Room.Room());
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

        public void Connect(string roomId)
        {
            client.Multiplayer.JoinRoom(
                roomId, 
                null,
                delegate(PlayerIOClient.Connection tempConnection)
                {
                    connection = tempConnection;
                    connection.Send("init");
                    mainForm.Invoke(new Action(() => { mainForm.onConnectFinished(true); }));
                    connection.AddOnDisconnect(new DisconnectEventHandler(onDisconnect));
                    connection.AddOnMessage(new MessageReceivedEventHandler(onMessage));
                },
                delegate(PlayerIOError tempError)
                {
                    mainForm.Invoke(new Action(() => { mainForm.onConnectFinished(false); }));
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
    }
}