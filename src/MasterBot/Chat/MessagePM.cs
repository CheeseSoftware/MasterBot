using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Chat
{
	class MessagePM : Message
	{
		private IPlayer _receiver;

		public MessagePM(string message, IPlayer receiver)
			: base(message)
		{
			_receiver = receiver;
		}

		public IPlayer Receiver { get { return _receiver; } }
	}
}
