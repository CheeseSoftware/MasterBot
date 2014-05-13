using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public interface ISubBotHandler : ISubBot
    {
        void AddSubBot(ASubBot subBot, bool enabledByDefault = true);
        void RemoveSubBot(string name);
        ASubBot GetSubBot(string name);
        Dictionary<string, ASubBot> SubBots { get; }
    }
}
