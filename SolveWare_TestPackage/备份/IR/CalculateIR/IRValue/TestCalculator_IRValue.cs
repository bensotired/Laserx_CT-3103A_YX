using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolveWare_TestPackage
{
    public class TestCalculator_IRValue : TestCalculatorBase
    {
        public TestCalculator_IRValue() : base() { }

        public CalcRecipe_IRValue CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_IRValue);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_IRValue)testRecipe;
        }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}IR{CalcRecipe.CalcData_PostFix}"); //{0}_@45C
                dataSummary_1.Value = double.NaN;
                const string IR_Index_Tag = "IRIndex";
                const string IR_Current_Tag = "IR_Current_A";
                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(IR_Index_Tag,IR_Current_Tag);
 
                if (dict[IR_Index_Tag]?.Count <= 0 || dict[IR_Current_Tag]?.Count <= 0 | dict[IR_Index_Tag]?.Count != dict[IR_Current_Tag]?.Count)
                {
                    throw new Exception($" xArray and yArray are of unequal size!");
                }
                dataSummary_1.Value = dict[IR_Current_Tag][0];
                summaryDataWithoutSpec.Add(dataSummary_1);

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_IR_ParamError, ex);
            }
        }
    }
}