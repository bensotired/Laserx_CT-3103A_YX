using System;

namespace SolveWare_Vision
{
    public class PixPoint
    {
        public static bool IsPixPointInRange(PixPoint actualPoint, PixPoint basePoint)
        {
            if ((actualPoint.X_Pix - basePoint.X_Pix) > basePoint.X_tolerance_Pix)
            {
                return false;
            }
            if ((actualPoint.Y_Pix - basePoint.Y_Pix) > basePoint.Y_tolerance_Pix)
            {
                return false;
            }
            return true;
        }
        double _X_tolerance_Pix = 0.0;
        double _Y_tolerance_Pix = 0.0;
        public PixPoint()
        {
            X_Pix = 0.0;
            Y_Pix = 0.0;
        }
        public PixPoint(double x_pix, double y_pix)
        {
            X_Pix = x_pix;
            Y_Pix = y_pix;
        }
        public double X_Pix { get; set; }
        public double Y_Pix { get; set; }

        public double X_tolerance_Pix
        {
            get
            {
                return _X_tolerance_Pix;
            }
            set
            {
                _X_tolerance_Pix = Math.Abs(value);
            }
        }

        public double Y_tolerance_Pix
        {
            get
            {
                return _Y_tolerance_Pix;
            }
            set
            {
                _Y_tolerance_Pix = Math.Abs(value);
            }
        }
    }
}
