using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class BlockCoinDoor : NormalBlock
    {
        public int coins;

        public BlockCoinDoor(int coins)
            : base(43, 0)
        {
            this.coins = coins;
        }

        public override void PlaceNormally(IBot bot, int x, int y)
        {
            bot.Connection.Send("b", Layer, x, y, Id, coins);
            OnSend(bot, x, y);
        }
    }
}
