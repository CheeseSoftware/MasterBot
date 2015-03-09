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

        public HouseType(string name, int width = 9, int height = 9, int baseBlock = 16, int wallBlock = 9, int backgroundBlock = 555, HashSet<int> alloweGroundBlocks = null)
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
                alloweGroundBlocks = new HashSet<int>();
                alloweGroundBlocks.Add(4);
                alloweGroundBlocks.Add(414);
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
    public struct House
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
    struct CurrentMiningBlock {
        public int x;
        public int y;
        public int health;

        public CurrentMiningBlock(int x, int y) {
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
        Dictionary<string, HouseType> houseTypes;


        public HouseManager(IBot bot)
        {
            this.bot = bot;

            HouseType smallHouse = new HouseType("smallhouse", 13, 9);
            HouseType mediumHouse = new HouseType("mediumhouse", 13, 21);
            HouseType largeHouse = new HouseType("largehouse", 21, 35);

            RegisterHouseType(smallHouse);
            RegisterHouseType(mediumHouse);
            RegisterHouseType(largeHouse);
        }

        public bool BuildHouse(IPlayer builder, string houseTypeStr)
        {


            if (buildingHouses.ContainsKey(builder))
                return false;


            HouseType houseType = null;

            if (!houseTypes.ContainsKey(houseTypeStr))
            {
                builder.Reply("There is no building called '" + houseTypeStr"'!")
                
                List<string> houseTypes;

                foreach(HouseType h in houseTypes)
                    houseTypes.Add(h);

                string text = "";

                //for (int i = 0; i < houseTypes.Count-1; ++i) {
                //    text += houseTypes[i] + ", ";

                //    if (i%4 == 0) {
                //        builder.Reply(text);
                //        text = "";
                //    }
                //}

                return false;
            }

            houseType = houseTypes[houseType];

            House house = new House(HouseState.Building, builder, x, y, width, height);

            if (!isValidHousePosition(house))
                return false;

            buildingHouses.Add(builder, house);
            houses.Add(house);

            for (int xx = 0; xx < width; ++xx)
            {
                for (int yy = 0; yy < height; ++yy)
                {
                    if (builder.BlockX == x + xx && builder.BlockY == y + yy)
                        continue;

                    if (xx == 0 || xx == width - 1 || yy == 0 || yy == height - 1)
                    {
                        this.bot.Room.BlockDrawer.PlaceBlock(
                            new Room.Block.BlockWithPos(xx + x, yy + y,
                                new Room.Block.NormalBlock(45)));
                    }
                    else
                    {
                        this.bot.Room.BlockDrawer.PlaceBlock(
                            new Room.Block.BlockWithPos(xx + x, yy + y,
                                new Room.Block.NormalBlock(93)));

                    }


                }
                return true;
            }

        }

        public void OnPlayerMine(IPlayer player, int x1, int y1, int x2, int y2)
        {
            if (buildingHouses.ContainsKey(player)) {
                House house = buildingHouses[player];
                if (x2 >= house.x && y2 >= house.y && x2 < house.x + house.width && y2 < house.y + house.height)
                {
                    CurrentMiningBlock currentMiningBLock = new CurrentMiningBlock(x2, y2);

                    if (miningBlocks.ContainsKey(player)) {
                        currentMiningBLock = miningBlocks[player];

                        if (currentMiningBLock.x != x2 || currentMiningBLock.y != y2)
                            currentMiningBLock = new CurrentMiningBlock(x2, y2);
                    }
                    else {
                        miningBlocks.Add(player, currentMiningBLock);
                    }

                    currentMiningBLock.health--;
                    miningBlocks[player] = currentMiningBLock;

                    if (currentMiningBLock.health == 0) {
                        miningBlocks.Remove(player);
                        bot.Room.BlockDrawer.PlaceBlock(
                            new Room.Block.BlockWithPos(x2, y2,
                                new Room.Block.NormalBlock(414)));
                    }


                }
            }
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

                    return false;
                }
            }

            foreach (var isValidPos in isValidPosEvent)
            {
                if (!isValidPos(house))
                    return false;
            }

            return true;
        }

        public bool RegisterHouseType(HouseType houseType) {
            if (houseTypes.ContainsKey(houseType.Name))
                return false;

            houseTypes.Add(houseType.Name, houseType);
            return true;
        }

    }
}
