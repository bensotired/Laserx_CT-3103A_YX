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
    public class TestCalculator_H_width_13p5_Deg : TestCalculatorBase
    {
        public TestCalculator_H_width_13p5_Deg() : base() { } 
        public CalcRecipe_NanoScanAnalyse_CalH_width_13p5 CalcRecipe { get; private set; } 
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_NanoScanAnalyse_CalH_width_13p5); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_NanoScanAnalyse_CalH_width_13p5)testRecipe; }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}H_width_13p5_Deg{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);              

                Double MoveDistance_mm = (rawData as RawData_NanoScanAnalyse).MoveDistance_mm;
                double  H_width = ((rawData as RawData_NanoScanAnalyse).BeamWidth_13p5_X_2nd - (rawData as RawData_NanoScanAnalyse).BeamWidth_13p5_X_1st )/ 2 /( MoveDistance_mm*1000);
                double H_width_13p5 = Math.Atan(H_width)*180/Math.PI * 2;
                dataSummary_1.Value = H_width_13p5; 
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_FF_H_width_13p5_ParamError, ex);
            }
        }      
    }
}