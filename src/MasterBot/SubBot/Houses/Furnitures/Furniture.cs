using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses.Furnitures
{
	public abstract class Furniture
	{
		public abstract void OnPush(IBot bot, IPlayer player, House house, int x, int y, int dx, int dy);
		public abstract IBlock getBlock(IBot bot, IPlayer player, House house);
	}
}
