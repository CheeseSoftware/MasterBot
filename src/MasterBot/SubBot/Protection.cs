using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.SubBot
{
    public class Protection : ISubBot
    {
        ISet<Player> protectedPlayers = new HashSet<Player>();
        ISet<Player> disabledPlayers = new HashSet<Player>();

        private void Rollback(IBot bot, Func<bool, IBlock> lambda)
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
                            
                            /*if (lambda(block))
                            {
                                bot.Room.BlockMap.setBlock(x, y, block);
                                break;
                            }*/
                        }
                    }
                }
            }
        }


        public void onConnect(IBot bot)
        {

        }

        public void onDisconnect(IBot bot, string reason)
        {

        }

        public void onMessage(IBot bot, PlayerIOClient.Message m)
        {

        }

        public void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource)
        {
            /*if (cmdSource is Player)
            {
                Player player = (Player)cmdSource;
                if (player.name != "ostkaka" && player.name != "gustav9797" && player.name != "botost" && player.name != "gbot" && player.name != bot.Room.Owner)
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
            }*/
        }

        public void onBlockChange(IBot bot, int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
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

        public void Update(IBot bot)
        {

        }
    }
}
