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
    /// 找出SE最大值
    /// </summary>
    public class TestCalculator_LIV_Kink1 : TestCalculatorBase
    {
        public TestCalculator_LIV_Kink1() : base() { }

        public CalcRecipe_LIV_Kink1 CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Kink1);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Kink1)(testRecipe);
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Kink1{CalcRecipe.CalcData_PostFix}");
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
                    throw new Exception($"Kink1: xArray and yArray are of unequal size!");
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

                //计算出区间的SE 寻找最大的SE
                double SE_Find_StartCurrent = ith2 + this.CalcRecipe.SEMax_Start_AboveIthCurrent;
                double SE_Find_EndCurrent   = ith2 + this.CalcRecipe.SEMax_End_AboveIthCurrent;
                List<double> SElist_SelectRange = new List<double>();
                for (int i = 0; i < dict[CurrentTag].Count; i++)
                {
                    if (SE_Find_StartCurrent <= dict[CurrentTag][i] && dict[CurrentTag][i] <= SE_Find_EndCurrent)
                    {
                        SElist_SelectRange.Add(SElist_All[i]);
                    }
                }
                double SEMax  = SElist_SelectRange.Max();
                double SE0 = Interpolator.DoPiecewiseLinearInterpolation(CalcRecipe. SE0_Power, dict[PowerTag].ToArray(), SElist_All);
                dataSummary_1.Value = Math.Round(100 * SEMax / SE0);//使用的是百分比


                #region
                //Dictionary<double, double> dldi = new Dictionary<double, double>();
                //List<double> sorted_I = new List<double>();
                //List<double> sorted_SE = new List<double>();
                //var len = dict[CurrentTag].Count;
                //const int sampleRange = 2;
                //const int sampleSeed = 3;

                //for (int i = sampleRange; i < len - sampleRange; i++)
                //{
                //    var currs_21 = dict[CurrentTag].GetRange(i - sampleRange, sampleSeed);
                //    var pows_21 = dict[PowerTag].GetRange(i - sampleRange, sampleSeed);
                //    var polyFitLI = PolyFitMath.PolynomialFit(currs_21.ToArray(), pows_21.ToArray(), 1);

                //    dldi.Add(dict[CurrentTag][i], polyFitLI.Coeffs[1]);

                //}
                //double[] currSECalCurrent = dldi.Keys.ToArray();
                //double[] dldiFitSECalCurrent = dldi.Values.ToArray();
                //var seFittingCoeff = PolyFitMath.PolynomialFit(currSECalCurrent, dldiFitSECalCurrent, 4);

                //for (int i = 0; i < dict[CurrentTag].Count; i++)
                //{
                //    var current = dict[CurrentTag][i];
                //    if (currSECalCurrent.Contains(current))
                //    {

                //        var se = seFittingCoeff.Coeffs[4] * Math.Pow(current, 4) +
                //                 seFittingCoeff.Coeffs[3] * Math.Pow(current, 3) +
                //                 seFittingCoeff.Coeffs[2] * Math.Pow(current, 2) +
                //                 seFittingCoeff.Coeffs[1] * Math.Pow(current, 1) +
                //                 seFittingCoeff.Coeffs[0];

                //        sorted_I.Add(current);
                //        sorted_SE.Add(se);
                //    }
                //}
                //double SEMax_CurrentStart = ith2 + this.CalcRecipe.SEMax_Start_AboveIthCurrent;
                //double SEMax_CurrentEnd = ith2 + this.CalcRecipe.SEMax_End_AboveIthCurrent;
                //double SEMax = double.MinValue;
                //for (int i = 0; i < sorted_I.Count; i++)
                //{
                //    if (sorted_I[i] >= SEMax_CurrentStart && sorted_I[i] <= SEMax_CurrentEnd)
                //    {
                //        if (SEMax <= sorted_SE[i])
                //        {
                //            SEMax = sorted_SE[i];
                //        }
                //    }
                //}

                //double SE0 = GetSlopeEfficiencyForSpecifiedCurrent(sorted_SE, sorted_I, CalcRecipe.SE0_Power);
                //dataSummary_1.Value = Math.Round(100 * SEMax / SE0);//使用的是百分比
                #endregion
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Kink1_ParamError, ex);
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