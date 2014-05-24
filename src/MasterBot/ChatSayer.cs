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
        const int interval = 500;
        IBot bot;
        Thread chatThread;
        Queue<string> messages = new Queue<string>();

        public ChatSayer(IBot bot)
        {
            this.bot = bot;

            chatThread = new Thread(() =>
                {
                    while (bot.Connected)
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
            if (chatThread.ThreadState == ThreadState.Background
                || chatThread.ThreadState == ThreadState.Running
                || chatThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                this.chatThread.Abort();

            }
        }
    }
}
