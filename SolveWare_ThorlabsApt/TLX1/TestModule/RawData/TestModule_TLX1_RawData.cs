using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class RawData_TLX1 : RawDataCollectionBase<RawDatumItem_TLX1>
    {
        public RawData_TLX1() 
        {
         
        }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public bool SetLaserPower_SW { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public bool SetDither_SW { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public int SetGetITUChannel { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public int SetGetFrequency { get; set; }

        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public int SetGetSystemWavelength { get; set; }
        //[DisplayName("SetGetBand")]
        //[PropEditable(true)]
        //public string SetGetBand { get; set; }



        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public bool SetVOAPower_SW { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public bool SetVOAMode_SW { get; set; }


        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public float SetOpticalAttenuationValue { get; set; }

        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public bool Use_dBm0rmW { get; set; }

        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public float SetOpticalOutputPowerValue_dBm { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public float SetOpticalOutputPower_mW { get; set; }
        [RawDataBrowsableElement]
        //[RawDataPrintableElement]
        public double TLX1Power { get; set; } 
    }
}
