using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockWorldPortal : NormalBlock
    {
        public string destination;

        public BlockWorldPortal(string destination)
            : base(374, 0)
        {
            this.destination = destination;
        }
    }
}
