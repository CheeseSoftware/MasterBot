using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class BlockDrums : NormalBlock
    {
        public int note;

        public BlockDrums(int note)
            : base(83, 0)
        {
            this.note = note;
        }
    }
}
