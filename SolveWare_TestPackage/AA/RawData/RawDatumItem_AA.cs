using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestPackage
{
    public class RawDatumItem_AA : RawDatumItemBase
    {
        public RawDatumItem_AA()
        {
            Vertical_Theta = 0.0;
            Vertical_Point_PD_Reading = 0.0;
            Vertical_Point_PD_ReadingTo1 = 0.0;
            Horizon_Theta = 0.0;
            Horizon_Point_PD_Reading = 0.0;
            Horizon_Point_PD_ReadingTo1 = 0.0;
        }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Vertical_Theta")]
        public double Vertical_Theta { get; set; }

        [RawDataCollectionItemElement("Vertical_Point_PD_Reading")]
        public double Vertical_Point_PD_Reading { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Vertical_Point_PD_ReadingTo1")]
        public double Vertical_Point_PD_ReadingTo1 { get; set; }

        [RawDataChartAxisElement(CEAxisXY.X2)]
        [RawDataCollectionItemElement("Horizon_Theta")]
        public double Horizon_Theta { get; set; }

        [RawDataCollectionItemElement("Horizon_Point_PD_Reading")]
        public double Horizon_Point_PD_Reading { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Horizon_Point_PD_ReadingTo1")]
        public double Horizon_Point_PD_ReadingTo1 { get; set; }
    }
}