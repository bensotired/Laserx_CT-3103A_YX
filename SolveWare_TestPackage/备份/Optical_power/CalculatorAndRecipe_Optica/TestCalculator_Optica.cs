using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    public class TestCalculator_Optica : TestCalculatorBase
    {
        public TestCalculator_Optica() : base() { }
        public CalcRecipe_Optical CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_Optical);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_Optical)testRecipe;
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}AlignmentDemo{CalcRecipe.CalcData_PostFix}"); //{0}_@45C
                dataSummary_1.Value = double.NaN;

                //获取传入参数
                double current_mA = 5;// CalcRecipe.AnyParamYouWantToUseForCalculatingDueToAlignmentDemo;//传入的电流值
                //计算结果
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
