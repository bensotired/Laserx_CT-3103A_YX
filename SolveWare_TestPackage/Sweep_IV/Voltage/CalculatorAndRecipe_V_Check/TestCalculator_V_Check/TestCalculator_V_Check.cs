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
    public class TestCalculator_V_Check : TestCalculatorBase
    {
        public TestCalculator_V_Check() : base() { }
        public CalcRecipe_V_Check CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_V_Check);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_V_Check)testRecipe;
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {

            try
            {
                if (rawData is IRawDataMenuCollection)
                {
                    var localRawData = rawData as IRawDataMenuCollection;
                    var rawdata = localRawData.GetDataMenuCollection();
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
                        int startVoltage = 0;
                        int endVoltage = 2;
                        switch (Section)
                        {
                            case Section_Volt.GAIN:
                                {
                                    var para = this.CalcRecipe.GAIN_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.SOA1:
                                {
                                    var para = this.CalcRecipe.SOA1_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.SOA2:
                                {
                                    var para = this.CalcRecipe.SOA2_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.MIRROR1:
                                {
                                    var para = this.CalcRecipe.MIRROR1_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.MIRROR2:
                                {
                                    var para = this.CalcRecipe.MIRROR2_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.PH1:
                                {
                                    var para = this.CalcRecipe.PH1_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.PH2:
                                {
                                    var para = this.CalcRecipe.PH2_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.LP:
                                {
                                    var para = this.CalcRecipe.LP_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.MPD1:
                                {
                                    var para = this.CalcRecipe.MPD1_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.MPD2:
                                {
                                    var para = this.CalcRecipe.MPD2_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.BIAS1:
                                {
                                    var para = this.CalcRecipe.BIAS1_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                            case Section_Volt.BIAS2:
                                {
                                    var para = this.CalcRecipe.BIAS2_Parameter.Split(',');
                                    if (para.Length < 2)
                                    {

                                    }
                                    else
                                    {
                                        startVoltage = int.Parse(para[0]);
                                        endVoltage = int.Parse(para[1]);
                                    }
                                }
                                break;
                        }

                        int startindex = 0;
                        int endindex = 0;

                        startindex = ArrayMath.FindIndexOfNearestElement(data[Voltage_V].ToArray(), startVoltage);
                        endindex = ArrayMath.FindIndexOfNearestElement(data[Voltage_V].ToArray(), endVoltage);

                        for (int i = startindex; i <= endindex; i++)
                        {
                            Currentlist.Add(data[CurrentTag][i]);
                            Voltagelist.Add(data[Voltage_V][i]);

                        }
                        var PolyFit = PolyFitMath.PolynomialFit(Voltagelist.ToArray(),Currentlist.ToArray(),  1);

                        var k = PolyFit.Coeffs[0];

                        dataSummary_1.Value = k;

                        SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();

                        dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}{section}_Volt_{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}";

                        dataSummary_2.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_2);

                        double[] derivative = Currentlist.Zip(Voltagelist, (a, b) => a * b).ToArray();
                        int max_index = 0;
                        int min_index = 0;
                        ArrayMath.GetMaxAndMinIndex(derivative, out max_index, out min_index);
                        var derivative_Max_Volt = Voltagelist[max_index];
                        dataSummary_2.Value = derivative_Max_Volt;
                    }
                }
            }
            catch (Exception ex)
            {
                this._core.Log_Global(ex.Message);
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.CreateCalculatorFailed, ex);
            }










        }
    }
}
