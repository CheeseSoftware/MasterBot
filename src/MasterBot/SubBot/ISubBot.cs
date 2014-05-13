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
        void onEnable();
        void onDisable();
        void onConnect();
        void onDisconnect(string reason);
        void onMessage(PlayerIOClient.Message m);
        void onCommand(string cmd, string[] args, ICmdSource cmdSource);
        void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock);
        void Update();
        
        bool HasTab { get; }
        bool Enabled { get; set; }
    }
}
