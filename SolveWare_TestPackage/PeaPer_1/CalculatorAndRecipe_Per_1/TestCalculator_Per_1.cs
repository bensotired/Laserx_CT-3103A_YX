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
    public class TestCalculator_Per_1 : TestCalculatorBase
    {
        public TestCalculator_Per_1() : base() { }
        public CalcRecipe_Per_1 CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_Per_1);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_Per_1)testRecipe;
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {

            try
            {

            }
            catch (Exception ex)
            {
                this._core.Log_Global(ex.Message);
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_PER_PEA_ParamError, ex);
            }










        }
    }
}
