using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestPackage
{
    public class RawDatumItem_Optical : RawDatumItemBase
    {
        public RawDatumItem_Optical()
        {


        }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Angle")]
        public double Angle { get; set; }
        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Position_X_mm")]
        //public double Position_X_mm { get; set; }
        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Position_Y_mm")]
        //public double Position_Y_mm { get; set; }
        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Position_Z_mm")]
        //public double Position_Z_mm { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }



    }
}
