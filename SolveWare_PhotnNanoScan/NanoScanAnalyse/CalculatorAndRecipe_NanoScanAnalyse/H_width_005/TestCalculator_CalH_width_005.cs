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
    public class TestCalculator_CalH_width_005 : TestCalculatorBase
    {
        public TestCalculator_CalH_width_005() : base() { } 
        public CalcRecipe_NanoScanAnalyse_CalH_width_005 CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_NanoScanAnalyse_CalH_width_005); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_NanoScanAnalyse_CalH_width_005)testRecipe; }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}CalH_width_005{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);

                const string X_Position_1st = "X_Position_1st";

                const string X_Amplitude_1st = "X_Amplitude_1st";
                const string Y_Amplitude_1st = "Y_Amplitude_1st";
                const string X_Amplitude_2nd = "X_Amplitude_2nd";
                const string Y_Amplitude_2nd = "Y_Amplitude_2nd";               
                
                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(X_Position_1st, X_Amplitude_1st, Y_Amplitude_1st, X_Amplitude_2nd, Y_Amplitude_2nd);

                if (dict[X_Position_1st]?.Count <= 0 || dict[X_Amplitude_1st]?.Count <= 0 || dict[Y_Amplitude_1st]?.Count <= 0 || dict[X_Amplitude_2nd]?.Count <= 0 || dict[Y_Amplitude_2nd]?.Count <= 0 |
                    (dict[X_Amplitude_1st]?.Count != dict[X_Amplitude_1st]?.Count|| dict[X_Amplitude_1st]?.Count != dict[Y_Amplitude_1st]?.Count 
                    || dict[Y_Amplitude_1st]?.Count != dict[X_Amplitude_2nd]?.Count || dict[X_Amplitude_2nd]?.Count != dict[Y_Amplitude_2nd]?.Count))
                {
                    throw new Exception($"Iop: xArray and yArray are of unequal size!");
                }
                Double MoveDistance_mm = (rawData as RawData_NanoScanAnalyse).MoveDistance_mm;
                double resultCurrent = AllNanoScanAnalyse.CalculatoWidth(MoveDistance_mm, dict[X_Position_1st], dict[X_Amplitude_1st],dict[Y_Amplitude_1st], 
                                       dict[X_Amplitude_2nd], dict[Y_Amplitude_2nd]).H_width_5;
                dataSummary_1.Value = resultCurrent; 
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Iop_ParamError, ex);
            }
        }      
    }
}