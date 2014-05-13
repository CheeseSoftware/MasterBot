using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    class BoxCodes : ISubBot
    {
        public void onConnect(IBot bot)
        {
            throw new NotImplementedException();
        }

        public void onDisconnect(IBot bot, string reason)
        {
            throw new NotImplementedException();
        }

        public void onMessage(IBot bot, PlayerIOClient.Message m)
        {
            throw new NotImplementedException();
        }

        public void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource)
        {
            throw new NotImplementedException();
        }

        public void onBlockChange(IBot bot, int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            throw new NotImplementedException();
        }

        public void Update(IBot bot)
        {
            throw new NotImplementedException();
        }
    }
}
