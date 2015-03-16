using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses.Furnitures
{
    public class FurnitureSpawn : Furniture
    {

        public override IBlock getBlock(IBot bot, IPlayer player, House house)
        {
            return new NormalBlock(360, 0);
        }

        public override void OnPush(IBot bot, IPlayer player, House house, int x, int y, int dx, int dy)
        {
        }

        public override string getType()
        {
            return "spawn";
        }
    }
}
