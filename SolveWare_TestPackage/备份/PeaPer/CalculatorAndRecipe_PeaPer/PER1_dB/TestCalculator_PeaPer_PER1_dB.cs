using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestPackage.PeaPer;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// PER1_dB算子
    /// 
    /// </summary>
    public class TestCalculator_PeaPer_PER1_dB : TestCalculatorBase
    {
        public TestCalculator_PeaPer_PER1_dB() : base() { } 
        public CalcRecipe_PeaPer_PER1_dB CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_PeaPer_PER1_dB); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_PeaPer_PER1_dB)testRecipe; }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}PER1_dB{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";
                const string Angle = "Angle_deg"; 


                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag,Angle);

                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 || dict[Angle]?.Count <= 0 | 
                    (dict[Angle]?.Count != dict[CurrentTag]?.Count|| dict[Angle]?.Count != dict[PowerTag]?.Count))
                {
                    throw new Exception($"Iop: xArray and yArray are of unequal size!");
                }
                //double power_mW = this.CalcRecipe.Power_mW; //可以从Recipe里面选的，不选了，直接用第一个方法
                //bool UsePDCurrentCalculatePER = this.CalcRecipe.UsePDCurrentCalculatePER;
                bool Curve_Fit = true;
                bool UsePDCurrentCalculatePER = true;
                double resultCurrent = PeaPerCalcuator.CalculatotAllData_ReturnSingle(Curve_Fit, UsePDCurrentCalculatePER, dict[CurrentTag], dict[PowerTag], dict[Angle]).PER1_dB;
                dataSummary_1.Value = resultCurrent; 
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_PER_PER1_dB_ParamError, ex);
            }
        }    
    }
}