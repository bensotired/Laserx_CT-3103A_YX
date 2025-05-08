using LX_BurnInSolution.Utilities;
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
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    public class TestCalculator_FF2 : TestCalculatorBase
    {
        public TestCalculator_FF2() : base() { }
        public CalcRecipe_FF CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_FF);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_FF)testRecipe;
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {

            try
            {

            }
            catch (Exception ex)
            {
                this._core.Log_Global(ex.Message);
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_FF_PrintFFTestParams, ex);
            }










        }
    }
}
