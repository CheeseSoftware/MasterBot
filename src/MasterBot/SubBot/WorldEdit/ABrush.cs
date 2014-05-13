using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit
{
    public abstract class ABrush : IBrush
    {
        private int size;
        private IBlock block;

        public abstract void onUse(IBot bot, int x, int y);


        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public IBlock Block
        {
            get
            {
                return block;
            }
            set
            {
                block = value;
            }
        }
    }
}
