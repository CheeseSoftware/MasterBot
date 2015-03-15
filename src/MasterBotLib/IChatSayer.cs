using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    public interface IChatSayer
    {
        void Say(string message);
        void Say(IPlayer receiver, string message);
    }
}
