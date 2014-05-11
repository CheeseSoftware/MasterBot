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
    public class Commands : ISubBot
    {
        private List<string> disabledPlayers = new List<string>();
        private List<string> protectedPlayers = new List<string>();
        private List<string> getPlacerPlayers = new List<string>();

        public Commands()
        {
        }

        public void onCommand(IBot bot, string cmd, string[] args, ICmdSource sender)
        {
            if (sender is Player)
            {
                Player player = (Player)sender;
                switch (cmd)
                {
                    case "fillexpand":
                        {
                            int toReplace = 0;
                            int toReplaceLayer = 0;
                            int toReplaceWith = 0;
                            if (args.Length == 1)
                            {
                                if (!int.TryParse(args[0], out toReplaceWith))
                                {
                                    bot.Connection.Send("say", "Usage: !fillexpand <from id=0> <to id>");
                                    return;
                                }
                            }
                            else if (args.Length >= 2)
                            {
                                if (!int.TryParse(args[1], out toReplaceWith) || !int.TryParse(args[0], out toReplace))
                                {
                                    bot.Connection.Send("say", "Usage: !fillexpand <from id=0> <to id>");
                                    return;
                                }
                            }
                            if (toReplace >= 500)
                                toReplaceLayer = 1;
                            IBlock startBlock = bot.Room.BlockMap.getBlock(toReplaceLayer, player.BlockX, player.BlockY);
                            if (startBlock.Id == toReplace)
                            {
                                int total = 0;
                                List<Point> closeBlocks = new List<Point> { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
                                Queue<Point> blocksToCheck = new Queue<Point>();
                                List<Point> blocksToFill = new List<Point>();
                                blocksToCheck.Enqueue(new Point(player.BlockX, player.BlockY));
                                while (blocksToCheck.Count > 0)
                                {
                                    Point parent = blocksToCheck.Dequeue();
                                    //if (!blocksToFill.Contains(parent))
                                    for (int i = 0; i < closeBlocks.Count; i++)
                                    {
                                        Point current = new Point(closeBlocks[i].X + parent.X, closeBlocks[i].Y + parent.Y);
                                        IBlock currentBlock = bot.Room.BlockMap.getBlock(toReplaceLayer, current.X, current.Y);
                                        if (currentBlock.Id == toReplace && !blocksToCheck.Contains(current) && !blocksToFill.Contains(current) && current.X >= 0 && current.Y >= 0 && current.X <= bot.Room.Width && current.Y <= bot.Room.Height)
                                        {
                                            blocksToFill.Add(current);
                                            blocksToCheck.Enqueue(current);
                                            total++;
                                            if (total > 10000)
                                            {
                                                bot.Connection.Send("say", "Don't try to fill the whole world, fool!");
                                                return;
                                            }
                                        }
                                    }
                                }
                                bot.Connection.Send("say", "total blocks: " + total + ". Filling..");
                                int layer = 0;
                                if (toReplaceWith >= 500)
                                    layer = 1;
                                foreach (Point p in blocksToFill)
                                {
                                    bot.Room.setBlock((int)p.X, (int)p.Y, new NormalBlock(toReplaceWith, layer));
                                }
                            }
                        }
                        break;
                }
            }
        }


        public void onConnect(IBot bot)
        {
        }

        public void onDisconnect(IBot bot, string reason)
        {
        }

        public void onMessage(IBot bot, PlayerIOClient.Message m)
        {
        }

        public void onBlockChange(IBot bot, int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
        }

        public void Update(IBot bot)
        {
        }
    }
}
