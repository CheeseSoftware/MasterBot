﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.Room
{
    public class BlockDrawer : IBlockDrawer
    {
        private IBlockDrawerPool blockDrawerPool;
        private IBot bot;
        private byte priority;
        private bool running = false;
        private Queue<BlockWithPos> blocksToDraw = new Queue<BlockWithPos>();
        private Queue<BlockWithPos> blocksToRepair = new Queue<BlockWithPos>();

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
            BlockPos blockPos = new BlockPos(blockWithPos.Block.Layer, blockWithPos.X, blockWithPos.Y);

            if (!blockWithPos.Block.Equals(blockDrawerPool.getWaitingBlock(blockPos)) && bot.Room.BlockMap.getBlock(blockWithPos.Block.Layer, blockPos.X, blockPos.Y).Id != blockWithPos.Block.Id)
            {
                if (bot.Room.BlockMap.isPlaceAble(blockWithPos))
                {
                    blockDrawerPool.AddWaitingBlock(blockWithPos);

                    lock (blocksToDraw)
                    {
                        blocksToDraw.Enqueue(blockWithPos);
                    }
                }
            }
        }

        public void PlaceBlock(int x, int y, IBlock block)
        {
            PlaceBlock(new BlockWithPos(x, y, block));
        }

        public bool DrawBlock()
        {
            if (blocksToDraw.Count <= 0 && blocksToRepair.Count <= 0)
                return false;
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
                if (blockWithPos.Block.Equals(blockDrawerPool.getWaitingBlock(new BlockPos(blockWithPos.Block.Layer, blockWithPos.X, blockWithPos.Y))))// (!bot.Room.getBlock(blockWithPos.Block.Layer, blockWithPos.X, blockWithPos.Y).Equals(blockWithPos.Block))
                {
                    blockWithPos.Block.Send(bot, blockWithPos.X, blockWithPos.Y);

                    lock (blocksToRepair)
                    {
                        if (!blocksToRepair.Contains(blockWithPos))
                            blocksToRepair.Enqueue(blockWithPos);
                    }
                    return true;
                }
                return false;
            }
            else return false;

        }

        public int BlocksToDrawSize
        {
            get { return blocksToDraw.Count; }
        }

        public int BlocksToRepairSize
        {
            get { return blocksToRepair.Count; }
        }

        public bool HasWork
        {
            get { return blocksToRepair.Count > 0 || blocksToDraw.Count > 0; }
        }
    }
}
