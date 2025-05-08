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
    public class TestCalculator_NanoTrakAlignment_RX_Power_mW : TestCalculatorBase
    {
        public TestCalculator_NanoTrakAlignment_RX_Power_mW() : base() { } 
        public CalcRecipe_NanoTrakAlignment_RX_AveragPower_mW CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_NanoTrakAlignment_RX_AveragPower_mW); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_NanoTrakAlignment_RX_AveragPower_mW)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}LX_AveragPower_mW{CalcRecipe.CalcData_PostFix}"); //{0}_@45C
                dataSummary_1.Value = double.NaN;

                //获取传入参数
                double Power_mW = (rawData as RawData_NanoTrakAlignment_RX).NanoTrak_RX_Power_mW; 
                //计算结果
                dataSummary_1.Value = Power_mW; 

                summaryDataWithoutSpec.Add(dataSummary_1);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_NanoTrakAlignment_RX_Power_mW_ParamError, ex);
            }
        }
    }
}