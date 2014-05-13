using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.SubBot;
using MasterBot.Room.Block;
using MasterBot.Movement;
using System.Timers;
using System.Threading;

namespace MasterBot.Room
{
    public class Room : ASubBot, IRoom
    {
        private BlockMap blockMap;
        private SafeDictionary<int, Player> players = new SafeDictionary<int, Player>();
        private MicroTimer playerTickTimer = new MicroTimer();
        private Minimap.Minimap minimap = null;
        BlockDrawerPool blockDrawerPool;
        BlockDrawer blockDrawer;
        public BlockDrawer BlockDrawer { get { return blockDrawer; } }//TODO: fix temporary public >.<
        //private Thread playerTickThread;

        #region EE_Variables
        private string owner = "";
        private string title = "";
        private int plays = 0;
        private int woots = 0;
        private int totalWoots = 0;
        private string worldKey = "";
        private bool isOwner = false;
        private int width = 0;
        private int height = 0;
        private bool isTutorialRoom = false;
        private float gravity = 0.0F;
        private bool potionsAllowed = false;
        private bool hasAccess = false;
        private bool hideRed = false;
        private bool hideGreen = false;
        private bool hideBlue = false;
        private bool hideTimeDoor = false;
        #endregion

        private List<BlockWithPos> blocksSent = new List<BlockWithPos>();
        private Queue<BlockWithPos> blocksToSend = new Queue<BlockWithPos>();

        private Thread blockRepairThread;
        private Thread checkSentBlocksThread;

        public Room(IBot bot)
            : base(bot)
        {
            this.bot = bot;
            this.blockMap = new BlockMap(bot);
            this.blockDrawerPool = new BlockDrawerPool(bot, this);
            this.blockDrawer = blockDrawerPool.CreateBlockDrawer(15);
            this.blockDrawer.Start();
            //playerTickThread = new Thread(UpdatePhysics);
            playerTickTimer.Interval = 1000 * Config.physics_ms_per_tick;
            playerTickTimer.MicroTimerElapsed += UpdatePhysics;
            playerTickTimer.Start();
        }

        private void UpdatePhysics(object sender, EventArgs e)
        {
            //Dictionary<int, Player> tempPlayers = new Dictionary<int, Player>(players);
            foreach (Player player in players.Values)
            {
                if (player.Moved && minimap != null)
                    minimap.DrawPlayer(player);
                player.tick();
            }
        }

        private void HandleBlockPlace(PlayerIOClient.Message m)
        {
            int layer;
            int x;
            int y;
            int blockId;
            int playerId = -1;
            IBlock result = null;

            if (m.Type == "b")
            {
                layer = m.GetInt(0);
                x = m.GetInt(1);
                y = m.GetInt(2);
                blockId = m.GetInt(3);
                if (m.Count >= 5)
                    playerId = m.GetInt(4);
                result = new NormalBlock(blockId, layer);
            }
            else
            {
                layer = 0;
                x = m.GetInt(0);
                y = m.GetInt(1);
                blockId = m.GetInt(2);
                if (m.Type != "pt" && m.Type != "bs" && m.Count >= 5)
                    playerId = m.GetInt(4);
                switch (m.Type)
                {
                    case "bc":
                        {
                            int coins = m.GetInt(3);
                            if (blockId == 43)
                                result = new BlockCoinDoor(coins);
                            else if (blockId == 165)
                                result = new BlockCoinGate(coins);
                            break;
                        }
                    case "bs":
                        {
                            int note = m.GetInt(3);
                            if (blockId == 77)
                                result = new BlockPiano(note);
                            else if (blockId == 83)
                                result = new BlockDrums(note);
                            break;
                        }
                    case "pt":
                        {
                            int rotation = m.GetInt(3);
                            int portalId = m.GetInt(4);
                            int targetId = m.GetInt(5);
                            //playerId = m.GetInt(6);
                            result = new BlockPortal(rotation, portalId, targetId);
                            break;
                        }
                    case "lb":
                        {
                            string text = m.GetString(3);
                            result = new BlockText(text);
                            break;
                        }
                    case "br":
                        {
                            int rotation = m.GetInt(3);
                            result = new BlockSpikes(rotation);
                            break;
                        }
                    case "wp":
                        {
                            string destination = m.GetString(3);
                            result = new BlockWorldPortal(destination);
                            break;
                        }
                    case "ts":
                        {
                            string text = m.GetString(3);
                            result = new BlockSign(text);
                            break;
                        }
                }
            }
            if (result != null && blockMap != null)
            {
                result.Placer = (players.ContainsKey(playerId) ? players[playerId] : null);
                result.OnReceive(bot, x, y);
                IBlock oldBlock = blockMap.getBlock(layer, x, y);
                blockMap.setBlock(x, y, result);
                bot.SubBotHandler.onBlockChange(x, y, result, oldBlock);
                BlockWithPos b = new BlockWithPos(x, y, result);
                lock (blocksSent)
                { }
                while (blocksSent.Contains(b))
                {
                    blocksSent.Remove(b);
                }
            }
        }

