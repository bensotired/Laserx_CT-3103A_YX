using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawDataMenu_QWLT2 : RawDataMenuCollection<RawData_QWLT2>
    {
        public RawDataMenu_QWLT2()
        {
        }
        [RawDataBrowsableElement]
        public double PH_Halfway_1 { get; set; }//0
        [RawDataBrowsableElement]
        public double PH_Halfway_2 { get; set; }//1
        [RawDataBrowsableElement]
        public double MIRROR1_mid_slope_val { get; set; }
        [RawDataBrowsableElement]
        public double MIRROR2_mid_slope_val { get; set; }
        [RawDataBrowsableElement]
        public double LP { get; set; }
        [RawDataBrowsableElement]
        public double PH_Max_1 { get; set; }
        [RawDataBrowsableElement]
        public double PH_Max_2 { get; set; }
        [RawDataBrowsableElement]
        public double PH_Max_Sec_1 { get; set; }
        [RawDataBrowsableElement]
        public double PH_Max_Sec_2 { get; set; }//8
        [RawDataBrowsableElement]
        public double mPd1_V { get; set; }//9
        [RawDataBrowsableElement]
        public double mPd2_V { get; set; }

        [RawDataBrowsableElement]
        public double Bais1_V { get; set; }
        [RawDataBrowsableElement]
        public double Bais2_V { get; set; }
    }
}