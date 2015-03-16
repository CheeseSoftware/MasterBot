using MasterBot.IO;
using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses.Furnitures
{
	class FurnitureDoor : Furniture
	{
		public FurnitureDoor(int x, int y)
			: base(x, y)
		{

		}

		public override void OnPush(IBot bot, IPlayer player, House house, int dx, int dy)
		{
			int blockId = bot.Room.getBlock(0, X + dx, Y + dy).Id;

			if (blockId == 414 || blockId == 4)
			{
				int xx = X + dx + 1;
				int yy = Y + dy + 1;
				string text = "/teleport " + player.Name + " " + xx + " " + yy;
				if (player.Name == house.builder)
					bot.ChatSayer.Say(text);
			}
		}

		public override IBlock getBlock(IBot bot, IPlayer player, House house)
		{
			return new NormalBlock(45, 0);
		}

		public override Furniture FromNode(Node node)
		{
			return new FurnitureDoor(int.Parse(node.Nodes["x"].Value), int.Parse(node.Nodes["y"].Value));
		}

		public override string Type { get { return "door"; } }
	}
}
