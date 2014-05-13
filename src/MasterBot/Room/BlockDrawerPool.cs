using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room
{
    public class BlockDrawerPool : IBlockDrawerPool
    {
        SafeList<BlockDrawer> queuedBlockdrawers = new SafeList<BlockDrawer>();

        public BlockDrawer CreateBlockDrawer(byte priority)
        {
            return new BlockDrawer(this, priority);
        }

        public void StartBlockDrawer(BlockDrawer blockDrawer)
        {
            for (int i = 0; i <= blockDrawer.
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
