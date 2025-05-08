using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;

using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    [ConfigurableInstrument("OpticalSwitch", "OpticalSwitch", "用于切换光路(1*4切换器)")]

    [StaticResource(ResourceItemType.IO, "OpticaFiberSW_OutPut", "光纤切换继电器（IO）")]

    public class TestModule_OpticalSwitch : TestModuleBase
    {
        public TestModule_OpticalSwitch() : base() { } 
        #region 以get属性获取测试模块运行所需资源
        OpticalSwitch OpticalSwitch { get { return (OpticalSwitch)this.ModuleResource["OpticalSwitch"]; } }
        IOBase OpticaFiberSW { get { return (IOBase)this.ModuleResource["OpticaFiberSW_OutPut"]; } }

        #endregion

        TestRecipe_OpticalSwitch TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_OpticalSwitch); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_OpticalSwitch>(testRecipe); }
        RawData_OpticalSwitchLite RawData { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_OpticalSwitchLite(); return RawData; }
        public override void Run(CancellationToken token)
        {
            try
            {
                if (OpticalSwitch.IsOnline&& this.TestRecipe.OpticalSwitchEnable)
                {
                    this.Log_Global($"开始进行光开关[{OpticalSwitch.Name}]光路切换");
                    if (OpticalSwitch.SetCH(Convert.ToByte(this.TestRecipe.ChangeNo)))
                    {
                        this.Log_Global($"光开关[{OpticalSwitch.Name}]光路切换完成，当前光路通道是[{this.TestRecipe.ChangeNo}]");
                    }
                    // OpticalSwitch_F.SetCH(Convert.ToByte(this.TestRecipe.ChangeNo));
                   // this.Log_Global($"开始进行光模块光路切换");
                }
                if (this.TestRecipe.IOSwitchEnable)
                {
                    if (this.TestRecipe.IOSwitchStatus)
                    {
                        this.Log_Global($"控制右侧六轴光路的IO为开启状态");
                        OpticaFiberSW.TurnOn(true);
                    }
                    else
                    {
                        this.Log_Global($"控制右侧六轴光路的IO为关闭状态");
                        OpticaFiberSW.TurnOn(false);
                    }
                }                             

            }
            catch (Exception ex)
            {
                this._core.ReportException("光开关切换模块运行错误", ErrorCodes.Module_OpticalSwitch_Failed, ex);
               // throw ex;
            }
        }
    }
}