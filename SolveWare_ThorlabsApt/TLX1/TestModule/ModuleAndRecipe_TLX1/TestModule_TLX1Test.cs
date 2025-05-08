using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_BurnInInstruments.TLX1.TestModule;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;

using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_Print_TLX1TestParams" ,
                         "TestCalculator_TLX1Power"
        )]

    [ConfigurableInstrument("Thorlabs_TLX1", "TLX1", "用于可调激光器控制")]
    
    public class TestModule_TLX1 : TestModuleBase
    {
        public TestModule_TLX1() : base() { }
        #region 以get属性获取测试模块运行所需资源       
        Thorlabs_TLX1 TLX1 { get { return (Thorlabs_TLX1)this.ModuleResource["TLX1"]; } }
        #endregion

        TestRecipe_TLX1 TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_TLX1); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_TLX1>(testRecipe); }
        RawData_TLX1 RawData { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_TLX1(); return RawData; } 
        public override void Run(CancellationToken token)
        {
            try
            {
                this.Log_Global($"开始设置仪器[{TLX1.Name}]");
                if (TLX1.IsOnline == false || TLX1 == null)
                {
                    Log_Global($"仪器[{TLX1.Name}]状态为[{TLX1.IsOnline}]");
                    return;
                }
                if (true)//Laser
                {
                    this.TLX1.SetGetSystemWavelength = this.TestRecipe.SetGetSystemWavelength;
                    this.TLX1.SetLaserPower_SW = this.TestRecipe.SetLaserPower_SW;
                    this.TLX1.SetDither_SW = this.TestRecipe.SetDither_SW;
                    this.TLX1.SetGetITUChannel = this.TestRecipe.SetGetITUChannel;
                    this.TLX1.SetGetFrequency = this.TestRecipe.SetGetFrequency;

                    this.Log_Global($"设置仪器[{TLX1.Name}]SystemWavelength[{this.TestRecipe.SetGetSystemWavelength}]"+
                                     $"LaserPower_SW[{this.TestRecipe.SetLaserPower_SW}]"+
                                     $"Dither_SW[{this.TestRecipe.SetDither_SW}]"+
                                     $"ITUChannel[{this.TestRecipe.SetGetITUChannel}]"+
                                     $"Frequency[{this.TestRecipe.SetGetFrequency}]");
                }
                if (true)//VOA
                {
                    this.TLX1.SetVOAPower_SW = this.TestRecipe.SetVOAPower_SW;
                    if (this.TestRecipe.SetVOAMode_SW)
                    {
                        if (this.TestRecipe.Use_dBm0rmW)
                        {
                            this.TLX1.SetOpticalOutputPowerValue_dBm = this.TestRecipe.SetOpticalOutputPowerValue_dBm;
                            this.Log_Global($"设置仪器[{TLX1.Name}]OpticalOutputPowerValue_dBm[{this.TestRecipe.SetOpticalOutputPowerValue_dBm}]");
                        }
                        else
                        {
                            this.TLX1.SetOpticalOutputPower_mW = this.TestRecipe.SetOpticalOutputPower_mW;
                            this.Log_Global($"设置仪器[{TLX1.Name}]OpticalOutputPower_mW[{this.TestRecipe.SetOpticalOutputPower_mW}]");
                        }
                    }
                    else
                    {
                        this.TLX1.SetOpticalAttenuationValue = this.TestRecipe.SetOpticalAttenuationValue;
                        this.Log_Global($"设置仪器[{TLX1.Name}]OpticalAttenuationValue[{this.TestRecipe.SetOpticalAttenuationValue}]");
                    }
                }
                //传参数
                RawData.SetLaserPower_SW = this.TestRecipe.SetLaserPower_SW;
                RawData.SetDither_SW = this.TestRecipe.SetDither_SW;
                RawData.SetGetITUChannel = this.TestRecipe.SetGetITUChannel;
                RawData.SetGetFrequency = this.TestRecipe.SetGetFrequency;

                RawData.SetGetSystemWavelength = this.TestRecipe.SetGetSystemWavelength;

                RawData.SetVOAPower_SW = this.TestRecipe.SetVOAPower_SW;

                RawData.SetVOAMode_SW = this.TestRecipe.SetVOAMode_SW;
                RawData.SetOpticalAttenuationValue = this.TestRecipe.SetOpticalAttenuationValue;

                RawData.Use_dBm0rmW = this.TestRecipe.Use_dBm0rmW;
                RawData.SetOpticalOutputPowerValue_dBm = this.TestRecipe.SetOpticalOutputPowerValue_dBm;
                RawData.SetOpticalOutputPower_mW = this.TestRecipe.SetOpticalOutputPower_mW;

                Thread.Sleep(300);
                Form_SetTLX1Wavelength from = new Form_SetTLX1Wavelength();
                from.ShowDialog();
                RawData.TLX1Power = from.TLX1Power;
                from.Dispose();

                this.Log_Global($"结束设置仪器[{TLX1.Name}]");
            }
            catch (Exception ex)
            {
                this._core.ReportException("激光器模块运行错误", ErrorCodes.Module_TLX1_Failed, ex);
                //throw ex;
            }
        }
    }
}