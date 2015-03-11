using MasterBot.SubBot.Houses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    class HouseBuilding : ASubBot
    {
        HouseManager houseManager;
        FurnitureManager furnitureManager;

        public HouseBuilding(IBot bot)
            : base(bot)
        {
            this.houseManager = new HouseManager(bot);
            this.furnitureManager = new FurnitureManager(bot, houseManager);
        }


        public override void onEnable()
        {
            return;
        }

        public override void onDisable()
        {
            return;
        }

        public override void onConnect()
        {
            return;
        }

        public override void onDisconnect(string reason)
        {
            return;
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "m":
                    {
                        int userId = m.GetInt(0);
                        float playerPosX = m.GetFloat(1);
                        float playerPosY = m.GetFloat(2);
                        float speedX = m.GetFloat(3);
                        float speedY = m.GetFloat(4);
                        float modifierX = m.GetFloat(5);
                        float modifierY = m.GetFloat(6);
                        float horizontal = m.GetFloat(7);
                        float vertical = m.GetFloat(8);
                        int Coins = m.GetInt(9);

                        int blockX = (int)(playerPosX / 16 + 0.5);
                        int blockY = (int)(playerPosY / 16 + 0.5);

                        IPlayer player = bot.Room.getPlayer(userId);
                        if (player == null || player.IsGod || player.IsMod)
                            return;

                       

                        int blockId = (bot.Room.getBlock(0, blockX + (int)horizontal, blockY + (int)vertical).Id);
                        
                        {
                            int x1 = blockX;
                            int y1 = blockY;
                            int x2 = blockX + (int)horizontal;
                            int y2 = blockY + (int)vertical;

                            if (horizontal + vertical == 1 || horizontal + vertical == -1)
                            {
                                houseManager.OnPlayerMine(player, x1, y1, x2, y2);
                                furnitureManager.OnPlayerPush(player, x1, y1, (int)horizontal, (int)vertical);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            switch (cmd)
            {
                case "build":
                    if (cmdSource is IPlayer && args.Count() >= 1) {
                        int width = 12;
                        int height = 12;
                        IPlayer builder = cmdSource as IPlayer;

                        int x = builder.BlockX-width;
                        int y = builder.BlockY-height;

                        string houseType = args[0];


                        houseManager.BuildHouse(builder, houseType);

                        builder.Reply("say !finishhouse when you're done!");
                    }
                    break;

                case "place":
                    if (cmdSource is IPlayer)
                    {
                        IPlayer builder = cmdSource as IPlayer;

                        furnitureManager.PlaceFurniture(builder, new Door(builder.BlockX, builder.BlockY));
                    }
                    break;

                case "finishouse":
                case "finishhouse":
                    if (cmdSource is IPlayer)
                    {
                        IPlayer builder = cmdSource as IPlayer;

                        houseManager.FinishHouse(builder);
                    }
                    break;

                case "painthouse":
                    if (cmdSource is IPlayer)
                    {
                        IPlayer builder = cmdSource as IPlayer;

                        //houseManager.PaintHouse(builder);
                    }
                    break;

                default:
                    break;
            }
        }

        public override void onBlockChange(int x, int y, Room.Block.IBlock newBlock, Room.Block.IBlock oldBlock)
        {
            return;
        }

        public override void onTick()
        {
            return;
        }

        public override bool HasTab
        {
            get { return false; }
        }


        public override string SubBotName
        {
            get { return "HouseBuilding"; }
        }
    }
}
