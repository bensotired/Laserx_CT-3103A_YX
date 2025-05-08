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
        "TestCalculator_SPECT_PowerMax",
        "TestCalculator_SPECT_Wavelength"
    )]
    [ConfigurableInstrument("ISourceMeter_Golight", "SourceMeter_Master", "用于LIV扫描")]
    [ConfigurableInstrument("IOSA", "OSA_Master", "用于LIV扫描")]
    public class TestModule_SPECT : TestModuleBase
    {
        public TestModule_SPECT() : base() { }

        ISourceMeter_Golight SourceMeter_Master;
        IOSA OSA_Master;
        TestRecipe_SPECT TestRecipe { get; set; }
        RawData_SPECT RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_SPECT);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_SPECT();

            return RawData;
        }

        public override bool SetupResources(DataBook<string, string> userDefineInstrumentConfig, DataBook<string, string> userDefineAxisConfig, DataBook<string, string> userDefinePositionConfig, ITestPluginResourceProvider resourceProvider)
       
        {
            #region SourceMeter_Master
            {
                if (userDefineInstrumentConfig.ContainsKey(nameof(SourceMeter_Master)) == false)
                {
                    this.Log_Global($"用户定义仪器列表内未包含测试项目{this.Name}所需仪器[{nameof(SourceMeter_Master)}]");
                    return false;
                }
                var userDefineInstrKey = userDefineInstrumentConfig[nameof(SourceMeter_Master)];

                if (resourceProvider.Resource_Instruments.ContainsKey(userDefineInstrKey) == false)
                //if (unitInstruments.ContainsKey(userDefineInstrKey) == false)
                {
                    this.Log_Global($"仪器资源列表内未包含测试项目{this.Name}所需仪器[{nameof(SourceMeter_Master)}]指定的仪器实例[{userDefineInstrKey}]");
                    return false;
                }
                SourceMeter_Master = resourceProvider.Resource_Instruments[userDefineInstrKey] as ISourceMeter_Golight;
            }

            #endregion

            #region OSA_Master
            {
                if (userDefineInstrumentConfig.ContainsKey(nameof(OSA_Master)) == false)
                {
                    this.Log_Global($"用户定义仪器列表内未包含测试项目{this.Name}所需仪器[{nameof(OSA_Master)}]");
                    return false;
                }
                var userDefineInstrKey = userDefineInstrumentConfig[nameof(OSA_Master)];

                if (resourceProvider.Resource_Instruments.ContainsKey(userDefineInstrKey) == false)
                //if (unitInstruments.ContainsKey(userDefineInstrKey) == false)
                {
                    this.Log_Global($"仪器资源列表内未包含测试项目{this.Name}所需仪器[{nameof(OSA_Master)}]指定的仪器实例[{userDefineInstrKey}]");
                    return false;
                }
                OSA_Master = resourceProvider.Resource_Instruments[userDefineInstrKey] as IOSA;
            }

            #endregion


            return true;
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_SPECT>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //使用CT-3102A的方法
               
        
                if (SourceMeter_Master == null || OSA_Master == null)
                {
                    return;
                }
                this.Log_Global($"开始测试!");

                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Timeout_ms = 1000;
                    SourceMeter_Master.CurrentSetpoint_A = 0;
                    SourceMeter_Master.VoltageSetpoint_V = 0;
                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    SourceMeter_Master.IsOutputEnable = false;
                }
              
                //spectrumData.ExecuteStatus = TestExecuteStatus.Normal;
                //spectrumData.SummaryParameterPostFix = this.TestRecipe.SummaryParameterPostFix;
                //spectrumData.SpecificCenterWavelength_nm = this.TestRecipe.CenterWavelength_nm;
                //spectrumData.SpecificSpan_nm = this.TestRecipe.WavelengthSpan_nm;

                OSA_Master.CalibrationAutoZeroEnable = false;//20190321 新增 避免ZERO导致产品无数据
                OSA_Master.ResolutionBandwidth_nm = this.TestRecipe.OsaRbw_nm;
                OSA_Master.CenterWavelength_nm = this.TestRecipe.CenterWavelength_nm;
                OSA_Master.WavelengthSpan_nm = this.TestRecipe.WavelengthSpan_nm;
                OSA_Master.TraceLength = this.TestRecipe.OsaTraceLength;
                OSA_Master.Sensitivity = YokogawaAQ6370SensitivityModes.Mid;
                OSA_Master.SmsrModeDiff = this.TestRecipe.SmsrModeDiff_dB;
                OSA_Master.SMSRMask_nm = this.TestRecipe.SMSRMask_nm;
               
                //这组数据的显示宽度
                double Wavelength_Min_Limit = double.MaxValue;
                double Wavelength_Max_Limit = double.MinValue;
                double tCenterWave = this.TestRecipe.CenterWavelength_nm; //临时保存
                double tWaveSpan = this.TestRecipe.WavelengthSpan_nm;   //临时保存


                //先使用普通的不增加ith模式的计算     //注意 123456根据,分解是123456    123456,654321分解是123456和654321
                List<string> BiasCurrentList = this.TestRecipe.strBiasCurrentList_mA.Split(',').ToList();
                for (int CurrentIndex = 0; CurrentIndex < BiasCurrentList.Count; CurrentIndex++)
                {
                    double BiasCurrent_mA = 0;
                    double.TryParse(BiasCurrentList[CurrentIndex],out BiasCurrent_mA);

                    //加电前等待
                    if (this.TestRecipe.BiasBeforeWait_ms > 0)
                    {
                        this.Log_Global($"加电前等待 {this.TestRecipe.BiasBeforeWait_ms}ms.");
                        Thread.Sleep(this.TestRecipe.BiasBeforeWait_ms);
                    }

                    //这里要加电了
                    SourceMeter_Master.SetupSMU_LD(BiasCurrent_mA, this.TestRecipe.complianceVoltage_V); //默认2.5V
                    if (this.TestRecipe.En_EAOutput)
                    {
                        SourceMeter_Master.SetupSMU_EA(this.TestRecipe.EAVoltage_V, this.TestRecipe.EACurrentClampI_mA);
                    }
               
                    //加电后等待至少500ms
                    if (this.TestRecipe.BiasAfterWait_ms > 0)
                    {
                        int waittime = 500;
                        waittime = Math.Max(waittime, this.TestRecipe.BiasAfterWait_ms);
                        this.Log_Global($"加电后等待 {waittime}ms." );
                        Thread.Sleep(waittime);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                    OsaTrace traceEnum = (OsaTrace)Enum.Parse(typeof(OsaTrace), this.TestRecipe.OsaTrace);

                    PowerWavelengthTrace trace = OSA_Master.GetOpticalSpectrumTrace(true, traceEnum);
        
                    //RawData.Trace = trace;
                    //RawData.Smsr_dB = OSA_Master.GetSmsr_dB();
                    //RawData.Wavelength_nm = OSA_Master.ReadCenterWL_nm();
                    //RawData.PeakPower_dbm = OSA_Master.ReadPowerAtPeak_dbm();
                    //RawData.SpectrumWidth_20db_nm = OSA_Master.ReadSpectrumWidth_nm(20);
                    //RawData.SpectrumWidth_3db_nm = OSA_Master.ReadSpectrumWidth_nm(3);
                    RawData.LaserCurrent_mA = BiasCurrent_mA;
                    
                    for (int i = 0; i < trace.Count; i++)
                    {
                        RawData.Add(new RawDatumItem_SPECT()
                        {
                            Power = trace[i].Power_dBm,
                            Wavelength_nm = trace[i].Wavelength_nm,

                        });
                    }
                }

                //做完后必须要reset 带电操作会不好
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Timeout_ms = 1000;
                    SourceMeter_Master.CurrentSetpoint_A = 0;
                    SourceMeter_Master.VoltageSetpoint_V = 0;
                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    SourceMeter_Master.IsOutputEnable = false;
                }

            }
            catch (Exception ex)
            {
            }
        }

      
    }
}
