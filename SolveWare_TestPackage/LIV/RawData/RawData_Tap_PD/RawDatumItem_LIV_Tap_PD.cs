using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_LIV_Tap_PD : RawDatumItemBase
    {
        public RawDatumItem_LIV_Tap_PD()
        {
            Section = string.Empty;
            Current_mA = double.NaN;
            Power_mW = double.NaN;
            Voltage_V = double.NaN;
        }
        [RawDataCollectionItemElement("Section")]
        public string Section { get; set; }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA")]
        public double Current_mA { get; set; }
       
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power_mW")]
        public double Power_mW { get; set; }


        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Voltage_V")]
        public double Voltage_V { get; set; }
    }


    [Serializable]

    public class RawDatumItem_LIV_Tap_PD_SP : RawDatumItemBase
    {
        public RawDatumItem_LIV_Tap_PD_SP()
        {
            Section = string.Empty;
            Wavelength_nm = double.NaN;
            Power_dBm = double.NaN;
        }
        [RawDataCollectionItemElement("Section")]
        public string Section { get; set; }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Wavelength_nm")]
        public double Wavelength_nm { get; set; }
       
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power_dbm")]
        public double Power_dBm { get; set; }
    }

}