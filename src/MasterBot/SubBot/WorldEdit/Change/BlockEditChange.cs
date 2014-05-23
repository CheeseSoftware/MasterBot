using MasterBot.Room;
using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit.Change
{
    class BlockEditChange : IEditChange
    {
        BlockWithPos newBlock;
        BlockWithPos oldBlock;
        bool undone = false;

        public BlockEditChange(BlockWithPos newBlock, BlockWithPos oldBlock)
        {
            this.newBlock = newBlock;
            this.oldBlock = oldBlock;
        }

        public void Undo(IBlockDrawer drawer)
        {
            drawer.PlaceBlock(oldBlock);
            undone = true;
        }

        public void Redo(IBlockDrawer drawer)
        {
            drawer.PlaceBlock(newBlock);
            undone = false;
        }

        public bool IsUndone
        {
            get { return undone; }
        }

        public bool IsRedone
        {
            get { return !undone; }
        }
    }
}
