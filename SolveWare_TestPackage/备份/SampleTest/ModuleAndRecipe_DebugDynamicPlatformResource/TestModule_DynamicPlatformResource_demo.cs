using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_Sample")]
 
 
    [ConfigurableInstrument("IScanner", "Scanner_1", "用于条码扫描")]

    [ConfigurableAxis("Axis_X", "运动轴")]
 

    [ConfigurablePosition("Position_H", "运动点")]
 
 
 
    public class TestModule_DynamicPlatformResource_demo : TestModuleBase
    {
        public TestModule_DynamicPlatformResource_demo() : base() { }
        #region 以get属性获取测试模块运行所需资源
        IScanner Scanner_1 { get { return (IScanner)this.ModuleResource["Scanner_1"]; } }
        MotorAxisBase Axis_Y { get { return (MotorAxisBase)this.ModuleResource["Axis_Y"]; } }
        MotorAxisBase Axis_X { get { return (MotorAxisBase)this.ModuleResource["Axis_X"]; } }

        AxesPosition Position_H { get { return (AxesPosition)this.ModuleResource["Position_H"]; } }
        AxesPosition Position_V { get { return (AxesPosition)this.ModuleResource["Position_V"]; } }


        VisionController_LaserX_Image VisionController { get { return (VisionController_LaserX_Image)this.ModuleResource["VisionController_LaserX"]; } }
        #endregion

        TestRecipe_Sample TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_Sample); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_Sample>(testRecipe); }
        RawData_Sample RawData { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_Sample(); return RawData; }
        public override void Run(CancellationToken token)
        {
            try
            {
                var ax = this.Axis_X;
            
                var scanner = this.Scanner_1;
                var ph = this.Position_H;
                //ax.SingleAxisMotion(this.LocalResource.Axes[AxisName], ph);
            }
            catch (Exception ex)
            {

            }
        }
    }
}