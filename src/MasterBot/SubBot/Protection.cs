using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.SubBot
{
    public class Protection : ASubBot
    {
        HashSet<IPlayer> protectedPlayers = new HashSet<IPlayer>();
        HashSet<IPlayer> disabledPlayers = new HashSet<IPlayer>();

        private void Rollback(IBot bot, Func<IBlock, IBlock, bool> lambda)
        {
            for (int l = 0; l < 2; l++)
            {
                for (int x = 0; x < bot.Room.Width; x++)
                {
                    for (int y = 0; y < bot.Room.Width; y++)
                    {
                        Stack<IBlock> blocks = new Stack<IBlock>(new Stack<IBlock>(bot.Room.BlockMap.getBlockHistory(l, x, y)));

                        while (blocks.Count > 0)
                        {
                            IBlock block = blocks.Pop();
                            IBlock oldBlock = blocks.Count > 0 ? blocks.Peek() : null;
                            //if its the block that we hate, send the old one
                            if (lambda(block, oldBlock))
                            {
                                bot.Room.setBlock(x, y, oldBlock != null ? oldBlock : new NormalBlock(0, l));
                            }
                        }
                    }
                }
            }
        }

        #region properties
        public override string SubBotName
        {
            get { return "Protection"; }
        }

        public override bool HasTab
        {
            get { return false; }
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
            if (cmdSource is IPlayer)
            {
                IPlayer player = (IPlayer)cmdSource;
                if (!player.IsOp)
                    return;
            }
            IPlayer arg = null;
            if (args.Length >= 1 && bot.Room.getPlayer(args[0].Trim().ToLower()) != null)
            {
                arg = bot.Room.getPlayer(args[0].Trim().ToLower());
            }
            if (arg != null)
            {
                switch (cmd)
                {
                    case "protect":
                        if (args.Length >= 1)
                        {
                            lock (protectedPlayers)
                            {
                                if (!protectedPlayers.Contains(arg))
                                    protectedPlayers.Add(arg);
                            }
                        }
                        break;
                    case "unprotect":
                        if (args.Length >= 1)
                        {
                            lock (protectedPlayers)
                            {
                                if (protectedPlayers.Contains(arg))
                                    protectedPlayers.Remove(arg);
                            }
                        }
                        break;
                    case "disableedit":
                        if (args.Length >= 1)
                        {
                            lock (disabledPlayers)
                            {
                                if (!disabledPlayers.Contains(arg))
                                    disabledPlayers.Add(arg);
                            }
                        }
                        break;
                    case "enableedit":
                        if (args.Length >= 1)
                        {
                            lock (disabledPlayers)
                            {
                                if (disabledPlayers.Contains(arg))
                                    disabledPlayers.Remove(arg);
                            }
                        }
                        break;
                    case "disabletroll":
                        if (args.Length >= 1)
                        {
                            lock (disabledPlayers)
                            {
                                if (!disabledPlayers.Contains(arg))
                                    disabledPlayers.Add(arg);
                            }
                        }
                        goto case "rollbackpl";
                    case "rollbackplayer":
                    case "rollbackpl":
                        Rollback(bot, (IBlock block, IBlock oldBlock) =>
                        {
                            if (block.Placer != null)
                            {
                                if (block.Placer.Name == arg.Name)
                                {
                                    if (oldBlock == null || oldBlock.Placer == null || oldBlock.Placer.Name != arg.Name)
                                        return true;
                                }
                            }
                            return false;
                        });
                        break;
                    case "repair":
                        Rollback(bot, (IBlock block, IBlock oldBlock) =>
                        {
                            if(oldBlock != null && block.Placer != oldBlock.Placer)
                            {
                                if (oldBlock.Placer != null && oldBlock.Placer.Name == arg.Name)
                                    return true;
                            }
                            return false;
                        });
                        break;
                }
            }
            //else
            //  cmdSource.Reply("Could not find player");

            switch (cmd)
            {
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
                    repair = true;
                else if (disabledPlayers.Contains(newBlock.Placer))
                    repair = true;

                if (repair)
                    bot.Room.setBlock(x, y, oldBlock);
            }
        }

        public override void onTick()
        {
        }

    }
}
