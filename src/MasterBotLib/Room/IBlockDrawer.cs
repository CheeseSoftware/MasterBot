using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.Room
{
    public interface IBlockDrawer
    {
        byte Priority { get; }
        int BlocksToDrawSize { get; }
        int BlocksToRepairSize { get; }
        bool HasWork { get; }

        void Start();
        void Stop();
        void setPriority(byte priority);
        void PlaceBlock(BlockWithPos blockWithPos);
        void PlaceBlock(int x, int y, IBlock block);
        bool DrawBlock();
    }
}
