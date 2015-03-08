using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses
{
    enum HouseState
    {
        Building,
        Painting,
        Finished
    }
    struct House
    {
        public HouseState houseState;
        public IPlayer builder;
        public int x;
        public int y;
        public int width;
        public int height;

        public House(HouseState houseState,
            IPlayer builder,
            int x,
            int y,
            int width,
            int height)
        {
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

    class HouseManager
    {
        List<House> houses = new List<House>();
        Dictionary<IPlayer, House> buildingHouses = new Dictionary<IPlayer, House>();
        Dictionary<IPlayer, CurrentMiningBlock> miningBlocks = new Dictionary<IPlayer, CurrentMiningBlock>();
        IBot bot = null;


        public HouseManager(IBot bot)
        {
            this.bot = bot;
        }

        public bool BuildHouse(IPlayer builder, int x, int y, int width, int height)
        {
            if (buildingHouses.ContainsKey(builder))
            {
                return false;
            }
            else
            {
                buildingHouses.Add(builder, new House(HouseState.Building, builder, x, y, width, height));

                for (int xx = x; xx < x + width; ++xx)
                {
                    for (int yy = y; yy < y + height; ++yy)
                    {
                        if (xx == x + width / 2 && yy == y + height / 2)
                            continue;

                        this.bot.Room.BlockDrawer.PlaceBlock(
                            new Room.Block.BlockWithPos(xx, yy,
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
    }
}
