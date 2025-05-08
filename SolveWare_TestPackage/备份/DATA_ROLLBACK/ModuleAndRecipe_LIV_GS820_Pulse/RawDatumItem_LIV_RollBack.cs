using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_LIV_RollBack : RawDatumItemBase
    {
        public RawDatumItem_LIV_RollBack()
        {
            Current_mA = 0.0;
            Voltage_V = 0.0;
            PDCurrent_mA = 0.0;
            //PDVoltage_V = 0.0;
            Power_mW = 0.0;
        }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA")]
        public double Current_mA { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }
        [RawDataCollectionItemElement("PDCurrent_mA")]
        public double PDCurrent_mA { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }
    }
}