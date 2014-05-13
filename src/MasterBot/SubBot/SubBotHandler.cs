using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot.SubBot
{
    public class SubBotHandler : ISubBot, ISubBotHandler
    {
        Dictionary<string, ASubBot> subBots = new Dictionary<string, ASubBot>();
        TabControl tabControl;

        public SubBotHandler(TabControl tabControl)
        {

        }

        public void AddSubBot(string name, ASubBot subBot)
        {
            if (!subBots.ContainsKey(name))
            {
                lock (subBots)
                {
                    subBots.Add(name, subBot);
                }
            }
        }

        public void RemoveSubBot(string name)
        {
            if (subBots.ContainsKey(name))
            {
                lock (subBots)
                {
                    subBots.Remove(name);
                }
            }
        }

        public ASubBot GetSubBot(string name)
        {
            if (subBots.ContainsKey(name))
            {
                return subBots[name];
            }
            return null;
        }

        public Dictionary<string, ASubBot> SubBots
        {
            get { return subBots; }
        }

        public void onConnect(IBot bot)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onConnect(bot);
                }
            }
        }

        public void onDisconnect(IBot bot, string reason)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onDisconnect(bot, reason);
                }
            }
        }

        public void onMessage(IBot bot, PlayerIOClient.Message m)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onMessage(bot, m);
                }
            }
        }

        public void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onCommand(bot, cmd, args, cmdSource);
                }
            }
        }

        public void Update(IBot bot)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.Update(bot);
                }
            }
        }

        public void onBlockChange(IBot bot, int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onBlockChange(bot, x, y, newBlock, oldBlock);
                }
            }
        }
    }
}
