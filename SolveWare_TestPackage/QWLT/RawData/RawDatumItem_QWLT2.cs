using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_QWLT2 : RawDatumItemBase
    {
        public RawDatumItem_QWLT2()
        {
            Section = string.Empty;
            Current_mA_or_Mirror_Diagonal_Offset = 0.0;
            MPD_Current_mA = 0.0;
            M2_Current_mA = 0.0;
        }
        [RawDataCollectionItemElement("Section")]
        public string Section { get; set; }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA_or_Mirror_Diagonal_Offset")]
        public double Current_mA_or_Mirror_Diagonal_Offset { get; set; }
       
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("MPD_Current_mA")]
        public double MPD_Current_mA { get; set; }

        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Mirror_Diagonal_Offset")]
        //public double Mirror_Diagonal_Offset { get; set; }

        //[RawDataCollectionItemElement("M2_Current_mA")]
        public double M2_Current_mA { get; set; }


        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Derivated_1st")]
        public double Derivated_1st { get; set; }

        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Derivated_2nd")]
        public double Derivated_2nd { get; set; }

        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Differentiate")]
        //public double Differentiate { get; set; }

        //[RawDataCollectionItemElement("PCE")]
        //public double PCE{ get; set; } 

        ////[RawDataChartAxisElement(CEAxisXY.X)]
        //[RawDataCollectionItemElement("PDCurrent_mA")]
        //public double PDCurrent_mA { get; set; }
        ////[RawDataChartAxisElement(CEAxisXY.Y)]
        //[RawDataCollectionItemElement("PDVoltage_V")]
        //public double PDVoltage_V { get; set; }

    }
}