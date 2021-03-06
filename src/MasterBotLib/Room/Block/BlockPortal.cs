﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class BlockPortal : NormalBlock
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
            bot.Connection.Send(bot.Room.WorldKey, Layer, x, y, Id, rotation, myId, destinationId);
            OnSend(bot, x, y);
        }
    }
}
