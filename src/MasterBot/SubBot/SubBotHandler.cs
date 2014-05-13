using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot.SubBot
{
    public class SubBotHandler : ISubBotHandler
    {
        private IBot bot;
        private Dictionary<string, ASubBot> subBots = new Dictionary<string, ASubBot>();
        private TabControl tabControl;

        public SubBotHandler(IBot bot, TabControl tabControl)
        {
            this.bot = bot;
            this.tabControl = tabControl;
        }

        private void AddTab(ASubBot subBot)
        {
            if (subBot.HasTab)
                tabControl.Invoke(new Action(() => { tabControl.TabPages.Add(subBot); }));
        }

        private void RemoveTab(ASubBot subBot)
        {
            if (tabControl.TabPages.Contains(subBot))
                tabControl.Invoke(new Action(() => { tabControl.TabPages.Remove(subBot); }));
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

        public void onConnect()
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onConnect();
                }
            }
        }

        public void onDisconnect(string reason)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onDisconnect(reason);
                }
            }
        }

        public void onMessage(PlayerIOClient.Message m)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onMessage(m);
                }
            }
        }

        public void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onCommand(cmd, args, cmdSource);
                }
            }
        }

        public void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onBlockChange(x, y, newBlock, oldBlock);
                }
            }
        }

        public void onTick()
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    subBot.onTick();
                }
            }
        }
    }
}
