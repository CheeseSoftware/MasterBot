﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public interface IBlock
    {
        int Id { get; }
        int Layer { get; }
        bool Background { get; }
        DateTime DatePlaced { get; }
        double TimeSincePlaced { get; }

        void Send(IBot bot, int x, int y);
    }
}
