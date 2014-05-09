using MasterBot.SubBot;
using System;
using System.Collections.Generic;
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
        private Timer updateMinimapTimer;

        public Minimap.Minimap minimap;

        public BlockMap(IBot bot, int width, int height)
        {
            backgroundMap = new Stack<IBlock>[width + 1, height + 1];
            foregroundMap = new Stack<IBlock>[width + 1, height + 1];
            this.width = width;
            this.height = height;
            minimap = new Minimap.Minimap(bot, width, height, bot.Room.Players);
            updateMinimapTimer = new Timer();
            updateMinimapTimer.Interval = 20;
            updateMinimapTimer.AutoReset = true;
            updateMinimapTimer.Elapsed += UpdateMinimap;
            updateMinimapTimer.Start();
            Reset();
        }

        private void UpdateMinimap(object sender, EventArgs e)
        {
            minimap.Update(this);
        }

        private bool isWithinMatrix(int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= width && y <= height)
                return true;
            return false;
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

        public void DrawBorder()
        {
            for (int x = 0; x < width + 1; x++)
            {
                for (int y = 0; y < height + 1; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == width - 1)
                    {
                        backgroundMap[x, y].Push(new NormalBlock(9, 0));
                        foregroundMap[x, y].Push(new NormalBlock(9, 0));
                    }
                }
            }
        }

        public void setSize(int width, int height)
        {
            backgroundMap = new Stack<IBlock>[width + 1, height + 1];
            foregroundMap = new Stack<IBlock>[width + 1, height + 1];
            this.width = width;
            this.height = height;
        }

        public void Die()
        {
            updateMinimapTimer.Stop();
        }

        public void setBlock(int x, int y, IBlock block)
        {
            if (isWithinMatrix(x, y))
            {
                if (block.Layer == 0)
                    foregroundMap[x, y].Push(block);
                else if (block.Layer == 1)
                    backgroundMap[x, y].Push(block);
            }
        }

        public Stack<IBlock> getOldBlocks(int layer, int x, int y)
        {
            if (isWithinMatrix(x, y))
            {
                if (layer == 0)
                    return foregroundMap[x, y];
                else if (layer == 1)
                    return backgroundMap[x, y];
            }
            return new Stack<IBlock>(new[] { new NormalBlock(0, 0) });
        }

        public IBlock getBlock(int layer, int x, int y)
        {
            if (layer == 0)
                return getBlock(x, y);
            else if (layer == 1)
                return getBackgroundBlock(x, y);
            return new NormalBlock(0, 0);
        }

        public IBlock getBlock(int x, int y)
        {
            if (isWithinMatrix(x, y) && foregroundMap[x, y].Count > 0)
                return foregroundMap[x, y].Peek();
            return new NormalBlock(0, 0); ;
        }

        public IBlock getBackgroundBlock(int x, int y)
        {
            if (isWithinMatrix(x, y) && backgroundMap[x, y].Count > 0)
                return backgroundMap[x, y].Peek();
            return new NormalBlock(0, 1); ;
        }

        public void Clear()
        {
            Reset();
        }
    }
}
