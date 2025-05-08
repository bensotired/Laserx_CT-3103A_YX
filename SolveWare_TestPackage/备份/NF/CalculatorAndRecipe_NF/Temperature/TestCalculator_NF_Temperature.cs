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
    public class TestCalculator_NF_Temperature : TestCalculatorBase
    {
        public TestCalculator_NF_Temperature() : base() { }   
        public CalcRecipe_NF_Temperature CalcRecipe { get; private set; } 
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_NF_Temperature); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_NF_Temperature)testRecipe; }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}SPECT_Temperature_degC{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);

                dataSummary_1.Value = (rawData as RawData_NF).NF_Temperature_degC;              
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_NF_Temperature_ParamError, ex);
            }
        }      
    }
}