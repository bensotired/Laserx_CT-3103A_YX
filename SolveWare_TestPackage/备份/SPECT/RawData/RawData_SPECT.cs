using SolveWare_TestComponents.Attributes;
using System;
using SolveWare_BurnInInstruments;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_SPECT : RawDataCollectionBase<RawDatumItem_SPECT>
    {
        public RawData_SPECT()
        {
            CenterWavelength_nm = 1300;
            PeakPower_dbm = 0;
            PeakWavelength_nm = 0;
            SpectrumWidth_20db_nm = 0;
            SpectrumWidth_3db_nm = 0;
            Smsr_dB = 0;
            Trace = new PowerWavelengthTrace();
            LaserCurrent_mA = 0;
            SPECT_Temperature_degC = 25;
            SPECT_DrivingCurrent_mA = "40";
        }
        [RawDataBrowsableElement]
        public double CenterWavelength_nm { get; set; }

        [RawDataBrowsableElement]
        public double PeakPower_dbm { get; set; }
        [RawDataBrowsableElement]
        public double PeakWavelength_nm { get; set; }
        [RawDataBrowsableElement]
        public double SpectrumWidth_20db_nm { get; set; }

        [RawDataBrowsableElement]
        public double SpectrumWidth_3db_nm { get; set; }

        [RawDataBrowsableElement]
        public double Smsr_dB { get; set; }


        [RawDataBrowsableElement]
        public PowerWavelengthTrace Trace { get; set; }

        [RawDataBrowsableElement]
        public double LaserCurrent_mA { get; set; }
        [RawDataBrowsableElement]
        public string SPECT_DrivingCurrent_mA { get; set; }
        [RawDataBrowsableElement]
        public double SPECT_Temperature_degC { get; set; }
    }
}