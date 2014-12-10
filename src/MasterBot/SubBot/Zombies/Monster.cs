using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    public class Monster
    {
        private int xPos_ = 0;
        private int yPos_ = 0;
        private int xOldPos_ = 0;
        private int yOldPos_ = 0;
        public Monster(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        public int xPos { get { return xPos_; } set { xOldPos_ = xPos; xPos_ = value; } }
        public int yPos { get { return yPos_; } set { yOldPos_ = yPos; yPos_ = value; } }
        public int xBlock { get { return xPos_ / 16; } set { xOldPos_ = xPos; xPos_ = value * 16; } }
        public int yBlock { get { return yPos_ / 16; } set { yOldPos_ = yPos; yPos_ = value * 16; } }
        public int xOldBlock { get { return xOldPos_ / 16; } }
        public int yOldBlock { get { return yOldPos_ / 16; } }
        public int xOldPos { get { return xOldPos_; } }
        public int yOldPos { get { return yOldPos_; } }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }
    }

}
