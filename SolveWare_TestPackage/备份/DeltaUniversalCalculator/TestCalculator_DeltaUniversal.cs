using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestPackage
{
    /// <summary>
    /// 数据对比
    /// </summary>
    public class TestCalculator_DeltaUniversal : TestCalculatorBase
    {
        public TestCalculator_DeltaUniversal() : base() { }
        public CalcRecipe_DeltaUniversal CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_DeltaUniversal); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_DeltaUniversal)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                //当前测试结果中的值
                var realtimeDeltaSummaryName = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.ParamName}{CalcRecipe.CalcData_PostFix}");
                double realtimeDeltaSummary = 0;
                foreach (var item in summaryDataWithoutSpec)
                {
                    if (item.Name == realtimeDeltaSummaryName)
                    {
                        realtimeDeltaSummary = Convert.ToDouble(item.Value);
                        break;
                    }
                }
                //从数据库中获取的过往测试值作差
                //var difference = this._core.DataBaseContrast(this._core.DataBaseTableName, realtimeDeltaSummaryName, realtimeDeltaSummary);
                var difference = this._core.DataBaseContrast(this._core.DataBaseConnectionString, this._core.DataBaseTableName, realtimeDeltaSummaryName, realtimeDeltaSummary);
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}delta{CalcRecipe.ParamName}{CalcRecipe.CalcData_PostFix}");
                dataSummary_1.Value = difference;
                //将结果添加到结果列表
                summaryDataWithoutSpec.Add(dataSummary_1);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Iop_ParamError, ex);
            }
        }
    }
}