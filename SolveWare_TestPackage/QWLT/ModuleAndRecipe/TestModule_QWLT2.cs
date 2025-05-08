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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     ("TestCalculator_QWLT2")]


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
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]

    #endregion
    public class TestModule_QWLT2 : TestModuleBase
    {

        public TestModule_QWLT2() : base() { }

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
        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }

        #endregion

        TestRecipe_QWLT2 TestRecipe { get; set; }
        RawData_QWLT2 RawData { get; set; }
        RawDataMenu_QWLT2 RawDataMenu { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_QWLT2);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawDataMenu = new RawDataMenu_QWLT2();
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
            TestRecipe = ConvertObjectTo<TestRecipe_QWLT2>(testRecipe);
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
                string path = Application.StartupPath + $@"\Data\QWLT2\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\QWLT2";
                }
                if (string.IsNullOrEmpty(MaskName))
                {
                    MaskName = "DO721";
                }

                //Turn on : Gain ? L-phase ?Mirror#1?Mirror#2?Phase#1?Phase#2?SOA#1?SOA#2?others
                //Turn off : others? SOA#2 ? SOA#1 ?Phase#2?Phase#1?Mirror#2?Mirror#1?L-phase?Gain

                Merged_PXIe_4143.Reset();

                //OSwitch切换:
                {
                    var och = Convert.ToByte(this.TestRecipe.OpticalSwitchChannel);
                    if (OSwitch.SetCH(och) == false)
                    {
                        string msg = "光开关通道切换失败！";
                        this.Log_Global(msg);
                        throw new Exception(msg);
                    }
                }

                this.Log_Global($"开始测试!");
                GAIN.AssignmentMode_Current(120, 2.5);
                LP.AssignmentMode_Current(4, 2.5);
                MIRROR1.AssignmentMode_Current(10, 2.5);
                MIRROR2.AssignmentMode_Current(10, 2.5);
                PH1.AssignmentMode_Current(1, 2.5);
                PH2.AssignmentMode_Current(0, 2.5);
                SOA1.AssignmentMode_Current(50, 2.5);
                SOA2.AssignmentMode_Current(40, 2.5);

                double mpd1 = this.TestRecipe.mPd1_V;//-2.5;
                double mpd2 = this.TestRecipe.mPd2_V;//-2.5;
                double bias1 = this.TestRecipe.Bais1_V;//-2;
                double bias2 = this.TestRecipe.Bais2_V;// -2;

                this.RawDataMenu.mPd1_V = mpd1;
                this.RawDataMenu.mPd2_V = mpd2;
                this.RawDataMenu.Bais1_V = bias1;
                this.RawDataMenu.Bais2_V = bias2;

                MPD1.AssignmentMode_Voltage(mpd1, 20);
                MPD2.AssignmentMode_Voltage(mpd2, 20);
                BIAS1.AssignmentMode_Voltage(bias1, 20);
                BIAS2.AssignmentMode_Voltage(bias2, 20);


                PXISourceMeter_4143 MPD = null;
                Choose(this.TestRecipe.PH_MPD.ToString(), out MPD);

                float mpdcomplianceCurrent_mA = 20f;
                float mpdsourceVoltage_V = -2f;
                double sourceDelay_s = 0.001;
                double ApertureTime_s = 0.001;

                float complianceVoltage_V = 2.5f;
                //double timeout_ms_sweep = 10 * 1000;
                double timeout_ms_fetchdata = 10 * 1000;

                double mpd_measureCurrentRange_mA = 1;

                switch (MPD.Name.ToUpper())
                {
                    case "MPD1":
                        mpd_measureCurrentRange_mA = 20;
                        break;
                    case "MPD2":
                        mpd_measureCurrentRange_mA = 1;
                        break;
                }



                var array_p1_p2_sweep_setting = this.TestRecipe.P1_P2_mA.Split(',');//0·20
                if (array_p1_p2_sweep_setting.Length != 3)
                {
                    this.Log_Global("[P1_P2_mA] Parameter error");
                    return;
                }
                double[] p1_p2_Currents_mA = ArrayMath.CalculateArray(double.Parse(array_p1_p2_sweep_setting[0]), double.Parse(array_p1_p2_sweep_setting[2]), double.Parse(array_p1_p2_sweep_setting[1]));
                this.Log_Global($"-----PH_Halfway-----");
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消 QWLT2 ");
                    return;
                }
                #region PH_Halfway
                this.RawData = new RawData_QWLT2();
                this.RawData.TestStepStartTime = DateTime.Now;

                PH1.Reset();
                PH2.Reset();
                MPD.Reset();
                PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V,(float) mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                PH1.TriggerOutputOn = true;
                MPD.TriggerOutputOn = true;

                var slavers = new PXISourceMeter_4143[] { MPD };
                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH1, slavers);
                Merged_PXIe_4143.Trigger(PH1, slavers/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                //var PH1_Curr = PH1.Fetch_MeasureVals(p1_p2_Currents_A.Length, timeout_ms_fetchdata).CurrentMeasurements;
                var MPD_P1_Curr = MPD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;

                PH1.Reset();
                PH2.Reset();
                MPD.Reset();
                PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V, (float)mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                PH2.TriggerOutputOn = true;
                MPD.TriggerOutputOn = true;

                var slavers2 = new PXISourceMeter_4143[] { MPD };
                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH2, slavers2);
                Merged_PXIe_4143.Trigger(PH2, slavers2/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                //var PH2_Curr = PH2.Fetch_MeasureVals(p1_p2_Currents_A.Length, timeout_ms_fetchdata).CurrentMeasurements;
                var MPD_P2_Curr = MPD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;


                List<double> ph_driving_currents_mA = new List<double>();
                List<double> mpd_feedback_currents_mA = new List<double>();

                var ph_value_to_set = QWLT_InternalMath.PH_Halfway_Data_Workes(p1_p2_Currents_mA, MPD_P1_Curr, MPD_P2_Curr,
                    out ph_driving_currents_mA, out mpd_feedback_currents_mA);


                #region  remove
                //List<double> ph_driving_currents_mA = new List<double>();
                //List<double> mpd_feedback_currents_mA = new List<double>();

                //List<RawDatumItem_QWLT> tempData = new List<RawDatumItem_QWLT>();

                //for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
                //{

                //    ph_driving_currents_mA.Add(-p1_p2_Currents_mA[i]);
                //    mpd_feedback_currents_mA.Add(MPD_P2_Curr[i] * 1000);
                //}
                //for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
                //{
                //    ph_driving_currents_mA.Add(p1_p2_Currents_mA[i]);
                //    mpd_feedback_currents_mA.Add(MPD_P1_Curr[i] * 1000);
                //}

                //var mpd_increasingRange = QWLT_InternalMath.FindIncreasingRanges(mpd_feedback_currents_mA.ToArray());

                //double temp_delta_mpd = double.MinValue;
                //Range target_mpd_increasingRange = mpd_increasingRange.First();

                //foreach (var item in mpd_increasingRange)
                //{
                //    if (Math.Abs(mpd_feedback_currents_mA[item.Start] - mpd_feedback_currents_mA[item.End]) > temp_delta_mpd)
                //    {
                //        target_mpd_increasingRange = item;
                //    }
                //}

                //#region no use
                ////var der_1st = ArrayMath.CalculateFirstDerivate(ph_driving_currents_mA.ToArray(), mpd_feedback_currents_mA.ToArray());
                ////var der_2nd = ArrayMath.CalculateFirstDerivate(ph_driving_currents_mA.ToArray(), der_1st);

                ////int der_2nd_max_index = 0;
                ////int der_2nd_min_index = 0;

                ////ArrayMath.GetMaxAndMinIndex(der_2nd, out der_2nd_max_index, out der_2nd_min_index);

                ////for (int i = 0; i < ph_driving_currents_mA.Count; i++)
                ////{
                ////    RawData.Add(new RawDatumItem_QWLT()
                ////    {
                ////        Section = "PH1&PH2",
                ////        Current_mA = ph_driving_currents_mA[i],
                ////        MPD = mpd_feedback_currents_mA[i],
                ////        Derivated_1st = der_1st[i],
                ////        Derivated_2nd = der_2nd[i],
                ////    });
                ////}

                ////int mdp_max_index = 0;
                ////int mdp_min_index = 0;
                ////ArrayMath.GetMaxAndMinIndex(mpd_feedback_currents_mA.ToArray(), out mdp_max_index, out mdp_min_index);

                ////double sorted_min_mpd_current_mA = mpd_feedback_currents_mA.Min();
                ////double sorted_max_mpd_current_mA = 0;
                ////double sorted_mid_mpd_current_mA = 0;
                ////List<double> sorted_on_off_mpd_currents_mA = new List<double>();

                ////List<double> sorted_on_off_ph_currents_mA = new List<double>();
                //////从最小mpd电流点往右边找递增区间顶点   关闭->开启
                ////for (int i = der_2nd_max_index; i < mpd_feedback_currents_mA.Count - 1; i++)
                //////for (int i = mdp_min_index; i < mpd_feedback_currents_mA.Count - 1; i++)
                ////{
                ////    if (mpd_feedback_currents_mA[i] < mpd_feedback_currents_mA[i + 1])
                ////    {
                ////        sorted_max_mpd_current_mA = mpd_feedback_currents_mA[i + 1];
                ////        sorted_on_off_mpd_currents_mA.Add(mpd_feedback_currents_mA[i]);
                ////        sorted_on_off_ph_currents_mA.Add(ph_driving_currents_mA[i]);
                ////    }
                ////    else
                ////    {
                ////        break;
                ////    }
                ////}
                //#endregion
                //List<double> sorted_on_off_mpd_currents_mA = new List<double>();
                //List<double> sorted_on_off_ph_currents_mA = new List<double>();


                //for (int i = target_mpd_increasingRange.Start; i < target_mpd_increasingRange.End; i++)
                //{
                //    sorted_on_off_mpd_currents_mA.Add(mpd_feedback_currents_mA[i]);
                //    sorted_on_off_ph_currents_mA.Add(ph_driving_currents_mA[i]);
                //}

                //var sorted_mid_mpd_current_mA = (mpd_feedback_currents_mA[target_mpd_increasingRange.Start] + mpd_feedback_currents_mA[target_mpd_increasingRange.End]) / 2.0;



                //int mdp_mid_index = ArrayMath.FindIndexOfNearestElement(sorted_on_off_mpd_currents_mA.ToArray(), sorted_mid_mpd_current_mA);
                //var ph_value_to_set = sorted_on_off_ph_currents_mA[mdp_mid_index];
                #endregion
                string ph_resut = string.Empty;
                if (ph_value_to_set > 0 && ph_value_to_set != 0)
                {
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_value_to_set), complianceVoltage_V);
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                 
                    this.RawData.PH_Halfway_1 = ph_value_to_set;
                    this.RawData.PH_Halfway_2 = 0;
                    this.RawDataMenu.PH_Halfway_1 = ph_value_to_set;
                    this.RawDataMenu.PH_Halfway_2 = 0;
                    ph_resut = $"PH1=[{Math.Abs(ph_value_to_set)}]mA & PH2=[0]mA";
                    this.Log_Global($"[PH_Halfway] - {ph_resut}");
                }
                else if (ph_value_to_set < 0 && ph_value_to_set != 0)
                {
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_value_to_set), complianceVoltage_V);
                    this.RawData.PH_Halfway_1 = 0;
                    this.RawData.PH_Halfway_2 = ph_value_to_set;
                    this.RawDataMenu.PH_Halfway_1 = 0;
                    this.RawDataMenu.PH_Halfway_2 = ph_value_to_set;

                    ph_resut = $"PH1=[0]mA & PH2=[{Math.Abs(ph_value_to_set)}]mA";

                    this.Log_Global($"[PH_Halfway] - {ph_resut}");
                }
                else
                {
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_value_to_set), complianceVoltage_V);
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(ph_value_to_set), complianceVoltage_V);
                    ph_resut = $"PH1=[{Math.Abs(ph_value_to_set)}]mA & PH2=[{Math.Abs(ph_value_to_set)}]mA";
                    this.RawData.PH_Halfway_1 = ph_value_to_set;
                    this.RawData.PH_Halfway_2 = ph_value_to_set;
                    this.RawDataMenu.PH_Halfway_1 = ph_value_to_set;
                    this.RawDataMenu.PH_Halfway_2 = ph_value_to_set;

                    this.Log_Global($"[PH_Halfway] result = {ph_resut}");

                }



                for (int i = 0; i < ph_driving_currents_mA.Count; i++)
                {
                    RawData.Add(new RawDatumItem_QWLT2()
                    {
                        Section = "PH1&PH2",
                        Current_mA_or_Mirror_Diagonal_Offset = ph_driving_currents_mA[i],
                        MPD_Current_mA = mpd_feedback_currents_mA[i],
                        //Derivated_1st = der_1st[i],
                        //Derivated_2nd = der_2nd[i],
                    });
                }
                this.RawData.Driver_mA = $"{double.Parse(array_p1_p2_sweep_setting[0])}-{double.Parse(array_p1_p2_sweep_setting[2])} step[{double.Parse(array_p1_p2_sweep_setting[1])}]";
                this.RawData.Section = "PH_Halfway";
                this.RawData.resut = ph_resut;
                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                this.RawDataMenu.Add(RawData);
                //return;

                #endregion
                this.Log_Global($"-----MIRROR-----");
                #region MIRROR
                this.RawData = new RawData_QWLT2();
                this.RawData.TestStepStartTime = DateTime.Now;
                Thread.Sleep(5 * 1000);
                var CentralCurrent = this.TestRecipe.CentralCurrent.Split(',');
                if (CentralCurrent.Length != 2)
                {
                    this.Log_Global("[Central current mA] Parameter error");
                    return;
                }

                var M1central = double.Parse(CentralCurrent[0]);
                var M2central = double.Parse(CentralCurrent[1]);
                var offset = this.TestRecipe.Offset;
                var M1start = M1central + offset;
                var M1stop = M1central - offset;
                var M2start = M2central - offset;
                var M2stop = M2central + offset;
                var m1m2_sourceDelay_s = 0.01;
                var m1m2_apertureTime_s = 0.005;

                var step = this.TestRecipe.ScanningStep;

                double[] M1CurrentArray = M1start < M1stop ? ArrayMath.CalculateArray(M1start, M1stop, step) : ArrayMath.CalculateArray(M1start, M1stop, -step);//5 15
                double[] M2CurrentArray = M2start < M2stop ? ArrayMath.CalculateArray(M2start, M2stop, step) : ArrayMath.CalculateArray(M2start, M2stop, -step);//15 5
                List<double> offsetArray = new List<double>();
                for (int i = 0; i < M1CurrentArray.Length; i++)
                {
                    offsetArray.Add(Math.Round(offset + i * step, 3));
                }

                MIRROR1.Reset();
                MIRROR2.Reset();
                MPD.Reset();
                double[] MPD_Curr = null;

                if (true)
                {
              

                    MPD.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(mpdsourceVoltage_V, mpdcomplianceCurrent_mA, mpd_measureCurrentRange_mA);
                   
                    List<double> MPD_Curr_list = new List<double>();
                    for (int i = 0; i < M1CurrentArray.Length; i++)
                    {
                        MIRROR1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(M1CurrentArray[i], complianceVoltage_V);
                        Thread.Sleep(1);
                        MIRROR2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(M2CurrentArray[i], complianceVoltage_V);
                        Thread.Sleep(1);
                        MPD_Curr_list.Add(MPD.ReadCurrent_A());

                    }
                    MPD_Curr = MPD_Curr_list.ToArray();

                    MIRROR1.Reset();
                    MIRROR2.Reset();
                    MPD.Reset();
            
                }
                else
                {
                    MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage((float)M1start, step, (float)M1stop, complianceVoltage_V, m1m2_sourceDelay_s, m1m2_apertureTime_s, true);
                    MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage((float)M2start, step, (float)M2stop, complianceVoltage_V, m1m2_sourceDelay_s, m1m2_apertureTime_s, true);
                    MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V, (float)mpd_measureCurrentRange_mA, M1CurrentArray.Length, m1m2_sourceDelay_s, m1m2_apertureTime_s, true);

                    MIRROR1.TriggerOutputOn = true;
                    MIRROR2.TriggerOutputOn = true;
                    MPD.TriggerOutputOn = true;

                    var slavers3 = new PXISourceMeter_4143[] { MIRROR2, MPD };
                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR1, slavers3);
                    Merged_PXIe_4143.Trigger(MIRROR1, slavers3/*, M1CurrentArray.Length, timeout_ms_sweep*/);

                    //var M1_Curr = MIRROR1.Fetch_MeasureVals(M1CurrentArray.Length, timeout_ms_fetchdata).CurrentMeasurements;
                    //var M2_Curr = MIRROR2.Fetch_MeasureVals(M2CurrentArray.Length, timeout_ms_fetchdata).CurrentMeasurements;
                    MPD_Curr = MPD.Fetch_MeasureVals(M1CurrentArray.Length, timeout_ms_fetchdata).CurrentMeasurements;

                }



                for (int i = 0; i < MPD_Curr.Length; i++)
                {
                    MPD_Curr[i] = MPD_Curr[i] * -1;
                }

                for (int i = 0; i < M1CurrentArray.Length; i++)
                {
                    RawData.Add(new RawDatumItem_QWLT2()
                    {
                        Section = "MIRROR",
                        //Current_mA = offsetArray[i],
                        Current_mA_or_Mirror_Diagonal_Offset = offsetArray[i],
                        //Current_mA = M1_Curr[i] * 1000,
                        MPD_Current_mA = MPD_Curr[i] * 1000,
                        //M2_Current_mA = M2_Curr[i] * 1000
                    });
                }
                #region remove
                ////找出所有递增 /递减区间
                //var increasingRanges = QWLT_InternalMath.FindIncreasingRanges(MPD_Curr);
                //var decreasingRanges = QWLT_InternalMath.FindDecreasingRanges(MPD_Curr);
                //List<int> allRange_mpd_Index_array = new List<int>();

                ////遍历所有递增区间，并计算该区间的中值,并存储该中值在全mpd电流范围内的索引值

                //for (int i = 0; i < increasingRanges.Count; i++)
                //{
                //    if (increasingRanges[i].End - increasingRanges[i].Start < 5)
                //    {
                //        continue;
                //    }
                //    else//有效区间
                //    {
                //        List<int> thisRange_mpd_Index_array = new List<int>();
                //        var start_mpd = MPD_Curr[increasingRanges[i].Start];
                //        var stop_mpd = MPD_Curr[increasingRanges[i].End];
                //        var range_mid_mpd = (start_mpd + stop_mpd) / 2.0;

                //        List<double> thisRange_mpd_curr_array = new List<double>();

                //        for (int j = increasingRanges[i].Start; j < increasingRanges[i].End; j++)
                //        {
                //            thisRange_mpd_curr_array.Add(MPD_Curr[j]);
                //            thisRange_mpd_Index_array.Add(j);
                //        }
                //        var minddleindex = ArrayMath.FindIndexOfNearestElement(thisRange_mpd_curr_array.ToArray(), range_mid_mpd);
                //        var index = thisRange_mpd_Index_array[minddleindex];
                //        allRange_mpd_Index_array.Add(index);
                //    }
                //}
                ////遍历所有递减区间，并计算该区间的中值,并存储该中值在全mpd电流范围内的索引值
                //for (int i = 0; i < decreasingRanges.Count; i++)
                //{
                //    if (decreasingRanges[i].End - decreasingRanges[i].Start < 5)
                //    {
                //        continue;
                //    }
                //    else
                //    {
                //        List<int> thisRange_mpd_Index_array = new List<int>();
                //        var start_mpd = MPD_Curr[decreasingRanges[i].Start];
                //        var stop_mpd = MPD_Curr[decreasingRanges[i].End];
                //        var range_mid_mpd = (start_mpd + stop_mpd) / 2.0;

                //        List<double> thisRange_mpd_curr_array = new List<double>();

                //        for (int j = decreasingRanges[i].Start; j < decreasingRanges[i].End; j++)
                //        {
                //            thisRange_mpd_curr_array.Add(MPD_Curr[j]);
                //            thisRange_mpd_Index_array.Add(j);
                //        }
                //        var minddleindex = ArrayMath.FindIndexOfNearestElement(thisRange_mpd_curr_array.ToArray(), range_mid_mpd);
                //        var index = thisRange_mpd_Index_array[minddleindex];
                //        allRange_mpd_Index_array.Add(index);
                //    }
                //}
                ////比较所有中值索引，找出最接近 0 offset 的索引
                //double tar_zero_mirror_offset_index = MPD_Curr.Length / 2.0;
                //int final_mirror_diagonal_offset_index = 0;
                //double offset_abs_value = double.MaxValue;
                //for (int i = 0; i < allRange_mpd_Index_array.Count; i++)
                //{
                //    if (Math.Abs(allRange_mpd_Index_array[i] - tar_zero_mirror_offset_index) < offset_abs_value)
                //    {
                //        offset_abs_value = Math.Abs(allRange_mpd_Index_array[i] - tar_zero_mirror_offset_index);
                //        final_mirror_diagonal_offset_index = allRange_mpd_Index_array[i];
                //    }
                //}

                //double m1_mid_slope_val = M1CurrentArray[final_mirror_diagonal_offset_index];
                //double m2_mid_slope_val = M2CurrentArray[final_mirror_diagonal_offset_index];
                #endregion
                double m1_mid_slope_val = 0;
                double m2_mid_slope_val = 0;

                QWLT_InternalMath.Mirror_Data_Workes(MPD_Curr, M1CurrentArray, M2CurrentArray,
                    out m1_mid_slope_val, out m2_mid_slope_val);

                MIRROR1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(m1_mid_slope_val, complianceVoltage_V);
                MIRROR2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(m2_mid_slope_val, complianceVoltage_V);

                this.RawData.Section = "MIRROR";
                this.RawData.MIRROR1_mid_slope_val = m1_mid_slope_val;
                this.RawData.MIRROR2_mid_slope_val = m2_mid_slope_val;
                this.RawData.resut = $"MIRROR1=[{m1_mid_slope_val}]mA & MIRROR2=[{m2_mid_slope_val}]mA";
                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                this.RawDataMenu.Add(RawData);
                this.RawDataMenu.MIRROR1_mid_slope_val = m1_mid_slope_val;
                this.RawDataMenu.MIRROR2_mid_slope_val = m2_mid_slope_val;
              
                this.Log_Global($"Mirror diagonal scan result MIRROR1=[{m1_mid_slope_val}]mA & MIRROR2=[{m2_mid_slope_val}]mA");
                //return;
                #endregion
                this.Log_Global($"-----PH_Max-----");

                #region PH_Max
                double p1_p2_idle_val_mA = 0.0;
                this.RawData = new RawData_QWLT2();
                this.RawData.TestStepStartTime = DateTime.Now;

                PH2.Reset();
                PH1.Reset();
                MPD.Reset();
                
                PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(p1_p2_idle_val_mA, complianceVoltage_V);
                PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V,(float) mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                PH1.TriggerOutputOn = true;
                MPD.TriggerOutputOn = true;

                var slavers_Max = new PXISourceMeter_4143[] { MPD };
                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH1, slavers_Max);
                Merged_PXIe_4143.Trigger(PH1, slavers_Max/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                //var PH1_Curr_Max = PH1.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;
                var MPD_P1_Curr_Max = MPD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;

                for (int i = 0; i < MPD_P1_Curr_Max.Length; i++)
                {
                    MPD_P1_Curr_Max[i] = MPD_P1_Curr_Max[i] * -1;
                }

                PH2.Reset();
                PH1.Reset();
                MPD.Reset();

                PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(p1_p2_idle_val_mA, complianceVoltage_V);
                PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V, (float)mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                PH2.TriggerOutputOn = true;
                MPD.TriggerOutputOn = true;

                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH2, slavers_Max);
                Merged_PXIe_4143.Trigger(PH2, slavers_Max/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                //var PH2_Curr_Max = PH2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;
                var MPD_P2_Curr_Max = MPD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;

                for (int i = 0; i < MPD_P2_Curr_Max.Length; i++)
                {
                    MPD_P2_Curr_Max[i] = MPD_P2_Curr_Max[i] * -1;
                }


                List<double> phase_max_1st_Currents = new List<double>();
                List<double> phase_max_1st_MPDList = new List<double>();
                //p2
                for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
                {
                    phase_max_1st_Currents.Add(-p1_p2_Currents_mA[i]);
                    phase_max_1st_MPDList.Add(MPD_P2_Curr_Max[i] * 1000);
                    RawData.Add(new RawDatumItem_QWLT2()
                    {
                        Section = "PH_Max",
                        Current_mA_or_Mirror_Diagonal_Offset = -p1_p2_Currents_mA[i],
                        MPD_Current_mA = MPD_P2_Curr_Max[i] * 1000,
                    });
                }
                //p1
                for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
                {
                    phase_max_1st_Currents.Add(p1_p2_Currents_mA[i]);
                    phase_max_1st_MPDList.Add(MPD_P1_Curr_Max[i] * 1000);
                    RawData.Add(new RawDatumItem_QWLT2()
                    {
                        Section = "PH_Max",
                        Current_mA_or_Mirror_Diagonal_Offset = p1_p2_Currents_mA[i],
                        MPD_Current_mA = MPD_P1_Curr_Max[i] * 1000,
                    });
                }
                double max_PhaseValue = 0;
                int max_mpd_index = 0;
                int min_mpd_index = 0;

                ArrayMath.GetMaxAndMinIndex(phase_max_1st_MPDList.ToArray(), out max_mpd_index, out min_mpd_index);  
                max_PhaseValue = phase_max_1st_Currents[max_mpd_index];

                //for (int i = 0; i < MPDList.Count - 2; i++)
                //{
                //    if (MPDList[i] > MPDList[i + 1])
                //    {
                //        max_PhaseValue = Current[i];
                //    }
                //}
                string resut = string.Empty;
                if (max_PhaseValue > 0 && max_PhaseValue != 0)
                {
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                    resut = $"PH1=[{Math.Abs(max_PhaseValue)}]mA & PH2=[0]mA";
                    this.RawData.PH_Max_1 = Math.Abs(max_PhaseValue);
                    this.RawData.PH_Max_2 = 0;
                    this.RawDataMenu.PH_Max_1 = Math.Abs(max_PhaseValue);
                    this.RawDataMenu.PH_Max_2 = 0;
                    this.Log_Global($"PH_Max 1st result = {resut}");
                }
                else if (max_PhaseValue < 0 && max_PhaseValue != 0)
                {
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                    resut = $"PH1=[0]mA & PH2=[{Math.Abs(max_PhaseValue)}]mA";
                    this.RawData.PH_Max_1 = 0;
                    this.RawData.PH_Max_2 = Math.Abs(max_PhaseValue);
                    this.RawDataMenu.PH_Max_1 = 0;
                    this.RawDataMenu.PH_Max_2 = Math.Abs(max_PhaseValue);
 
                    this.Log_Global($"PH_Max 1st result = {resut}");
                }
                else
                {
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                    resut = $"PH1=[{Math.Abs(max_PhaseValue)}]mA & PH2=[{Math.Abs(max_PhaseValue)}]mA";
                    this.RawData.PH_Max_1 = 0;
                    this.RawData.PH_Max_2 = Math.Abs(max_PhaseValue);
                    this.RawDataMenu.PH_Max_1 = 0;
                    this.RawDataMenu.PH_Max_2 = Math.Abs(max_PhaseValue);

                    this.Log_Global($"PH_Max 1st result = {resut}");
                }


                this.RawData.Section = "PH_Max";
                this.RawData.Driver_mA = $"{double.Parse(array_p1_p2_sweep_setting[0])}-{double.Parse(array_p1_p2_sweep_setting[2])} step[{double.Parse(array_p1_p2_sweep_setting[1])}]";
                this.RawData.resut = resut;
                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                this.RawDataMenu.Add(RawData);
                #endregion
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消 QWLT2 ");
                    return;
                }

                this.Log_Global($"-----LP-----");

                #region LP
                this.RawData = new RawData_QWLT2();
                this.RawData.TestStepStartTime = DateTime.Now;

                var LParray = this.TestRecipe.LP_mA.Split(',');
                if (LParray.Length != 3)
                {
                    this.Log_Global("[LP_mA] Parameter error");
                    return;
                }

                List<double> lp_current_list = new List<double>();
                List<double> lp_vs_mpd_current_list = new List<double>();
                double[] LPCurrentarray = ArrayMath.CalculateArray(double.Parse(LParray[0]), double.Parse(LParray[2]), double.Parse(LParray[1]));

                lp_current_list.AddRange(LPCurrentarray);

                if (true)
                {
                    //double mpd_measureCurrentRange_mA = 1;

                    //switch (MPD.Name.ToUpper())
                    //{
                    //    case "MPD1":
                    //        mpd_measureCurrentRange_mA = 20;
                    //        break;
                    //    case "MPD2":
                    //        mpd_measureCurrentRange_mA = 1;
                    //        break;
                    //}


                    MPD.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(mpdsourceVoltage_V, mpdcomplianceCurrent_mA, mpd_measureCurrentRange_mA);

                   
                    for (int i = 0; i < LPCurrentarray.Length; i++)
                    {
                        LP.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(LPCurrentarray[i], complianceVoltage_V);
                        Thread.Sleep(1);

                        lp_vs_mpd_current_list.Add(MPD.ReadCurrent_A());

                    }

                
                    LP.Reset();
                 
                    MPD.Reset();

                }

                else
                {
                    LP.Reset();
                    MPD.Reset();
                    Thread.Sleep(10 * 1000);
      
                    LP.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(LParray[0]), float.Parse(LParray[1]), float.Parse(LParray[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                    MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V,(float) mpd_measureCurrentRange_mA, LPCurrentarray.Length, sourceDelay_s, ApertureTime_s, true);

                    LP.TriggerOutputOn = true;
                    MPD.TriggerOutputOn = true;

                    var slavers_LP = new PXISourceMeter_4143[] { MPD };
                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(LP, slavers_LP);
                    Merged_PXIe_4143.Trigger(LP, slavers_LP/*, LPCurrentarray.Length, timeout_ms_sweep*/);

                    var LP_Curr = LP.Fetch_MeasureVals(LPCurrentarray.Length, timeout_ms_fetchdata).CurrentMeasurements;
                    var MPD_LP_Curr = MPD.Fetch_MeasureVals(LPCurrentarray.Length, timeout_ms_fetchdata).CurrentMeasurements;

                 
                    lp_vs_mpd_current_list.AddRange(MPD_LP_Curr);
                }


                for (int i = 0; i < lp_vs_mpd_current_list.Count; i++)
                {
                    lp_vs_mpd_current_list[i] = lp_vs_mpd_current_list[i] * -1;
                }

                for (int i = 0; i < LPCurrentarray.Length; i++)
                {
                    RawData.Add(new RawDatumItem_QWLT2()
                    {
                        Section = "LP",
                        Current_mA_or_Mirror_Diagonal_Offset = lp_current_list[i],
                        //Current_mA_or_Mirror_Diagonal_Offset = lp_current_list[i] * 1000,
                        MPD_Current_mA = lp_vs_mpd_current_list[i] * 1000,
                    });
                }

                var LP_der_1st = ArrayMath.CalculateFirstDerivate(lp_current_list.ToArray(), lp_vs_mpd_current_list.ToArray());
                //var LP_der_2nd = ArrayMath.CalculateFirstDerivate(LP_Curr, LP_der_1st);

                //Array.Sort(LP_der_2nd);
                int max_index = 0;
                int min_index = 0;

                ArrayMath.GetMaxAndMinIndex(LP_der_1st, out max_index, out min_index);

                var LP_Current_1 = lp_current_list[max_index];
                var LP_Current_2 = lp_current_list[min_index];

                var LP_value = (LP_Current_1 + LP_Current_2) / 2.0;

                LP.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(LP_value , complianceVoltage_V);


                this.RawData.Section = "LP";
                this.RawData.Driver_mA = $"{double.Parse(array_p1_p2_sweep_setting[0])}-{double.Parse(array_p1_p2_sweep_setting[2])} step[{double.Parse(array_p1_p2_sweep_setting[1])}]";
                this.RawData.resut = $"LP_value=[{LP_value * 1000}]mA";
                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                this.RawDataMenu.Add(RawData);
                this.RawDataMenu.LP = LP_value  ;

                this.Log_Global($"LP result = [{LP_value  }]mA");
                #endregion
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消 QWLT2 ");
                    return;
                }
                this.Log_Global($"-----PH_Max_Second-----");

                #region PH_Max_Second
                this.RawData = new RawData_QWLT2();
                this.RawData.TestStepStartTime = DateTime.Now;

                PH2.Reset();
                PH1.Reset();
                MPD.Reset();
        
                PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);

                MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V, (float)mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                PH1.TriggerOutputOn = true;
                MPD.TriggerOutputOn = true;

                var slavers_Max_Sec = new PXISourceMeter_4143[] { MPD };
                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH1, slavers_Max_Sec);
                Merged_PXIe_4143.Trigger(PH1, slavers_Max_Sec/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                //var PH1_Curr_Max_Sec = PH1.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;
                var MPD_P1_Curr_Max_Sec = MPD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;

                PH2.Reset();
                PH1.Reset();
                MPD.Reset();
                PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);

                PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                MPD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V, (float)mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                PH2.TriggerOutputOn = true;
                MPD.TriggerOutputOn = true;

                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH2, slavers_Max_Sec);
                Merged_PXIe_4143.Trigger(PH2, slavers_Max_Sec/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                //var PH2_Curr_Max_Sec = PH2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;
                var MPD_P2_Curr_Max_Sec = MPD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;

                for (int i = 0; i < MPD_P1_Curr_Max_Sec.Length; i++)
                {
                    MPD_P1_Curr_Max_Sec[i] = MPD_P1_Curr_Max_Sec[i] * -1;
                }

                for (int i = 0; i < MPD_P2_Curr_Max_Sec.Length; i++)
                {
                    MPD_P2_Curr_Max_Sec[i] = MPD_P2_Curr_Max_Sec[i] * -1;
                }

                List<double> phase_max_2nd_Currents = new List<double>();
                List<double> phase_max_2nd_MPDList = new List<double>();
                //p2取反放前面
                for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
                {
                    phase_max_2nd_Currents.Add(-p1_p2_Currents_mA[i]);
                    phase_max_2nd_MPDList.Add(MPD_P2_Curr_Max_Sec[i] * 1000);
                    RawData.Add(new RawDatumItem_QWLT2()
                    {
                        Section = "PH_Max_Sec",
                        Current_mA_or_Mirror_Diagonal_Offset = -p1_p2_Currents_mA[i],
                        MPD_Current_mA = MPD_P2_Curr_Max_Sec[i] * 1000,
                    });
                }
                for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
                {
                    phase_max_2nd_Currents.Add(p1_p2_Currents_mA[i]);
                    phase_max_2nd_MPDList.Add(MPD_P1_Curr_Max_Sec[i] * 1000);
                    RawData.Add(new RawDatumItem_QWLT2()
                    {
                        Section = "PH_Max_Sec",
                        Current_mA_or_Mirror_Diagonal_Offset = p1_p2_Currents_mA[i],
                        MPD_Current_mA = MPD_P1_Curr_Max_Sec[i] * 1000,
                    });
                }
                double max_PhaseValue_final = 0;
                int max_mpd_index_final = 0;
                int min_mpd_index_final = 0;

                ArrayMath.GetMaxAndMinIndex(phase_max_2nd_MPDList.ToArray(), out max_mpd_index_final, out min_mpd_index_final);
                max_PhaseValue_final = phase_max_2nd_Currents[max_mpd_index_final];



                string resut_Sec = string.Empty;
                if (max_PhaseValue_final > 0 && max_PhaseValue_final != 0)
                {
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue_final), complianceVoltage_V);
                    resut_Sec = $"PH1=[{Math.Abs(max_PhaseValue_final)}]mA & PH2=[0]mA";
                    this.RawData.PH_Max_Sec_1 = Math.Abs(max_PhaseValue_final);
                    this.RawData.PH_Max_Sec_2 = 0;
                    this.RawDataMenu.PH_Max_Sec_1 = Math.Abs(max_PhaseValue_final);
                    this.RawDataMenu.PH_Max_Sec_2 = 0;
                    this.Log_Global($"[PH_Max] 2nd result {resut_Sec}");
                }
                else if (max_PhaseValue_final < 0 && max_PhaseValue_final != 0)
                {
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue_final), complianceVoltage_V);
                    resut_Sec = $"PH1=[0]mA & PH2=[{Math.Abs(max_PhaseValue_final)}]mA";
                    this.RawData.PH_Max_Sec_1 = 0;
                    this.RawData.PH_Max_Sec_2 = Math.Abs(max_PhaseValue_final);
                    this.RawDataMenu.PH_Max_Sec_1 = 0;
                    this.RawDataMenu.PH_Max_Sec_2 = Math.Abs(max_PhaseValue_final);
                    this.Log_Global($"[PH_Max] 2nd result {resut_Sec}");
                }
                else
                {
                    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue_final), complianceVoltage_V);
                    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue_final), complianceVoltage_V);
                    resut_Sec = $"PH1=[0]mA & PH2=[{Math.Abs(max_PhaseValue_final)}]mA";
                    this.RawData.PH_Max_Sec_1 = 0;
                    this.RawData.PH_Max_Sec_2 = Math.Abs(max_PhaseValue_final);
                    this.RawDataMenu.PH_Max_Sec_1 = 0;
                    this.RawDataMenu.PH_Max_Sec_2 = Math.Abs(max_PhaseValue_final);
                    this.Log_Global($"[PH_Max] 2nd result {resut_Sec}");
                }


                this.RawData.Section = "PH_Max_Sec";
                this.RawData.Driver_mA = $"{double.Parse(array_p1_p2_sweep_setting[0])}-{double.Parse(array_p1_p2_sweep_setting[2])} step[{double.Parse(array_p1_p2_sweep_setting[1])}]";
                this.RawData.resut = resut_Sec;
                this.RawData.TestStepEndTime = DateTime.Now;
                this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                this.RawDataMenu.Add(RawData);
                #endregion
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消 QWLT2 ");
                    return;
                }

                #region 打印
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string defaultFileName = string.Concat(@"QWLT2_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                var finalFileName = $@"{path}\{defaultFileName}";

                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine($"{MaskName}");//"DO721");
                    sw.WriteLine($"CH51 SMSR[dB]=****");
                    sw.WriteLine();
                    sw.WriteLine($"Gain[mA]= 120 mA");
                    sw.WriteLine($"LasPhase[mA]= {LP_value }mA");
                    sw.WriteLine($"Mirror1[mA]= {m1_mid_slope_val}mA");
                    sw.WriteLine($"Mirror2[mA]= {m2_mid_slope_val}mA");
                    sw.WriteLine($"Phase1[mA]= {this.RawDataMenu.PH_Max_Sec_1}mA");
                    sw.WriteLine($"Phase2[mA]= {this.RawDataMenu.PH_Max_Sec_2}mA");
                    sw.WriteLine($"SOA1[mA]= 50 mA");
                    sw.WriteLine($"SOA2[mA]= 40 mA");
                    sw.WriteLine($"MZM1 Bias[V]= {bias1} V");
                    sw.WriteLine($"MZM2 Bias[V]= {bias2} V");
                    sw.WriteLine($"MPD1 Bias[V]= {mpd1} V");
                    sw.WriteLine($"MPD2 Bias[V]= {mpd2} V");
                    sw.WriteLine($"MZM En=True");
                    sw.WriteLine($"MPD En=True");
                    sw.WriteLine();
                    sw.WriteLine($"PH_Halfway:"); 
                    sw.WriteLine($"current[mA],MPD[mA]");
                    for (int i = 0; i < ph_driving_currents_mA.Count; i++)
                    {
                        sw.WriteLine($"{ph_driving_currents_mA[i]}, {mpd_feedback_currents_mA[i]}");
                    }
                    sw.WriteLine();
                    sw.WriteLine($"MIRROR:");
                    sw.WriteLine($"current[mA],MPD[mA]");
                    for (int i = 0; i < M1CurrentArray.Length; i++)
                    {
                        sw.WriteLine($"{offsetArray[i]}, {MPD_Curr[i] * 1000}");
                    }
                    sw.WriteLine();
                    sw.WriteLine($"PH_Max:");
                    sw.WriteLine($"current[mA],MPD[mA]");
                    for (int i = 0; i < phase_max_1st_Currents.Count; i++)
                    {
                        sw.WriteLine($"{phase_max_1st_Currents[i]}, {phase_max_1st_MPDList[i]}");
                    }
                    sw.WriteLine();
                    sw.WriteLine($"LP:");
                    sw.WriteLine($"current[mA],MPD[mA]");
                    for (int i = 0; i < LPCurrentarray.Length; i++)
                    {
                        sw.WriteLine($"{lp_current_list[i] }, {lp_vs_mpd_current_list[i] * 1000}");
                    }
                    sw.WriteLine();
                    sw.WriteLine($"PH_Max_Sec:");
                    sw.WriteLine($"current[mA],MPD[mA]");
                    for (int i = 0; i < phase_max_2nd_Currents.Count; i++)
                    {
                        sw.WriteLine($"{phase_max_2nd_Currents[i]}, {phase_max_2nd_MPDList[i]}");
                    }
                    //sw.WriteLine($"data:");
                    //sw.WriteLine();
                    //for (int i = 0; i < GainCurrents_mA.Length; i++)
                    //{
                    //    sw.WriteLine($"{DriveCurrent[i] * 1000}, {DriveVoltage[i] * 1000},{PDCurrent[i] * 1000},{MPD1Current[i]},{MPD2Current[i]}");
                    //}

                }

                #endregion


                //20241121 WHB增加图片存储功能
                string imagePath = Path.Combine(path, $@"..\QWLT2_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
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
                this.Log_Global($"结束测试!");
            }
        }
    }
}