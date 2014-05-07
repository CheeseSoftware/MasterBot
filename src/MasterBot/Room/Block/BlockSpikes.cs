using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockSpikes : NormalBlock
    {
        public int rotation;

        public BlockSpikes(int rotation)
            : base(361, 0)
        {
            this.rotation = rotation;
        }
    }
}
