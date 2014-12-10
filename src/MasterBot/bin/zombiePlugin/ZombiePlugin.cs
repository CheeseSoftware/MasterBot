using MasterBot;
using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zombiePlugin
{
    class ZombiePlugin : ASubBot, IPlugin
    {
        public override bool HasTab
        {
            get { return false; }
        }

        public override string SubBotName
        {
            get { "Zombie Plugin"; }
        }

        public ZombiePlugin() : base(null) { }

        public void PerformAction(IBot bot)
        {
            // This is required since I can't fix this in the constructor. :/
            this.bot = bot;

            // Adds the SubBot to the SubBot handler so the SubBot will get callbacks.
            bot.SubBotHandler.AddSubBot(this, true);

            if (this.HasTab)
                this.InitializeComponent();

            // 10ms should the player physics update be.
            this.EnableTick(10);
        }

        public override void onEnable()
        {
            // Initialize as much as possible here.
        }

        public override void onDisable()
        {
            // Clean up
        }

        public override void onConnect()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onBlockChange(int x, int y, MasterBot.Room.Block.IBlock newBlock, MasterBot.Room.Block.IBlock oldBlock)
        {
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
        }

        public override void onTick()
        {
            foreach(var player bot.Room.Players)
            {
                if (isHologram(player))
                {
                    foreach(var player bot.Room.Players)
                    {
                        player.g
                    }
                }
            }
        }
    }
}
