using MasterBot.Inventory;
using MasterBot.IO;
using MasterBot.Room.Block;
using MasterBot.SubBot.Houses.Furnitures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses
{
	public class HouseType
	{
		HashSet<int> alloweGroundBlocks;
		string name;
		int width;
		int height;
		int baseBlock;
		int wallBlock;
		int backgroundBlock;

		Dictionary<string, int> cost = new Dictionary<string, int>();

		public HouseType(string name, int width = 9, int height = 9, int wallBlock = 9, int baseBlock = 16, int backgroundBlock = 555, HashSet<int> alloweGroundBlocks = null)
		{
			this.name = name;
			this.width = width;
			this.height = height;
			this.baseBlock = baseBlock;
			this.wallBlock = wallBlock;
			this.backgroundBlock = backgroundBlock;

			if (alloweGroundBlocks != null)
			{
				this.alloweGroundBlocks = alloweGroundBlocks;
			}
			else
			{
				this.alloweGroundBlocks = new HashSet<int>();
				this.alloweGroundBlocks.Add(4);
				this.alloweGroundBlocks.Add(414);
			}

		}

		public void AddCost(string item, int value)
		{
			if (cost.ContainsKey(item))
				cost[item] += value;
			else
				cost.Add(item, value);
		}

		public bool CanBuy(IPlayer player)
		{
			//MasterDig.DigPlayer digPlayer = MasterDig.DigPlayer.FromPlayer(player);
			IInventoryContainer inventoryPlayer = (IInventoryContainer)player.GetMetadata("digplayer");

			// Check if we can buy the house.
			foreach (var pair in cost)
			{
				if (inventoryPlayer.Inventory.GetItemCount(pair.Key) < pair.Value)
				{
					player.Reply("You don't have enough resources to build " + this.name + ".");
					PrintCost(player, inventoryPlayer.Inventory);
					return false;
				}
			}


			return true;
		}

		public bool Buy(IPlayer player)
		{
			//int cost = houseType.Cost; ?

			IInventoryContainer inventoryPlayer = (IInventoryContainer)player.GetMetadata("digplayer");

			if (!CanBuy(player))
				return false;

			// Buy it.
			foreach (var pair in cost)
			{
				inventoryPlayer.Inventory.RemoveItem(pair.Key, pair.Value);
			}

			return true;
		}
		public void PrintCost(IPlayer player, IInventory inventory)
		{
			string text = "";
			foreach (var pair in cost)
			{
				text += pair.Key + ": " + inventory.GetItemCount(pair.Key) + "/" + pair.Value + ",  ";
			}
			player.Reply(text);
		}

		public bool isGroundBlockAllowed(int blockId)
		{
			return alloweGroundBlocks.Contains(blockId);
		}

		public string Name { get { return name; } }
		public int Width { get { return width; } }
		public int Height { get { return height; } }
		public int BaseBlock { get { return baseBlock; } }
		public int WallBlock { get { return wallBlock; } }
		public int BackgroundBlock { get { return backgroundBlock; } }
	}

	public enum HouseState
	{
		Building,
		Painting,
		Finished
	}
	public class House
	{
		public HouseType houseType;
		public HouseState houseState;
		public string builder;
		public int x;
		public int y;
		public int width;
		public int height;
		private Dictionary<BlockPos, Furniture> furniture = new Dictionary<BlockPos, Furniture>();

		public House(HouseType houseType,
			string builder,
			int x,
			int y,
			int width,
			int height,
			HouseState houseState = HouseState.Building)
		{
			this.houseType = houseType;
			this.houseState = houseState;
			this.builder = builder;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public Boolean IsValidFurniturePosition(int x, int y)
		{
			return x > this.x && y > this.y && x < this.x + this.width && y < this.y + this.height;
		}

		public Dictionary<BlockPos, Furniture> Furniture { get { return furniture; } }
	}
	struct CurrentMiningBlock
	{
		public int x;
		public int y;
		public int health;

		public CurrentMiningBlock(int x, int y)
		{
			this.x = x;
			this.y = y;
			health = 3;
		}
	}

	public class HouseManager
	{
		Dictionary<string, House> houses = new Dictionary<string, House>();
		Dictionary<IPlayer, House> buildingHouses = new Dictionary<IPlayer, House>();
		Dictionary<IPlayer, CurrentMiningBlock> miningBlocks = new Dictionary<IPlayer, CurrentMiningBlock>();
		IBot bot = null;
		public delegate bool isValidPosDelegate(House house);
		List<isValidPosDelegate> isValidPosEvent = new List<isValidPosDelegate>();
		Dictionary<string, HouseType> houseTypes = new Dictionary<string, HouseType>();

		// TODO: Use lock(this)

		public HouseManager(IBot bot)
		{
			this.bot = bot;
		}

		public void Save()
		{
			SaveFile saveFile = new SaveFile("data/houses");
			Node houses = new Node("houses");
			foreach (KeyValuePair<string, House> house in this.houses)
			{
				Node playername = new Node(house.Key);
				playername.AddNode("x", new Node(house.Value.x.ToString()));
				playername.AddNode("y", new Node(house.Value.y.ToString()));
				playername.AddNode("type", new Node(house.Value.houseType.Name));

				if (house.Value.Furniture.Count > 0)
				{
					Node furniture = new Node("furniture");
					foreach (KeyValuePair<BlockPos, Furniture> f in house.Value.Furniture)
					{
						Node furnitureKey = new Node(f.Key.X + "|" + f.Key.Y);
						furnitureKey.AddNode("x", new Node(f.Key.X.ToString()));
						furnitureKey.AddNode("y", new Node(f.Key.Y.ToString()));
						furnitureKey.AddNode("type", new Node(f.Value.getType()));
						furniture.AddNode(f.Key.X + "|" + f.Key.Y, furnitureKey);
					}
					playername.AddNode("furniture", furniture);
				}
				houses.AddNode(house.Key, playername);
			}
			saveFile.AddNode("houses", houses);
			saveFile.Save();
		}

		public void Load()
		{
			SaveFile saveFile = new SaveFile("data/houses");
			saveFile.Load();
			Dictionary<string, Node> nodes = saveFile.Nodes;
			if (nodes.ContainsKey("houses"))
			{
				Node houses = nodes["houses"];
				foreach (KeyValuePair<string, Node> house in houses.Nodes)
				{
					int x = int.Parse(house.Value.Nodes["x"].Value);
					int y = int.Parse(house.Value.Nodes["y"].Value);
					string type = house.Value.Nodes["type"].Value;

					House newhouse = new House(houseTypes[type], house.Key, x, y, houseTypes[type].Width, houseTypes[type].Height);

					if (house.Value.Nodes.ContainsKey("furniture"))
					{
						Node furniture = house.Value.Nodes["furniture"];
						foreach (KeyValuePair<string, Node> furnitures in furniture.Nodes)
						{
							int furniturex = int.Parse(furnitures.Value.Nodes["x"].Value);
							int furniturey = int.Parse(furnitures.Value.Nodes["y"].Value);
							string furnituretype = furnitures.Value.Nodes["type"].Value;

							Furniture newFurniture = FurnitureManager.FurnitureTypes[furnituretype];
							newhouse.Furniture.Add(new BlockPos(0, furniturex, furniturey), newFurniture);
						}
					}
					this.houses.Add(newhouse.builder, newhouse);
					DrawHouse(newhouse);
				}
			}
		}

		public void DestroyHouse(IPlayer builder)
		{
			if (houses.ContainsKey(builder.Name))
			{
				House house = houses[builder.Name];
				if (buildingHouses.ContainsKey(builder))
					buildingHouses.Remove(builder);

				for (int x = house.x; x < house.x + house.width; x++)
				{
					for (int y = house.y; y < house.y + house.height; y++)
					{
						bot.Room.setBlock(x, y, new NormalBlock(142, 0));
						bot.Room.setBlock(x, y, new NormalBlock(0, 1));
					}
				}

				houses.Remove(builder.Name);
				Save();

				builder.Send("House destroyed!");
			}
			else
				builder.Send("You have no house to destroy.");
		}

		public void EditHouse(IPlayer builder)
		{
			if (houses.ContainsKey(builder.Name))
			{
				House house = houses[builder.Name];
				buildingHouses.Add(builder, house);
				builder.Send("You are now editing your house.");
			}
			else
				builder.Send("You have no house to edit.");
		}

		public void ShowHouses(IPlayer player)
		{
			List<string> houseTypeNames = new List<string>();
			foreach (HouseType h in houseTypes.Values)
				houseTypeNames.Add(h.Name);
			string text = "You can build these houses: ";
			for (int i = 0; i < houseTypeNames.Count; ++i)
			{
				if (i == houseTypeNames.Count - 1)
					text += houseTypeNames[i];
				else
					text += houseTypeNames[i] + ", ";
			}
			player.Reply(text);
		}

		public bool BuildHouse(IPlayer builder, string houseTypeStr)
		{
			if (buildingHouses.ContainsKey(builder))
			{
				builder.Reply("You are already building a house, use !finishhouse to finish it.");
				return false;
			}

			if (houses.ContainsKey(builder.Name))
			{
				builder.Reply("You already have a house. Use !destroyhouse to delete it. ;)");
				return false;
			}

			HouseType houseType = null;

			if (!houseTypes.ContainsKey(houseTypeStr))
			{
				builder.Reply("There is no building called '" + houseTypeStr + "'!");
				ShowHouses(builder);
				return false;
			}

			houseType = houseTypes[houseTypeStr];


			// Make sure the player cna buy the house!
			if (!houseType.CanBuy(builder))
			{
				//builder.Reply("You don't have enough resources to build " + houseType.Name + ".");
				//houseType.PrintCost(builder);
				return false;
			}

			int x = builder.BlockX - houseType.Width / 2;
			int y = builder.BlockY - houseType.Height / 2;

			House house = new House(houseType, builder.Name, x, y, houseType.Width, houseType.Height, HouseState.Building);
			if (!isValidHousePosition(house))
				return false;
			buildingHouses.Add(builder, house);
			houses.Add(builder.Name, house);
			DrawHouse(house, builder);
			bot.ChatSayer.Command("/tp " + builder.Name + " " + builder.BlockX + " " + builder.BlockY);

			// Nothing went wrong, let the player pay for his house.
			houseType.Buy(builder);

			return true;
		}

		public void DrawHouse(House house, IPlayer builder = null)
		{
			for (int xx = 0; xx < house.houseType.Width; ++xx)
			{
				for (int yy = 0; yy < house.houseType.Height; ++yy)
				{
					int blockId = 22;
					int backgroundBlock = house.houseType.BackgroundBlock;

					if (xx == 0 || xx == house.houseType.Width - 1 || yy == 0 || yy == house.houseType.Height - 1)
					{
						blockId = house.houseType.WallBlock;
					}
					else
					{
						blockId = house.houseType.BaseBlock;
					}

					if (builder == null || (builder.BlockX != house.x + xx || builder.BlockY != house.y + yy))
					{
						this.bot.Room.BlockDrawer.PlaceBlock(
								new Room.Block.BlockWithPos(xx + house.x, yy + house.y,
									new Room.Block.NormalBlock(blockId)));
					}

					this.bot.Room.BlockDrawer.PlaceBlock(
							new Room.Block.BlockWithPos(xx + house.x, yy + house.y,
								new Room.Block.NormalBlock(backgroundBlock)));

				}
			}
			foreach (var v in house.Furniture)
			{
				bot.Room.setBlock(v.Key.X, v.Key.Y, v.Value.getBlock(bot, builder, house));
			}
		}

		public void FinishHouse(IPlayer builder)
		{
			if (buildingHouses.ContainsKey(builder))
			{
				House house = buildingHouses[builder];
				if (builder.BlockX < house.x || builder.BlockX >= house.x + house.width
					|| builder.BlockY < house.y || builder.BlockY >= house.y + house.height)
				{
					buildingHouses.Remove(builder);
					Save();
					builder.Reply("You got a nice house there! :P");
				}
				else
				{
					builder.Reply("You must go out from your house to finish it!");
				}
			}
			else
				builder.Reply("You are not building any house. Say '!build house' to build a house.");
		}

		public void OnPlayerMine(IPlayer player, int x1, int y1, int x2, int y2)
		{
			if (buildingHouses.ContainsKey(player))
			{
				House house = buildingHouses[player];
				if (x2 >= house.x && y2 >= house.y && x2 < house.x + house.width && y2 < house.y + house.height)
				{
					CurrentMiningBlock currentMiningBLock = new CurrentMiningBlock(x2, y2);

					if (miningBlocks.ContainsKey(player))
					{
						currentMiningBLock = miningBlocks[player];

						if (currentMiningBLock.x != x2 || currentMiningBLock.y != y2)
							currentMiningBLock = new CurrentMiningBlock(x2, y2);
					}
					else
					{
						miningBlocks.Add(player, currentMiningBLock);
					}

					currentMiningBLock.health--;
					miningBlocks[player] = currentMiningBLock;

					if (currentMiningBLock.health == 0)
					{
						BlockPos pos = new BlockPos(0, x2, y2);
						if (!house.Furniture.ContainsKey(pos))
							house.Furniture.Add(pos, new FurnitureEmpty());
						else
							house.Furniture[pos] = new FurnitureEmpty();
						Save();
						miningBlocks.Remove(player);
						bot.Room.BlockDrawer.PlaceBlock(
							new Room.Block.BlockWithPos(x2, y2,
								new Room.Block.NormalBlock(414)));
					}


				}
			}
			//else
			//    furnitureManager.OnPlayerPush(x1, y1, x2, y2);
		}

		bool isValidHousePosition(House house)
		{
			foreach (House other in houses.Values)
			{
				bool xIntersects = false;
				bool yIntersects = false;

				if (house.x >= other.x && house.x < other.x + other.width)
					xIntersects = true;

				if (other.x >= house.x && other.x < house.x + house.width)
					xIntersects = true;

				if (house.y >= other.y && house.y < other.y + other.width)
					yIntersects = true;

				if (other.y >= house.y && other.y < house.y + house.width)
					yIntersects = true;

				if (xIntersects && yIntersects)
				{
					IPlayer player = bot.Room.getPlayer(house.builder);
					if (player != null)
					{
						string builder = house.builder;
						string neighbor = other.builder;

						player.Reply("Your house would be too close to " + neighbor + "'s house.");
						player.Reply("The size of the house is " + house.width + "x" + house.height + " blocks.");
					}

					return false;
				}
			}

			for (int x = 0; x < house.width; ++x)
			{
				for (int y = 0; y < house.height; ++y)
				{
					int bx = house.x + x;
					int by = house.y + y;

					int blockId = bot.Room.getBlock(0, bx, by).Id;

					if (!house.houseType.isGroundBlockAllowed(blockId))
					{
						IPlayer player = bot.Room.getPlayer(house.builder);
						if (player != null)
						{
							player.Reply("You must build the house on empty space without dirt and ores!");
							player.Reply("The size of the house is " + house.width + "x" + house.height + " blocks.");
						}
						return false;
					}
				}
			}

			foreach (var isValidPos in isValidPosEvent)
			{
				if (!isValidPos(house))
					return false;
			}

			return true;
		}

		public bool RegisterHouseType(HouseType houseType)
		{
			if (houseTypes.ContainsKey(houseType.Name))
				return false;

			houseTypes.Add(houseType.Name, houseType);
			return true;
		}

		public HouseType GetHouseType(string name)
		{
			if (!houseTypes.ContainsKey(name))
				return null;
			else
				return houseTypes[name];
		}

		public void ListHouseTypes(IPlayer player)
		{
			player.Reply("TODO: ListHouseTypes");
		}

		public House FindHouse(int x, int y)
		{
			lock (this)
			{
				foreach (House house in houses.Values)
				{
					if (x >= house.x && x < house.x + house.width && y >= house.y && y < house.y + house.height)
						return house;
				}
			}
			return null;
		}

		public void OnPlayerPush(IPlayer player, int x, int y, int dx, int dy)
		{
			lock (this)
			{
				BlockPos pos = new BlockPos(0, x + dx, y + dy);
				House house = FindHouse(pos.X, pos.Y);

				if (house != null && house.Furniture.ContainsKey(pos))
				{
					Furniture furniture = house.Furniture[pos];

					furniture.OnPush(bot, player, house, x, y, dx, dy);
				}
			}
		}

		public void OnBlockPlace(IPlayer player, int x, int y, int layer, int blockId)
		{
			lock (this)
			{
				BlockPos pos = new BlockPos(layer, x, y);
				House house = FindHouse(pos.X, pos.Y);

				if (house != null && house.Furniture.ContainsKey(pos))
				{
					Furniture furniture = house.Furniture[pos];
					if (blockId != furniture.getBlock(bot, player, house).Id)
					{
						house.Furniture.Remove(pos);
					}
				}
			}
		}

	}
}