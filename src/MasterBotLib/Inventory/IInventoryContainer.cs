using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Inventory
{
	public interface IInventoryContainer
	{
		IInventory Inventory { get; }
	}
}
