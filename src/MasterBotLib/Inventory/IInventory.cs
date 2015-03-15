using MasterBot.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Inventory
{
	public interface IInventory
	{
		IInventoryItem GetItem(string name);
		int GetItemCount(string name);

		int GetItemCount(IInventoryItem item);

		List<Pair<IInventoryItem, int>> GetItems();

		bool RemoveItem(IInventoryItem item, int amount);

		bool RemoveItem(string item, int amount);

		bool AddItem(IInventoryItem item, int amount);

		bool Contains(string item);

		bool Contains(IInventoryItem item);

		SaveFile Save(SaveFile saveFile);

		void Load(SaveFile file);

	}
}
