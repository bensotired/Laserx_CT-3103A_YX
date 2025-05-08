using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_ChartDemo : RawDatumItemBase
    {
        public RawDatumItem_ChartDemo()
        {
            
        }
 
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Value_X")]
        public int Value_X { get; set; }



        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Value_Y1")]
        public double Value_Y1 { get; set; }



        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Value_Y11")]
        public double Value_Y11 { get; set; }


        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Value_Y12")]
        public double Value_Y12 { get; set; }



        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Value_Y2")]
        public double Value_Y2 { get; set; }



        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Value_Y21")]
        public double Value_Y21 { get; set; }


        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Value_Y22")]
        public double Value_Y22 { get; set; }


    }
}