using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_SPECT : RawDatumItemBase
    {
        public RawDatumItem_SPECT()
        {
            Wavelength_nm = 0.0;
            Power = 0.0;
        }
        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Wavelength_nm") ]
        public double Wavelength_nm { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power")]
        public double Power { get; set; }
    }
}