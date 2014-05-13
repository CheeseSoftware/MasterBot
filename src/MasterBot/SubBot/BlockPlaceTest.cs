using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    class BlockPlaceTest : ASubBot
    {
        List<int> blockPlayers = new List<int>();

        public BlockPlaceTest(IBot bot)
            : base(bot)
        {

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

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmd == "test")
            {
                if (cmdSource is Player)
                    blockPlayers.Add(((Player)cmdSource).Id);
                else
                    bot.Connection.Send("say", "You are not a player.");
            }
        }

        public override void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock)
        {
            if (newBlock.Placer != null && blockPlayers.Contains(newBlock.Placer.Id))
            {
                bot.Connection.Send("say", "That block is: " + oldBlock.Id + ", placed by " + (oldBlock.Placer != null ? oldBlock.Placer.name : "undefined " + "X:" + x + " Y:" + y));
                blockPlayers.Remove(newBlock.Placer.Id);
            }
        }

        public override void onTick()
        {
        }

        public override bool HasTab
        {
            get { return false; }
        }

        public override string Name
        {
            get { return "BlockPlaceTest"; }
        }
    }
}
