using MasterBot.Minimap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public class NormalBlock : IBlock
    {
        protected int id;
        protected int layer;
        protected bool placed = false;
        protected bool sent = false;
        protected DateTime datePlaced = DateTime.MinValue;
        protected DateTime dateSent = DateTime.MinValue;
        protected IPlayer placer = null;
        protected int timesSent = 0;

        public NormalBlock(int id, int layer)
        {
            this.id = id;
            this.layer = layer;
        }

        public NormalBlock(int id)
        {
            this.id = id;
            this.layer = id >= 500 ? 1 : 0;
        }

        public Color Color
        {
            get 
            {
                if (id == 0)
                    return Color.Black;
                else if (MinimapColors.ColorCodes.ContainsKey(id))
                    return MinimapColors.ColorCodes[id];
                return Color.Black;
            }
        }

        public int Id
        {
            get { return id; }
        }

        public int Layer
        {
            get { return layer; }
        }

        public bool Background
        {
            get { return layer >= 500; }
        }

        public DateTime DatePlaced
        {
            get { return datePlaced; }
            set { datePlaced = value; }
        }

        public double TimeSincePlaced
        {
            get { return !placed ? 0 : (DateTime.Now - datePlaced).TotalMilliseconds; }
        }

        public IPlayer Placer
        {
            get
            {
                return placer;
            }
            set
            {
                placer = value;
            }
        }

        public virtual void Send(IBot bot, int x, int y)
        {
            dateSent = DateTime.Now;
            bot.Connection.Send(bot.Room.WorldKey, Layer, x, y, Id);
            timesSent++;
        }

        public virtual void OnReceive(IBot bot, int x, int y)
        {
            datePlaced = DateTime.Now;
            placed = true;
            //bot.SubBotHandler.onBlockChange(x, y, this, bot.Room.getOldBlocks(layer, x, y).Count >= 2 ? bot.Room.getOldBlocks(layer, x, y).ElementAt(1) : new NormalBlock(0));
        }

        public bool Placed
        {
            get { return placed; }
        }

        public double TimeSinceSent
        {
            get { return dateSent == DateTime.MinValue ? 0 : (DateTime.Now - dateSent).TotalMilliseconds; }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is IBlock)
            {
                IBlock other = (IBlock)obj;
                if (other.Id == id && other.Layer == layer)
                    return true;
            }
            return false;
        }

        public int TimesSent
        {
            get { return timesSent; }
        }
    }
}
