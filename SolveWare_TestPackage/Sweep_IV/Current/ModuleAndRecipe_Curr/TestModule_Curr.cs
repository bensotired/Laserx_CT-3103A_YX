using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_I_Check")]


    #region  轴、位置、IO、仪器
    //[StaticResource(ResourceItemType.AXIS, "左短摆臂旋转", "左短摆臂")]
    //[StaticResource(ResourceItemType.IO, "Input_左PER前后移动气缸动作", "左PER前后动作")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PD", "PD")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA1", "SOA1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA2", "SOA2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "LP", "LP")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH1", "PH1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH2", "PH2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR1", "MIRROR1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR2", "MIRROR2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS1", "BIAS1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS2", "BIAS2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MPD1", "MPD1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MPD2", "MPD2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "GAIN", "GAIN")]
    [ConfigurableInstrument("PXISourceMeter_6683H", "6683H", "6683H")]
    #endregion
    public class TestModule_Curr : TestModuleBase
    {

        public TestModule_Curr() : base() { }

        #region 以Get获取资源
        PXISourceMeter_4143 PD { get { return (PXISourceMeter_4143)this.ModuleResource["PD"]; } }
        PXISourceMeter_4143 SOA1 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA1"]; } }
        PXISourceMeter_4143 SOA2 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA2"]; } }
        PXISourceMeter_4143 LP { get { return (PXISourceMeter_4143)this.ModuleResource["LP"]; } }
        PXISourceMeter_4143 PH1 { get { return (PXISourceMeter_4143)this.ModuleResource["PH1"]; } }
        PXISourceMeter_4143 PH2 { get { return (PXISourceMeter_4143)this.ModuleResource["PH2"]; } }
        PXISourceMeter_4143 MIRROR1 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR1"]; } }
        PXISourceMeter_4143 MIRROR2 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR2"]; } }
        PXISourceMeter_4143 BIAS1 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS1"]; } }
        PXISourceMeter_4143 BIAS2 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS2"]; } }
        PXISourceMeter_4143 MPD1 { get { return (PXISourceMeter_4143)this.ModuleResource["MPD1"]; } }
        PXISourceMeter_4143 MPD2 { get { return (PXISourceMeter_4143)this.ModuleResource["MPD2"]; } }
        PXISourceMeter_4143 GAIN { get { return (PXISourceMeter_4143)this.ModuleResource["GAIN"]; } }
        PXISourceMeter_6683H S_6683H { get { return (PXISourceMeter_6683H)this.ModuleResource["6683H"]; } }

        #endregion

        TestRecipe_Curr TestRecipe { get; set; }
        RawData_Curr RawData { get; set; }
        RawDataMenu_Curr RawDataMenu { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_Curr);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawDataMenu = new RawDataMenu_Curr();
            return RawDataMenu;
        }
        public void Choose(string section, out PXISourceMeter_4143 pXISource)
        {
            var source = (Section)Enum.Parse(typeof(Section), section);
            switch (source)
            {
                case Section.PD:
                    pXISource = PD;
                    break;
                case Section.SOA1:
                    pXISource = SOA1;
                    break;
                case Section.SOA2:
                    pXISource = SOA2;
                    break;
                case Section.LP:
                    pXISource = LP;
                    break;
                case Section.PH1:
                    pXISource = PH1;
                    break;
                case Section.PH2:
                    pXISource = PH2;
                    break;
                case Section.MIRROR1:
                    pXISource = MIRROR1;
                    break;
                case Section.MIRROR2:
                    pXISource = MIRROR2;
                    break;
                case Section.BIAS1:
                    pXISource = BIAS1;
                    break;
                case Section.BIAS2:
                    pXISource = BIAS2;
                    break;
                case Section.MPD1:
                    pXISource = MPD1;
                    break;
                case Section.MPD2:
                    pXISource = MPD2;
                    break;
                case Section.GAIN:
                    pXISource = GAIN;
                    break;
                default:
                    pXISource = null;
                    break;
            }
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_Curr>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                this.Log_Global($"开始测试!");
                //foreach (string name in Enum.GetNames(typeof(Section_Curr)))
                //{

                //Section_Curr section = (Section_Curr)Enum.Parse(typeof(Section_Curr), name);
                //if (section == Section_Curr.All)
                //{
                //    continue;
                //}
                //if (this.TestRecipe.Section != Section_Curr.All)
                //{
                //if (this.TestRecipe.Section != section)
                //{
                //    continue;
                //}
                //}
                double sourceDelay_s = 0.01;
                this.RawData = new RawData_Curr();
                this.RawData.TestStepStartTime = DateTime.Now;
                //Reset();

                PXISourceMeter_4143 pXISource = null;
                string sectionname = this.TestRecipe.Section.ToString();
                Choose(sectionname, out pXISource);

                if (pXISource == null)
                {
                    return;
                }
                this.Log_Global($"通道[{sectionname}]");
                pXISource.Reset();
                if (!pXISource.IsOnline)
                {
                    pXISource.Timeout_ms = 1000;
                    pXISource.CurrentSetpoint_A = 0;
                    pXISource.VoltageSetpoint_V = 0;
                    pXISource.IsOutputOn = false;
                    return;
                }

                pXISource.Timeout_ms = 60 * 1000;
                //建立数组表
                int sweepPoints = Convert.ToInt32((this.TestRecipe.I_Stop_mA - this.TestRecipe.I_Start_mA) / this.TestRecipe.I_Step_mA) + 1;
                //float time = 50;
                //if (this.TestRecipe.ApertureTime_s >= 9)
                //{
                //    time = 9;
                //}
                this.Log_Global($"Start IV sweep ...{this.TestRecipe.I_Start_mA}~{this.TestRecipe.I_Stop_mA}mA step {this.TestRecipe.I_Step_mA}mA ");

                //var resut = pXISource.Sweep_LD_once(this.TestRecipe.I_Start_mA / 1000, this.TestRecipe.I_Step_mA / 1000, this.TestRecipe.I_Stop_mA / 1000,
                //    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s);
                pXISource.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.I_Start_mA, this.TestRecipe.I_Step_mA,
                    this.TestRecipe.I_Stop_mA, this.TestRecipe.complianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(pXISource, null);
                Merged_PXIe_4143.Trigger(pXISource, null);

                var resut = pXISource.Fetch_MeasureVals(sweepPoints, 10 * 1000.0);
                double[] curr = resut.CurrentMeasurements;//pXISource.Fetch_SenseCurrents(sweepPoints, 10 * 1000.0);
                for (int i = 0; i < curr.Length; i++)
                {
                    curr[i] = curr[i] * 1000;
                }
                double[] volt = resut.VoltageMeasurements;//pXISource.Fetch_SenseVoltages(sweepPoints, 10 * 1000.0);
                double[] derivative = curr.Zip(volt, (a, b) => a * b).ToArray();

                this.Log_Global("IV sweep finished..");

                for (int i = 0; i < sweepPoints; i++)
                {
                    RawData.Add(new RawDatumItem_Curr()
                    {
                        Section = sectionname,
                        Current_mA = curr[i],
                        Voltage_V = volt[i],
                        Differentiate = derivative[i]
                    });
                }
                this.RawData.I_Start_mA = this.TestRecipe.I_Start_mA;
                this.RawData.I_Step_mA = this.TestRecipe.I_Step_mA;
                this.RawData.I_Stop_mA = this.TestRecipe.I_Stop_mA;
                this.RawData.Section = sectionname;
                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                this.RawDataMenu.Add(RawData);
                //}
            }
            catch (Exception ex)
            {
                this._core.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {

            }
        }



    }
}