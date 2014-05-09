using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Movement;
using MasterBot.Room;

namespace MasterBot
{
    public class Player : PhysicsPlayer, ICmdSource
    {
        private int id;
        public int Id { get { return id; } }

        public Player(IRoom room, int id, string name, int smiley, double xPos, double yPos, bool isGod, bool isMod, bool hasChat, int coins, bool purple, bool isFriend, int level)
            : base(room, id, name, smiley, xPos, yPos, isGod, isMod, hasChat, coins, purple, isFriend, level)
        {
            this.id = id;
        }
    }
}
