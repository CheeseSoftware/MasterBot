using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.Room
{
    public class BlockDrawer
    {
        IBlockDrawerPool blockDrawerPool;
        IRoom room;
        byte priority;
        bool running = false;

        Queue<BlockWithPos> blocksToDraw = new Queue<BlockWithPos>();
        Queue<BlockWithPos> blocksToRepair = new Queue<BlockWithPos>();

        public BlockDrawer(IBlockDrawerPool blockDrawerPool, IRoom room, byte priority = 0)
        {
            this.blockDrawerPool = blockDrawerPool;
            this.room = room;
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

        public void PlaceBlock(BlockWithPos blockWithPos)
        {

        }

        public bool DrawBlock()
        {
            return false;
        }
    }
}
