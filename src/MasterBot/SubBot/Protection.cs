using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.SubBot
{
   /*public class Protection : ASubBot
    {
        ISet<Player> protectedPlayers = new HashSet<Player>();
        ISet<Player> disabledPlayers = new HashSet<Player>();

        #region private
        private void Rollback(IBot bot, Func<IBlock, bool> lambda)
        {
            for (int l = 0; l < 2; l++)
            {
                for (int x = 0; x < bot.Room.Width; x++)
                {
                    for (int y = 0; y < bot.Room.Width; y++)
                    {
                        Stack<IBlock> blocks = bot.Room.BlockMap.getOldBlocks(l, x, y);

                        while (blocks.Count > 0)
                        {
                            IBlock block = blocks.Pop();
                            
                            if (lambda(block))
                            {
                                bot.Room.BlockMap.setBlock(x, y, block);
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion // private

#region public
        #region properties
        public override string Name
        {
            get { return "Protection"; }
        }

        public override bool HasTab
        {
            get { throw new NotImplementedException(); }
        }
        #endregion // properties

        public Protection(IBot bot)
            : base(bot)
        {
        }


        public override void onEnable()
        {

        }

        public override void onDisable()
        {

        }

        public override void onConnect()
        {

        }

        public override void onDisconnect(string reason)
        {

        }

        public override void onMessage(PlayerIOClient.Message m)
        {

        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmdSource is Player)
            {
                Player player = (Player)cmdSource;
                if (player.Name != "ostkaka" && player.Name != "gustav9797" && player.Name != "botost" && player.Name != "gbot" && player.Name != bot.Room.Owner)
                    return;
            }
            switch (cmd)
            {
                case "protect":
                    if (args.Length >= 1)
                    {
                        //bot.Room.pla

                        lock (protectedPlayers)
                        {
                            if (!protectedPlayers.Contains(args[0]))
                                protectedPlayers.Add(args[0]);
                        }
                    }
                    break;
                case "unprotect":
                    if (args.Length >= 1)
                    {
                        lock (protectedPlayers)
                        {
                            if (protectedPlayers.Contains(args[0]))
                                protectedPlayers.Remove(args[0]);
                        }
                    }
                    break;
                case "disableedit":
                    if (args.Length >= 1)
                    {
                        lock (disabledPlayers)
                        {
                            if (!disabledPlayers.Contains(args[0]))
                                disabledPlayers.Add(args[0]);
                        }
                    }
                    break;
                case "enableedit":
                    if (args.Length >= 1)
                    {
                        lock (disabledPlayers)
                        {
                            if (!disabledPlayers.Contains(args[0]))
                                disabledPlayers.Add(args[0]);
                        }
                    }
                    break;
                case "rollbackplayer":
                case "rollbackpl":
                    Rollback(bot, (IBlock block) => { return true; });
                    break;
                case "disabletroll":
                    if (args.Length >= 1)
                    {
                        lock (disabledPlayers)
                        {
                            if (!disabledPlayers.Contains(args[0]))
                                disabledPlayers.Add(args[0]);
                        }
                    }
                    goto case "rollbackpl";
                    break;
                case "repair":
                    break;
                case "repairprotected":
                    break;
                case "cleantroll":
                    break;
            }
        }

        public override void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock)
        {
            if (!protectedPlayers.Contains(newBlock.Placer))
            {
                bool repair = false;
                if (protectedPlayers.Contains(oldBlock.Placer))
                {
                    repair = true;
                } if (repair);
                else if (disabledPlayers.Contains(newBlock.Placer))
                {
                    repair = true;
                }

                if (repair)
                {
                    //bot.Room.BlockMap.setBlock(oldBlock);
                }
            }
        }

        public override void onTick()
        {
        }
#endregion // public

    }*/
}
