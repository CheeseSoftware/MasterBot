using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    public interface ISubBot
    {
        void onConnect(IBot bot);
        void onDisconnect(IBot bot, string reason);
        void onMessage(IBot bot, PlayerIOClient.Message m);
        void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource);
        void onBlockChange(IBot bot, int x, int y, IBlock newBlock, IBlock oldBlock);
        void Update(IBot bot);
    }
}
