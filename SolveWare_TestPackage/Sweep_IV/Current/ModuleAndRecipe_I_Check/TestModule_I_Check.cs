using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.IO;
using System.Linq;
using System.Text;
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
    public class TestModule_I_Check : TestModuleBase
    {

        public TestModule_I_Check() : base() { }

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

        TestRecipe_I_Check TestRecipe { get; set; }
        RawData_Curr RawData { get; set; }
        RawDataMenu_Curr RawDataMenu { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_I_Check);
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
        public void ChooseDriveParameters(Section_Curr section, out string drive)
        {
            switch (section)
            {
                case Section_Curr.GAIN:
                    drive = this.TestRecipe.GAIN_Drive_Parameters;
                    break;
                case Section_Curr.SOA1:
                    drive = this.TestRecipe.SOA1_Drive_Parameters;
                    break;
                case Section_Curr.SOA2:
                    drive = this.TestRecipe.SOA2_Drive_Parameters;
                    break;
                case Section_Curr.LP:
                    drive = this.TestRecipe.LP_Drive_Parameters;
                    break;
                case Section_Curr.PH1:
                    drive = this.TestRecipe.PH1_Drive_Parameters;
                    break;
                case Section_Curr.PH2:
                    drive = this.TestRecipe.PH2_Drive_Parameters;
                    break;
                case Section_Curr.MIRROR1:
                    drive = this.TestRecipe.MIRROR1_Drive_Parameters;
                    break;
                case Section_Curr.MIRROR2:
                    drive = this.TestRecipe.MIRROR2_Drive_Parameters;
                    break;
                default:
                    drive = "";
                    break;
            }
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_I_Check>(testRecipe);
        }

        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            MaskName = dutStreamData.MaskName;
            SerialNumber = dutStreamData.SerialNumber;
        }
        string MaskName { get; set; }
        string SerialNumber { get; set; }
        public override void Run(CancellationToken token)
        {
            try
            {
                this.Log_Global($"开始测试!");
                string path = Application.StartupPath + $@"\Data\ICheck\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\ICheck";
                }

                if (string.IsNullOrEmpty(MaskName))
                {
                    MaskName = "DO721";
                }


                Merged_PXIe_4143.Reset();

                double sourceDelay_s = 0.01;
                foreach (string name in Enum.GetNames(typeof(Section_Curr)))
                {
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消 ICheck {name}");
                        return;
                    }
                    Section_Curr section = (Section_Curr)Enum.Parse(typeof(Section_Curr), name);
                    //if (section == Section_Curr.All)
                    //{
                    //    continue;
                    //}
                    this.RawData = new RawData_Curr();
                    this.RawData.TestStepStartTime = DateTime.Now;
                    //Reset();

                    PXISourceMeter_4143 pXISource = null;
                    Choose(name, out pXISource);
                    if (pXISource == null)
                    {
                        return;
                    }
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

                    string dirveparameters = string.Empty;
                    ChooseDriveParameters(section, out dirveparameters);
                    var dirveparametersarray = dirveparameters.Split(',');
                    if (dirveparametersarray.Length != 4)
                    {
                        this.Log_Global($"[{section}] Parameter error");
                        return;
                    }
                    float I_Start_mA = float.Parse(dirveparametersarray[0]);
                    float I_Step_mA = float.Parse(dirveparametersarray[1]);
                    float I_Stop_mA = float.Parse(dirveparametersarray[2]);
                    float complianceVoltage_V = float.Parse(dirveparametersarray[3]);
                    //建立数组表
                    int sweepPoints = Convert.ToInt32((I_Stop_mA - I_Start_mA) / I_Step_mA) + 1;
                    this.Log_Global($"Start IV [{section}] sweep ...{I_Start_mA}~{I_Stop_mA}mA step {I_Step_mA}mA ");

                    pXISource.SetupMaster_Sequence_SourceCurrent_SenseVoltage(I_Start_mA, I_Step_mA,
                        I_Stop_mA, complianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(pXISource, null);
                    Merged_PXIe_4143.Trigger(pXISource, null);

                    var resut = pXISource.Fetch_MeasureVals(sweepPoints, 10 * 1000.0);
                    double[] curr = resut.CurrentMeasurements;//pXISource.Fetch_SenseCurrents(sweepPoints, 10 * 1000.0);
                    for (int i = 0; i < curr.Length; i++)
                    {
                        curr[i] = Math.Round(curr[i] * 1000, 1);
                    }
                    double[] volt = resut.VoltageMeasurements;//pXISource.Fetch_SenseVoltages(sweepPoints, 10 * 1000.0);
                    double[] derivative = curr.Zip(volt, (a, b) => a * b).ToArray();

                    pXISource.Reset();

                    this.Log_Global("IV sweep finished..");

                    for (int i = 0; i < sweepPoints; i++)
                    {
                        RawData.Add(new RawDatumItem_Curr()
                        {
                            Section = name,
                            Current_mA = curr[i],
                            Voltage_V = volt[i],
                            Differentiate = derivative[i]
                        });
                    }
                    RawData.I_Start_mA = I_Start_mA;
                    RawData.I_Step_mA = I_Step_mA;
                    RawData.I_Stop_mA = I_Stop_mA;
                    this.RawData.Section = name;
                    this.RawData.TestStepEndTime = DateTime.Now;
                    this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                    this.RawDataMenu.Add(RawData);


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string defaultFileName = string.Concat(@"ICheck_", name, "_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                    var finalFileName = $@"{path}\{defaultFileName}";

                    using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                    {
                        sw.WriteLine($"Measure Items : { MaskName}"); //DO721");
                        sw.WriteLine($"Time : {BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}, Temp[C]:0.00, Resistance: 0 ohm");
                        sw.WriteLine("test: Current Sweep");
                        sw.WriteLine($"Current[mA], Voltage[V]");
                        sw.WriteLine($"data:");
                        sw.WriteLine();
                        for (int i = 0; i < sweepPoints; i++)
                        {
                            sw.WriteLine($"{curr[i]}, {volt[i]}");
                        }
                    } 
                }

                //20241121 WHB增加图片存储功能
                string imagePath = Path.Combine(path, $@"..\ICHECK_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                imagePath = Path.GetFullPath(imagePath);
                ChartsSaver.SaveCharts(RawDataMenu, imagePath);
            }
            catch (Exception ex)
            {
                this.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                Merged_PXIe_4143.Reset();
            }
        }



    }
}