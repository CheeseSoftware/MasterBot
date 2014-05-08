using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class BlockPortal : NormalBlock
    {
        public int rotation;
        public int myId;
        public int destinationId;

        public BlockPortal(int rotation, int myId, int destinationId)
            : base(242, 0)
        {
            this.rotation = rotation;
            this.myId = myId;
            this.destinationId = destinationId;
        }

        public override void Send(IBot bot, int x, int y)
        {
            bot.Connection.Send("b", bot.Room.WorldKey, Layer, x, y, Id, rotation, myId, destinationId);
            datePlaced = DateTime.Now;
        }
    }
}
