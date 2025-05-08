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
    /// NullA1_Deg算子
    /// 
    /// </summary>
    public class TestCalculator_PrintFFTestParams : TestCalculatorBase
    {
        public TestCalculator_PrintFFTestParams() : base() { }  
        public CalcRecipe_PrintFFTestParams CalcRecipe { get; private set; } 
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_PrintFFTestParams); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_PrintFFTestParams)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                var sourceRawData_FF = (rawData as RawData_NanoScanAnalyse); ;
                var prtEleProps = PropHelper.GetAttributeProps<RawDataPrintableElementAttribute>(typeof(RawData_NanoScanAnalyse).GetProperties());


                foreach (var prop in prtEleProps)
                {
                    SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                    dataSummary_1.Value = 0.0;
                    summaryDataWithoutSpec.Add(dataSummary_1);
                    try
                    {
                        dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{prop.Name}{CalcRecipe.CalcData_PostFix}");
                        dataSummary_1.Value = prop.GetValue(sourceRawData_FF);
                    }
                    catch (Exception innerEx)
                    {
                        this._core.ReportException($"{this.Name} print value [{prop.Name}] exception: - {innerEx.Message}", ErrorCodes.Calc_FF_PrintFFTestParams, innerEx);
                    }
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_FF_PrintFFTestParams, ex);
            }
        }

      
    }
}