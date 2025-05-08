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
    /// NullA1_Deg算子
    /// 
    /// </summary>
    public class TestCalculator_FF_Temperature : TestCalculatorBase
    {
        public TestCalculator_FF_Temperature() : base() { }  
        public CalcRecipe_NanoScanAnalyse_Temperature CalcRecipe { get; private set; } 
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_NanoScanAnalyse_Temperature); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_NanoScanAnalyse_Temperature)testRecipe; }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}FF_Temperature_degC{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);

                dataSummary_1.Value = (rawData as RawData_NanoScanAnalyse).FF_Temperature_degC;              
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_FF_Temperature_ParamError, ex);
            }
        }      
    }
}