        private uint LoadWorld(PlayerIOClient.Message m, uint ws, int width, int height)
        {
            if (minimap != null)
            {
                minimap.Die();
                minimap = new Minimap.Minimap(bot, width, height);
                foreach (Player player in players.Values)
                    minimap.DrawPlayer(player);
            }
            else
                minimap = new Minimap.Minimap(bot, width, height);
            blockMap.setSize(width, height);
            blockMap.Clear();
            //world start at 17 "ws"
            uint i = ws;
            for (; (int)(i - 2) <= ws || !(m[i - 2] is string); i++)
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

                        IBlock result = null;

                        switch (blockId)
                        {
                            case 242: //portal
                            case 381: //portal
                                {
                                    int rotation = m.GetInt(i + 2);
                                    int id = m.GetInt(i + 3);
                                    int destination = m.GetInt(i + 4);
                                    result = new BlockPortal(rotation, id, destination);
                                    toAdd = 3;
                                    break;
                                }
                            case 43: //coin door
                                {
                                    int coins = m.GetInt(i + 2);
                                    result = new BlockCoinDoor(coins);
                                    toAdd = 1;
                                    break;
                                }
                            case 165: //coin gate
                                {
                                    int coins = m.GetInt(i + 2);
                                    result = new BlockCoinGate(coins);
                                    toAdd = 1;
                                    break;
                                }
                            case 361: //spikes
                                {
                                    int rotation = m.GetInt(i + 2);
                                    result = new BlockSpikes(rotation);
                                    toAdd = 1;
                                    break;
                                }
                            case 77: //piano
                                {
                                    int note = m.GetInt(i + 2);
                                    result = new BlockPiano(note);
                                    toAdd = 1;
                                    break;
                                }
                            case 83: //drums
                                {
                                    int note = m.GetInt(i + 2);
                                    result = new BlockDrums(note);
                                    toAdd = 1;
                                    break;
                                }
                            case 1000: //text
                                {
                                    string text = m.GetString(i + 2);
                                    result = new BlockText(text);
                                    toAdd = 1;
                                    break;
                                }
                            case 385: //sign
                                {
                                    string text = m.GetString(i + 2);
                                    result = new BlockSign(text);
                                    toAdd = 1;
                                    break;
                                }
                            case 374: //world portal
                                {
                                    string destination = m.GetString(i + 2);
                                    result = new BlockWorldPortal(destination);
                                    toAdd = 1;
                                    break;
                                }
                            default:
                                {
                                    result = new NormalBlock(blockId, layer);
                                    break;
                                }
                        }
                        result.OnReceive(bot, xIndex, yIndex);
                        blockMap.setBlock(xIndex, yIndex, result);
                        bot.SubBotHandler.onBlockChange(xIndex, yIndex, blockMap.getBlock(layer, xIndex, yIndex), blockMap.getOldBlocks(layer, xIndex, yIndex).Count >= 2 ? blockMap.getOldBlocks(layer, xIndex, yIndex).ElementAt(1) : new NormalBlock(0, layer));
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
            worldKey = rot13(m.GetString(5));

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

        private void BlockRepairLoop()
        {
            while (true)
            {
                while (blocksToSend.Count > 0)
                {
                    lock (blocksToSend)
                    {
                        BlockWithPos block = blocksToSend.Dequeue();
                        if (blockMap.getBlock(block.X, block.Y) != block.Block)
                        {
                            block.Block.Send(bot, block.X, block.Y);
                            lock (blocksSent)
                            {
                                if (block.Block.TimesSent < 10)
                                {
                                    if (!blocksSent.Contains(block))
                                        blocksSent.Add(block);
                                }
                                else
                                {
                                    while (blocksSent.Contains(block))
                                        blocksSent.Remove(block);
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(11);
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        private void CheckSentBlocks()
        {
            List<BlockWithPos> tempBlocksSent;
            while (true)
            {
                lock (blocksSent)
                    tempBlocksSent = new List<BlockWithPos>(blocksSent);
                foreach (BlockWithPos block in tempBlocksSent.ToList())
                {
                    if (block != null && block.Block != null && !block.Block.Placed && !blocksToSend.Contains(block))
                    {
                        lock (blocksToSend)
                        {
                            if (block.Block.TimeSinceSent > 500)
                                blocksToSend.Enqueue(block);
                        }
                    }
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        private string rot13(string arg1)
        {
            int num = 0;
            string str = "";
            for (int i = 0; i < arg1.Length; i++)
            {
                num = arg1[i];
                if ((num >= 0x61) && (num <= 0x7a))
                {
                    if (num > 0x6d)
                    {
                        num -= 13;
                    }
                    else
                    {
                        num += 13;
                    }
                }
                else if ((num >= 0x41) && (num <= 90))
                {
                    if (num > 0x4d)
                    {
                        num -= 13;
                    }
                    else
                    {
                        num += 13;
                    }
                }
                str = str + ((char)num);
            }
            return str;
        }

        public Stack<IBlock> getOldBlocks(int layer, int x, int y)
        {
            return blockMap.getOldBlocks(layer, x, y);
        }

        public IBlock getBlock(int layer, int x, int y)
        {
            return blockMap.getBlock(layer, x, y);
        }

        public void setBlock(int x, int y, IBlock block)
        {
            if (block != null && block.Id != getBlock(block.Layer, x, y).Id && blockMap.isWithinMap(x, y))
            {
                blockDrawer.PlaceBlock(new BlockWithPos(x, y, block));//blocksToSend.Enqueue(new BlockWithPos(x, y, block));
            }
        }

        public override void onEnable()
        {
        }

        public override void onDisable()
        {
        }

        public override void onConnect()
        {
            blockDrawerPool.Start();
            playerTickTimer.Start();
            blockRepairThread = new Thread(BlockRepairLoop);
            checkSentBlocksThread = new Thread(CheckSentBlocks);
            blockRepairThread.Start();
            checkSentBlocksThread.Start();
        }

        public override void onDisconnect(string reason)
        {
            blockDrawerPool.Stop();
            playerTickTimer.Stop();
            if (blockRepairThread != null)
                blockRepairThread.Abort();
            if (checkSentBlocksThread != null)
                checkSentBlocksThread.Abort();
            if (minimap != null)
                minimap.onDisconnect(reason);
            if (blockMap != null)
                blockMap.Clear();
            if (players != null)
                players.Clear();
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    {
                        DeserializeInit(m);
                        bot.Connection.Send("init2");
                        break;
                    }
                case "reset":
                    {
                        lock (blocksToSend)
                            blocksToSend.Clear();
                        lock (blocksSent)
                            blocksSent.Clear();
                        LoadWorld(m, 0, width, height);
                        break;
                    }
                case "add":
                    {
                        int id = m.GetInt(0);
                        if (!players.ContainsKey(id))
                        {
                            Player player = new Player(this, id, m.GetString(1), m.GetInt(2), m.GetDouble(3), m.GetDouble(4), m.GetBoolean(5), m.GetBoolean(6), m.GetBoolean(7), m.GetInt(8), m.GetBoolean(10), m.GetBoolean(9), m.GetInt(11));
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
                    {
                        string type = m.GetString(0);
                        switch (type)
                        {
                            case "red":
                                this.hideRed = false;
                                break;
                            case "green":
                                this.hideGreen = false;
                                break;
                            case "blue":
                                this.hideBlue = false;
                                break;
                            case "timedoor":
                                this.hideTimeDoor = false;
                                break;
                        }
                    }
                    break;
                case "hide":
                    {
                        string type = m.GetString(0);
                        switch (type)
                        {
                            case "red":
                                this.hideRed = true;
                                break;
                            case "green":
                                this.hideGreen = true;
                                break;
                            case "blue":
                                this.hideBlue = true;
                                break;
                            case "timedoor":
                                this.hideTimeDoor = true;
                                break;
                        }
                    }
                    break;
                case "allowpotions":
                    break;
                case "wu":
                    break;
                case "w":
                    break;
                case "levelup":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].level = m.GetInt(1);
                        }
                    }
                    break;
                case "say":
                    {
                        int id = m.GetInt(0);
                        string text = m.GetString(1);
                        if (players.ContainsKey(id))
                        {
                            Player player = players[id];
                            if (text.Length > 0 && text[0].Equals('!'))
                            {
                                string textCommandCharRemoved = text.Remove(0, 1);
                                string[] textSplit = textCommandCharRemoved.Split(' ');
                                string cmd = textSplit[0];
                                string[] args = new string[textSplit.Length - 1];
                                if (textSplit.Length > 0)
                                    Array.Copy(textSplit, 1, args, 0, textSplit.Length - 1);
                                bot.SubBotHandler.onCommand(cmd, args, player);
                            }
                        }
                    }
                    break;
                case "say_old":
                    break;
                case "updatemeta":
                    {
                        owner = m.GetString(0);
                        title = m.GetString(1);
                        plays = m.GetInt(2);
                        woots = m.GetInt(3);
                        totalWoots = m.GetInt(4);
                    }
                    break;
                case "autotext":
                    break;
                case "clear":
                    {
                        blockMap.Clear();
                        if (minimap != null)
                        {
                            minimap.Die();
                            minimap = new Minimap.Minimap(bot, width, height);
                            foreach (Player player in players.Values)
                                minimap.DrawPlayer(player);
                        }
                        else
                            minimap = new Minimap.Minimap(bot, width, height);
                        DrawBorder();
                        blocksToSend.Clear();
                        blocksSent.Clear();
                    }
                    break;
                case "tele":
                    {
                        bool usedReset = m.GetBoolean(0);
                        for (int i = 1; i <= m.Count - 1; i += 3)
                        {
                            int playerId = m.GetInt(1);
                            int x = m.GetInt(2);
                            int y = m.GetInt(3);
                            if (players.ContainsKey(playerId))
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

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {

        }

        public override void onTick()
        {

        }

        public override void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock)
        {
            if (minimap != null)
                minimap.onBlockChange(x, y, newBlock, oldBlock);
        }

        public void DrawBorder()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == width - 1)
                    {
                        blockMap.setBlock(x, y, new NormalBlock(9, 0));
                        bot.SubBotHandler.onBlockChange(x, y, new NormalBlock(9, 0), new NormalBlock(0, 0));
                    }
                }
            }
        }

        public string Owner
        {
            get { return owner; }
        }

        public string Title
        {
            get { return title; }
        }

        public int Plays
        {
            get { return plays; }
        }

        public int Woots
        {
            get { return woots; }
        }

        public int TotalWoots
        {
            get { return totalWoots; }
        }

        public string WorldKey
        {
            get { return worldKey; }
        }

        public bool IsOwner
        {
            get { return isOwner; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public bool TutorialRoom
        {
            get { return TutorialRoom; }
        }

        public float Gravity
        {
            get { return gravity; }
        }

        public bool PotionsAllowed
        {
            get { return potionsAllowed; }
        }

        public bool HasCode
        {
            get { return isOwner || hasAccess; }
        }

        public bool HideRed
        {
            get { return hideRed; }
        }

        public bool HideGreen
        {
            get { return hideGreen; }
        }

        public bool HideBlue
        {
            get { return hideBlue; }
        }

        public bool HideTimeDoor
        {
            get { return hideTimeDoor; }
        }

        public SafeDictionary<int, Player> Players
        {
            get { return players; }
        }

        public BlockMap BlockMap
        {
            get { return blockMap; }
        }


        public int BlocksToSendSize
        {
            get { return blocksToSend.Count; }
        }

        public int BlocksSentSize
        {
            get { return blocksSent.Count; }
        }

        public override bool HasTab
        {
            get { return true; }
        }

        public override string Name
        {
            get { return "Room"; }
        }

        public BlockDrawerPool BlockDrawerPool
        {
            get { return this.blockDrawerPool; }
        }

    }
}
