using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterBot.Room
{
    public interface IBlockDrawerPool
    {
        BlockDrawer CreateBlockDrawer(byte priority = 0);

        void StartBlockDrawer(BlockDrawer blockDrawer);
        void StopBlockDrawer(BlockDrawer blockDrawer);
    }
}
