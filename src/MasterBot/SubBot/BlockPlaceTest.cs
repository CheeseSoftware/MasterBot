using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public class BlockPlaceTest : ASubBot
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
            switch(cmd)
            {
                case "test":
                    {
                        if (cmdSource is IPlayer)
                            blockPlayers.Add(((Player)cmdSource).Id);
                        else
                            cmdSource.Reply("You are not a player.");
                        break;
                    }
                case "testf":
                case "testb":
                    {
                        int layer = 0; ;
                        if (cmd == "testb")
                            layer = 1;

                        if (cmdSource is IPlayer)
                        {
                            IPlayer player = (IPlayer)cmdSource;
                            int x = player.BlockX;
                            int y = player.BlockY;
                            IBlock block = bot.Room.getBlock(layer, x, y);
                            if (block != null)
                                player.Reply("ID:" + block.Id + " X:" + x + " Y:" + y + " Placer:" + (block.Placer != null ? block.Placer.Name : "undefined"));
                            else
                                player.Reply("That is a null block, not good!");
                        }
                        else
                            cmdSource.Reply("You are not a player.");
                        break;
                    }
            }
        }

        public override void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock)
        {
            if (newBlock.Placer != null && blockPlayers.Contains(newBlock.Placer.Id))
            {
                bot.Say(newBlock.Placer, "That block is: " + oldBlock.Id + ", placed by " + (oldBlock.Placer != null ? oldBlock.Placer.Name : "undefined " + "X:" + x + " Y:" + y));
                bot.Room.setBlock(x, y, oldBlock);
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

        public override string SubBotName
        {
            get { return "BlockPlaceTest"; }
        }
    }
}
