using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.SubBot;
using MasterBot.Room;
using MasterBot.Network;

namespace MasterBot
{
    public interface IBot
    {
        bool LoggedIn { get; }
        bool Connected { get; }
        ISubBotHandler SubBotHandler { get; }
        IMainForm MainForm { get; }
        PIOCon Connection { get; }
        IRoom Room { get; }

        void Login(string game, string email, string password);
        bool Connect(string roomId);
        void Disconnect(string reason);
        void onDisconnect(object sender, string reason);
        void onMessage(object sender, Message m);
    }
}
