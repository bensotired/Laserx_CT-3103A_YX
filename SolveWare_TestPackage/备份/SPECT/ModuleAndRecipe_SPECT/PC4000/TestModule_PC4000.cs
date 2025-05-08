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
using System.Diagnostics;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
    (
        "TestCalculator_Spect_PeakWavelength",
        "TestCalculator_Spect_CenterWavelength",
        "TestCalculator_Spect_FWHM",
        "TestCalculator_Spect_SMSR"
    )]
    [ConfigurableInstrument("ISourceMeter_GS820", "SourceMeter_GS820", "用于LIV扫描")]
    [ConfigurableInstrument("IOSAUSB", "OSA_PC4000", "用于LIV扫描")]
    public class TestModule_PC4000 : TestModuleBase
    {
        public TestModule_PC4000() : base() { }

        ISourceMeter_GS820 sourceMeter_GS820;
        IOSAUSB osa_PC4000;
        TestRecipe_PC4000 TestRecipe { get; set; }
        RawData_SPECT_PC4000 RawData { get; set; }

      
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_PC4000);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_SPECT_PC4000();

            return RawData;
        }

        public override bool SetupResources(DataBook<string, string> userDefineInstrumentConfig, DataBook<string, string> userDefineAxisConfig, DataBook<string, string> userDefinePositionConfig, ITestPluginResourceProvider resourceProvider)
        {
            #region SourceMeter_Master
            {
                if (userDefineInstrumentConfig.ContainsKey(nameof(SourceMeter_GS820)) == false)
                {
                    this.Log_Global($"用户定义仪器列表内未包含测试项目{this.Name}所需仪器[{nameof(SourceMeter_GS820)}]");
                    return false;
                }
                var userDefineInstrKey = userDefineInstrumentConfig[nameof(SourceMeter_GS820)];

                if (resourceProvider.Resource_Instruments.ContainsKey(userDefineInstrKey) == false)
                //if (unitInstruments.ContainsKey(userDefineInstrKey) == false)
                {
                    this.Log_Global($"仪器资源列表内未包含测试项目{this.Name}所需仪器[{nameof(SourceMeter_GS820)}]指定的仪器实例[{userDefineInstrKey}]");
                    return false;
                }
                sourceMeter_GS820 = resourceProvider.Resource_Instruments[userDefineInstrKey] as ISourceMeter_GS820;
            }

            #endregion

            #region OSA_Master
            {
                if (userDefineInstrumentConfig.ContainsKey(nameof(OSA_PC4000)) == false)
                {
                    this.Log_Global($"用户定义仪器列表内未包含测试项目{this.Name}所需仪器[{nameof(OSA_PC4000)}]");
                    return false;
                }
                var userDefineInstrKey = userDefineInstrumentConfig[nameof(OSA_PC4000)];

                if (resourceProvider.Resource_Instruments.ContainsKey(userDefineInstrKey) == false)
                //if (unitInstruments.ContainsKey(userDefineInstrKey) == false)
                {
                    this.Log_Global($"仪器资源列表内未包含测试项目{this.Name}所需仪器[{nameof(OSA_PC4000)}]指定的仪器实例[{userDefineInstrKey}]");
                    return false;
                }
                osa_PC4000 = resourceProvider.Resource_Instruments[userDefineInstrKey] as IOSAUSB;
            }

            #endregion


            return true;
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_PC4000>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();


                //采集光谱
                int opticalCount = 0;
                osa_PC4000.Initialize();
                opticalCount = osa_PC4000.GetSpectrometersCount();
                if (opticalCount != 1)
                {
                    return;
                }

                

                //配置光谱参数s
                int integrationTime = Convert.ToInt32( this.TestRecipe.IntegrationTime_ms);
                int boxcarWidth = this.TestRecipe.BoxcarWidth;
                int scansToAverage = this.TestRecipe.ScansToAverage;
                osa_PC4000.SetParameters(integrationTime, boxcarWidth, scansToAverage);

                //先配置电流原进行加电
                double current_A = this.TestRecipe.BiasCurrent_mA / 1000;
                double complianceVoltage_V = this.TestRecipe.ComplianceVoltage_V ;
                sourceMeter_GS820.Reset();
                sourceMeter_GS820.SetMode(Keithley2602BChannel.CHA,SourceMeterMode.SourceCurrentSenceVoltage);
                sourceMeter_GS820.SetCurrent_A(Keithley2602BChannel.CHA, current_A);
                sourceMeter_GS820.SetComplianceVoltage_V(Keithley2602BChannel.CHA, complianceVoltage_V);
                Thread.Sleep(10);
                sourceMeter_GS820.OutputOn();
                Thread.Sleep(30);
                double[] x = null;
                double[] y = null;
                try
                {
                    x = osa_PC4000.GetWaveLength();//这里就是横坐标 波长
                    y = osa_PC4000.GetOpticalData();//这里就是纵坐标 功率值得
                }
                catch
                {

                }
                finally
                {
                    sourceMeter_GS820.OutputOff();
                }
                
                if (x.Length == y.Length)
                {
                    for (int i = 0; i < x.Length; i++)
                    {
                        RawData.Add(new RawDatumItem_SPECT_PC4000()
                        {
                            Wavelength_nm =x[i],
                            Power = y[i]
                        });
                    }
                }
                RawData.LaserCurrent_mA = this.TestRecipe.BiasCurrent_mA;
                //计算时间
                stopwatch.Stop();
                var allTimeofGetSpect = stopwatch.ElapsedMilliseconds;


            }
            catch (Exception ex)
            {
            }
        }
        public void outputOn()
        {

        }

        public void outputOff()
        {

        }

    }
}
