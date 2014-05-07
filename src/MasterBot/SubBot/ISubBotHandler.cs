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

        /*public void onConnect(MasterBot masterBot);
        public void onDisconnect(MasterBot masterBot, string reason);
        public void onMessage(MasterBot masterBot, PlayerIOClient.Message m);
        public void onCommand(MasterBot masterBot, string cmd, string[] args, ICmdSource cmdSource);
        public void Update(MasterBot masterBot); Antar vi inte ska ha dessa?*/
    }
}
