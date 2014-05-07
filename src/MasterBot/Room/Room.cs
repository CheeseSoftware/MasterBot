using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.SubBot;
using MasterBot.Room.Block;
using MasterBot.Movement;
using System.Windows.Forms;

namespace MasterBot.Room
{
    public class Room : IRoom, ISubBot
    {
        private BlockMatrix blockMap = null;
        private Dictionary<int, PhysicsPlayer> players = new Dictionary<int, PhysicsPlayer>();

        public string owner = "";
        public string title = "";
        public int plays = 0;
        public int woots = 0;
        public int totalWoots = 0;
        public string worldKey = "";
        public bool isOwner = false;
        public int width = 0;
        public int height = 0;
        public bool isTutorialRoom = false;
        public float gravity = 0.0F;
        public bool potionsAllowed = false;

        private void DeserializeInit(PlayerIOClient.Message m)
        {
            owner = m.GetString(0);
            title = m.GetString(1);
            plays = m.GetInt(2);
            woots = m.GetInt(3);
            totalWoots = m.GetInt(4);
            worldKey = m.GetString(5);

            //irrelevant information
            /*int myId = m.GetInt(6);
            int myX = m.GetInt(7);
            int myY = m.GetInt(8);
            string myName = m.GetString(9);
            bool IHaveCode = m.GetBoolean(10);*/

            isOwner = m.GetBoolean(11);
            width = m.GetInt(12);
            height = m.GetInt(13);
            isTutorialRoom = m.GetBoolean(14);
            gravity = m.GetFloat(15);
            potionsAllowed = m.GetBoolean(16);

            blockMap = new BlockMatrix(width, height);
            //world start at 17 "ws"
            for (uint i = 18; !(m[i] is string); i++)
            {
                if (m[i] is byte[])
                {
                    int blockId = m.GetInt(i - 2);
                    int layer = m.GetInt(i - 1);
                    byte[] xArray = m.GetByteArray(i);
                    byte[] yArray = m.GetByteArray(i + 1);

                    for (int x = 0; x < xArray.Length; x += 2)
                    {
                        int xIndex = xArray[x] * 256 + xArray[x + 1];
                        int yIndex = yArray[x] * 256 + yArray[x + 1];

                        switch (blockId)
                        {
                            case 242: //portal
                            case 381: //portal
                                {
                                    int rotation = m.GetInt(i + 2);
                                    int id = m.GetInt(i + 3);
                                    int destination = m.GetInt(i + 4);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockPortal(rotation, id, destination));
                                    i += 3;
                                    break;
                                }
                            case 43: //coin door
                                {
                                    int coins = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockCoinDoor(coins));
                                    i++;
                                    break;
                                }
                            case 165: //coin gate
                                {
                                    int coins = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockCoinGate(coins));
                                    i++;
                                    break;
                                }
                            case 361: //spikes
                                {
                                    int rotation = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockSpikes(rotation));
                                    i++;
                                    break;
                                }
                            case 77: //piano
                                {
                                    int note = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockPiano(note));
                                    i++;
                                    break;
                                }
                            case 83: //drums
                                {
                                    int note = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockDrums(note));
                                    i++;
                                    break;
                                }
                            case 1000: //text
                                {
                                    string text = m.GetString(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockText(text));
                                    i++;
                                    break;
                                }
                            case 385: //sign
                                {
                                    string text = m.GetString(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockSign(text));
                                    i++;
                                    break;
                                }
                            case 374: //world portal
                                {
                                    string destination = m.GetString(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockWorldPortal(destination));
                                    i++;
                                    break;
                                }
                            default:
                                {
                                    blockMap.setBlock(layer, xIndex, yIndex, new NormalBlock(blockId, layer));
                                    break;
                                }
                        }
                    }

                    i += 3;
                }
            }
            //world end "we"

            //potions start "ps"
            //not implemented
            //potions end "pe"
        }

        public IBlock getBlock(int layer, int x, int y)
        {
            return blockMap.getBlock(layer, x, y);
        }

        public void onConnect(MasterBot masterBot)
        {

        }

        public void onDisconnect(MasterBot masterBot, string reason)
        {

        }

        public void onMessage(MasterBot masterBot, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    {
                        DeserializeInit(m);
                        break;
                    }
                case "add":
                    {
                        int id = m.GetInt(0);
                        if (!players.ContainsKey(id))
                        {
                            PhysicsPlayer player = new PhysicsPlayer(this, id, m.GetString(1), m.GetInt(2), m.GetFloat(3), m.GetFloat(4), m.GetBoolean(5), m.GetBoolean(6), m.GetBoolean(7), m.GetInt(8), false, false, 0);
                            players.Add(id, player);
                        }
                        break;
                    }
                case "left":
                    {
                        int id = m.GetInt(0);
                        if (!players.ContainsKey(id))
                        {
                            players.Remove(id);
                        }
                        break;
                    }
            }
        }

        public void onCommand(MasterBot masterBot, string cmd, string[] args, ICmdSource cmdSource)
        {

        }

        public void Update(MasterBot masterBot)
        {

        }

    }
}
