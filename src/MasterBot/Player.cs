using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Movement;
using MasterBot.Room;

namespace MasterBot
{
    public class Player : IPlayer
    {
        IBot bot;
        PhysicsPlayer physicsPlayer;
        private int id;
        private Dictionary<string, object> metadata = new Dictionary<string, object>();
        public bool hascrownsilver;

        public int Id { get { return id; } }

        public Player(IBot bot, int id, string name, int smiley, double xPos, double yPos, bool isGod, bool isMod, bool hasChat, int coins, bool purple, bool isFriend, int level)
            //: base(bot, id, name, smiley, xPos, yPos, isGod, isMod, hasChat, coins, purple, isFriend, level)
        {
            this.id = id;
            this.bot = bot;

            physicsPlayer = new PhysicsPlayer(bot, id, name, smiley, xPos, yPos, isGod, isMod, hasChat, coins, purple, isFriend, level);
        }

        public void Reply(string message)
        {
            Send(message);
        }

        public void Send(string message)
        {
            bot.Say(Name + ": " + message);
        }

        public void SetMetadata(string key, object value)
        {
            if (metadata.ContainsKey(key))
                metadata.Remove(key);
            metadata.Add(key, value);
        }

        public bool HasMetadata(string key)
        {
            return metadata.ContainsKey(key);
        }

        public void RemoveMetadata(string key)
        {
            if (metadata.ContainsKey(key))
                metadata.Remove(key);
        }

        public object GetMetadata(string key)
        {
            if (metadata.ContainsKey(key))
                return metadata[key];
            return null;
        }

        public bool IsBot
        {
            get { return this.id == bot.Room.BotId; }
        }

        public bool IsOp
        {
            get { return Name == "ostkaka" || Name == "gustav9797" || Name == "gbot" || Name == "botost" || bot.Room.Owner == Name; }
        }

#region PhysicsPlayer
        public int BlockX { get { return physicsPlayer.BlockX; } set { physicsPlayer.BlockX = value; } }

        public int BlockY { get { return physicsPlayer.BlockY; } set { physicsPlayer.BlockY = value; } }

        public double X { get { return physicsPlayer.x; } set { physicsPlayer.x = value; } }

        public double Y { get { return physicsPlayer.y; } set { physicsPlayer.y = value; } }

        public double SpeedX { get { return physicsPlayer.speedX; } set { physicsPlayer.speedX = value; } }

        public double SpeedY { get { return physicsPlayer.speedY; } set { physicsPlayer.speedY = value; } }

        public double ModifierX { get { return physicsPlayer.modifierX; } set { physicsPlayer.modifierX = value; } }

        public double ModifierY { get { return physicsPlayer.modifierY; } set { physicsPlayer.modifierY = value; } }

        public int Horizontal { get { return physicsPlayer.horizontal; } set { physicsPlayer.horizontal = value; } }

        public int Vertical { get { return physicsPlayer.vertical; } set { physicsPlayer.vertical = value; } }

        public int Coins { get { return physicsPlayer.coins; } set { physicsPlayer.coins = value; } }

        public bool Purple { get { return physicsPlayer.purple; } set { physicsPlayer.purple = value; } }

        public bool Levitation { get { return physicsPlayer.Levitation; } set { physicsPlayer.Levitation = value; } }

        public bool HasCrown { get { return physicsPlayer.hascrown; } set { physicsPlayer.hascrown = value; } }

        public bool HasCrownSilver { get { return physicsPlayer.hascrownsilver; } set { physicsPlayer.hascrownsilver = value; } }

        public int Smiley { get { return physicsPlayer.smiley; } set { physicsPlayer.smiley = value; } }

        public bool IsGod { get { return physicsPlayer.isgod; } set { physicsPlayer.isgod = value; } }

        public bool IsMod { get { return physicsPlayer.ismod; } set { physicsPlayer.ismod = value; } }

        public bool IsClubMember { get { return physicsPlayer.isclubmember; } set { physicsPlayer.isclubmember = value; } }

        public int Level { get { return physicsPlayer.level; } set { physicsPlayer.level = value; } }

        public bool Moved { get { return physicsPlayer.Moved; } }

        public string Name { get { return physicsPlayer.name; } set { physicsPlayer.name = value; } }

        public int OldBlockX { get { return physicsPlayer.OldBlockX; } }

        public int OldBlockY { get { return physicsPlayer.OldBlockY; } }

        public void tick()
        {
            physicsPlayer.tick();
        }

        public void Respawn()
        {
            physicsPlayer.respawn();
        }
#endregion

    }
}
