using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// Iop算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_LIV_Dseop : TestCalculatorBase
    {
        public TestCalculator_LIV_Dseop() : base() { }

        public CalcRecipe_LIV_Dseop CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Dseop);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Dseop)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Dseop{CalcRecipe.CalcData_PostFix}");
                }

                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);

                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;

                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 ||
                    dict[PowerTag]?.Count <= 0 ||
                    dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"Dseop: xArray and yArray are of unequal size!");
                }

                double power1 = this.CalcRecipe.SE_Power1;
                double power2 = this.CalcRecipe.SE_Power2;
                double[] SElist_All = ArrayMath.CalculateFirstDerivate(dict[CurrentTag].ToArray(), dict[PowerTag].ToArray());
                double SE1 = Interpolator.DoPiecewiseLinearInterpolation(power1, dict[PowerTag].ToArray(), SElist_All);
                double SE2 = Interpolator.DoPiecewiseLinearInterpolation(power2, dict[PowerTag].ToArray(), SElist_All);
                dataSummary_1.Value = Math.Round((SE2 - SE1) / SE1, 3);




                #region 保留  
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
                //double SE1 = GetSlopeEfficiencyForSpecifiedPower(sorted_SE, sorted_I, CalcRecipe.SE_Power1);
                //double SE2 = GetSlopeEfficiencyForSpecifiedPower(sorted_SE, sorted_I, CalcRecipe.SE_Power2);
                //dataSummary_1.Value =Math.Round( (SE2-SE1)/SE1,3);
                #endregion
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Dseop_ParamError, ex);
            }
        }

        public double GetSlopeEfficiencyForSpecifiedPower(List<double> slopeEffs, List<double> powers, double power_mW)
        {
            try
            {

                int indexStart;
                int indexEnd;

                //找到比需求功率大的功率
                for (indexEnd = 0; indexEnd < slopeEffs.Count; indexEnd++)
                {
                    if (powers[indexEnd] >= power_mW)
                    {
                        break;
                    }
                }
                if (indexEnd == slopeEffs.Count) return 0;

                //找到比需求功率小的功率
                for (indexStart = indexEnd; indexStart > 0; indexStart--)
                {
                    if (powers[indexStart] < power_mW)
                    {
                        break;
                    }
                }
                if (indexStart == 0) return 0;


                //将该区间的数据进行直线拟合
                double[] powerArr = new double[indexEnd - indexStart + 1];  //功率组成的数组
                double[] slopeEffArr = new double[indexEnd - indexStart + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexStart; i <= indexEnd; i++)
                {
                    slopeEffArr[i - indexStart] = slopeEffs[i];
                    powerArr[i - indexStart] = powers[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:SE                      
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, slopeEffArr, 1);

                //计算出SE      
                double CalcSE = polyData.Coeffs[1] * power_mW + polyData.Coeffs[0];
                return CalcSE;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
    }
}