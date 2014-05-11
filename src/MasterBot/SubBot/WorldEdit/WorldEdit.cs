using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit
{
    class WorldEdit : ISubBot
    {

        public WorldEdit()
        {

        }

        private void DrawLine(IBot bot, int x1, int y1, int x2, int y2, IBlock block)
        {
            int iTag = 0;
            int dx, dy;
            int tx, ty;
            int inc1, inc2;
            int d;
            int curx, cury;

            bot.Room.setBlock(x1, y1, block);

            if (x1 == x2 && y1 == y2)
            {
                return;
            }

            dx = Math.Abs(x2 - x1);
            dy = Math.Abs(y2 - y1);
            if (dx < dy)
            {
                iTag = 1;
                int use1;
                use1 = x1;
                x1 = y1;
                y1 = use1;
                int use2;
                use2 = x2;
                x2 = y2;
                y2 = use2;
                int use3;
                use3 = dx;
                dx = dy;
                dy = use3;
            }

            tx = (x2 - x1) > 0 ? 1 : -1;
            ty = (y2 - y1) > 0 ? 1 : -1;
            curx = x1; cury = y1;
            inc1 = 2 * dy;
            inc2 = 2 * (dy - dx);
            d = inc1 - dx;

            while (curx != x2)
            {
                if (d < 0)
                    d += inc1;
                else
                {
                    cury += ty;
                    d += inc2;
                }
                if (iTag == 1)
                    bot.Room.setBlock(cury, curx, block);
                else
                    bot.Room.setBlock(curx, cury, block);
                curx += tx;
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

        public void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmdSource is Player)
            {
                Player player = (Player)cmdSource;
                EditRegion region = (EditRegion)player.GetMetadata("editregion");
                if (region != null && region.Set)
                {
                    switch (cmd)
                    {
                        case "set":
                            {
                                if (args.Length >= 1)
                                {
                                    int id = -1;
                                    int.TryParse(args[0], out id);
                                    if (id != -1)
                                    {
                                        for (int x = region.FirstCorner.X; x <= region.SecondCorner.X; x++)
                                        {
                                            for (int y = region.FirstCorner.Y; y <= region.SecondCorner.Y; y++)
                                            {
                                                bot.Room.setBlock(x, y, new NormalBlock(id, id >= 500 ? 1 : 0));
                                            }
                                        }
                                    }
                                    else
                                        bot.Connection.Send("say", player.name + ": Invalid ID.");
                                }
                                else
                                    bot.Connection.Send("say", player.name + ": Usage: !set <id>");
                                break;
                            }
                        case "replace":
                            {
                                if (!string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]))
                                {
                                    int blockToReplace = int.Parse(args[0]);
                                    int blockToReplaceWith = int.Parse(args[1]);
                                    for (int x = region.FirstCorner.X; x <= region.SecondCorner.X; x++)
                                    {
                                        for (int y = region.FirstCorner.Y; y <= region.SecondCorner.Y; y++)
                                        {
                                            if (bot.Room.getBlock(blockToReplace >= 500 ? 1 : 0, x, y).Id == blockToReplace)
                                            {
                                                bot.Room.setBlock(x, y, new NormalBlock(blockToReplaceWith, blockToReplaceWith >= 500 ? 1 : 0));
                                            }
                                        }
                                    }
                                }
                                else
                                    bot.Connection.Send("say", player.name + ": Usage: !replace <from> <to>");
                                break;
                            }
                    }
                }
                else
                    bot.Connection.Send("say", player.name + ": You do not have a region set.");
            }
        }

        public void onBlockChange(IBot bot, int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            if (newBlock.Id == 32)
            {
                if (newBlock.Placer != null)
                {
                    Player player = newBlock.Placer;
                    if (player.GetMetadata("editregion") == null)
                        player.SetMetadata("editregion", new EditRegion());
                    EditRegion region = (EditRegion)player.GetMetadata("editregion");

                    string output = "";
                    if (region.FirstCornerSet && !region.Set)
                    {
                        region.SecondCorner = new Point(x, y);
                        bot.Room.setBlock(region.SecondCorner.X, region.SecondCorner.Y, oldBlock);

                        if (region.FirstCorner.X > region.SecondCorner.X)
                        {
                            int buffer = region.FirstCorner.X;
                            region.FirstCorner = new Point(region.SecondCorner.X, region.FirstCorner.Y);
                            region.SecondCorner = new Point(buffer, region.SecondCorner.Y);
                        }
                        if (region.FirstCorner.Y > region.SecondCorner.Y)
                        {
                            int buffer = region.FirstCorner.Y;
                            region.FirstCorner = new Point(region.FirstCorner.X, region.SecondCorner.Y);
                            region.SecondCorner = new Point(region.SecondCorner.X, buffer);
                        }
                        output = "Second corner set";
                    }
                    else
                    {
                        if (region.Set)
                            region.SecondCorner = new Point(-1, -1);
                        region.FirstCorner = new Point(x, y);
                        bot.Room.setBlock(region.FirstCorner.X, region.FirstCorner.Y, oldBlock);
                        output = "First corner set";
                    }
                    bot.Connection.Send("say", player.name + ": " + output + " " + new Random().Next(10));
                }
            }
        }

        public void Update(IBot bot)
        {
        }
    }
}
