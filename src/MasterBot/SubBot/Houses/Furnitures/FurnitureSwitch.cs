using MasterBot.IO;
using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses.Furnitures
{
	class FurnitureSwitch : Furniture
	{

		public FurnitureSwitch(int x, int y)
			: base(x, y)
		{

		}

		public override void OnPush(IBot bot, IPlayer player, House house, int dx, int dy)
		{
			return;
		}

		public override IBlock getBlock(IBot bot, IPlayer player, House house)
		{
			return new BlockSwitch((player != null ? player.Id % 100 : 0));
		}

		public override Furniture FromNode(Node node)
		{
			return new FurnitureSwitch(int.Parse(node.Nodes["x"].Value), int.Parse(node.Nodes["y"].Value));
		}

		public override string Type { get { return "switch"; } }
	}
}
