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
		public IPlayer builder;
		public int x;
		public int y;
		public int width;
		public int height;

		public House(HouseType houseType,
			IPlayer builder,
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

	//enum isHouseValidReasonEnum
	//{
	//    BadLocation,
	//    ToNearNeighbor,
	//    NotEnoughResources
	//}

	//interface class InvalidHouseReason
	//{
	//}

	//class InvalidHouseBadLocation : InvalidHouseReason{
	//    public string badLocation = "something";
	//}

	//class InvalidHouseBadLocation : InvalidHouseReason{
	//    public string neighbor = "neighbors";
	//}

	//class InvalidHouseNotEnoughResources : InvalidHouseReason {
	//    List<string, int>
	//}

	public class HouseManager
	{
		List<House> houses = new List<House>();
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

		public void EditHouse(IPlayer builder)
		{
			bool editSuccess = false;
			foreach (House house in houses)
			{
				if (house.builder.Name.Equals(builder.Name))
				{
					buildingHouses.Add(builder, house);
					editSuccess = true;
				}
			}
			if (editSuccess)
				builder.Send("You are now able to edit your houses.");
			else
				builder.Send("You have no houses to edit.");
		}

		public bool BuildHouse(IPlayer builder, string houseTypeStr)
		{
			return false;
			if (buildingHouses.ContainsKey(builder))
				return false;


			HouseType houseType = null;

			if (!houseTypes.ContainsKey(houseTypeStr))
			{
				builder.Reply("There is no building called '" + houseTypeStr + "'!");

				List<string> houseTypeNames = new List<string>();

				foreach (HouseType h in houseTypes.Values)
					houseTypeNames.Add(h.Name);

				string text = "You can build this: ";

				for (int i = 0; i < houseTypeNames.Count; ++i)
				{
					if (i == houseTypeNames.Count - 1)
						text += houseTypeNames[i];
					else
						text += houseTypeNames[i] + ", ";

					if (i % 4 == 3 || i == 2 || i == houseTypeNames.Count - 1)
					{
						builder.Reply(text);
						text = "";
					}
				}

				return false;
			}

			houseType = houseTypes[houseTypeStr];

			//////////TITTA HÄRRRRRRRRRRRRRRRRRR
			//////////TITTA HÄRRRRRRRRRRRRRRRRRR
			//////////TITTA HÄRRRRRRRRRRRRRRRRRR

			//int cost = houseType.Cost; ?
			/*MasterDig.DigPlayer digPlayer = MasterDig.DigPlayer.FromPlayer(builder);
			if (digPlayer.inventory.GetItemCount("stone") > 5)
			{
				digPlayer.inventory.RemoveItem("stone", 5);
			}
			else
			{
				builder.Reply("You do not have enough resources to build this house type!");
				return false;
			}*/

			//////////TITTA HÄRRRRRRRRRRRRRRRRRR
			//////////TITTA HÄRRRRRRRRRRRRRRRRRR
			//////////TITTA HÄRRRRRRRRRRRRRRRRRR

			int x = builder.BlockX - houseType.Width / 2;
			int y = builder.BlockY - houseType.Height / 2;

			House house = new House(houseType, builder, x, y, houseType.Width, houseType.Height, HouseState.Building);

			if (!isValidHousePosition(house))
				return false;

			buildingHouses.Add(builder, house);
			houses.Add(house);

			for (int xx = 0; xx < houseType.Width; ++xx)
			{
				for (int yy = 0; yy < houseType.Height; ++yy)
				{



					int blockId = 22;
					int backgroundBlock = houseType.BackgroundBlock;

					if (xx == 0 || xx == houseType.Width - 1 || yy == 0 || yy == houseType.Height - 1)
					{
						blockId = houseType.WallBlock;
					}
					else
					{
						blockId = houseType.BaseBlock;
					}

					if (builder.BlockX != x + xx || builder.BlockY != y + yy)
					{
						this.bot.Room.BlockDrawer.PlaceBlock(
								new Room.Block.BlockWithPos(xx + x, yy + y,
									new Room.Block.NormalBlock(blockId)));
					}

					this.bot.Room.BlockDrawer.PlaceBlock(
							new Room.Block.BlockWithPos(xx + x, yy + y,
								new Room.Block.NormalBlock(backgroundBlock)));

				}
			}

			bot.Say("/tp " + builder.Name + " " + builder.BlockX + " " + builder.BlockY);

			return true;
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
			foreach (House other in houses)
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
					IPlayer builder = house.builder;
					IPlayer neighbor = other.builder;

					builder.Reply("Your house would be too close to " + neighbor.Name + "'s house.");
					house.builder.Reply("The size of the house is " + house.width + "x" + house.height + " blocks.");

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
						house.builder.Reply("You must build the house on empty space without dirt and ores!");
						house.builder.Reply("The size of the house is " + house.width + "x" + house.height + " blocks.");
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

		public House FindHouse(int x, int y)
		{
			lock (this)
			{
				foreach (House house in houses)
				{
					if (x >= house.x && x < house.x + house.width && y >= house.y && y < house.y + house.height)
						return house;
				}
			}
			return null;
		}

	}
}