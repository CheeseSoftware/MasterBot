﻿using System;
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
        int blockPackToUse;
        Random r = new Random();

        public static int[][] blockPacks = new int[][] { 
            new int[] { 20, 21, 19, 17, 1023, 18, 1022, 1024 },  //brick blocks 
            //new int[] { 142, 138, 1022, 159, 30, 29, 31, 74 },  //digable blocks 
            new int[] { 182, 1021, 1024, 33, 1026, 50, 136 },  //dark blocks 
            new int[] { 119, 416, 369, 202, 203, 204, 196, 197, 198 },  //liquid blocks 
            new int[] { 23, 24, 25, 1005, 1006, 1007, 27, 7, 1018, 10, 11, 12, 13, 14, 15, 416, 119 },  //doorgatekey+basic blocks 
            new int[] { 176, 177, 178, 179, 416 },  //mars blocks 
            new int[] { 9, 9, 9, 9, 68 }, // gray basic, blood blocks 
            new int[] { 9, 10, 11, 12, 13, 14, 15, 1018, 182 }, // basic blocks 
            new int[] { 16, 16, 35 }, // dirt and grass blocks 
            new int[] { 166, 167, 168, 169, 170, 171}, // pipes blocks 
            new int[] { 162, 163, 193, 199, 211, 212}, // non-square blocks 
            new int[] { 146, 147, 148, 149, 150, 151, 152, 153}, // industrial blocks 
            new int[] { 202, 203, 204, 416, 416, 416, 416, 416, 119}, // lava blocks 
            new int[] { 416, 203}, // deadly blocks 
            new int[] { 92, 92, 92, 92, 92, 261}, // prison blocks 
            new int[] { 114, 115, 116, 117, 114, 115, 116, 117, 114, 115, 116, 117, 114, 115, 116, 117, 416, 119}, // boostup blocks 
            //new int[] { 0, 0, 0, 0, 0 }, // gravity down blocks 
            //new int[] { 262, 263, 264, 265, 266, 267, 268, 269, 270, 54, 55}, // glass gravity down blocks 


            new int[] { 9, 42, 1022, 46, 68, 92, 95, 159, 144, 186, 193, 195 }  //gray blocks
        };


        public GodPlayer(IPlayer player, int x, int y, int blockId)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.oldX = x;
            this.oldY = y;
            this.blockPackToUse = r.Next(blockPacks.Length);
        }

        public void UpdatePosition()
        {
            oldX = x;
            oldY = y;
            x = player.BlockX;
            y = player.BlockY;
        }

        public void DrawLine(IBot bot, IBlockDrawer blockDrawer, Random random, List<BlockPos> blocksToRemove)
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

            int dx = x2 - x1;
            int dy = y2 - y1;

            if (dx == 0 && dy == 0)
                return;

            if (dx > dy)
            {
                for (int xx = x1; xx <= x2; ++xx)
                {
                    int yy = y + sign * ((dy * (xx - x)) / dx);
                    if (random.Next(10) >= 8)
                        continue;

                    int otherId = bot.Room.getBlock(0, xx, yy).Id;

                    if (otherId != 4 && otherId != 414)
                            continue;

                    BlockWithPos block = new BlockWithPos(xx, yy, new NormalBlock(BlockId));
                    blockDrawer.PlaceBlock(block);
                    blocksToRemove.Add(new BlockPos(0, xx, yy));

                }
            }
            else
            {
                for (int yy = y1; yy <= y2; ++yy)
                {
                    int xx = x + sign * ((dx * (yy - y)) / dy);
                    if (random.Next(100) >= 98)
                        continue;

                    int otherId = bot.Room.getBlock(0, xx, yy).Id;

                     if (otherId != 4 && otherId != 414)
                            continue;

                    BlockWithPos block = new BlockWithPos(xx, yy, new NormalBlock(BlockId));
                    blockDrawer.PlaceBlock(block);
                    blocksToRemove.Add(new BlockPos(0, xx, yy));

                }
            }
        }

        public int BlockId { get {/* return 1022;*/ return blockPacks[blockPackToUse][r.Next(blockPacks[blockPackToUse].Length)]; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int OldX { get { return oldX; } }
        public int OldY { get { return oldY; } }
        public IPlayer Player { get { return player; } }
    }

    public class RunFromGods : ASubBot
    {
        Dictionary<IPlayer, GodPlayer> gods = new Dictionary<IPlayer, GodPlayer>();
        List<IPlayer> survivors = new List<IPlayer>();
        List<IPlayer> playersThatDied = new List<IPlayer>();
        List<BlockPos> blocksToRemove = new List<BlockPos>();

        enum State
        {
            Start,
            RunFromSpawn,
            RunFromGods,
            End
        }
        State state = State.End;
        float startTime = 1;
        float runFromSpawnTime = 3;
        float runFromGodsTime = 60;
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

                switch (this.state)
                {
                    case State.Start:
                        this.stateTime = startTime;
                        this.InitStart();
                        break;
                    case State.RunFromSpawn:
                        if (survivors.Count < 3)
                        {
                            this.bot.Say("There are not enough players");
                            this.bot.Say("At least 3 players must be in the room.");
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
                        this.stateTime = 20;
                        this.InitEnd();
                        break;
                }
            }

            switch (this.state)
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
            this.bot.Command("/reset");
            this.bot.Connection.Send(bot.Room.WorldKey + "r");

            survivors.Clear();

            foreach (IPlayer player in bot.Room.Players)
                survivors.Add(player);

            foreach (IPlayer player in this.survivors)
            {
                if (player.AfkStopwatch.Elapsed.Seconds > 60)
                {
                    player.Reply("You are afk!");
                    playersThatDied.Add(player);
                }
            }


            foreach (IPlayer player in playersThatDied)
            {
                survivors.Remove(player);
            }
        }
        private void InitRunFromSpawn()
        {
            this.bot.Connection.Send(bot.Room.WorldKey + "r");

            
        }
        private void InitRunFromGods()
        {
            // Kill the afkers which are now "dead".
            foreach (IPlayer player in playersThatDied)
            {
                bot.Command("/kill " + player.Name);
            }
            playersThatDied.Clear();

            for (int i = 0; i < 4 && i < survivors.Count - 2; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    int randomIndex = random.Next(survivors.Count);
                    IPlayer player = survivors[randomIndex];
                    


                    // afker
                    if (player.AfkStopwatch.Elapsed.Seconds > 30)
                        continue;

                    survivors.RemoveAt(randomIndex);

                    GodPlayer godPlayer = new GodPlayer(player, player.BlockX, player.BlockY, random.Next(9, 21));
                    this.gods.Add(player, godPlayer);

                    bot.Command("/godon " + player.Name);
                    bot.Say(player.Name + " is god! RUN!!!");
                    player.Reply("You are a god, your goal is to block in the players!");

                    break;
                }
            }
        }
        private void InitEnd()
        {
            foreach (GodPlayer god in this.gods.Values)
            {
                bot.Command("/godoff " + god.Player.Name);
            }
            this.gods.Clear();
            this.survivors.Clear();
            this.bot.Command("/reset");
            this.bot.Command("/loadlevel");


            foreach (BlockPos blockPos in blocksToRemove)
            {
                int blockId = bot.Room.getBlock(0, blockPos.X, blockPos.Y).Id;

                if (blockId == 368)
                    this.blockDrawer.PlaceBlock(new BlockWithPos(blockPos.X, blockPos.Y, new NormalBlock(414)));
            }
            blocksToRemove.Clear();
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
                god.DrawLine(this.bot, this.blockDrawer, this.random, this.blocksToRemove);
            }

            

            foreach(IPlayer player in this.survivors)
            {


                bool dead = true;

                for (int i = 0; i < 9; ++i)
                {
                    int x = i % 3 + player.BlockX - 1;
                    int y = i / 3 + player.BlockY - 1;

                    if (x == 1 && y == 1)
                        continue;

                    int blockId = bot.Room.getBlock(0, x, y).Id;

                    if (blockId == 4 || blockId == 414)
                        dead = false;

                }
                if (dead || bot.Room.getPlayer(player.Id) == null)
                    playersThatDied.Add(player);
                else if (player.AfkStopwatch.Elapsed.Seconds > 5)
                {
                    player.Reply("You were afk or hiding for too long!");
                    playersThatDied.Add(player);
                }
            }

            foreach(IPlayer player in playersThatDied)
            {
                survivors.Remove(player);
                bot.Command("/kill " + player.Name);
                player.Reply("You died! :P");
            
            }
            playersThatDied.Clear();

            // Restart
            if (survivors.Count <= 1)
                this.stateTime = 0;

            if (survivors.Count == 1)
            {
                IPlayer winner = survivors[0];
                bot.Say(winner.Name + " won the game!");
                bot.Command("/givecrown " + winner.Name);
            }


        }


    }
}
