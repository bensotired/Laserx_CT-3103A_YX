using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
 
    public class RawDatumItem_Sample : RawDatumItemBase
    {
        public RawDatumItem_Sample()
        {
            Current_mA = 0.0;
            Voltage_V = 0.0;
            Power_mW = 0.0;
        }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA") ]
        public double Current_mA { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }
    }
}