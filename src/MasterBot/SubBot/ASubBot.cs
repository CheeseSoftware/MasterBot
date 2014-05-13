using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot.SubBot
{
    public abstract class ASubBot : TabPage, ISubBot
    {
        protected IBot bot;
        private bool enabled = false;
        private System.Timers.Timer updateTimer;

        public ASubBot(IBot bot)
        {
            this.bot = bot;
            this.updateTimer = new System.Timers.Timer();
            updateTimer.Elapsed += delegate { onTick(); };
        }

        public abstract void onEnable();
        public abstract void onDisable();
        public abstract void onConnect();
        public abstract void onDisconnect(string reason);
        public abstract void onMessage(PlayerIOClient.Message m);
        public abstract void onCommand(string cmd, string[] args, ICmdSource cmdSource);
        public abstract void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock);
        public abstract void onTick();

        public abstract bool HasTab { get; }
        public new bool Enabled { get { return enabled; } 
            set 
            { 
                enabled = value;
                if (value)
                    onEnable();
                else
                {
                    onDisable();
                    DisableTick();
                }
                bot.MainForm.Console("Subbot " + Name + (value ? " enabled." : " disabled."));
            } 
        }

        public abstract new string Name { get; }

        protected void EnableTick(double interval)
        {
            updateTimer.Interval = interval;
            updateTimer.Start();
        }

        protected void DisableTick()
        {
            updateTimer.Stop();
        }

    }
}
