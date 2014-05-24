using MasterBot;
using MasterBot.Room;
using MasterBot.Room.Block;
using MasterBot.SubBot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandCelluralAutomaton
{
    public class SandCelluralAutomaton : ASubBot, IPlugin
    {
        ISet<BlockPos> updateSet = new HashSet<BlockPos>();
        Dictionary<BlockPos, IBlock> blocks = new Dictionary<BlockPos, IBlock>();
        Dictionary<BlockPos, IBlock> coveredBlocks = new Dictionary<BlockPos, IBlock>();
        IBlockDrawer blockDrawer;

        #region properties
        public override bool HasTab
        {
            get { return true; }
        }

        public override string SubBotName
        {
            get { return "SandCelluralAutomaton"; }
        }
        #endregion

        #region public
        public SandCelluralAutomaton()
            : base(null)
        {
        }

        public void PerformAction(IBot bot)
        {
            this.bot = bot;

            // This is enables tick with a interval of 500ms.
            this.EnableTick(500);

            // A BlockDrawer allows parallel drawing of blocks. This one has 31(+1) as priority, the default one has 15(+1).
            this.blockDrawer = bot.Room.BlockDrawerPool.CreateBlockDrawer(31);

            // Important line!
            this.blockDrawer.Start();

            // This initializes GUI.
            this.InitializeComponent();

            // Adds the subbot to the subbot handler and the bot.
            bot.SubBotHandler.AddSubBot(this, false);
        }

        public override void onBlockChange(int x, int y, MasterBot.Room.Block.IBlock newBlock, MasterBot.Room.Block.IBlock oldBlock)
        {
            if (!newBlock.Placer.IsBot)
            {
                lock (blocks)
                {
                    BlockPos blockPos = new BlockPos(0, x, y);
                    if (blocks.ContainsKey(blockPos))
                        blocks[blockPos] = newBlock;
                    else
                        blocks.Add(blockPos, newBlock);
                }
                //if (!newBlock.Placer.IsBot)
                NotifyNeighbors(x, y);
            }
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
        }

        public override void onConnect()
        {
        }

        public override void onDisable()
        {
        }

        public override void onDisconnect(string reason)
        {
        }

        public override void onEnable()
        {
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
        }

        public override void onTick()
        {
            Random random = new Random();
            //List<BlockPos> blocksToUpdate = new List<BlockPos>(updateSet);
            IOrderedEnumerable<BlockPos> blocksToUpdate;
            
            lock (updateSet)
            {
                blocksToUpdate = updateSet.OrderBy(item => random.Next());
                updateSet = new HashSet<BlockPos>();
            }

            // We should randomize the the list to avoid order and uggly patterns:
            
            

            foreach (BlockPos blockPos in blocksToUpdate)
            {
                UpdateBlock(blockPos.X, blockPos.Y);
            }
        }

        #endregion
        #region private
        void UpdateBlock(int x, int y)
        {
            // Local block is required. Room.getBlock returns what the bot got from the block messages.
            // getLocalBlock does not wait on any messages and is therefore required in cellural automaton and maze generators.
            IBlock block = getBlock2(0, x, y);
            IBlock coveredBlock;
            BlockPos blockPos = new BlockPos(0, x, y);

            // We need to know what block there was behind.
            if (coveredBlocks.ContainsKey(blockPos))
                coveredBlock = coveredBlocks[blockPos];
            else
                coveredBlock = new NormalBlock(0);

            switch (block.Id)
            {
                case 59:
                    {
                        int x2 = x;
                        int y2 = y;

                        switch (coveredBlock.Id)
                        {
                            default:
                            case 0:
                                y2++;
                                break;
                            case 1:
                                x--;
                                break;
                            case 2:
                                y--;
                                break;
                            case 3:
                                x++;
                                break;
                            case 4:
                                break;
                        }

                        // Do physics!
                        if (!IsSolid(getBlock2(0, x2, y2)))
                            MoveBlock(x, y, x2, y2);

                    }
                    break;
            }
        }

        void NotifyNeighbors(int x, int y)
        {
            NotifyBlock(x, y);
            NotifyBlock(x-1, y);
            NotifyBlock(x+1, y);
            NotifyBlock(x, y-1);
            NotifyBlock(x, y+1);
        }

        void NotifyBlock(int x, int y)
        {
            if (x > 1 && x < bot.Room.Width - 1 && y > 1 && y < bot.Room.Height - 1)
            {
                BlockPos blockPos = new BlockPos(0, x, y);

                lock (updateSet)
                {
                    if (!updateSet.Contains(blockPos))
                        updateSet.Add(blockPos);
                }
            }
        }

        /// <summary>
        /// Move the block from x1, y1 to x2, y2.
        /// </summary>
        /// <param name="x1">moving block x</param>
        /// <param name="y1">moving block y</param>
        /// <param name="x2">destination x</param>
        /// <param name="y2">destination y</param>
        void MoveBlock(int x1, int y1, int x2, int y2)
        {
            IBlock block = getBlock2(0, x1, y1);
            IBlock destinationBlock = getBlock2(0, x2, y2);
            IBlock coveredBlock = null;
            BlockPos pos1 = new BlockPos(0, x1, y1);
            BlockPos pos2 = new BlockPos(0, x2, y2);

            // Extracts the block behind the moving block(x1, y1)
            if (coveredBlocks.ContainsKey(pos1))
            {
                coveredBlock = coveredBlocks[pos1];
                coveredBlocks.Remove(pos1);
            }



            // If it is a solid block the blocks should just swap.
            if (IsSolid(destinationBlock))
            {
                // Swap!
                setBlock2(x1, y1, destinationBlock);
            }
            else
            {
                // Otherwise the covered block should get visible again.

                // Place the covered block. If it is null/doesn't exist it is 0/air.
                if (coveredBlock != null)
                    setBlock2(x1, y1, coveredBlock);
                else
                    setBlock2(x1, y1, new NormalBlock(0));

                // If there is a block in the destionation we must save so we can put it back later.
                if (destinationBlock != null)
                {
                    // 0/air is the standard block, we don't need to add it in the dictionary.
                    if (destinationBlock.Id != 0)
                    {
                        if (coveredBlocks.ContainsKey(pos2))
                            coveredBlocks[pos2] = destinationBlock;
                        else
                            coveredBlocks.Add(pos2, destinationBlock);
                    }
                }
            }

            // Moves the block to the destionation.
            setBlock2(x2, y2, block);

            NotifyNeighbors(x1, y1);
            NotifyNeighbors(x2, y2);
        }

        bool IsSolid(IBlock block)
        {
            switch (block.Id)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    return false;
                default:
                    return true;
            }
        }

        void setBlock2(int x, int y, IBlock block)
        {
            BlockPos blockPos = new BlockPos(0, x, y);

            lock (blocks)
            {
                if (block.Id == 59)
                {
                    if (blocks.ContainsKey(blockPos))
                        blocks[blockPos] = block;
                    else
                        blocks.Add(blockPos, block);
                }
                else
                {
                    if (blocks.ContainsKey(blockPos))
                        blocks.Remove(blockPos);
                }
            }

            blockDrawer.PlaceBlock(new BlockWithPos(x, y, block));
        }

        IBlock getBlock2(int layer, int x, int y)
        {
            BlockPos blockPos = new BlockPos(0, x, y);

            lock (blocks)
            {
                if (blocks.ContainsKey(blockPos))
                {
                    return blocks[blockPos];
                }
                else
                {
                    IBlock block = bot.Room.getLocalBlock(0, x, y);
                    if (block.Id != 59)
                        return block;

                    Stack<IBlock> stack = bot.Room.getBlockHistory(0, x, y);

                    while (stack.Count > 0)
                    {
                        block = stack.Pop();

                        if (block.Id != 59)
                            return block;
                    }

                    return new NormalBlock(0);
                }
            }
        }

        #endregion
    }
}
