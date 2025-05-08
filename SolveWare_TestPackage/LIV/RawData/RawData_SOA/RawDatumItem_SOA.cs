using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_SOA : RawDatumItemBase
    {
        public RawDatumItem_SOA()
        {
            G_Current_mA = double.NaN;
            G_Voltage_V = double.NaN;
            S1_Current_mA = double.NaN;
            S2_Current_mA = double.NaN;
        }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("G_Current_mA")]
        public double G_Current_mA { get; set; }
       
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("G_Voltage_V")]
        public double G_Voltage_V { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("S1_Current_mA")]
        public double S1_Current_mA { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("S2_Current_mA")]
        public double S2_Current_mA { get; set; }

    }
}