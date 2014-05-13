using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit
{
    interface IBrush
    {
        void onUse(IBot bot, int x, int y);

        int Size { get; set; }
        IBlock Block { get; set; }
    }
}
