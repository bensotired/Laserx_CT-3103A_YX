using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_NanoTrakAlignment_LX__AvgCurrent_mA",
        "TestCalculator_NanoTrakAlignment_LX__AveragPower_mW"
        )]
    
    [ConfigurableInstrument("Keithley2602B", "Source_2602B", "用于驱动器件")]
    
    [ConfigurableInstrument("Thorlabs_NanoTrak", "NanoTrak_LX", "用于LX端耦合")]

    [ConfigurableAxis("TestY_测试LY轴", "TestY_测试LY轴_耦合时移动")]

    [ConfigurablePosition("耦合测试位", "耦合测试位_耦合时移动")]
    public class TestModule_NanoTrakAlignment_LX : TestModuleBase
    {
        public TestModule_NanoTrakAlignment_LX() : base() { }
        #region 以get属性获取测试模块运行所需资源
        Keithley2602B SourceMeter_Master { get { return (Keithley2602B)this.ModuleResource["Source_2602B"]; } }
        Thorlabs_NanoTrak NanoTrak { get { return (Thorlabs_NanoTrak)this.ModuleResource["NanoTrak_LX"]; } }
        MotorAxisBase TestLYAxis { get { return (MotorAxisBase)this.ModuleResource["TestY_测试LY轴"]; } }
        AxesPosition AlignmentPos { get { return (AxesPosition)this.ModuleResource["耦合测试位"]; } } 

        #endregion

        TestRecipe_NanoTrakAlignment_LX TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_NanoTrakAlignment_LX); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_NanoTrakAlignment_LX>(testRecipe); }
        RawData_NanoTrakAlignment_LX RawData { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_NanoTrakAlignment_LX(); return RawData; }
        public override void Run(CancellationToken token)
        {
            try
            {
                if (SourceMeter_Master.IsOnline == false || SourceMeter_Master == null)
                {
                    Log_Global($"仪器[{SourceMeter_Master.Name}]状态为[{SourceMeter_Master.IsOnline}]");
                    return;
                }
                if (NanoTrak.IsOnline == false || NanoTrak == null)
                {
                    Log_Global($"仪器[{NanoTrak.Name}]状态为[{NanoTrak.IsOnline}]");
                    return;
                }               
                this.Log_Global("开始进行耦合...");
                ///需根据实际使用条件定制
                this.Log_Global($"开始上电...[{this.TestRecipe.DrivingCurrent_mA}]mA...");
                #region  点位和轴校验
                if (this.AlignmentPos.ItemCollection.Count != 3)
                {
                    throw new Exception($"点位[{this.AlignmentPos}]串行移动错误:该点位所需的轴数量为[{this.AlignmentPos.ItemCollection.Count}],实际需要三个轴!");
                }
                bool isVerified = true;
                string ErrMsg = string.Empty;
                int Count = 0;
                //得到设定的原始0度
                double StarAlignmentPos = 0;
                //校验点位所需轴和提取轴位置
                foreach (var item in this.AlignmentPos)// TestX_测试LY轴
                {
                    Count++;
                    if (TestLYAxis.Name == item.Name)
                    {
                        StarAlignmentPos = item.Position;
                        break;
                    }
                    else if (Count == 3)
                    {
                        isVerified = false;
                        ErrMsg = ($"点位[{this.AlignmentPos}]串行移动错误:该点位所需的轴为[{item.Name}],加载的轴名为[{TestLYAxis.Name}]!");
                        break;
                    }
                }
                if (isVerified == false)
                {
                    throw new Exception(ErrMsg);
                }
                #endregion

                SourceMeter_Master.SetupSMU_LD(this.TestRecipe.DrivingCurrent_mA, 2.5); //默认2.5V
                NanoTrak.StartControl();

                #region 查找最大电流位置
                Dictionary<double, double> AlignmentCurrentList = new Dictionary<double, double>();
                List<double> NanoTrakCurrentList = new List<double>() { 0, 0, 0 };
                double average = 0;
                double FrontCurrent = 0;
                double BackCurrent = 0;
                int err = 0;
                //先向后找
                for (int i = 0; i < this.TestRecipe.MoveCount; i++)
                {
                    //先运动
                    this.TestLYAxis.MoveToV3(StarAlignmentPos - (i * 0.001), SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                     err = this.TestLYAxis.WaitMotionDone();
                    if (err == ErrorCodes.MotorMoveTimeOutError)
                    {
                        this.Log_Global($"轴[{this.TestLYAxis.Name}]运动超时！");
                        throw new Exception(string.Format("轴{0}运动超时！", this.TestLYAxis.Name));
                    }
                    //取平均值
                    NanoTrakCurrentList.Clear();
                    NanoTrakCurrentList = new List<double>() { 0, 0, 0 };
                    for (int step_mm = 0; step_mm <= 9; step_mm++)
                    {
                        NanoTrak.Track();

                        NanoTrak.SetCircleDiameter(2.0);
                        Thread.Sleep(200);

                        NanoTrak.SetCircleDiameter(1.0);

                        Thread.Sleep(200);
                        NanoTrak.SetCircleDiameter(0.5);

                        Thread.Sleep(200);
                        NanoTrak.Latch();


                        ///过程中读取功率啥的                 
                        double NanoTrakCurrent = NanoTrak.ReadCurrent();
                        if (step_mm % 3 == 0) NanoTrakCurrentList[0] = NanoTrakCurrent;
                        if (step_mm % 3 == 1) NanoTrakCurrentList[1] = NanoTrakCurrent;
                        if (step_mm % 3 == 2) NanoTrakCurrentList[2] = NanoTrakCurrent;

                        if (step_mm > 1/*&& step_mm % 3 == 0*/)
                        {
                            average = NanoTrakCurrentList.Average();
                            if (Math.Abs(average - NanoTrakCurrentList[0]) < this.TestRecipe.Three_Current_mA_Diff
                                && Math.Abs(average - NanoTrakCurrentList[1]) < this.TestRecipe.Three_Current_mA_Diff
                                && Math.Abs(average - NanoTrakCurrentList[2]) < this.TestRecipe.Three_Current_mA_Diff)
                            {
                                this.Log_Global($"向前耦合的第[{i}]步动作完成最终光电流平均值为[{average}]...");
                                double NowPos = this.TestLYAxis.Get_CurUnitPos();
                                AlignmentCurrentList.Add(Math.Round(NowPos, 5), average);
                                if (average < FrontCurrent) break;
                                else FrontCurrent = average;
                                break;
                            }
                            else
                                continue;
                        }
                    }
                }
                //再向前找
                for (int i = 0; i < this.TestRecipe.MoveCount; i++)
                {
                    //先运动
                    double _newPos = StarAlignmentPos + (i + 1) * 0.001;//+1是避免重复一个位置
                    this.TestLYAxis.MoveToV3(_newPos, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                     err = this.TestLYAxis.WaitMotionDone();
                    if (err == ErrorCodes.MotorMoveTimeOutError)
                    {
                        this.Log_Global($"轴[{this.TestLYAxis.Name}]运动超时！");
                        throw new Exception(string.Format("轴{0}运动超时！", this.TestLYAxis.Name));
                    }
                    //取平均值
                    NanoTrakCurrentList.Clear();
                    NanoTrakCurrentList = new List<double>() { 0, 0, 0 };
                    for (int step_mm = 0; step_mm <= 9; step_mm++)
                    {
                        NanoTrak.Track();

                        NanoTrak.SetCircleDiameter(2.0);
                        Thread.Sleep(200);

                        NanoTrak.SetCircleDiameter(1.0);

                        Thread.Sleep(200);
                        NanoTrak.SetCircleDiameter(0.5);

                        Thread.Sleep(200);
                        NanoTrak.Latch();


                        ///过程中读取功率啥的                 
                        double NanoTrakCurrent = NanoTrak.ReadCurrent();
                        if (step_mm % 3 == 0) NanoTrakCurrentList[0] = NanoTrakCurrent;
                        if (step_mm % 3 == 1) NanoTrakCurrentList[1] = NanoTrakCurrent;
                        if (step_mm % 3 == 2) NanoTrakCurrentList[2] = NanoTrakCurrent;

                        if (step_mm > 1/*&& step_mm % 3 == 0*/)
                        {
                            average = NanoTrakCurrentList.Average();
                            if (Math.Abs(average - NanoTrakCurrentList[0]) < this.TestRecipe.Three_Current_mA_Diff
                                && Math.Abs(average - NanoTrakCurrentList[1]) < this.TestRecipe.Three_Current_mA_Diff
                                && Math.Abs(average - NanoTrakCurrentList[2]) < this.TestRecipe.Three_Current_mA_Diff)
                            {
                                this.Log_Global($"向后耦合的第[{i}]步动作完成最终光电流平均值为[{average}]...");
                                double NowPos = this.TestLYAxis.Get_CurUnitPos();
                                AlignmentCurrentList.Add(Math.Round(NowPos, 5), average);
                                if (average < BackCurrent) break;
                                else BackCurrent = average;
                                break;
                            }
                            else
                                continue;
                        }
                    }
                }
                #endregion

                //运动去最大光的位置
                double NewPos = AlignmentCurrentList.OrderByDescending(x => x.Value).First().Key;
                this.Log_Global($"找到最大光功率的轴位置为[{NewPos}]，耦合的最大电流值为[{AlignmentCurrentList[NewPos]}]！");
                this.TestLYAxis.MoveToV3(NewPos, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                 err = this.TestLYAxis.WaitMotionDone();
                if (err == ErrorCodes.MotorMoveTimeOutError)
                {
                    this.Log_Global($"轴[{this.TestLYAxis.Name}]运动超时！");
                    throw new Exception(string.Format("轴{0}运动超时！", this.TestLYAxis.Name));
                }

                Thread.Sleep(1000);
                //正式耦合
                int pos_index = 0;
                bool Alignment_ok = false;
                //你的耦合动作
                NanoTrakCurrentList.Clear();
                NanoTrakCurrentList = new List<double>() { 0, 0, 0 };
                for (int step_mm = 0; step_mm <= 27; step_mm++)
                {
                    NanoTrak.Track();

                    NanoTrak.SetCircleDiameter(2.0);
                    Thread.Sleep(200);

                    NanoTrak.SetCircleDiameter(1.0);

                    Thread.Sleep(200);
                    NanoTrak.SetCircleDiameter(0.5);

                    Thread.Sleep(200);
                    NanoTrak.Latch();


                    ///过程中读取功率啥的
                    //var pdCurr = this.SourceMeter_Master.ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL.CH1);
                    //var pdCurr = this.SourceMeter_Master.MeasureCurrent_A(Keithley2602BChannel.CHA); 
                   
                    double NanoTrakCurrent = NanoTrak.ReadCurrent();
                    if (step_mm % 3 == 0) NanoTrakCurrentList[0] = NanoTrakCurrent;
                    if (step_mm % 3 == 1) NanoTrakCurrentList[1] = NanoTrakCurrent;
                    if (step_mm % 3 == 2) NanoTrakCurrentList[2] = NanoTrakCurrent;

                    if (step_mm > 1/*&& step_mm % 3 == 0*/)
                    {
                        average = NanoTrakCurrentList.Average();
                        if (Math.Abs(average - NanoTrakCurrentList[0]) < this.TestRecipe.Three_Current_mA_Diff
                            && Math.Abs(average - NanoTrakCurrentList[1]) < this.TestRecipe.Three_Current_mA_Diff
                            && Math.Abs(average - NanoTrakCurrentList[2]) < this.TestRecipe.Three_Current_mA_Diff)
                        {
                            this.Log_Global($"耦合动作完成最终光电流平均值为[{average}]...");
                            Alignment_ok = true;
                            break;
                        }
                        else
                            continue;
                    }

                    //this.RawData.Add(new RawDatumItem_NanoTrakAlignment_LX
                    //{
                    //    Position_Index = ++pos_index,
                    //    Position_X_mm = step_mm,
                    //    Position_Y_mm = Math.Round(step_mm * Math.PI, 2),
                    //    Position_Z_mm = Math.Round(step_mm * Math.E, 2),
                    //    Power_mW = NanoTrakCurrent * this.TestRecipe.Power_Factor_K + this.TestRecipe.Power_Factor_B
                    //});
                }
                if (Alignment_ok == false)
                {
                    throw new Exception($"最终耦合失败，三次平均值为[{NanoTrakCurrentList.Average()}],设置差值为[{this.TestRecipe.Three_Current_mA_Diff}]轴{0}运动超时！" +
                         $"耦合列表值1为[{NanoTrakCurrentList[0]}],耦合列表值2为[{NanoTrakCurrentList[1]}],耦合列表值3为[{NanoTrakCurrentList[2]}],");
                }
                this.RawData.NanoTrakAverageCurrent_mA = average;
                this.RawData.NanoTrakAveragPower_mW = average*this.TestRecipe.Power_Factor_K+ this.TestRecipe.Power_Factor_B;
                // var finalData = this.RawData.DataCollection.Last();
                //this.RawData.Final_Power_mW = finalData.Power_mW;

                //this.RawData.Final_Position_X_mm = finalData.Position_X_mm;
                //this.RawData.Final_Position_Y_mm = finalData.Position_Y_mm;
                //this.RawData.Final_Position_Z_mm = finalData.Position_Z_mm;
                //下电
                //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(0.0);
                //this.SourceMeter_Master.IsOutputEnable = false;
                SourceMeter_Master.Reset();
                NanoTrak.StartControl();

                if (token.IsCancellationRequested)
                {
                    this.RawData.IsAlignmentDone = false;
                }
                else
                {
                    this.RawData.IsAlignmentDone = true;

                }
                this.Log_Global("TestModule_NanoTrakAlignment_LX 模块运行完成");
            }
            catch (Exception ex)
            {
                //下电
                //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(0.0);
                //this.SourceMeter_Master.IsOutputEnable = false;
                this.SourceMeter_Master.Reset();
                this._core.ReportException("耦合模块运行错误", ErrorCodes.Module_NanoTrakAlignment_LX_Failed, ex);
                ///根据模块的影响决定是否往上抛出错误
                ///throw ex;s
            }
        }
    }
}