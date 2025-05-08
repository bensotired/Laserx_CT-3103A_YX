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
    [Serializable]
    /// <summary>
    /// Ith3算子 拟合 根据CT3102
    /// 找出目标电流点下对应的器件输出功率
    /// </summary>
    public class TestCalculator_LIV_Ith3 : TestCalculatorBase
    {
        public TestCalculator_LIV_Ith3() : base() { }
        public CalcRecipe_LIV_Ith3 CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_LIV_Ith3); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_LIV_Ith3)(testRecipe); }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith3{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";
                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);
                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"Ith3: xArray and yArray are of unequal size!");
                }
                if (CalcRecipe.ThresholdCurrentUpperLimit_mA <= CalcRecipe.ThresholdCurrentLowerLimit_mA)
                {
                    throw new Exception($"Ith3: ThresholdCurrentUpperLimit_mA is smaller than ThresholdCurrentLowerLimit_mA!");
                }


                List<double> ldCurrs = new List<double>();
                List<double> ldPows = new List<double>();
                
                List<double> ldCurrs_org = dict[CurrentTag];
                List<double> ldPows_org = dict[PowerTag];
                int dataCount = localRawData.Count;
                for(int i = 0; i< dataCount; i++)
                {
                    if(ldCurrs_org[i] >=  CalcRecipe.ThresholdCurrentLowerLimit_mA &&
                       ldCurrs_org[i] <=  CalcRecipe.ThresholdCurrentUpperLimit_mA)
                    {
                        ldCurrs.Add(ldCurrs_org[i]);
                        ldPows.Add(ldPows_org[i]);
                    }
                }
           
                var firstDerivateArr = ArrayMath.CalculateFirstDerivate(ldCurrs.ToArray(), ldPows.ToArray());
                var secondDerivateArr = ArrayMath.CalculateFirstDerivate(ldCurrs.ToArray(), firstDerivateArr);
                int maxIndex = 0;
                int minIndex = 0;
                ArrayMath.GetMaxAndMinIndex(secondDerivateArr, out maxIndex, out minIndex);
                dataSummary_1.Value = ldCurrs[maxIndex];
            
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Ith3_ParamError, ex);
            }
        }
    }
}