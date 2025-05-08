using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     ("TestCalculator_MTuning")]


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
    [ConfigurableInstrument("ScpiOsa", "OSA", "OSA")]
    [ConfigurableInstrument("OSA_AQ67370", "OSA_6370", "OSA_6370")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]

    #endregion
    public class TestModule_MTuning : TestModuleBase
    {

        public TestModule_MTuning() : base() { }

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

        ScpiOsa OSA_86142B { get { return (ScpiOsa)this.ModuleResource["OSA"]; } }
        OSA_AQ67370 OSA_6370 { get { return (OSA_AQ67370)this.ModuleResource["OSA_6370"]; } }
        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }

        #endregion

        TestRecipe_MTuning TestRecipe { get; set; }
        RawData_MTuning RawData { get; set; }
        QWLT2_TestDta qWLT2_TestDta { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_MTuning);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_MTuning();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_MTuning>(testRecipe);
        }

        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            _dutStreamData = dutStreamData;
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

        int retestcount = 5;//最大测试次数

        IDeviceStreamDataBase _dutStreamData;
        string SerialNumber { get; set; }
        public override void Run(CancellationToken token)
        {
            try
            {
                string path = Application.StartupPath + $@"\Data\MirrorTuning\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\MirrorTuning";
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
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

                double qwlt_ph1_driving_crruent_mA = 0;
                double qwlt_ph2_driving_crruent_mA = 2.5;
                double original_Ph1_current_mA = 0.0;
                double original_Ph2_current_mA = 0.0;

                double LPCurrent = 0.0;
                double MPD2Voltage = 0.0;
                //从QWLT拿到所需通道家电数值
                //LP,MIRROR1,MIRROR2,PH1,PH2,MPD1,MPD2,BIAS1,BIAS2
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

                    original_Ph1_current_mA = qWLT2_TestDta.PH1;
                    original_Ph2_current_mA = qWLT2_TestDta.PH2;
                    LPCurrent = qWLT2_TestDta.LP;
                    MPD2Voltage = qWLT2_TestDta.MPD2;
                }
                else
                {
                    original_Ph1_current_mA = 6;
                    original_Ph2_current_mA = 0;
                    LPCurrent = 4.75;
                    MPD2Voltage = -2;
                    //临时设定为定制  以下数据需要从qwlt2获取
                    GAIN.AssignmentMode_Current(120, 2.5);
                    LP.AssignmentMode_Current(LPCurrent, 2.5);
                    MIRROR1.AssignmentMode_Current(9.9, 1.6);
                    MIRROR2.AssignmentMode_Current(10.1, 1.6);

                    SOA1.AssignmentMode_Current(50, 2.5);
                    SOA2.AssignmentMode_Current(40, 2.5);

                    PH1.AssignmentMode_Current(original_Ph1_current_mA, 2.5);
                    PH2.AssignmentMode_Current(original_Ph2_current_mA, 2.5);

                    MPD1.AssignmentMode_Voltage(MPD2Voltage, 20);
                    MPD2.AssignmentMode_Voltage(MPD2Voltage, 20);
                    BIAS1.AssignmentMode_Voltage(-3, 20);
                    BIAS2.AssignmentMode_Voltage(-3, 20);

                }

                this.Log_Global($"开始测试!");

                MTuningData mTuningData = new MTuningData();

                var M1M2_Start_mA = this.TestRecipe.M1M2_Start_mA;
                var M1M2_Stop_mA = this.TestRecipe.M1M2_Stop_mA;
                var CurrentPoint = this.TestRecipe.StepCount - 1;
                List<double> currentlist = new List<double>();
                //List<double> currentlistM1 = new List<double>();
                //List<double> currentlistM2 = new List<double>();

                for (int i = 0; i <= CurrentPoint; i++)
                {
                    var current = M1M2_Stop_mA * Math.Pow((i / CurrentPoint), 2);
                    currentlist.Add(current);
                    //if(i%2==0)
                    //{
                    //    currentlistM1.Add(0);
                    //    currentlistM2.Add(0);
                    //}
                    //else
                    //{
                    //    currentlistM1.Add(4.3);
                    //    currentlistM2.Add(37);
                    //}
                }

                DataBook<double, DataBook<double, double>> MirrorTuning =
                    new DataBook<double, DataBook<double, double>>();
                List<MTuningDataItem> Aggregate = new List<MTuningDataItem>();

                bool isPass = true;
                int solution = 0;
                int retest = 0; //20240718 波长计增加重测

                switch (solution)
                {
                    case 0:
                        {

                            for (int m1_current_index = 0; m1_current_index < currentlist.Count; m1_current_index++)
                            {
                                for (int m2_current_index = 0; m2_current_index < currentlist.Count; m2_current_index++)
                                {
                                    MTuningDataItem Single = new MTuningDataItem();
                                    Single.Index = m1_current_index * currentlist.Count + m2_current_index;
                                    Single.Mirror1_Current_mA = currentlist[m1_current_index];
                                    Single.Mirror2_Current_mA = currentlist[m2_current_index];
                                    Aggregate.Add(Single);
                                }
                            }

                            Aggregate.Sort((item1, item2) => { return item1.MirrorMix.CompareTo(item2.MirrorMix); });

                            List<double> mirror1_sort_current = new List<double>();
                            List<double> mirror2_sort_current = new List<double>();

                            for (int i = 0; i < Aggregate.Count; i++)
                            {
                                mirror1_sort_current.Add(Aggregate[i].Mirror1_Current_mA);
                                mirror2_sort_current.Add(Aggregate[i].Mirror2_Current_mA);
                            }

                            //mirror1_sort_current.Add(0);
                            //mirror2_sort_current.Add(0);
                            string mirror2_trigger_signal_name = "";
                            WavelengthAndPower waveandpower = new WavelengthAndPower();
                            for (retest = 0; retest <= retestcount; retest++)
                            {
                                if (retest == retestcount)
                                {
                                    throw new Exception($"波长计获取结果数量[{retestcount}]次重试, 均与需求目标数量不等.");
                                }

                                FWM8612.RST();
                                FWM8612.SetTriggerSource(Source.EXTernal);
                                MIRROR1.Reset();
                                MIRROR2.Reset();


                                mirror2_trigger_signal_name = MIRROR2.BuildTermialName();
                                MIRROR2.SetDefaultTermialName(mirror2_trigger_signal_name);
                                S_6683H.TrigTerminalsStart(mirror2_trigger_signal_name);

                                MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, mirror2_sort_current.ToArray(),
                                  this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, mirror1_sort_current.ToArray(),
                                   this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);


                                MIRROR1.TriggerOutputOn = true;
                                MIRROR2.TriggerOutputOn = true;


                                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1 });

                                FWM8612.EXTernalStart2();

                                this.Log_Global($"开始Trigger扫描!");

                                Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1 });

                                
                                if (FWM8612.FethEXTernalData(SerialNumber, mirror2_sort_current.Count, out waveandpower))//波长计
                                {
                                    break;
                                }
                                else
                                {
                                    this.Log_Global($"波长计获取结果数量[{waveandpower.Power.Count}]与需求目标数量[{mirror2_sort_current.Count}]不等");
                                    FWM8612.EXTernalStop();
                                    S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);

                                }

                            }

                            this.Log_Global($"波长数据取回完成!");
                            // var   result_mirroe1 = MIRROR1.Fetch_MeasureVals(Aggregate.Count, 100 * 1000.0);
                            // var   result_mirroe2 = MIRROR2.Fetch_MeasureVals(Aggregate.Count, 100 * 1000.0);

                            FWM8612.EXTernalStop();
                            S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);

                            for (int i = 0; i < Aggregate.Count; i++)
                            {
                                Aggregate[i].Wavelength_nm = waveandpower.Wavelength[i];
                                Aggregate[i].Power_dBM = waveandpower.Power[i];
                            }
                            Aggregate.Sort((item1, item2) => { return item1.Index.CompareTo(item2.Index); });

                            if (token.IsCancellationRequested)
                            {
                                this.Log_Global("用户取消Mirror tuning.");
                                return;
                            }

                            string defaultFileName_org = string.Concat(@"MirrorTuning_补点前_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                            var finalFileName_org = $@"{path}\{defaultFileName_org}";

                            using (StreamWriter sw = new StreamWriter(finalFileName_org, false, Encoding.GetEncoding("gb2312")))
                            {
                                string heade = "Mirror1_Current_mA,Mirror2_Current_mA,Wavelength_nm,Power_dBM";

                                sw.WriteLine(heade);
                                sw.WriteLine("[RAW DATA]");

                                for (int i = 0; i < Aggregate.Count; i++)
                                {
                                    sw.WriteLine($"{Aggregate[i].Mirror1_Current_mA},{Aggregate[i].Mirror2_Current_mA}," +
                                        $"{Aggregate[i].Wavelength_nm},{ Aggregate[i].Power_dBM}");
                                }
                            }


                            const double phase_start_current_mA = 0;
                            const double phase_stop_current_mA = 10;
                            const double phase_step_current_mA = 1;
                            double[] p1_p2_Currents_mA = ArrayMath.CalculateArray(phase_start_current_mA, phase_stop_current_mA, phase_step_current_mA);

                            const double laserphase_start_current_mA = 0;
                            const double laserphase_stop_current_mA = 20;
                            const double laserphase_step_current_mA = 1;
                            double[] LPCurrentarray = ArrayMath.CalculateArray(laserphase_start_current_mA, laserphase_stop_current_mA, laserphase_step_current_mA);

                            List<double> m1fullcurrentAlgorithm2 = new List<double>();
                            List<double> m2fullcurrentAlgorithm2 = new List<double>();
                            List<double> ph1fullcurrentAlgorithm2 = new List<double>();
                            List<double> ph2fullcurrentAlgorithm2 = new List<double>();
                            for (int i = 0; i < Aggregate.Count; i++)
                            {
                                if (Aggregate[i].Wavelength_nm < 1500)
                                {
                                    //phase scan  Sequence
                                    //ph1=0,扫ph2
                                    for (int j = 0; j < p1_p2_Currents_mA.Length; j++)
                                    {
                                        m1fullcurrentAlgorithm2.Add(Aggregate[i].Mirror1_Current_mA);
                                        m2fullcurrentAlgorithm2.Add(Aggregate[i].Mirror2_Current_mA);
                                        ph1fullcurrentAlgorithm2.Add(0);// (p1_p2_Currents_mA[j]);
                                        ph2fullcurrentAlgorithm2.Add(p1_p2_Currents_mA[j]);

                                    }
                                    //ph2=0,扫ph1
                                    for (int j = 0; j < p1_p2_Currents_mA.Length; j++)
                                    {
                                        m1fullcurrentAlgorithm2.Add(Aggregate[i].Mirror1_Current_mA);
                                        m2fullcurrentAlgorithm2.Add(Aggregate[i].Mirror2_Current_mA);
                                        ph1fullcurrentAlgorithm2.Add(p1_p2_Currents_mA[j]);
                                        ph2fullcurrentAlgorithm2.Add(0);// (p1_p2_Currents_mA[m]);
                                    }
                                }
                            }
                            
                            if (m1fullcurrentAlgorithm2.Count <= 100000)
                            {
                                List<double> total_MPD_CurrAlgorithm2 = new List<double>();
                                WavelengthAndPower total_waveandpowerAlgorithm2 = new WavelengthAndPower();
                                double mpd2_comp_current_mA = 20.0;
                                double mpd2_source_delay_s = 0.001;
                                int seed = 10000;
                                int majorCount = m1fullcurrentAlgorithm2.Count / seed;
                                int minorCount = m1fullcurrentAlgorithm2.Count % seed;
                                for (int i = 0; i <= majorCount; i++)
                                {
                                    double[] tempMPD_CurrAlgorithm2 = new double[1];
                                    WavelengthAndPower temp_waveandpowerAlgorithm2 = new WavelengthAndPower();

                                    for (retest = 0; retest <= retestcount; retest++)
                                    {
                                        if (retest == retestcount)
                                        {
                                            throw new Exception($"波长计获取结果数量[{retestcount}]次重试, 均与需求目标数量不等.");
                                        }
                                        List<double> sub_m1fullcurrentAlgorithm2 = new List<double>();
                                        List<double> sub_m2fullcurrentAlgorithm2 = new List<double>();
                                        List<double> sub_ph1fullcurrentAlgorithm2 = new List<double>();
                                        List<double> sub_ph2fullcurrentAlgorithm2 = new List<double>();

                                        if (i != majorCount)
                                        {
                                            this.Log_Global($"补点扫描总点数{ m1fullcurrentAlgorithm2.Count}..现在进行第{i}次分包扫描,此包范围索引为{i * seed}~{i * seed + seed}");
                                            sub_m1fullcurrentAlgorithm2 = m1fullcurrentAlgorithm2.GetRange(i * seed, seed);
                                            sub_m2fullcurrentAlgorithm2 = m2fullcurrentAlgorithm2.GetRange(i * seed, seed);
                                            sub_ph1fullcurrentAlgorithm2 = ph1fullcurrentAlgorithm2.GetRange(i * seed, seed);
                                            sub_ph2fullcurrentAlgorithm2 = ph2fullcurrentAlgorithm2.GetRange(i * seed, seed);
                                        }
                                        else
                                        {
                                            this.Log_Global($"补点扫描总点数{ m1fullcurrentAlgorithm2.Count}..现在进行最后一次分包扫描,此包范围索引为{majorCount * seed}~{majorCount * seed + minorCount}");
                                            sub_m1fullcurrentAlgorithm2 = m1fullcurrentAlgorithm2.GetRange(majorCount * seed, minorCount);
                                            sub_m2fullcurrentAlgorithm2 = m2fullcurrentAlgorithm2.GetRange(majorCount * seed, minorCount);
                                            sub_ph1fullcurrentAlgorithm2 = ph1fullcurrentAlgorithm2.GetRange(majorCount * seed, minorCount);
                                            sub_ph2fullcurrentAlgorithm2 = ph2fullcurrentAlgorithm2.GetRange(majorCount * seed, minorCount);
                                        }

                                        if (token.IsCancellationRequested)
                                        {
                                            this.Log_Global("用户取消Mirror tuning.");
                                            return;
                                        }

                                        MIRROR1.Reset();
                                        MIRROR2.Reset();
                                        PH1.Reset();
                                        PH2.Reset();
                                        MPD2.Reset();

                                        FWM8612.RST();
                                        //Thread.Sleep(200);
                                        FWM8612.SetTriggerSource(Source.EXTernal);
                                        //Thread.Sleep(200);

                                        MIRROR2.SetDefaultTermialName(mirror2_trigger_signal_name);
                                        S_6683H.TrigTerminalsStart(mirror2_trigger_signal_name);

                                        MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m1fullcurrentAlgorithm2.ToArray(),
                                                                                        this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                        MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m2fullcurrentAlgorithm2.ToArray(),
                                                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                        PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)phase_start_current_mA, (float)phase_stop_current_mA, sub_ph1fullcurrentAlgorithm2.ToArray(),
                                                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                        PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)phase_start_current_mA, (float)phase_stop_current_mA, sub_ph2fullcurrentAlgorithm2.ToArray(),
                                                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                        MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)MPD2Voltage, (float)mpd2_comp_current_mA, sub_ph2fullcurrentAlgorithm2.Count,
                                            mpd2_source_delay_s, this.TestRecipe.ApertureTime_s, true);



                                        MIRROR1.TriggerOutputOn = true;
                                        MIRROR2.TriggerOutputOn = true;
                                        PH1.TriggerOutputOn = true;
                                        PH2.TriggerOutputOn = true;
                                        MPD2.TriggerOutputOn = true;

                                        Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, PH1, PH2, MPD2 });

                                        FWM8612.EXTernalStart2();
                                        //Thread.Sleep(200);
                                        Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, PH1, PH2, MPD2 });

                                        tempMPD_CurrAlgorithm2 = MPD2.Fetch_MeasureVals(sub_m1fullcurrentAlgorithm2.Count, 100 * 1000).CurrentMeasurements;

                                        if (FWM8612.FethEXTernalData(SerialNumber, sub_m1fullcurrentAlgorithm2.Count, out temp_waveandpowerAlgorithm2))//波长计
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            this.Log_Global($"波长计获取结果数量[{temp_waveandpowerAlgorithm2.Power.Count}]与需求目标数量[{sub_m1fullcurrentAlgorithm2.Count}]不等");
                                            FWM8612.EXTernalStop();
                                            S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);

                                        }
                                    }

                                    FWM8612.EXTernalStop();
                                    //Thread.Sleep(200);
                                    S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);


                                    total_MPD_CurrAlgorithm2.AddRange(tempMPD_CurrAlgorithm2);
                                    total_waveandpowerAlgorithm2.Power.AddRange(temp_waveandpowerAlgorithm2.Power);
                                    total_waveandpowerAlgorithm2.Wavelength.AddRange(temp_waveandpowerAlgorithm2.Wavelength);

                                }

                                int count = 0;
                                int index = 0;
                                double max_MPD_CurrAlgorithm2 = 0;
                                for (int i = 0; i < total_MPD_CurrAlgorithm2.Count; i++)
                                {
                                    if (Math.Abs(total_MPD_CurrAlgorithm2[i]) > max_MPD_CurrAlgorithm2)
                                    {
                                        max_MPD_CurrAlgorithm2 = Math.Abs(total_MPD_CurrAlgorithm2[i]);
                                        index = i;
                                    }
                                    count++;
                                    if (count == p1_p2_Currents_mA.Length * 2)
                                    {
                                        max_MPD_CurrAlgorithm2 = 0;
                                        count = 0;
                                        var m1current = m1fullcurrentAlgorithm2[index];
                                        var m2current = m2fullcurrentAlgorithm2[index];
                                        var Wavelength = total_waveandpowerAlgorithm2.Wavelength[index];
                                        var power = total_waveandpowerAlgorithm2.Power[index];
                                        for (int j = 0; j < Aggregate.Count; j++)
                                        {
                                            if (Aggregate[j].Mirror1_Current_mA == m1current &&
                                                Aggregate[j].Mirror2_Current_mA == m2current)
                                            {
                                                Aggregate[j].Wavelength_nm = Wavelength;
                                                Aggregate[j].Power_dBM = power;
                                                break;
                                            }
                                        }
                                    }
                                }
                                PH1.AssignmentMode_Current(qwlt_ph1_driving_crruent_mA, 2.5);
                                PH2.AssignmentMode_Current(qwlt_ph2_driving_crruent_mA, 2.5);

                                #region LP Scan 不使用
                                //{
                                //    //LP  Scan
                                //    List<double> m1fullcurrentAlgorithm3 = new List<double>();
                                //    List<double> m2fullcurrentAlgorithm3 = new List<double>();
                                //    List<double> LPfullcurrentAlgorithm3 = new List<double>();
                                //    for (int i = 0; i < Aggregate.Count; i++)
                                //    {
                                //        if (Aggregate[i].Wavelength_nm < 1500)
                                //        {
                                //            for (int j = 0; j < LPCurrentarray.Length; j++)
                                //            {
                                //                m1fullcurrentAlgorithm3.Add(Aggregate[i].Mirror1_Current_mA);
                                //                m2fullcurrentAlgorithm3.Add(Aggregate[i].Mirror2_Current_mA);
                                //                LPfullcurrentAlgorithm3.Add(LPCurrentarray[j]);
                                //            }
                                //        }
                                //    }
                                //    if (LPfullcurrentAlgorithm3.Count > seed)
                                //    {
                                //        List<double> total_MPD_CurrAlgorithm3 = new List<double>();
                                //        WavelengthAndPower total_waveandpowerAlgorithm3 = new WavelengthAndPower();
                                //        majorCount = m1fullcurrentAlgorithm3.Count / seed;
                                //        minorCount = m1fullcurrentAlgorithm3.Count % seed;

                                //        List<double> sub_m1fullcurrentAlgorithm3 = new List<double>();
                                //        List<double> sub_m2fullcurrentAlgorithm3 = new List<double>();
                                //        List<double> sub_LPfullcurrentAlgorithm3 = new List<double>();

                                //        for (int i = 0; i <= majorCount; i++)
                                //        {
                                //            if (i != majorCount)
                                //            {
                                //                this.Log_Global($"LPase 补点扫描总点数{ m1fullcurrentAlgorithm3.Count}..现在进行第{i}次分包扫描,此包范围索引为{i * seed}~{i * seed + seed}");
                                //                sub_m1fullcurrentAlgorithm3 = m1fullcurrentAlgorithm3.GetRange(i * seed, seed);
                                //                sub_m2fullcurrentAlgorithm3 = m2fullcurrentAlgorithm3.GetRange(i * seed, seed);
                                //                sub_LPfullcurrentAlgorithm3 = LPfullcurrentAlgorithm3.GetRange(i * seed, seed);
                                //            }
                                //            else
                                //            {
                                //                this.Log_Global($"LP 补点扫描总点数{ m1fullcurrentAlgorithm3.Count}..现在进行最后一次分包扫描,此包范围索引为{majorCount * seed}~{majorCount * seed + minorCount}");
                                //                sub_m1fullcurrentAlgorithm3 = m1fullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                                //                sub_m2fullcurrentAlgorithm3 = m2fullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                                //                sub_LPfullcurrentAlgorithm3 = LPfullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                                //            }


                                //            MIRROR1.Reset();
                                //            MIRROR2.Reset();
                                //            LP.Reset();
                                //            MPD2.Reset();

                                //            FWM8612.RST();
                                //            Thread.Sleep(150);
                                //            FWM8612.SetTriggerSource(Source.EXTernal);
                                //            Thread.Sleep(150);

                                //            MIRROR2.SetDefaultTermialName(mirror2_trigger_signal_name);
                                //            S_6683H.TrigTerminalsStart(mirror2_trigger_signal_name);

                                //            MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m1fullcurrentAlgorithm3.ToArray(),
                                //                                                            this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //            MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m2fullcurrentAlgorithm3.ToArray(),
                                //                                                        this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //            LP.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)laserphase_start_current_mA, (float)laserphase_stop_current_mA, sub_LPfullcurrentAlgorithm3.ToArray(),
                                //                                                        this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //            MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)MPD2Voltage, (float)mpd2_comp_current_mA, sub_LPfullcurrentAlgorithm3.Count,
                                //                mpd2_source_delay_s, this.TestRecipe.ApertureTime_s, true);



                                //            MIRROR1.TriggerOutputOn = true;
                                //            MIRROR2.TriggerOutputOn = true;
                                //            LP.TriggerOutputOn = true;
                                //            MPD2.TriggerOutputOn = true;

                                //            Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                                //            FWM8612.EXTernalStart2();
                                //            Thread.Sleep(150);
                                //            Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                                //            var tempMPD_CurrAlgorithm3 = MPD2.Fetch_MeasureVals(sub_m1fullcurrentAlgorithm3.Count, 100 * 1000).CurrentMeasurements;
                                //            WavelengthAndPower temp_waveandpowerAlgorithm3 = FWM8612.FethEXTernalData(sub_m1fullcurrentAlgorithm3.Count);//波长计
                                //            FWM8612.EXTernalStop();
                                //            Thread.Sleep(150);
                                //            S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);


                                //            total_MPD_CurrAlgorithm3.AddRange(tempMPD_CurrAlgorithm3);
                                //            total_waveandpowerAlgorithm3.Power.AddRange(temp_waveandpowerAlgorithm3.Power);
                                //            total_waveandpowerAlgorithm3.Wavelength.AddRange(temp_waveandpowerAlgorithm3.Wavelength);

                                //        }
                                //        //{

                                //        //    this.Log_Global($"LP 补点扫描总点数{ m1fullcurrentAlgorithm3.Count}..现在进行最后一次分包扫描,此包范围索引为{majorCount * seed}~{majorCount * seed + minorCount}");
                                //        //    var sub_m1fullcurrentAlgorithm3 = m1fullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                                //        //    var sub_m2fullcurrentAlgorithm3 = m2fullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                                //        //    var sub_LPfullcurrentAlgorithm3 = LPfullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);

                                //        //    MIRROR1.Reset();
                                //        //    MIRROR2.Reset();
                                //        //    LP.Reset();
                                //        //    MPD2.Reset();

                                //        //    FWM8612.RST();
                                //        //    Thread.Sleep(150);
                                //        //    FWM8612.SetTriggerSource(Source.EXTernal);
                                //        //    Thread.Sleep(150);

                                //        //    MIRROR2.SetDefaultTermialName(name);
                                //        //    S_6683H.TrigTerminalsStart(name);

                                //        //    MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m1fullcurrentAlgorithm3.ToArray(),
                                //        //                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //        //    MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m2fullcurrentAlgorithm3.ToArray(),
                                //        //                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //        //    LP.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)lpstart, (float)lpstop, sub_LPfullcurrentAlgorithm3.ToArray(),
                                //        //                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //        //    MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)MPD2Voltage, (float)mpd2_comp_current_mA, sub_LPfullcurrentAlgorithm3.Count,
                                //        //        mpd2_source_delay_s, this.TestRecipe.ApertureTime_s, true);



                                //        //    MIRROR1.TriggerOutputOn = true;
                                //        //    MIRROR2.TriggerOutputOn = true;
                                //        //    LP.TriggerOutputOn = true;
                                //        //    MPD2.TriggerOutputOn = true;

                                //        //    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                                //        //    FWM8612.EXTernalStart2();
                                //        //    Thread.Sleep(150);
                                //        //    Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                                //        //    var tempMPD_CurrAlgorithm3 = MPD2.Fetch_MeasureVals(sub_m1fullcurrentAlgorithm3.Count, 100 * 1000).CurrentMeasurements;
                                //        //    WavelengthAndPower temp_waveandpowerAlgorithm3 = FWM8612.FethEXTernalData(sub_m1fullcurrentAlgorithm3.Count);//波长计
                                //        //    FWM8612.EXTernalStop();
                                //        //    Thread.Sleep(150);
                                //        //    S_6683H.TrigTerminalsStop(name);


                                //        //    total_MPD_CurrAlgorithm3.AddRange(tempMPD_CurrAlgorithm3);
                                //        //    total_waveandpowerAlgorithm3.Power.AddRange(temp_waveandpowerAlgorithm3.Power);
                                //        //    total_waveandpowerAlgorithm3.Wavelength.AddRange(temp_waveandpowerAlgorithm3.Wavelength);
                                //        //}
                                //        double max_MPD_CurrAlgorithm3 = 0;
                                //        for (int i = 0; i < total_MPD_CurrAlgorithm3.Count; i++)
                                //        {
                                //            if (Math.Abs(total_MPD_CurrAlgorithm3[i]) > max_MPD_CurrAlgorithm3)
                                //            {
                                //                max_MPD_CurrAlgorithm3 = Math.Abs(total_MPD_CurrAlgorithm3[i]);
                                //                index = i;
                                //            }
                                //            count++;
                                //            if (count == LPCurrentarray.Length)
                                //            {
                                //                max_MPD_CurrAlgorithm3 = 0;
                                //                count = 0;
                                //                var m1current = m1fullcurrentAlgorithm3[index];
                                //                var m2current = m2fullcurrentAlgorithm3[index];
                                //                var Wavelength = total_waveandpowerAlgorithm3.Wavelength[index];
                                //                var power = total_waveandpowerAlgorithm3.Power[index];
                                //                for (int j = 0; j < Aggregate.Count; j++)
                                //                {
                                //                    if (Aggregate[j].Mirror1_Current_mA == m1current &&
                                //                        Aggregate[j].Mirror2_Current_mA == m2current)
                                //                    {
                                //                        Aggregate[j].Wavelength_nm = Wavelength;
                                //                        Aggregate[j].Power_dBM = power;
                                //                        break;
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //    else if (LPfullcurrentAlgorithm3.Count > 1)
                                //    {
                                //        this.Log_Global($"LPase 补点扫描总点数{ m1fullcurrentAlgorithm3.Count}..");

                                //        MIRROR1.Reset();
                                //        MIRROR2.Reset();
                                //        LP.Reset();
                                //        MPD2.Reset();

                                //        FWM8612.RST();
                                //        Thread.Sleep(150);
                                //        FWM8612.SetTriggerSource(Source.EXTernal);
                                //        Thread.Sleep(150);

                                //        MIRROR2.SetDefaultTermialName(mirror2_trigger_signal_name);
                                //        S_6683H.TrigTerminalsStart(mirror2_trigger_signal_name);

                                //        MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, m1fullcurrentAlgorithm3.ToArray(),
                                //                                                        this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //        MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, m2fullcurrentAlgorithm3.ToArray(),
                                //                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //        LP.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)laserphase_start_current_mA, (float)laserphase_stop_current_mA, LPfullcurrentAlgorithm3.ToArray(),
                                //                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                                //        MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)MPD2Voltage, (float)mpd2_comp_current_mA, LPfullcurrentAlgorithm3.Count,
                                //            mpd2_source_delay_s, this.TestRecipe.ApertureTime_s, true);



                                //        MIRROR1.TriggerOutputOn = true;
                                //        MIRROR2.TriggerOutputOn = true;
                                //        LP.TriggerOutputOn = true;
                                //        MPD2.TriggerOutputOn = true;

                                //        Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                                //        FWM8612.EXTernalStart2();
                                //        Thread.Sleep(150);
                                //        Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                                //        var tempMPD_CurrAlgorithm3 = MPD2.Fetch_MeasureVals(m1fullcurrentAlgorithm3.Count, 100 * 1000).CurrentMeasurements;
                                //        WavelengthAndPower temp_waveandpowerAlgorithm3 = FWM8612.FethEXTernalData(m1fullcurrentAlgorithm3.Count);//波长计
                                //        FWM8612.EXTernalStop();
                                //        Thread.Sleep(150);
                                //        S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);


                                //        double max_MPD_CurrAlgorithm3 = 0;
                                //        for (int i = 0; i < tempMPD_CurrAlgorithm3.Length; i++)
                                //        {
                                //            if (Math.Abs(tempMPD_CurrAlgorithm3[i]) > max_MPD_CurrAlgorithm3)
                                //            {
                                //                max_MPD_CurrAlgorithm3 = Math.Abs(tempMPD_CurrAlgorithm3[i]);
                                //                index = i;
                                //            }
                                //            count++;
                                //            if (count == LPCurrentarray.Length)
                                //            {
                                //                max_MPD_CurrAlgorithm3 = 0;
                                //                count = 0;
                                //                var m1current = m1fullcurrentAlgorithm3[index];
                                //                var m2current = m2fullcurrentAlgorithm3[index];
                                //                var Wavelength = temp_waveandpowerAlgorithm3.Wavelength[index];
                                //                var power = temp_waveandpowerAlgorithm3.Power[index];
                                //                for (int j = 0; j < Aggregate.Count; j++)
                                //                {
                                //                    if (Aggregate[j].Mirror1_Current_mA == m1current &&
                                //                        Aggregate[j].Mirror2_Current_mA == m2current)
                                //                    {
                                //                        Aggregate[j].Wavelength_nm = Wavelength;
                                //                        Aggregate[j].Power_dBM = power;
                                //                        break;
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }

                                //    LP.AssignmentMode_Current(LPCurrent, 2.5);









                                //}
                                #endregion

                                if (token.IsCancellationRequested)
                                {
                                    this.Log_Global("用户取消Mirror tuning.");
                                    return;
                                }
                                Algorithm2_PhaseSweep(
                                    qwlt_ph1_driving_crruent_mA,
                                    qwlt_ph2_driving_crruent_mA,
                                    MPD2Voltage,
                                    M1M2_Start_mA,
                                    M1M2_Stop_mA,
                                    ref Aggregate,
                                    mirror2_trigger_signal_name,
                                    phase_start_current_mA,
                                    phase_stop_current_mA,
                                    p1_p2_Currents_mA,
                                    mpd2_comp_current_mA,
                                    mpd2_source_delay_s, token);
                                if (token.IsCancellationRequested)
                                {
                                    this.Log_Global("用户取消Mirror tuning.");
                                    return;
                                }
                                //Algorithm2_PhaseSweep(
                                //    qwlt_ph1_driving_crruent_mA,
                                //    qwlt_ph2_driving_crruent_mA,
                                //    MPD2Voltage,
                                //    M1M2_Start_mA,
                                //    M1M2_Stop_mA,
                                //    ref Aggregate,
                                //    mirror2_trigger_signal_name,
                                //    phase_start_current_mA,
                                //    phase_stop_current_mA,
                                //    p1_p2_Currents_mA,
                                //    mpd2_comp_current_mA,
                                //    mpd2_source_delay_s, token);
                                //if (token.IsCancellationRequested)
                                //{
                                //    this.Log_Global("用户取消Mirror tuning.");
                                //    return;
                                //}
                                //Algorithm2_PhaseSweep(
                                //   qwlt_ph1_driving_crruent_mA,
                                //   qwlt_ph2_driving_crruent_mA,
                                //   MPD2Voltage,
                                //   M1M2_Start_mA,
                                //   M1M2_Stop_mA,
                                //   ref Aggregate,
                                //   mirror2_trigger_signal_name,
                                //   phase_start_current_mA,
                                //   phase_stop_current_mA,
                                //   p1_p2_Currents_mA,
                                //   mpd2_comp_current_mA,
                                //   mpd2_source_delay_s, token);
                                //if (token.IsCancellationRequested)
                                //{
                                //    this.Log_Global("用户取消Mirror tuning.");
                                //    return;
                                //}
                                Algorithm3_LPhaseSweep(
                                    LPCurrent,
                                    MPD2Voltage,
                                    M1M2_Start_mA,
                                    M1M2_Stop_mA,
                                    ref Aggregate,
                                    laserphase_start_current_mA,
                                    laserphase_stop_current_mA,
                                     LPCurrentarray,
                                     mpd2_comp_current_mA,
                                     mpd2_source_delay_s, token
                                    );
                                if (token.IsCancellationRequested)
                                {
                                    this.Log_Global("用户取消Mirror tuning.");
                                    return;
                                }
                                //Algorithm3_LPhaseSweep(
                                //    LPCurrent,
                                //    MPD2Voltage,
                                //    M1M2_Start_mA,
                                //    M1M2_Stop_mA,
                                //    ref Aggregate,
                                //    laserphase_start_current_mA,
                                //    laserphase_stop_current_mA,
                                //     LPCurrentarray,
                                //     mpd2_comp_current_mA,
                                //     mpd2_source_delay_s, token
                                //    );
                                //if (token.IsCancellationRequested)
                                //{
                                //    this.Log_Global("用户取消Mirror tuning.");
                                //    return;
                                //}
                                //Algorithm3_LPhaseSweep(
                                //    LPCurrent,
                                //    MPD2Voltage,
                                //    M1M2_Start_mA,
                                //    M1M2_Stop_mA,
                                //    ref Aggregate,
                                //    laserphase_start_current_mA,
                                //    laserphase_stop_current_mA,
                                //     LPCurrentarray,
                                //     mpd2_comp_current_mA,
                                //     mpd2_source_delay_s, token
                                //    );
                                //if (token.IsCancellationRequested)
                                //{
                                //    this.Log_Global("用户取消Mirror tuning.");
                                //    return;
                                //}
                                //Algorithm3_LPhaseSweep(
                                //    LPCurrent,
                                //    MPD2Voltage,
                                //    M1M2_Start_mA,
                                //    M1M2_Stop_mA,
                                //    ref Aggregate,
                                //    laserphase_start_current_mA,
                                //    laserphase_stop_current_mA,
                                //     LPCurrentarray,
                                //     mpd2_comp_current_mA,
                                //     mpd2_source_delay_s, token
                                //    );
                                //if (token.IsCancellationRequested)
                                //{
                                //    this.Log_Global("用户取消Mirror tuning.");
                                //    return;
                                //}
                                if (this.TestRecipe.IsOSA)
                                {
                                    Algorithm4_86142BSweep(ref Aggregate, token);
                                    //Algorithm4_OSASweep(ref Aggregate, token);
                                }
                                if (token.IsCancellationRequested)
                                {
                                    this.Log_Global("用户取消Mirror tuning.");
                                    return;
                                }
                            }
                            else
                            {
                                isPass = false;
                                this.Log_Global("坏点数量过多，取消Mirror tuning 补点操作.");
                            }
                        }
                        break;
                    case 1:
                        {
                            //FWM8612.SetTriggerSource(Source.EXTernal);
                            for (int i = 0; i < currentlist.Count; i++)
                            {
                                string name = "";
                                double m1current = currentlist[i];
                                WavelengthAndPower waveandpower = new WavelengthAndPower();
                                for (retest = 0; retest <= retestcount; retest++)
                                {
                                    if (retest == retestcount)
                                    {
                                        throw new Exception($"波长计获取结果数量[{retestcount}]次重试, 均与需求目标数量不等.");
                                    }
                                    //FWM8612.RST();
                                    FWM8612.SetTriggerSource(Source.EXTernal);

                                    MIRROR1.Reset();
                                    MIRROR2.Reset();


                                    m1current = currentlist[i];
                                    //this.Log_Global($"Start MIRROR1 SinglePoint_Current_mA ...{m1current} mA");
                                    this.Log_Global($"Start MIRROR1 = {m1current} mA, MIRROR2 sweep ...{M1M2_Start_mA}~{M1M2_Stop_mA} mA");
                                    MIRROR1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(m1current, this.TestRecipe.complianceVoltage_V);

                                    //double[] Currents_mA = ArrayMath.CalculateArray(M2Start_mA, M2Stop_mA, M2Step_mA);




                                    name = MIRROR2.BuildTermialName();
                                    MIRROR2.SetDefaultTermialName(name);
                                    S_6683H.TrigTerminalsStart(name);
                                    MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, currentlist.ToArray(),
                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);

                                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, null);

                                    FWM8612.EXTernalStart2();

                                    Merged_PXIe_4143.Trigger(MIRROR2, null);

                                    var result = MIRROR2.Fetch_MeasureVals(currentlist.Count, 10 * 1000.0);

                                    if (FWM8612.FethEXTernalData(SerialNumber, currentlist.Count, out waveandpower))//波长计
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        this.Log_Global($"波长计获取结果数量[{waveandpower.Power.Count}]与需求目标数量[{currentlist.Count}]不等");
                                        FWM8612.EXTernalStop();
                                        S_6683H.TrigTerminalsStop(name);

                                    }
                                }

                                FWM8612.EXTernalStop();
                                S_6683H.TrigTerminalsStop(name);

                                for (int j = 0; j < currentlist.Count; j++)
                                {
                                    mTuningData.Mirror1_Current_mA.Add(m1current);
                                    mTuningData.Mirror2_Current_mA.Add(currentlist[j]);
                                    mTuningData.Wavelength_nm.Add(waveandpower.Wavelength[j]);
                                    mTuningData.Power_dBM.Add(waveandpower.Power[j]);
                                }

                            }
                        }
                        break;
                    case 3:
                        {
                            //for (int i = 0; i < abc.Count; i++)
                            //{
                            //    if (token.IsCancellationRequested)
                            //    {
                            //        this.Log_Global("用户取消Mirror tuning.");
                            //        return;
                            //    }


                            //    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                            //    sw.Start();
                            //    double m1current = abc[i].Mirror1_Current_mA;
                            //    double m2current = abc[i].Mirror2_Current_mA;
                            //    this.Log_Global($"Step MIRROR1 =[{m1current}]mA,MIRROR2=[{m2current}]mA");


                            //    MIRROR1.AssignmentMode_Current_FastMode(m1current, false);
                            //    MIRROR2.AssignmentMode_Current_FastMode(m2current, false);

                            //    sw.Stop();
                            //    this.Log_Global($"setting smu cost time = {sw.Elapsed.TotalMilliseconds} ms");


                            //    WavelengthAndPower_SinglePoint manualReadingSinglePoint = NewMethod(this.PH1, this.PH2, this.FWM8612, original_Ph1_current_mA, original_Ph2_current_mA);
                            //    abc[i].Wavelength_nm = manualReadingSinglePoint.Wavelength_nm;
                            //    abc[i].Power_dBM = manualReadingSinglePoint.Power_dbm;
                            //    //mTuningData.Mirror1_Current_mA.Add(m1current);
                            //    //mTuningData.Mirror2_Current_mA.Add(m2current);
                            //    //mTuningData.Wavelength_nm.Add(manualReadingSinglePoint.Wavelength_nm);
                            //    //mTuningData.Power_dBM.Add(manualReadingSinglePoint.Power_dbm);

                            //    this.Log_Global($"Step MIRROR1 =[{m1current}]mA,MIRROR2=[{m2current}]mA,WL={manualReadingSinglePoint.Wavelength_nm}nm,Pow=[{manualReadingSinglePoint.Power_dbm}]dbm");


                            //}

                            //Aggregate.Sort((item1, item2) => { return item1.Index.CompareTo(item2.Index); });
                        }
                        break;
                    default:
                        {


                            //老的
                            double mirror_scan_source_range_current_mA = 100;
                            double mirror_scan_source_comp_voltage_V = 2.5;
                            double mirror_idle_source_current_mA = 0;

                            MIRROR1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(mirror_idle_source_current_mA, mirror_scan_source_range_current_mA, mirror_scan_source_comp_voltage_V);
                            MIRROR2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(mirror_idle_source_current_mA, mirror_scan_source_range_current_mA, mirror_scan_source_comp_voltage_V);

                            for (int m1_current_index = 0; m1_current_index < currentlist.Count; m1_current_index++)
                            {
                                double m1current = currentlist[m1_current_index];

                                //this.Log_Global($"Start MIRROR1 SinglePoint_Current_mA ...{m1current} mA");
                                MIRROR1.AssignmentMode_Current_FastMode(m1current, false);

                                for (int m2_current_index = 0; m2_current_index < currentlist.Count; m2_current_index++)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        this.Log_Global("用户取消Mirror tuning.");
                                        return;
                                    }
                                    double m2current = currentlist[m2_current_index];
                                    //Thread.Sleep(5);
                                    //MIRROR2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(m2current, this.TestRecipe.complianceVoltage_V);
                                    //After setting mirror 1 and mirror 2 - ------------------
                                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                                    sw.Start();

                                    MIRROR2.AssignmentMode_Current_FastMode(m2current, false);
                                    Thread.Sleep(1);
                                    sw.Stop();
                                    this.Log_Global($"setting smu cost time = {sw.Elapsed.TotalMilliseconds} ms");

                                    WavelengthAndPower_SinglePoint manualReadingSinglePoint = NewMethod(this.PH1, this.PH2, this.FWM8612, original_Ph1_current_mA, original_Ph2_current_mA, path);
                                    manualReadingSinglePoint = Algorithm3(manualReadingSinglePoint, LPCurrent, MPD2Voltage);

                                    mTuningData.Mirror1_Current_mA.Add(m1current);
                                    mTuningData.Mirror2_Current_mA.Add(m2current);
                                    mTuningData.Wavelength_nm.Add(manualReadingSinglePoint.Wavelength_nm);
                                    mTuningData.Power_dBM.Add(manualReadingSinglePoint.Power_dbm);

                                    this.Log_Global($"Step MIRROR1 =[{m1current}]mA,MIRROR2=[{m2current}]mA,WL={manualReadingSinglePoint.Wavelength_nm}nm,Pow=[{manualReadingSinglePoint.Power_dbm}]dbm");
                                    if (manualReadingSinglePoint.IsLucky == false)
                                    {
                                        PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(qwlt_ph1_driving_crruent_mA, 2.5);
                                        PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(qwlt_ph2_driving_crruent_mA, 2.5);
                                    }


                                }
                                //PH1.AssignmentMode_Current_FastMode(qwlt_ph1_driving_crruent_mA);
                                //PH2.AssignmentMode_Current_FastMode(qwlt_ph2_driving_crruent_mA);
                                //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(qwlt_ph1_driving_crruent_mA, 2.5);
                                //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(qwlt_ph2_driving_crruent_mA, 2.5);
                            }
                        }
                        break;
                }






                string defaultFileName = string.Concat(@"MirrorTuning_补点后_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                var finalFileName = $@"{path}\{defaultFileName}";
                if (isPass)
                {
                    this.RawData.Path = finalFileName;
                }

                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    string heade = "Mirror1_Current_mA,Mirror2_Current_mA,Wavelength_nm,Power_dBM";

                    sw.WriteLine(heade);
                    sw.WriteLine("[RAW DATA]");
                    if (solution == 0)
                    {
                        for (int i = 0; i < Aggregate.Count; i++)
                        {
                            sw.WriteLine($"{Math.Round(Aggregate[i].Mirror1_Current_mA, 4)}," +
                                $"{Math.Round(Aggregate[i].Mirror2_Current_mA, 4)}," +
                                $"{Math.Round(Aggregate[i].Wavelength_nm, 4)}," +
                                $"{ Math.Round(Aggregate[i].Power_dBM, 4)}");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < mTuningData.Mirror1_Current_mA.Count; i++)
                        {
                            sw.WriteLine($"{Math.Round(mTuningData.Mirror1_Current_mA[i], 4)}," +
                                $"{Math.Round(mTuningData.Mirror2_Current_mA[i], 4)}," +
                                $"{Math.Round(mTuningData.Wavelength_nm[i], 4)}," +
                                $"{ Math.Round(mTuningData.Power_dBM[i], 4)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                Merged_PXIe_4143.Reset();
                this.Log_Global($"测试完成！");
            }
        }
        private void Algorithm4_86142BSweep(ref List<MTuningDataItem> Aggregate, CancellationToken token)
        {
            //20240628 如果需要补的点多，直接就不补了
            int failcount = 0;
            for (int i = 0; i < Aggregate.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                if (Aggregate[i].Wavelength_nm < 1500)
                {
                    failcount++;
                }
            }
            if(failcount>this.TestRecipe.Algorithm4_CountLimit)
            {
                this.Log_Global($"Algorithm4跳过不执行, [需执行总数:{failcount}]>[限制:{this.TestRecipe.Algorithm4_CountLimit}]");
                return;

            }    
            else
            {
                this.Log_Global($"Algorithm4需执行总数[{failcount}]");
            }
            
            
            if (!OSA_86142B.IsOnline)
            {
                this.Log_Global($"OSA_6370 连接失败！");
                return;
            }
            OSA_86142B.Reset();
            double CenterWavelength_nm = 1550;
            double WavelengthSpan_nm = 100;
            string Sensitivity_dbm = "AUTO";
            double Res_nm = 0.1;

            OSA_86142B.CenterWavelength_nm = CenterWavelength_nm;
            OSA_86142B.WavelengthSpan_nm = WavelengthSpan_nm;
            OSA_86142B.Sensitivity_dBm = Sensitivity_dbm; //20240709增加最低希望的底噪
            OSA_86142B.ResolutionBandwidth_nm = Res_nm; //20240709分辨率
            OSA_86142B.TraceLength = 1001;
            //List<double> m1fullcurrentAlgorithm4 = new List<double>();
            //List<double> m2fullcurrentAlgorithm4 = new List<double>();



            for (int i = 0, fail=1; i < Aggregate.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                if (Aggregate[i].Wavelength_nm < 1500)
                {

                    //m1fullcurrentAlgorithm4.Add(Aggregate[i].Mirror1_Current_mA);
                    //m2fullcurrentAlgorithm4.Add(Aggregate[i].Mirror2_Current_mA);
                    MIRROR1.AssignmentMode_Current(Aggregate[i].Mirror1_Current_mA, 2.5);
                    MIRROR2.AssignmentMode_Current(Aggregate[i].Mirror2_Current_mA, 2.5);
                    Thread.Sleep(50);
                    var waveandpower = OSA_86142B.GetOpticalSpectrumTrace(true);
                    List<double> wave = new List<double>();
                    List<double> power = new List<double>();
                    for (int j = 0; j < waveandpower.Count; j++)
                    {
                        wave.Add(waveandpower[j].Wavelength_nm);
                    }
                    for (int j = 0; j < waveandpower.Count; j++)
                    {
                        power.Add(waveandpower[j].Power_dBm);
                    }
                    
                    var Power_dBM = power.Max();

                    int max_index = 0;
                    int min_index = 0;
                    ArrayMath.GetMaxAndMinIndex(power.ToArray(), out max_index, out min_index);
                    var Wavelength_nm = wave[max_index];
                    Aggregate[i].Wavelength_nm = Wavelength_nm;
                    Aggregate[i].Power_dBM = Power_dBM;

                    this.Log_Global($"Algorithm4_OSASweep({fail++}/{failcount}) M1[{Aggregate[i].Mirror1_Current_mA.ToString("#.####")}]mA M2[{Aggregate[i].Mirror2_Current_mA.ToString("#.####")}]mA");
                    this.Log_Global($"Wavelength_nm[{Wavelength_nm}]nm Power_dBM[{Power_dBM.ToString("#.###")}]dBm");
                }
            }
        }
        private void Algorithm4_OSASweep(ref List<MTuningDataItem> Aggregate, CancellationToken token)
        {
            if (!OSA_6370.IsOnline)
            {
                this.Log_Global($"OSA_6370 连接失败！");
                return;
            }
            OSA_6370.Reset();
            double CenterWavelength_nm = 1550;
            double WavelengthSpan_nm = 100;
            OSA_6370.CenterWavelength_nm = CenterWavelength_nm;
            OSA_6370.WavelengthSpan_nm = WavelengthSpan_nm;

            //List<double> m1fullcurrentAlgorithm4 = new List<double>();
            //List<double> m2fullcurrentAlgorithm4 = new List<double>();
            for (int i = 0; i < Aggregate.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                if (Aggregate[i].Wavelength_nm < 1500)
                {

                    //m1fullcurrentAlgorithm4.Add(Aggregate[i].Mirror1_Current_mA);
                    //m2fullcurrentAlgorithm4.Add(Aggregate[i].Mirror2_Current_mA);
                    MIRROR1.AssignmentMode_Current(Aggregate[i].Mirror1_Current_mA, 2.5);
                    MIRROR2.AssignmentMode_Current(Aggregate[i].Mirror2_Current_mA, 2.5);
                    Thread.Sleep(10);
                    OSA_6370.TriggerSweep();
                    var Wavelength_nm = OSA_6370.GetPeakWavelength_nm();
                    var Power_dBM = OSA_6370.GetPeakPower_dBM();
                    Aggregate[i].Wavelength_nm = Wavelength_nm;
                    Aggregate[i].Power_dBM = Power_dBM;

                    this.Log_Global($"Algorithm4_OSASweep M1 = {Aggregate[i].Mirror1_Current_mA} mA , M2 = {Aggregate[i].Mirror2_Current_mA} mA ！");
                    this.Log_Global($"Wavelength_nm = {Wavelength_nm} nm , Power_dBM = {Power_dBM} dBm ！");
                }
            }
        }
        private void Algorithm3_LPhaseSweep(double LPCurrent,
            double MPD2Voltage,
            float M1M2_Start_mA,
            float M1M2_Stop_mA,
            ref List<MTuningDataItem> Aggregate,
            double laserphase_start_current_mA,
            double laserphase_stop_current_mA,
            double[] LPCurrentarray,
            double mpd2_comp_current_mA,
            double mpd2_source_delay_s,
            CancellationToken token)
        {
            //LP  Scan
            List<double> m1fullcurrentAlgorithm3 = new List<double>();
            List<double> m2fullcurrentAlgorithm3 = new List<double>();
            List<double> LPfullcurrentAlgorithm3 = new List<double>();
            for (int i = 0; i < Aggregate.Count; i++)
            {
                if (Aggregate[i].Wavelength_nm < 1500)
                {
                    for (int j = 0; j < LPCurrentarray.Length; j++)
                    {
                        m1fullcurrentAlgorithm3.Add(Aggregate[i].Mirror1_Current_mA);
                        m2fullcurrentAlgorithm3.Add(Aggregate[i].Mirror2_Current_mA);
                        LPfullcurrentAlgorithm3.Add(LPCurrentarray[j]);
                    }
                }
            }
            int seed = 10000;
            //if (LPfullcurrentAlgorithm3.Count > seed)
            //{
            List<double> total_MPD_CurrAlgorithm3 = new List<double>();
            WavelengthAndPower total_waveandpowerAlgorithm3 = new WavelengthAndPower();
            var minorCount = m1fullcurrentAlgorithm3.Count % seed;
            var majorCount = m1fullcurrentAlgorithm3.Count / seed;

            List<double> sub_m1fullcurrentAlgorithm3 = new List<double>();
            List<double> sub_m2fullcurrentAlgorithm3 = new List<double>();
            List<double> sub_LPfullcurrentAlgorithm3 = new List<double>();
            int retest = 0; //20240718 波长计增加重测

            for (int i = 0; i <= majorCount; i++)
            {
                if (i != majorCount)
                {
                    this.Log_Global($"LPase 补点扫描总点数{ m1fullcurrentAlgorithm3.Count}..现在进行第{i}次分包扫描,此包范围索引为{i * seed}~{i * seed + seed}");
                    sub_m1fullcurrentAlgorithm3 = m1fullcurrentAlgorithm3.GetRange(i * seed, seed);
                    sub_m2fullcurrentAlgorithm3 = m2fullcurrentAlgorithm3.GetRange(i * seed, seed);
                    sub_LPfullcurrentAlgorithm3 = LPfullcurrentAlgorithm3.GetRange(i * seed, seed);
                }
                else
                {
                    this.Log_Global($"LP 补点扫描总点数{ m1fullcurrentAlgorithm3.Count}..现在进行最后一次分包扫描,此包范围索引为{majorCount * seed}~{majorCount * seed + minorCount}");
                    sub_m1fullcurrentAlgorithm3 = m1fullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                    sub_m2fullcurrentAlgorithm3 = m2fullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                    sub_LPfullcurrentAlgorithm3 = LPfullcurrentAlgorithm3.GetRange(majorCount * seed, minorCount);
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }

                string mirror2_trigger_signal_name = "";
                double[] tempMPD_CurrAlgorithm3 =new double[1];
                WavelengthAndPower temp_waveandpowerAlgorithm3 = new WavelengthAndPower();
                for (retest = 0; retest <= retestcount; retest++)
                {
                    if (retest == retestcount)
                    {
                        throw new Exception($"波长计获取结果数量[{retestcount}]次重试, 均与需求目标数量不等.");
                    }

                    MIRROR1.Reset();
                    MIRROR2.Reset();
                    LP.Reset();
                    MPD2.Reset();

                    FWM8612.RST();
                    //Thread.Sleep(200);
                    FWM8612.SetTriggerSource(Source.EXTernal);
                    //Thread.Sleep(200);

                    mirror2_trigger_signal_name = MIRROR2.BuildTermialName();
                    MIRROR2.SetDefaultTermialName(mirror2_trigger_signal_name);
                    S_6683H.TrigTerminalsStart(mirror2_trigger_signal_name);

                    MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m1fullcurrentAlgorithm3.ToArray(),
                                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                    MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_m2fullcurrentAlgorithm3.ToArray(),
                                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                    LP.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)laserphase_start_current_mA, (float)laserphase_stop_current_mA, sub_LPfullcurrentAlgorithm3.ToArray(),
                                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                    MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)MPD2Voltage, (float)mpd2_comp_current_mA, sub_LPfullcurrentAlgorithm3.Count,
                        mpd2_source_delay_s, this.TestRecipe.ApertureTime_s, true);



                    MIRROR1.TriggerOutputOn = true;
                    MIRROR2.TriggerOutputOn = true;
                    LP.TriggerOutputOn = true;
                    MPD2.TriggerOutputOn = true;

                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                    FWM8612.EXTernalStart2();
                    //Thread.Sleep(500);
                    Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

                    tempMPD_CurrAlgorithm3 = MPD2.Fetch_MeasureVals(sub_m1fullcurrentAlgorithm3.Count, 100 * 1000).CurrentMeasurements;

                    if (FWM8612.FethEXTernalData(SerialNumber, sub_m1fullcurrentAlgorithm3.Count, out temp_waveandpowerAlgorithm3))//波长计
                    {
                        break;
                    }
                    else
                    {
                        this.Log_Global($"波长计获取结果数量[{temp_waveandpowerAlgorithm3.Power.Count}]与需求目标数量[{sub_m1fullcurrentAlgorithm3.Count}]不等");
                        FWM8612.EXTernalStop();
                        S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);

                    }
                }

                FWM8612.EXTernalStop();
                //Thread.Sleep(500);
                S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);


                total_MPD_CurrAlgorithm3.AddRange(tempMPD_CurrAlgorithm3);
                total_waveandpowerAlgorithm3.Power.AddRange(temp_waveandpowerAlgorithm3.Power);
                total_waveandpowerAlgorithm3.Wavelength.AddRange(temp_waveandpowerAlgorithm3.Wavelength);

            }
            int index = 0;
            int count = 0;
            double max_MPD_CurrAlgorithm3 = 0;
            for (int i = 0; i < total_MPD_CurrAlgorithm3.Count; i++)
            {
                if (Math.Abs(total_MPD_CurrAlgorithm3[i]) > max_MPD_CurrAlgorithm3)
                {
                    max_MPD_CurrAlgorithm3 = Math.Abs(total_MPD_CurrAlgorithm3[i]);
                    index = i;
                }
                count++;
                if (count == LPCurrentarray.Length)
                {
                    max_MPD_CurrAlgorithm3 = 0;
                    count = 0;
                    var m1current = m1fullcurrentAlgorithm3[index];
                    var m2current = m2fullcurrentAlgorithm3[index];
                    var Wavelength = total_waveandpowerAlgorithm3.Wavelength[index];
                    var power = total_waveandpowerAlgorithm3.Power[index];
                    for (int j = 0; j < Aggregate.Count; j++)
                    {
                        if (Aggregate[j].Mirror1_Current_mA == m1current &&
                            Aggregate[j].Mirror2_Current_mA == m2current)
                        {
                            Aggregate[j].Wavelength_nm = Wavelength;
                            Aggregate[j].Power_dBM = power;
                            break;
                        }
                    }
                }
            }
            //}
            //else if (LPfullcurrentAlgorithm3.Count > 1)
            //{
            //    this.Log_Global($"LPase 补点扫描总点数{ m1fullcurrentAlgorithm3.Count}..");

            //    MIRROR1.Reset();
            //    MIRROR2.Reset();
            //    LP.Reset();
            //    MPD2.Reset();

            //    FWM8612.RST();
            //    Thread.Sleep(150);
            //    FWM8612.SetTriggerSource(Source.EXTernal);
            //    Thread.Sleep(150);

            //    var mirror2_trigger_signal_name = MIRROR2.BuildTermialName();
            //    MIRROR2.SetDefaultTermialName(mirror2_trigger_signal_name);
            //    S_6683H.TrigTerminalsStart(mirror2_trigger_signal_name);

            //    MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, m1fullcurrentAlgorithm3.ToArray(),
            //                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
            //    MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, m2fullcurrentAlgorithm3.ToArray(),
            //                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
            //    LP.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)laserphase_start_current_mA, (float)laserphase_stop_current_mA, LPfullcurrentAlgorithm3.ToArray(),
            //                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
            //    MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)MPD2Voltage, (float)mpd2_comp_current_mA, LPfullcurrentAlgorithm3.Count,
            //        mpd2_source_delay_s, this.TestRecipe.ApertureTime_s, true);



            //    MIRROR1.TriggerOutputOn = true;
            //    MIRROR2.TriggerOutputOn = true;
            //    LP.TriggerOutputOn = true;
            //    MPD2.TriggerOutputOn = true;

            //    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

            //    FWM8612.EXTernalStart2();
            //    Thread.Sleep(150);
            //    Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, LP, MPD2 });

            //    var tempMPD_CurrAlgorithm3 = MPD2.Fetch_MeasureVals(m1fullcurrentAlgorithm3.Count, 100 * 1000).CurrentMeasurements;
            //    WavelengthAndPower temp_waveandpowerAlgorithm3 = FWM8612.FethEXTernalData(m1fullcurrentAlgorithm3.Count);//波长计
            //    FWM8612.EXTernalStop();
            //    Thread.Sleep(150);
            //    S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);

            //    int index = 0;
            //    int count = 0;
            //    double max_MPD_CurrAlgorithm3 = 0;
            //    for (int i = 0; i < tempMPD_CurrAlgorithm3.Length; i++)
            //    {
            //        if (Math.Abs(tempMPD_CurrAlgorithm3[i]) > max_MPD_CurrAlgorithm3)
            //        {
            //            max_MPD_CurrAlgorithm3 = Math.Abs(tempMPD_CurrAlgorithm3[i]);
            //            index = i;
            //        }
            //        count++;
            //        if (count == LPCurrentarray.Length)
            //        {
            //            max_MPD_CurrAlgorithm3 = 0;
            //            count = 0;
            //            var m1current = m1fullcurrentAlgorithm3[index];
            //            var m2current = m2fullcurrentAlgorithm3[index];
            //            var Wavelength = temp_waveandpowerAlgorithm3.Wavelength[index];
            //            var power = temp_waveandpowerAlgorithm3.Power[index];
            //            for (int j = 0; j < Aggregate.Count; j++)
            //            {
            //                if (Aggregate[j].Mirror1_Current_mA == m1current &&
            //                    Aggregate[j].Mirror2_Current_mA == m2current)
            //                {
            //                    Aggregate[j].Wavelength_nm = Wavelength;
            //                    Aggregate[j].Power_dBM = power;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            LP.AssignmentMode_Current(LPCurrent, 2.5);

        }
        private void Algorithm2_PhaseSweep(
            double qwlt_ph1_driving_crruent_mA,
            double qwlt_ph2_driving_crruent_mA,
            double MPD2Voltage,
            float M1M2_Start_mA,
            float M1M2_Stop_mA,
            ref List<MTuningDataItem> Aggregate,
            string trigger_signal_name,
            double pstart,
            double pstop,
            double[] p1_p2_Currents_mA,
            double mpd2_comp_current_mA,
            double mpd2_source_delay_s,
            CancellationToken token)
        {
            List<double> final_m1fullcurrentAlgorithm2 = new List<double>();
            List<double> final_m2fullcurrentAlgorithm2 = new List<double>();
            List<double> final_ph1fullcurrentAlgorithm2 = new List<double>();
            List<double> final_ph2fullcurrentAlgorithm2 = new List<double>();

            for (int i = 0; i < Aggregate.Count; i++)
            {
                if (Aggregate[i].Wavelength_nm < 1500)
                {
                    //phase scan  Sequence
                    //ph1=0,扫ph2
                    for (int j = 0; j < p1_p2_Currents_mA.Length; j++)
                    {
                        final_m1fullcurrentAlgorithm2.Add(Aggregate[i].Mirror1_Current_mA);
                        final_m2fullcurrentAlgorithm2.Add(Aggregate[i].Mirror2_Current_mA);
                        final_ph1fullcurrentAlgorithm2.Add(0);// (p1_p2_Currents_mA[j]);
                        final_ph2fullcurrentAlgorithm2.Add(p1_p2_Currents_mA[j]);

                    }
                    //ph2=0,扫ph1
                    for (int j = 0; j < p1_p2_Currents_mA.Length; j++)
                    {
                        final_m1fullcurrentAlgorithm2.Add(Aggregate[i].Mirror1_Current_mA);
                        final_m2fullcurrentAlgorithm2.Add(Aggregate[i].Mirror2_Current_mA);
                        final_ph1fullcurrentAlgorithm2.Add(p1_p2_Currents_mA[j]);
                        final_ph2fullcurrentAlgorithm2.Add(0);// (p1_p2_Currents_mA[m]);


                    }
                }
            }


            List<double> final_total_MPD_CurrAlgorithm2 = new List<double>();
            WavelengthAndPower final_total_waveandpowerAlgorithm2 = new WavelengthAndPower();
            //double final_mpd2_comp_current_mA = 20.0;
            //double final_mpd2_source_delay_s = 0.001;
            int final_seed = 10000;
            int final_majorCount = final_m1fullcurrentAlgorithm2.Count / final_seed;
            int final_minorCount = final_m2fullcurrentAlgorithm2.Count % final_seed;

            int retest = 0; //20240718 波长计增加重测
            for (int i = 0; i <= final_majorCount; i++)
            {
                List<double> sub_final_m1fullcurrentAlgorithm2 = new List<double>();
                List<double> sub_final_m2fullcurrentAlgorithm2 = new List<double>();
                List<double> sub_final_ph1fullcurrentAlgorithm2 = new List<double>();
                List<double> sub_final_ph2fullcurrentAlgorithm2 = new List<double>();

                if (i != final_majorCount)
                {
                    this.Log_Global($"补点扫描总点数{ final_m1fullcurrentAlgorithm2.Count}..现在进行第{i}次分包扫描,此包范围索引为{i * final_seed}~{i * final_seed + final_seed}");
                    sub_final_m1fullcurrentAlgorithm2 = final_m1fullcurrentAlgorithm2.GetRange(i * final_seed, final_seed);
                    sub_final_m2fullcurrentAlgorithm2 = final_m2fullcurrentAlgorithm2.GetRange(i * final_seed, final_seed);
                    sub_final_ph1fullcurrentAlgorithm2 = final_ph1fullcurrentAlgorithm2.GetRange(i * final_seed, final_seed);
                    sub_final_ph2fullcurrentAlgorithm2 = final_ph2fullcurrentAlgorithm2.GetRange(i * final_seed, final_seed);
                }
                else
                {
                    this.Log_Global($"补点扫描总点数{ final_m1fullcurrentAlgorithm2.Count}..现在进行最后一次分包扫描,此包范围索引为{final_majorCount * final_seed}~{final_majorCount * final_seed + final_minorCount}");
                    sub_final_m1fullcurrentAlgorithm2 = final_m1fullcurrentAlgorithm2.GetRange(final_majorCount * final_seed, final_minorCount);
                    sub_final_m2fullcurrentAlgorithm2 = final_m2fullcurrentAlgorithm2.GetRange(final_majorCount * final_seed, final_minorCount);
                    sub_final_ph1fullcurrentAlgorithm2 = final_ph1fullcurrentAlgorithm2.GetRange(final_majorCount * final_seed, final_minorCount);
                    sub_final_ph2fullcurrentAlgorithm2 = final_ph2fullcurrentAlgorithm2.GetRange(final_majorCount * final_seed, final_minorCount);
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }

                double[] tempMPD_CurrAlgorithm2 = new double[1];
                WavelengthAndPower temp_waveandpowerAlgorithm2 = new WavelengthAndPower();

                for (retest = 0; retest <= retestcount; retest++)
                {
                    if (retest == retestcount)
                    {
                        throw new Exception($"波长计获取结果数量[{retestcount}]次重试, 均与需求目标数量不等.");
                    }
                    MIRROR1.Reset();
                    MIRROR2.Reset();
                    PH1.Reset();
                    PH2.Reset();
                    MPD2.Reset();

                    FWM8612.RST();
                    //Thread.Sleep(200);
                    FWM8612.SetTriggerSource(Source.EXTernal);
                    //Thread.Sleep(200);

                    MIRROR2.SetDefaultTermialName(trigger_signal_name);
                    S_6683H.TrigTerminalsStart(trigger_signal_name);

                    MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_final_m1fullcurrentAlgorithm2.ToArray(),
                                                                    this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                    MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(M1M2_Start_mA, M1M2_Stop_mA, sub_final_m2fullcurrentAlgorithm2.ToArray(),
                                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                    PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)pstart, (float)pstop, sub_final_ph1fullcurrentAlgorithm2.ToArray(),
                                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                    PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)pstart, (float)pstop, sub_final_ph2fullcurrentAlgorithm2.ToArray(),
                                                                this.TestRecipe.complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                    MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent((float)MPD2Voltage, (float)mpd2_comp_current_mA, sub_final_ph2fullcurrentAlgorithm2.Count,
                        mpd2_source_delay_s, this.TestRecipe.ApertureTime_s, true);



                    MIRROR1.TriggerOutputOn = true;
                    MIRROR2.TriggerOutputOn = true;
                    PH1.TriggerOutputOn = true;
                    PH2.TriggerOutputOn = true;
                    MPD2.TriggerOutputOn = true;

                    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, PH1, PH2, MPD2 });

                    FWM8612.EXTernalStart2();
                    //Thread.Sleep(500);
                    Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1, PH1, PH2, MPD2 });

                    tempMPD_CurrAlgorithm2 = MPD2.Fetch_MeasureVals(sub_final_m1fullcurrentAlgorithm2.Count, 100 * 1000).CurrentMeasurements;
                    if (FWM8612.FethEXTernalData(SerialNumber, sub_final_m1fullcurrentAlgorithm2.Count, out temp_waveandpowerAlgorithm2))//波长计
                    {
                        break;
                    }
                    else
                    {
                        this.Log_Global($"波长计获取结果数量[{temp_waveandpowerAlgorithm2.Power.Count}]与需求目标数量[{sub_final_m1fullcurrentAlgorithm2.Count}]不等");
                        FWM8612.EXTernalStop();
                        S_6683H.TrigTerminalsStop(trigger_signal_name);

                    }
                }
                FWM8612.EXTernalStop();
                //Thread.Sleep(500);
                S_6683H.TrigTerminalsStop(trigger_signal_name);


                final_total_MPD_CurrAlgorithm2.AddRange(tempMPD_CurrAlgorithm2);
                final_total_waveandpowerAlgorithm2.Power.AddRange(temp_waveandpowerAlgorithm2.Power);
                final_total_waveandpowerAlgorithm2.Wavelength.AddRange(temp_waveandpowerAlgorithm2.Wavelength);
            }
            int final_count = 0;
            int final_index = 0;
            double final_max_MPD_CurrAlgorithm2 = 0;
            for (int i = 0; i < final_total_MPD_CurrAlgorithm2.Count; i++)
            {
                if (Math.Abs(final_total_MPD_CurrAlgorithm2[i]) > final_max_MPD_CurrAlgorithm2)
                {
                    final_max_MPD_CurrAlgorithm2 = Math.Abs(final_total_MPD_CurrAlgorithm2[i]);
                    final_index = i;
                }
                final_count++;
                if (final_count == p1_p2_Currents_mA.Length * 2)
                {
                    final_max_MPD_CurrAlgorithm2 = 0;
                    final_count = 0;
                    var m1current = final_m1fullcurrentAlgorithm2[final_index];
                    var m2current = final_m2fullcurrentAlgorithm2[final_index];
                    var Wavelength = final_total_waveandpowerAlgorithm2.Wavelength[final_index];
                    var power = final_total_waveandpowerAlgorithm2.Power[final_index];
                    for (int j = 0; j < Aggregate.Count; j++)
                    {
                        if (Aggregate[j].Mirror1_Current_mA == m1current &&
                            Aggregate[j].Mirror2_Current_mA == m2current)
                        {
                            Aggregate[j].Wavelength_nm = Wavelength;
                            Aggregate[j].Power_dBM = power;
                            break;
                        }
                    }
                }
            }
            PH1.AssignmentMode_Current(qwlt_ph1_driving_crruent_mA, 2.5);
            PH2.AssignmentMode_Current(qwlt_ph2_driving_crruent_mA, 2.5);
        }

        WavelengthAndPower_SinglePoint Algorithm3(
            WavelengthAndPower_SinglePoint manualReadingSinglePoint,
            double LPCurrent, double MPD2Voltage)
        {
            if (manualReadingSinglePoint.Wavelength_nm < 1500)//garbage
            {
                List<double> lp_vs_mpd_current_list = new List<double>();
                List<double> Wavelenthlist = new List<double>();
                List<double> Powerlist = new List<double>();
                double[] LPCurrentarray = ArrayMath.CalculateArray(0, 20, 1);
                float mpdsourceVoltage_V = -2f;
                float mpdcomplianceCurrent_mA = 20f;
                double mpd_measureCurrentRange_mA = 1;
                float complianceVoltage_V = 2.5f;
                MPD2.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(mpdsourceVoltage_V, mpdcomplianceCurrent_mA, mpd_measureCurrentRange_mA);

                for (int i = 0; i < LPCurrentarray.Length; i++)
                {
                    LP.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(LPCurrentarray[i], complianceVoltage_V);
                    Thread.Sleep(1);
                    lp_vs_mpd_current_list.Add(MPD2.ReadCurrent_A());
                    var Wavelenth = FWM8612.GetWavelenth();
                    var power = FWM8612.GetPower();
                    if (Wavelenth < 1500)
                    {
                        Wavelenthlist.Add(Wavelenth);
                        Powerlist.Add(power);
                    }
                }
                int max_index = 0;
                int min_index = 0;

                ArrayMath.GetMaxAndMinIndex(Wavelenthlist.ToArray(), out max_index, out min_index);

                manualReadingSinglePoint.Wavelength_nm = Wavelenthlist[max_index];
                manualReadingSinglePoint.Power_dbm = Powerlist[max_index];

                LP.AssignmentMode_Current(LPCurrent, 2.5); //LP.Reset();
                MPD2.AssignmentMode_Voltage(MPD2Voltage, 20); //MPD2.Reset();

                return manualReadingSinglePoint;



                //for (int i = 0; i < lp_vs_mpd_current_list.Count; i++)
                //{
                //    lp_vs_mpd_current_list[i] = lp_vs_mpd_current_list[i] * -1;
                //}
                //var LP_der_1st = ArrayMath.CalculateFirstDerivate(LPCurrentarray, lp_vs_mpd_current_list.ToArray());
                ////var LP_der_2nd = ArrayMath.CalculateFirstDerivate(LP_Curr, LP_der_1st);

                ////Array.Sort(LP_der_2nd);
                //int max_index = 0;
                //int min_index = 0;

                //ArrayMath.GetMaxAndMinIndex(LP_der_1st, out max_index, out min_index);

                //var LP_Current_1 = LPCurrentarray[max_index];
                //var LP_Current_2 = LPCurrentarray[min_index];

                //var LP_value = (LP_Current_1 + LP_Current_2) / 2.0;

                //LP.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(LP_value, complianceVoltage_V);


            }
            else
            {
                return manualReadingSinglePoint;
            }
        }
        WavelengthAndPower_SinglePoint NewMethod(
            PXISourceMeter_4143 source_ph1,
            PXISourceMeter_4143 source_ph2,
            FWM8612 wlm,
            double original_Ph1_current_mA,
            double original_Ph2_current_mA,
            string path)
        {
            WavelengthAndPower_SinglePoint wavelengthAndPower = new WavelengthAndPower_SinglePoint();


            if (source_ph1.IsOnline == false ||
                source_ph2.IsOnline == false ||
                wlm.IsOnline == false)

            { return wavelengthAndPower; }


            const double ph_scan_source_range_current_mA = 10.0;
            const double ph_scan_start_current_mA = 0.0;

            const double ph_scan_stop_current_mA = 10.0;
            const double ph_scan_step_current_mA = 1.0;
            const double ph_idle_current_mA = 0.0;
            const double ph_comp_voltage_V = 2.5;
            const double ph_scan_threshold_power_dbm = -15;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            //2.Read the WL from the WLM.If it is a valid value(i.e >= 1500), return it immediately
            //有可能被存到数据内的波长值
            var luckyWavelength = wlm.GetWavelenth();
            //有可能被存到数据内的功率值
            var luckyPower = wlm.GetPower();
            if (luckyWavelength >= 1500)
            {
                sw.Stop();

                this.Log_Global($"new method cost time = {sw.Elapsed.TotalMilliseconds} ms");
                return new WavelengthAndPower_SinglePoint()
                {
                    Power_dbm = luckyPower,
                    Wavelength_nm = luckyWavelength,
                    IsLucky = true

                };
            }
            else //如果Wavelength读数不理想 //3.If it is a bad signal(value could read 0, or whatever the WLM returns with a  bad signal)
            {
                this.Log_Global($"new method - phase scan  started");

                //3.1.Run a coarse grained phase sweep(0 - 10mA for phase 1 & 2 with 10 steps(for both phase 1 & 2)
                double[] p1_p2_Currents_mA = ArrayMath.CalculateArray(ph_scan_start_current_mA, ph_scan_stop_current_mA, ph_scan_step_current_mA);

                List<double> power_dbM_list = new List<double>();
                List<PhaseScanCurrentGroup> phaseDrivingCurrent_list = new List<PhaseScanCurrentGroup>();
                float mpdcomplianceCurrent_mA = 20f;
                float mpdsourceVoltage_V = -2f;
                double sourceDelay_s = 0.001;
                double ApertureTime_s = 0.001;
                double mpd_measureCurrentRange_mA = 1;


                source_ph1.Reset();
                source_ph2.Reset();
                MPD2.Reset();
                source_ph1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_idle_current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);
                //source_ph2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_idle_current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);
                source_ph2.SetupMaster_Sequence_SourceCurrent_SenseVoltage(
                    (float)ph_scan_start_current_mA,
                    (float)ph_scan_step_current_mA,
                    (float)ph_scan_stop_current_mA,
                   (float)ph_comp_voltage_V,
                   sourceDelay_s,
                   ApertureTime_s,
                   true);
                MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V, (float)mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                source_ph2.TriggerOutputOn = true;
                MPD2.TriggerOutputOn = true;

                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH2, new PXISourceMeter_4143[] { MPD2 });
                Merged_PXIe_4143.Trigger(PH2, new PXISourceMeter_4143[] { MPD2 }/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                var MPD_P2_Curr = MPD2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000).CurrentMeasurements;
                for (int i = 0; i < MPD_P2_Curr.Length; i++)
                {
                    MPD_P2_Curr[i] = MPD_P2_Curr[i] * -1;
                }
                int max_mpd_index = 0;
                int min_mpd_index = 0;

                ArrayMath.GetMaxAndMinIndex(MPD_P2_Curr, out max_mpd_index, out min_mpd_index);
                var MPD_PH2 = p1_p2_Currents_mA[max_mpd_index];


                source_ph1.Reset();
                source_ph2.Reset();
                MPD2.Reset();

                source_ph2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_idle_current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);
                source_ph1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(
                    (float)ph_scan_start_current_mA,
                    (float)ph_scan_step_current_mA,
                    (float)ph_scan_stop_current_mA,
                   (float)ph_comp_voltage_V,
                   sourceDelay_s,
                   ApertureTime_s,
                   true);
                MPD2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(mpdsourceVoltage_V, (float)mpd_measureCurrentRange_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                source_ph1.TriggerOutputOn = true;
                MPD2.TriggerOutputOn = true;

                Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH1, new PXISourceMeter_4143[] { MPD2 });
                Merged_PXIe_4143.Trigger(PH1, new PXISourceMeter_4143[] { MPD2 }/*, dfbCurrents_A.Length, timeout_ms_sweep*/);

                var MPD_P1_Curr = MPD2.Fetch_MeasureVals(p1_p2_Currents_mA.Length, 10 * 1000).CurrentMeasurements;
                for (int i = 0; i < MPD_P1_Curr.Length; i++)
                {
                    MPD_P1_Curr[i] = MPD_P1_Curr[i] * -1;
                }
                int max_mpd_index_P1 = 0;
                int min_mpd_index_P1 = 0;

                ArrayMath.GetMaxAndMinIndex(MPD_P1_Curr, out max_mpd_index_P1, out min_mpd_index_P1);
                var MPD_PH1 = p1_p2_Currents_mA[max_mpd_index_P1];

                if (MPD_PH2 > MPD_PH1)
                {
                    phaseDrivingCurrent_list.Add(new PhaseScanCurrentGroup()
                    {
                        Phase_1_Current_mA = ph_idle_current_mA,
                        Phase_2_Current_mA = MPD_PH2
                    });
                }
                else
                {
                    phaseDrivingCurrent_list.Add(new PhaseScanCurrentGroup()
                    {
                        Phase_1_Current_mA = MPD_PH1,
                        Phase_2_Current_mA = ph_idle_current_mA
                    });
                }
                for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
                {
                    power_dbM_list.Add(MPD_P2_Curr[i]);
                }
                for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
                {
                    power_dbM_list.Add(MPD_P1_Curr[i]);

                }

                if (false)
                {
                    //先扫ph2 此时ph1 = 0mA 手动
                    source_ph1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_idle_current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);
                    source_ph2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_idle_current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);

                    for (int ph_current_index = 0; ph_current_index < p1_p2_Currents_mA.Length; ph_current_index++)
                    {
                        var ph2_driving_current_mA = p1_p2_Currents_mA[ph_current_index];
                        source_ph2.AssignmentMode_Current_FastMode(ph2_driving_current_mA, false);
                        //       Thread.Sleep(5);
                        var pow = this.FWM8612.GetPower();
                        power_dbM_list.Add(pow);

                        phaseDrivingCurrent_list.Add(new PhaseScanCurrentGroup()
                        {
                            Phase_1_Current_mA = ph_idle_current_mA,
                            Phase_2_Current_mA = ph2_driving_current_mA
                        });
                    }
                    //再扫ph1 此时ph2 = 0mA 手动

                    source_ph1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_idle_current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);
                    source_ph2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_idle_current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);

                    Thread.Sleep(5);
                    for (int ph_current_index = 0; ph_current_index < p1_p2_Currents_mA.Length; ph_current_index++)
                    {
                        var ph1_driving_current_mA = p1_p2_Currents_mA[ph_current_index];

                        source_ph1.AssignmentMode_Current_FastMode(ph1_driving_current_mA, false);
                        //    Thread.Sleep(5);
                        var pow = this.FWM8612.GetPower();
                        power_dbM_list.Add(pow);

                        phaseDrivingCurrent_list.Add(new PhaseScanCurrentGroup()
                        {
                            Phase_1_Current_mA = ph1_driving_current_mA,
                            Phase_2_Current_mA = ph_idle_current_mA
                        });

                    }
                }


                int ph_scan_max_power_index = 0;
                int ph_scan_min_power_index = 0;

                ArrayMath.GetMaxAndMinIndex(power_dbM_list.ToArray(), out ph_scan_max_power_index, out ph_scan_min_power_index);
                double ph_scan_max_pow = power_dbM_list[ph_scan_max_power_index];
                PhaseScanCurrentGroup ph_scan_max_driving_Group = phaseDrivingCurrent_list[ph_scan_max_power_index];

                if (ph_scan_max_pow < ph_scan_threshold_power_dbm)
                {
                    //如果失败后恢复
                    //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(original_Ph1_current_mA, 2.5);
                    //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(original_Ph2_current_mA, 2.5);

                    sw.Stop();

                    this.Log_Global($"new method cost - phase scan Get_TRUE_BLACK_SPOT time = {sw.Elapsed.TotalMilliseconds} ms");
                    //3.2.If the power value of the peak is still bad (< -10dBm), then you can return a value that signals that it is a TRUE_BLACK_SPOT. 
                    //If there are a few 0s in the data, it is not the end of the world.
                    return WavelengthAndPower_SinglePoint.Get_TRUE_BLACK_SPOT();
                }
                else
                {
                    //3.3.If the power is good enough, set the phase section to the point of max power using the same rules as QWLT
                    //3.4.Now, read the WLM 5 times(in case it hasn’t updated yet), and return the reading if it is valid
                    source_ph1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_scan_max_driving_Group.Phase_1_Current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);
                    source_ph2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(ph_scan_max_driving_Group.Phase_2_Current_mA, ph_scan_source_range_current_mA, ph_comp_voltage_V);
                    Thread.Sleep(5);
                    double tempWavelength = 0.0;
                    double tempPower = 0.0;
                    List<double> temp_power_list = new List<double>();
                    List<double> temp_Wavelength_list = new List<double>();
                    for (int i = 0; i < 5; i++)
                    {
                        //有可能被存到数据内的波长值
                        tempWavelength = wlm.GetWavelenth();
                        Thread.Sleep(1);
                        //有可能被存到数据内的功率值
                        tempPower = wlm.GetPower();
                        Thread.Sleep(1);

                        temp_power_list.Add(tempPower);
                        temp_Wavelength_list.Add(tempWavelength);
                    }


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string defaultFileName = string.Concat(@"MirrorTuning_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                    var finalFileName = $@"{path}\{defaultFileName}";

                    using (StreamWriter sw2 = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                    {
                        string heade = "MirrorTuning_PH1_PH2";

                        sw2.WriteLine(heade);
                        sw2.WriteLine($"power,Wavelength");
                        for (int i = 0; i < temp_power_list.Count; i++)
                        {
                            sw2.WriteLine($"{temp_power_list[i]},{temp_Wavelength_list[i]}");
                        }


                    }
                    int final_max_power_index = 0;
                    int final_min_power_index = 0;

                    ArrayMath.GetMaxAndMinIndex(temp_power_list.ToArray(), out final_max_power_index, out final_min_power_index);


                    sw.Stop();

                    this.Log_Global($"new method- phase scan done  cost time = {sw.Elapsed.TotalMilliseconds} ms");

                    //如果失败后恢复
                    //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(original_Ph1_current_mA, 2.5);
                    //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(original_Ph2_current_mA, 2.5);

                    return new WavelengthAndPower_SinglePoint()
                    {
                        Power_dbm = temp_power_list[final_max_power_index],
                        Wavelength_nm = temp_Wavelength_list[final_max_power_index],
                        IsLucky = false
                    };


                }


            }
        }
        public class MTuningData
        {
            public MTuningData()
            {
                Mirror1_Current_mA = new List<double>();
                Mirror2_Current_mA = new List<double>();
                Wavelength_nm = new List<double>();
                Power_dBM = new List<double>();

            }

            public List<double> Mirror1_Current_mA { get; set; }
            public List<double> Mirror2_Current_mA { get; set; }
            public List<double> Wavelength_nm { get; set; }
            public List<double> Power_dBM { get; set; }
        }

        public class MTuningDataItem
        {
            public MTuningDataItem()
            {
                Index = 0;
                Mirror1_Current_mA = 0;
                Mirror2_Current_mA = 0;
                Wavelength_nm = 0;
                Power_dBM = 0;

            }

            public double MirrorMix// { get; set; }
            {
                get
                {
                    return Math.Sqrt(Math.Pow(Mirror1_Current_mA, 2) + Math.Pow(Mirror2_Current_mA, 2));
                }
            }

            public int Index { get; set; }
            public double Mirror1_Current_mA { get; set; }
            public double Mirror2_Current_mA { get; set; }
            public double Wavelength_nm { get; set; }
            public double Power_dBM { get; set; }
        }

    }
}