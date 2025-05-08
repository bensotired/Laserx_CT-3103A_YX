using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_NanoScanAnalyse : RawDatumItemBase
    {
        public RawDatumItem_NanoScanAnalyse() 
        {
            X_Amplitude_1st = 0.0;
            X_Position_1st = 0.0;
            Y_Amplitude_1st = 0.0;
            Y_Position_1st = 0.0;

            X_Amplitude_2nd = 0.0;
            X_Position_2nd = 0.0;
            Y_Amplitude_2nd = 0.0; 
            Y_Position_2nd = 0.0;
        }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("X_Position_1st")]
        public double X_Position_1st { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("X_Amplitude_1st")]
        public double X_Amplitude_1st { get; set; }


        //[RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Y_Position_1st")]
        public double Y_Position_1st { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Y_Amplitude_1st")]
        public double Y_Amplitude_1st { get; set; }




        //只能显示一个，这后面不显示了
        //[RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("X_Position_2nd")]
        public double X_Position_2nd { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("X_Amplitude_2nd")]
        public double X_Amplitude_2nd { get; set; }



        //[RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Y_Position_2nd")]
        public double Y_Position_2nd { get; set; }
        [RawDataChartAxisElement(CEAxisXY.Y2)]
        [RawDataCollectionItemElement("Y_Amplitude_2nd")]
        public double Y_Amplitude_2nd { get; set; }
       

    }
}