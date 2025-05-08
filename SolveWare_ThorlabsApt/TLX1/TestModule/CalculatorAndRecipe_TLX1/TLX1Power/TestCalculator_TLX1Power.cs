using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.Attributes;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// TLX1Wavelength算子
    /// 
    /// </summary>
    public class TestCalculator_TLX1Power : TestCalculatorBase
    {
        public TestCalculator_TLX1Power() : base() { }    
        public CalcRecipe_TLX1Power CalcRecipe { get; private set; }  
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_TLX1Power); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_TLX1Power)testRecipe; }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}TLX1Wavelength{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);

                dataSummary_1.Value = (rawData as RawData_TLX1).TLX1Power;
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_TLX1_TLX1Power_ParamError, ex);
            }
        }

      
    }
}