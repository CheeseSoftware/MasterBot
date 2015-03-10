using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.IO
{
    public class Node : NodeContainer
    {
        private string value;

        public Node(string value = "")
        {
            this.value = value;
        }

        public string Value { get { return value; } }
    }
}
