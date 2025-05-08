using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;

namespace SolveWare_Vision
{
    public class PixPerMM
    {
        public PixPerMM()
        {
            X_ppm = 0.0;
            Y_ppm = 0.0;
        }
        public PixPerMM(double x_ppm, double y_ppm)
        {
            X_ppm = x_ppm;
            Y_ppm = y_ppm;
        }
        public double X_ppm { get; set; }
        public double Y_ppm { get; set; }
    }
}