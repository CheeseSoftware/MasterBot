﻿using System;
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
        private MasterBot masterBot;
        private BlockMatrix blockMap = null;
        private Dictionary<int, Player> players = new Dictionary<int, Player>();
        private Timer playerTickTimer = new Timer();

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

        public bool hasAccess = false;
        public bool HasCode { get { return isOwner || hasAccess; } }

        public Room(MasterBot masterBot)
        {
            this.masterBot = masterBot;
            playerTickTimer.Tick += UpdatePhysics;
            playerTickTimer.Interval = 10;
        }

        private void UpdatePhysics(object sender, EventArgs e)
        {
            lock (players)
            {
                foreach (PhysicsPlayer player in players.Values)
                {
                    player.tick();
                }
            }
        }

        private void HandleBlockPlace(PlayerIOClient.Message m)
        {
            int layer;
            int x;
            int y;
            int blockId;
            int playerId;

            if (m.Type == "b")
            {
                layer = m.GetInt(0);
                x = m.GetInt(1);
                y = m.GetInt(2);
                blockId = m.GetInt(3);
                if (m.Count >= 5)
                    playerId = m.GetInt(4);
                blockMap.setBlock(layer, x, y, new NormalBlock(blockId, layer));
            }
            else
            {
                layer = 0;
                x = m.GetInt(0);
                y = m.GetInt(1);
                blockId = m.GetInt(2);
                if (m.Type != "pt" && m.Type != "bs")
                    playerId = m.GetInt(4);
                switch (m.Type)
                {
                    case "bc":
                        {
                            int coins = m.GetInt(3);
                            if (blockId == 43)
                                blockMap.setBlock(layer, x, y, new BlockCoinDoor(coins));
                            else if (blockId == 165)
                                blockMap.setBlock(layer, x, y, new BlockCoinGate(coins));
                            break;
                        }
                    case "bs":
                        {
                            int note = m.GetInt(3);
                            if (blockId == 77)
                                blockMap.setBlock(layer, x, y, new BlockPiano(note));
                            else if (blockId == 83)
                                blockMap.setBlock(layer, x, y, new BlockDrums(note));
                            break;
                        }
                    case "pt":
                        {
                            int rotation = m.GetInt(3);
                            int portalId = m.GetInt(4);
                            int targetId = m.GetInt(5);
                            //playerId = m.GetInt(6);
                            blockMap.setBlock(layer, x, y, new BlockPortal(rotation, portalId, targetId));
                            break;
                        }
                    case "lb":
                        {
                            string text = m.GetString(3);
                            blockMap.setBlock(layer, x, y, new BlockText(text));
                            break;
                        }
                    case "br":
                        {
                            int rotation = m.GetInt(3);
                            blockMap.setBlock(layer, x, y, new BlockSpikes(rotation));
                            break;
                        }
                    case "wp":
                        {
                            string destination = m.GetString(3);
                            blockMap.setBlock(layer, x, y, new BlockWorldPortal(destination));
                            break;
                        }
                    case "ts":
                        {
                            string text = m.GetString(3);
                            blockMap.setBlock(layer, x, y, new BlockSign(text));
                            break;
                        }
                }
            }
        }

        private uint LoadWorld(PlayerIOClient.Message m, uint ws, int width, int height)
        {
            blockMap = new BlockMatrix(width, height);
            //world start at 17 "ws"
            uint i = ws;
            for (; !(m[i + 2] is string); i++)
            {
                if (m[i] is byte[])
                {
                    int blockId = m.GetInt(i - 2);
                    int layer = m.GetInt(i - 1);
                    byte[] xArray = m.GetByteArray(i);
                    byte[] yArray = m.GetByteArray(i + 1);

                    uint toAdd = 0;

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
                                    toAdd = 3;
                                    break;
                                }
                            case 43: //coin door
                                {
                                    int coins = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockCoinDoor(coins));
                                    toAdd = 1;
                                    break;
                                }
                            case 165: //coin gate
                                {
                                    int coins = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockCoinGate(coins));
                                    toAdd = 1;
                                    break;
                                }
                            case 361: //spikes
                                {
                                    int rotation = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockSpikes(rotation));
                                    toAdd = 1;
                                    break;
                                }
                            case 77: //piano
                                {
                                    int note = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockPiano(note));
                                    toAdd = 1;
                                    break;
                                }
                            case 83: //drums
                                {
                                    int note = m.GetInt(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockDrums(note));
                                    toAdd = 1;
                                    break;
                                }
                            case 1000: //text
                                {
                                    string text = m.GetString(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockText(text));
                                    toAdd = 1;
                                    break;
                                }
                            case 385: //sign
                                {
                                    string text = m.GetString(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockSign(text));
                                    toAdd = 1;
                                    break;
                                }
                            case 374: //world portal
                                {
                                    string destination = m.GetString(i + 2);
                                    blockMap.setBlock(layer, xIndex, yIndex, new BlockWorldPortal(destination));
                                    toAdd = 1;
                                    break;
                                }
                            default:
                                {
                                    blockMap.setBlock(layer, xIndex, yIndex, new NormalBlock(blockId, layer));
                                    break;
                                }
                        }
                    }
                    i += toAdd;
                    i += 3;
                }
            }
            return i + 2;
            //world end "we"
        }

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

            uint we = LoadWorld(m, 17, width, height);

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
            playerTickTimer.Start();
        }

        public void onDisconnect(MasterBot masterBot, string reason)
        {
            playerTickTimer.Stop();
        }

        public void onMessage(MasterBot masterBot, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    {
                        DeserializeInit(m);
                        masterBot.connection.Send("init2");
                        break;
                    }
                case "reset":
                    {
                        LoadWorld(m, 0, width, height);
                        break;
                    }
                case "add":
                    {
                        int id = m.GetInt(0);
                        if (!players.ContainsKey(id))
                        {
                            Player player = new Player(this, id, m.GetString(1), m.GetInt(2), m.GetFloat(3), m.GetFloat(4), m.GetBoolean(5), m.GetBoolean(6), m.GetBoolean(7), m.GetInt(8), m.GetBoolean(10), m.GetBoolean(9), m.GetInt(11));
                            player.isclubmember = m.GetBoolean(12);
                            players.Add(id, player);
                        }
                        break;
                    }
                case "left":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players.Remove(id);
                        }
                        break;
                    }
                case "m":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].x = m.GetDouble(1);
                            players[id].y = m.GetDouble(2);
                            players[id].speedX = m.GetDouble(3);
                            players[id].speedY = m.GetDouble(4);
                            players[id].modifierX = m.GetInt(5);
                            players[id].modifierY = m.GetInt(6);
                            players[id].horizontal = m.GetInt(7);
                            players[id].vertical = m.GetInt(8);
                            players[id].coins = m.GetInt(9);
                            players[id].purple = m.GetBoolean(10);
                            players[id].Levitation = m.GetBoolean(11);
                        }
                        break;
                    }
                case "c":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].coins = m.GetInt(1);
                        }
                        break;
                    }
                case "k":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].hascrown = true;
                        }
                        break;
                    }
                case "ks":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].hascrownsilver = true;
                        }
                        break;
                    }
                case "face":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].smiley = m.GetInt(1);
                        }
                        break;
                    }
                case "god":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                            players[id].isgod = m.GetBoolean(1);
                        break;
                    }
                case "mod":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                            players[id].ismod = m.GetBoolean(1);

                        break;
                    }
                case "lostaccess":
                    {
                        hasAccess = false;
                        break;
                    }
                case "access":
                    {
                        hasAccess = true;
                        break;
                    }
                case "info":
                    break;
                case "p":
                    break;
                case "write":
                    break;
                case "upgrade":
                    break;
                case "b":
                case "bc":
                case "bs":
                case "pt":
                case "lb":
                case "br":
                case "wp":
                case "ts":
                    HandleBlockPlace(m);
                    break;
                case "show":
                    break;
                case "hide":
                    break;
                case "allowpotions":
                    break;
                case "wu":
                    break;
                case "w":
                    break;
                case "levelup":
                    break;
                case "say":
                    break;
                case "say_old":
                    break;
                case "updatemeta":
                    break;
                case "autotext":
                    break;
                case "clear":
                    blockMap.Clear();
                    break;
                case "tele":
                    {
                        bool usedReset = m.GetBoolean(0);
                        for (int i = 1; i <= m.Count - 1; i += 3)
                        {
                            int playerId = m.GetInt(1);
                            int x = m.GetInt(2);
                            int y = m.GetInt(3);
                            if(players.ContainsKey(playerId))
                            {
                                players[playerId].x = x;
                                players[playerId].y = y;
                            }
                        }
                        break;
                    }
                case "saved":
                    break;
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
