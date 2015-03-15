using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Chat
{
	class Message
	{
		private string _message;
		public Message(string message)
		{
			_message = message;
		}

		public string Content { get { return _message; } }
	}
}
