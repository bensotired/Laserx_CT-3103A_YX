using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_NF : RawDatumItemBase
    {
        public RawDatumItem_NF() 
        {
            TRA_Wavelength_nm = 0.0;
            TRA_Power = 0.0;
            //TRB_Wavelength_nm = 0.0;
            TRB_Power = 0.0;
        }
       [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("TRA_Wavelength_nm")]
        public double TRA_Wavelength_nm { get; set; }
       [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("TRA_Power")]
        public double TRA_Power { get; set; }

        // [RawDataChartAxisElement(CEAxisXY.X)]
        //[RawDataCollectionItemElement("TRB_Wavelength_nm")]
        //public double TRB_Wavelength_nm { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("TRB_Power")]
        public double TRB_Power { get; set; }
        //[RawDataCollectionItemElement("EmptyList")]
        //public string EmptyList { get; set; }

        //NF结果
        //[RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Wavelength_nm")]
        public double Wavelength_nm { get; set; }
        [RawDataCollectionItemElement("Input_Lvl_dBm")]
        public double Input_Lvl_dBm { get; set; }
        [RawDataCollectionItemElement("Oupt_Lvl_dBm")]
        public double Oupt_Lvl_dBm { get; set; }
        [RawDataCollectionItemElement("Ase_Lvl_dBm")]
        public double Ase_Lvl_dBm { get; set; }
        [RawDataCollectionItemElement("Resoln_nm")]
        public double Resoln_nm { get; set; }
        //[RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Gain_dB")]
        public double Gain_dB { get; set; }
        //[RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("NF_dB")]
        public double NF_dB { get; set; }
    }
}