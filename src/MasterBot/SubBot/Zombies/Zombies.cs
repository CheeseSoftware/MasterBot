using MasterBot.SubBot;
using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    public class Zombies : ASubBot
    {
        public static List<Zombie> zombieList = new List<Zombie>();
        public static Stopwatch zombieUpdateStopWatch = new Stopwatch();
        public static Stopwatch zombieDrawStopWatch = new Stopwatch();
        Random r = new Random();

        public Zombies(IBot bot) 
            : base(bot)
        {
            Enabled = true;
            EnableTick(100);
            zombieUpdateStopWatch.Start();
            zombieDrawStopWatch.Start();
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmdSource is Player)
            {
                Player player = (Player)cmdSource;
                switch (cmd)
                {
                    case "zombie":
                        {
                            Zombie zombie = new Zombie(bot, player.BlockX * 16, player.BlockY * 16);
                            lock (zombieList)
                            {
                                zombieList.Add(zombie);
                            }
                            bot.Room.setBlock(player.BlockX, player.BlockY, new NormalBlock(32));
                        }
                        break;
                    case "zombies":
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                int x = r.Next(1, bot.Room.Width - 1);
                                int y = r.Next(1, bot.Room.Height - 1);
                                Zombie zombie = new Zombie(bot, x * 16, y * 16);
                                lock (zombieList)
                                {
                                    zombieList.Add(zombie);
                                }
                            }
                        }
                        break;
                    case "removezombies":
                        {
                            lock (zombieList)
                            {
                                zombieList.Clear();
                            }
                        }
                        break;
                }
            }
        }

        public override void onEnable()
        {
        }

        public override void onDisable()
        {
        }

        public override void onConnect()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
        }

        public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
        }

        public override void onTick()
        {
            if (bot.Connected)
            {
                long lag = 0;
                lock (zombieList)
                {
                    foreach (Zombie zombie in zombieList)
                    {
                        zombie.Update();
                        zombie.Draw();
                        System.Threading.Thread.Sleep((int)(200 / zombieList.Count) - (int)lag);
                    }
                }
                lag = zombieUpdateStopWatch.ElapsedMilliseconds;
                zombieUpdateStopWatch.Restart();
                //Console.WriteLine(lag);
            }
        }

        public override bool HasTab
        {
            get { return false; }
        }

        public override string SubBotName
        {
            get { return "Zombies"; }
        }
    }
}
