using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_Volt : RawDataCollectionBase<RawDatumItem_Volt>
    {
        public RawData_Volt()
        {
            
        }
        [RawDataBrowsableElement]
        public string Section { get; set; }
        [RawDataBrowsableElement]
        public double Start_V { get; set; }

        [RawDataBrowsableElement]
        public double Step_V { get; set; }

        [RawDataBrowsableElement]
        public double Stop_V { get; set; }

    }
}