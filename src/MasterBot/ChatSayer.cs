using PlayerIOClient;
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
        char[] characters = new char[] { '\'', ',', '-', '.'};
        int character = 0;
        const int interval = 1000;
        IBot bot;
        SafeThread chatThread;
        Queue<string> messages = new Queue<string>();

        public ChatSayer(IBot bot)
        {
            this.bot = bot;

            chatThread = new SafeThread(() =>
                {
                    if (bot.Connected)
                    {
                        string message;
                        lock (messages)
                        {
                            

                            if (messages.Count > 0)
                                message = messages.Dequeue();
                            else
                                message = null;
                        }

                        if (message != null)
                        {
                            if (message != "")
                            {
                                bot.Connection.Send(Message.Create("say", message));
                            }
                        }

                        Thread.Sleep(interval);
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
                        messages.Enqueue("/pm " + receiver.Name + " " + message + "  " + characters[character]);
                        character++;
                        if (character > characters.Length - 1)
                            character = 0;
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
                            messages.Enqueue(message);
                        else
                        {

                            messages.Enqueue(message + "  " + characters[character]);

                            character++;
                            if (character > characters.Length - 1)
                                character = 0;
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
