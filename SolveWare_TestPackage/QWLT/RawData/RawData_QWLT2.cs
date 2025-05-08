using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_QWLT2 : RawDataCollectionBase<RawDatumItem_QWLT2>
    {
        public RawData_QWLT2()
        {
            
        }
        [RawDataBrowsableElement]
        public string Section { get; set; }
        [RawDataBrowsableElement]
        public string Driver_mA { get; set; }
        [RawDataBrowsableElement]
        public string resut { get; set; }
        //[RawDataBrowsableElement]
        public double PH_Halfway_1 { get; set; }
        //[RawDataBrowsableElement]
        public double PH_Halfway_2 { get; set; }
        //[RawDataBrowsableElement]
        public double MIRROR1_mid_slope_val { get; set; }
        //[RawDataBrowsableElement]
        public double MIRROR2_mid_slope_val { get; set; }
        //[RawDataBrowsableElement]
        public double PH_Max_1 { get; set; }
        //[RawDataBrowsableElement]
        public double PH_Max_2 { get; set; }
        //[RawDataBrowsableElement]
        public double PH_Max_Sec_1 { get; set; }
        //[RawDataBrowsableElement]
        public double PH_Max_Sec_2 { get; set; }

    }
}