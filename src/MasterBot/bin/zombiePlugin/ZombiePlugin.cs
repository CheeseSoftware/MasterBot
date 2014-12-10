using MasterBot;
using MasterBot.SubBot;
using MasterBot.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zombiePlugin
{
    class ZombiePlugin : ASubBot, IPlugin
    {
        MasterBot.Room.Block.NormalBlock cake = null;
        int cakeX = 1;
        int cakeY = 1;

        public override bool HasTab
        {
            get { return false; }
        }

        public override string SubBotName
        {
            get { return "Zombie Plugin"; }
        }

        public ZombiePlugin() : base(null) { }

        public void PerformAction(IBot bot)
        {
            // This is required since I can't fix this in the constructor. :/
            this.bot = bot;

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

        public override void onBlockChange(int x, int y, MasterBot.Room.Block.IBlock newBlock, MasterBot.Room.Block.IBlock oldBlock)
        {
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
            switch(m.Type)
            {
                case "face":
                    {
                        int playerId = m.GetInt(0);
                        int faceId = m.GetInt(1);

                        IPlayer player = bot.Room.getPlayer(playerId);
                        if (player != null)
                        {
                            if (isZombieFace(faceId))
                            {
                                object isZombie = player.GetMetadata("isZombie");
                                if (!isZombie.Equals(true))
                                {
                                    makeZombie(player);
                                }
                            }

                            if (player.GetMetadata("isZombie").Equals(true))
                            {
                                if (player.GetMetadata("isForcing").Equals(true))
                                    break;

                                forceToZombie(player);
                            }
                            
                        }
                    }
                    break;
            }
        }

        public override void onTick()
        {
            foreach(var zombie in bot.Room.Players)
            {
                if (!zombie.HasMetadata("ost.isZombie"))
                {
                    zombie.SetMetadata("ost.isZombie", false);
                    continue;
                }

                if (!zombie.GetMetadata("ost.isZombie").Equals(true))
                    continue;

                foreach(var human in bot.Room.Players)
                {
                    if (human.GetMetadata("ost.isZombie").Equals(true))
                        continue;

                    if (intersects(zombie, human))
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
                            makeZombie(human);
                        }
                    }
                }
            }
        }

        /***************************************************
         * private functions:
         * 
         ********************************************************/
        private void makeZombie(IPlayer human)
        {
            

            

            human.SetMetadata("isZombie", "true");

            




            bot.Say(human.Name + " has turned into a zombie! Run!!!");
            forceToZombie(human);

            
        }

        private void forceToZombie(IPlayer human)
        {
            human.SetMetadata("zombieForcing", "true");

            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(delegate
            {
                double oldX = human.X;
                double oldY = human.Y;

                if (this.cake != null)
                    bot.Say("/teleport " + human.Name + " " + this.cakeX + " " + this.cakeY);

                System.Threading.Thread.Sleep(500);
                bot.Say("/kill " + human.Name);
                bot.Say("/teleport " + human.Name + " " + oldX + " " + oldY);
                System.Threading.Thread.Sleep(500);
                bot.Say("/teleport " + human.Name + " " + oldX + " " + oldY);
                
                human.SetMetadata("zombieForcing", "false");
            });
            task.Start();
        }

        private bool isZombieFace(int faceId)
        {
            return (faceId == 87 ||
                (faceId >= 72 && faceId <= 75) ||
                faceId == 100);
        }
        
        
        private float intersectsWithDistance(IPlayer a, IPlayer b)
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

        private bool intersects(IPlayer a, IPlayer b)
        {
            return (intersectsWithDistance(a, b) != float.NaN);
        }
    }
}
