using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_Coupling_LD : RawDataCollectionBase<RawDatumItem_Coupling_LD>
    {
        public RawData_Coupling_LD()
        {
        }

        [RawDataBrowsableElement]
        public double Position_X_mm { get; set; }
        [RawDataBrowsableElement]
        public double Position_Y_mm { get; set; }
        [RawDataBrowsableElement]
        public double Position_Z_mm { get; set; }
        [RawDataBrowsableElement]
        public double PDCurrent_mA { get; set; }
    }
}