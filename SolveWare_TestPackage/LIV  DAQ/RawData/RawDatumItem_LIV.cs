using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_LIV : RawDatumItemBase
    {
        public RawDatumItem_LIV()
        {
            
        }
        //源表输出电流
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA")]
        public double Current_mA { get; set; }

        //源表实际回读
        [RawDataCollectionItemElement("ActualCurrent_mA")]
        public double ActualCurrent_mA { get; set; }

        //源表实际回读
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }


        [RawDataCollectionItemElement("PDCurrent_mA")]
        public double PDCurrent_mA { get; set; }


        //daq采集
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }


        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("MPDCurrent_mA")]
        public double MPDCurrent_mA { get; set; }



        //[RawDataCollectionItemElement("MPDPower_mW")]
        //public double MPDPower_mW { get; set; }
    }
}