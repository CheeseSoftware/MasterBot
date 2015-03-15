using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MasterBot
{
    [Serializable]
    public class Pair<A, B>
    {
        public A first { get; set; }
        public B second { get; set; }
        public Pair(A first, B second)
        {
            this.first = first;
            this.second = second;
        }
    }
}