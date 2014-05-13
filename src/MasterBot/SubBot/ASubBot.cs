using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterBot.SubBot
{
    public abstract class ASubBot : TabPage, ISubBot
    {
        public abstract void onConnect(IBot bot);
        public abstract void onDisconnect(IBot bot, string reason);
        public abstract void onMessage(IBot bot, PlayerIOClient.Message m);
        public abstract void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource);
        public abstract void onBlockChange(IBot bot, int x, int y, IBlock newBlock, IBlock oldBlock);
        public abstract void Update(IBot bot);

        public abstract bool HasTab { get; }

    }
}
