using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestPackage
{
    public class RawDatumItem_FF : RawDatumItemBase
    {
        public RawDatumItem_FF()
        {

        }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Short_Theta")]
        public double Short_Theta { get; set; }

        [RawDataCollectionItemElement("Short_Point_PD_Reading")]
        public double Short_Point_PD_Reading { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Short_Point_PD_ReadingTo1")]
        public double Short_Point_PD_ReadingTo1 { get; set; }



        [RawDataChartAxisElement(CEAxisXY.X2)]
        [RawDataCollectionItemElement("Long_Theta")]
        public double Long_Theta { get; set; }

        [RawDataCollectionItemElement("Long_Point_PD_Reading")]
        public double Long_Point_PD_Reading { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Long_Point_PD_ReadingTo1")]
        public double Long_Point_PD_ReadingTo1 { get; set; }


    }
}
