using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using MasterBot.SubBot;
using MasterBot.Room.Block;

namespace MasterBot
{
    public class Commands : ASubBot
    {
        private List<string> disabledPlayers = new List<string>();
        private List<string> protectedPlayers = new List<string>();
        private List<string> getPlacerPlayers = new List<string>();

        public Commands(IBot bot)
            : base(bot)
        {
        }

        public override void onCommand(string cmd, string[] args, ICmdSource sender)
        {
            if (sender is Player)
            {
                Player player = (Player)sender;
                switch (cmd)
                {
                    case "clearworld":
                        {
                            if (bot.Room.IsOwner && player.IsOp)
                                bot.Connection.Send("clear");
                            else
                                sender.Reply("You are not the owner.");
                            break;
                        }
                    case "loadworld":
                        {
                            if (bot.Room.IsOwner && player.IsOp)
                                bot.ChatSayer.Command("/loadlevel");
                            else
                                sender.Reply("You are not the owner.");
                            break;
                        }
                    case "saveworld":
                        {
                            if (bot.Room.IsOwner && player.IsOp)
                                bot.Connection.Send("save");
                            else
                                sender.Reply("You are not the owner.");
                            break;
                        }
                    case "name":
                        {
                            if (bot.Room.IsOwner && player.IsOp)
                            {
                                string name = "";
                                if(args != null && args.Length > 0)
                                {
                                    for (int i = 0; i < args.Length; i++)
                                        name += args[i] + " ";
                                }
                                bot.Connection.Send("name", name);
                            }
                            else
                                sender.Reply("You are not the owner.");
                            break;
                        }
                }
            }
        }

        public override void onEnable()
        {
        }

        public override void onDisable()
        {
        }

        public override void onConnect()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
        }

        public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
        }

        public override void onTick()
        {
        }

        public override bool HasTab
        {
            get { return false; }
        }

        public override string SubBotName
        {
            get { return "Commands"; }
        }
    }
}
