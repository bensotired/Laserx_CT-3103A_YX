using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawDataMenu_Tap_PD : RawDataMenuCollection<RawData_LIV_Tap_PD>
    {
        public RawDataMenu_Tap_PD()
        {
            mPd1_uA = double.NaN;
            mPd2_uA = double.NaN;
            Wavelength = double.NaN;
            Deviation = double.NaN;
            SMSR = double.NaN;
        }
        [RawDataBrowsableElement]
        public double mPd1_uA { get; set; }
        [RawDataBrowsableElement]
        public double mPd2_uA { get; set; }
        //Wavelength[nm]	Deviation[nm]	SMSR[dB]

        [RawDataBrowsableElement]
        public double Wavelength { get; set; }
        [RawDataBrowsableElement]
        public double Deviation { get; set; }
        [RawDataBrowsableElement]
        public double SMSR { get; set; }


    }


    [Serializable]
    public class RawDataMenu_Tap_PD_SP : RawDataMenuCollection<RawData_LIV_Tap_PD_SP>
    {
        public RawDataMenu_Tap_PD_SP()
        {

        }

    }

}