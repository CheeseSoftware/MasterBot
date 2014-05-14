using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class BlockCoinGate : NormalBlock
    {
        public int coins;

        public BlockCoinGate(int coins)
            : base(165, 0)
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
