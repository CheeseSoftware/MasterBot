using MasterBot.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Zombies
{
    public struct Position
    {
        public int x;
        public int y;
        public int cost;

        public Position(int x, int y, int cost)
        {
            this.x = x;
            this.y = y;
            this.cost = cost;
        }
    }

    class Node : IComparable
    {
        public int x;
        public int y;
        private int g = 0;
        private int h = 0;
        private Node mother = null;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Node(int x, int y, int g, int h)
        {
            this.x = x;
            this.y = y;
            this.g = g;
            this.h = h;
        }

        public override bool Equals(object obj)
        {
            return ((Node)obj).x == x && ((Node)obj).y == y;
        }

        public override int GetHashCode()
        {
            Int32 temp = x | (y << 16);
            return temp.GetHashCode();
        }

        public int F { get { return g + h; } }
        public int H { get { return h; } set { h = value; } }
        public int G { get { return g; } set { g = value; } }
        public Node Mother { get { return mother; } set { mother = value; } }

        public int CompareTo(object obj)
        {
            return (F < ((Node)obj).F ? 1 : 0);
        }
    }

    class PathFinding
    {
        //public static int wallCost = 10000;

        private Position[] adjacentNodes = new Position[8] { 
        new Position(1, 1, 14), 
        new Position(-1, 1, 14), 
        new Position(-1, -1, 14),
        new Position(1, -1, 14),
        new Position(1, 0, 10), 
        new Position(-1, 0, 10),
        new Position(0, 1, 10),
        new Position(0, -1, 10)};

        private PathHeap open = new PathHeap();
        private LowHPathHeap search = new LowHPathHeap();
        //private SortedSet<Node> open = new SortedSet<Node>();


        //private HashSet<Node> closed = new HashSet<Node>();
        private bool[,] isOpen;
        private bool[,] isClosed;
        private int[,] gValues;

        public Queue<Node> debug = new Queue<Node>();

        public Stack<Node> FindPath(int xs, int ys, int xt, int yt, ICollection<Zombie> zombies, IBot bot)
        {
            int maxSearch = 2560;
            int currentSearch = 0;
            bool isStartingSquare = true;

            isOpen = new bool[bot.Room.Width, bot.Room.Height];
            isClosed = new bool[bot.Room.Width, bot.Room.Height];
            gValues = new int[bot.Room.Width, bot.Room.Height];

            debug = new Queue<Node>();
            open = new PathHeap();
            search = new LowHPathHeap();
            //open = new SortedSet<Node>();

            //closed = new HashSet<Node>();
            Node start = new Node(xs, ys);
            start.H = Heuristic(xs, ys, xt, yt);
            Node target = new Node(xt, yt);
            isOpen[start.x, start.y] = true;
            open.Add(start);
            search.Add(start);

            while (!open.IsEmpty() && currentSearch < maxSearch)
            //while (open.Count > 0)
            {
                Node current = open.GetRemoveFirst();
                //Node current = open.First();
                //open.Remove(current);
                isOpen[current.x, current.y] = false;
                isClosed[current.x, current.y] = true;
                if (current.Equals(target))
                    return GetPath(current, bot);
                for (int i = 4; i < 8; i++)
                {
                    int x = adjacentNodes[i].x + current.x;
                    int y = adjacentNodes[i].y + current.y;
                    //Console.WriteLine("X:" + x + " Y:" + y + " " + room.GetMapBlock(x, y));
                    int id = bot.Room.BlockMap.getForegroundBlockIdFast(x, y);
                    if (x >= 0 && x <= bot.Room.Width - 1 && y >= 0 && y <= bot.Room.Height - 1 && (id == 4 || id == 32) && (isStartingSquare ? !isZombie(x, y, zombies): true))
                    {
                        int totalAddCost = adjacentNodes[i].cost;// + (room.GetMapBlock(x, y) != 0 ? wallCost : 0);
                        Node baby = new Node(x, y);
                        if (isClosed[baby.x, baby.y])
                            continue;
                        if (!isOpen[baby.x, baby.y])
                        {
                            int addg = totalAddCost;
                            baby.G = current.G + addg;
                            gValues[baby.x, baby.y] = baby.G;
                            baby.H = Heuristic(baby.x, baby.y, target.x, target.y);
                            baby.Mother = current;
                            open.Add(baby);
                            //bot.Connection.Send(bot.Room.WorldKey, 0, baby.x, baby.y, 9);
                            //System.Threading.Thread.Sleep(10);
                            isOpen[baby.x, baby.y] = true;
                            //debug.Enqueue(baby);
                            search.Add(baby);
                            ++currentSearch;
                        }
                        else
                        {
                            int addg = totalAddCost;
                            if (current.G + addg < gValues[baby.x, baby.y])
                            {
                                //Path to that Node is better, switch parents!
                                baby.Mother = current;
                            }
                        }
                    }
                }
                isStartingSquare = false;

            }
            //Target was not found, use closest search node
            if (!search.IsEmpty())
                return GetPath(search.GetRemoveFirst(), bot);
            return null;
        }

        private Boolean isZombie(int x, int y, ICollection<Zombie> zombies)
        {
            foreach(Zombie zombie in zombies)
            {
                if(zombie.x == x && zombie.y == y)
                    return true;
            }
            return false;
        }

        private Stack<Node> GetPath(Node target, IBot bot)
        {
            //bot.Connection.Send(bot.Room.WorldKey, 0, target.x, target.y, 9);
            //System.Threading.Thread.Sleep(10);
            //room.DrawSquare(target.x, target.y, System.Drawing.Color.Black);
            Stack<Node> result = new Stack<Node>();
            Node current = target;
            result.Push(current);
            while (current.Mother != null)
            {
                /*if (current.F >= wallCost)
                    result.Clear();*/
                result.Push(current.Mother);
                current = current.Mother;
            }
            return result;
        }

        private Node GetLowestF(ICollection<Node> list)
        {
            int lowest = int.MaxValue;
            Node lowestNode = null;
            foreach (Node s in list)
            {
                if (s.F < lowest)
                {
                    lowest = s.F;
                    lowestNode = s;
                }
            }
            return lowestNode;
        }

        private int Heuristic(int currentX, int currentY, int targetX, int targetY)
        {
            //return 10 * (Math.Abs(targetX - currentX) + Math.Abs(targetY - currentY));
            int H;
            int xDistance = Math.Abs(currentX-targetX);
            int yDistance = Math.Abs(currentY - targetY);
            if (xDistance > yDistance)
                 H = 14*yDistance + 10*(xDistance-yDistance);
            else
                 H = 14*xDistance + 10*(yDistance-xDistance);
            return H;
        }
    }


}
