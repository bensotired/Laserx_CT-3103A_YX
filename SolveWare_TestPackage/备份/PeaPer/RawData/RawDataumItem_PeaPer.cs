using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDataumItem_PeaPer : RawDatumItemBase
    {              
        public RawDataumItem_PeaPer() 
        {
            Angle_deg = 0.0;
            Power_mW = 0.0;
            Current_mA = 0.0;
        }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Angle_deg")]
        public double Angle_deg { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }
        [RawDataCollectionItemElement("Current_mA")]
        public double Current_mA { get; set; }
    }
}