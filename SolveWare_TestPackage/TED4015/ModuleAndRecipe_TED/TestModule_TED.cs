using LX_BurnInSolution.Utilities;
using Newtonsoft.Json.Linq;
using SolveWare_Analog;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static SolveWare_BurnInInstruments.LaserX_9078_Utilities;
using static SolveWare_TestPackage.LaserX_9078_Traj_Function;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_TED")]

    #region  轴、位置、IO、仪器
    [StaticResource(ResourceItemType.AXIS, "LY", "LY")]
    [StaticResource(ResourceItemType.IO, "TEC_Left", "左载台TEC电磁阀")]
    [StaticResource(ResourceItemType.IO, "TEC_Right", "右载台TEC电磁阀")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "左载台控温")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_2", "右载台控温")]
    [ConfigurableInstrument("TED4015", "TED4015", "产品控温")]
    #endregion
    public class TestModule_TED : TestModuleBase
    {
        public TestModule_TED() : base()
        {
        }

        #region 以Get获取资源
        MotorAxisBase LY { get { return (MotorAxisBase)this.ModuleResource["LY"]; } }
        IOBase TEC_Left { get { return (IOBase)this.ModuleResource["TEC_Left"]; } }
        IOBase TEC_Right { get { return (IOBase)this.ModuleResource["TEC_Right"]; } }
        MeerstetterTECController_1089 TC_1 { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }
        MeerstetterTECController_1089 TC_2 { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_2"]; } }
        TED4015 TED4015 { get { return (TED4015)this.ModuleResource["TED4015"]; } }

        #endregion 以Get获取资源

        private TestRecipe_TED TestRecipe { get; set; }
        private RawData_TED RawData { get; set; }
        RawDataMenu_TED RawDataMenu { get; set; }

        public override IRawDataBaseLite CreateRawData()
        {
            RawDataMenu = new RawDataMenu_TED();
            return RawDataMenu;
        }

        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_TED);
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_TED>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                MeerstetterTECController_1089 controller_1089 = null;
                IOBase iO = null;
                var LYPosition = LY.Get_CurUnitPos();
                if (LYPosition > 100)
                {
                    controller_1089 = TC_1;
                    iO = TEC_Left;
                }
                else
                {
                    controller_1089 = TC_2;
                    iO = TEC_Right;
                }
                if (controller_1089 == null && controller_1089.IsOnline == false)
                {
                    this.Log_Global($"TECController error");
                    return;
                }
                if (iO == null)
                {
                    this.Log_Global($"TEC Solenoid valve error");
                    return;
                }


                if (this.TestRecipe.OnOrOff)
                {
                    this.Log_Global($"开启控温 [{this.TestRecipe.TemperatureSetpoint}]C 范围[{this.TestRecipe.TemperatureTolerance}]C 时长[{this.TestRecipe.Temperature_StabilizingTime_sec}]Sec");

                    this.RawData = new RawData_TED();
                    this.RawData.TestStepStartTime = DateTime.Now;

                    TED4015.Reset();
                    int count = 0;
                    while (true)
                    {
                        Thread.Sleep(1000);
                        var error = TED4015.ErrorQuery();
                        if (error == 0)
                        {
                            break;
                        }
                        else
                        {

                            count++;
                        }
                        if (count > 4)
                        {
                            this.Log_Global($"TED4015 异常!");

                            throw new Exception("TED4015 异常!");
                        }
                    }


                    controller_1089.IsOutputEnabled = false;
                    iO.TurnOn(true);

                    TED4015.SetCurrentLimit(this.TestRecipe.CurrentLimit_mA);
                    TED4015.SetTemperatureSetpoint(this.TestRecipe.TemperatureSetpoint);

                    var pid = this.TestRecipe.PID.Split(',');
                    if (pid.Length != 4)
                    {
                        this.Log_Global($"PID Parameter error");
                    }
                    else
                    {

                        //TED4015.SetPID(double.Parse(pid[0]), double.Parse(pid[1]), double.Parse(pid[2]), double.Parse(pid[3]));
                    }

                    var rtb = this.TestRecipe.RTB.Split(',');
                    if (rtb.Length != 3)
                    {
                        this.Log_Global($"RTB Parameter error");
                    }
                    else
                    {

                        //TED4015.SetRTB(double.Parse(rtb[0]), double.Parse(rtb[1]), double.Parse(rtb[2]));
                    }

                    TED4015.IsOutPut(true);


                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Restart();

                    //温度数组
                    List<double> lstTemp = new List<double>();

                    var temp = TED4015.GetTempTemperature();

                    lstTemp.Add(temp);

                    while (true)
                    {
                        Thread.Sleep(100);

                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global("用户取消 Temperature Control.");
                            return;
                        }
                        var error = TED4015.ErrorQuery();
                        if (error != 0)
                        {
                            this.Log_Global($"TED4015 异常!");

                            throw new Exception("TED4015 异常!");
                        }
                        if (stopwatch.ElapsedMilliseconds > this.TestRecipe.Timeout_Min * 60 * 1000)
                        {
                            string TData = "";
                            foreach (var titem in lstTemp)
                            {
                                TData += titem.ToString() + ";";
                            }
                            this.Log_Global($"0.1秒温度历史:{TData}");


                            this.Log_Global($"控温超时, Timeout error , End temperature control and bounce out");
                            break;
                        }

                        temp = TED4015.GetTempTemperature();
                        //时间范围内
                        lstTemp.Add(temp);

                        //时间
                        if (stopwatch.ElapsedMilliseconds > this.TestRecipe.Temperature_StabilizingTime_sec * 1000)
                        {
                            //时间范围外
                            lstTemp.RemoveAt(0);

                        }



                        var Tolerance = Math.Abs(lstTemp.Max() - this.TestRecipe.TemperatureSetpoint);
                        Tolerance = Math.Max(Tolerance, Math.Abs(lstTemp.Max() - this.TestRecipe.TemperatureSetpoint));

                        if (Tolerance <= this.TestRecipe.TemperatureTolerance)
                        {
                            this.Log_Global($"Finish temperature control");
                            break;
                        }

                    }
                    stopwatch.Restart();
                    this.Log_Global($"Start temperature Penetration [{this.TestRecipe.TemperaturePenetration_S}] S");
                    while (stopwatch.ElapsedMilliseconds < this.TestRecipe.TemperaturePenetration_S * 1000)
                    {
                        Thread.Sleep(100);
                    }

                    this.RawData.TestStepEndTime = DateTime.Now;
                    this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;


                    //获取当前温度
                    temp = TED4015.GetTempTemperature();
                    this.RawDataMenu.FinishTemp_DegC = temp;

                    this.RawDataMenu.Add(RawData);
                }
                else
                {
                    this.Log_Global($"关闭控温");
                    TED4015.IsOutPut(false);
                    Thread.Sleep(500);
                    iO.TurnOn(false);
                    Thread.Sleep(500);
                    controller_1089.IsOutputEnabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {

            }
        }

    }
}