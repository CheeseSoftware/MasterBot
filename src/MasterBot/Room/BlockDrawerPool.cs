using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterBot.Room
{
    public class BlockDrawerPool : IBlockDrawerPool
    {
        SafeList<BlockDrawer> queuedBlockdrawers = new SafeList<BlockDrawer>();
        Thread drawerThread;

        public BlockDrawerPool(IBot bot)
        {
            drawerThread = new Thread(() =>
                {
                    int i = 0;

                    while (bot.Connected)
                    {
                        while (bot.Room.HasCode)
                        {
                            bool success;

                            lock (queuedBlockdrawers)
                            {
                                BlockDrawer blockDrawer;
                                i = (i+1)%queuedBlockdrawers.Count;
                                blockDrawer = queuedBlockdrawers[i];

                                success = blockDrawer.DrawBlock();
                            }

                            if (success)
                            {
                                Thread.Sleep(10);
                            }
                        }
                        Thread.Sleep(100);
                    }
                });

            drawerThread.Start();
        }

        public BlockDrawer CreateBlockDrawer(byte priority)
        {
            return new BlockDrawer(this, priority);
        }

        public void StartBlockDrawer(BlockDrawer blockDrawer)
        {
            for (int i = 0; i <= blockDrawer.Priority; i++)
            {
                int j = queuedBlockdrawers.Count * i / blockDrawer.Priority;
                queuedBlockdrawers.Insert(j, blockDrawer);
            }
        }

        public void StopBlockDrawer(BlockDrawer blockDrawer)
        {
            for (int i = queuedBlockdrawers.Count - 1; i >= 0; i--)
            {
                if (queuedBlockdrawers[i] == blockDrawer)
                    queuedBlockdrawers.RemoveAt(i);
            }
        }

    }
}
