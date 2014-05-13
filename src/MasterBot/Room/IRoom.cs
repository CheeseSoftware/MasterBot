using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room
{
    public interface IRoom
    {
        #region properties
        string Owner { get; }
        string Title { get; }
        int Plays { get; }
        int Woots { get; }
        int TotalWoots { get; }
        string WorldKey { get; }
        bool IsOwner { get; }
        int Width { get; }
        int Height { get; }
        bool TutorialRoom { get; }
        float Gravity { get; }
        bool PotionsAllowed { get; }
        bool HasCode { get; }
        bool HideRed { get; }
        bool HideGreen { get; }
        bool HideBlue { get; }
        bool HideTimeDoor { get; }
        SafeDictionary<int, Player> Players { get; }
        BlockMap BlockMap { get; }
        int BlocksToSendSize { get; }
        int BlocksSentSize { get; }
        #endregion

        IBlock getBlock(int layer, int x, int y);
        Stack<IBlock> getOldBlocks(int layer, int x, int y);
        void setBlock(int x, int y, IBlock block);
    }
}
