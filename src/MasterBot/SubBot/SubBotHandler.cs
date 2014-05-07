using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public class SubBotHandler : ISubBot, ISubBotHandler
    {
        Dictionary<string, ISubBot> subBots = new Dictionary<string, ISubBot>();

        public SubBotHandler()
        {

        }

        public void AddSubBot(string name, ISubBot subBot)
        {
            if (!subBots.ContainsKey(name))
            {
                subBots.Add(name, subBot);
            }
        }

        public void RemoveSubBot(string name)
        {
            if (subBots.ContainsKey(name))
            {
                subBots.Remove(name);
            }
        }

        public ISubBot GetSubBot(string name)
        {
            if (subBots.ContainsKey(name))
            {
                return subBots[name];
            }
            return null;
        }

        public Dictionary<string, ISubBot> SubBots
        {
            get { return subBots; }
        }

        public void onConnect(MasterBot masterBot)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.onConnect(masterBot);
            }
        }

        public void onDisconnect(MasterBot masterBot, string reason)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.onDisconnect(masterBot, reason);
            }
        }

        public void onMessage(MasterBot masterBot, PlayerIOClient.Message m)
        {
            foreach(ISubBot subBot in subBots.Values)
            {
                subBot.onMessage(masterBot, m);
            }
        }

        public void onCommand(MasterBot masterBot, string cmd, string[] args, Player player)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.onCommand(masterBot, cmd, args, player);
            }
        }

        public void Update(MasterBot masterBot)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.Update(masterBot);
            }
        }

    }
}
