using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_AlignmentDemo : RawDatumItemBase
    {
        public RawDatumItem_AlignmentDemo()
        {
            Current_mA = 0.0;
            Voltage_V = 0.0;
            Power_mW = 0.0;
            Temperature = 0.0;
        }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA")]
        public double Current_mA { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }

        [RawDataCollectionItemElement("Temperature")]
        public double Temperature { get; set; }
    }
}