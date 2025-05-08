using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    [Serializable]
    public class XYCoord
    {
        public double X { get; set; }

        public double Y { get; set; }

        public XYCoord()
        {
            X = 0.0;
            Y = 0.0;        
        }

        public XYCoord(double x,double y)
        {
            X = x;
            Y = y;        
        }
    }

    [Serializable]
    public class XYZCoord:XYCoord
    {
        public double Z { get; set; }

        public XYZCoord():base()
        {
            Z = 0.0;        
        }

        public XYZCoord(double x,double y,double z) : base(x,y)
        {
            Z = z;
        }
    }
}
