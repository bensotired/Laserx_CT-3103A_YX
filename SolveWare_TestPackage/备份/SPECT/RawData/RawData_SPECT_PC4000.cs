using SolveWare_TestComponents.Attributes;
using System;
using SolveWare_BurnInInstruments;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_SPECT_PC4000 : RawDataCollectionBase<RawDatumItem_SPECT_PC4000>
    {
        public RawData_SPECT_PC4000()
        {
            LaserCurrent_mA = 0;
        }
       

        [RawDataBrowsableElement]
        public double LaserCurrent_mA { get; set; }

    }
}