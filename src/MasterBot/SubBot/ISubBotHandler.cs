﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public interface ISubBotHandler
    {
        void AddSubBot(string name, ASubBot subBot);
        void RemoveSubBot(string name);
        ASubBot GetSubBot(string name);
        Dictionary<string, ASubBot> SubBots { get; }
    }
}
