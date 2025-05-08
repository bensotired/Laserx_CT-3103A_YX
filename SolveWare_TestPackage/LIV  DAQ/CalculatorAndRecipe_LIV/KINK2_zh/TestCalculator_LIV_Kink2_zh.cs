using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// kink2(纵慧版本)
    /// </summary>
    public class TestCalculator_LIV_Kink2_Percent_zh : TestCalculatorBase
    {
        public TestCalculator_LIV_Kink2_Percent_zh() : base() { }  

        public CalcRecipe_LIV_Kink2_zh CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Kink2_zh);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Kink2_zh)(testRecipe);
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                if (CalcRecipe.IsForceRename == true)
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                }
                else
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Kink2_zh{CalcRecipe.CalcData_PostFix}");
                }

                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";
                const string VoltageTag = "Voltage_V";
                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag, VoltageTag);

                if (dict[CurrentTag]?.Count <= 0 ||
                    dict[PowerTag]?.Count <= 0 ||
                    dict[VoltageTag]?.Count <= 0 ||
                    dict[CurrentTag]?.Count != dict[PowerTag]?.Count ||
                    dict[CurrentTag]?.Count != dict[VoltageTag]?.Count)
                {
                    throw new Exception($"Kink2_zh: xArray and yArray are of unequal size!");
                }
                double current1_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], CalcRecipe.Ith2_StartP_mW);   //计算出功率对应的电流
                double current2_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], CalcRecipe.Ith2_StopP_mW);   //计算出功率对应的电流

                //将这2个点数据进行直线拟合
                double[] powerArr = new double[] { CalcRecipe.Ith2_StartP_mW, CalcRecipe.Ith2_StopP_mW };

                double[] currentArr = new double[] { current1_mA, current2_mA };

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                //计算出电流
                var ith2 = polyData.Coeffs[0];


                double[] SElist_All = ArrayMath.CalculateFirstDerivate(dict[CurrentTag].ToArray(), dict[PowerTag].ToArray());

                double SEpmin =(double)this.GetSlopeEfficiencyForSpecifiedCurrent(SElist_All.ToList(), dict[CurrentTag], ith2 + CalcRecipe.Ith2_StartP_mW);   //计算出电流对应的SE
                //计算出区间的SE 寻找最大的SE
                double SE_Find_StartCurrent = ith2 + this.CalcRecipe.SEMax_Start_AboveIthCurrent;
                double SE_Find_EndCurrent   = ith2 + this.CalcRecipe.SEMax_End_AboveIthCurrent;
                List<double> SElist_SelectRange = new List<double>();
                List<double> Currentlist_SelectRange = new List<double>(); 
                for (int i = 0; i < dict[CurrentTag].Count; i++)
                {
                    if (SE_Find_StartCurrent <= dict[CurrentTag][i] && dict[CurrentTag][i] <= SE_Find_EndCurrent)
                    {
                        SElist_SelectRange.Add(SElist_All[i]);
                        Currentlist_SelectRange.Add(dict[CurrentTag][i]);
                    }
                }
                if (Currentlist_SelectRange.Count <= 0 ||SElist_SelectRange.Count <= 0)
                {
                    throw new Exception($"Kink2_zh: SElist_SelectRangeArray and SEpolyData_2Array are of unequal size!");
                }

                PolyFitData SEpolyData_2  = PolyFitMath.PolynomialFit(Currentlist_SelectRange.ToArray(), SElist_SelectRange.ToArray(),2);              

                List<double> SEdiff = new List<double>();
                for (int i = 0; i < SElist_SelectRange.Count; i++)
                {                   
                    SEdiff.Add(SElist_SelectRange[i] - SEpolyData_2.FittedYArray[i]);
                }

                double SEmax = SEdiff.Max();
                double SEmin = SEdiff.Min();

                dataSummary_1.Value = (SEmax - SEmin) / SEpmin * 100;
                //double SEMax  = SElist_SelectRange.Max();              
               

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Kink2_Percent_zh_ParamError, ex);
            }
        }
       
        public double? GetCurrentForSpecifiedPower(List<double> CurrentList, List<double> PowerList, double Power_mW)
        {
            try
            {

                int indexS;
                int indexE;

                //找到比需求功率大的功率
                for (indexE = 0; indexE < PowerList.Count; indexE++)
                {
                    if (PowerList[indexE] >= Power_mW)
                    {
                        break;
                    }
                }
                if (indexE == PowerList.Count) return 0;

                //找到比需求功率小的功率
                for (indexS = indexE; indexS > 0; indexS--)
                {
                    if (PowerList[indexS] < Power_mW)
                    {
                        break;
                    }
                }
                if (indexS == 0) return 0;



                //将该区间的数据进行直线拟合
                double[] powerArr = new double[indexE - indexS + 1];  //功率组成的数组
                double[] currentArr = new double[indexE - indexS + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexS; i <= indexE; i++)
                {
                    currentArr[i - indexS] = CurrentList[i];
                    powerArr[i - indexS] = PowerList[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                //计算出电流
                double CalcCurrent = polyData.Coeffs[1] * Power_mW + polyData.Coeffs[0];

                return CalcCurrent;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
        public double GetSlopeEfficiencyForSpecifiedCurrent(List<double> slopeEffs, List<double> currents, double current_mA)
        {
            try
            {

                int indexStart;
                int indexEnd;

                //找到比需求功率大的功率
                for (indexEnd = 0; indexEnd < slopeEffs.Count; indexEnd++)
                {
                    if (currents[indexEnd] >= current_mA)
                    {
                        break;
                    }
                }
                if (indexEnd == slopeEffs.Count) return 0;

                //找到比需求功率小的功率
                for (indexStart = indexEnd; indexStart > 0; indexStart--)
                {
                    if (currents[indexStart] < current_mA)
                    {
                        break;
                    }
                }
                if (indexStart == 0) return 0;


                //将该区间的数据进行直线拟合
                double[] currentArr = new double[indexEnd - indexStart + 1];  //功率组成的数组
                double[] slopeEffArr = new double[indexEnd - indexStart + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexStart; i <= indexEnd; i++)
                {
                    slopeEffArr[i - indexStart] = slopeEffs[i];
                    currentArr[i - indexStart] = currents[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:SE                      
                PolyFitData polyData = PolyFitMath.PolynomialFit(currentArr, slopeEffArr, 1);

                //计算出SE      
                double CalcSE = polyData.Coeffs[1] * current_mA + polyData.Coeffs[0];
                return CalcSE;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
    }
}