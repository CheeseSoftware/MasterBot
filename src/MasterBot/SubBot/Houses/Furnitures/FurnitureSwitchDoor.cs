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
		public override void OnPush(IBot bot, IPlayer player, House house, int x, int y, int dx, int dy)
		{
			int blockId = bot.Room.getBlock(0, x + 2*dx, y + 2*dy).Id;

			if (blockId == 414 || blockId == 4)
			{
				int xx = x + 2*dx + 1;
				int yy = y + 2*dy + 1;
				string text = "/teleport " + player.Name + " " + xx + " " + yy;
				if (player == house.builder)
					bot.ChatSayer.Say(text);
			}
		}

		public override IBlock getBlock(IBot bot, IPlayer player, House house)
		{
			return new BlockSwitchDoor(player.Id % 100);
		}
	}
}
