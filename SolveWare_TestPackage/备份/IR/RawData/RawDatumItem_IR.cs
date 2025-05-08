using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_IR : RawDatumItemBase
    {
        public RawDatumItem_IR()
        {
         
        }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("IRIndex")]
        public double IRIndex { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("IR_Current_A")]
        public double IR_Current_A { get; set; }
    }
}