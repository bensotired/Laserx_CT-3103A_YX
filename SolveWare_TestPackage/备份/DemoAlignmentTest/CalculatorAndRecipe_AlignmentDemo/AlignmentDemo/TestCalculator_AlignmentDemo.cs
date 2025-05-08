using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;

namespace SolveWare_TestPackage
{
    /// <summary>
    /// iop算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_AlignmentDemo : TestCalculatorBase
    {
        public TestCalculator_AlignmentDemo() : base() { }
        public CalcRecipe_AlignmentDemo CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_AlignmentDemo); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_AlignmentDemo)testRecipe; }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                //SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                //dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}AlignmentDemo{CalcRecipe.CalcData_PostFix}"); //{0}_@45C
                //dataSummary_1.Value = double.NaN;

                ////var localRawData = rawData as IRawDataMenuCollection;
                ////var dict = localRawData.GetDataDictByPropNames<double>("Wavelength", "Power_mW");
                ////获取传入参数
                //double current_mA = CalcRecipe.AnyParamYouWantToUseForCalculatingDueToAlignmentDemo;//传入的电流值
                ////计算结果
                //dataSummary_1.Value = current_mA * 100;

                //summaryDataWithoutSpec.Add(dataSummary_1);



                if(rawData is IRawDataMenuCollection)
                {
                    var localRawData = rawData as IRawDataMenuCollection;
                    var rawdata = localRawData.GetDataMenuCollection();
                    foreach (var item in rawdata)
                    {
                        var raw = item as IRawDataCollectionBase;
                        var data=raw.GetDataDictByPropNames<double>("Current_mA", "Power_mW");
                        var rawProps = raw.GetType().GetProperties();
                        var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                        var name = string.Empty;
                        foreach (var bp in bowProps)
                        {
                            if (bp.Name== "IO_Channel")
                            {
                                name = bp.GetValue(raw).ToString();
                            }
                            
                        }

                        if (data["Current_mA"]?.Count <= 0 | data["Current_mA"]?.Count != data["Power_mW"]?.Count)
                        {
                            throw new Exception($" xArray and yArray are of unequal size!");
                        }

                        SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                        dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Demo_{name}{CalcRecipe.CalcData_PostFix}"); //{0}_@45C
                        dataSummary_1.Value = 100;
                        summaryDataWithoutSpec.Add(dataSummary_1);
                    }


                }
                else
                {

                }



            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Iop_ParamError, ex);
            }
        }
    }
}