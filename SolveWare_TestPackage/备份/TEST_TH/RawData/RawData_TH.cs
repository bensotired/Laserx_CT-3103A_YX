using SolveWare_TestComponents.Attributes;
using System;
using SolveWare_BurnInInstruments;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_TH : RawDataCollectionBase<RawDatumItem_TH>
    {
        public RawData_TH()
        {
            //Timeinterval_s = 5;
            //SamplingTemperature_C = 25;
        }
        [RawDataBrowsableElement]
        public double Timeinterval_s { get; set; }

        //[RawDataBrowsableElement]
        //public double SamplingTemperature_C { get; set; }

    }
}