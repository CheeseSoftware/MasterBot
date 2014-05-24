using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;
using MasterBot.SubBot;
using MasterBot.Room;

namespace MasterBot
{
    public interface IBot : IChatSayer
    {
        bool LoggedIn { get; }
        bool Connected { get; }
        ISubBotHandler SubBotHandler { get; }
        IMainForm MainForm { get; }
        Client Client { get; }
        Connection Connection { get; }
        IRoom Room { get; }

        void Login(string game, string email, string password);
        bool Connect(string roomId);
        void Disconnect(string reason);
        void onDisconnect(object sender, string reason);
        void onMessage(object sender, PlayerIOClient.Message m);
    }
}
