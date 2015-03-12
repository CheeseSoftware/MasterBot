using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class BlockSwitch : NormalBlock
    {
        public int switchId;

        public BlockSwitch(int switchId)
            : base(113, 0)
        {
            this.switchId = switchId;
        }

        public override void Send(IBot bot, int x, int y)
        {
            bot.Connection.Send(bot.Room.WorldKey, Layer, x, y, Id, switchId);
            OnSend(bot, x, y);
        }
    }
}
