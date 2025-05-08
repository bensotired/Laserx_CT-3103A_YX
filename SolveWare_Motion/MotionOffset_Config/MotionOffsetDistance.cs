namespace SolveWare_Motion
{

    public class MotionOffsetDistance
    {
        public MotionOffsetDistance()
        {
            Offset_X = 0.0;
            Offset_Y = 0.0;
        }

        public MotionOffsetDistance(double offset_x, double offset_y)
        {
            Offset_X = offset_x;
            Offset_Y = offset_y;
        }


        public double Offset_X { get; set; }

        public double Offset_Y { get; set; }
    }
}