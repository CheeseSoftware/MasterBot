using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room;
using MasterBot.Room.Block;

namespace MasterBot
{
    public class Zombie : Monster
    {
        PathFinding pathFinding = new PathFinding();
        IBlock zombieBlock = null;
        IBlock zombieOldBlock = null;
        IPlayer targetBotPlayer = null;
        Stopwatch updateTimer = new Stopwatch();
        Stopwatch lagTimer = new Stopwatch();
        Stack<Node> pathToGo = null;
        IBot bot;

        public Zombie(IBot bot, int x, int y)
            : base(x, y)
        {
            this.bot = bot;
            updateTimer.Start();
            lagTimer.Start();
            zombieBlock = new NormalBlock(32); // Block.CreateBlock(0, xBlock, yBlock, 32, 0);
        }

        public static double GetDistanceBetween(IPlayer player, int targetX, int targetY)
        {
            double a = player.BlockX - targetX;
            double b = player.BlockY - targetY;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        public override void Update()
        {
            if (updateTimer.ElapsedMilliseconds >= 1000)
            {
                updateTimer.Restart();
                double lowestDistance = 0;
                IPlayer lowestDistancePlayer = null;
                lock (bot.Room.Players)
                {
                    foreach (IPlayer player in bot.Room.Players)
                    {
                        if (player.IsGod)
                            continue;
                        double currentDistance = GetDistanceBetween(player, xBlock, yBlock);
                        if (currentDistance < lowestDistance || lowestDistance == 0)
                        {
                            lowestDistance = currentDistance;
                            lowestDistancePlayer = player;
                        }
                    }
                }
                if (lowestDistancePlayer != null)
                {
                    targetBotPlayer = lowestDistancePlayer;

                }
            }

            if (targetBotPlayer != null && xBlock != targetBotPlayer.X && yBlock != targetBotPlayer.Y)
            {
                //pathFinding = null;
                pathFinding = new PathFinding();
                //lagTimer.Restart();
                pathToGo = pathFinding.FindPath(xBlock, yBlock, targetBotPlayer.BlockX, targetBotPlayer.BlockY, bot.Room);
                //Console.WriteLine("elapsed shitlagtime " + lagTimer.ElapsedMilliseconds + "MS");

                if (pathToGo != null && pathToGo.Count != 0)
                {
                    Node temp;
                    if (pathToGo.Count >= 2)
                        temp = pathToGo.Pop();
                    Node next = pathToGo.Pop();
                    xBlock = next.x;
                    yBlock = next.y;
                    zombieBlock = new NormalBlock(32); // Block.CreateBlock(0, xBlock, yBlock, 32, -1);
                    zombieOldBlock = new NormalBlock(0, 0); // Block.CreateBlock(0, xOldBlock, yOldBlock, 4, -1);
                    bot.Room.setBlock(xOldBlock, yOldBlock, zombieOldBlock);
                    System.Threading.Thread.Sleep(500);
                    bot.Room.setBlock(xBlock, yBlock, zombieBlock);

                }

                if (targetBotPlayer != null)
                {
                    if (!/*player isdead*/false && GetDistanceBetween(targetBotPlayer, xBlock, yBlock) <= 1 && !targetBotPlayer.IsGod)
                    {
                        //targetBotPlayer.killPlayer();
                        bot.Connection.Send("say", "/kill " + targetBotPlayer.Name);
                    }
                }
            }
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
