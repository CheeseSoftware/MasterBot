using MasterBot.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit.Change
{
    class EditChangeList : IEditChange
    {
        List<IEditChange> editChanges;

        public EditChangeList(List<IEditChange> editChanges)
        {
            this.editChanges = editChanges;
        }

        public void Undo(IBlockDrawer drawer)
        {
            foreach (IEditChange change in editChanges)
                change.Undo(drawer);
        }

        public void Redo(IBlockDrawer drawer)
        {
            foreach (IEditChange change in editChanges)
                change.Redo(drawer);
        }
    }
}
