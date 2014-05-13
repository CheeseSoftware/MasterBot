using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit
{
    class WorldEdit : ASubBot
    {
        public WorldEdit(IBot bot)
            : base(bot)
        {

        }

        public void DrawLine(int x1, int y1, int x2, int y2, IBlock block)
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

        public void DrawCircle(int x, int y, int radius, IBlock block)
        {
            for (int xx = -radius; xx <= radius; xx++)
            {
                for (int yy = -radius; yy <= radius; yy++)
                {
                    if (xx * xx + yy * yy <= radius * radius)
                        bot.Room.setBlock(xx + x, yy + y, block);
                }
            }
        }

        public void SetSize(IBot bot, int x, int y, int width, int height, IBlock replaceWith, IBlock replace = null)
        {
            Set(bot, x, y, x + width, y + height, replaceWith, replace);
        }

        public void Set(IBot bot, int x1, int y1, int x2, int y2, IBlock replaceWith, IBlock replace = null)
        {
            EditRegion region = new EditRegion();
            region.FirstCorner = new Point(x1, y1);
            region.SecondCorner = new Point(x2, y2);
            SetRegion(bot, region, replaceWith, replace);
        }

        public void SetRegion(IBot bot, EditRegion region, IBlock replaceWith, IBlock replace = null)
        {
            if (replace == null)
            {
                foreach (Point pos in region)
                {
                    bot.Room.setBlock(pos.X, pos.Y, replaceWith);
                }
            }
            else
            {
                foreach (Point pos in region)
                {
                    if (bot.Room.getBlock(replaceWith.Layer, pos.X, pos.Y).Id == replace.Id)
                        bot.Room.setBlock(pos.X, pos.Y, replaceWith);
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

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmdSource is Player)
            {
                Player player = (Player)cmdSource;
                if (player.GetMetadata("editregion") == null)
                    player.SetMetadata("editregion", new EditRegion());
                EditRegion region = (EditRegion)player.GetMetadata("editregion");
                switch (cmd)
                {
                    case "set":
                        {
                            if (region.Set)
                            {
                                if (args.Length >= 1)
                                {
                                    int id = -1;
                                    int.TryParse(args[0], out id);
                                    if (id != -1)
                                    {
                                        int layer = id >= 500 ? 1 : 0;
                                        SetRegion(bot, region, new NormalBlock(id, layer));
                                    }
                                    else
                                        bot.Connection.Send("say", player.name + ": Invalid ID.");
                                }
                                else
                                    bot.Connection.Send("say", player.name + ": Usage: !set <id>");
                            }
                            else
                                player.Send(bot, "You have to set a region.");
                            break;
                        }
                    case "replace":
                        {
                            if (region.Set)
                            {
                                if (!string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]))
                                {
                                    int blockToReplace = int.Parse(args[0]);
                                    int blockToReplaceWith = int.Parse(args[1]);
                                    SetRegion(bot, region, new NormalBlock(blockToReplaceWith), new NormalBlock(blockToReplace));
                                }
                                else
                                    bot.Connection.Send("say", player.name + ": Usage: !replace <from> <to>");
                            }
                            else
                                player.Send(bot, "You have to set a region.");
                            break;
                        }
                    case "replacenear":
                        {
                            if (!string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]) && !string.IsNullOrEmpty(args[2]))
                            {
                                int range = int.Parse(args[0]);
                                int blockToReplace = int.Parse(args[1]);
                                int blockToReplaceWith = int.Parse(args[2]);
                                EditRegion closeRegion = new EditRegion();
                                closeRegion.FirstCorner = new Point(player.BlockX - range, player.BlockY - range);
                                closeRegion.SecondCorner = new Point(player.BlockX + range, player.BlockY + range);
                                SetRegion(bot, closeRegion, new NormalBlock(blockToReplaceWith), new NormalBlock(blockToReplace));
                            }
                            else
                                bot.Connection.Send("say", player.name + ": Usage: !replace <from> <to>");
                            break;
                        }
                    case "copy":
                        {
                            if (region.FirstCornerSet)
                            {
                                BlockMap selection = new BlockMap(bot, region.Width, region.Height);
                                foreach (Point pos in region)
                                {
                                    selection.setBlock(pos.X - region.FirstCorner.X, pos.Y - region.FirstCorner.Y, bot.Room.getBlock(1, pos.X, pos.Y));
                                    selection.setBlock(pos.X - region.FirstCorner.X, pos.Y - region.FirstCorner.Y, bot.Room.getBlock(0, pos.X, pos.Y));
                                }
                                player.SetMetadata("selection", selection);
                            }
                            else
                                player.Send(bot, "You have to place a region block.");
                            break;
                        }
                    case "paste":
                        {
                            if (region.FirstCornerSet)
                            {
                                BlockMap selection = (BlockMap)player.GetMetadata("selection");
                                if (selection != null)
                                {
                                    for (int x = 0; x < selection.Width; x++)
                                    {
                                        for (int y = 0; y < selection.Height; y++)
                                        {
                                            int blax = x + region.FirstCorner.X;
                                            int blay = y + region.FirstCorner.Y;
                                            bot.Room.setBlock(blax, blay, selection.getBackgroundBlock(x, y));
                                            bot.Room.setBlock(blax, blay, selection.getBlock(x, y));
                                        }
                                    }
                                }
                                else
                                    player.Send(bot, "You have to copy first.");
                            }
                            else
                                player.Send(bot, "You have to place a region block.");
                            break;
                        }
                    case "circle":
                        {
                            if (region.FirstCornerSet)
                            {
                                int radius;
                                int block;
                                if (args.Length >= 2 && int.TryParse(args[0], out radius) && int.TryParse(args[1], out block))
                                {
                                    DrawCircle(region.FirstCorner.X, region.FirstCorner.Y, radius, new NormalBlock(block, block >= 500 ? 1 : 0));
                                }
                                else
                                    player.Send(bot, "Usage: !circle <radius> <block>");
                            }
                            else
                                player.Send(bot, "You have to place a region block.");
                            break;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
        }

        public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
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

        public override void onTick()
        {
        }

        public override bool HasTab
        {
            get { return false; }
        }

        public override string Name
        {
            get { return "WorldEdit"; }
        }
    }
}
