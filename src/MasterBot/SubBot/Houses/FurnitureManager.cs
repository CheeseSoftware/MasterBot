using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses
{
	abstract class Furniture
	{
		protected House house = null;

		protected int x;
		protected int y;

		public abstract void OnPush(IBot bot, IPlayer player, int dx, int dy);
		public abstract IBlock getBlock();

		public void setHouse(House house)
		{
			this.house = house;
		}

		public int X { get { return x; } }
		public int Y { get { return y; } }
		public House House { get { return house; } }
	}

	class LockedDoor : Furniture
	{
		public LockedDoor(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override void OnPush(IBot bot, IPlayer player, int dx, int dy)
		{
			int blockId = bot.Room.getBlock(0, x + dx, y + dy).Id;

			if (blockId == 414 || blockId == 4)
			{
				int xx = x + dx + 1;
				int yy = y + dy + 1;
				string text = "/teleport " + player.Name + " " + xx + " " + yy;
				if (player == house.builder)
					bot.Say(text);
			}

		}

		public override IBlock getBlock()
		{
			return new NormalBlock(45);
		}
	}

	class SwitchDoor : Furniture
	{
		int playerId;

		public SwitchDoor(IPlayer placer, int x, int y)
		{
			this.x = x;
			this.y = y;
			this.playerId = placer.Id;
		}

		public override void OnPush(IBot bot, IPlayer player, int dx, int dy)
		{
			int blockId = bot.Room.getBlock(0, x + dx, y + dy).Id;

			if (blockId == 414 || blockId == 4)
			{
				int xx = x + dx + 1;
				int yy = y + dy + 1;
				string text = "/teleport " + player.Name + " " + xx + " " + yy;
				if (player == house.builder)
					bot.Say(text);
			}

		}

		public override IBlock getBlock()
		{
			return new BlockSwitchDoor(playerId % 100);
		}
	}

	class Switch : Furniture
	{
		int playerId;

		public Switch(IPlayer placer, int x, int y)
		{
			this.x = x;
			this.y = y;
			this.playerId = placer.Id;
		}

		public override void OnPush(IBot bot, IPlayer player, int dx, int dy)
		{
			return;
		}

		public override IBlock getBlock()
		{
			return new BlockSwitch(playerId % 100);
		}
	}

	class FurnitureManager
	{
		IBot bot = null;
		HouseManager houseManager;
		Dictionary<BlockPos, Furniture> furnitures = new Dictionary<BlockPos, Furniture>();

		public FurnitureManager(IBot bot, HouseManager houseManager)
		{
			this.bot = bot;
			this.houseManager = houseManager;
		}

		public bool PlaceFurniture(IPlayer player, Furniture furniture)
		{
			House house = houseManager.FindHouse(furniture.X, furniture.Y);

			lock (this)
			{
				if (house == null)
				{
					player.Reply("You're not inside any house!");
					return false;
				}

				if (house.builder != player)
				{
					player.Reply("You can only place furnitures in your own houses!");
					return false;
				}

				if (furnitures.ContainsKey(new BlockPos(0, furniture.X, furniture.Y)))
				{
					player.Reply("There's already a furniture there somehow...");
					return false;
				}

				bot.Room.setBlock(furniture.X, furniture.Y, furniture.getBlock());

				furniture.setHouse(house);
				furnitures.Add(new BlockPos(0, furniture.X, furniture.Y), furniture);
				return true;
			}
		}

		public void OnPlayerPush(IPlayer player, int x, int y, int dx, int dy)
		{
			lock (this)
			{
				BlockPos pos = new BlockPos(0, x + dx, y + dy);

				if (furnitures.ContainsKey(pos))
				{
					Furniture furniture = furnitures[pos];

					furniture.OnPush(bot, player, dx, dy);
				}
			}
		}

		public void OnBlockPlace(int x, int y, int layer, int blockId)
		{
			lock (this)
			{
				BlockPos pos = new BlockPos(layer, x, y);
				if (this.furnitures.ContainsKey(pos))
				{
					Furniture furniture = furnitures[pos];
					if (blockId != furniture.getBlock().Id)
						furnitures.Remove(pos);
				}
			}
		}

	}
}