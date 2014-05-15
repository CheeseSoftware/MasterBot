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
        private int layer;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Layer { get { return layer; } }

        public BlockPos(int layer, int x, int y)
        {
            this.layer = layer;
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is BlockPos)
            {
                if ((obj as BlockPos).x == this.x && (obj as BlockPos).y == this.y && (obj as BlockPos).layer == this.layer)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (x << 16) | (y << 8) | layer;
        }
    }
}
