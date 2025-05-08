using SolveWare_TestComponents.Attributes;
using System;
using SolveWare_BurnInInstruments;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_NF : RawDataCollectionBase<RawDatumItem_NF>
    {
        public RawData_NF() 
        {          
           // Gain = 0;
            NF_Temperature_degC = 25;
           // NF = 0;
        }    

        //[RawDataBrowsableElement]
        ////[RawDataPrintableElement]
        //public double Gain { get; set; }
        //[RawDataBrowsableElement]
        ////[RawDataPrintableElement]
        //public double NF { get; set; }
        [RawDataBrowsableElement]
        //[RawDataPrintableElement]
        public double TraceA_CenterWavelength_nm { get; set; } 
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double NF_Temperature_degC { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double DrivingCurrent_mA { get; set; }

        //计算参数
        //public int AALGo { get; set; } = 0;
        //public int FALGo { get; set; } = 0;
        //public double FARea { get; set; } = 1;
        //public double IOFFset { get; set; } = 0;
        //public double IRANge { get; set; } = 10;
        //public double MARea { get; set; } = 0.4;
        //public double MDIFF { get; set; } = 3.0;
        //public double OOFFset { get; set; } = 0;
        //public int PDISplay { get; set; } = 1;
        //public double TH { get; set; } = 20;
        //public int RBWidth { get; set; } = 1;
        //public int SNOISE { get; set; } = 1;
        //public int SPOWer { get; set; } = 0;
    }
}