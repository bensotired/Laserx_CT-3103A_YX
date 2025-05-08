using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// Ith3算子 拟合 根据CT3102
    /// 找出目标电流点下对应的器件输出功率
    /// </summary>
    public class TestCalculator_LIV_Tap_PD_Ith3_MultiRawData : TestCalculatorBase
    {
        public TestCalculator_LIV_Tap_PD_Ith3_MultiRawData() : base() { }
        public CalcRecipe_LIV_Tap_PD_Ith3_MultiRawData CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_LIV_Tap_PD_Ith3_MultiRawData); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_LIV_Tap_PD_Ith3_MultiRawData)(testRecipe); }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                if (rawData is IRawDataMenuCollection)
                {
                    var multiRawData = rawData as IRawDataMenuCollection;
                    var rawdata = multiRawData.GetDataMenuCollection();
                    foreach (var item in rawdata)
                    {
                        const string CurrentTag = "Current_mA";
                        const string PowerTag = "Power_mW";
                        const string SectionTag = "Section";
                        var localRawData = item as IRawDataCollectionBase;
                        var dict = localRawData.GetDataDictByPropNames<object>(CurrentTag, PowerTag, SectionTag);
                        var sectionName = dict[SectionTag].First().ToString();
                        if(sectionName != "Gain")
                        {
                            continue;
                        }

                        SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}Tap_PD_Ith3{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}{sectionName}_{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}";
                        //}
                        //else
                        //{
                        //    dataSummary_1.Name = string.Format(item.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{sectionName}Ith3{CalcRecipe.CalcData_PostFix}");
                        //}
                        dataSummary_1.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_1);

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

                        List<double> ldCurrs_org = dict[CurrentTag].ConvertAll<double>(new Converter<object, double>((obj) => { return Convert.ToDouble(obj); }));
                        List<double> ldPows_org = dict[PowerTag].ConvertAll<double>(new Converter<object, double>((obj) => { return Convert.ToDouble(obj); }));
                        int dataCount = localRawData.Count;
                        for (int i = 0; i < dataCount; i++)
                        {
                            if (ldCurrs_org[i] >= CalcRecipe.ThresholdCurrentLowerLimit_mA &&
                               ldCurrs_org[i] <= CalcRecipe.ThresholdCurrentUpperLimit_mA)
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
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Ith3_ParamError, ex);
            }
        }
    }
}