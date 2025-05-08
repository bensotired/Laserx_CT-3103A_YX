using SolveWare_BurnInCommon;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;

using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    //[SupportedCalculator("TestCalculator_Sample")]


    //[ConfigurableInstrument("IScanner", "Scanner_1", "用于条码扫描")]

    //[ConfigurableAxis("Axis_Name", "轴名称")]
    [ConfigurableAxis("Axis", "轴_名称")]
    //[ConfigurableAxis("Axis_Y", "主导轨Y轴")]
    //[ConfigurableAxis("Axis_Z", "主导轨Z轴")]

    [ConfigurablePosition("Position", "运动点位")]
    //[ConfigurablePosition("Position_V", "远场扫描V轴中点")]
 
 
    public class TestModule_MotionTest_SingleAxis : TestModuleBase
    {
        public TestModule_MotionTest_SingleAxis() : base() { }
        #region 以get属性获取测试模块运行所需资源
        //IScanner Scanner_1 { get { return (IScanner)this.ModuleResource["Scanner_1"]; } }
        MotorAxisBase Axis { get { return (MotorAxisBase)this.ModuleResource["Axis_X"]; } }//1st 
        //MotorAxisBase Axis_2nd { get { return (MotorAxisBase)this.ModuleResource["Axis_Y"]; } }//2nd 
        //MotorAxisBase Axis_3rd { get { return (MotorAxisBase)this.ModuleResource["Axis_Z"]; } }//3rd
        //MotorAxisBase Axis_Name { get { return (MotorAxisBase)this.ModuleResource["Axis_Name"]; } } 

        AxesPosition Position { get { return (AxesPosition)this.ModuleResource["Position"]; } }

        // AxesPosition Position_V { get { return (AxesPosition)this.ModuleResource["Position_V"]; } }

        //VisionController_LaserX_Image VisionController { get { return (VisionController_LaserX_Image)this.ModuleResource["VisionController_LaserX"]; } }
        #endregion

        TestRecipe_MotionLite TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_MotionLite); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_MotionLite>(testRecipe); }
        RawData_MotionLite RawData { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_MotionLite(); return RawData; }
        public override void Run(CancellationToken token)
        {
            try
            {
                double X_Position = 0;
                double Y_Position = 0;
                double Z_Position = 0;
                //校验点位长度
                if (this.Position.ItemCollection.Count != 1)
                {
                    throw new Exception($"点位[{this.Position}]串行移动错误:该点位所需的轴数量为[{this.Position.ItemCollection.Count}],实际需要一个轴!");
                }
                ////只允许添加左侧测试平台的轴
                //if (!this.Axis_X.Name.ToUpper().Contains("TEST"))
                //{
                //    throw new Exception($"轴[{this.Axis_X.Name}]串行移动错误:该轴不是左侧平台的XYZ轴");
                //}
                //if (!this.Axis_Y.Name.ToUpper().Contains("TEST"))
                //{
                //    throw new Exception($"轴[{this.Axis_Y.Name}]串行移动错误:该轴不是左侧平台的XYZ轴");
                //}
                //if (!this.Axis_Z.Name.ToUpper().Contains("TEST"))
                //{
                //    throw new Exception($"轴[{this.Axis_Z.Name}]串行移动错误:该轴不是左侧平台的XYZ轴");
                //}

                bool isVerified = true;
                string ErrMsg = string.Empty;
                //校验点位所需轴和提取轴位置
                foreach (var item in this.Position)// TestX_测试LX轴
                {

                    if (Axis.Name == item.Name)
                    {
                        X_Position = item.Position;
                        continue;
                    }
                    //else if (Axis_2nd.Name == item.Name)
                    //{
                    //    Y_Position = item.Position;
                    //    continue;
                    //}
                    //else if (Axis_3rd.Name == item.Name)
                    //{
                    //    Z_Position = item.Position;
                    //    continue;
                    //}
                    else
                    {
                        isVerified = false;
                        ErrMsg = ($"点位[{this.Position}]串行移动错误:该点位所需的轴为[{item.Name}],加载的轴名为[{Axis.Name}]!");
                        break;
                    }
                }
                if(isVerified == false)
                {
                    throw new Exception(ErrMsg);
                }
 

                //按逻辑运行三个轴
                //double aa=this.Axis_X.Get_CurUnitPos();//获取轴当前位置
                this.Axis.MoveToV3(X_Position, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                Thread.Sleep(100);
                // Parallel.Invoke(
                //() =>
                //{
                //    this.Axis_Y.MoveToV3(Y_Position, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                //},
                //() =>
                //{
                //    this.Axis_Z.MoveToV3(Z_Position, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                //});

                //this.Axis_2nd.MoveToV3(Y_Position, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);

                //this.Axis_3rd.MoveToV3(Z_Position, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
            }
            catch (Exception ex)
            {
                this._core.ReportException("单轴模块运行错误", ErrorCodes.TestModuleRuntimeExceptionRaised, ex);
               // throw ex;
            }
        }
    }
}