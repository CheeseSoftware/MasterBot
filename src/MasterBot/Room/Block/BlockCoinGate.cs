using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockCoinGate : NormalBlock
    {
        public int coins;

        public BlockCoinGate(int coins)
            : base(165, 0)
        {
            this.coins = coins;
        }
    }
}
