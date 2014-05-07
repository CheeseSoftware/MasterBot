using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;

namespace MasterBot
{
    public interface IBot
    {
        void Login(string game, string email, string password);
        void Connect(string roomId);
        void Disconnect(string reason);
        void onDisconnect(object sender, string reason);
        void onMessage(object sender, PlayerIOClient.Message m);
    }
}
