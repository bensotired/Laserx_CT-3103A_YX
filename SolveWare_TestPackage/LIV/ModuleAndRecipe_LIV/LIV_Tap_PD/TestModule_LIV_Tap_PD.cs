using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     ("TestCalculator_LIV_Tap_PD_Ith1_MultiRawData",
         "TestCalculator_LIV_Tap_PD_Ith2_MultiRawData",
         "TestCalculator_LIV_Tap_PD_Ith3_MultiRawData"
     )]


    #region  轴、位置、IO、仪器

    //[StaticResource(ResourceItemType.AXIS, "LNX", "LNX")]
    //[StaticResource(ResourceItemType.AXIS, "LNY", "LNY")]
    //[StaticResource(ResourceItemType.AXIS, "LNZ", "LNZ")]
    //[StaticResource(ResourceItemType.POS, "LN_Integratingsphere", "Integratingsphere")]
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
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
    [ConfigurableInstrument("ScpiOsa", "OSA", "OSA")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]

    #endregion
    public class TestModule_LIV_Tap_PD : TestModuleBase
    {

        public TestModule_LIV_Tap_PD() : base() { }

        #region 以Get获取资源
        //MotorAxisBase X { get { return (MotorAxisBase)this.ModuleResource["LNX"]; } }
        //MotorAxisBase Y { get { return (MotorAxisBase)this.ModuleResource["LNY"]; } }
        //MotorAxisBase Z { get { return (MotorAxisBase)this.ModuleResource["LNZ"]; } }
        //AxesPosition LN_Integratingsphere { get { return (AxesPosition)this.ModuleResource["LN_Integratingsphere"]; } }
        IOBase SwitchPD { get { return (IOBase)this.ModuleResource["PD_3"]; } }
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
        ScpiOsa OSA_86142B { get { return (ScpiOsa)this.ModuleResource["OSA"]; } }
        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }

        #endregion

        TestRecipe_LIV_Tap_PD TestRecipe { get; set; }
        RawData_LIV_Tap_PD RawData { get; set; }
        RawDataMenu_Tap_PD RawDataMenu { get; set; }

        //光谱
        RawData_LIV_Tap_PD_SP RawData_SP { get; set; }
        RawDataMenu_Tap_PD_SP RawDataMenu_SP { get; set; }

        QWLT2_TestDta qWLT2_TestDta { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV_Tap_PD);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawDataMenu = new RawDataMenu_Tap_PD();
            RawDataMenu_SP = new RawDataMenu_Tap_PD_SP();
            return RawDataMenu;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV_Tap_PD>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //if (LN_Integratingsphere.ItemCollection.Count == 3)
                //{
                //    X.MoveToV3(LN_Integratingsphere.ItemCollection[0].Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                //    X.WaitMotionDone();
                //    Z.MoveToV3(LN_Integratingsphere.ItemCollection[1].Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                //    Z.WaitMotionDone();
                //    Y.MoveToV3(LN_Integratingsphere.ItemCollection[2].Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                //    Y.WaitMotionDone();
                //}

                string path = Application.StartupPath + $@"\Data\LIV_Tap_PD\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\LIV_Tap_PD";
                }

                if (string.IsNullOrEmpty(MaskName))
                {
                    MaskName = "DO721";
                }

                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "CalibrationFactor.xml");
                PDCalibrationData compensation = XmlHelper.DeserializeFile<PDCalibrationData>(configFileFullPath);

                Merged_PXIe_4143.Reset();

                SwitchPD.TurnOn(true);
                //OSwitch切换:
                {
                    var och = Convert.ToByte(this.TestRecipe.LIVOpticalSwitchChannel);
                    if (OSwitch.SetCH(och) == false)
                    {
                        string msg = "光开关通道切换失败！";
                        this.Log_Global(msg);
                        throw new Exception(msg);
                    }
                }

                //从QWLT拿到所需通道家电数值
                //LP,MIRROR1,MIRROR2,SOA1,SOA2,PH1,PH2,MPD1,MPD2,BIAS1,BIAS2
                double Ph_1_idle_current_mA = 0;
                double Ph_2_idle_current_mA = 0;
                double sourceDelay_s = 0.01;
                float MPD1_ComplianceCurrent_mA = 20;
                float MPD2_ComplianceCurrent_mA = 1;

                this.Log_Global($"开始测试!");
                if (this.TestRecipe.Inherit)
                {
                    GAIN.AssignmentMode_Current(qWLT2_TestDta.GAIN, 2.5);
                    LP.AssignmentMode_Current(qWLT2_TestDta.LP, 2.5);
                    MIRROR1.AssignmentMode_Current(qWLT2_TestDta.MIRROR1, 2.5);
                    MIRROR2.AssignmentMode_Current(qWLT2_TestDta.MIRROR2, 2.5);

                    SOA1.AssignmentMode_Current(qWLT2_TestDta.SOA1, 2.5);
                    SOA2.AssignmentMode_Current(qWLT2_TestDta.SOA2, 2.5);

                    PH1.AssignmentMode_Current(qWLT2_TestDta.PH1, 2.5);
                    PH2.AssignmentMode_Current(qWLT2_TestDta.PH2, 2.5);


                    MPD1.AssignmentMode_Voltage(qWLT2_TestDta.MPD1, 20);
                    MPD2.AssignmentMode_Voltage(qWLT2_TestDta.MPD2, 20);
                    BIAS1.AssignmentMode_Voltage(qWLT2_TestDta.BIAS1, 20);
                    BIAS2.AssignmentMode_Voltage(qWLT2_TestDta.BIAS2, 20);


                }
                else
                {
                    //临时设定为定制  以下数据需要从qwlt2获取
                    GAIN.AssignmentMode_Current(120, 2.5);
                    LP.AssignmentMode_Current(0, 2.5);
                    MIRROR1.AssignmentMode_Current(9.7, 2.5);
                    MIRROR2.AssignmentMode_Current(10.3, 2.5);

                    SOA1.AssignmentMode_Current(50, 2.5);
                    SOA2.AssignmentMode_Current(40, 2.5);
                    PH1.AssignmentMode_Current(0, 2.5);
                    PH2.AssignmentMode_Current(0, 2.5);


                    MPD1.AssignmentMode_Voltage(-2, 20);
                    MPD2.AssignmentMode_Voltage(-2, 20);
                    BIAS1.AssignmentMode_Voltage(-3, 20);
                    BIAS2.AssignmentMode_Voltage(-3, 20);


                }
                #region

                //GAIN.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test
                //    (
                //    this.TestRecipe.Gain_DrivingCurrent_Of_PhaseScan_mA,
                //    this.TestRecipe.GainComplianceVoltage_V);

                //this.Log_Global($"Start PH2 sweep ...{this.TestRecipe.Phase_Start_mA}~{this.TestRecipe.Phase_Stop_mA} mA step {this.TestRecipe.Phase_Step_mA} mA ");
                //this.RawData = new RawData_LIV_Tap_PD();
                //this.RawData.TestStepStartTime = DateTime.Now;
                //double[] p1_p2_Currents_mA = ArrayMath.CalculateArray(this.TestRecipe.Phase_Start_mA, this.TestRecipe.Phase_Stop_mA, this.TestRecipe.Phase_Step_mA);
                //PH1.Reset();
                //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_1_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);

                //DataBook<double, double> datas_2 = new DataBook<double, double>();



                //PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.Phase_Start_mA, this.TestRecipe.Phase_Step_mA,
                //        this.TestRecipe.Phase_Stop_mA, this.TestRecipe.PhaseComplianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                //PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                //PH2.TriggerOutputOn = true;
                //PD.TriggerOutputOn = true;

                //var slaverPd_of_ph1_ph2_gain = new PXISourceMeter_4143[] { PD };
                //Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH2, slaverPd_of_ph1_ph2_gain);
                //Merged_PXIe_4143.Trigger(PH2, slaverPd_of_ph1_ph2_gain);

                //var resut_ph2_voltage_V = PH2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).VoltageMeasurements;
                //var resut_pd_of_ph2_currents_mA = PD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;


                //this.Log_Global("PH2 sweep finished..");

                //this.Log_Global($"Start PH1 sweep ...{this.TestRecipe.Phase_Start_mA}~{this.TestRecipe.Phase_Stop_mA} mA step {this.TestRecipe.Phase_Step_mA} mA ");

                ////this.RawData = new RawData_LIV_Tap_PD();
                ////this.RawData.TestStepStartTime = DateTime.Now;

                //PD.Reset();
                //PH1.Reset();
                //PH2.Reset();

                //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA(Ph_2_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);

                //PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.Phase_Start_mA, this.TestRecipe.Phase_Step_mA,
                //        this.TestRecipe.Phase_Stop_mA, this.TestRecipe.PhaseComplianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);


                //PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                //PH1.TriggerOutputOn = true;
                //PD.TriggerOutputOn = true;


                //Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH1, slaverPd_of_ph1_ph2_gain);
                //Merged_PXIe_4143.Trigger(PH1, slaverPd_of_ph1_ph2_gain);


                //var resut_ph1_voltage_V = PH1.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).VoltageMeasurements;
                //var resut_pd_of_ph1_currents_mA = PD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;




                //List<double> Current_Sec = new List<double>();
                //List<double> PDList_Sec = new List<double>();
                ////p2取反放前面
                //for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
                //{
                //    Current_Sec.Add(-p1_p2_Currents_mA[i]);
                //    PDList_Sec.Add(resut_pd_of_ph2_currents_mA[i] * 1000);
                //    RawData.Add(new RawDatumItem_LIV_Tap_PD()
                //    {
                //        Section = "PH_Max",
                //        Current_mA = -p1_p2_Currents_mA[i],
                //        Power_PD = resut_pd_of_ph2_currents_mA[i] * 1000,
                //        Voltage_V = resut_ph2_voltage_V[i]
                //    });
                //}
                //for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
                //{
                //    Current_Sec.Add(p1_p2_Currents_mA[i]);
                //    PDList_Sec.Add(resut_pd_of_ph1_currents_mA[i] * 1000);
                //    RawData.Add(new RawDatumItem_LIV_Tap_PD()
                //    {
                //        Section = "PH_Max",
                //        Current_mA = p1_p2_Currents_mA[i],
                //        Power_PD = resut_pd_of_ph1_currents_mA[i] * 1000,
                //        Voltage_V = resut_ph1_voltage_V[i]
                //    });
                //}


                //this.RawData.TestStepEndTime = DateTime.Now;
                //this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;

                //this.RawDataMenu.Add(RawData);

                //int max_pd_current_index = 0;
                //int min_pd_current_index = 0;
                //ArrayMath.GetMaxAndMinIndex(PDList_Sec.ToArray(), out max_pd_current_index, out min_pd_current_index);

                //var ph_current_to_set = Current_Sec[max_pd_current_index];

                //if (ph_current_to_set > 0 && ph_current_to_set != 0)
                //{

                //    this._core.Log_Global($"[PH_Status_of_PD_Max_Sec]  PH1 =[{ Math.Abs(ph_current_to_set)}]mA & PH2 =[0]mA");
                //    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_current_to_set), this.TestRecipe.PhaseComplianceVoltage_V);
                //    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_2_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);
                //}
                //else
                //{
                //    this._core.Log_Global($"[PH_Status_of_PD_Max_Sec]  PH1 =[0]mA & PH2 =[{ Math.Abs(ph_current_to_set)}]mA");
                //    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_1_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);
                //    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_current_to_set), this.TestRecipe.PhaseComplianceVoltage_V);
                //}

                //this.Log_Global($"Start Gain sweep ...{this.TestRecipe.Gain_Start_mA}~{this.TestRecipe.Gain_Stop_mA} mA step {this.TestRecipe.Gain_Step_mA} mA ");
                #endregion
                this.RawData = new RawData_LIV_Tap_PD();
                this.RawData.TestStepStartTime = DateTime.Now;


                double[] GainCurrents_mA = ArrayMath.CalculateArray(this.TestRecipe.Gain_Start_mA, this.TestRecipe.Gain_Stop_mA, this.TestRecipe.Gain_Step_mA);


                GAIN.Reset();
                PD.Reset();
                MPD1.Reset();
                MPD2.Reset();
                GAIN.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.Gain_Start_mA, this.TestRecipe.Gain_Step_mA,
                  this.TestRecipe.Gain_Stop_mA, this.TestRecipe.GainComplianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                MPD1.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD1_ComplianceCurrent_mA, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD2_ComplianceCurrent_mA, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                GAIN.TriggerOutputOn = true;
                PD.TriggerOutputOn = true;
                MPD1.TriggerOutputOn = true;
                MPD2.TriggerOutputOn = true;

                var slaverPd_MPD1_MPD2 = new PXISourceMeter_4143[] { PD, MPD1, MPD2 };
                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, slaverPd_MPD1_MPD2, 0.001);
                Merged_PXIe_4143.Trigger(GAIN, slaverPd_MPD1_MPD2);



                //Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, null);
                //Merged_PXIe_4143.Trigger(GAIN, null);

                var GAINresut = this.GAIN.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                var DriveCurrent = GAINresut.CurrentMeasurements;// Merged_PXIe_4143.FetchLDSweepData(this.GAIN, SweepData.Sense_Current_mA, GainCurrents_A.Length);
                var DriveVoltage = GAINresut.VoltageMeasurements;// Merged_PXIe_4143.FetchLDSweepData(this.GAIN, SweepData.Sense_Voltage_V, GainCurrents_A.Length);
                var PDCurrent = this.PD.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0).CurrentMeasurements; //Merged_PXIe_4143.FetchLDSweepData(this.PD, SweepData.Sense_Current_mA, GainCurrents_A.Length);
                var MPD1Current = this.MPD1.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                var MPD2Current = this.MPD2.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0).CurrentMeasurements;

                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;

                var PD_K = this.TestRecipe.PD_K;
                var PD_B = this.TestRecipe.PD_B;
                for (int i = 0; i < GainCurrents_mA.Length; i++)
                {
                    RawData.Add(new RawDatumItem_LIV_Tap_PD()
                    {
                        Section = "Gain",
                        Current_mA = GainCurrents_mA[i],
                        Power_mW = PD_K * PDCurrent[i] * 1000 + PD_B,
                        Voltage_V = DriveVoltage[i]
                    });
                }
                this.RawDataMenu.Add(RawData);
                this.Log_Global("Gain sweep finished..");

                #region 新增扫光谱
                Thread.Sleep(200); //防止加电失败


                this.Log_Global("Spectrum sweep");

                if (this.TestRecipe.Inherit)
                {
                    GAIN.AssignmentMode_Current(qWLT2_TestDta.GAIN, 2.5);
                    LP.AssignmentMode_Current(qWLT2_TestDta.LP, 2.5);
                    MIRROR1.AssignmentMode_Current(qWLT2_TestDta.MIRROR1, 2.5);
                    MIRROR2.AssignmentMode_Current(qWLT2_TestDta.MIRROR2, 2.5);

                    SOA1.AssignmentMode_Current(qWLT2_TestDta.SOA1, 2.5);
                    SOA2.AssignmentMode_Current(qWLT2_TestDta.SOA2, 2.5);

                    PH1.AssignmentMode_Current(qWLT2_TestDta.PH1, 2.5);
                    PH2.AssignmentMode_Current(qWLT2_TestDta.PH2, 2.5);


                    MPD1.AssignmentMode_Voltage(qWLT2_TestDta.MPD1, 20);
                    MPD2.AssignmentMode_Voltage(qWLT2_TestDta.MPD2, 20);
                    BIAS1.AssignmentMode_Voltage(qWLT2_TestDta.BIAS1, 20);
                    BIAS2.AssignmentMode_Voltage(qWLT2_TestDta.BIAS2, 20);


                }
                else
                {
                    //临时设定为定制  以下数据需要从qwlt2获取
                    GAIN.AssignmentMode_Current(120, 2.5);
                    LP.AssignmentMode_Current(0, 2.5);
                    MIRROR1.AssignmentMode_Current(9.7, 2.5);
                    MIRROR2.AssignmentMode_Current(10.3, 2.5);

                    SOA1.AssignmentMode_Current(50, 2.5);
                    SOA2.AssignmentMode_Current(40, 2.5);
                    PH1.AssignmentMode_Current(0, 2.5);
                    PH2.AssignmentMode_Current(0, 2.5);


                    MPD1.AssignmentMode_Voltage(-2, 20);
                    MPD2.AssignmentMode_Voltage(-2, 20);
                    BIAS1.AssignmentMode_Voltage(-3, 20);
                    BIAS2.AssignmentMode_Voltage(-3, 20);


                }



                if (!OSA_86142B.IsOnline)
                {
                    this.Log_Global($"OSA 连接失败！");
                    return;
                }
                //OSwitch切换:
                {
                    var och = Convert.ToByte(this.TestRecipe.SPOpticalSwitchChannel);
                    if (OSwitch.SetCH(och) == false)
                    {
                        string msg = "光开关通道切换失败！";
                        this.Log_Global(msg);
                        throw new Exception(msg);
                    }
                }

                Thread.Sleep(200); //防止加电失败

                PXISourceMeter_4143[] SourceMeterCheckDataList = new PXISourceMeter_4143[]
                {
                        GAIN,
                        LP,
                        MIRROR1,
                        MIRROR2,

                        SOA1,
                        SOA2,

                        PH1,
                        PH2,
                    //MPD1,
                    //MPD2,
                    //BIAS1,
                    //BIAS2,
                };


                string name = "";
                double curr = 0;
                double volt = 0;

                foreach (var item in SourceMeterCheckDataList)
                {
                    name = item.Name.ToString();
                    curr = item.ReadCurrent_A() * 1000.0;
                    volt = item.ReadVoltage_V();
                    this.Log_Global($"{name}:Curr[{curr}mA] Volt[{volt}V]");
                }

                //光谱数据
                this.RawData_SP = new RawData_LIV_Tap_PD_SP();
                this.RawData_SP.TestStepStartTime = DateTime.Now;


                OSA_86142B.Reset();
                double CenterWavelength_nm = this.TestRecipe.CenterWavelength_nm;
                double WavelengthSpan_nm = this.TestRecipe.WavelengthSpan_nm;
                string Sensitivity_dbm = "-70";
                double Res_nm = this.TestRecipe.Resolution_nm;

                OSA_86142B.CenterWavelength_nm = CenterWavelength_nm;
                OSA_86142B.WavelengthSpan_nm = WavelengthSpan_nm;
                OSA_86142B.Sensitivity_dBm = Sensitivity_dbm; //20240709增加最低希望的底噪
                OSA_86142B.ResolutionBandwidth_nm = Res_nm; //20240709分辨率

                int tl = 1001;
                if(int.TryParse(this.TestRecipe.OsaTraceLength_string,out tl))
                {
                    OSA_86142B.TraceLength = tl;
                }
                else
                {
                    OSA_86142B.TraceLength = 1001;
                }

                //光谱曲线
                var waveandpower = OSA_86142B.GetOpticalSpectrumTrace(true);
                List<double> wave = new List<double>();
                List<double> power = new List<double>();
                for (int j = 0; j < waveandpower.Count; j++)
                {
                    RawData_SP.Add(new RawDatumItem_LIV_Tap_PD_SP()
                    {
                        Section = "Spectrum",
                        Wavelength_nm = waveandpower[j].Wavelength_nm,
                        Power_dBm = waveandpower[j].Power_dBm
                    });

                    wave.Add(waveandpower[j].Wavelength_nm);
                    power.Add(waveandpower[j].Power_dBm);
                }

                //var Power_dBM = power.Max();
                var Power_dBM = OSA_86142B.GetSMSR();

                int max_index = 0;
                int min_index = 0;
                ArrayMath.GetMaxAndMinIndex(power.ToArray(), out max_index, out min_index);
                var Wavelength_nm = wave[max_index];

                this.RawDataMenu.Wavelength = Wavelength_nm;
                this.RawDataMenu.SMSR = Power_dBM;

                MPD1.IsOutputOn = true;
                MPD2.IsOutputOn = true;
                this.RawDataMenu.mPd1_uA = Math.Round(MPD1.ReadCurrent_A() * 1000 * 1000, 6);
                this.RawDataMenu.mPd2_uA = Math.Round(MPD2.ReadCurrent_A() * 1000 * 1000, 6);
                


                this.RawData_SP.TestStepEndTime = DateTime.Now;
                this.RawData_SP.TestCostTimeSpan = this.RawData_SP.TestStepEndTime - this.RawData_SP.TestStepStartTime;

                this.RawDataMenu_SP.Add(RawData_SP);
                this.Log_Global("Spectrum sweep finish");

                #endregion
                //}
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string defaultFileName = string.Concat(@"LIV_Tap_PD_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                var finalFileName = $@"{path}\{defaultFileName}";

                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine($"{MaskName}");//"DO721");
                    sw.WriteLine($"elapsed time: {this.RawData.TestCostTimeSpan}, Temp[C]:0.00,  Threshold[mA]: 0 ");
                    sw.WriteLine("test: LIVtoTapPD Sweep");
                    if (qWLT2_TestDta != null)
                    {
                        sw.WriteLine($"SOA#1:: {qWLT2_TestDta.SOA1}mA,SOA#2:: {qWLT2_TestDta.SOA2}mA," +
                            $"Mirror#1: {qWLT2_TestDta.MIRROR1}mA, Mirror#2: {qWLT2_TestDta.MIRROR2}mA," +
                            $"Phase#1: {qWLT2_TestDta.PH1}mA,Phase#2: {qWLT2_TestDta.PH2}mA," +
                            $"L-Phase: {qWLT2_TestDta.LP}mA," +
                            $"MOD#1: {qWLT2_TestDta.BIAS1}V,MOD#2: {qWLT2_TestDta.BIAS2}V," +
                            $"MPD#1: {qWLT2_TestDta.MPD1}V,MPD#2: {qWLT2_TestDta.MPD2}V," +
                            $"Mirror1-2_en: ON,Phase1-2_en: ON,L-Phase_en: ON,MOD1-2_en: ON,MPD1-2_en: ON");
                    }
                    sw.WriteLine($"Gain current[mA],Gain voltage[V],Power[mW],MPD1[uA],MPD2 [uA]");
                    //sw.WriteLine($"data:");
                    //sw.WriteLine();
                    for (int i = 0; i < GainCurrents_mA.Length; i++)
                    {
                        sw.WriteLine($"{DriveCurrent[i] * 1000}, {DriveVoltage[i] },{PD_K * PDCurrent[i] * 1000 + PD_B},{MPD1Current[i] * (-1) * 1000 * 1000},{MPD2Current[i] * (-1) * 1000 * 1000}");
                    }

                }

                string SPdefaultFileName = string.Concat(@"LIV_Tap_PD_SP_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                var SPFileName = $@"{path}\{SPdefaultFileName}";
                using (StreamWriter sw = new StreamWriter(SPFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine("OSA_86142B");
                    sw.WriteLine($"Wavelength: {Wavelength_nm}nm,SMSR: {Power_dBM}dBm");
                    sw.WriteLine("Wavelength_nm,Power_dBm");
                    for (int i = 0; i < wave.Count; i++)
                    {
                        sw.WriteLine($"{wave[i]}, {power[i]}");
                    }

                }

                //20241121 WHB增加图片存储功能
                string imagePath = Path.Combine(path, $@"..\LIV_Tap_PD_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                imagePath = Path.GetFullPath(imagePath);
                ChartsSaver.SaveCharts(RawDataMenu, imagePath);


                //20241121 WHB增加图片存储功能
                imagePath = Path.Combine(path, $@"..\LIV_Tap_PD_SP_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                imagePath = Path.GetFullPath(imagePath);
                ChartsSaver.SaveCharts(RawDataMenu_SP, imagePath);

            }
            catch (Exception ex)
            {
                this.Log_Global($"[{ex.Message}]");
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                Merged_PXIe_4143.Reset();
                SwitchPD.TurnOn(false);
            }
        }
        string MaskName { get; set; }
        string SerialNumber { get; set; }
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