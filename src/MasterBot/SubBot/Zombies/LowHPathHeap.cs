using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Zombies
{
    class LowHPathHeap : List<Node>
    {
        //List<Node> data = new List<Node>();
        //Dictionary<Node, int> nodeFinder = new Dictionary<Node, int>();

        public LowHPathHeap()
        {
            base.Add(null);
        }

        public void Add(Node node)
        {
            base.Add(node);
            HeapUp(Count - 1);
        }

        public Node GetRemoveFirst()
        {
            if (Count > 1)
            {
                Node result = base[1];
                base[1] = base[base.Count - 1];
                base.RemoveAt(base.Count - 1);
                HeapDown(1);
                return result;
            }
            return null;
        }

        /*public bool HasNode(Node node)
        {
            for (int i = 1; i < data.Count; i++)
                if (data[i].x == node.x && data[i].y == node.y)
                    return true;
            return false;
        }*/

        public Node GetNode(int x, int y)
        {
            for (int i = 1; i < base.Count; i++)
                if (base[i].x == x && base[i].y == y)
                    return base[i];
            return null;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 1; i < base.Count; i++)
                s += base[i].F + " ";
            return s;
        }

        public bool IsEmpty()
        {
            return base.Count == 1;
        }

        private void HeapUp(int old)
        {
            /*int current = old / 2;
            if (current != 0 && base[old].H < base[current].H)
            {
                Node buffer = base[current];
                base[current] = base[old];
                base[old] = buffer;
                HeapUp(current);
            }*/
            /*
            int current = old / 2;
            while (current >= 1 && base[old].H < base[current].H)
            {
                Node buffer = base[current];
                base[current] = base[old];
                base[old] = buffer;
                current = old / 2;
            }*/
            int current = old / 2;
            if (current != 0 && base[old].H < base[current].H)
            {
                Node buffer = base[current];
                base[current] = base[old];
                base[old] = buffer;
                HeapUp(current);
            }
        }

        private void HeapDown(int old)
        {
            int firstchild = old * 2;
            int secondchild = old * 2 + 1;

            if (firstchild < base.Count)
            {
                Node firstchildData = base[firstchild];
                Node oldData = base[old];

                int minIndex = old;

                if (oldData.H > firstchildData.H)
                {
                    minIndex = firstchild;
                }

                if ((secondchild < base.Count) && (base[minIndex].H > base[secondchild].H))
                {
                    minIndex = secondchild;
                }

                if (minIndex != old)
                {
                    Node temp = oldData;
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
