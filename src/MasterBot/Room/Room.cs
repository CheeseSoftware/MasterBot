using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterBot.SubBot;
using MasterBot.Room.Block;
using MasterBot.Movement;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace MasterBot.Room
{
    public class Room : ASubBot, IRoom
    {
        BlockMap blockMap;
        SafeDictionary<int, IPlayer> players = new SafeDictionary<int, IPlayer>();
        SafeDictionary<string, List<IPlayer>> namePlayers = new SafeDictionary<string, List<IPlayer>>();
        SafeDictionary<string, List<IPlayer>> disconnectedPlayers = new SafeDictionary<string, List<IPlayer>>();
        Stopwatch playerTickStopwatch = new Stopwatch();
        SafeThread playerTickThread;
        //System.Timers.Timer playerTickTimer = new System.Timers.Timer();
        Minimap.Minimap minimap = null;
        IBlockDrawerPool blockDrawerPool;
        IBlockDrawer blockDrawer;

        IPlayer botPlayer = null;

        #region EE_Variables
        string owner = "";
        string title = "";
        int plays = 0;
        int woots = 0;
        int totalWoots = 0;
        string worldKey = "";
        int botId;

        bool isOwner = false;
        int width = 0;
        int height = 0;
        bool isTutorialRoom = false;
        float gravity = 0.0F;
        bool potionsAllowed = false;
        bool hasAccess = false;
        bool hideRed = false;
        bool hideGreen = false;
        bool hideBlue = false;
        System.Windows.Forms.Label labelBlocksToRepair;
        System.Windows.Forms.NumericUpDown numericUpDownBlocksToRepair;
        System.Windows.Forms.Label labelBlocksToPlace;
        System.Windows.Forms.NumericUpDown numericUpDownBlocksToPlace;
        System.Windows.Forms.Label labelWaitingBlocks;
        System.Windows.Forms.NumericUpDown numericUpDownWaitingBlocks;
        bool hideTimeDoor = false;
        #endregion

        public Room(IBot bot)
            : base(bot)
        {
            this.bot = bot;
            this.blockMap = new BlockMap(bot);
            this.blockDrawerPool = new BlockDrawerPool(bot, this);
            this.blockDrawer = blockDrawerPool.CreateBlockDrawer(15);
            this.blockDrawer.Start();

            playerTickThread = new SafeThread(UpdatePhysics);

            EnableTick(50);
        }

        #region properties
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

        public int BotId
        {
            get { return botId; }
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

        public ICollection<IPlayer> Players
        {
            get { return players.Values; }
        }

        public BlockMap BlockMap
        {
            get { return blockMap; }
        }

        public override bool HasTab
        {
            get { return true; }
        }

        public override string SubBotName
        {
            get { return "Room"; }
        }

        public IBlockDrawerPool BlockDrawerPool
        {
            get { return blockDrawerPool; }
        }

        public IBlockDrawer BlockDrawer
        {
            get { return blockDrawer; }
        }

        public IPlayer getPlayer(string name)
        {
            if (namePlayers.ContainsKey(name))
            {
                List<IPlayer> players = namePlayers[name];
                if (players != null && players.Count > 0)
                    return players.First();
            }
            else if (disconnectedPlayers.ContainsKey(name))
            {
                List<IPlayer> players = disconnectedPlayers[name];
                if (players != null && players.Count > 0)
                    return players.First();
            }
            return null;
        }
        #endregion

        private void UpdatePhysics()
        {
            long elapsed = playerTickStopwatch.ElapsedMilliseconds;
            if (elapsed >= Config.physics_ms_per_tick)
            {
                playerTickStopwatch.Restart();
                foreach (Player player in players.Values)
                {
                    if (player.Moved && minimap != null)
                        minimap.DrawPlayer(player);
                    player.tick();
                }
                Thread.Sleep(9);
            }
            else if (elapsed > 2)
                Thread.Sleep((int)(Config.physics_ms_per_tick - elapsed) - 1);
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
                    case "m": break;

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

                blockDrawerPool.OnBlockPlace(b);
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
                        if (result != null)
                        {
                            result.OnReceive(bot, xIndex, yIndex);
                            blockMap.setBlock(xIndex, yIndex, result);
                            bot.SubBotHandler.onBlockChange(xIndex, yIndex, blockMap.getBlock(layer, xIndex, yIndex), blockMap.getBlockHistory(layer, xIndex, yIndex).Count >= 2 ? blockMap.getBlockHistory(layer, xIndex, yIndex).ElementAt(1) : new NormalBlock(0, layer));
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
            worldKey = rot13(m.GetString(5));
            botId = m.GetInt(6);

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
            int unknownValue = m.GetInt(17);
            bool unknownBoolean = m.GetBoolean(18);

            uint we = LoadWorld(m, 19, width, height);

            botPlayer = new Player(bot, botId, "[bot]", 0, 0, 0, false, false, true, 0, false, true, 0);
            //potions start "ps"
            //not implemented
            //potions end "pe"
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

        public Stack<IBlock> getBlockHistory(int layer, int x, int y)
        {
            return blockMap.getBlockHistory(layer, x, y);
        }

        public IBlock getBlock(int layer, int x, int y)
        {
            return blockDrawerPool.getWaitingBlock(new BlockPos(layer, x, y)) != null ? blockDrawerPool.getWaitingBlock(new BlockPos(layer, x, y)) : blockMap.getBlock(layer, x, y);
        }

        public IBlock getLocalBlock(int layer, int x, int y)
        {
            IBlock block = this.blockDrawerPool.getWaitingBlock(new BlockPos(layer, x, y));

            if (block != null)
                return block;
            else return getBlock(layer, x, y);
        }

        public void setBlock(int x, int y, IBlock block)
        {
            if (block.Id != getBlock(block.Layer, x, y).Id && blockMap.isPlaceAble(new BlockWithPos(x, y, block)))
                blockDrawer.PlaceBlock(x, y, block);
        }

        public override void onEnable()
        {
        }

        public override void onDisable()
        {
        }

        public override void onConnect()
        {
            blockDrawer.Start();
            blockDrawerPool.Start();

            playerTickThread.Start();

            playerTickStopwatch.Start();
        }

        public override void onDisconnect(string reason)
        {
            blockDrawer.Stop();
            blockDrawerPool.Stop();
            playerTickThread.Stop();
            if (minimap != null)
                minimap.onDisconnect(reason);
            if (blockMap != null)
                blockMap.Clear();
            if (players != null)
                players.Clear();
            if (namePlayers != null)
                namePlayers.Clear();
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
                        LoadWorld(m, 0, width, height);
                        break;
                    }
                case "add":
                    {
                        int id = m.GetInt(0);
                        if (!players.ContainsKey(id))
                        {
                            Player player = new Player(bot, id, m.GetString(1), m.GetInt(2), m.GetDouble(3), m.GetDouble(4), m.GetBoolean(5), m.GetBoolean(6), m.GetBoolean(7), m.GetInt(8), m.GetBoolean(10), m.GetBoolean(11), m.GetInt(9));
                            //player.IsClubMember = m.GetBoolean(14);
                            players.Add(id, player);
                            if (!namePlayers.ContainsKey(player.Name))
                                namePlayers.Add(new KeyValuePair<string, List<IPlayer>>(player.Name, new List<IPlayer>()));
                            namePlayers[player.Name].Add(player);
                            if (disconnectedPlayers.ContainsKey(player.Name))
                                disconnectedPlayers.Remove(player.Name);
                        }
                        break;
                    }
                case "left":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            IPlayer left = players[id];
                            if (left != null)
                            {
                                if (!disconnectedPlayers.ContainsKey(left.Name))
                                    disconnectedPlayers.Add(new KeyValuePair<string, List<IPlayer>>(left.Name, new List<IPlayer>()));
                                disconnectedPlayers[left.Name].Add(left);
                            }
                            namePlayers.Remove(players[id].Name);
                            players.Remove(id);
                        }
                        break;
                    }
                case "m":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].X = m.GetDouble(1);
                            players[id].Y = m.GetDouble(2);
                            players[id].SpeedX = m.GetDouble(3);
                            players[id].SpeedY = m.GetDouble(4);
                            players[id].ModifierX = m.GetInt(5);
                            players[id].ModifierY = m.GetInt(6);
                            players[id].Horizontal = m.GetInt(7);
                            players[id].Vertical = m.GetInt(8);
                            players[id].Coins = m.GetInt(9);
                            players[id].Purple = m.GetBoolean(10);
                            //players[id].Levitation = m.GetBoolean(11);
                        }
                        break;
                    }
                case "c":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].Coins = m.GetInt(1);
                        }
                        break;
                    }
                case "k":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].HasCrown = true;
                        }
                        break;
                    }
                case "ks":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].HasCrownSilver = true;
                        }
                        break;
                    }
                case "face":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].Smiley = m.GetInt(1);
                        }
                        break;
                    }
                case "god":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                        {
                            players[id].Respawn();
                            players[id].IsGod = m.GetBoolean(1);
                        }
                        break;
                    }
                case "mod":
                    {
                        int id = m.GetInt(0);
                        if (players.ContainsKey(id))
                            players[id].IsMod = m.GetBoolean(1);

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
                            players[id].Level = m.GetInt(1);
                        }
                    }
                    break;
                case "say":
                    {
                        int id = m.GetInt(0);
                        string text = m.GetString(1);
                        if (players.ContainsKey(id))
                        {
                            IPlayer player = players[id];
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
                                players[playerId].X = x;
                                players[playerId].Y = y;
                            }
                        }
                        break;
                    }
                case "saved":
                    bot.Say("World saved.");
                    break;


                default:
                    Console.WriteLine(m.Type);
                    break;
            }


        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
            if (cmdSource is Player && ((Player)cmdSource).IsOp)
            {
                Player player = (Player)cmdSource;
                switch (cmd)
                {
                    case "reset":
                        bot.Connection.Send("say", "/reset");
                        break;
                    case "clear":
                        bot.Connection.Send("clear");
                        break;
                    case "load":
                        bot.Connection.Send("say", "/loadlevel");
                        break;
                    case "setcode":
                        {
                            if (args.Length > 0)
                            {
                                bot.Connection.Send("key", args[0]);
                            }
                            else
                                player.Reply("Usage: !setcode <code>");
                        }
                        break;
                    case "title":
                        {
                            if (args.Length > 0)
                            {
                                string title = "";
                                foreach (string s in args)
                                    title += s + " ";
                                bot.Connection.Send("name", title);
                            }
                            else
                                player.Reply("Usage: !title <title>");
                        }
                        break;
                    case "visible":
                        {
                            if (args.Length > 0)
                            {
                                bot.Connection.Send("say", "/visible " + args[0]);
                            }
                            else
                                player.Reply("Usage: !visible <true/false>");
                        }
                        break;
                    case "kick":
                        {
                            if (args.Length > 0)
                            {
                                bot.Connection.Send("say", "/kick " + args[0]);
                            }
                            else
                                player.Reply("Usage: !kick <player>");
                        }
                        break;
                }
            }
        }

        public override void onTick()
        {
            SafeInvoke.Invoke(this, new Action(() =>
            {
                if (blockDrawer != null)
                {
                    numericUpDownBlocksToPlace.Value = blockDrawerPool.TotalBlocksToDrawSize;
                    numericUpDownBlocksToRepair.Value = blockDrawerPool.TotalBlocksToRepairSize;
                    numericUpDownWaitingBlocks.Value = blockDrawerPool.WaitingBlocks.Count;
                }
            }));
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
                    if (blockMap.isOnBorder(x, y))
                    {
                        blockMap.setBlock(x, y, new NormalBlock(9, 0));
                        bot.SubBotHandler.onBlockChange(x, y, new NormalBlock(9, 0), new NormalBlock(0, 0));
                    }
                }
            }
        }



        public List<IPlayer> getPlayers(string name)
        {
            if (namePlayers.ContainsKey(name))
                return namePlayers[name];
            else if (disconnectedPlayers.ContainsKey(name))
                return disconnectedPlayers[name];
            return null;
        }

        public IPlayer getPlayer(int id)
        {
            if (id == botId)
                return botPlayer;
            else if (players.ContainsKey(id))
                return players[id];
            return null;
        }

        #region gui
        protected override void InitializeComponent()
        {
            this.labelBlocksToRepair = new System.Windows.Forms.Label();
            this.numericUpDownBlocksToRepair = new System.Windows.Forms.NumericUpDown();
            this.labelBlocksToPlace = new System.Windows.Forms.Label();
            this.numericUpDownBlocksToPlace = new System.Windows.Forms.NumericUpDown();
            this.labelWaitingBlocks = new System.Windows.Forms.Label();
            this.numericUpDownWaitingBlocks = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToRepair)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToPlace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitingBlocks)).BeginInit();
            this.SuspendLayout();
            // 
            // labelBlocksToRepair
            // 
            this.labelBlocksToRepair.AutoSize = true;
            this.labelBlocksToRepair.Location = new System.Drawing.Point(5, 31);
            this.labelBlocksToRepair.Name = "labelBlocksToRepair";
            this.labelBlocksToRepair.Size = new System.Drawing.Size(109, 13);
            this.labelBlocksToRepair.TabIndex = 11;
            this.labelBlocksToRepair.Text = "Total blocks to repair:";
            // 
            // numericUpDownBlocksToRepair
            // 
            this.numericUpDownBlocksToRepair.Location = new System.Drawing.Point(120, 29);
            this.numericUpDownBlocksToRepair.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownBlocksToRepair.Name = "numericUpDownBlocksToRepair";
            this.numericUpDownBlocksToRepair.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownBlocksToRepair.TabIndex = 10;
            // 
            // labelBlocksToPlace
            // 
            this.labelBlocksToPlace.AutoSize = true;
            this.labelBlocksToPlace.Location = new System.Drawing.Point(5, 5);
            this.labelBlocksToPlace.Name = "labelBlocksToPlace";
            this.labelBlocksToPlace.Size = new System.Drawing.Size(109, 13);
            this.labelBlocksToPlace.TabIndex = 9;
            this.labelBlocksToPlace.Text = "Total blocks to place:";
            // 
            // numericUpDownBlocksToPlace
            // 
            this.numericUpDownBlocksToPlace.Location = new System.Drawing.Point(120, 3);
            this.numericUpDownBlocksToPlace.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownBlocksToPlace.Name = "numericUpDownBlocksToPlace";
            this.numericUpDownBlocksToPlace.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownBlocksToPlace.TabIndex = 8;
            // 
            // labelWaitingBlocks
            // 
            this.labelWaitingBlocks.AutoSize = true;
            this.labelWaitingBlocks.Location = new System.Drawing.Point(34, 57);
            this.labelWaitingBlocks.Name = "labelWaitingBlocks";
            this.labelWaitingBlocks.Size = new System.Drawing.Size(80, 13);
            this.labelWaitingBlocks.TabIndex = 13;
            this.labelWaitingBlocks.Text = "Waiting blocks:";
            // 
            // numericUpDownWaitingBlocks
            // 
            this.numericUpDownWaitingBlocks.Location = new System.Drawing.Point(120, 55);
            this.numericUpDownWaitingBlocks.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownWaitingBlocks.Name = "numericUpDownWaitingBlocks";
            this.numericUpDownWaitingBlocks.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWaitingBlocks.TabIndex = 12;
            // 
            // Room
            // 
            this.Controls.Add(this.labelWaitingBlocks);
            this.Controls.Add(this.numericUpDownWaitingBlocks);
            this.Controls.Add(this.labelBlocksToRepair);
            this.Controls.Add(this.numericUpDownBlocksToRepair);
            this.Controls.Add(this.labelBlocksToPlace);
            this.Controls.Add(this.numericUpDownBlocksToPlace);
            this.Name = "Room";
            this.Size = new System.Drawing.Size(332, 275);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToRepair)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlocksToPlace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitingBlocks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
