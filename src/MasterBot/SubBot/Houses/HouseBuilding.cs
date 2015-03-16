using MasterBot.Inventory;
using MasterBot.Room.Block;
using MasterBot.SubBot.Houses;
using MasterBot.SubBot.Houses.Furnitures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
	class HouseBuilding : ASubBot
	{
		HouseManager houseManager;
		FurnitureManager furnitureManager;

		DateTime dateConnected;
		bool connected = false;

		public HouseBuilding(IBot bot)
			: base(bot)
		{
			this.houseManager = new HouseManager(bot);
			this.furnitureManager = new FurnitureManager(bot);


			HouseType tinyHouse = new HouseType("tinyhouse", 7, 7, 46, 48, 541);
			tinyHouse.AddCost("stone", 25);
			HouseType smallHouse = new HouseType("smallhouse", 9, 9, 93, 93);
			smallHouse.AddCost("stone", 100);
			HouseType mediumHouse = new HouseType("mediumhouse", 11, 11, 1023, 1024);
			mediumHouse.AddCost("stone", 400);
			mediumHouse.AddCost("copper", 20);
			HouseType largeHouse = new HouseType("largehouse", 13, 13, 14, 1018, 505);
			largeHouse.AddCost("stone", 600);
			largeHouse.AddCost("iron", 30);
			HouseType veryLargeHouse = new HouseType("verylargehouse", 15, 15, 1021, 42);
			veryLargeHouse.AddCost("stone", 800);
			veryLargeHouse.AddCost("gold", 50);
			HouseType hugeHouse = new HouseType("hugehouse", 17, 17, 196, 195, 618);
			hugeHouse.AddCost("stone", 1000);
			hugeHouse.AddCost("copper", 50);
			hugeHouse.AddCost("iron", 50);
			hugeHouse.AddCost("gold", 50);

			HouseType weirdHouse = new HouseType("weirdhouse", 17, 5, 80, 82, 548);
			weirdHouse.AddCost("stone", 400);
			HouseType strangeHouse = new HouseType("strangehouse", 3, 15, 50, 156, 575);
			strangeHouse.AddCost("stone", 300);
			HouseType candyHouse = new HouseType("candyhouse", 11, 11, 67, 60, 539);
			candyHouse.AddCost("stone", 400);
			candyHouse.AddCost("copper", 50);
			HouseType basicHouse = new HouseType("basichouse", 11, 11, 9, 10, 501);
			basicHouse.AddCost("stone", 400);
			HouseType brickHouse = new HouseType("brickhouse", 11, 11, 1024, 1023, 647);
			brickHouse.AddCost("stone", 400);
			HouseType coinHouse = new HouseType("coinhouse", 5, 5, 41, 100, 581);
			coinHouse.AddCost("stone", 400);

			this.houseManager.RegisterHouseType(tinyHouse);
			this.houseManager.RegisterHouseType(smallHouse);
			this.houseManager.RegisterHouseType(mediumHouse);
			this.houseManager.RegisterHouseType(largeHouse);
			this.houseManager.RegisterHouseType(veryLargeHouse);
			this.houseManager.RegisterHouseType(hugeHouse);

			this.houseManager.RegisterHouseType(weirdHouse);
			this.houseManager.RegisterHouseType(strangeHouse);
			this.houseManager.RegisterHouseType(candyHouse);
			this.houseManager.RegisterHouseType(basicHouse);
			this.houseManager.RegisterHouseType(brickHouse);
			this.houseManager.RegisterHouseType(coinHouse);

			EnableTick(5000);

		}


		public override void onEnable()
		{
			return;
		}

		public override void onDisable()
		{
			connected = false;
			return;
		}

		public override void onConnect()
		{
			dateConnected = DateTime.Now;
			connected = true;
			return;
		}

		public override void onDisconnect(string reason)
		{
			connected = false;
			return;
		}

		public override void onMessage(PlayerIOClient.Message m)
		{
			switch (m.Type)
			{
				case "m":
					{
						int userId = m.GetInt(0);
						float playerPosX = m.GetFloat(1);
						float playerPosY = m.GetFloat(2);
						float speedX = m.GetFloat(3);
						float speedY = m.GetFloat(4);
						float modifierX = m.GetFloat(5);
						float modifierY = m.GetFloat(6);
						float horizontal = m.GetFloat(7);
						float vertical = m.GetFloat(8);
						int Coins = m.GetInt(9);

						int blockX = (int)(playerPosX / 16 + 0.5);
						int blockY = (int)(playerPosY / 16 + 0.5);

						IPlayer player = bot.Room.getPlayer(userId);
						if (player == null || player.IsGod || player.IsMod)
							return;



						int blockId = (bot.Room.getBlock(0, blockX + (int)horizontal, blockY + (int)vertical).Id);

						{
							int x1 = blockX;
							int y1 = blockY;
							int x2 = blockX + (int)horizontal;
							int y2 = blockY + (int)vertical;

							if (horizontal + vertical == 1 || horizontal + vertical == -1)
							{
								houseManager.OnPlayerMine(player, x1, y1, x2, y2);

								if (speedX * horizontal == 0 && speedY * vertical == 0)
									houseManager.OnPlayerPush(player, x1, y1, (int)horizontal, (int)vertical);
							}
						}
					}
					break;


				case "b":
					{
						int layer = m.GetInt(0);
						int x = m.GetInt(1);
						int y = m.GetInt(2);
						int blockId = m.GetInt(3);
						if (m.Count >= 5)
						{
							int userId = m.GetInt(4);

							houseManager.OnBlockPlace(bot.Room.getPlayer(userId), x, y, layer, blockId);
						}
					}
					break;

				default:
					break;
			}
		}

		public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
		{
			switch (cmd)
			{
				case "loadhouses":
					if (cmdSource is IPlayer)
					{
						IPlayer player = cmdSource as IPlayer;
						if (player.IsOp)
							houseManager.Load();
					}
					break;
				case "helphouse":
				case "househelp":
					if (cmdSource is IPlayer)
					{
						IPlayer player = cmdSource as IPlayer;
						player.Send("House commands: !houseinfo, !build, !finishhouse, !edithouse, !destroyhouse, !place");
					}
					break;
				case "deletehouse":
				case "removehouse":
				case "destroyhouse":
					if (cmdSource is IPlayer)
					{
						IPlayer builder = cmdSource as IPlayer;
						houseManager.DestroyHouse(builder);
					}
					break;
				case "changehouse":
				case "edithouse":
					if (cmdSource is IPlayer)
					{
						IPlayer builder = cmdSource as IPlayer;
						houseManager.EditHouse(builder);
					}
					break;
				case "buildhouse":
				case "build":
					if (cmdSource is IPlayer)
					{
						IPlayer builder = cmdSource as IPlayer;
						if (args.Count() >= 1)
						{
							int width = 12;
							int height = 12;

							int x = builder.BlockX - width;
							int y = builder.BlockY - height;

							string houseType = args[0];

							if (houseManager.BuildHouse(builder, houseType))
								builder.Reply("say !finishhouse when you're done!");
						}
						else
							houseManager.ShowHouses(builder);
					}
					break;
				case "placefurniture":
				case "placef":
				case "place":
					if (cmdSource is IPlayer)
					{
						IPlayer player = cmdSource as IPlayer;
						if (args.Length >= 1)
						{
							string furnitureType = args[0];

                            Furniture furniture = (Furniture)Activator.CreateInstance(furnitureManager.GetFurnitureType(furnitureType).GetType(), new object[] {-1, -1});
							if (furnitureType != null)
							{
								House house = houseManager.FindHouse(player.BlockX, player.BlockY);
								if (house != null)
								{
									if (house.IsValidFurniturePosition(player.BlockX, player.BlockY))
									{
										BlockPos pos = new BlockPos(0, player.BlockX, player.BlockY);
										furniture.X = pos.X;
										furniture.Y = pos.Y;
										bot.Room.setBlock(pos.X, pos.Y, furniture.getBlock(bot, player, house));
										if (house.Furniture.ContainsKey(pos))
										{
											if (house.Furniture[pos].Type != "empty")
												player.Reply("Replaced old furniture.");
											else
												player.Reply("Furniture placed.");

											house.Furniture[pos] = furniture;
										}
										else
										{
											house.Furniture.Add(pos, furniture);
											player.Reply("Furniture placed.");
										}
										houseManager.Save();
									}
									else
										player.Reply("You can't place furniture there.");
								}
								else
									player.Reply("You must be inside your house to place furniture!");
							}
							else
							{
								string s = "You can place: ";
								foreach (var v in FurnitureManager.FurnitureTypes)
									s += v.Key + ", ";
								s = s.Remove(s.Length - 3, 2);
								player.Reply(s);
							}
						}
						else
						{
							string s = "You can place: ";
							foreach (var v in FurnitureManager.FurnitureTypes)
								s += v.Key + ", ";
							s = s.Remove(s.Length - 3, 2);
							player.Reply(s);
						}
					}
					break;
				case "finishouse":
				case "finishhouse":
					if (cmdSource is IPlayer)
					{
						IPlayer builder = cmdSource as IPlayer;
						houseManager.FinishHouse(builder);
					}
					break;
				case "houseinfo":
					if (cmdSource is IPlayer && args.Length >= 1)
					{
						IPlayer player = cmdSource as IPlayer;

						HouseType houseType = houseManager.GetHouseType(args[0]);

						if (houseType == null)
						{
							houseManager.ListHouseTypes(player);
						}
						else
						{
							IInventoryContainer inventoryPlayer = (IInventoryContainer)player.GetMetadata("digplayer");
							player.Reply(houseType.Name + "   " + "Size: " + houseType.Width + "*" + houseType.Height);
							houseType.PrintCost(player, inventoryPlayer.Inventory);
						}
					}
					break;

				default:
					break;
			}
		}

		public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
		{
			return;
		}

		public override void onTick()
		{
			if (connected && (DateTime.Now - dateConnected).Seconds > 2)
			{
				bot.ChatSayer.Say("Loading houses..");
				houseManager.Load();
				DisableTick();
			}
			return;
		}

		public override bool HasTab
		{
			get { return false; }
		}


		public override string SubBotName
		{
			get { return "HouseBuilding"; }
		}
	}
}
