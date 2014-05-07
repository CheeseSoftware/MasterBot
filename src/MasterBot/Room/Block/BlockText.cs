using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockText : NormalBlock
    {
        public string text;

        public BlockText(string text)
            : base(1000, 0)
        {
            this.text = text;
        }
    }
}
