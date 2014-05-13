using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class BlockPiano : NormalBlock
    {
        public int note;

        public BlockPiano(int note)
            : base(77, 0)
        {
            this.note = note;
        }
    }
}
