using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawDatumItem_FarField : RawDatumItemBase
    {
        public RawDatumItem_FarField()
        {
            Theta = 0.0;
            PDCurrent = 0.0;
        }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Theta")]
        public double Theta { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("PDCurrent")]
        public double PDCurrent { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("AnalogVoltage")]
        public double AnalogVoltage { get; set; }
    }
}