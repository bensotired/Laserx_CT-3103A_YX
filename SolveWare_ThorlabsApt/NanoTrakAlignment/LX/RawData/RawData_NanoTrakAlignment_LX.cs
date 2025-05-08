using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_NanoTrakAlignment_LX : RawDataCollectionBase<RawDatumItem_NanoTrakAlignment_LX>
    {
        public RawData_NanoTrakAlignment_LX()
        {
            IsAlignmentDone = false;
            Final_Power_mW = 0.0;
            //Final_Position_X_mm = 0.0;
            //Final_Position_Y_mm = 0.0;
            //Final_Position_Z_mm = 0.0;
            NanoTrakAverageCurrent_mA = 0.0;
        }
        [RawDataBrowsableElement]
        public bool IsAlignmentDone { get; set; }
        [RawDataBrowsableElement]
        public double Final_Power_mW { get; set; }
        //[RawDataBrowsableElement]
        //public double Final_Position_X_mm { get; set; }
        //[RawDataBrowsableElement]
        //public double Final_Position_Y_mm { get; set; }
        //[RawDataBrowsableElement]
        //public double Final_Position_Z_mm { get; set; }
        [RawDataBrowsableElement]
        public double NanoTrakAverageCurrent_mA { get; set; }
        [RawDataBrowsableElement]
        public double NanoTrakAveragPower_mW { get; set; } = 0.0;

    }
}