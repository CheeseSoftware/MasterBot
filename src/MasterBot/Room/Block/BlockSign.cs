using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockSign : NormalBlock
    {
        public string text;

        public BlockSign(string text)
            : base(385, 0)
        {
            this.text = text;
        }
    }
}
