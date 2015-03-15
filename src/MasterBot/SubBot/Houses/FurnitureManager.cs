using MasterBot.Room.Block;
using MasterBot.SubBot.Houses.Furnitures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses
{
	class FurnitureManager
	{
		private IBot bot = null;
		private static Dictionary<string, Furniture> furnitureTypes = new Dictionary<string, Furniture>();

		public FurnitureManager(IBot bot)
		{
			this.bot = bot;

			furnitureTypes.Add("door", new FurnitureLockedDoor());
			furnitureTypes.Add("switch", new FurnitureSwitch());
			furnitureTypes.Add("switchdoor", new FurnitureSwitchDoor());
		}

		public Furniture GetFurnitureType(string type)
		{
			if (furnitureTypes.ContainsKey(type))
				return furnitureTypes[type];
			return null;
		}

		public Dictionary<string, Furniture> FurnitureTypes { get { return furnitureTypes; } }

	}
}