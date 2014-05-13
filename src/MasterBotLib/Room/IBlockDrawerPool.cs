using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterBot.Room.Block;

namespace MasterBot.Room
{
    public interface IBlockDrawerPool
    {
        IBlockDrawer CreateBlockDrawer(byte priority = 0);

        void StartBlockDrawer(IBlockDrawer blockDrawer);
        void StopBlockDrawer(IBlockDrawer blockDrawer);
        void OnBlockPlace(BlockWithPos blockWithPos);
        void Start();
        void Stop();

        /// <summary>
        /// Adds a "waiting block". A block that will be placed, but is waiting for the server to be placed should be there.
        /// </summary>
        /// <param name="blockWithPos"></param>
        void AddWaitingBlock(BlockWithPos blockWithPos);
        
        /// <summary>
        /// returns the "waiting block" from position. A block that will be placed, but is waiting for the server to be placed should be there.
        /// Returns null if block wasn't found.
        /// </summary>
        /// <param name="blockWithPos"></param>
        IBlock getWaitingBlock(BlockPos blockPos);
    }
}
