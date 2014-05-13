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

        public BlockPlaceTest()
        {

        }

        public override void onConnect(IBot bot)
        {
        }

        public override void onDisconnect(IBot bot, string reason)
        {
        }

        public override void onMessage(IBot bot, PlayerIOClient.Message m)
        {

        }

        public override void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource)
        {
            if(cmd == "test")
            {
                if (cmdSource is Player)
                    blockPlayers.Add(((Player)cmdSource).Id);
                else
                    bot.Connection.Send("say", "You are not a player.");
            }
        }

        public override void Update(IBot bot)
        {
        }

        public override void onBlockChange(IBot bot, int x, int y, IBlock newBlock, IBlock oldBlock)
        {
            if(newBlock.Placer != null && blockPlayers.Contains(newBlock.Placer.Id))
            {
                bot.Connection.Send("say", "That block is: " + oldBlock.Id + ", placed by " + (oldBlock.Placer != null ? oldBlock.Placer.name : "undefined " + "X:" + x + " Y:" + y));
                blockPlayers.Remove(newBlock.Placer.Id);
            }
        }
    }
}
