using MasterBot.Room;
using MasterBot.Room.Block;
using MasterBot.SubBot.WorldEdit.Change;
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
        //List<IEditChange> history = new List<IEditChange>();

        private IBlockDrawer blockDrawer;
        private System.Windows.Forms.Label labelBlocksToRepair;
        private System.Windows.Forms.NumericUpDown numericUpDownBlocksToRepair;
        private System.Windows.Forms.Label labelBlocksToPlace;
        private System.Windows.Forms.NumericUpDown numericUpDownBlocksToPlace;
        private IPlayer recordingPlayer;

        public WorldEdit(IBot bot)
            : base(bot)
        {
            blockDrawer = bot.Room.BlockDrawerPool.CreateBlockDrawer(0);
            blockDrawer.Start();
            EnableTick(50);
        }

        private void BeginRecord(IPlayer player)
        {
            player.SetMetadata("worldeditrecord", new List<IEditChange>());
            recordingPlayer = player;
        }

        private void EndRecord(IPlayer player)
        {
            List<IEditChange> record = (List<IEditChange>)recordingPlayer.GetMetadata("worldeditrecord");
            if (record.Count > 0)
            {
                if (!player.HasMetadata("worldedithistory"))
                    player.SetMetadata("worldedithistory", new List<IEditChange>());
                List<IEditChange> history = (List<IEditChange>)player.GetMetadata("worldedithistory");
                history.Add(new EditChangeList((List<IEditChange>)record));
                if (!player.HasMetadata("worldedithistoryindex"))
                    player.SetMetadata("worldedithistoryindex", 0);
                else
                    player.SetMetadata("worldedithistoryindex", ((int)player.GetMetadata("worldedithistoryindex")) + 1);
            }
            player.RemoveMetadata("worldeditrecord");
            recordingPlayer = null;
        }

        private void RecordSetBlock(int x, int y, IBlock block)
        {
            if (recordingPlayer != null)
            {
                IBlock oldBlock = bot.Room.getBlock(block.Layer, x, y);
                BlockWithPos oldBlockWithPos = new BlockWithPos(x, y, oldBlock);
                ((List<IEditChange>)recordingPlayer.GetMetadata("worldeditrecord")).Add(new BlockEditChange(new BlockWithPos(x, y, block), oldBlockWithPos));
            }
            blockDrawer.PlaceBlock(new BlockWithPos(x, y, block));
        }

        public void DrawLine(int x1, int y1, int x2, int y2, IBlock block)
        {
            int iTag = 0;
            int dx, dy;
            int tx, ty;
            int inc1, inc2;
            int d;
            int curx, cury;

            RecordSetBlock(x1, y1, block);//RecordSetBlock((x1, y1, block));

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
                    RecordSetBlock(cury, curx, block);
                else
                    RecordSetBlock(curx, cury, block);
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
                        RecordSetBlock(xx + x, yy + y, block);
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
                    RecordSetBlock(pos.X, pos.Y, replaceWith);
                }
            }
            else
            {
                foreach (Point pos in region)
                {
                    if (bot.Room.getBlock(replaceWith.Layer, pos.X, pos.Y).Id == replace.Id)
                        RecordSetBlock(pos.X, pos.Y, replaceWith);
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
            if (cmdSource is Player && ((Player)cmdSource).IsOp)
            {
                Player player = (Player)cmdSource;
                if (player.GetMetadata("editregion") == null)
                    player.SetMetadata("editregion", new EditRegion());
                EditRegion region = (EditRegion)player.GetMetadata("editregion");
                BeginRecord(player);
                switch (cmd)
                {
                    case "undo":
                        {
                            if (player.HasMetadata("worldedithistory") && player.HasMetadata("worldedithistoryindex"))
                            {
                                List<IEditChange> history = (List<IEditChange>)player.GetMetadata("worldedithistory");
                                int index = (int)player.GetMetadata("worldedithistoryindex");
                                if (history.Count > index)
                                {
                                    history[index].Undo(blockDrawer);
                                    if (index - 1 >= 0)
                                        player.SetMetadata("worldedithistoryindex", ((int)player.GetMetadata("worldedithistoryindex")) - 1);
                                }
                                else
                                    player.Reply("Nothing left to undo.");
                            }
                            else
                                player.Reply("No history.");
                            break;
                        }
                    case "redo":
                        {
                            if (player.HasMetadata("worldedithistory") && player.HasMetadata("worldedithistoryindex"))
                            {
                                List<IEditChange> history = (List<IEditChange>)player.GetMetadata("worldedithistory");
                                int index = (int)player.GetMetadata("worldedithistoryindex");
                                if (index <= history.Count - 1)
                                {
                                    history[index].Redo(blockDrawer);
                                    if (index + 1 <= history.Count - 1)
                                        player.SetMetadata("worldedithistoryindex", ((int)player.GetMetadata("worldedithistoryindex")) + 1);
                                }
                                else
                                    player.Reply("Nothing left to redo.");
                            }
                            else
                                player.Reply("No history.");
                            break;
                        }
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
                                        bot.Connection.Send("say", player.Name + ": Invalid ID.");
                                }
                                else
                                    bot.Connection.Send("say", player.Name + ": Usage: !set <id>");
                            }
                            else
                                player.Send("You have to set a region.");
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
                                    bot.Connection.Send("say", player.Name + ": Usage: !replace <from> <to>");
                            }
                            else
                                player.Send("You have to set a region.");
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
                                bot.Connection.Send("say", player.Name + ": Usage: !replace <from> <to>");
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
                                player.Send("You have to place a region block.");
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
                                            RecordSetBlock(blax, blay, selection.getBackgroundBlock(x, y));
                                            RecordSetBlock(blax, blay, selection.getBlock(x, y));
                                        }
                                    }
                                }
                                else
                                    player.Send("You have to copy first.");
                            }
                            else
                                player.Send("You have to place a region block.");
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
                                    player.Send("Usage: !circle <radius> <block>");
                            }
                            else
                                player.Send("You have to place a region block.");
                            break;
                        }
                    default:
                        {
                            return;
                        }
                }
                EndRecord(player);
            }
        }

        public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            if (newBlock.Id == 32)
            {
                if (newBlock.Placer != null)
                {
                    IPlayer player = newBlock.Placer;
                    if (player.GetMetadata("editregion") == null)
                        player.SetMetadata("editregion", new EditRegion());
                    EditRegion region = (EditRegion)player.GetMetadata("editregion");

                    string output = "";
                    if (region.FirstCornerSet && !region.Set)
                    {
                        region.SecondCorner = new Point(x, y);
                        RecordSetBlock(region.SecondCorner.X, region.SecondCorner.Y, oldBlock);
                        output = "Second corner set";
                    }
                    else
                    {
                        if (region.Set)
                            region.SecondCorner = new Point(-1, -1);
                        region.FirstCorner = new Point(x, y);
                        RecordSetBlock(region.FirstCorner.X, region.FirstCorner.Y, oldBlock);
                        output = "First corner set";
                    }
                    bot.Connection.Send("say", player.Name + ": " + output + " " + new Random().Next(10));
                }
            }
        }

        public override void onTick()
        {
            SafeInvoke.Invoke(this, new Action(() =>
            {
                if (blockDrawer != null)
                {
                    numericUpDownBlocksToPlace.Value = blockDrawer.BlocksToDrawSize;
                    numericUpDownBlocksToRepair.Value = blockDrawer.BlocksToRepairSize;
                }
            }));
        }

        public override bool HasTab
        {
            get { return true; }
        }

        public override string BotName
        {
            get { return "WorldEdit"; }
        }

        protected override void InitializeComponent()
        {
            this.labelBlocksToRepair = new System.Windows.Forms.Label();
            this.numericUpDownBlocksToRepair = new System.Windows.Forms.NumericUpDown();
            this.labelBlocksToPlace = new System.Windows.Forms.Label();
            this.numericUpDownBlocksToPlace = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToRepair)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToPlace)).BeginInit();
            this.SuspendLayout();
            // 
            // labelBlocksToRepair
            // 
            this.labelBlocksToRepair.AutoSize = true;
            this.labelBlocksToRepair.Location = new System.Drawing.Point(20, 46);
            this.labelBlocksToRepair.Name = "labelBlocksToRepair";
            this.labelBlocksToRepair.Size = new System.Drawing.Size(83, 13);
            this.labelBlocksToRepair.TabIndex = 7;
            this.labelBlocksToRepair.Text = "Blocks to repair:";
            // 
            // numericUpDownBlocksToRepair
            // 
            this.numericUpDownBlocksToRepair.Location = new System.Drawing.Point(109, 44);
            this.numericUpDownBlocksToRepair.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownBlocksToRepair.Name = "numericUpDownBlocksToRepair";
            this.numericUpDownBlocksToRepair.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownBlocksToRepair.TabIndex = 6;
            // 
            // labelBlocksToPlace
            // 
            this.labelBlocksToPlace.AutoSize = true;
            this.labelBlocksToPlace.Location = new System.Drawing.Point(20, 20);
            this.labelBlocksToPlace.Name = "labelBlocksToPlace";
            this.labelBlocksToPlace.Size = new System.Drawing.Size(83, 13);
            this.labelBlocksToPlace.TabIndex = 5;
            this.labelBlocksToPlace.Text = "Blocks to place:";
            // 
            // numericUpDownBlocksToPlace
            // 
            this.numericUpDownBlocksToPlace.Location = new System.Drawing.Point(109, 18);
            this.numericUpDownBlocksToPlace.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownBlocksToPlace.Name = "numericUpDownBlocksToPlace";
            this.numericUpDownBlocksToPlace.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownBlocksToPlace.TabIndex = 4;
            // 
            // WorldEdit
            // 
            this.Controls.Add(this.labelBlocksToRepair);
            this.Controls.Add(this.numericUpDownBlocksToRepair);
            this.Controls.Add(this.labelBlocksToPlace);
            this.Controls.Add(this.numericUpDownBlocksToPlace);
            this.Name = "WorldEdit";
            this.Size = new System.Drawing.Size(319, 272);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToRepair)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToPlace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
