using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSight
{
    public class PositionList
    {
        public double[] Xd { get; set; }
        public double[] Yd { get; set; }
        public double[] Zd { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public PositionList(int _x, int _y, int _z)
        {
            
            X = _x;
            Y = _y;
            Z = _z;

            Xd = new double[]{ X, X, X, X };

            Yd = new double[] { Y, Y, Y, Y };

            Zd = new double[] { Z, Z, Z, Z };
        }

    }
}
