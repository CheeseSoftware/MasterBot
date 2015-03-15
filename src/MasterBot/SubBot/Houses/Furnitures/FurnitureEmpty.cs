using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.SubBot.Houses.Furnitures
{
	class FurnitureEmpty : Furniture
	{
		public override IBlock getBlock(IBot bot, IPlayer player, House house)
		{
			return new NormalBlock(414, 0);
		}

		public override string getType()
		{
			return "empty";
		}

		public override void OnPush(IBot bot, IPlayer player, House house, int x, int y, int dx, int dy)
		{
		}
	}
}
