using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_TED : RawDatumItemBase
    {
        public RawDatumItem_TED()
        {
            Section = string.Empty;
            Current_mA = 0.0;
            Voltage_V = 0.0;
            Differentiate = 0.0;
        }
        [RawDataCollectionItemElement("Section")]
        public string Section { get; set; } 
        
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }

        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Current_mA")]
        public double Current_mA { get; set; }
       
     

        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Differentiate")]
        public double Differentiate { get; set; }

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