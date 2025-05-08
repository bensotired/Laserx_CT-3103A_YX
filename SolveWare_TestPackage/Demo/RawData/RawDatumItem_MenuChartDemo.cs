using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_MenuChartDemo : RawDatumItemBase
    {
        public RawDatumItem_MenuChartDemo()
        {
            
        }
 
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Value_X")]
        public int Value_X { get; set; }



        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Value_Y1")]
        public double Value_Y1 { get; set; }





        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        //[RawDataCollectionItemElement("Value_Y2")]
        //public double Value_Y2 { get; set; }



    }
}