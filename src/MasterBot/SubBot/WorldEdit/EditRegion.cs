using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.WorldEdit
{
    class EditRegion : IEnumerable
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

        public int Width { get { return Math.Abs(x2 - x1); } }

        public int Height { get { return Math.Abs(y2 - y1); } }

        public EditRegion()
        {

        }

        public void Reset()
        {
            x1 = -1;
            y1 = -1;
            x2 = -1;
            y2 = -1;
        }

        public IEnumerator GetEnumerator()
        {
            int tempx1 = x1;
            int tempx2 = x2;
            int tempy1 = y1;
            int tempy2 = y2;

            if (tempx1 > tempx2)
            {
                int buffer = tempx1;
                tempx1 = tempx2;
                tempx2 = buffer;
            }
            if (tempy1 > tempy2)
            {
                int buffer = tempy1;
                tempy1 = tempy2;
                tempy2 = buffer;
            }

            for (int x = tempx1; x <= tempx2; x++)
            {
                for (int y = tempy1; y <= tempy2; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }
    }
}
