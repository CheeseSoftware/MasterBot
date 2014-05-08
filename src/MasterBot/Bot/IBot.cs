using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;
using MasterBot.SubBot;

namespace MasterBot
{
    public interface IBot
    {
        bool LoggedIn { get; }
        bool Connected { get; }
        SubBotHandler SubBotHandler { get; }
        MainForm MainForm { get; }
        Client Client { get; }
        Connection Connection { get; }
        Room.IRoom Room { get; }

        void Login(string game, string email, string password);
        bool Connect(string roomId);
        void Disconnect(string reason);
        void onDisconnect(object sender, string reason);
        void onMessage(object sender, PlayerIOClient.Message m);
    }
}
