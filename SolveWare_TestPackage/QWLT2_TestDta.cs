using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    //public class AfterTest
    //{
    //    public AfterTest()
    //    {
    //    }
    //    public static double BIAS2 { get; set; }
    //    public static double SOA1 { get; set; }
    //    public static double SOA2 { get; set; }
    //    public static double MIRROR2 { get; set; }
    //    public static double LP { get; set; }
    //    public static double MPD1 { get; set; }
    //    public static double MIRROR1 { get; set; }
    //    public static double BIAS1 { get; set; }
    //    public static double MPD2 { get; set; }
    //    public static double PH2 { get; set; }
    //    public static double PH1 { get; set; }
    //    public static double GAIN { get; set; }

    //}
    public class QWLT2_TestDta
    {
        public QWLT2_TestDta()
        {
            GAIN = 120;
            SOA1 = 50;
            SOA2 = 40;
            MIRROR1 = 0;
            MIRROR2 = 0;
            PH1 = 1;
            PH2 = 0;
            LP = 4;
            MPD1 = -2.5;
            MPD2 = -2.5;
            BIAS1 = -2;
            BIAS2 = -2;
        }
        public  double BIAS2 { get; set; }
        public  double SOA1 { get; set; }
        public  double SOA2 { get; set; }
        public  double MIRROR2 { get; set; }
        public  double LP { get; set; }
        public  double MPD1 { get; set; }
        public  double MIRROR1 { get; set; }
        public  double BIAS1 { get; set; }
        public  double MPD2 { get; set; }
        public  double PH2 { get; set; }
        public  double PH1 { get; set; }
        public  double GAIN { get; set; }

    }
    public class PDCalibrationData
    {
        public string Description { get; set; }
        public double PD_B { get; set; }
        public DataBook<double,double> PD_K { get; set; }
    }
}
