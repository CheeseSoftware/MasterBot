using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.IO
{
    public class NodePath
    {
        Queue<string> _path = new Queue<string>();

        public NodePath(string path)
        {
            string[] temp = path.Split('.');
            foreach(string s in temp)
                _path.Enqueue(s);
        }

        public string Dequeue()
        {
            return _path.Dequeue();
        }

        public Queue<string> Path { get { return _path; } }
    }
}
