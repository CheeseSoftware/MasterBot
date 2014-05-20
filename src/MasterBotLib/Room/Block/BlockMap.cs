using MasterBot.SubBot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MasterBot.Room.Block
{
    public class BlockMap
    {
        private Stack<IBlock>[,] backgroundMap = null;
        private Stack<IBlock>[,] foregroundMap = null;
        private int width;
        private int height;
        private bool reseting = false;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public BlockMap(IBot bot, int width = 0, int height = 0)
        {
            backgroundMap = new Stack<IBlock>[width + 1, height + 1];
            foregroundMap = new Stack<IBlock>[width + 1, height + 1];
            this.width = width;
            this.height = height;
            Reset();
        }

        private void Reset()
        {
            for (int x = 0; x < width + 1; x++)
            {
                for (int y = 0; y < height + 1; y++)
                {
                    backgroundMap[x, y] = new Stack<IBlock>();
                    foregroundMap[x, y] = new Stack<IBlock>();
                }
            }
        }

        public bool isWithinMap(int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= width && y <= height)
                return true;
            return false;
        }

        public bool isOnBorder(int x, int y)
        {
            return x == 0 || y == 0 || x == width || y == height;
        }

        public bool isPlaceAble(BlockWithPos block)
        {
            if (isWithinMap(block.X, block.Y))
            {
                if (isOnBorder(block.X, block.Y))
                {
                    switch (block.Block.Id)
                    {
                        case 44:
                        case 182:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            return true;
                        default:
                            return false;
                    }
                }
                else //TODO: Check if bot has the block pack
                    return true;
            }
            return false;
        }

        public void setSize(int width, int height)
        {
            backgroundMap = new Stack<IBlock>[width + 1, height + 1];
            foregroundMap = new Stack<IBlock>[width + 1, height + 1];
            this.width = width;
            this.height = height;
        }

        public void setBlock(int x, int y, IBlock block)
        {
            if (block == null)
                throw (new Exception("block should not be null!"));
            if (isWithinMap(x, y))
            {
                if (block.Layer == 0)
                    foregroundMap[x, y].Push(block);
                else if (block.Layer == 1)
                    backgroundMap[x, y].Push(block);
            }
        }

        public Stack<IBlock> getBlockHistory(int layer, int x, int y)
        {
            if (isWithinMap(x, y))
            {
                if (layer == 0)
                    return foregroundMap[x, y];
                else if (layer == 1)
                    return backgroundMap[x, y];
            }
            return new Stack<IBlock>(new[] { new NormalBlock(0, layer) });
        }

        public IBlock getBlock(int layer, int x, int y)
        {
            if (!reseting)
            {
                if (layer == 0)
                    return getForegroundBlock(x, y);
                else if (layer == 1)
                    return getBackgroundBlock(x, y);
            }
            return new NormalBlock(0, layer);
        }

        public IBlock getForegroundBlock(int x, int y)
        {
            if (!reseting)
            {
                if (isWithinMap(x, y) && foregroundMap[x, y].Count > 0)
                    return foregroundMap[x, y].Peek();
            }
            return new NormalBlock(0, 0); ;
        }

        public IBlock getBackgroundBlock(int x, int y)
        {
            if (!reseting)
            {
                if (isWithinMap(x, y) && backgroundMap[x, y].Count > 0)
                    return backgroundMap[x, y].Peek();
            }
            return new NormalBlock(0, 1); ;
        }

        public void Clear()
        {
            reseting = true;
            Reset();
            reseting = false;
        }

        public Color getColor(int x, int y)
        {
            if (getForegroundBlock(x, y).Id != 0 && getForegroundBlock(x, y).Color != Color.Transparent)
                return getForegroundBlock(x, y).Color;
            else if (getBackgroundBlock(x, y).Color != Color.Transparent)
                return getBackgroundBlock(x, y).Color;
            else
                return Color.Black;

        }
    }
}
