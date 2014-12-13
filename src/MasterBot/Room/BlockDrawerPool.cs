using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.Room
{
    public class BlockDrawerPool : IBlockDrawerPool
    {
        IBot bot;
        IRoom room;
        SafeList<IBlockDrawer> queuedBlockdrawers = new SafeList<IBlockDrawer>();
        SafeThread drawerThread;
        int i = 0;

        // Blocks that are not placed yet...
        Dictionary<BlockPos, IBlock> waitingBlocks = new Dictionary<BlockPos, IBlock>();

        public BlockDrawerPool(IBot bot, IRoom room)
        {
            this.bot = bot;
            this.room = room;
            drawerThread = new SafeThread(Draw);
        }

        private void Draw()
        {
            if (bot.Room.HasCode)
            {
                bool hasWork = false;
                foreach (IBlockDrawer blockDrawer in queuedBlockdrawers)
                {
                    if (blockDrawer.HasWork)
                    {
                        hasWork = true;
                        break;
                    }
                }

                if (hasWork)
                {
                    bool success;

                    lock (queuedBlockdrawers)
                    {
                        IBlockDrawer blockDrawer;
                        i = (i + 1) % queuedBlockdrawers.Count;
                        blockDrawer = queuedBlockdrawers[i];

                        success = blockDrawer.DrawBlock();
                    }

                    if (success)
                    {
                        Thread.Sleep(10);
                    }
                }
                else
                    Thread.Sleep(1);
            }
        }
        public IBlockDrawer CreateBlockDrawer(byte priority)
        {
            return new BlockDrawer(this, bot, priority);
        }

        public void StartBlockDrawer(IBlockDrawer blockDrawer)
        {
            for (int i = 0; i <= blockDrawer.Priority; i++)
            {
                int j = queuedBlockdrawers.Count * i / (blockDrawer.Priority + 1);
                queuedBlockdrawers.Insert(j, blockDrawer);
            }
        }

        public void StopBlockDrawer(IBlockDrawer blockDrawer)
        {
            for (int i = queuedBlockdrawers.Count - 1; i >= 0; i--)
            {
                if (queuedBlockdrawers[i] == blockDrawer)
                    queuedBlockdrawers.RemoveAt(i);
            }
        }

        public void OnBlockPlace(BlockWithPos blockWithPos)
        {
            lock (waitingBlocks)
            {
                BlockPos blockPos = new BlockPos(blockWithPos.Block.Layer, blockWithPos.X, blockWithPos.Y);
                if (waitingBlocks.ContainsKey(blockPos))
                {
                    if (blockWithPos.Block.Equals(waitingBlocks[blockPos]))
                        waitingBlocks.Remove(blockPos);
                }
            }
        }

        public void Start()
        {
            drawerThread.Start();
        }

        public void Stop()
        {
            drawerThread.Stop();

            lock (waitingBlocks)
            {
                waitingBlocks.Clear();
            }
        }

        public void AddWaitingBlock(BlockWithPos blockWithPos)
        {
            lock (waitingBlocks)
            {
                BlockPos blockPos = new BlockPos(blockWithPos.Block.Layer, blockWithPos.X, blockWithPos.Y);
                if (waitingBlocks.ContainsKey(blockPos))
                {
                    if (blockWithPos.Block.Equals(waitingBlocks[blockPos]))
                        return;

                    waitingBlocks.Remove(blockPos);
                }
                waitingBlocks.Add(blockPos, blockWithPos.Block);
            }
        }

        public IBlock getWaitingBlock(BlockPos blockPos)
        {
            if (waitingBlocks.ContainsKey(blockPos))
                return waitingBlocks[blockPos];
            else
                return null;
        }

        public Dictionary<BlockPos, IBlock> WaitingBlocks
        {
            get { return waitingBlocks; }
        }

        public int TotalBlocksToDrawSize
        {
            get { int total = 0; foreach (IBlockDrawer drawer in queuedBlockdrawers) total += drawer.BlocksToDrawSize; return total; }
        }

        public int TotalBlocksToRepairSize
        {
            get { int total = 0; foreach (IBlockDrawer drawer in queuedBlockdrawers) total += drawer.BlocksToRepairSize; return total; }
        }
    }
}
