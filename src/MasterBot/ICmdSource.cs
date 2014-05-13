using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    public interface ICmdSource
    {
        void Reply(string message);
    }
}
