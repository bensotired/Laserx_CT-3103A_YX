using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     (   "TestCalculator_LIV_SOA_Ith1_MultiRawData",
         "TestCalculator_LIV_SOA_Ith2_MultiRawData",
         "TestCalculator_LIV_SOA_Ith3_MultiRawData"
     )]


    #region  轴、位置、IO、仪器
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
    [ConfigurableInstrument("FWM8612", "FWM8612", "波长计")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]

    #endregion
    public class TestModule_LIV_SOA : TestModuleBase
    {

        public TestModule_LIV_SOA() : base() { }

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

        FWM8612 FWM8612 { get { return (FWM8612)this.ModuleResource["FWM8612"]; } }
        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }

        #endregion

        TestRecipe_LIV_SOA TestRecipe { get; set; }
        RawData_SOA RawData { get; set; }
        QWLT2_TestDta qWLT2_TestDta { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV_SOA);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_SOA();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV_SOA>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                string path = Application.StartupPath + $@"\Data\LIV_SOA\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\LIV_SOA";
                }

                if (string.IsNullOrEmpty(MaskName))
                {
                    MaskName = "DO721";
                }

                var statr = DateTime.Now;
                Merged_PXIe_4143.Reset();

                //OSwitch切换:
                //{
                //    var och = Convert.ToByte(this.TestRecipe.OpticalSwitchChannel);
                //    if (OSwitch.SetCH(och) == false)
                //    {
                //        string msg = "光开关通道切换失败！";
                //        this.Log_Global(msg);
                //        throw new Exception(msg);
                //    }
                //}


                //从QWLT拿到所需通道加电数值
                //LP,MIRROR1,MIRROR2,PH1,PH2,MPD1,MPD2,BIAS1,BIAS2
                if (this.TestRecipe.Inherit)
                {
                    //GAIN.AssignmentMode_Current(120, 2.5);
                    LP.AssignmentMode_Current(qWLT2_TestDta.LP, 2.5);
                    MIRROR1.AssignmentMode_Current(qWLT2_TestDta.MIRROR1, 2.5);
                    MIRROR2.AssignmentMode_Current(qWLT2_TestDta.MIRROR2, 2.5);

                    PH1.AssignmentMode_Current(qWLT2_TestDta.PH1, 2.5);
                    PH2.AssignmentMode_Current(qWLT2_TestDta.PH2, 2.5);
                    //SOA1.AssignmentMode_Current(50, 2.5);
                    //SOA2.AssignmentMode_Current(50, 2.5);

                    MPD1.AssignmentMode_Voltage(qWLT2_TestDta.MPD1, 20);
                    MPD2.AssignmentMode_Voltage(qWLT2_TestDta.MPD2, 20);
                    BIAS1.AssignmentMode_Voltage(qWLT2_TestDta.BIAS1, 20);
                    BIAS2.AssignmentMode_Voltage(qWLT2_TestDta.BIAS2, 20);


                }
                else
                {
                    //GAIN.AssignmentMode_Current(120, 2.5);
                    LP.AssignmentMode_Current(4, 2.5);
                    MIRROR1.AssignmentMode_Current(10, 2.5);
                    MIRROR2.AssignmentMode_Current(10, 2.5);
                    PH1.AssignmentMode_Current(1, 2.5);
                    PH2.AssignmentMode_Current(0, 2.5);
                    //SOA1.AssignmentMode_Current(50, 2.5);
                    //SOA2.AssignmentMode_Current(50, 2.5);

                    MPD1.AssignmentMode_Voltage(-2, 20);
                    MPD2.AssignmentMode_Voltage(-2, 20);
                    BIAS1.AssignmentMode_Voltage(-3, 20);
                    BIAS2.AssignmentMode_Voltage(-3, 20);

                }

                this.Log_Global($"开始测试!");
                this.Log_Global($"Start Gain SOA sweep ...{this.TestRecipe.I_Start_A}~{this.TestRecipe.I_Stop_A} mA step {this.TestRecipe.I_Step_A} mA ");
                double sourceDelay_s = 0.01;
                double[] Currents_mA = ArrayMath.CalculateArray(this.TestRecipe.I_Start_A, this.TestRecipe.I_Stop_A, this.TestRecipe.I_Step_A);
                //Merged_PXIe_4143.SweepSOA(this.TestRecipe.I_Start_A, this.TestRecipe.I_Step_A, this.TestRecipe.I_Stop_A, this.TestRecipe.complianceVoltage_V, this.TestRecipe.SOAVoltage_V, this.TestRecipe.SOA_ComplianceCurrent_mA, 0);

                GAIN.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.I_Start_A, this.TestRecipe.I_Step_A,
                      this.TestRecipe.I_Stop_A, this.TestRecipe.complianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                SOA1.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.SOAVoltage_V, this.TestRecipe.SOA_ComplianceCurrent_mA, Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                SOA2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.SOAVoltage_V, this.TestRecipe.SOA_ComplianceCurrent_mA, Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                GAIN.TriggerOutputOn = true;
                SOA1.TriggerOutputOn = true;
                SOA2.TriggerOutputOn = true;

                var slaverPd_of_soa1_soa2_gain = new PXISourceMeter_4143[] { SOA1, SOA2 };

                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, slaverPd_of_soa1_soa2_gain, 0.001);
                Merged_PXIe_4143.Trigger(GAIN, slaverPd_of_soa1_soa2_gain);


                var SOA1_Current = this.SOA1.Fetch_MeasureVals(Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                var SOA2_Current = this.SOA2.Fetch_MeasureVals(Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                var GAINresut = this.GAIN.Fetch_MeasureVals(Currents_mA.Length, 10 * 1000.0);
                var Gain_Current = GAINresut.CurrentMeasurements;
                var Gain_Voltage = GAINresut.VoltageMeasurements;
                this.Log_Global("Gain SOA sweep finished..");

                for (int i = 0; i < Currents_mA.Length; i++)
                {
                    RawData.Add(new RawDatumItem_SOA()
                    {
                        G_Current_mA = Currents_mA[i],
                        G_Voltage_V = Gain_Voltage[i],
                        S1_Current_mA = SOA1_Current[i] * 1000,
                        S2_Current_mA = SOA2_Current[i] * 1000
                    });
                }
                var stop = DateTime.Now;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string defaultFileName = string.Concat(@"LIV_SOA_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                var finalFileName = $@"{path}\{defaultFileName}";

                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine($"{MaskName}");//"DO721");
                    sw.WriteLine($"elapsed time: {statr-stop}, Temp[C]:0.00,  Threshold[mA]: 0 ");
                    sw.WriteLine("test: LIVtoSOA Sweep");
                    if (qWLT2_TestDta != null)
                    {
                        sw.WriteLine($"Mirror#1: {qWLT2_TestDta.MIRROR1}mA, Mirror#2: {qWLT2_TestDta.MIRROR2}mA,Phase#1: {qWLT2_TestDta.PH1}mA," +
                            $"Phase#2: {qWLT2_TestDta.PH2}mA,L-Phase: {qWLT2_TestDta.LP}mA,Mirror1-2_en: ON,Phase1-2_en: ON,L-Phase_en: ON");
                        sw.WriteLine($"Gain current[mA],Gain voltage[V],SOA1 Current[mA],SOA2 Current[mA]");
                    }
                    //sw.WriteLine($"data:");
                    //sw.WriteLine();
                    for (int i = 0; i < Currents_mA.Length; i++)
                    {
                        sw.WriteLine($"{Gain_Current[i] * 1000}, {Gain_Voltage[i]  },{SOA1_Current[i] * 1000},{SOA2_Current[i] * 1000}");
                    }

                }


                //20241121 WHB增加图片存储功能
                string imagePath = Path.Combine(path, $@"..\LIV_OSA_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                imagePath = Path.GetFullPath(imagePath);
                ChartsSaver.SaveCharts(RawData, imagePath);

            }
            catch (Exception ex)
            {
                this.Log_Global($"[{ex.Message}]");
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                Merged_PXIe_4143.Reset();
            }
        }
        string SerialNumber { get; set; }
        string MaskName { get; set; }
        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            MaskName = dutStreamData.MaskName;
            SerialNumber = dutStreamData.SerialNumber;
            if (dutStreamData.RawDataCollecetionCount < 2)
            {
                return;
            }
            qWLT2_TestDta = new QWLT2_TestDta();
            //var dataMenu = dutStreamData.RawDataCollection[2];
            foreach (var dataMenu in dutStreamData.RawDataCollection)
            {
                if (dataMenu is IRawDataMenuCollection)
                {

                    var rawd = dataMenu as IRawDataMenuCollection;
                    var type = rawd.GetType();
                    if (type.Name == "RawDataMenu_QWLT2")
                    {
                        var props = rawd.GetType().GetProperties();
                        var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);
                        foreach (var bp in broEleProps)
                        {
                            if (bp.Name == "MIRROR1_mid_slope_val")
                            {
                                qWLT2_TestDta.MIRROR1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "MIRROR2_mid_slope_val")
                            {
                                qWLT2_TestDta.MIRROR2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "LP")
                            {
                                qWLT2_TestDta.LP = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "PH_Max_Sec_1")
                            {
                                qWLT2_TestDta.PH1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "PH_Max_Sec_2")
                            {
                                qWLT2_TestDta.PH2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "mPd1_V")
                            {
                                qWLT2_TestDta.MPD1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "mPd2_V")
                            {
                                qWLT2_TestDta.MPD2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "Bais1_V")
                            {
                                qWLT2_TestDta.BIAS1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "Bais2_V")
                            {
                                qWLT2_TestDta.BIAS2 = (double)bp.GetValue(rawd);
                            }


                        }
                        //qWLT2_TestDta.MIRROR1 = (double)broEleProps[2].GetValue(rawd);
                        //qWLT2_TestDta.MIRROR2 = (double)broEleProps[3].GetValue(rawd);
                        //qWLT2_TestDta.LP = (double)broEleProps[4].GetValue(rawd);
                        //qWLT2_TestDta.PH1 = (double)broEleProps[7].GetValue(rawd);
                        //qWLT2_TestDta.PH2 = (double)broEleProps[8].GetValue(rawd);
                    }
                }
            }

        }

    }
}