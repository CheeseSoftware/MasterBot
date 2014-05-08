using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockMap
    {
        private Stack<IBlock>[,] backgroundMap = null;
        private Stack<IBlock>[,] foregroundMap = null;
        private int width;
        private int height;

        public BlockMap(int width, int height)
        {
            backgroundMap = new Stack<IBlock>[width + 1, height + 1];
            foregroundMap = new Stack<IBlock>[width + 1, height + 1];
            this.width = width;
            this.height = height;
            Reset();
        }

        private bool isWithinMatrix(int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= width && y <= height)
                return true;
            return false;
        }

        private void Reset()
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    backgroundMap[x, y] = new Stack<IBlock>();
                    foregroundMap[x, y] = new Stack<IBlock>();
                }
            }
        }

        public void setBlock(int x, int y, IBlock block)
        {
            if(isWithinMatrix(x, y))
            {
                if (block.Layer == 0)
                    foregroundMap[x, y].Push(block);
                else if (block.Layer == 1)
                    backgroundMap[x, y].Push(block);
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
                return foregroundMap[x, y].Peek();
            return null;
        }

        public IBlock getBackgroundBlock(int x, int y)
        {
            if (isWithinMatrix(x, y))
                return backgroundMap[x, y].Peek();
            return null;
        }

        public void Clear()
        {
            Reset();
        }
    }
}
