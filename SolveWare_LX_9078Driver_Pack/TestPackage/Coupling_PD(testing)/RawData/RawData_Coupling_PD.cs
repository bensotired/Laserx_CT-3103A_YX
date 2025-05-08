using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_Coupling_PD : RawDataCollectionBase<RawDatumItem_Coupling_PD>
    {
        public RawData_Coupling_PD()
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