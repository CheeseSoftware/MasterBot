using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Movement;
using MasterBot.Room;

namespace MasterBot
{
    public class Player : PhysicsPlayer
    {
        public Player(IRoom room, int ID, string name, int smiley, float xPos, float yPos, bool isGod, bool isMod, bool bla, int coins, bool purple, bool isFriend, int level)
            : base(room, ID, name, smiley, xPos, yPos, isGod, isMod, bla, coins, purple, isFriend, level)
        {

        }
    }
}
