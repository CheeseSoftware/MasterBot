using MasterBot.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterBot
{
	public class ChatSayer : IChatSayer
	{
		char[] characters = new char[] { '\'', ',', '-', '.' };
		int character = 0;
		const int interval = 1000;
		IBot bot;
		SafeThread chatThread;
		Queue<Message> messages = new Queue<Message>();

		public ChatSayer(IBot bot)
		{
			this.bot = bot;

			chatThread = new SafeThread(() =>
				{
					if (bot.Connected)
					{
						lock (messages)
						{
							if (messages.Count > 0)
							{
								Message message = messages.Dequeue();

								if (message is MessageCommand)
								{
									bot.Connection.Send(PlayerIOClient.Message.Create("say", message.Content));
								}
								else
								{
									//message is PM or broadcast, may need to be split
									List<string> splitMessage = new List<string>();
									if (message is MessagePM)
									{
										MessagePM messagePM = (MessagePM)message;
										string totalMessage = message.Content;
										int length = 80 - (8 + messagePM.Receiver.Name.Length);
										while (totalMessage != "")
										{
											if (totalMessage.Length >= length)
											{
												splitMessage.Add(totalMessage.Substring(0, length));
												totalMessage = totalMessage.Remove(0, length);
											}
											else
											{
												splitMessage.Add(totalMessage);
												break;
											}
										}

										bot.Connection.Send(PlayerIOClient.Message.Create("say", "/pm " + messagePM.Receiver.Name + " " + splitMessage.First() + "  " + characters[character]));
										splitMessage.RemoveAt(0);
										foreach (string s in splitMessage)
										{
											messages.Enqueue(new MessagePM(s, messagePM.Receiver));
										}
									}
									else
									{
										string totalMessage = message.Content;
										int length = 77;
										while (totalMessage != "")
										{
											if (totalMessage.Length >= length)
											{
												splitMessage.Add(totalMessage.Substring(0, length));
												totalMessage = totalMessage.Remove(0, length);
											}
											else
											{
												splitMessage.Add(totalMessage);
												break;
											}
										}

										bot.Connection.Send(PlayerIOClient.Message.Create("say", splitMessage.First() + "  " + characters[character]));
										splitMessage.RemoveAt(0);
										foreach (string s in splitMessage)
										{
											messages.Enqueue(new Message(s));
										}
									}
								}

								character++;
								if (character > characters.Length - 1)
									character = 0;

								Thread.Sleep(interval);
							}
							else
								Thread.Sleep(1);
						}


						/*-message.Split(
						bot.Connection.Send(Message.Create("say", message));
						"/pm " + receiver.Name + " " + message + "  " + characters[character]
						message + "  " + characters[character]*/


					}
				});

			chatThread.Start();
		}


		public void Say(IPlayer receiver, string message)
		{
			if (message != null)
			{
				if (message != "")
				{
					lock (messages)
					{
						messages.Enqueue((Message)new MessagePM(message, receiver));
					}
				}
			}
		}

		public void Say(string message)
		{
			if (message != null)
			{
				if (message != "")
				{
					lock (messages)
					{
						if (message[0] == '/')
							messages.Enqueue(new MessageCommand(message));
						else
						{
							messages.Enqueue(new Message(message));
						}
					}
				}
			}
		}

		public void onDisconnect()
		{
			chatThread.Stop();
		}
	}
}
