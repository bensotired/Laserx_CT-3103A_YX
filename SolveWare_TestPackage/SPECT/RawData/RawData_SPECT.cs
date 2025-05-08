using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_SPECT : RawDataCollectionBase<RawDatumItem_SPECT>
    {
        public RawData_SPECT()
        {
            //Temperature = 25;
            Wavelength_nm = 1300;
            PeakPower_dbm = 0;
            SpectrumWidth_20db_nm = 0;
            SpectrumWidth_3db_nm = 0;
            Smsr_dB = 0;
            Trace = new PowerWavelengthTrace();
            LaserCurrent_A = 0;
        }

        [RawDataBrowsableElement]
        public double Wavelength_nm { get; set; }

        [RawDataBrowsableElement]
        public double PeakPower_dbm { get; set; }

        [RawDataBrowsableElement]
        public double SpectrumWidth_20db_nm { get; set; }

        [RawDataBrowsableElement]
        public double SpectrumWidth_3db_nm { get; set; }

        [RawDataBrowsableElement]
        public double Smsr_dB { get; set; }


        [RawDataBrowsableElement]
        public PowerWavelengthTrace Trace { get; set; }

        [RawDataBrowsableElement]
        public double LaserCurrent_A { get; set; }

    }
}