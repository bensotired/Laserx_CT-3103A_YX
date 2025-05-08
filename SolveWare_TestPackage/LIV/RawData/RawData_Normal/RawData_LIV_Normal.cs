using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_LIV_Normal : RawDataCollectionBase<RawDatumItem_LIV_Normal>
    {
        public RawData_LIV_Normal()
        {
            MAX_Power = double.NaN;
            Pout_120mA_Power = double.NaN;
        }
        [RawDataBrowsableElement]
        public double MAX_Power { get; set; }

        [RawDataBrowsableElement]
        public double Pout_120mA_Power { get; set; }

    }
}