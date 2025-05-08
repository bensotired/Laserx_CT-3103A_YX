using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawDatumItem_Coupling_PD : RawDatumItemBase
    {
        public RawDatumItem_Coupling_PD()
        {
            Power = 0.0;
            PDCurrent_mA = 0;
        }

        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power")]
        public double Power { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("PDCurrent_mA")]
        public double PDCurrent_mA { get; set; }

        [RawDataCollectionItemElement("Pos_X")]
        public double Pos_X { get; set; }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Pos_Y")]
        public double Pos_Y { get; set; }

        [RawDataCollectionItemElement("Pos_Z")]
        public double Pos_Z { get; set; }
    }
}