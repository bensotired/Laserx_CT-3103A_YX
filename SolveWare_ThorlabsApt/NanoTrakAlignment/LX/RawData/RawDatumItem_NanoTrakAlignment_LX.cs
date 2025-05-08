using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_NanoTrakAlignment_LX : RawDatumItemBase
    {
        public RawDatumItem_NanoTrakAlignment_LX()
        {
            //Position_X_mm = 0.0;
            //Position_Y_mm = 0.0;
            //Position_Z_mm = 0.0;
           // Power_mW = 0.0;
        }

        ////[RawDataChartAxisElement(CEAxisXY.X)]
        //[RawDataCollectionItemElement("Position_Index")]
        //public double Position_Index { get; set; }
        //// [RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Position_X_mm")]
        //public double Position_X_mm { get; set; }
        //// [RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Position_Y_mm")]
        //public double Position_Y_mm { get; set; }
        //// [RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Position_Z_mm")]
        //public double Position_Z_mm { get; set; }
        //// [RawDataChartAxisElement(CEAxisXY.Y)]
        //[RawDataCollectionItemElement("Power_mW")]
        //public double Power_mW { get; set; }
    }
}