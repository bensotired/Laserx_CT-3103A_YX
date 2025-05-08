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
    public class TestCalculator_Print_TLX1TestParams : TestCalculatorBase
    {
        public TestCalculator_Print_TLX1TestParams() : base() { }   
        public CalcRecipe_Print_TLX1TestParams CalcRecipe { get; private set; }  
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_Print_TLX1TestParams); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_Print_TLX1TestParams)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                var sourceRawData_TLX1 = (rawData as RawData_TLX1); 
                var prtEleProps = PropHelper.GetAttributeProps<RawDataPrintableElementAttribute>(typeof(RawData_TLX1).GetProperties());


                foreach (var prop in prtEleProps)
                {
                    SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                    dataSummary_1.Value = 0.0;
                    summaryDataWithoutSpec.Add(dataSummary_1);
                    try
                    {
                        dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{prop.Name}{CalcRecipe.CalcData_PostFix}");
                        dataSummary_1.Value = prop.GetValue(sourceRawData_TLX1);
                    }
                    catch (Exception innerEx)
                    {
                        this._core.ReportException($"{this.Name} print value [{prop.Name}] exception: - {innerEx.Message}", ErrorCodes.Calc_LIV_PrintLIVTestParams, innerEx);
                    }
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_TLX1_Testparameter_ParamError, ex);
            }
        }

      
    }
}