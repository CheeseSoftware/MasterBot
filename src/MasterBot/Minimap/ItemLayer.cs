using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterBot.Minimap
{
    class ItemLayer
    {
        public const int BACKGROUND = 0;
        public const int FORGROUND = 1;
        public const int DECORATION = 2;
        public const int ABOVE = 3;
    }

    class ItemTab
    {
        public const int BACKGROUND = 0;
        public const int BLOCK = 1;
        public const int DECORATIVE = 2;
        public const int ACTION = 3;
    }

    class ItemId
    {
        public const int SWITCH_PURPLE = 113;
        public const int  DOOR_PURPLE = 184;
        public const int  GATE_PURPLE = 185;
        public const int  SPEED_LEFT = 114;
        public const int  SPEED_RIGHT = 115;
        public const int  SPEED_UP = 116;
        public const int  SPEED_DOWN = 117;
        public const int  CHAIN = 118;
        public const int  WATER = 119;
        public const int  NINJA_LADDER = 120;
        public const int  BRICK_COMPLETE = 121;
        public const int  TIMEDOOR = 156;
        public const int  TIMEGATE = 157;
        public const int  COINDOOR = 43;
        public const int  COINGATE = 165;
        public const int  WINE_V = 98;
        public const int  WINE_H = 99;
        public const int  DIAMOND = 241;
        public const int  WAVE = 300;
        public const int  CAKE = 337;

        public static bool isSolid(int param1)
        {
            return param1 >= 9 && param1 <= 97 || param1 >= 122 && param1 <= 217;
        }// end function

        public static bool isClimbable(int param1)
        {
            switch(param1)
            {
                case NINJA_LADDER:
                case CHAIN:
                case WINE_V:
                case WINE_H:
                {
                    return true;
                }
                default:
                {
                    break;
                }
            }
            return false;
        }// end function
    }
}
