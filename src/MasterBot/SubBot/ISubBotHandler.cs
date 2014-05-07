﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    interface ISubBotHandler
    {
        void AddSubBot(string name, ISubBot subBot);
        void RemoveSubBot(string name);
        ISubBot GetSubBot(string name);
        Dictionary<string, ISubBot> SubBots { get; }
    }
}
