using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Zombies
{
    class BinaryHeap : List<int>
    {
        //List<Node> data = new List<Node>();
        //Dictionary<Node, int> nodeFinder = new Dictionary<Node, int>();

        public BinaryHeap()
        {
            base.Add(-1);
        }

        public new void Add(int item)
        {
            if (item != null)
            {
                base.Add(item);
                HeapUp(Count - 1);
            }
        }

        public int GetRemoveFirst()
        {
            if (Count > 1)
            {
                int result = base[1];
                base[1] = base[base.Count - 1];
                base.RemoveAt(base.Count - 1);
                HeapDown(1);
                return result;
            }
            return -1;
        }

        /*public bool HasNode(Node node)
        {
            for (int i = 1; i < data.Count; i++)
                if (data[i].x == node.x && data[i].y == node.y)
                    return true;
            return false;
        }*/

        public override string ToString()
        {
            string s = "";
            for (int i = 1; i < base.Count; i++)
                s += base[i] + " ";
            return s;
        }

        public bool IsEmpty()
        {
            return base.Count == 1;
        }

        private void HeapUp(int old)
        {
            int current = old / 2;
            if (current != 0 && base[old] < base[current])
            {
                int buffer = base[current];
                base[current] = base[old];
                base[old] = buffer;
                HeapUp(current);
            }
        }

        private void HeapDown(int old)
        {
            int firstchild = old * 2;
            int secondchild = old * 2 + 1;

            if (firstchild < base.Count && firstchild > 0)
            {
                int firstchildData = base[firstchild];
                int oldData = base[old];

                int minIndex = old;

                if (oldData > firstchildData)
                {
                    minIndex = firstchild;
                }

                if ((secondchild < base.Count) && (base[minIndex] > base[secondchild]))
                {
                    minIndex = secondchild;
                }

                if (minIndex != old)
                {
                    int temp = oldData;
                    base[old] = base[minIndex];
                    base[minIndex] = temp;
                    HeapDown(minIndex);
                }
            }
        }

        //private int HeapUp(int old)
        //{
        //    int current = old / 2;
        //    if (current != 0 && data[old].F < data[current].F)
        //    {
        //        Node buffer = data[current];
        //        data[current] = data[old];
        //        data[old] = buffer;
        //        return HeapUp(current);
        //    }
        //    return old;
        //}

        //private int HeapDown(int old)
        //{
        //    int firstchild = old * 2;
        //    int secondchild = old * 2 + 1;
        //    if(firstchild < data.Count && secondchild < data.Count)
        //    {      
        //        if(!(data[old].F < data[firstchild].F && data[old].F < data[secondchild].F))
        //        {
        //            int lowerDataIndex = (data[secondchild].F < data[firstchild].F ? secondchild : firstchild);
        //            Node buffer = data[lowerDataIndex];
        //            data[lowerDataIndex] = data[old];
        //            data[old] = buffer;
        //            return HeapDown(lowerDataIndex);
        //        }
        //    }
        //    else if (firstchild < data.Count)
        //    {
        //        if (!(data[old].F < data[firstchild].F))
        //        {
        //            Node buffer = data[firstchild];
        //            data[firstchild] = data[old];
        //            data[old] = buffer;
        //            return HeapDown(firstchild);
        //        }
        //    }
        //    return old;
        //}
    }
}
