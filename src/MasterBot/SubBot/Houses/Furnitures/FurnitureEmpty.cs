using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.IO;
using MasterBot.Room.Block;

namespace MasterBot.SubBot.Houses.Furnitures
{
	class FurnitureEmpty : Furniture
	{
		public FurnitureEmpty(int x, int y)
			: base(x, y)
		{

		}

		public override void OnPush(IBot bot, IPlayer player, House house, int dx, int dy)
		{
		}

		public override IBlock getBlock(IBot bot, IPlayer player, House house)
		{
			return new NormalBlock(414, 0);
		}

		public override Furniture FromNode(Node node)
		{
			return new FurnitureEmpty(int.Parse(node.Nodes["x"].Value), int.Parse(node.Nodes["y"].Value));
		}

		public override string Type { get { return "empty"; } }
	}
}
