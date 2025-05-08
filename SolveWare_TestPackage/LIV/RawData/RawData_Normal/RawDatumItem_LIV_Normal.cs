using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_LIV_Normal : RawDatumItemBase
    {
        public RawDatumItem_LIV_Normal()
        {
            Section = string.Empty;
            Current_mA = double.NaN;
            Power_mW = double.NaN;
            Voltage_V = double.NaN;
        }
        [RawDataCollectionItemElement("Section")]
        public string Section { get; set; }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA")]
        public double Current_mA { get; set; }
       
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }


        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }
    }
}