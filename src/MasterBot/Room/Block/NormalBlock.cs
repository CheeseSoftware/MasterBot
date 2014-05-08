﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class NormalBlock : IBlock
    {
        protected int id;
        protected int layer;
        protected DateTime datePlaced = DateTime.MinValue;

        public NormalBlock(int id, int layer)
        {
            this.id = id;
            this.layer = layer;
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
        }

        public double TimeSincePlaced
        {
            get { return datePlaced == null ? Double.MaxValue : (DateTime.Now - datePlaced).TotalMilliseconds; }
        }

        public virtual void Send(IBot bot, int x, int y)
        {
            bot.Connection.Send("b", bot.Room.WorldKey, Layer, x, y, Id);
            datePlaced = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if(obj != null && obj is IBlock)
            {
                IBlock other = (IBlock)obj;
                if (other.Id == id && other.Layer == layer)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 5;
            hash = (hash * 3) + id.GetHashCode();
            hash = (hash * 3) + layer.GetHashCode();
            return hash;
        }
    }
}
