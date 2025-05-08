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
    /// NullA1_Deg算子
    /// 
    /// </summary>
    public class TestCalculator_LIV_PulsedMode : TestCalculatorBase
    {
        public TestCalculator_LIV_PulsedMode() : base() { }   
        public CalcRecipe_LIV_PulsedMode CalcRecipe { get; private set; } 
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_LIV_PulsedMode); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_LIV_PulsedMode)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();

                if (CalcRecipe.IsForceRename == true)
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                }
                else
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}LIV_PulsedMode{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);

                dataSummary_1.Value = (rawData as RawData_LIV).PulsedMode;               
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_PulsedMode_ParamError, ex); 
            }
        }      
    }
}