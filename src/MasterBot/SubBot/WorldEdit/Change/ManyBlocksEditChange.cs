using MasterBot.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit.Change
{
    class ManyBlocksEditChange : IEditChange
    {
        ICollection<BlockEditChange> blocks;

        public ManyBlocksEditChange(ICollection<BlockEditChange> blocks)
        {
            this.blocks = blocks;
        }

        public void Undo(IBlockDrawer drawer)
        {
            foreach (BlockEditChange change in blocks)
                change.Undo(drawer);
        }

        public void Redo(IBlockDrawer drawer)
        {
            foreach (BlockEditChange change in blocks)
                change.Redo(drawer);
        }
    }
}
