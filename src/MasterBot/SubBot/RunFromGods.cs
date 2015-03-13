using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;
using MasterBot.Room;
using System.Timers;
using System.Diagnostics;

namespace MasterBot.SubBot
{
    class GodPlayer
    {
        IPlayer player;
        int x;
        int y;
        int oldX;
        int oldY;
        int blockId;

        public GodPlayer(IPlayer player, int x, int y, int blockId)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.oldX = x;
            this.oldY = y;
            this.blockId = blockId;
        }

        public void UpdatePosition()
        {
            oldX = x;
            oldY = y;
            x = player.BlockX;
            y = player.BlockY;
        }

        public void DrawLine(IBot bot, IBlockDrawer blockDrawer, Random random) 
        {
            int x1, x2, y1, y2;
            int sign = 1;

            if (x < oldX)
            {
                x1 = x;
                x2 = oldX;
                sign *= -1;
            }
            else
            {
                x1 = oldX;
                x2 = x;
            }

            if (y < oldY)
            {
                y1 = y;
                y2 = oldY;
                sign *= -1;
            }
            else
            {
                y1 = oldY;
                y2 = y;
            }

            int dx = x2-x1;
            int dy = y2-y1;

            if (dx == 0 && dy == 0)
                return;

            if (dx > dy)
            {
                for (int xx = x1; xx <= x2; ++xx)
                {
                    int yy = y + sign*((dy *(xx-x))/dx);
                        if (random.Next(10) >= 8)
                            continue;

                        int blockId = bot.Room.getBlock(0, xx, yy).Id;

                        if (blockId != 4 && blockId != 414)
                            continue;

                        BlockWithPos block = new BlockWithPos(xx, yy, new NormalBlock(this.blockId));
                        blockDrawer.PlaceBlock(block);
                    
                }
            }
            else
            {
                for (int yy = y1; yy <= y2; ++yy)
                {
                    int xx = x + sign*((dx *(yy-y))/dy);
                        if (random.Next(10) >= 8)
                            continue;

                        int blockId = bot.Room.getBlock(0, xx, yy).Id;

                        if (blockId != 4 && blockId != 414)
                            continue;

                        BlockWithPos block = new BlockWithPos(xx, yy, new NormalBlock(this.blockId));
                        blockDrawer.PlaceBlock(block);
                    
                }
            }
        }


        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int OldX { get { return oldX; } }
        public int OldY { get { return oldY; } }
        public IPlayer Player { get { return player; } }
    }

    public class RunFromGods : ASubBot
    {
        Dictionary<IPlayer, GodPlayer> gods = new Dictionary<IPlayer,GodPlayer>();
        List<IPlayer> survivors = new List<IPlayer>();

        enum State 
        {
            Start,
            RunFromSpawn,
            RunFromGods,
            End
        }
        State state = State.End;
        float startTime = 10;
        float runFromSpawnTime = 10;
        float runFromGodsTime = 300;
        float stateTime = 1;

        Stopwatch stateTimer = new Stopwatch();
        Random random = new Random();
        IBlockDrawer blockDrawer;


        #region properties
        public override string SubBotName
        {
            get { return "RunFromGods"; }
        }

        public override bool HasTab
        {
            get { return false; }
        }
        #endregion // properties

        public RunFromGods(IBot bot)
            : base(bot)
        {
            // 100ms is the player physics.

            this.blockDrawer = bot.Room.BlockDrawerPool.CreateBlockDrawer(127);
            

        }


        public override void onEnable()
        {
            this.EnableTick(100);

            stateTimer.Start();
            this.blockDrawer.Start();
        }

        public override void onDisable()
        {
            this.DisableTick();
            this.stateTimer.Stop();
            this.stateTime = 1;
            this.state = State.End;
            this.blockDrawer.Stop();
            this.InitEnd();
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

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            //if (cmdSource is IPlayer)
            //{
            //    IPlayer player = (IPlayer)cmdSource;
            //    if (!player.IsOp)
            //        return;
            //}
            //IPlayer arg = null;
            //if (args.Length >= 1 && bot.Room.getPlayer(args[0].Trim().ToLower()) != null)
            //{
            //    arg = bot.Room.getPlayer(args[0].Trim().ToLower());
            //}
            //if (arg != null)
            //{
            //    switch (cmd)
            //    {
                    
            //    }
            //}
            ////else
            ////  cmdSource.Reply("Could not find player");
        }

        public override void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock)
        {
        }

        public override void onTick()
        {
            if (stateTimer.Elapsed.Seconds >= stateTime)
            {

                switch (this.state)
                {
                    case State.Start:
                        this.state = State.RunFromSpawn;
                        break;
                    case State.RunFromSpawn:
                        this.state = State.RunFromGods;
                        break;
                    case State.RunFromGods:
                        this.state = State.End;
                        break;
                    case State.End:
                        this.state = State.Start;
                        break;
                }

                this.stateTimer.Restart();

                switch(this.state)
                {
                    case State.Start:
                        this.stateTime = startTime;
                        this.InitStart();
                        break;
                    case State.RunFromSpawn:
                        if (bot.Room.Players.Count < 3)
                        {
                            this.bot.Say("There are not enough players");
                            this.bot.Say("At least 5 players must be in the room.");
                            this.state = State.Start;
                        }
                        else
                        {
                            this.stateTime = runFromSpawnTime;
                            this.InitRunFromSpawn();
                        }
                        break;
                    case State.RunFromGods:
                        this.stateTime = runFromGodsTime;
                        this.InitRunFromGods();
                        break;
                    case State.End:
                        this.stateTime = 1;
                        this.InitEnd();
                        break;
                }
            }

                switch(this.state)
                {
                    case State.Start:
                        this.TickStart();
                        break;
                    case State.RunFromSpawn:
                        this.TickRunFromSpawn();
                        break;
                    case State.RunFromGods:
                        this.TickRunFromGods();
                        break;
                    case State.End:
                        break;
                }
        }

        private void InitStart()
        {
            this.bot.Say("/reset");
        }
        private void InitRunFromSpawn()
        {
            //this.bot.Connection.Send(bot.Room.WorldKey, "r");
        }
        private void InitRunFromGods()
        {
            survivors.Clear();

            foreach (IPlayer player in bot.Room.Players)
                survivors.Add(player);

            for (int i = 0; i < 5 && i < survivors.Count - 2; ++i)
            {
                int randomIndex = random.Next(survivors.Count);
                IPlayer player = survivors[randomIndex];
                survivors.RemoveAt(randomIndex);

                GodPlayer godPlayer = new GodPlayer(player, player.BlockX, player.BlockY, random.Next(9, 21));
                this.gods.Add(player, godPlayer);

                bot.Say("/godon " + player.Name);
                bot.Say(player.Name + " is god! RUN!!!");
            }
        }
        private void InitEnd()
        {
            foreach(GodPlayer god in this.gods.Values)
            {
                bot.Say("/godoff " + god.Player.Name);
            }
            this.gods.Clear();
            this.survivors.Clear();
            this.bot.Say("/reset");
        }

        private void TickStart()
        {

        }
        private void TickRunFromSpawn()
        {

        }
        private void TickRunFromGods()
        {
            foreach (GodPlayer god in this.gods.Values)
            {
                god.UpdatePosition();
                god.DrawLine(this.bot, this.blockDrawer, this.random);
            }
        }


    }
}
