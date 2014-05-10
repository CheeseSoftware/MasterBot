using MasterBot.Room.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot
{
    class BlockPlaceTest : ISubBot
    {
        List<int> blockPlayers = new List<int>();

        public BlockPlaceTest()
        {

        }

        public void onConnect(IBot bot)
        {
        }

        public void onDisconnect(IBot bot, string reason)
        {
        }

        public void onMessage(IBot bot, PlayerIOClient.Message m)
        {
            if (m.Type == "b" && m.Count >= 5)
            {
                int playerId = m.GetInt(4);
                if (blockPlayers.Contains(playerId))
                {
                    int layer = m.GetInt(0);
                    int x = m.GetInt(1);
                    int y = m.GetInt(2);
                    Stack<IBlock> blocks = bot.Room.getOldBlocks(layer, x, y);
                    if(blocks.Count >= 2)
                        bot.Connection.Send("say", "That block is: " + blocks.ElementAt(1).Id);
                    else
                        bot.Connection.Send("say", "No block.");
                    blockPlayers.Remove(playerId);
                }
            }
        }

        public void onCommand(IBot bot, string cmd, string[] args, ICmdSource cmdSource)
        {
            if(cmd == "test")
            {
                if (cmdSource is Player)
                    blockPlayers.Add(((Player)cmdSource).Id);
                else
                    bot.Connection.Send("say", "You are not a player.");
            }
        }

        public void Update(IBot bot)
        {
        }

        public void onBlockChange(IBot bot, int x, int y, IBlock newBlock, IBlock oldBlock)
        {
            
        }
    }
}
