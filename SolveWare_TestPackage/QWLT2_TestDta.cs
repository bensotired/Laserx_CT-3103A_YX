using SolveWare_BurnInCommon;

namespace SolveWare_TestPackage
{


    public class PDCalibrationData
    {
        public string Description { get; set; }
        public double PD_B { get; set; }
        public DataBook<double,double> PD_K { get; set; }
    }
}
