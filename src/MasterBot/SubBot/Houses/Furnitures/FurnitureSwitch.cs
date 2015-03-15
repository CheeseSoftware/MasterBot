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
		public override void OnPush(IBot bot, IPlayer player, House house, int x, int y, int dx, int dy)
		{
			return;
		}

		public override IBlock getBlock(IBot bot, IPlayer player, House house)
		{
			return new BlockSwitch(player.Id % 100);
		}

		public override string getType()
		{
			return "switch";
		}
	}
}
