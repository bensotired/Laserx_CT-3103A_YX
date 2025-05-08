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
    /// iop算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_Sample : TestCalculatorBase
    {
        public TestCalculator_Sample() : base() { }
        public CalcRecipe_Sample CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_Sample); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_Sample)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Sample{CalcRecipe.CalcData_PostFix}"); //{0}_@45C
                dataSummary_1.Value = double.NaN;

                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($" xArray and yArray are of unequal size!");
                }
                //根据1100A的接近原则
                double current_mA = CalcRecipe.Current_mA;//传入的电流值
                dataSummary_1.Value = current_mA * 100;
                summaryDataWithoutSpec.Add(dataSummary_1);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Iop_ParamError, ex);
            }
        }
    }
}