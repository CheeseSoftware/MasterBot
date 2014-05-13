using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    class BoxCodes : ASubBot
    {
        public BoxCodes(IBot bot)
            : base(bot)
        {
        }


        public override void onEnable()
        {
            throw new NotImplementedException();
        }

        public override void onDisable()
        {
            throw new NotImplementedException();
        }

        public override void onConnect()
        {
            throw new NotImplementedException();
        }

        public override void onDisconnect(string reason)
        {
            throw new NotImplementedException();
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
            throw new NotImplementedException();
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            throw new NotImplementedException();
        }

        public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            throw new NotImplementedException();
        }

        public override void onTick()
        {
            throw new NotImplementedException();
        }

        public override bool HasTab
        {
            get { throw new NotImplementedException(); }
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }
    }
}
