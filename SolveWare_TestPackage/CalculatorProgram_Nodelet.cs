using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    class CalculatorProgram
    {
        //[STAThread]
        public static void Main(params string[] args)
        {
            double[] temp = { 1, 2, 3, 4, 5, 0, -1, -2, 3, 4, 5, 6, 3, 2, 0, -1 };

            List<Range> IncreasingRanges = QWLT_InternalMath.FindIncreasingRanges(temp);
            List<Range> DecreasingRanges = QWLT_InternalMath.FindDecreasingRanges(temp);

            //var tempqqq  = QWLT_InternalMath.FindIncreasingDecreasingRanges(temp);

        


            //    try
            //    {
            //        var path = @"C:\Users\Administrator\Desktop\纵慧\单模块测试\debug.csv";
            //        //Current_mA	Power_mW	Voltage_V	PCE	PDCurrent_mA	PDVoltage_V

            //        //List<double> currs = new List<double>();
            //        //List<double> pows = new List<double>();
            //        //List<double> volts = new List<double>();

            //        RawData_LIV rd = new RawData_LIV();
            //        rd.RawDataFixFormat = "";
            //        StreamReader sr = new StreamReader(path);
            //        {
            //            sr.ReadLine();
            //            do
            //            {
            //                var line = sr.ReadLine();
            //                var lineArr = line.Split(',');



            //                rd.Add(new RawDatumItem_LIV
            //                {
            //                    Current_mA = Convert.ToDouble(lineArr[0]),
            //                    Power_mW = Convert.ToDouble(lineArr[1]),
            //                    Voltage_V = Convert.ToDouble(lineArr[2])
            //                });


            //            } while (sr.EndOfStream == false);
            //        }
            //        sr.Close();


            //        //ith + 90 
            //        CancellationTokenSource cts = new CancellationTokenSource();
            //        List<SummaryDatumItemBase> summaryDataWithoutSpec = new List<SummaryDatumItemBase>();
            //        TestCalculator_LIV_Kink1_zh calc = new TestCalculator_LIV_Kink1_zh();
            //        CalcRecipe_LIV_Kink1_zh rcp = new CalcRecipe_LIV_Kink1_zh();


            //        rcp.A2_Power_mW = 5.0;
            //        rcp.A1_AboveIthCurrent = 70;
            //        rcp.SEMax_End_AboveIthCurrent = 90;
            //        rcp.SEMax_Start_AboveIthCurrent = 1;

            //        rcp.Ith2_StartP_mW = 0.5;
            //        rcp.Ith2_StopP_mW = 3;
            //        calc.Localization(rcp);



            //        calc.Run(rd, ref summaryDataWithoutSpec, cts.Token);


            //    }
            //    catch (Exception ex)
            //    {

            //    }
        }
    }
}