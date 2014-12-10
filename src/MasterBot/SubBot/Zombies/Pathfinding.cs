using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room;

namespace MasterBot
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

    class Node
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
            Int32 temp = x | (y<<16);
            return temp.GetHashCode();
        }

        public int F { get { return g + h; } }
        public int H { get { return h; } set { h = value; } }
        public int G { get { return g; } set { g = value; } }
        public Node Mother { get { return mother; } set { mother = value; } }
    }

    class PathFinding
    {
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
        //private HashSet<Node> closed = new HashSet<Node>();
        private bool[,] isOpen = new bool[64, 64];
        private bool[,] isClosed = new bool[64, 64];
        private int[,] gValues = new int[64, 64];

        public Queue<Node> debug = new Queue<Node>();

        public Stack<Node> FindPath(int xs, int ys, int xt, int yt, IRoom room)
        {
            isOpen = new bool[64, 64];
            isClosed = new bool[64, 64];

            debug = new Queue<Node>();
            open = new PathHeap();
            //closed = new HashSet<Node>();
            Node start = new Node(xs, ys);
            Node target = new Node(xt, yt);
            isOpen[start.x, start.y] = true;
            open.Add(start);

            while (!open.IsEmpty())
            {
                Node current = open.GetRemoveFirst();
                isOpen[current.x, current.y] = false;
                isClosed[current.x, current.y] = true;
                if (current.Equals(target))
                    return GetPath(current);
                for (int i = 4; i < 8; i++)
                {
                    int x = adjacentNodes[i].x + current.x;
                    int y = adjacentNodes[i].y + current.y;
                    //Console.WriteLine("X:" + x + " Y:" + y + " " + room.GetMapBlock(x, y));
                    if (x >= 0 && x <= 63 && y >= 0 && y <= 63)
                    {
                        int totalAddCost = adjacentNodes[i].cost + (room.getBlock(0, x, y).Id != 0 ? 50000 : 0);
                        Node baby = new Node(x, y);
                        if (isClosed[baby.x, baby.y])
                            continue;
                        if (!isOpen[baby.x, baby.y])
                        {
                            int addg = totalAddCost;
                            baby.G = current.G + addg;
                            gValues[baby.x, baby.y] = baby.G;
                            baby.H = Heuristic(x, y, target.x, target.y);
                            baby.Mother = current;
                            open.Add(baby);
                            isOpen[baby.x, baby.y] = true;
                            debug.Enqueue(baby);
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

            }
            //Target was not found.
            return null;

        }

        private Stack<Node> GetPath(Node target)
        {
            Stack<Node> result = new Stack<Node>();
            Node current = target;
            result.Push(current);
            while (current.Mother != null)
            {
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
            return 10 * (Math.Abs(currentX - targetX) + Math.Abs(currentY - targetY));
        }
    }


}
