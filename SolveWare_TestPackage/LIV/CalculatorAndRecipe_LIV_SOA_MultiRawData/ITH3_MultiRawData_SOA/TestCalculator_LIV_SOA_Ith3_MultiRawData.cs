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
    public class TestCalculator_LIV_SOA_Ith3_MultiRawData : TestCalculatorBase
    {
        public TestCalculator_LIV_SOA_Ith3_MultiRawData() : base() { }
        public CalcRecipe_LIV_SOA_Ith3_MultiRawData CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_LIV_SOA_Ith3_MultiRawData); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_LIV_SOA_Ith3_MultiRawData)(testRecipe); }
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
                        const string CurrentTag = "G_Current_mA";
                        const string PowerTag_S1 = "S1_Current_mA";
                        const string PowerTag_S2 = "S2_Current_mA";

                        var localRawData = item as IRawDataCollectionBase;
                        var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag_S1, PowerTag_S2);
                        if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag_S1]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag_S1]?.Count)
                        {
                            throw new Exception($"Ith1: xArray and yArray are of unequal size!");
                        }
                        if (CalcRecipe.ThresholdCurrentUpperLimit_mA <= CalcRecipe.ThresholdCurrentLowerLimit_mA)
                        {
                            throw new Exception($"Ith3: ThresholdCurrentUpperLimit_mA is smaller than ThresholdCurrentLowerLimit_mA!");
                        }

                        #region SOA1
                        SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}SOA1_Ith3{CalcRecipe.CalcData_PostFix}";
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

                        for (int i = 0; i < dict[PowerTag_S1].Count; i++)
                        {
                            dict[PowerTag_S1][i] = dict[PowerTag_S1][i] * (-1);
                        }


                        List<double> ldCurrs = new List<double>();
                        List<double> ldPows = new List<double>();

                        List<double> ldCurrs_org = dict[CurrentTag];
                        List<double> ldPows_org = dict[PowerTag_S1];
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
                        #endregion

                        #region SOA2
                        SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();
                        dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}SOA2_Ith3{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}{sectionName}_{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}";
                        //}
                        //else
                        //{
                        //    dataSummary_1.Name = string.Format(item.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{sectionName}Ith3{CalcRecipe.CalcData_PostFix}");
                        //}
                        dataSummary_2.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_2);

                        for (int i = 0; i < dict[PowerTag_S2].Count; i++)
                        {
                            dict[PowerTag_S2][i] = dict[PowerTag_S2][i] * (-1);
                        }


                        List<double> ldCurrs2 = new List<double>();
                        List<double> ldPows2 = new List<double>();

                        List<double> ldCurrs_org2 = dict[CurrentTag];
                        List<double> ldPows_org2 = dict[PowerTag_S2];
                        int dataCount2 = localRawData.Count;
                        for (int i = 0; i < dataCount2; i++)
                        {
                            if (ldCurrs_org2[i] >= CalcRecipe.ThresholdCurrentLowerLimit_mA &&
                               ldCurrs_org2[i] <= CalcRecipe.ThresholdCurrentUpperLimit_mA)
                            {
                                ldCurrs2.Add(ldCurrs_org2[i]);
                                ldPows2.Add(ldPows_org2[i]);
                            }
                        }

                        var firstDerivateArr2 = ArrayMath.CalculateFirstDerivate(ldCurrs2.ToArray(), ldPows2.ToArray());
                        var secondDerivateArr2 = ArrayMath.CalculateFirstDerivate(ldCurrs2.ToArray(), firstDerivateArr2);
                        int maxIndex2 = 0;
                        int minIndex2 = 0;
                        ArrayMath.GetMaxAndMinIndex(secondDerivateArr2, out maxIndex2, out minIndex2);
                        dataSummary_2.Value = ldCurrs2[maxIndex2];
                        #endregion
                    }
                }
                else
                {
                    const string CurrentTag = "G_Current_mA";
                    const string PowerTag_S1 = "S1_Current_mA";
                    const string PowerTag_S2 = "S2_Current_mA";

                    var localRawData = rawData as IRawDataCollectionBase;
                    var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag_S1, PowerTag_S2);
                    if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag_S1]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag_S1]?.Count)
                    {
                        throw new Exception($"Ith1: xArray and yArray are of unequal size!");
                    }
                    if (CalcRecipe.ThresholdCurrentUpperLimit_mA <= CalcRecipe.ThresholdCurrentLowerLimit_mA)
                    {
                        throw new Exception($"Ith3: ThresholdCurrentUpperLimit_mA is smaller than ThresholdCurrentLowerLimit_mA!");
                    }

                    #region SOA1
                    SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}SOA1_Ith3{CalcRecipe.CalcData_PostFix}";
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

                    for (int i = 0; i < dict[PowerTag_S1].Count; i++)
                    {
                        dict[PowerTag_S1][i] = dict[PowerTag_S1][i] * (-1);
                    }


                    List<double> ldCurrs = new List<double>();
                    List<double> ldPows = new List<double>();

                    List<double> ldCurrs_org = dict[CurrentTag];
                    List<double> ldPows_org = dict[PowerTag_S1];
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
                    #endregion

                    #region SOA2
                    SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();
                    dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}SOA2_Ith3{CalcRecipe.CalcData_PostFix}";
                    //if (CalcRecipe.IsForceRename == true)
                    //{
                    //    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}{sectionName}_{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}";
                    //}
                    //else
                    //{
                    //    dataSummary_1.Name = string.Format(item.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{sectionName}Ith3{CalcRecipe.CalcData_PostFix}");
                    //}
                    dataSummary_2.Value = double.NaN;
                    summaryDataWithoutSpec.Add(dataSummary_2);

                    for (int i = 0; i < dict[PowerTag_S2].Count; i++)
                    {
                        dict[PowerTag_S2][i] = dict[PowerTag_S2][i] * (-1);
                    }


                    List<double> ldCurrs2 = new List<double>();
                    List<double> ldPows2 = new List<double>();

                    List<double> ldCurrs_org2 = dict[CurrentTag];
                    List<double> ldPows_org2 = dict[PowerTag_S2];
                    int dataCount2 = localRawData.Count;
                    for (int i = 0; i < dataCount2; i++)
                    {
                        if (ldCurrs_org2[i] >= CalcRecipe.ThresholdCurrentLowerLimit_mA &&
                           ldCurrs_org2[i] <= CalcRecipe.ThresholdCurrentUpperLimit_mA)
                        {
                            ldCurrs2.Add(ldCurrs_org2[i]);
                            ldPows2.Add(ldPows_org2[i]);
                        }
                    }

                    var firstDerivateArr2 = ArrayMath.CalculateFirstDerivate(ldCurrs2.ToArray(), ldPows2.ToArray());
                    var secondDerivateArr2 = ArrayMath.CalculateFirstDerivate(ldCurrs2.ToArray(), firstDerivateArr2);
                    int maxIndex2 = 0;
                    int minIndex2 = 0;
                    ArrayMath.GetMaxAndMinIndex(secondDerivateArr2, out maxIndex2, out minIndex2);
                    dataSummary_2.Value = ldCurrs2[maxIndex2];
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Ith3_ParamError, ex);
            }
        }
    }
}