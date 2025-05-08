using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;

namespace SolveWare_TestPackage
{
    /// <summary>
    /// Iop算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_LIV_LiERR : TestCalculatorBase
    {
        public TestCalculator_LIV_LiERR() : base() { }

        public CalcRecipe_LIV_LiERR CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_LiERR);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_LiERR)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Lierr{CalcRecipe.CalcData_PostFix}");
                }

                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string VoltageTag = "Voltage_V";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, VoltageTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[VoltageTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0
                    |
                    dict[CurrentTag]?.Count != dict[VoltageTag]?.Count || dict[CurrentTag]?.Count != dict[PowerTag]?.Count || dict[PowerTag]?.Count != dict[VoltageTag]?.Count)
                {
                    throw new Exception($"Lierr: xArray and yArray are of unequal size!");
                }

                
                //找出拟合的线的power线计算出功率对应的电流
                int indexS;
                int indexE;
                
                for (indexE = 0; indexE < dict[PowerTag]?.Count; indexE++)
                {
                    if (dict[PowerTag][indexE] >= CalcRecipe.SE_PowerStart_mW)
                    {
                        break;
                    }
                }
                if (indexE == dict[PowerTag]?.Count)
                {
                    dataSummary_1.Value = 0;
                    return;
                }
                
                for (indexS = indexE; indexS > 0; indexS--)
                {
                    if (dict[PowerTag][indexS] < CalcRecipe.SE_PowerEnd_mW)
                    {
                        break;
                    }
                }
                if (indexS == 0)
                {
                    dataSummary_1.Value = 0;
                    return;
                }
                double[] powerArr = new double[indexE - indexS + 1]; 
                double[] currentArr = new double[indexE - indexS + 1]; 
                for (int i = indexS; i <= indexE; i++)
                {
                    currentArr[i - indexS] = dict[CurrentTag][i];
                    powerArr[i - indexS] = dict[PowerTag][i];
                }
                
                PolyFitData polyData = PolyFitMath.PolynomialFit(currentArr, powerArr, 1);
                double powerFit= polyData.Coeffs[1] * CalcRecipe.CurrentSet_mA + polyData.Coeffs[0];//获取拟合功率
                double powerActual =Interpolator.DoPiecewiseLinearInterpolation(CalcRecipe.CurrentSet_mA, dict[CurrentTag].ToArray(), dict[PowerTag].ToArray()); //获取实际的电流点最接近的功率
                dataSummary_1.Value = 100*(powerFit - powerActual);

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Lierr_ParamError, ex);
            }
        }
       
    }
}