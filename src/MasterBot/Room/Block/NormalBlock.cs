using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    class NormalBlock : IBlock
    {
        private int id;
        private int layer;

        public NormalBlock(int id, int layer)
        {
            this.id = id;
            this.layer = layer;
        }

        public int Id
        {
            get { return id; }
        }

        public int LayerId
        {
            get { return layer; }
        }

        public bool Background
        {
            get { return LayerId >= 500; }
        }
    }
}
