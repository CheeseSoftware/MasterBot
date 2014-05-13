using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.Room
{
    public class BlockDrawer : IBlockDrawer
    {
        IBlockDrawerPool blockDrawerPool;
        IBot bot;
        byte priority;
        bool running = false;

        Queue<BlockWithPos> blocksToDraw = new Queue<BlockWithPos>();
        Queue<BlockWithPos> blocksToRepair = new Queue<BlockWithPos>();

        public BlockDrawer(IBlockDrawerPool blockDrawerPool, IBot bot, byte priority = 0)
        {
            this.blockDrawerPool = blockDrawerPool;
            this.bot = bot;
            this.priority = priority;
        }

        public void Start()
        {
            if (!running)
            {
                blockDrawerPool.StartBlockDrawer(this);
                running = true;
            }
        }

        public void Stop()
        {
            if (running)
            {
                blockDrawerPool.StopBlockDrawer(this);
                running = false;
            }
        }

        public void setPriority(byte priority)
        {
            this.priority = priority;
        }

        public byte Priority
        {
            get { return priority; }
        }

        public void PlaceBlock(BlockWithPos blockWithPos)
        {
            //Block a
            //Block b

            BlockPos blockPos = new BlockPos(blockWithPos.X, blockWithPos.Y);

            if (!blockWithPos.Block.Equals(blockDrawerPool.getWaitingBlock(blockPos))/* && !bot.Room.getBlock(blockWithPos.Block.Layer, blockWithPos.X, blockWithPos.Y).Equals(blockWithPos.Block) &&
                !blocksToRepair.Contains(blockWithPos)*/)
            {
                blockDrawerPool.AddWaitingBlock(blockWithPos);

                lock (blocksToDraw)
                {
                    blocksToDraw.Enqueue(blockWithPos); 
                }
            }
        }

        public bool DrawBlock()
        {
            BlockWithPos blockWithPos = null;

            lock (blocksToDraw)
            {
                if (blocksToDraw.Count > 0)
                    blockWithPos = blocksToDraw.Dequeue(); 
            }

            if (blockWithPos == null)
            {
                lock (blocksToRepair)
                {
                    if (blocksToRepair.Count > 0)
                        blockWithPos = blocksToRepair.Dequeue();  
                }
            }

            if (blockWithPos != null)
            {
                if (blockWithPos.Block.Equals(blockDrawerPool.getWaitingBlock(new BlockPos(blockWithPos.X, blockWithPos.Y))))// (!bot.Room.getBlock(blockWithPos.Block.Layer, blockWithPos.X, blockWithPos.Y).Equals(blockWithPos.Block))
                {
                    blockWithPos.Block.Send(bot, blockWithPos.X, blockWithPos.Y);

                    lock (blocksToRepair)
                    {
                        blocksToRepair.Enqueue(blockWithPos); 
                    }
                    return true;
                }
                return false;
            }
            else return false;

        }

    }
}
