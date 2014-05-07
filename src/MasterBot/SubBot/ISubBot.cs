using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    interface ISubBot
    {
        void onConnect(MasterBot masterBot);
        void onDisconnect(MasterBot masterBot, string reason);
        void onMessage(MasterBot masterBot, PlayerIOClient.Message m);
        void onCommand(MasterBot masterBot, string cmd, string[] args, Player player);
        void Update(MasterBot masterBot);
    }
}
