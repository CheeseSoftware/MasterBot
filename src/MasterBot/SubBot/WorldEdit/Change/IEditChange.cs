using MasterBot.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit.Change
{
    interface IEditChange
    {
        void Undo(IBlockDrawer drawer);
        void Redo(IBlockDrawer drawer);
        bool IsUndone { get; }
        bool IsRedone { get; }
    }
}
