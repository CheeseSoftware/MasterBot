using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot.SubBot
{
    public class SubBotHandler : ASubBot, ISubBotHandler
    {
        private Dictionary<string, ASubBot> subBots = new Dictionary<string, ASubBot>();
        private TabControl tabControl;

        public SubBotHandler(IBot bot, TabControl tabControl)
            : base(bot)
        {
            this.tabControl = tabControl;
        }

        private void AddTab(ASubBot subBot)
        {
            if (subBot.HasTab)
                SafeInvoke.Invoke(tabControl, new Action(() => { subBot.Text = subBot.SubBotName; tabControl.TabPages.Add(subBot); }));
        }

        private void RemoveTab(ASubBot subBot)
        {
            if (tabControl.TabPages.Contains(subBot))
                SafeInvoke.Invoke(tabControl, new Action(() => { tabControl.TabPages.Remove(subBot); }));
        }

        public void AddSubBot(ASubBot subBot, bool enabledByDefault = true)
        {
            if (!subBots.ContainsKey(subBot.SubBotName))
            {
                lock (subBots)
                {
                    subBots.Add(subBot.SubBotName, subBot);
                    AddTab(subBot);
                    if (enabledByDefault)
                        subBot.Enabled = true;
                    bot.MainForm.UpdateSubbotsDatasource(subBots);
                    bot.MainForm.Console("Subbot " + subBot.SubBotName + " added.");
                }
            }
        }

        public void RemoveSubBot(string name)
        {
            if (subBots.ContainsKey(name))
            {
                lock (subBots)
                {
                    subBots[name].Enabled = false;
                    RemoveTab(subBots[name]);
                    subBots.Remove(name);
                    bot.MainForm.UpdateSubbotsDatasource(subBots);
                    bot.MainForm.Console("Subbot " + name + " removed.");
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

        public override void onEnable()
        {
        }

        public override void onDisable()
        {
        }

        public override void onConnect()
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    if (subBot.Enabled)
                        subBot.onConnect();
                }
            }
        }

        public override void onDisconnect(string reason)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    if (subBot.Enabled)
                        subBot.onDisconnect(reason);
                }
            }
        }

        public override void onMessage(Network.Message m)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    if (subBot.Enabled)
                        subBot.onMessage(m);
                }
            }
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    if (subBot.Enabled)
                        subBot.onCommand(cmd, args, cmdSource);
                }
            }
        }

        public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            lock (subBots)
            {
                foreach (ASubBot subBot in subBots.Values)
                {
                    if (subBot.Enabled)
                        subBot.onBlockChange(x, y, newBlock, oldBlock);
                }
            }
        }

        public override void onTick()
        {
        }

        public override bool HasTab
        {
            get { return true; }
        }

        public override string SubBotName
        {
            get { return "SubBotHandler"; }
        }
    }
}
