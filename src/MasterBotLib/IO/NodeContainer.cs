using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.IO
{
    public class NodeContainer
    {
        protected Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        public void AddNode(NodePath path, Node node)
        {
            AddNodeRecursively(path, node, null);
        }

        public void AddNode(string key, Node node)
        {
            AddNodeRecursively(new NodePath(key), node, null);
        }

        protected void AddNodeRecursively(NodePath pathToGoal, Node node, NodeContainer mother)
        {
            mother = (mother != null ? mother : this);
            string key = pathToGoal.Dequeue();
            if (pathToGoal.Path.Count > 0)
            {
                if (!mother.Nodes.ContainsKey(key))
                    mother.Nodes.Add(key, new Node(key));
                Node newNode = mother.Nodes[key];
                AddNodeRecursively(pathToGoal, node, newNode);
                return;
            }
            if (!mother.Nodes.ContainsKey(key))
                mother.Nodes.Add(key, node);
            else
                mother.Nodes[key] = node;
        }

        public bool RemoveNode(NodePath path)
        {
            return RemoveNodeRecursively(path, null);
        }

        protected bool RemoveNodeRecursively(NodePath path, NodeContainer currentNode)
        {
            currentNode = (currentNode != null ? currentNode : this);
            string key = path.Dequeue();
            if (currentNode.Nodes.ContainsKey(key))
            {
                if (path.Path.Count <= 0)
                    return currentNode.Nodes.Remove(key);
                else
                    RemoveNodeRecursively(path, currentNode.Nodes[key]);
            }
            return false;
        }

        public Node GetNode(NodePath path)
        {
            string key = path.Dequeue();
            if (path.Path.Count > 0)
            {
                if (nodes.ContainsKey(key))
                    return nodes[key].GetNode(path);
            }
            else
            {
                return (nodes[key] != null ? nodes[key] : null);
            }
            return null;
        }

        public Node GetNode(string path)
        {
            return GetNode(new NodePath(path));
        }

        public bool HasNodes { get { return nodes.Count > 0; } }

        public Dictionary<string, Node> Nodes { get { return nodes; } }
    }
}
