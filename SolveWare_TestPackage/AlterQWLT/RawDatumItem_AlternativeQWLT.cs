using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_AlternativeQWLT : RawDatumItemBase
    {
        public RawDatumItem_AlternativeQWLT()
        {
            Section = string.Empty;
            Current_mA_or_Mirror_Diagonal_Offset = 0.0;
            MPD_Current_mA = 0.0;
            M2_Current_mA = 0.0;
        }
        [RawDataCollectionItemElement("Section")]
        public string Section { get; set; }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Current_mA_or_Mirror_Diagonal_Offset")]
        public double Current_mA_or_Mirror_Diagonal_Offset { get; set; }
       
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("MPD_Current_mA")]
        public double MPD_Current_mA { get; set; }

        [RawDataCollectionItemElement("Wavelength")]
        public double Wavelength_nm { get; set; }   //波长

  
        public double M2_Current_mA { get; set; }

 

    }
}