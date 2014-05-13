using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room
{
    public class BlockDrawer
    {
        IBlockDrawerPool blockDrawerPool;
        byte priority;
        bool running = false;

        public BlockDrawer(IBlockDrawerPool blockDrawerPool, byte priority = 0)
        {
            this.priority = priority;
        }

        public void Start()
        {
            blockDrawerPool.StartBlockDrawer(this);
            running = true;
        }

        public void Stop()
        {
            blockDrawerPool.StopBlockDrawer(this);
            running = false;
        }

        public void setPriority(byte priority)
        {
            this.priority = priority;
        }

        public byte Priority
        {
            get { return priority; }
        }
        
    }
}
