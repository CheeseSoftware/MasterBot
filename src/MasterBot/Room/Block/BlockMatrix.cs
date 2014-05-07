using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockMatrix
    {
        private IBlock[,] backgroundMap = null;
        private IBlock[,] foregroundMap = null;
        private int width;
        private int height;

        public BlockMatrix(int width, int height)
        {
            backgroundMap = new IBlock[width + 1, height + 1];
            foregroundMap = new IBlock[width + 1, height + 1];
            this.width = width;
            this.height = height;
        }

        private bool isWithinMatrix(int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= width && y <= height)
                return true;
            return false;
        }

        public void setBlock(int layer, int x, int y, IBlock block)
        {
            if(isWithinMatrix(x, y))
            {
                if (layer == 0)
                    foregroundMap[x, y] = block;
                else if (layer == 1)
                    backgroundMap[x, y] = block;
            }
        }

        public IBlock getBlock(int layer, int x, int y)
        {
            if (layer == 0)
                return getBlock(x, y);
            else if (layer == 1)
                return getBackgroundBlock(x, y);
            return null;
        }

        public IBlock getBlock(int x, int y)
        {
            if (isWithinMatrix(x, y))
                return foregroundMap[x, y];
            return null;
        }

        public IBlock getBackgroundBlock(int x, int y)
        {
            if (isWithinMatrix(x, y))
                return backgroundMap[x, y];
            return null;
        }
    }
}
