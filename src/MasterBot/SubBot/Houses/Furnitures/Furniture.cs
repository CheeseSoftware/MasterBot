using MasterBot.IO;
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
		private int x;
		private int y;

		public Furniture(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public abstract void OnPush(IBot bot, IPlayer player, House house, int dx, int dy);

		public abstract IBlock getBlock(IBot bot, IPlayer player, House house);

		public abstract Furniture FromNode(Node node);

		public abstract string Type { get; }

		public int X { get { return this.x; } }

		public int Y { get { return this.y; } }
	}
}
