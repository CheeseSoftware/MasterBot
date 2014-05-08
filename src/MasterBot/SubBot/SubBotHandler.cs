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

        public void onConnect(IBot bot)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.onConnect(bot);
            }
        }

        public void onDisconnect(IBot bot, string reason)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.onDisconnect(bot, reason);
            }
        }

        public void onMessage(IBot bot, PlayerIOClient.Message m)
        {
            foreach(ISubBot subBot in subBots.Values)
            {
                subBot.onMessage(bot, m);
            }
        }

        public void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.onCommand(bot, cmd, args, cmdSource);
            }
        }

        public void Update(IBot bot)
        {
            foreach (ISubBot subBot in subBots.Values)
            {
                subBot.Update(bot);
            }
        }

    }
}
