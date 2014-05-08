using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public interface ISubBot
    {
        void onConnect(IBot bot);
        void onDisconnect(IBot bot, string reason);
        void onMessage(IBot bot, PlayerIOClient.Message m);
        void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource);
        void Update(IBot bot);
    }
}
