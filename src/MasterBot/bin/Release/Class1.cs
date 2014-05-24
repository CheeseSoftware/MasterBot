using MasterBot;
using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class Class1 : ASubBot, IPlugin
    {
        public override bool HasTab
        {
            get { return false; }
        }

        public override string SubBotName
        {
            get { "[NoName]"; }
        }

        public Class1() : base(null) {  }

        public void PerformAction(IBot bot)
        {
            // This is required since I can't fix this in the constructor. :/
            this.bot = bot;

            // Adds the SubBot to the SubBot handler so the SubBot will get callbacks.
            bot.SubBotHandler.AddSubBot(this, true);

            if (this.HasTab)
                this.InitializeComponent();

            // Enable tick with an interval of 500ms. It will call onTick() every tick.
            //this.EnableTick(500)
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
        }
    }
}
