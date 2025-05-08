using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolveWare_TestPackage
{   
    [SupportedCalculator
    (
        "TestCalculator_Spect_PeakWavelength",
        "TestCalculator_Spect_SpectrumWidth_20db_nm",
        "TestCalculator_Spect_SpectrumWidth_3db_nm",
        "TestCalculator_Spect_FWHM",
        "TestCalculator_Spect_SMSR",
        "TestCalculator_Spect_Temperature",
        "TestCalculator_Spect_DrivingCurrent_mA"
    )]


    [ConfigurableInstrument("Keithley2602B", "SourceMeter_2602B", "用于加电")]
    [ConfigurableInstrument("IOSA", "OSA", "用于光谱扫描")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "用于获取载台当前温度")]

    public class TestModule_SPECT_2602B : TestModuleBase
    {
        public TestModule_SPECT_2602B() : base() { }
        
        Keithley2602B SourceMeter_Master { get { return (Keithley2602B)this.ModuleResource["SourceMeter_2602B"]; } }
        MeerstetterTECController_1089 TEC { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }                
        IOSA OSA_Master { get { return (IOSA)this.ModuleResource["OSA"]; } }
        TestRecipe_SPECT_2602B TestRecipe { get; set; }
        RawData_SPECT RawData { get; set; }
        double DrivingCurrent_mA { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_SPECT_2602B);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_SPECT();

            return RawData;
        }

 
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_SPECT_2602B>(testRecipe);
        }

        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            this.DrivingCurrent_mA = 0.0;
            bool Findok = dutStreamData.SummaryDataCollection.ItemCollection.Exists(t => t.Name == this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString());
            if (this.TestRecipe.UseRefData_DrivingCurrent_mA == true&& Findok)
            {               
                var sdata = dutStreamData.SummaryDataCollection.GetSingleItem(this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString());
                DrivingCurrent_mA = Convert.ToDouble(sdata.Value);
                DrivingCurrent_mA += this.TestRecipe.RefData_DrivingCurrent_Offset_mA;
            }
            else
            {
                DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
            }
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //使用CT-3102A的方法


                if (SourceMeter_Master.IsOnline == false|| SourceMeter_Master==null)
                {
                    Log_Global($"仪器[{SourceMeter_Master.Name}]状态为[{SourceMeter_Master.IsOnline}]");
                    return;
                }
                if (OSA_Master.IsOnline == false || OSA_Master == null)
                {
                    Log_Global($"仪器[{OSA_Master.Name}]状态为[{OSA_Master.IsOnline}]");
                    return;
                }
                if (/*TEC.IsOnline == false ||*/ TEC == null)
                {
                    Log_Global($"仪器[{TEC.Name}]状态为[{TEC.IsOnline}]");
                    return;
                }
                this.Log_Global($"TestModule_SPECT_2602B 开始测试!");
               

                if (DrivingCurrent_mA == 0)
                {
                    DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
                }

                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Reset();
                    //SourceMeter_Master.Timeout_ms = 1000;
                    //SourceMeter_Master.CurrentSetpoint_A = 0;
                    //SourceMeter_Master.VoltageSetpoint_V = 0;
                    //SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    ////SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    //SourceMeter_Master.IsOutputEnable = true;
                }

                //spectrumData.ExecuteStatus = TestExecuteStatus.Normal;
                //spectrumData.SummaryParameterPostFix = this.TestRecipe.SummaryParameterPostFix;
                //spectrumData.SpecificCenterWavelength_nm = this.TestRecipe.CenterWavelength_nm;
                //spectrumData.SpecificSpan_nm = this.TestRecipe.WavelengthSpan_nm;

                OSA_Master.CalibrationAutoZeroEnable = false;//20190321 新增 避免ZERO导致产品无数据
                OSA_Master.ResolutionBandwidth_nm = this.TestRecipe.OsaRbw_nm;
                OSA_Master.CenterWavelength_nm = this.TestRecipe.CenterWavelength_nm;
                OSA_Master.WavelengthSpan_nm = this.TestRecipe.WavelengthSpan_nm;
                //OSA_Master.TraceLength = this.TestRecipe.OsaTraceLength;
                OSA_Master.TraceLength_string = this.TestRecipe.OsaTraceLength_string;
                //OSA_Master.Sensitivity = YokogawaAQ6370SensitivityModes.Mid;
                OSA_Master.Sensitivity = this.TestRecipe.SensitivityModes;
                OSA_Master.SmsrModeDiff = this.TestRecipe.SmsrModeDiff_dB;
                OSA_Master.SMSRMask_nm = this.TestRecipe.SMSRMask_nm;

                //这组数据的显示宽度
                double Wavelength_Min_Limit = double.MaxValue;
                double Wavelength_Max_Limit = double.MinValue;
                double tCenterWave = this.TestRecipe.CenterWavelength_nm; //临时保存
                double tWaveSpan = this.TestRecipe.WavelengthSpan_nm;   //临时保存


                ////先使用普通的不增加ith模式的计算     //注意 123456根据,分解是123456    123456,654321分解是123456和654321
                //List<string> BiasCurrentList = this.TestRecipe.strBiasCurrentList_mA.Split(',').ToList();
                //for (int CurrentIndex = 0; CurrentIndex < BiasCurrentList.Count; CurrentIndex++)
                //{
                //double BiasCurrent_mA = 0;
                //double.TryParse(BiasCurrentList[CurrentIndex],out BiasCurrent_mA);

                //加电前等待
                if (this.TestRecipe.BiasBeforeWait_ms > 0)
                {
                    this.Log_Global($"加电前等待 {this.TestRecipe.BiasBeforeWait_ms}ms.");
                    Thread.Sleep(this.TestRecipe.BiasBeforeWait_ms);
                }

                //这里要加电了
                // SourceMeter_Master.SetupSMU_LD(BiasCurrent_mA, this.TestRecipe.complianceVoltage_V); //默认2.5V
                SourceMeter_Master.SetupSMU_LD(DrivingCurrent_mA, this.TestRecipe.complianceVoltage_V); //默认2.5V
                                                                                                        //SourceMeter_Master.CurrentSetpoint_A = (float)BiasCurrent_mA;
                                                                                                        //if (this.TestRecipe.En_EAOutput)
                                                                                                        //{
                                                                                                        //    SourceMeter_Master.SetupSMU_EA(this.TestRecipe.EAVoltage_V, this.TestRecipe.EACurrentClampI_mA); //2002
                                                                                                        //}

                //加电后等待至少500ms
                if (this.TestRecipe.BiasAfterWait_ms > 0)
                {
                    int waittime = 500;
                    waittime = Math.Max(waittime, this.TestRecipe.BiasAfterWait_ms);
                    this.Log_Global($"加电后等待 {waittime}ms.");
                    Thread.Sleep(waittime);
                }
                else
                {
                    Thread.Sleep(500);
                }
                OsaTrace traceEnum = (OsaTrace)Enum.Parse(typeof(OsaTrace), this.TestRecipe.OsaTrace);

                PowerWavelengthTrace trace = OSA_Master.GetOpticalSpectrumTrace(true, traceEnum);

                RawData.Trace = trace;
                RawData.Smsr_dB = OSA_Master.GetSmsr_dB();
                RawData.CenterWavelength_nm = OSA_Master.ReadCenterWL_nm();
                RawData.PeakPower_dbm = OSA_Master.ReadPowerAtPeak_dbm();
                RawData.PeakWavelength_nm = OSA_Master.GetPeakWavelength_nm();
                RawData.SpectrumWidth_20db_nm = OSA_Master.ReadSpectrumWidth_nm(20);
                RawData.SpectrumWidth_3db_nm = OSA_Master.ReadSpectrumWidth_nm(3);
                RawData.LaserCurrent_mA = DrivingCurrent_mA;
                if (this.TestRecipe.UseRefData_DrivingCurrent_mA == true)
                {
                    RawData.SPECT_DrivingCurrent_mA = this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString() + this.TestRecipe.RefData_DrivingCurrent_Offset_mA.ToString();
                }
                else
                {
                    RawData.SPECT_DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA.ToString();
                }

                for (int i = 0; i < trace.Count; i++)
                {
                    RawData.Add(new RawDatumItem_SPECT()
                    {
                        Power = trace[i].Power_dBm,
                        Wavelength_nm = trace[i].Wavelength_nm,

                    });
                }
                //}

                //做完后必须要reset 带电操作会不好
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Reset();
                    //SourceMeter_Master.Timeout_ms = 1000;
                    //SourceMeter_Master.CurrentSetpoint_A = 0;
                    //SourceMeter_Master.VoltageSetpoint_V = 0;
                    //SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    ////SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    //SourceMeter_Master.IsOutputEnable = false;                   
                }
                //获取温度
                RawData.SPECT_Temperature_degC = TEC.CurrentObjectTemperature;
                this.Log_Global("TestModule_SPECT_2602B 模块运行完成");
            }
            catch (Exception ex)
            {
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Reset();
                }
                this._core.ReportException("光谱模块运行错误", ErrorCodes.Module_SPECT_Failed, ex);
            }
        }
        
    }
}
