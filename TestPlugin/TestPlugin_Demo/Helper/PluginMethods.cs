using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class PluginMethods
    {

        public static bool FitCircle_From_ThreePoints(XYCoord point1, XYCoord point2, XYCoord point3, out XYCoord circlePoint, out double radius)
        {
            circlePoint = new XYCoord();
            var x1 = point1.X;
            var y1 = point1.Y;
            var x2 = point2.X;
            var y2 = point2.Y;
            var x3 = point3.X;
            var y3 = point3.Y;

            var result = x1 * y2 + x2 * y3 + x3 * y1 - x1 * y3 - x2 * y1 - x3 * y2;
            var S = 0.5 * Math.Abs(result);

            var delta_S = S - 0;
            var condition = 1;

            if (delta_S <= condition)
            {
                circlePoint.X = 0;
                circlePoint.Y = 0;
                radius = 0;
                return false;
            }

            var A = 2 * (x1 - x2);
            var B = 2 * (y1 - y2);
            var C = 2 * (x1 - x3);
            var D = 2 * (y1 - y3);
            var E = Math.Pow(x1, 2) - Math.Pow(x2, 2) + Math.Pow(y1, 2) - Math.Pow(y2, 2);
            var F = Math.Pow(x1, 2) - Math.Pow(x3, 2) + Math.Pow(y1, 2) - Math.Pow(y3, 2);

            circlePoint.X = (B * F - D * E) / (B * C - A * D);
            circlePoint.Y = (C * E - A * F) / (B * C - A * D);

            radius = Math.Sqrt(Math.Pow((x1 - circlePoint.X), 2) + Math.Pow((y1 - circlePoint.Y), 2));
            return true;
        }


        public static bool CheckXYCoordIsValid(XYCoord point, XYCoord circlePoint, double radius)
        {
            var distance = Math.Sqrt(Math.Pow((point.X - circlePoint.X), 2) + Math.Pow((point.Y - circlePoint.Y), 2));

            if (distance > radius)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
