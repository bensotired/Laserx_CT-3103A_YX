using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_TH : RawDatumItemBase
    {
        public RawDatumItem_TH()
        {
            
            
        }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("SamplingTime_s") ]
        public double SamplingTime_s { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Resistance_R")]
        public double Resistance_R { get; set; }
        //[RawDataCollectionItemElement("SamplingTemperature_C")]
        //public double SamplingTemperature_C { get; set; }
    }
}