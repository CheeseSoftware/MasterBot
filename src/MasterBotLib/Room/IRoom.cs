using MasterBot.Minimap;
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
        int BotId { get; }
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
        ICollection<IPlayer> Players { get; }
        BlockMap BlockMap { get; }
        IBlockDrawerPool BlockDrawerPool { get; }
        IBlockDrawer BlockDrawer { get; }
        #endregion

        IPlayer getPlayer(string name);
        List<IPlayer> getPlayers(string name);
        IPlayer getPlayer(int id);

        /// <summary>
        /// Get a block. Use getLocalBlock(...) if you want to get blocks that are not sent yet.
        /// </summary>
        /// <param name="layer">0 is foreground, 1 is background.</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        IBlock getBlock(int layer, int x, int y);
        /// <summary>
        /// A lokal block is a block that is going or have been sent by the bot. This changes instantly while nromal getBlock changes after the message is received.
        /// </summary>
        /// <param name="layer">0 is foreground, 1 is background.</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        IBlock getLocalBlock(int layer, int x, int y);
        Stack<IBlock> getBlockHistory(int layer, int x, int y);
        void setBlock(int x, int y, IBlock block);
    }
}
