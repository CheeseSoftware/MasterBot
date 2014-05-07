using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public interface ISubBotHandler
    {
        void AddSubBot(string name, ISubBot subBot);
        void RemoveSubBot(string name);
        ISubBot GetSubBot(string name);
        Dictionary<string, ISubBot> SubBots { get; }

        void onCommand(MasterBot masterBot, string cmd, string[] args, ICmdSource cmdSource);
    }
}
