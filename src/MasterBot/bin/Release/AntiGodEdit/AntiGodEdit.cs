using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot;
using MasterBot.SubBot;
using MasterBot.Room;
using PlayerIOClient;
using System.Threading;
using MasterBot.Room.Block;


namespace AntiGodEdit
{
    public class AntiGodEdit : ASubBot, IPlugin
    {
        public AntiGodEdit() : base(null)
        {
        }

        public void PerformAction(IBot bot)
        {
            this.bot = bot;

            bot.SubBotHandler.AddSubBot(this, false);
        }

        public override bool HasTab
        {
            // This does not need a GUI.
            get { return false; }
        }

        public override string SubBotName
        {
            get { return "AntiGodEdit"; }
        }

        public override void onBlockChange(int x, int y, MasterBot.Room.Block.IBlock newBlock, MasterBot.Room.Block.IBlock oldBlock)
        {
            bool isPlayerGod = newBlock.Placer.IsGod;
            bool isOldPlayerGod = (oldBlock.Placer == null) ? false : oldBlock.Placer.IsGod;

            // We don't like god:
            // If the old block was placed by a god too we should not care.
            if (isPlayerGod && !isOldPlayerGod)
                bot.Room.BlockDrawer.PlaceBlock(new BlockWithPos(x, y, oldBlock));
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
        }

        public override void onConnect()
        {
        }

        public override void onDisable()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onEnable()
        {
        }

        public override void onMessage(Message m)
        {
            switch (m.Type)
            {
                case "god":
                    {
                        int userId = m.GetInt(0);
                        bool isGod = m.GetBoolean(1);

                        // No gods are allowed:
                        if (isGod)
                        {
                            IPlayer player = bot.Room.getPlayer(userId);
                            if (player != null)
                            {
                                string name = player.Name;

                                bot.Connection.Send(Message.Create("say", "/removeedit " + name));
                                Thread.Sleep(10);
                                bot.Connection.Send(Message.Create("say", "/kill " + name));
                                bot.Connection.Send(Message.Create("say", "/giveedit " + name));
                            }
                        }
                    }
                    break;
            }
        }

        public override void onTick()
        {
        }
    }
}
