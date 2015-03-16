using MasterBot.IO;
using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses.Furnitures
{
	class FurnitureSwitchDoor : Furniture
	{
		public FurnitureSwitchDoor(int x, int y)
			: base(x, y)
		{

		}

		public override void OnPush(IBot bot, IPlayer player, House house, int dx, int dy)
		{
			int blockId = bot.Room.getBlock(0, X + 2 * dx, Y + 2 * dy).Id;

			if (blockId == 414 || blockId == 4)
			{
				int xx = X + 2 * dx + 1;
				int yy = Y + 2 * dy + 1;
				string text = "/teleport " + player.Name + " " + xx + " " + yy;
				if (player.Name == house.builder)
					bot.ChatSayer.Say(text);
			}
		}

		public override IBlock getBlock(IBot bot, IPlayer player, House house)
		{
			return new BlockSwitchDoor((player != null ? player.Id % 100 : 0));
		}

		public override Furniture FromNode(Node node)
		{
			return new FurnitureSwitchDoor(int.Parse(node.Nodes["x"].Value), int.Parse(node.Nodes["y"].Value));
		}

		public override string Type { get { return "switchdoor"; } }
	}
}
