using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    class PathHeap
    {
        List<Node> data = new List<Node>();
        //Dictionary<Node, int> nodeFinder = new Dictionary<Node, int>();

        public PathHeap()
        {
            data.Add(null);
        }

        public void Add(Node node)
        {
            data.Add(node);
            HeapUp(data.Count - 1);
        }

        public Node GetRemoveFirst()
        {
            if (data.Count > 1)
            {
                Node result = data[1];
                data[1] = data[data.Count - 1];
                data.RemoveAt(data.Count - 1);
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
            for (int i = 1; i < data.Count; i++)
                if (data[i].x == x && data[i].y == y)
                    return data[i];
            return null;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 1; i < data.Count; i++)
                s += data[i].F + " ";
            return s;
        }

        public bool IsEmpty()
        {
            return data.Count == 1;
        }

        private void HeapUp(int old)
        {
            int current = old / 2;
            if (current != 0 && data[old].F < data[current].F)
            {
                Node buffer = data[current];
                data[current] = data[old];
                data[old] = buffer;
                HeapUp(current);
            }
        }

        private void HeapDown(int old)
        {
            int firstchild = old * 2;
            int secondchild = old * 2 + 1;
            if (firstchild < data.Count && secondchild < data.Count)
            {
                if (!(data[old].F < data[firstchild].F && data[old].F < data[secondchild].F))
                {
                    int lowerDataIndex = (data[secondchild].F < data[firstchild].F ? secondchild : firstchild);
                    Node buffer = data[lowerDataIndex];
                    data[lowerDataIndex] = data[old];
                    data[old] = buffer;
                    HeapDown(lowerDataIndex);
                }
            }
            else if (firstchild < data.Count)
            {
                if (!(data[old].F < data[firstchild].F))
                {
                    Node buffer = data[firstchild];
                    data[firstchild] = data[old];
                    data[old] = buffer;
                    HeapDown(firstchild);
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
