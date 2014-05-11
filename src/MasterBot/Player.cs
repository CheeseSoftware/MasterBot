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
        private Dictionary<string, object> metadata = new Dictionary<string, object>();

        public int Id { get { return id; } }

        public Player(IRoom room, int id, string name, int smiley, double xPos, double yPos, bool isGod, bool isMod, bool hasChat, int coins, bool purple, bool isFriend, int level)
            : base(room, id, name, smiley, xPos, yPos, isGod, isMod, hasChat, coins, purple, isFriend, level)
        {
            this.id = id;
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
    }
}
