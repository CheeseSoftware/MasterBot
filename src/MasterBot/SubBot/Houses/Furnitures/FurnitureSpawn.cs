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
        public FurnitureSpawn(int x, int y)
			: base(x, y)
		{

		}

        public override IBlock getBlock(IBot bot, IPlayer player, House house)
        {
            return new NormalBlock(360, 0);
        }

        public override void OnPush(IBot bot, IPlayer player, House house, int dx, int dy)
        {
        }

        public override Furniture FromNode(IO.Node node)
        {
            return new FurnitureSpawn(int.Parse(node.Nodes["x"].Value), int.Parse(node.Nodes["y"].Value));
        }

        public override string Type
        {
            get { return "spawn"; }
        }
    }
}
