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
    [ConfigurableAxis("Axis_1st", "主导轨1st轴_需要回零的轴")]
    [ConfigurableAxis("Axis_2nd", "主导轨2nd轴")]
    [ConfigurableAxis("Axis_3rd", "主导轨3rd轴")]

    [ConfigurablePosition("Position", "运动点位")]
    //[ConfigurablePosition("Position_V", "远场扫描V轴中点")]
 
 
    public class TestModule_MotionTest_ThreeAxis : TestModuleBase
    {
        public TestModule_MotionTest_ThreeAxis() : base() { }
        #region 以get属性获取测试模块运行所需资源
        //IScanner Scanner_1 { get { return (IScanner)this.ModuleResource["Scanner_1"]; } }
        MotorAxisBase Axis_1st { get { return (MotorAxisBase)this.ModuleResource["Axis_1st"]; } }//1st 
        MotorAxisBase Axis_2nd { get { return (MotorAxisBase)this.ModuleResource["Axis_2nd"]; } }//2nd 
        MotorAxisBase Axis_3rd { get { return (MotorAxisBase)this.ModuleResource["Axis_3rd"]; } }//3rd
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
                double Position_1st = 0;
                double Position_2nd = 0;
                double Position_3rd = 0;
                //校验点位长度
                if (this.Position.ItemCollection.Count != 3)
                {
                    throw new Exception($"点位[{this.Position}]串行移动错误:该点位所需的轴数量为[{this.Position.ItemCollection.Count}],实际需要三个轴!");
                }
                //只允许添加左侧测试平台的轴
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
                    if (!item.Name.ToUpper().Contains("TEST"))
                    {
                        throw new Exception($"轴[{item.Name}]串行移动错误:该轴不是左侧平台的XYZ轴");
                    }

                    if (Axis_1st.Name == item.Name)
                    {
                        Position_1st = item.Position;
                        continue;
                    }
                    else if (Axis_2nd.Name == item.Name)
                    {
                        Position_2nd = item.Position;
                        continue;
                    }
                    else if (Axis_3rd.Name == item.Name)
                    {
                        Position_3rd = item.Position;
                        continue;
                    }
                    else
                    {
                        ErrMsg = ($"点位[{this.Position}]串行移动错误:该点位所需的轴为[{item.Name}],加载的轴名分别为[{Axis_1st.Name}]&&[{Axis_2nd.Name}]&&[{Axis_3rd.Name}]!");
                        isVerified = false;
                        break;
                    }
                }
                if(isVerified == false)
                {
                    throw new Exception(ErrMsg);
                }
 

                //按逻辑运行三个轴
               // double aa=this.Axis_1st.Get_CurUnitPos();//获取轴当前位置
                this.Axis_1st.MoveToV3(0, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);//先回安全0位
               // bool _isok=this.Axis_1st.HomeRun();
                int err = this.Axis_1st.WaitMotionDone();
                if (err == ErrorCodes.MotorMoveTimeOutError/*|| !_isok*/)
                {
                    this.Log_Global($"轴[{this.Axis_1st.Name}]运动超时！");
                    throw new Exception(string.Format("轴{0}运动超时！", this.Axis_1st.Name));
                }
                Thread.Sleep(500);
                // Parallel.Invoke(
                //() =>
                //{
                //    this.Axis_Y.MoveToV3(Y_Position, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                //},
                //() =>
                //{
                //    this.Axis_Z.MoveToV3(Z_Position, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                //});

                this.Axis_2nd.MoveToV3(Position_2nd, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);  
                //Thread.Sleep(50);
                this.Axis_3rd.MoveToV3(Position_3rd, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal); 
                Thread.Sleep(50);
                int err2 = this.Axis_2nd.WaitMotionDone();
                if (err == ErrorCodes.MotorMoveTimeOutError)
                {
                    this.Log_Global($"轴[{this.Axis_2nd.Name}]运动超时！");
                    throw new Exception(string.Format("轴{0}运动超时！", this.Axis_2nd.Name));
                }
                err = this.Axis_3rd.WaitMotionDone();
                if (err == ErrorCodes.MotorMoveTimeOutError)
                {
                    this.Log_Global($"轴[{this.Axis_3rd.Name}]运动超时！");
                    throw new Exception(string.Format("轴{0}运动超时！", this.Axis_3rd.Name));
                }
                //Thread.Sleep(5000);
                this.Axis_1st.MoveToV3(Position_1st, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                err = this.Axis_1st.WaitMotionDone();
                if (err == ErrorCodes.MotorMoveTimeOutError)
                {
                    this.Log_Global($"轴[{this.Axis_1st.Name}]运动超时！");
                    throw new Exception(string.Format("轴{0}运动超时！", this.Axis_1st.Name));
                }
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                this._core.ReportException("三轴模块运行错误", ErrorCodes.Module_Motion_Failed, ex);
                //throw ex;
            }
        }
    }
}