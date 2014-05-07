using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public interface ISubBot
    {
        void onConnect(MasterBot masterBot);
        void onDisconnect(MasterBot masterBot, string reason);
        void onMessage(MasterBot masterBot, PlayerIOClient.Message m);
        void onCommand(MasterBot masterBot, string cmd, string[] args, ICmdSource cmdSource);
        void Update(MasterBot masterBot);
    }
}
