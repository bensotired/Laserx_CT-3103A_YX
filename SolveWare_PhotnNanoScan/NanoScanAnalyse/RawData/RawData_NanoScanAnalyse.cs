using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestPackage;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_NanoScanAnalyse : RawDataCollectionBase<RawDatumItem_NanoScanAnalyse>
    {
        public RawData_NanoScanAnalyse() 
        {
            IsAlignmentDone = false;
            BeamWidth_13p5_X_1st = 0.0;
            BeamWidth_50_X_1st = 0.0;
            BeamWidth_13p5_Y_1st = 0.0;
            BeamWidth_50_Y_1st = 0.0;

            BeamWidth_13p5_X_2nd = 0.0;
            BeamWidth_50_X_2nd = 0.0;
            BeamWidth_13p5_Y_2nd = 0.0;
            BeamWidth_50_Y_2nd = 0.0;
            FF_DrivingCurrent_mA = 0.0;
            MoveDistance_mm = 0;
            FF_Temperature_degC = 25.0;
        }
        [RawDataBrowsableElement]
        public bool IsAlignmentDone { get; set; }

        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_13p5_X_1st { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_50_X_1st { get; set; }
        //[RawDataBrowsableElement]
        //[RawDataPrintableElement]
        //public double BeamWidth_5_X_1st { get; set; }


        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_13p5_Y_1st { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_50_Y_1st { get; set; }

        //[RawDataBrowsableElement]
        //[RawDataPrintableElement]
        //public double BeamWidth_5_Y_1st { get; set; }




        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_13p5_X_2nd { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_50_X_2nd { get; set; }
        //[RawDataBrowsableElement]
        //[RawDataPrintableElement]
        //public double BeamWidth_5_X_2nd { get; set; }


        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_13p5_Y_2nd { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double BeamWidth_50_Y_2nd { get; set; }
        //[RawDataBrowsableElement]
        //[RawDataPrintableElement]
        //public double BeamWidth_5_Y_2nd { get; set; }



        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double FF_DrivingCurrent_mA { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double FF_Temperature_degC { get; set; } 
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double MoveDistance_mm { get; set; }
        

    }
}