using MasterBot;
using MasterBot.SubBot;
using MasterBot.Room;
using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.SubBot.Zombies;

namespace MasterBot
{
    class ZombiesSubbot : ASubBot
    {
        private BlockWithPos cake = null;
        private BlockWithPos hologram = null;
        public List<Zombie> zombies = new List<Zombie>();

        public override bool HasTab
        {
            get { return false; }
        }

        public override string SubBotName
        {
            get { return "Zombie Plugin"; }
        }

        public ZombiesSubbot(IBot bot)
            : base(bot)
        {
            // Adds the SubBot to the SubBot handler so the SubBot will get callbacks.
            bot.SubBotHandler.AddSubBot(this, true);

            if (this.HasTab)
                this.InitializeComponent();

            // 10ms is the player physics.
            this.EnableTick(10);
        }

        public override void onEnable()
        {
            // Initialize as much as possible here.
        }

        public override void onDisable()
        {
            // Clean up
        }

        public override void onConnect()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock)
        {
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmdSource is IPlayer)
            {
                IPlayer player = (IPlayer)cmdSource;
                switch (cmd)
                {
                    case "setcake":
                        {
                            cake = new BlockWithPos(player.BlockX, player.BlockY, new NormalBlock(337, 0));
                            bot.Room.setBlock(player.BlockX, player.BlockY, new NormalBlock(337, 0));
                        }
                        break;
                    case "sethologram":
                        {
                            hologram = new BlockWithPos(player.BlockX, player.BlockY, new NormalBlock(337, 0));
                            bot.Room.setBlock(player.BlockX, player.BlockY, new NormalBlock(337, 0));
                        }
                        break;
                    case "eatme":
                        MakeZombie(player);
                        break;
                    case "zombie":
                        zombies.Add(new Zombie(this, player.BlockX, player.BlockY, bot));
                        break;
                }
            }
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "face":
                    {
                        int playerId = m.GetInt(0);
                        int faceId = m.GetInt(1);

                        IPlayer player = bot.Room.getPlayer(playerId);
                        if (player != null)
                        {
                            //If player changed to zombie smiley but isn't zombie, put on cake
                            if (!isZombieFace(faceId))
                            {
                                if (player.HasMetadata("isZombie"))
                                {
                                    if (player.GetMetadata("isZombie").Equals(false))
                                        ForceToNormal(player);
                                }
                            }
                            else
                            {
                                //If player changed to other than zombie smiley but is zombie, put on hologram
                                if (player.HasMetadata("isZombie"))
                                {
                                    if (player.GetMetadata("isZombie").Equals(true))
                                    {
                                        if (player.HasMetadata("forcing"))
                                            break;

                                        ForceToZombie(player);
                                    }
                                }
                            }

                        }
                    }
                    break;
            }
        }

        public override void onTick()
        {
            /*//foreach (var zombie in bot.Room.Players)
            foreach (Zombie zombie in zombies)
            {
                /*if (zombie.HasMetadata("ost.isZombie"))
                {
                    if (zombie.GetMetadata("ost.isZombie").Equals(false))
                        continue;
                }
                else continue;*/

               /* foreach (var human in bot.Room.Players)
                {
                    /*if (human.HasMetadata("ost.isZombie"))
                    {
                        if (human.GetMetadata("ost.isZombie").Equals(true))
                            continue;
                    }*/

                    /*if (Intersects(zombie, human))
                    {
                        float health = 100;

                        if (human.HasMetadata("ost.health"))
                            health = (float)human.GetMetadata("ost.health");

                        health -= 3;
                        if (health < 0)
                            health = 0;

                        human.SetMetadata("ost.health", health);

                        if (health == 0)
                        {
                            human.SetMetadata("ost.isZombie", true);
                            human.SetMetadata("ost.health", (float)100);
                            MakeZombie(human);
                        }
                    }
                }
            }*/
            //foreach (Zombie zombie in zombies)
              //  zombie.Tick();
        }

        /***************************************************
         * private functions:
         * 
         ********************************************************/
        private void MakeZombie(IPlayer human)
        {
            human.SetMetadata("isZombie", "true");
            bot.Say(human.Name + " has turned into a zombie! Run!!!");
            ForceToZombie(human);
        }

        private void ForceToZombie(IPlayer human)
        {
            human.SetMetadata("zombieForcing", true);
            human.SetMetadata("forcing", true);

            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(delegate
            {
                double oldX = human.BlockX;
                double oldY = human.BlockY;

                if (this.hologram != null)
                    bot.Say("/teleport " + human.Name + " " + (int)(hologram.X + 1) + " " + (int)(hologram.Y - 1));

                System.Threading.Thread.Sleep(1000);
                bot.Say("/kill " + human.Name);
                //bot.Say("/teleport " + human.Name + " " + oldX + " " + oldY);
                System.Threading.Thread.Sleep(1000);
                bot.Say("/teleport " + human.Name + " " + oldX + " " + oldY);

                human.RemoveMetadata("zombieForcing");
                human.RemoveMetadata("forcing");
            });
            task.Start();
        }

        private void ForceToNormal(IPlayer zombie)
        {
            zombie.SetMetadata("humanForcing", true);
            zombie.SetMetadata("forcing", true);

            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(delegate
            {
                double oldX = zombie.X;
                double oldY = zombie.Y;

                if (this.cake != null)
                    bot.Say("/teleport " + zombie.Name + " " + cake.X + " " + cake.Y);

                System.Threading.Thread.Sleep(500);
                bot.Say("/kill " + zombie.Name);
                //bot.Say("/teleport " + zombie.Name + " " + oldX + " " + oldY);
                System.Threading.Thread.Sleep(500);
                bot.Say("/teleport " + zombie.Name + " " + oldX + " " + oldY);

                zombie.RemoveMetadata("humanForcing");
                zombie.RemoveMetadata("forcing");
            });
            task.Start();
        }

        private bool isZombieFace(int faceId)
        {
            return (faceId == 87 ||
                (faceId >= 72 && faceId <= 75) ||
                faceId == 100);
        }

        private float IntersectsWithDistance(IPlayer a, IPlayer b)
        {
            if (a.X < b.X - 16)
                return float.NaN;
            if (a.X > b.X + 16)
                return float.NaN;

            if (a.Y < b.Y - 16)
                return float.NaN;
            if (a.Y > b.Y + 16)
                return float.NaN;

            double distance = (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
            if (distance <= 16)
                return (float)distance;

            return float.NaN;
        }

        private float IntersectsWithDistance(Zombie a, IPlayer b)
        {
            if (a.x < b.X - 16)
                return float.NaN;
            if (a.x > b.X + 16)
                return float.NaN;

            if (a.x < b.Y - 16)
                return float.NaN;
            if (a.x > b.Y + 16)
                return float.NaN;

            double distance = (a.x - b.X) * (a.x - b.X) + (a.y - b.Y) * (a.y - b.Y);
            if (distance <= 16)
                return (float)distance;

            return float.NaN;
        }

        private bool Intersects(IPlayer a, IPlayer b)
        {
            return (IntersectsWithDistance(a, b) != float.NaN);
        }

        private bool Intersects(Zombie a, IPlayer b)
        {
            return (IntersectsWithDistance(a, b) != float.NaN);
        }
    }
}
