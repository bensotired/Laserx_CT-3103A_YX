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
    /// Rs_2Point算子  根据CT1100A
    /// 找出目标电流点下对应的电阻
    /// </summary>
    public class TestCalculator_LIV_Rd : TestCalculatorBase
    {
        public TestCalculator_LIV_Rd() : base() { }

        public CalcRecipe_LIV_Rd CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Rd);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Rd)(testRecipe);
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Rd{CalcRecipe.CalcData_PostFix}");
                }

                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);

                const string CurrentTag = "Current_mA";
                const string VoltageTag = "Voltage_V";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, VoltageTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[VoltageTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[VoltageTag]?.Count)
                {

                    throw new Exception($"Rd: xArray and yArray are of unequal size!");
                }
                Dictionary<double, double> dvdi = new Dictionary<double, double>();
                List<double> sorted_I = new List<double>();
                List<double> sorted_RS = new List<double>();
                var len = dict[CurrentTag].Count;
                const int sampleRange = 2;
                const int sampleSeed = 3;

                for (int i = sampleRange; i < len - sampleRange; i++)
                {
                    var currs_21 = dict[CurrentTag].GetRange(i - sampleRange, sampleSeed);
                    var volts_21 = dict[VoltageTag].GetRange(i - sampleRange, sampleSeed);
                    var polyFitVI = PolyFitMath.PolynomialFit(currs_21.ToArray(), volts_21.ToArray(), 1);

                    dvdi.Add(dict[CurrentTag][i], polyFitVI.Coeffs[1]);

                }
                double[] currSECalCurrent = dvdi.Keys.ToArray();
                double[] dvdiFitRsCalCurrent = dvdi.Values.ToArray();
                var rsFittingCoeff = PolyFitMath.PolynomialFit(currSECalCurrent, dvdiFitRsCalCurrent, 4);

                for (int i = 0; i < dict[CurrentTag].Count; i++)
                {
                    var current = dict[CurrentTag][i];
                    if (currSECalCurrent.Contains(current))
                    {

                        var se = rsFittingCoeff.Coeffs[4] * Math.Pow(current, 4) +
                                 rsFittingCoeff.Coeffs[3] * Math.Pow(current, 3) +
                                 rsFittingCoeff.Coeffs[2] * Math.Pow(current, 2) +
                                 rsFittingCoeff.Coeffs[1] * Math.Pow(current, 1) +
                                 rsFittingCoeff.Coeffs[0];

                        sorted_I.Add(current);
                        sorted_RS.Add(se);
                    }
                }
                dataSummary_1.Value = GetResistanceForSpecifiedCurrent(sorted_RS, sorted_I, CalcRecipe.Current_mA) * 1000;
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Rd_ParamError, ex);
            }
        }
       
        public double GetResistanceForSpecifiedCurrent(List<double> resList, List<double> currents, double current_mA)
        {
            try
            {

                int indexStart;
                int indexEnd;

                //找到比需求功率大的功率
                for (indexEnd = 0; indexEnd < resList.Count; indexEnd++)
                {
                    if (currents[indexEnd] >= current_mA)
                    {
                        break;
                    }
                }
                if (indexEnd == resList.Count) return 0;

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
                double[] resArr = new double[indexEnd - indexStart + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexStart; i <= indexEnd; i++)
                {
                    resArr[i - indexStart] = resList[i];
                    currentArr[i - indexStart] = currents[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:SE                      
                PolyFitData polyData = PolyFitMath.PolynomialFit(currentArr, resArr, 1);

                //计算出SE      
                double CalcRes = polyData.Coeffs[1] * current_mA + polyData.Coeffs[0];
                return CalcRes;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
    }
}