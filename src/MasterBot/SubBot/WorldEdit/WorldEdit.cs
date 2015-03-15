using MasterBot.Room;
using MasterBot.Room.Block;
using MasterBot.SubBot.WorldEdit.Change;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit
{
	class WorldEdit : ASubBot
	{
		private IBlockDrawer blockDrawer;
		private IPlayer recordingPlayer;

		public WorldEdit(IBot bot)
			: base(bot)
		{
			blockDrawer = bot.Room.BlockDrawerPool.CreateBlockDrawer(0);
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

		#region edit functions
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

		public void WriteLetter(int spacing, string filename, int x, int y, IBlock block)
		{
			switch (filename)
			{
				case ".":
					filename = "dota";
					break;
				case @":":
					filename = "dotb";
					break;
				case @"?":
					filename = "dotc";
					break;
				case "\"":
					filename = "dotd";
					break;
				case @"<":
					filename = "dote";
					break;
				case @">":
					filename = "dotf";
					break;
				case @"*":
					filename = "dotg";
					break;
				case @"/":
					filename = "doth";
					break;
				case @"\":
					filename = "doti";
					break;
				case @"|":
					filename = "dotj";
					break;
			}
			int bigletterspacex = 0;
			int bigletterspacey = 0;
			int bigletterspaceyup = 0;

			if (File.Exists(@"write/" + filename))
			{
				string attskriva1 = File.ReadAllText(@"write/" + filename);
				string[] attskriva = new string[100];
				attskriva = attskriva1.Split(',');
				int valdsak = 0;
				if (filename == @"@")
					bigletterspacex = 4;
				if (filename == @"apskfpasFIXTHISFY" || filename == @"w" || filename == @"#" || filename == @"&")
					bigletterspacex = 2;
				if (filename == @"n" || filename == @"%" || filename == @"doth" || filename == @"doti")
					bigletterspacex = 1;
				if (filename == @"," || filename == @"&" || filename == @"@")
					bigletterspacey = 1;
				if (filename == @"$")
					bigletterspacey = 2;

				for (int localwy = 0 - bigletterspaceyup; localwy < 5 + bigletterspacey; localwy++)
				{
					for (int localwx = 0; localwx <= 2 + bigletterspacex; localwx++)
					{
						int sendx = spacing + localwx + x;
						int sendy = localwy + y;
						if (Convert.ToInt32(attskriva[valdsak]) == 1)
							RecordSetBlock(sendx, sendy, block);
						valdsak++;
					}
				}
				valdsak = 0;
			}
		}

		public double RawDistance(Point first, Point second)
		{
			int dx = second.X - first.X;
			int dy = second.Y - first.Y;
			return Math.Pow(dx, 2) + Math.Pow(dy, 2);
		}

		public double Distance(Point first, Point second)
		{
			return Math.Sqrt(RawDistance(first, second));
		}

		#endregion

		public override void onEnable()
		{
		}

		public override void onDisable()
		{
		}

		public override void onConnect()
		{
			blockDrawer.Start();
		}

		public override void onDisconnect(string reason)
		{
			blockDrawer.Stop();
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
					case "fill":
						if (args.Length >= 1)
						{
							int id = -1;
							int layer = 0;

							int.TryParse(args[0], out id);
							if (args.Length >= 2)
								int.TryParse(args[1], out layer);

							if (id != -1)
							{
								layer = (id >= 500 && id < 1000) ? 1 : layer;

								EditRegion region2 = new EditRegion();
								region2.FirstCorner = new Point(1, 1);
								region2.SecondCorner = new Point(bot.Room.Width - 2, bot.Room.Height - 2);
								SetRegion(bot, region2, new NormalBlock(id, layer));
							}
							else
								bot.ChatSayer.Say(player.Name + ": Invalid ID.");
						}
						else
							bot.ChatSayer.Say(player.Name + ": Usage: !fill <id> [layer]");
						break;
					case "undo":
						if (player.HasMetadata("worldedithistory") && player.HasMetadata("worldedithistoryindex"))
						{
							List<IEditChange> history = (List<IEditChange>)player.GetMetadata("worldedithistory");
							int index = (int)player.GetMetadata("worldedithistoryindex");
							if (index >= 0 && index <= history.Count - 1 && (index != 0 || !history[index].IsUndone))
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
					case "redo":
						if (player.HasMetadata("worldedithistory") && player.HasMetadata("worldedithistoryindex"))
						{
							List<IEditChange> history = (List<IEditChange>)player.GetMetadata("worldedithistory");
							int index = (int)player.GetMetadata("worldedithistoryindex");
							if (index <= history.Count - 1 && (index != history.Count - 1 || !history[index].IsRedone))
							{
								if (history.Count - 1 >= index + 1)
									history[index + 1].Redo(blockDrawer);
								else
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
					case "set":
						if (region.Set)
						{
							if (args.Length >= 1)
							{
								int id = -1;
								int.TryParse(args[0], out id);
								if (id != -1)
								{
									int layer = (id >= 500 && id < 1000) || id == 1337 ? 1 : 0;
									if (args.Length >= 2)
										int.TryParse(args[1], out layer);

                                    SetRegion(bot, region, new NormalBlock(id, layer));
								}
								else
									bot.ChatSayer.Say(player.Name + ": Invalid ID.");
							}
							else
								bot.ChatSayer.Say(player.Name + ": Usage: !set <id> [layer]");
						}
						else
							player.Send("You have to set a region.");
						break;
					case "replace":
						if (region.Set)
						{
							int blockToReplace;
							int blockToReplaceWith;
							if (args.Length >= 2 && int.TryParse(args[0], out blockToReplace) && int.TryParse(args[1], out blockToReplaceWith))
							{
								SetRegion(bot, region, new NormalBlock(blockToReplaceWith), new NormalBlock(blockToReplace));
							}
							else
								bot.ChatSayer.Say(player.Name + ": Usage: !replace <from> <to>");
						}
						else
							player.Send("You have to set a region.");
						break;
					case "replacenear":
						{
							int range;
							int blockToReplace;
							int blockToReplaceWith;
							if (args.Length >= 3 && int.TryParse(args[0], out range) && int.TryParse(args[1], out blockToReplace) && int.TryParse(args[2], out blockToReplaceWith))
							{
								EditRegion closeRegion = new EditRegion();
								closeRegion.FirstCorner = new Point(player.BlockX - range, player.BlockY - range);
								closeRegion.SecondCorner = new Point(player.BlockX + range, player.BlockY + range);
								SetRegion(bot, closeRegion, new NormalBlock(blockToReplaceWith), new NormalBlock(blockToReplace));
							}
							else
								bot.ChatSayer.Say(player.Name + ": Usage: !replacenear <range> <from> <to>");
						}
						break;
					case "copy":
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
					case "paste":
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
										RecordSetBlock(blax, blay, selection.getForegroundBlock(x, y));
									}
								}
							}
							else
								player.Send("You have to copy first.");
						}
						else
							player.Send("You have to place a region block.");
						break;
					case "line":
						{
							int tempBlock;
							if (args.Length >= 1 && int.TryParse(args[0], out tempBlock))
							{
								if (region.Set)
									DrawLine(region.FirstCorner.X, region.FirstCorner.Y, region.SecondCorner.X, region.SecondCorner.Y, new NormalBlock(tempBlock, (tempBlock >= 500 && tempBlock < 1000) ? 1 : 0));
								else
									player.Reply("You have to set a region.");
							}
							else
								player.Reply("Usage: !line <block>");
						}
						break;
					case "circle":
						if (region.FirstCornerSet)
						{
							int radius;
							int block;
							if (args.Length >= 2 && int.TryParse(args[0], out radius) && int.TryParse(args[1], out block))
							{
								DrawCircle(region.FirstCorner.X, region.FirstCorner.Y, radius, new NormalBlock(block, (block >= 500 && block < 1000) ? 1 : 0));
							}
							else
								player.Send("Usage: !circle <radius> <block>");
						}
						else
							player.Send("You have to place a region block.");
						break;
					case "square":
						if (region.FirstCornerSet)
						{
							int radius;
							int block;
							if (args.Length >= 2 && int.TryParse(args[0], out radius) && int.TryParse(args[1], out block))
							{
								for (int x = region.FirstCorner.X - radius; x <= region.FirstCorner.X + radius; x++)
								{
									for (int y = region.FirstCorner.Y - radius; y <= region.FirstCorner.Y + radius; y++)
									{
										RecordSetBlock(x, y, new NormalBlock(block, (block >= 500 && block < 1000) ? 1 : 0));
									}
								}
							}
							else
								player.Send("Usage: !square <radius> <block>");
						}
						else
							player.Send("You have to place a region block.");
						break;
					case "fillexpand":
						{
							int toReplace = 0;
							int toReplaceLayer = 0;
							int toReplaceWith = 0;
							if (args.Length == 1)
							{
								if (!int.TryParse(args[0], out toReplaceWith))
								{
									player.Reply("Usage: !fillexpand <from=0> <to>");
									return;
								}
							}
							else if (args.Length >= 2)
							{
								if (!int.TryParse(args[1], out toReplaceWith) || !int.TryParse(args[0], out toReplace))
								{
									player.Reply("Usage: !fillexpand <from=0> <to>");
									return;
								}
							}
							else
							{
								player.Reply("Usage: !fillexpand <from=0> <to>");
								break;
							}
							if (toReplace >= 500 && toReplace < 1000)
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
									for (int i = 0; i < closeBlocks.Count; i++)
									{
										Point current = new Point(closeBlocks[i].X + parent.X, closeBlocks[i].Y + parent.Y);
										IBlock currentBlock = bot.Room.BlockMap.getBlock(toReplaceLayer, current.X, current.Y);
										if (currentBlock.Id == toReplace && !blocksToCheck.Contains(current) && !blocksToFill.Contains(current) && current.X >= 0 && current.Y >= 0 && current.X <= bot.Room.Width && current.Y <= bot.Room.Height)
										{
											blocksToFill.Add(current);
											blocksToCheck.Enqueue(current);
											total++;
										}
									}
								}
								bot.ChatSayer.Say("total blocks: " + total + ". Filling..");
								int layer = 0;
								if (toReplaceWith >= 500 && toReplaceWith < 1000)
									layer = 1;
								foreach (Point p in blocksToFill)
								{
									RecordSetBlock((int)p.X, (int)p.Y, new NormalBlock(toReplaceWith, layer));
								}
							}
						}
						break;
					case "stop":
						blockDrawer.Stop();
						break;
					case "start":
						blockDrawer.Start();
						break;
					case "clearrepairblocks":
						//TODO: add function to blockdrawer
						break;
					case "write":
						if (region.FirstCornerSet)
						{
							int drawBlock = 0;
							if (args.Length >= 2 && int.TryParse(args[0], out drawBlock))
							{
								List<char[]> letters = new List<char[]>();
								foreach (string str in args.Skip(1))
									letters.Add(str.ToLower().ToCharArray());

								int spacing = 0;
								foreach (char[] array in letters)
								{
									for (int letterindex = 0; letterindex < array.Length; letterindex++)
									{
										string l = array[letterindex].ToString();
										if (l != "_")
										{
											WriteLetter(spacing, l, region.FirstCorner.X, region.FirstCorner.Y, new NormalBlock(drawBlock, (drawBlock >= 500 && drawBlock < 1000) ? 1 : 0));
										}
										if (l == @"@")
											spacing += 4;
										else if (l == "m" || l == "w" || l == "#" || l == "&")
											spacing += 2;
										else if (l == "n" || l == "%" || l == @"/" || l == @"\")
											spacing += 1;

										if (l == "|" || l == "." || l == "," || l == "'" || l == "!")
										{
											spacing -= 2;
										}
										else if (l == ",")
										{
											spacing -= 1;
										}
										spacing += 4;
									}
									spacing += array.Length;
								}
							}
							else
								player.Reply("Usage: !write <block> <text..>");
						}
						else
							player.Reply("You have to set the first corner.");
						break;

					case "border":
						{
							int thickness;
							int block;
							if (args.Length >= 2 && int.TryParse(args[0], out thickness) && int.TryParse(args[1], out block))
							{
								IBlock baseBlock =
									(bot.Room.getBlock(1, player.BlockX, player.BlockY).Id == 0 ?
									bot.Room.getBlock(0, player.BlockX, player.BlockY) :
									bot.Room.getBlock(0, player.BlockX, player.BlockY));
								int layer = baseBlock.Layer;
								int x = player.BlockX;
								int y = player.BlockY;
								List<Point> closeBlocks = new List<Point> { new Point(1, 0), new Point(0, -1), new Point(-1, 0), new Point(0, 1) };
								HashSet<Point> previouslySetBlocks = new HashSet<Point>();

								for (int currentThickness = 0; currentThickness < thickness; currentThickness++)
								{
									int xx = x;
									while (bot.Room.getBlock(layer, xx + 1, y).Id == baseBlock.Id && !previouslySetBlocks.Contains(new Point(xx + 1, y)) && xx < 2000)
										xx++;
									List<Point> blocksToSet = new List<Point>();
									HashSet<Point> blocksChecked = new HashSet<Point>();
									Queue<Point> blocksToCheck = new Queue<Point>();
									Point startPoint = new Point(xx, y);
									blocksToCheck.Enqueue(startPoint);
									blocksToSet.Add(startPoint);

									while (blocksToCheck.Count > 0)
									{
										Point parent = blocksToCheck.Dequeue();
										for (int i = 0; i < closeBlocks.Count; i++)
										{
											Point current = new Point(closeBlocks[i].X + parent.X, closeBlocks[i].Y + parent.Y);
											IBlock currentBlock = bot.Room.BlockMap.getBlock(layer, current.X, current.Y);
											if (currentBlock.Id == baseBlock.Id && !previouslySetBlocks.Contains(current) && !blocksToCheck.Contains(current) && !blocksChecked.Contains(current))
											{
												blocksToCheck.Enqueue(current);
											}
											else if ((currentBlock.Id != baseBlock.Id || previouslySetBlocks.Contains(current)) && !blocksToSet.Contains(parent))
											{
												blocksToSet.Add(parent);
											}
											blocksChecked.Add(parent);
										}
									}

									foreach (Point p in blocksToSet)
									{
										RecordSetBlock(p.X, p.Y, new NormalBlock(block, (block >= 500 && block < 1000) ? 1 : 0));
										previouslySetBlocks.Add(p);
									}
								}
							}
							else
								player.Reply("Usage: !border <thickness> <block>");
						}
						break;

					case "worldborder":
						{
							int block;
							if (int.TryParse(args[0], out block))
							{
								for (int x = 0; x < bot.Room.Width; x++)
								{
									for (int y = 0; y < bot.Room.Height; y++)
									{
										if (bot.Room.BlockMap.isOnBorder(x, y))
											RecordSetBlock(x, y, new NormalBlock(block));
									}
								}
							}
							else
								player.Reply("Usage: !worldborder <block>");
						}
						break;
					default:

						return;

				}
				EndRecord(player);
			}
		}

		public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
		{
			if (newBlock.Id == 32)
			{
				if (newBlock.Placer != null && newBlock.Placer.IsOp)
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
					bot.ChatSayer.Say(player.Name + ": " + output + " " + new Random().Next(10));
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

		public override string SubBotName
		{
			get { return "WorldEdit"; }
		}

		protected override void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// WorldEdit
			// 
			this.Name = "WorldEdit";
			this.Size = new System.Drawing.Size(319, 272);
			this.ResumeLayout(false);

		}
	}
}
