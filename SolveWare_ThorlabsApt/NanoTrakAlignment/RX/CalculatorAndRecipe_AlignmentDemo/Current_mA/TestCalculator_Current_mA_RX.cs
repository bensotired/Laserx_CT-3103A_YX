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
    /// iop算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_NanoTrakAlignment_RX_Current_mA : TestCalculatorBase
    {
        public TestCalculator_NanoTrakAlignment_RX_Current_mA() : base() { } 
        public CalcRecipe_NanoTrakAlignment_RX_AvgCurrent_mA CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_NanoTrakAlignment_RX_AvgCurrent_mA); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_NanoTrakAlignment_RX_AvgCurrent_mA)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}TestCalculator_Current_mA{CalcRecipe.CalcData_PostFix}"); //{0}_@45C
                dataSummary_1.Value = double.NaN;

                //获取传入参数
                double current_mA = (rawData as RawData_NanoTrakAlignment_RX).NanoTrak_RX_Current_mA;
                //计算结果
                dataSummary_1.Value = current_mA;

                summaryDataWithoutSpec.Add(dataSummary_1);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_NanoTrakAverage_RX_Current_mA_ParamError, ex);
            }
        }
    }
}