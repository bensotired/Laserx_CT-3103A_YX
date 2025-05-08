using SolveWare_TestComponents.Attributes;
using System;
using SolveWare_BurnInInstruments;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_ACR : RawDataCollectionBase<RawDatumItem_ACR>
    {
        public RawData_ACR()
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