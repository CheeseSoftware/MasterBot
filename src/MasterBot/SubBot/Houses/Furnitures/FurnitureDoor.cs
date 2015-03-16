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
            int blockId = bot.Room.getBlock(0, x + 2 * dx, y + 2 * dy).Id;

            IPlayer builder = bot.Room.getPlayer(house.builder);
            if (builder == null)
                return;

            if (blockId == 414 || blockId == 4)
            {
                int xx = x + 2 * dx + 1;
                int yy = y + 2 * dy + 1;
                string text = "/teleport " + player.Name + " " + xx + " " + yy;
                HousePlayer housePlayer = HousePlayer.Get(builder);
                if (player.Name == house.builder || housePlayer.IsTrusted(player.Name))
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
