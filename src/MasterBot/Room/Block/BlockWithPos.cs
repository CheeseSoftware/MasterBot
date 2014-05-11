using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockWithPos
    {
        private int x;
        private int y;
        private IBlock block;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public IBlock Block { get { return block; } }

        public BlockWithPos(int x, int y, IBlock block)
        {
            this.x = x;
            this.y = y;
            this.block = block;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is BlockWithPos)
            {
                BlockWithPos other = (BlockWithPos)obj;
                if (other.block.Equals(block) && other.X == x && other.Y == y)
                    return true;
            }
            return false;
        }
    }
}
