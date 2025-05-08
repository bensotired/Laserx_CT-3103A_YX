using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    public class TestCalculator_MZMRdiff : TestCalculatorBase
    {
        public TestCalculator_MZMRdiff() : base() { } 
        public CalcRecipe_MZMRdiff CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_MZMRdiff);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_MZMRdiff)testRecipe;
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {

            try
            {
                if (rawData is IRawDataMenuCollection)
                {
                    var localRawData = rawData as IRawDataMenuCollection;
                    var rawdata = localRawData.GetDataMenuCollection();
                    double Resistance_BIAS1 = 0;
                    double Resistance_BIAS2 = 0;
                    bool find_BIAS1 = false;
                    bool find_BIAS2 = false;
                    foreach (var item in rawdata)
                    {
                        const string CurrentTag = "Current_mA";
                        const string Voltage_V = "Voltage_V";
                        const string SectionTag = "Section";

                        var raw = item as IRawDataCollectionBase;
                        var data = raw.GetDataDictByPropNames<double>(CurrentTag, Voltage_V);
                        var dataSection = raw.GetDataDictByPropNames<string>(SectionTag);
                        var rawProps = raw.GetType().GetProperties();
                        var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);

                        if (data[CurrentTag]?.Count <= 0 | data[CurrentTag]?.Count != data[Voltage_V]?.Count)
                        {
                            throw new Exception($" xArray and yArray are of unequal size!");
                        }

                        string section = dataSection[SectionTag].First();
                        SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();

                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}{section}_Rs_{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}";

                        dataSummary_1.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_1);

                        List<double> Currentlist = new List<double>();
                        List<double> Voltagelist = new List<double>();

                        Section_Volt Section = (Section_Volt)Enum.Parse(typeof(Section_Volt), section);
                        switch (Section)
                        {
                            case Section_Volt.BIAS1:
                                {
                                    double current1 = Interpolator.DoPiecewiseLinearInterpolation(this.CalcRecipe.FirstVoltageSet, 
                                                                                                  data[Voltage_V].ToArray(), data[CurrentTag].ToArray());
                                    double current2 = Interpolator.DoPiecewiseLinearInterpolation(this.CalcRecipe.SecondVoltageSet,
                                                                                                  data[Voltage_V].ToArray(), data[CurrentTag].ToArray());

                                    Resistance_BIAS1 = (this.CalcRecipe.SecondVoltageSet - this.CalcRecipe.FirstVoltageSet) / (current2 - current1) * 1000;
                                    find_BIAS1 = true;
                                }
                                break;
                            case Section_Volt.BIAS2:
                                {
                                    double current1 = Interpolator.DoPiecewiseLinearInterpolation(this.CalcRecipe.FirstVoltageSet,
                                                                                                  data[Voltage_V].ToArray(), data[CurrentTag].ToArray());
                                    double current2 = Interpolator.DoPiecewiseLinearInterpolation(this.CalcRecipe.SecondVoltageSet,
                                                                                                  data[Voltage_V].ToArray(), data[CurrentTag].ToArray());

                                    Resistance_BIAS2 = (this.CalcRecipe.SecondVoltageSet - this.CalcRecipe.FirstVoltageSet) / (current2 - current1) * 1000;
                                    find_BIAS2 = true;
                                }
                                break;
                        }
                    }

                    if (find_BIAS1 == true && find_BIAS2 == true)
                    {
                        SummaryDatumItemBase dataSummary_0 = new SummaryDatumItemBase();

                        dataSummary_0.Name = $"{CalcRecipe.CalcData_PreFix}MZMRdiff{CalcRecipe.CalcData_PostFix}";

                        dataSummary_0.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_0);
                        if (double.IsInfinity(Resistance_BIAS1) || double.IsInfinity(Resistance_BIAS2))
                        {
                            dataSummary_0.Value = 9999;
                        }
                        else
                        {
                            double RS_Diff = Resistance_BIAS1 - Resistance_BIAS2;
                            dataSummary_0.Value = RS_Diff;
                        }
                        SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();

                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}Resistance1{CalcRecipe.CalcData_PostFix}";

                        dataSummary_1.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_1);
                        if (double.IsInfinity(Resistance_BIAS1))
                        {
                            dataSummary_1.Value = 9999;
                        }
                        else
                        {
                            dataSummary_1.Value = Resistance_BIAS1;
                        }

                        SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();

                        dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}Resistance2{CalcRecipe.CalcData_PostFix}";

                        dataSummary_2.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_2);
                        if ( double.IsInfinity(Resistance_BIAS2))
                        {
                            dataSummary_2.Value = 9999;
                        }
                        else
                        {
                            dataSummary_2.Value = Resistance_BIAS2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._core.Log_Global(ex.Message);
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.CreateCalculatorFailed, ex);
            }
        }
        public double CalculateLaserResistance()
        {
            try
            {
                double rs = 0;


                return rs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
