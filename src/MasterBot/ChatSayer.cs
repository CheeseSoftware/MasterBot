﻿using PlayerIOClient;
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
        const int interval = 500;
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
                        messages.Enqueue("/pm " + receiver.Name + " " + message);
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
                        messages.Enqueue(message);
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
