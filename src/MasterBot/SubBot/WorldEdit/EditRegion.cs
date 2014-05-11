using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit
{
    class EditRegion
    {
        int x1 = -1;
        int y1 = -1;
        int x2 = -1;
        int y2 = -1;

        public bool Set { get { return x1 != -1 && x2 != -1; } }

        public bool FirstCornerSet { get { return x1 != -1 && y1 != -1; } }

        public bool SecondCornerSet { get { return x2 != -1 && y2 != -1; } }

        public Point FirstCorner { get { return new Point(x1, y1); } set { x1 = value.X; y1 = value.Y; } }

        public Point SecondCorner { get { return new Point(x2, y2); } set { x2 = value.X; y2 = value.Y; } }

        public EditRegion()
        {

        }
    }
}
