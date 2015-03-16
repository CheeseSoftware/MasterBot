﻿using MasterBot.Room.Block;
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
			furnitureTypes.Add("empty", new FurnitureEmpty());
            furnitureTypes.Add("spawn", new FurnitureSpawn());
		}

		public Furniture GetFurnitureType(string type)
		{
			if (furnitureTypes.ContainsKey(type))
				return furnitureTypes[type];
			return null;
		}

        public void PrintFurnitures(ICmdSource receiver)
        {
            string s = "You can place: ";
            foreach (var pair in furnitureTypes)
                s += pair.Key + ", ";
            s = s.Remove(s.Length - 3, 2);
            receiver.Reply(s);
        }

		public static Dictionary<string, Furniture> FurnitureTypes { get { return furnitureTypes; } }

	}
}