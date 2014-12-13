using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;
using System.Threading;
using System.Diagnostics;
using MasterBot.SubBot;
using System.Drawing;

namespace MasterBot.SubBot.Zombies
{
    class Zombie
    {
        public static int searchRange = 10;

        private IBot bot;
        public int x, y;
        private int oldx, oldy;
        private int ticksToNextSearch = 0;
        private int ticksBetweenNewPath = 0;
        private IPlayer target = null;
        private PathFinding pathFinding;
        private Stack<Node> currentPath = null;
        private static int msPerTick = 200;
        private Stopwatch playerTickStopwatch = new Stopwatch();
        private ZombiesSubbot zombies;

        public Zombie(ZombiesSubbot owner, int x, int y, IBot bot)
        {
            this.zombies = owner;
            this.bot = bot;
            this.x = x;
            this.y = y;
            this.oldx = 0;
            this.oldy = 0;
            pathFinding = new PathFinding();

            GenMaze();

            System.Timers.Timer updateTimer = new System.Timers.Timer();
            updateTimer.Elapsed += delegate { Tick(); };
            playerTickStopwatch.Start();
            updateTimer.Start();
        }

        public void GenMaze()
        {
            Random random = new Random();
            Point[] moves = new Point[] { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            Point startingSquare = new Point(bot.Room.Width / 2, 1);
            Point exitSquare = new Point(bot.Room.Width / 2, bot.Room.Height - 3);
            List<Point> points = new List<Point>();


            points.Add(moves[random.Next(moves.Length)]);
            points.Add(new Point(startingSquare.X, startingSquare.Y));

            int pathBlock = 9;
            //int pathMaxLength = 5;
            int randomFactor = 8;

            while (points.Count > 0)
            {
                Point point;
                Point move;


                if (random.Next(randomFactor) == 1)
                {// like prim's algorithm
                    int i = random.Next(points.Count) >> 1 << 1;

                    point = points[i + 1];
                    move = points[i];

                    points.RemoveAt(i + 1);
                    points.RemoveAt(i);
                }
                else
                {// like depth search
                    point = points.Last();
                    points.RemoveAt(points.Count - 1);

                    move = points.Last();
                    points.RemoveAt(points.Count - 1);
                }

                Point wall = new Point(point.X, point.Y);

                point.X += 2 * move.X;
                point.Y += 2 * move.Y;

                wall.X += move.X;
                wall.Y += move.Y;

                for (int i = 0; i * i < random.Next(randomFactor) + 1; i++)
                {
                    if (point.X >= 1 && point.Y >= 1 && point.X < bot.Room.Width - 2 && point.Y < bot.Room.Height - 1)
                    {
                        IBlock b = bot.Room.getBlock(0, point.X, point.Y);
                        if (b.Id != pathBlock)
                        {
                            bot.Room.setBlock(wall.X, wall.Y, new NormalBlock(pathBlock));
                            bot.Room.setBlock(point.X, point.Y, new NormalBlock(pathBlock));

                            List<Point> newpoints = new List<Point>();
                            newpoints.AddRange(moves);

                            while (newpoints.Count > 0)
                            {
                                int index = random.Next(newpoints.Count);

                                points.Add(newpoints[index]);
                                points.Add(new Point(point.X, point.Y));

                                newpoints.RemoveAt(index);
                            }
                        }
                        else break;
                    }
                    else break;

                    point.X += 2 * move.X;
                    point.Y += 2 * move.Y;

                    wall.X += 2 * move.X;
                    wall.Y += 2 * move.Y;
                }
            }
            Console.WriteLine("Done");
        }

        public void Tick()
        {
            long elapsed = playerTickStopwatch.ElapsedMilliseconds;
            if (elapsed >= msPerTick)
            {
                playerTickStopwatch.Restart();
                ticksToNextSearch--;
                if (ticksToNextSearch <= 0)
                {
                    //Find a target
                    IPlayer target = GetNearestPlayer(searchRange);
                    if (target != null)
                    {
                        this.target = target;
                    }
                    ticksToNextSearch = 5;
                }

                //Generate path
                ticksBetweenNewPath--;
                if (ticksBetweenNewPath <= 0 || currentPath == null || currentPath.Count <= 0)
                {
                    if (target != null && !(x == target.BlockX && y == target.BlockY))
                    {
                        pathFinding = new PathFinding();
                        int xx = target.BlockX;
                        int yy = target.BlockY;
                        DateTime first = DateTime.Now;
                        currentPath = pathFinding.FindPath(x, y, xx, yy, new List<Zombie>(zombies.zombies), bot);
                        DateTime second = DateTime.Now;
                        Console.WriteLine("Pahtfinding took " + (second - first).TotalMilliseconds);
                        ticksBetweenNewPath = 1;
                    }
                    else
                        ticksBetweenNewPath = 2;
                }

                //Walk with path
                if (currentPath != null && currentPath.Count > 0)
                {
                    //bot.Room.setBlock(oldx, oldy, new NormalBlock(0));
                    Node next = currentPath.Pop();
                    oldx = x;
                    oldy = y;
                    x = next.x;
                    y = next.y;
                    while (x == oldx && y == oldy && currentPath.Count > 0)
                    {
                        next = currentPath.Pop();
                        oldx = x;
                        oldy = y;
                        x = next.x;
                        y = next.y;
                    }


                    //Console.WriteLine("Moving to x:" + x + " y:" + y);

                    //bot.Room.setBlock(oldx, oldy, new NormalBlock(0, 0));
                    //bot.Room.setBlock(x, y, new NormalBlock(32, 0));
                    //bot.Room.setBlock(oldx, oldy, new NormalBlock(0));
                    if (!(x == oldx && y == oldy))
                    {
                        if (target != null && !target.IsGod && target.BlockX == x && target.BlockY == y)
                        {
                            KillPlayer(target);
                            target = null;
                        }

                        //bot.Connection.Send(bot.Room.WorldKey, 0, x, y, 32);
                        //bot.Connection.Send(bot.Room.WorldKey, 0, oldx, oldy, 0);
                        bot.Room.setBlock(x, y, new NormalBlock(32, 0));
                        bot.Room.setBlock(oldx, oldy, new NormalBlock(0));
                    }
                }
            }
            else if (elapsed > 2)
                Thread.Sleep((int)(msPerTick - elapsed) - 1);
        }

        private void KillPlayer(IPlayer player)
        {
            bot.Connection.Send("say", "Player " + player.Name + " was brutally murdered!");
            bot.Say("/kill " + player.Name);
            bot.Say("/teleport " + player.Name + " " + 1 + " " + 1);
        }

        private IPlayer GetNearestPlayer(int range)
        {
            ICollection<IPlayer> players = bot.Room.Players;
            //Queue<IPlayer> closePlayers = new Queue<IPlayer>();
            //SortedList<IPlayer, int> closePlayers = new SortedList<IPlayer, int>();
            Dictionary<IPlayer, int> closePlayers = new Dictionary<IPlayer, int>();

            if (players.Count > 0)
            {
                foreach (IPlayer player in players)
                {
                    if (!player.IsGod)
                    {
                        double distance = Math.Sqrt(Math.Abs((x - player.BlockX) ^ 2 + (y - player.BlockY) ^ 2));
                        closePlayers.Add(player, (int)distance);
                    }
                }

                List<KeyValuePair<IPlayer, int>> myList = closePlayers.ToList();
                myList.Sort((firstPair, nextPair) =>
                {
                    return firstPair.Value.CompareTo(nextPair.Value);
                }
                );
                return myList.Count > 0 ? myList.First().Key : null;
            }
            return null;
        }
    }
}
