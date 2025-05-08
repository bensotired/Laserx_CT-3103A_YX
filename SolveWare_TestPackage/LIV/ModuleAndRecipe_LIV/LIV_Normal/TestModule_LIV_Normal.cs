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
     (
        "TestCalculator_LIV_Ith3_MultiRawData",
       "TestCalculator_LIV_Ith2_MultiRawData",
        "TestCalculator_LIV_Ith1_MultiRawData"
     )]


    #region  轴、位置、IO、仪器

    [StaticResource(ResourceItemType.AXIS, "LNX", "LNX")]
    [StaticResource(ResourceItemType.AXIS, "LNY", "LNY")]
    [StaticResource(ResourceItemType.AXIS, "LNZ", "LNZ")]
    [StaticResource(ResourceItemType.POS, "LN_Integratingsphere", "Integratingsphere")]
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
    public class TestModule_LIV_Normal : TestModuleBase
    {

        public TestModule_LIV_Normal() : base() { }

        #region 以Get获取资源
        MotorAxisBase X { get { return (MotorAxisBase)this.ModuleResource["LNX"]; } }
        MotorAxisBase Y { get { return (MotorAxisBase)this.ModuleResource["LNY"]; } }
        MotorAxisBase Z { get { return (MotorAxisBase)this.ModuleResource["LNZ"]; } }
        AxesPosition LN_Integratingsphere { get { return (AxesPosition)this.ModuleResource["LN_Integratingsphere"]; } }
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

        TestRecipe_LIV_Normal TestRecipe { get; set; }
        RawData_LIV_Normal RawData { get; set; }
        RawDataMenu_Normal RawDataMenu { get; set; }
        QWLT2_TestDta qWLT2_TestDta { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV_Normal);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawDataMenu = new RawDataMenu_Normal();
            return RawDataMenu;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV_Normal>(testRecipe);
        }
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
                            if (bp.Name== "MIRROR1_mid_slope_val")
                            {
                                qWLT2_TestDta.MIRROR1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name== "MIRROR2_mid_slope_val")
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
        string SerialNumber { get; set; }
        string MaskName { get; set; }
        public override void Run(CancellationToken token)
        {
            try
            {
                string path = Application.StartupPath + $@"\Data\LIV_Normal\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\LIV_Normal";
                }

                if (string.IsNullOrEmpty(MaskName))
                {
                    MaskName = "DO721";
                }

                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "CalibrationFactor.xml");
                PDCalibrationData compensation = XmlHelper.DeserializeFile<PDCalibrationData>(configFileFullPath);

                float MPD1_ComplianceCurrent_mA = 20;
                float MPD2_ComplianceCurrent_mA = 1;
                if (LN_Integratingsphere.ItemCollection.Count == 3)
                {
                    X.MoveToV3(LN_Integratingsphere.ItemCollection[0].Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    X.WaitMotionDone();
                    Z.MoveToV3(LN_Integratingsphere.ItemCollection[1].Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    Z.WaitMotionDone();
                    Y.MoveToV3(LN_Integratingsphere.ItemCollection[2].Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    Y.WaitMotionDone();
                }
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

                //从QWLT拿到所需通道家电数值
                //LP,MIRROR1,MIRROR2,SOA1,SOA2,PH1,PH2,MPD1,MPD2,BIAS1,BIAS2
                double Ph_1_idle_current_mA = 0;
                double Ph_2_idle_current_mA = 0;

                double Ph_1_Result_current_mA = Ph_1_idle_current_mA;
                double Ph_2_Result_current_mA = Ph_2_idle_current_mA;


                this.Log_Global($"开始测试!");
                if (this.TestRecipe.Inherit)
                {
                    GAIN.AssignmentMode_Current(qWLT2_TestDta.GAIN, 2.5);
                    LP.AssignmentMode_Current(qWLT2_TestDta.LP, 2.5);
                    MIRROR1.AssignmentMode_Current(qWLT2_TestDta.MIRROR1, 2.5);
                    MIRROR2.AssignmentMode_Current(qWLT2_TestDta.MIRROR2, 2.5);

                    SOA1.AssignmentMode_Current(qWLT2_TestDta.SOA1, 2.5);
                    SOA2.AssignmentMode_Current(qWLT2_TestDta.SOA2, 2.5);


                    MPD1.AssignmentMode_Voltage(qWLT2_TestDta.MPD1, 20);
                    MPD2.AssignmentMode_Voltage(qWLT2_TestDta.MPD2, 20);
                    BIAS1.AssignmentMode_Voltage(qWLT2_TestDta.BIAS1, 20);
                    BIAS2.AssignmentMode_Voltage(qWLT2_TestDta.BIAS2, 20);


                }
                else
                {
                    //临时设定为定制  以下数据需要从qwlt2获取
                    GAIN.AssignmentMode_Current(120, 2.5);
                    LP.AssignmentMode_Current(1.25, 2.5);
                    MIRROR1.AssignmentMode_Current(9.3, 1.6);
                    MIRROR2.AssignmentMode_Current(10.7, 1.6);

                    SOA1.AssignmentMode_Current(50, 2.5);
                    SOA2.AssignmentMode_Current(40, 2.5);


                    MPD1.AssignmentMode_Voltage(-2, 20);
                    MPD2.AssignmentMode_Voltage(-2, 20);
                    BIAS1.AssignmentMode_Voltage(-3, 20);
                    BIAS2.AssignmentMode_Voltage(-3, 20);


                }


                GAIN.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test
                    (
                    this.TestRecipe.Gain_DrivingCurrent_Of_PhaseScan_mA,
                    this.TestRecipe.GainComplianceVoltage_V);

                //扫PH2
                this.Log_Global($"Start PH2 sweep ...{this.TestRecipe.Phase_Start_mA}~{this.TestRecipe.Phase_Stop_mA} mA step {this.TestRecipe.Phase_Step_mA} mA ");
                this.RawData = new RawData_LIV_Normal();
                this.RawData.TestStepStartTime = DateTime.Now;
                double[] p1_p2_Currents_mA = ArrayMath.CalculateArray(this.TestRecipe.Phase_Start_mA, this.TestRecipe.Phase_Stop_mA, this.TestRecipe.Phase_Step_mA);


                double sourceDelay_s = 0.001;
                double ApertureTime_s = 0.001;
                PH1.Reset();
                MPD1.Reset();
                MPD2.Reset();

                //PH1 加固定电流 0mA
                PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_1_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);

                //扫描PH2, 读取PD MPD
                PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.Phase_Start_mA, this.TestRecipe.Phase_Step_mA,
                        this.TestRecipe.Phase_Stop_mA, this.TestRecipe.PhaseComplianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                MPD1.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD1_ComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD2_ComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                PH2.TriggerOutputOn = true;
                PD.TriggerOutputOn = true;
                MPD1.TriggerOutputOn = true;
                MPD2.TriggerOutputOn = true;

                var slaverPd_of_ph1_ph2_gain = new PXISourceMeter_4143[] { PD, MPD1, MPD2 };
                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH2, slaverPd_of_ph1_ph2_gain);
                Merged_PXIe_4143.Trigger(PH2, slaverPd_of_ph1_ph2_gain);

                var resut_ph2_voltage_V = PH2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).VoltageMeasurements;
                var resut_pd_of_ph2_currents_mA = PD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                var mpd1_ph2_current_mA = MPD1.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                var mpd2_ph2_current_mA = MPD2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;

                this.Log_Global("PH2 sweep finished..");


                //扫PH1
                this.Log_Global($"Start PH1 sweep ...{this.TestRecipe.Phase_Start_mA}~{this.TestRecipe.Phase_Stop_mA} mA step {this.TestRecipe.Phase_Step_mA} mA ");

                PD.Reset();
                PH1.Reset();
                PH2.Reset();
                MPD1.Reset();
                MPD2.Reset();

                //PH2加固定电流 0mA
                PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA(Ph_2_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);

                //扫PH1, 读取 PD MPD
                PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.Phase_Start_mA, this.TestRecipe.Phase_Step_mA,
                        this.TestRecipe.Phase_Stop_mA, this.TestRecipe.PhaseComplianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                MPD1.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD1_ComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD2_ComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                PH2.TriggerOutputOn = true;
                PD.TriggerOutputOn = true;
                MPD1.TriggerOutputOn = true;
                MPD2.TriggerOutputOn = true;


                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH1, slaverPd_of_ph1_ph2_gain);
                Merged_PXIe_4143.Trigger(PH1, slaverPd_of_ph1_ph2_gain);


                var resut_ph1_voltage_V = PH1.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).VoltageMeasurements;
                var resut_pd_of_ph1_currents_mA = PD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                var mpd1_ph1_current_mA = MPD1.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                var mpd2_ph1_current_mA = MPD2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000.0).CurrentMeasurements;

                this.Log_Global("PH1 sweep finished..");

                //拼接扫描结果
                List<double> Current_Sec = new List<double>();
                List<double> PDList_Sec = new List<double>();
                //p2取反放前面
                for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
                {
                    //Current_Sec.Add(-p1_p2_Currents_mA[i]);
                    //PDList_Sec.Add(resut_pd_of_ph2_currents_mA[i] * 1000);
                    //RawData.Add(new RawDatumItem_LIV_Normal()
                    //{
                    //    Section = "PH_Max",
                    //    Current_mA = -p1_p2_Currents_mA[i],
                    //    Power_mW = resut_pd_of_ph2_currents_mA[i] * 1000,
                    //    Voltage_V = resut_ph2_voltage_V[i]
                    //});

                    Current_Sec.Add(-p1_p2_Currents_mA[i]);
                    PDList_Sec.Add(mpd2_ph2_current_mA[i] * 1000 * -1);
                    RawData.Add(new RawDatumItem_LIV_Normal()
                    {
                        Section = "PH_Max",
                        Current_mA = -p1_p2_Currents_mA[i],
                        Power_mW = mpd2_ph2_current_mA[i] * 1000 * -1,
                        Voltage_V = resut_ph2_voltage_V[i]
                    });
                }
                for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
                {
                    //Current_Sec.Add(p1_p2_Currents_mA[i]);
                    //PDList_Sec.Add(resut_pd_of_ph1_currents_mA[i] * 1000);
                    //RawData.Add(new RawDatumItem_LIV_Normal()
                    //{
                    //    Section = "PH_Max",
                    //    Current_mA = p1_p2_Currents_mA[i],
                    //    Power_mW = resut_pd_of_ph1_currents_mA[i] * 1000,
                    //    Voltage_V = resut_ph1_voltage_V[i]
                    //}); 
                    Current_Sec.Add(p1_p2_Currents_mA[i]);
                    PDList_Sec.Add(mpd2_ph1_current_mA[i] * 1000 * -1);
                    RawData.Add(new RawDatumItem_LIV_Normal()
                    {
                        Section = "PH_Max",
                        Current_mA = p1_p2_Currents_mA[i],
                        Power_mW = mpd2_ph1_current_mA[i] * 1000 * -1,
                        Voltage_V = resut_ph1_voltage_V[i]
                    });
                }


                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;

                //this.RawDataMenu.Add(RawData);

                int max_pd_current_index = 0;
                int min_pd_current_index = 0;
                ArrayMath.GetMaxAndMinIndex(PDList_Sec.ToArray(), out max_pd_current_index, out min_pd_current_index);

                var ph_current_to_set = Current_Sec[max_pd_current_index];

                if (ph_current_to_set > 0 && ph_current_to_set != 0)
                {

                    this.Log_Global($"[PH_Status_of_PD_Max_Sec]  PH1 =[{ Math.Abs(ph_current_to_set)}]mA & PH2 =[0]mA");
                    Ph_1_Result_current_mA = Math.Abs(ph_current_to_set);
                    ////给PH1 PH2加电
                    //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_current_to_set), this.TestRecipe.PhaseComplianceVoltage_V);
                    //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_2_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);
                }
                else
                {
                    this.Log_Global($"[PH_Status_of_PD_Max_Sec]  PH1 =[0]mA & PH2 =[{ Math.Abs(ph_current_to_set)}]mA");
                    Ph_2_Result_current_mA = Math.Abs(ph_current_to_set);
                    ////给PH1 PH2加电
                    //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_1_idle_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);
                    //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_current_to_set), this.TestRecipe.PhaseComplianceVoltage_V);
                }

                ////给PH1 PH2加电
                //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_1_Result_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);
                //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Ph_2_Result_current_mA, this.TestRecipe.PhaseComplianceVoltage_V);


                this.Log_Global($"Start Gain sweep ...{this.TestRecipe.Gain_Start_mA}~{this.TestRecipe.Gain_Stop_mA} mA step {this.TestRecipe.Gain_Step_mA} mA ");
                this.RawData = new RawData_LIV_Normal();
                this.RawData.TestStepStartTime = DateTime.Now;

                //Merged_PXIe_4143.Sweep_LD_PD(this.TestRecipe.Gain_Start_mA, this.TestRecipe.Gain_Step_mA, this.TestRecipe.Gain_Stop_mA, this.TestRecipe.GainComplianceVoltage_V, this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, 0);
                double[] GainCurrents_mA = ArrayMath.CalculateArray(this.TestRecipe.Gain_Start_mA, this.TestRecipe.Gain_Stop_mA, this.TestRecipe.Gain_Step_mA);


                ////if (true)
                //{
                //    GAIN.Reset();
                //    //PD.Reset();
                //    GAIN.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.Gain_Start_mA, this.TestRecipe.Gain_Step_mA,
                //      this.TestRecipe.Gain_Stop_mA, this.TestRecipe.GainComplianceVoltage_V, this.TestRecipe.ApertureTime_s, true);

                //    //PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, GainCurrents_mA.Length, this.TestRecipe.ApertureTime_s, true);

                //    GAIN.TriggerOutputOn = true;
                //    //PD.TriggerOutputOn = true;


                //    //Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, slaverPd_of_ph1_ph2_gain, 0.001);
                //    //Merged_PXIe_4143.Trigger(GAIN, slaverPd_of_ph1_ph2_gain);


                //    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, null);
                //    Merged_PXIe_4143.Trigger(GAIN, null);

                //    var GAINresut = this.GAIN.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                //    var DriveCurrent = GAINresut.CurrentMeasurements;// Merged_PXIe_4143.FetchLDSweepData(this.GAIN, SweepData.Sense_Current_mA, GainCurrents_A.Length);
                //    var DriveVoltage = GAINresut.VoltageMeasurements;// Merged_PXIe_4143.FetchLDSweepData(this.GAIN, SweepData.Sense_Voltage_V, GainCurrents_A.Length);
                //    //var PDCurrent = this.PD.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0).CurrentMeasurements; //Merged_PXIe_4143.FetchLDSweepData(this.PD, SweepData.Sense_Current_mA, GainCurrents_A.Length);

                //    for (int i = 0; i < GainCurrents_mA.Length; i++)
                //    {
                //        RawData.Add(new RawDatumItem_LIV_Normal()
                //        {
                //            Section = "Gain",
                //            Current_mA = GainCurrents_mA[i],
                //            Power_PD = 0,
                //            Voltage_V = DriveVoltage[i]
                //        });
                //    }
                //    this.RawDataMenu.Add(RawData);
                //    this.Log_Global("Gain sweep finished..");
                //}
                //else
                {
                    GAIN.Reset();
                    PD.Reset();
                    MPD1.Reset();
                    MPD2.Reset();

                    LP.Reset();
                    MIRROR1.Reset();
                    MIRROR2.Reset();
                    SOA1.Reset();
                    SOA2.Reset();
                    PH1.Reset();
                    PH2.Reset();

                    BIAS1.Reset();
                    BIAS2.Reset();

                    //扫GAIN 读PD MPD
                    GAIN.SetupMaster_Sequence_SourceCurrent_SenseVoltage(this.TestRecipe.Gain_Start_mA, this.TestRecipe.Gain_Step_mA,
                      this.TestRecipe.Gain_Stop_mA, this.TestRecipe.GainComplianceVoltage_V, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);

                    PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, this.TestRecipe.PDComplianceCurrent_mA, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    MPD1.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD1_ComplianceCurrent_mA, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(this.TestRecipe.PDBiasVoltage_V, MPD2_ComplianceCurrent_mA, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    
                    //20241111 将所有通道数据都存储出来
                    //电流
                    LP.SetupSlaver_Sequence_SourceCurrent_SenceVoltage((float)qWLT2_TestDta.LP, 2.5f, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    MIRROR1.SetupSlaver_Sequence_SourceCurrent_SenceVoltage((float)qWLT2_TestDta.MIRROR1, 2.5f, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    MIRROR2.SetupSlaver_Sequence_SourceCurrent_SenceVoltage((float)qWLT2_TestDta.MIRROR2, 2.5f, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    SOA1.SetupSlaver_Sequence_SourceCurrent_SenceVoltage((float)qWLT2_TestDta.SOA1, 2.5f, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    SOA2.SetupSlaver_Sequence_SourceCurrent_SenceVoltage((float)qWLT2_TestDta.SOA2, 2.5f, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    //给PH1 PH2加电
                    PH1.SetupSlaver_Sequence_SourceCurrent_SenceVoltage((float)Ph_1_Result_current_mA, this.TestRecipe.PhaseComplianceVoltage_V, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    PH2.SetupSlaver_Sequence_SourceCurrent_SenceVoltage((float)Ph_2_Result_current_mA, this.TestRecipe.PhaseComplianceVoltage_V, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    
                    //电压
                    BIAS1.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)qWLT2_TestDta.BIAS1, 20, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);
                    BIAS2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)qWLT2_TestDta.BIAS2, 20, GainCurrents_mA.Length, sourceDelay_s, this.TestRecipe.ApertureTime_s, true);


                    GAIN.TriggerOutputOn = true;
                    PD.TriggerOutputOn = true;
                    MPD1.TriggerOutputOn = true;
                    MPD2.TriggerOutputOn = true;

                    //电流
                    LP.TriggerOutputOn = true;
                    MIRROR1.TriggerOutputOn = true;
                    MIRROR2.TriggerOutputOn = true;
                    SOA1.TriggerOutputOn = true;
                    SOA2.TriggerOutputOn = true;
                    PH1.TriggerOutputOn = true;
                    PH2.TriggerOutputOn = true;
                    //电压
                    BIAS1.TriggerOutputOn = true;
                    BIAS2.TriggerOutputOn = true;

                    var slaverPd_MPD1_MPD2 = new PXISourceMeter_4143[] { PD, MPD1, MPD2, LP, MIRROR1 , MIRROR2, SOA1, SOA2, PH1, PH2, BIAS1, BIAS2 };
                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, slaverPd_MPD1_MPD2, 0.001);
                    Merged_PXIe_4143.Trigger(GAIN, slaverPd_MPD1_MPD2);


                    var GAINresut = this.GAIN.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var DriveCurrent = GAINresut.CurrentMeasurements;// Merged_PXIe_4143.FetchLDSweepData(this.GAIN, SweepData.Sense_Current_mA, GainCurrents_A.Length);
                    var DriveVoltage = GAINresut.VoltageMeasurements;// Merged_PXIe_4143.FetchLDSweepData(this.GAIN, SweepData.Sense_Voltage_V, GainCurrents_A.Length);
                    var PDCurrent = this.PD.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0).CurrentMeasurements; //Merged_PXIe_4143.FetchLDSweepData(this.PD, SweepData.Sense_Current_mA, GainCurrents_A.Length);
                    var MPD1Current = this.MPD1.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0).CurrentMeasurements;
                    var MPD2Current = this.MPD2.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0).CurrentMeasurements;

                    //电流
                    var LPresut = this.LP.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var MIRROR1resut = this.MIRROR1.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var MIRROR2resut = this.MIRROR2.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var SOA1resut = this.SOA1.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var SOA2resut = this.SOA2.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var PH1resut = this.PH1.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var PH2resut = this.PH2.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    //电压
                    var BIAS1resut = this.BIAS1.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                    var BIAS2resut = this.BIAS2.Fetch_MeasureVals(GainCurrents_mA.Length, 10 * 1000.0);
                                       


                    this.RawData.TestStepEndTime = DateTime.Now;
                    this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                    //PDCurrent= ArrayMath.CalculateMovingAverage(PDCurrent, 2);

                    var K = compensation.PD_K[1550];
                    List<double> resultPdCurrent = new List<double>();
                    for (int i = 0; i < GainCurrents_mA.Length; i++)
                    {
                        resultPdCurrent.Add(K * PDCurrent[i] * 1000);
                    }
                    for (int i = 0; i < GainCurrents_mA.Length; i++)
                    {
                        RawData.Add(new RawDatumItem_LIV_Normal()
                        {
                            Section = "Gain",
                            Current_mA = GainCurrents_mA[i],
                            Power_mW = resultPdCurrent[i],
                            Voltage_V = DriveVoltage[i]
                        });
                    }
                    RawData.MAX_Power = resultPdCurrent.Max();

                    //120mA下的光功率
                    {
                        double IS_Current = 120;   //客户要求固定的电流下的光功率

                        RawData.Pout_120mA_Power = GetPowerForSpecifiedCurrent(GainCurrents_mA, resultPdCurrent.ToArray(), IS_Current);

                    }


                    this.RawDataMenu.Add(RawData);
                    this.Log_Global("Gain sweep finished..");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string defaultFileName = string.Concat(@"LIV_Normal_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                    var finalFileName = $@"{path}\{defaultFileName}";

                    using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                    {
                        sw.WriteLine($"{MaskName}");//"DO721");
                        sw.WriteLine($"elapsed time: {this.RawData.TestCostTimeSpan}");//, Temp[C]:0.00,  Threshold[mA]: 0 
                        sw.WriteLine("test: LIVtoIS Sweep");
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
                        sw.WriteLine($"Gain current[mA],Gain voltage[V],Power[mW],MPD1[uA],MPD2 [uA]" +
                            $",Debug_Get," +
                            $"LP[V],MIRROR1[V],MIRROR2[V],SOA1[V],SOA2[V],PH1[V],PH2[V],BIAS1[mA],BIAS2[mA]" +
                            $",Debug_Set," +
                            $"LP[mA],MIRROR1[mA],MIRROR2[mA],SOA1[mA],SOA2[mA],PH1[mA],PH2[mA],BIAS1[V],BIAS2[V]");


                        //sw.WriteLine($"data:");
                        //sw.WriteLine();
                        for (int i = 0; i < GainCurrents_mA.Length; i++)
                        {
                            string str = $"{DriveCurrent[i] * 1000}, {DriveVoltage[i]  },{K * PDCurrent[i] * 1000},{MPD1Current[i] * (-1) * 1000 * 1000},{MPD2Current[i] * (-1) * 1000 * 1000}" +
                                $",X," +
                                $"{LPresut.VoltageMeasurements[i]}," +
                                $"{MIRROR1resut.VoltageMeasurements[i]},{MIRROR2resut.VoltageMeasurements[i]}," +
                                $"{SOA1resut.VoltageMeasurements[i]},{SOA2resut.VoltageMeasurements[i]}," +
                                $"{PH1resut.VoltageMeasurements[i]},{PH2resut.VoltageMeasurements[i]}," +
                                $"{BIAS1resut.CurrentMeasurements[i]*1000},{BIAS2resut.CurrentMeasurements[i]*1000}," +
                                $"X," +
                                $"{LPresut.CurrentMeasurements[i] * 1000}," +
                                $"{MIRROR1resut.CurrentMeasurements[i]*1000},{MIRROR2resut.CurrentMeasurements[i]*1000}," +
                                $"{SOA1resut.CurrentMeasurements[i]*1000},{SOA2resut.CurrentMeasurements[i]*1000}," +
                                $"{PH1resut.CurrentMeasurements[i]*1000},{PH2resut.CurrentMeasurements[i]*1000}," +
                                $"{BIAS1resut.VoltageMeasurements[i]},{BIAS2resut.VoltageMeasurements[i]}";


                            sw.WriteLine(str);
                            //sw.WriteLine($"{DriveCurrent[i] * 1000}, {DriveVoltage[i]  },{K * PDCurrent[i] * 1000},{MPD1Current[i] * (-1) * 1000 * 1000},{MPD2Current[i] * (-1) * 1000 * 1000}");
                        }
                    }
                }

                //20241121 WHB增加图片存储功能
                string imagePath = Path.Combine(path, $@"..\LIV_Normal_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                imagePath = Path.GetFullPath(imagePath);
                ChartsSaver.SaveCharts(RawDataMenu, imagePath);


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


        public double GetPowerForSpecifiedCurrent(double[] currents, double[] powers, double Current_mA)
        {
            try
            {

                if (currents.Length <= 0 || powers.Length <= 0 || currents.Length != powers.Length)
                {
                    throw new Exception($"Pout: xArray and yArray are of unequal size!");
                }

                double current_mA = Current_mA;//传入的电流值
                double minOffset_mA = double.MaxValue;
                double power_mW = 0.0;

                for (int i = 0; i < currents.Length; i++)
                {
                    double offset_mA = Math.Abs(currents[i] - current_mA);
                    if (offset_mA < minOffset_mA)
                    {
                        minOffset_mA = offset_mA;
                        power_mW = powers[i];
                    }
                }

                //两电流点之间至少应该相差5mA 否则无效
                if (minOffset_mA < 5)
                {
                    return power_mW;
                }
                else
                {
                    return -1;
                }



            }
            catch (Exception ex)
            {
                throw new Exception("GetPowerForSpecifiedCurrent:", ex);
            }

        }


    }
}