using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockCoinDoor : NormalBlock
    {
        public int coins;

        public BlockCoinDoor(int coins)
            : base(43, 0)
        {
            this.coins = coins;
        }
    }
}
