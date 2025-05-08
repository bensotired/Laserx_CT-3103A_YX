using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_Curr : RawDataCollectionBase<RawDatumItem_Curr>
    {
        public RawData_Curr()
        {
            
        }
        [RawDataBrowsableElement]
        public string Section { get; set; }
        [RawDataBrowsableElement]
        public double I_Start_mA { get; set; }

        [RawDataBrowsableElement]
        public double I_Step_mA { get; set; }

        [RawDataBrowsableElement]
        public double I_Stop_mA { get; set; }

    }
}