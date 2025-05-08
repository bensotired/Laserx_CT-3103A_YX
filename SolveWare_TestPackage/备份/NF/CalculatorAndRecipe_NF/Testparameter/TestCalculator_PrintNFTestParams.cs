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
    public class TestCalculator_PrintNFTestParams : TestCalculatorBase
    {
        public TestCalculator_PrintNFTestParams() : base() { }   
        public CalcRecipe_PrintNFTestParams CalcRecipe { get; private set; }  
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_PrintNFTestParams); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_PrintNFTestParams)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                var sourceRawData_NF = (rawData as RawData_NF); ;
                var prtEleProps = PropHelper.GetAttributeProps<RawDataPrintableElementAttribute>(typeof(RawData_NF).GetProperties());


                foreach (var prop in prtEleProps)
                {
                    SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                    dataSummary_1.Value = 0.0;
                    summaryDataWithoutSpec.Add(dataSummary_1);
                    try
                    {
                        dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{prop.Name}{CalcRecipe.CalcData_PostFix}");
                        dataSummary_1.Value = prop.GetValue(sourceRawData_NF);
                    }
                    catch (Exception innerEx)
                    {
                        this._core.ReportException($"{this.Name} print value [{prop.Name}] exception: - {innerEx.Message}", ErrorCodes.Calc_NF_Testparameter_ParamError, innerEx);
                    }
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_NF_Testparameter_ParamError, ex);
            }
        }

      
    }
}