using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class BlockPos
    {
        private int x;
        private int y;

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public BlockPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is BlockPos)
            {
                if ((obj as BlockPos).x == this.x && (obj as BlockPos).y == this.y)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (x << 8) | y;
        }
    }
}
