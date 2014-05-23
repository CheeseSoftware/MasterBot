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
        bool undone = false;

        public EditChangeList(List<IEditChange> editChanges)
        {
            this.editChanges = editChanges;
        }

        public void Undo(IBlockDrawer drawer)
        {
            foreach (IEditChange change in editChanges)
                change.Undo(drawer);
            undone = true;
        }

        public void Redo(IBlockDrawer drawer)
        {
            foreach (IEditChange change in editChanges)
                change.Redo(drawer);
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
