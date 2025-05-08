using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestPackage
{
    public class RawDatumItem_IRev : RawDatumItemBase
    {
        public RawDatumItem_IRev()
        {

        }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Current_A")]
        public double Current_A { get; set; }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }
    }
}
