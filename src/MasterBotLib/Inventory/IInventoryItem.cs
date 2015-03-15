using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Inventory
{
	public interface IInventoryItem
	{
		string Name { get; }

		Dictionary<string, object> GetData();

		object GetData(string key);

		Boolean HasData(string key);

		void SetData(Dictionary<string, object> data);

		void SetData(string key, object value);
    }
}
