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
    /// SE_mWpermA算子 CT1100A
    /// 找出目标电流点下对应的器件输出功率
    /// </summary>
    public class TestCalculator_LIV_SE_mWpermA : TestCalculatorBase
    {
        public TestCalculator_LIV_SE_mWpermA() : base() { }

        public CalcRecipe_LIV_SE_mWpermA CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_SE_mWpermA);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_SE_mWpermA)(testRecipe);
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}SE_mWpermA{CalcRecipe.CalcData_PostFix}");
                }
              
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0  | dict[CurrentTag]?.Count != dict[PowerTag]?.Count )
                {
                    throw new Exception($"SE_mWpermA: xArray and yArray are of unequal size!");
                }

                double[] powerArr = new double[dict[CurrentTag].Count];
                double[] currentArr = new double[dict[CurrentTag].Count];
                for (int i = 0; i < dict[CurrentTag].Count; i++)
                {
                    powerArr[i] = dict[PowerTag][i];
                    currentArr[i] = dict[CurrentTag][i];
                }
                
                double SlopeEfficiencyStartI_mA = this.CalcRecipe.StartCurrent_mA;
                double SlopeEfficiencyStop_mA = this.CalcRecipe.StopCurrent_mA;

                double powerAtStart_mW = Interpolator.DoPiecewiseLinearInterpolation(SlopeEfficiencyStartI_mA, currentArr, powerArr);
                double powerAtStop_mW = Interpolator.DoPiecewiseLinearInterpolation(SlopeEfficiencyStop_mA, currentArr, powerArr);

                dataSummary_1.Value = (powerAtStop_mW - powerAtStart_mW) / (SlopeEfficiencyStop_mA - SlopeEfficiencyStartI_mA);
        
            }                                                 
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_SE_mWpermA_ParamError, ex);
            }
        }
    }
}