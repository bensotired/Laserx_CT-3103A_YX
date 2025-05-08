using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_Per_1")]

    #region 轴、位置、IO、仪器
    [StaticResource(ResourceItemType.AXIS, "左偏振片旋转", "左偏振片")]
    [StaticResource(ResourceItemType.AXIS, "左短摆臂旋转", "左短摆臂")]
    [StaticResource(ResourceItemType.AXIS, "左长摆臂旋转", "左长摆臂")]


    [StaticResource(ResourceItemType.IO, "Input_左PER前后移动气缸动作", "左PER前后动作")]
    [StaticResource(ResourceItemType.IO, "Input_左PER前后移动气缸复位", "左PER前后复位")]
    [StaticResource(ResourceItemType.IO, "Input_左PER避位气缸动作", "左PER避位气缸动作")]
    [StaticResource(ResourceItemType.IO, "Input_左PER避位气缸复位", "左PER避位气缸复位")]

    [StaticResource(ResourceItemType.IO, "Input_左PER上下移动气缸动作", "左PER上下动作")]
    [StaticResource(ResourceItemType.IO, "Input_左PER上下移动气缸复位", "左PER上下复位")]

    [StaticResource(ResourceItemType.IO, "Output_左PER前后移动气缸电磁阀", "左PER前后电磁阀")]
    [StaticResource(ResourceItemType.IO, "Output_左PER避位气缸电磁阀", "左PER避位电磁阀")]

    [StaticResource(ResourceItemType.IO, "Output_左PER上下移动气缸电磁阀_下降", "左PER上下_下降")]
    [StaticResource(ResourceItemType.IO, "Output_左PER上下移动气缸电磁阀_上升", "左PER上下_上升")]

    [StaticResource(ResourceItemType.IO, "Output_工站一采集卡", "切换PD通道")]

    [ConfigurableInstrument("ISourceMeter_Golight", "GoLight_1", "用于驱动器件")]
    [ConfigurableInstrument("LaserX_SMU_3133", "LX_3133", "用于数据采集")]
    #endregion
    public class TestModule_PeaPer_1 : TestModuleBase
    {
        #region 以Get获取资源
        MotorAxisBase LeftPer { get { return (MotorAxisBase)this.ModuleResource["左偏振片旋转"]; } }
        MotorAxisBase LeftShort { get { return (MotorAxisBase)this.ModuleResource["左短摆臂旋转"]; } }
        MotorAxisBase LeftLong { get { return (MotorAxisBase)this.ModuleResource["左长摆臂旋转"]; } }
        IOBase In_LeftPerFrontAction { get { return (IOBase)this.ModuleResource["Input_左PER前后移动气缸动作"]; } }
        IOBase In_LeftPerFrontRest { get { return (IOBase)this.ModuleResource["Input_左PER前后移动气缸复位"]; } }
        IOBase In_LeftPerAvoidAction { get { return (IOBase)this.ModuleResource["Input_左PER避位气缸动作"]; } }
        IOBase In_LeftPerAvoidRest { get { return (IOBase)this.ModuleResource["Input_左PER避位气缸复位"]; } }

        IOBase In_LeftPerAction { get { return (IOBase)this.ModuleResource["Input_左PER上下移动气缸动作"]; } }
        IOBase In_LeftPerRest { get { return (IOBase)this.ModuleResource["Input_左PER上下移动气缸复位"]; } }

        IOBase Out_LeftPerFront { get { return (IOBase)this.ModuleResource["Output_左PER前后移动气缸电磁阀"]; } }
        IOBase Out_LeftPerAvoid { get { return (IOBase)this.ModuleResource["Output_左PER避位气缸电磁阀"]; } }
        IOBase Out_LeftPerDown { get { return (IOBase)this.ModuleResource["Output_左PER上下移动气缸电磁阀_下降"]; } }
        IOBase Out_LeftPerUp { get { return (IOBase)this.ModuleResource["Output_左PER上下移动气缸电磁阀_上升"]; } }

        IOBase Out_3133PD { get { return (IOBase)this.ModuleResource["Output_工站一采集卡"]; } }

        ISourceMeter_Golight sourceMeter { get { return (ISourceMeter_Golight)this.ModuleResource["GoLight_1"]; } }
        LaserX_SMU_3133 LX_3133 { get { return (LaserX_SMU_3133)this.ModuleResource["LX_3133"]; } }

        #endregion

        TestRecipe_PeaPer_1 TestRecipe { get; set; }
        RawData_PeaPer RawData { get; set; }
        public TestModule_PeaPer_1() : base() { }

        public override Type GetTestRecipeType() { return typeof(TestRecipe_PeaPer_1); }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_PeaPer_1>(testRecipe);
        }


        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_PeaPer();
            return RawData;
        }


        public override void Run(CancellationToken token)
        {
            try
            {
                if (this.sourceMeter == null)
                {
                    this._core.Log_Global("仪器连接错误！！！");
                    return;
                }
                if (!this.sourceMeter.IsOnline)
                {
                    this.Log_Global("仪表异常，取消测试！");
                    sourceMeter.Timeout_ms = 1000;
                    sourceMeter.CurrentSetpoint_A = 0;
                    sourceMeter.VoltageSetpoint_V = 0;
                    sourceMeter.VoltageSetpoint_PD_V = 0;
                    sourceMeter.VoltageSetpoint_EA_V = 0;
                    sourceMeter.IsOutputEnable = false;
                    return;
                }
                sourceMeter.Timeout_ms = 60 * 1000;

                Out_3133PD.TurnOn(true);
                #region 气缸
                if (!In_LeftPerFrontRest.Interation.IsActive)
                {
                    Out_LeftPerFront.TurnOn(false);
                }
                if (!In_LeftPerAvoidRest.Interation.IsActive)
                {
                    Out_LeftPerAvoid.TurnOn(false);
                }
                if (!In_LeftPerRest.Interation.IsActive)
                {
                    Out_LeftPerDown.TurnOn(false);
                    Out_LeftPerUp.TurnOn(true);
                }
                if (LeftShort.Get_CurUnitPos() != 0)
                {
                    LeftShort.HomeRun();
                    LeftShort.WaitHomeDone(new CancellationTokenSource());
                }
                if (LeftLong.Get_CurUnitPos() != 0)
                {
                    LeftLong.HomeRun();
                    LeftLong.WaitHomeDone(new CancellationTokenSource());
                }

                Out_LeftPerDown.TurnOn(true);
                Out_LeftPerUp.TurnOn(false);
                Out_LeftPerAvoid.TurnOn(true);

                int count = 0;
                while (true)
                {
                    count++;
                    Thread.Sleep(500);
                    if (In_LeftPerAction.Interation.IsActive)
                    {
                        break;
                    }
                    if (count > 5)
                    {
                        var mess = MessageBox.Show("左PER上下移动气缸感应异常，请人工确认", "人工确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (mess == DialogResult.OK)
                        {
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                }
                count = 0;
                while (true)
                {
                    count++;
                    Thread.Sleep(500);
                    if (In_LeftPerAvoidAction.Interation.IsActive)
                    {
                        break;
                    }
                    if (count > 5)
                    {
                        var mess = MessageBox.Show("左PER避位气缸感应异常，请人工确认", "继续", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (mess == DialogResult.OK)
                        {
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                }
                #endregion

                this.Log_Global($"开始测试!");
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消测试！");
                    token.ThrowIfCancellationRequested();
                    return;
                }

                if (this.TestRecipe.isTriggerModeEnable)
                {
                    //脉冲模式
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    this._core.Log_Global("开始加电");
                    sourceMeter.IsOutputEnable = true;
                    sourceMeter.CurrentSetpoint_A = Convert.ToSingle(this.TestRecipe.I_A);

                    #region 正向第一扇区
                    //
                    Thread.Sleep(20);
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    var startPosition = this.TestRecipe.CentralAngle_1 - this.TestRecipe.AcqDegStart_RoughSweep;
                    var stopPosition = this.TestRecipe.CentralAngle_1 + this.TestRecipe.AcqDegEnd_RoughSweep;

                    var stepDeg = this.TestRecipe.AcqStepDeg_RoughSweep;



                    List<double> abc1 = new List<double>();
                    List<double> abc2 = new List<double>();
                    List<double> abc3 = new List<double>();

                    abc1.Add(33);
                    abc2.Add(44);
                    abc3.Add(55);



                    List<double> posiList1 = new List<double>();


                    int times_deg = 0;
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消测试！");
                            token.ThrowIfCancellationRequested();
                            return;
                        }
                        var posi = startPosition + stepDeg * times_deg;
                        if (posi > stopPosition)
                        {
                            break;
                        }
                        posiList1.Add(posi);
                        times_deg++;
                    }
                    if (!posiList1.Contains(stopPosition))
                    {
                        posiList1.Add(stopPosition);
                    }


                    double sweepSpeed = this.TestRecipe.SweepSpeed;//速度参数可以开放出来，小于自动运行速度

                    double stepTime_s = 1 / sweepSpeed;

                    double acqTime_s = Math.Round(stepTime_s * stepDeg * 0.5, 6);    //0-1



                    int CardNo = 1;
                    int triggerNo = 7;
                    int samplNo = 0;

                    LX_3133.PDAndMPDRangeAdjust(this.TestRecipe.Range1, CardNo);
                    this.Log_Global($"正向第一扇区，开始采集");
                    //仪器数据
                    Action daqAct_Rough_1_2 = new Action(() =>
                    {
                        LX_3133.PeaPer_Acq_PD(acqTime_s, posiList1.Count, triggerNo, samplNo, CardNo);
                    });




                    LeftPer.MoveToV3(startPosition - 0.2, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    var errCode = LeftPer.WaitMotionDone();


                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }

                    daqAct_Rough_1_2.BeginInvoke(null, null);
                    Thread.Sleep(2000);

                    LaserX_9078_Cmp_Function cmp = new LaserX_9078_Cmp_Function(LeftPer as Motor_LaserX_9078, 0,
                        false,  //true 上升沿  
                        300);   //脉冲宽度
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    cmp.CmpMoveToV3(stopPosition + 0.2, posiList1.ToArray(), sweepSpeed);  //运行速度 度/s  .Select(item=> Convert.ToDouble(item)).ToArray()
                    // SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                    errCode = this.LeftPer.WaitMotionDone();
                    cmp.LaserX_9078_CmpAxis_Close();
                    cmp = null;
                    GC.Collect();

                    Thread.Sleep(500);

                    var data_1_2 = LX_3133.DataBook;
                    //
                    Thread.Sleep(20);
                    this.Log_Global($"正向第一扇区！{data_1_2.Count}");
                    var result_1_2 = Transform(GetData(data_1_2, posiList1), this.TestRecipe.Range1);
                    #endregion

                    Data result_3_4 = null;
                    Data result_4_3 = null;

                    if (this.TestRecipe.isDoubleEnable)
                    {
                        LX_3133.PDAndMPDRangeAdjust(this.TestRecipe.Range2, CardNo);

                        #region 正向第二扇区
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消测试！");
                            token.ThrowIfCancellationRequested();
                            return;
                        }
                        var startPosition2 = this.TestRecipe.CentralAngle_2 - this.TestRecipe.AcqDegStart_FineSweep;
                        var stopPosition2 = this.TestRecipe.CentralAngle_2 + this.TestRecipe.AcqDegEnd_FineSweep;


                        var stepDeg2 = this.TestRecipe.AcqStepDeg_FineSweep;

                        List<double> posiList2 = new List<double>();
                        int times_deg2 = 0;
                        while (true)
                        {
                            if (token.IsCancellationRequested)
                            {
                                this.Log_Global($"用户取消测试！");
                                token.ThrowIfCancellationRequested();
                                return;
                            }
                            var posi = startPosition2 + stepDeg2 * times_deg2;
                            if (posi > stopPosition2)
                            {
                                break;
                            }
                            posiList2.Add(posi);
                            times_deg2++;
                        }
                        if (!posiList2.Contains(stopPosition2))
                        {
                            posiList2.Add(stopPosition2);
                        }


                        Thread.Sleep(200);
                        this.Log_Global($"正向第二扇区，开始采集");
                        Action daqAct_Rough_3_4 = new Action(() =>
                        {
                            LX_3133.PeaPer_Acq_PD(acqTime_s, posiList2.Count, triggerNo, samplNo, CardNo);
                        });


                        LeftPer.MoveToV3(startPosition2 - 0.2, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        errCode = LeftPer.WaitMotionDone();


                        daqAct_Rough_3_4.BeginInvoke(null, null);
                        Thread.Sleep(2000);
                        cmp = new LaserX_9078_Cmp_Function(LeftPer as Motor_LaserX_9078, 0, false, 300);
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消测试！");
                            token.ThrowIfCancellationRequested();
                            return;
                        }
                        cmp.CmpMoveToV3(stopPosition2 + 0.2, posiList2.ToArray(), sweepSpeed);  //运行速度 度/s
                                                                                                // SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                        errCode = this.LeftPer.WaitMotionDone();
                        cmp.LaserX_9078_CmpAxis_Close();
                        cmp = null;
                        GC.Collect();
                        var data_3_4 = LX_3133.DataBook;
                        //
                        Thread.Sleep(20);

                        this.Log_Global($"正向第二扇区！{data_3_4.Count}");
                        result_3_4 = Transform(GetData(data_3_4, posiList2), this.TestRecipe.Range2);
                        #endregion

                        #region 反向第二扇区

                        this.Log_Global($"反向第二扇区，开始采集");
                        Action daqAct_Rough_4_3 = new Action(() =>
                        {
                            LX_3133.PeaPer_Acq_PD(acqTime_s, posiList2.Count, triggerNo, samplNo, CardNo);
                        });


                        LeftPer.MoveToV3(stopPosition2 + 0.2, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        errCode = LeftPer.WaitMotionDone();

                        daqAct_Rough_4_3.BeginInvoke(null, null);
                        Thread.Sleep(2000);
                        cmp = new LaserX_9078_Cmp_Function(LeftPer as Motor_LaserX_9078, 0, false, 300);
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消测试！");
                            token.ThrowIfCancellationRequested();
                            return;
                        }
                        posiList2.Reverse();
                        cmp.CmpMoveToV3(startPosition2 - 0.2, posiList2.ToArray(), sweepSpeed);  //运行速度 度/s
                                                                                                 // SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                        errCode = this.LeftPer.WaitMotionDone();
                        cmp.LaserX_9078_CmpAxis_Close();
                        cmp = null;
                        GC.Collect();
                        var data_4_3 = LX_3133.DataBook;
                        //
                        Thread.Sleep(20);
                        this.Log_Global($"反向第二扇区！{data_4_3.Count}");
                        result_4_3 = Transform(GetData(data_4_3, posiList2), this.TestRecipe.Range2);

                        #endregion

                        LX_3133.PDAndMPDRangeAdjust(this.TestRecipe.Range1, CardNo);
                    }


                    #region 反向第一扇区




                    this.Log_Global($"反向第一扇区，开始采集");
                    Action daqAct_Rough_2_1 = new Action(() =>
                    {
                        LX_3133.PeaPer_Acq_PD(acqTime_s, posiList1.Count, triggerNo, samplNo, CardNo);
                    });


                    LeftPer.MoveToV3(stopPosition + 0.2, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    errCode = LeftPer.WaitMotionDone();


                    daqAct_Rough_2_1.BeginInvoke(null, null);
                    Thread.Sleep(2000);
                    cmp = new LaserX_9078_Cmp_Function(LeftPer as Motor_LaserX_9078, 0, false, 300);
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    posiList1.Reverse();
                    cmp.CmpMoveToV3(startPosition - 0.2, posiList1.ToArray(), sweepSpeed); //运行速度 度/s
                    // SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                    errCode = this.LeftPer.WaitMotionDone();
                    cmp.LaserX_9078_CmpAxis_Close();
                    cmp = null;
                    GC.Collect();
                    var data_2_1 = LX_3133.DataBook;
                    //
                    Thread.Sleep(20);

                    this.Log_Global($"反向第一扇区！{data_2_1.Count}");
                    var result_2_1 = Transform(GetData(data_2_1, posiList1), this.TestRecipe.Range1);

                    #endregion

                    LeftPer.MoveToV3(0, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    LeftPer.WaitMotionDone();

                    double[] theta1 = new double[posiList1.Count];
                    double[] smooth1 = new double[posiList1.Count];
                    Calculate(result_1_2, result_2_1, out theta1, out smooth1);
                    for (int i = 0; i < theta1.Length; i++)
                    {
                        RawData.Add(new RawDatumItem_PeaPer()
                        {
                            Drgree = Convert.ToSingle(theta1[i]),
                            Power_mW = (Convert.ToSingle(smooth1[i]) * this.TestRecipe.Factor_K_1st + this.TestRecipe.Factor_B_1st)
                        });
                    }

                    if (this.TestRecipe.isDoubleEnable)
                    {
                        double[] theta2 = new double[posiList1.Count];
                        double[] smooth2 = new double[posiList1.Count];
                        Calculate(result_3_4, result_4_3, out theta2, out smooth2);
                        for (int i = 0; i < theta1.Length; i++)
                        {
                            RawData.Add(new RawDatumItem_PeaPer()
                            {
                                Drgree = Convert.ToSingle(theta2[i]),
                                Power_mW = (Convert.ToSingle(smooth2[i]) * this.TestRecipe.Factor_K_2ed + this.TestRecipe.Factor_B_2ed)
                            });
                        }
                    }




                }
                else
                {
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    LeftPer.MoveToV3(this.TestRecipe.CentralAngle_1, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    LeftPer.WaitMotionDone();
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    this._core.Log_Global("开始加电");
                    sourceMeter.IsOutputEnable = true;
                    sourceMeter.CurrentSetpoint_A = Convert.ToSingle(this.TestRecipe.I_A);
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }

                    List<float> angle = new List<float>();
                    List<float> pdCurrs_mA = new List<float>();

                    float startc = Convert.ToSingle(this.TestRecipe.CentralAngle_1 - this.TestRecipe.AcqDegStart_RoughSweep);
                    float stepc = Convert.ToSingle(this.TestRecipe.AcqStepDeg_RoughSweep);
                    float stopc = Convert.ToSingle(this.TestRecipe.CentralAngle_1 + this.TestRecipe.AcqDegEnd_RoughSweep);

                    for (float i = startc; i <= stopc; i += stepc)
                    {
                        LeftPer.MoveToV3(i, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        LeftPer.WaitMotionDone();
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消测试！");
                            token.ThrowIfCancellationRequested();
                            return;
                        }
                        angle.Add(i);
                        pdCurrs_mA.Add(this.sourceMeter.ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL.CH1) * this.TestRecipe.Factor_K_1st + this.TestRecipe.Factor_B_1st);

                    }

                    LeftPer.MoveToV3(this.TestRecipe.CentralAngle_2, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    LeftPer.WaitMotionDone();
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    float startc2 = Convert.ToSingle(this.TestRecipe.CentralAngle_2 - this.TestRecipe.AcqDegStart_FineSweep);
                    float stepc2 = Convert.ToSingle(this.TestRecipe.AcqStepDeg_FineSweep);
                    float stopc2 = Convert.ToSingle(this.TestRecipe.CentralAngle_2 + this.TestRecipe.AcqDegEnd_FineSweep);

                    for (float m = startc2; m <= stopc2; m += stepc2)
                    {
                        LeftPer.MoveToV3(m, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        LeftPer.WaitMotionDone();
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消测试！");
                            token.ThrowIfCancellationRequested();
                            return;
                        }
                        angle.Add(m);
                        pdCurrs_mA.Add(this.sourceMeter.ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL.CH1) * this.TestRecipe.Factor_K_2ed + this.TestRecipe.Factor_B_2ed);
                    }

                    GetMaxMin(RawData, angle, pdCurrs_mA);

                    for (int i = 0; i < angle.Count; i++)
                    {
                        RawData.Add(new RawDatumItem_PeaPer()
                        {
                            Drgree = angle[i],
                            Power_mW = pdCurrs_mA[i],
                        });
                    }

                    LeftPer.MoveToV3(0, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                    LeftPer.WaitMotionDone();




                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                sourceMeter.IsOutputEnable = false;

                Out_3133PD.TurnOn(false);
                Out_LeftPerDown.TurnOn(false);
                Out_LeftPerUp.TurnOn(true);
                Out_LeftPerAvoid.TurnOn(false);
                Thread.Sleep(1500);

                this.Log_Global($"结束测试!");
            }
        }
        public Data Transform(Data result, PDCurrentRange pD)
        {
            for (int i = 0; i < result.PDCurrentArr.Count; i++)
            {
                switch (pD)
                {
                    case PDCurrentRange.档位1_20uA:
                        {
                            result.PDCurrentArr[i] = result.PDCurrentArr[i] * 0.01;
                        }
                        break;
                    case PDCurrentRange.档位2_200uA:
                        {
                            result.PDCurrentArr[i] = result.PDCurrentArr[i] * 0.1;
                        }
                        break;
                    case PDCurrentRange.档位3_2mA:
                        break;
                    case PDCurrentRange.档位4_20mA:
                        {
                            result.PDCurrentArr[i] = result.PDCurrentArr[i] * 10;
                        }
                        break;
                }


            }


            return result;
        }
        public void GetMaxMin(RawData_PeaPer rawData, List<float> angle, List<float> pd)
        {
            int indexMax = 0;
            int indexMin = 0;
            var pdmax = pd.Max();
            rawData.PDMax = pdmax;
            for (int i = 0; i < pd.Count; i++)
            {
                if (pd[i] == pdmax)
                {
                    indexMax = i;
                    break;
                }
            }
            rawData.DegPosition_PDMax = angle[indexMax];

            var pdmin = pd.Min();
            rawData.PDMin = pdmin;
            for (int i = 0; i < pd.Count; i++)
            {
                if (pd[i] == pdmin)
                {
                    indexMin = i;
                    break;
                }
            }
            rawData.DegPosition_PDMin = angle[indexMin];

        }
        /// <summary>
        /// 拟合
        /// </summary>
        public void Calculate(Data result_P1_P2, Data result_P2_P1, out double[] outthetaList, out double[] outsmoothList)
        {
            //P1->P2
            List<double> pList_P1_P2 = result_P1_P2.PDCurrentArr;
            List<double> thetaList_P1_P2 = result_P1_P2.AxisPos;

            //移动平均法平滑
            List<double> smoothList_P1_P2 = new List<double>();
            //smoothList_P1_P2.AddRange(ArrayMath.CalculateMovingAverage(pList_P1_P2.ToArray(), 11));
            smoothList_P1_P2.AddRange(pList_P1_P2.ToArray());

            //P2->P1
            List<double> pList_P2_P1 = result_P2_P1.PDCurrentArr;
            List<double> thetaList_P2_P1 = result_P2_P1.AxisPos;

            //移动平均法平滑
            List<double> smoothList_P2_P1 = new List<double>();
            //smoothList_P2_P1.AddRange(ArrayMath.CalculateMovingAverage(pList_P2_P1.ToArray(), 11));
            smoothList_P2_P1.AddRange(pList_P2_P1.ToArray());

            //翻回来
            thetaList_P2_P1.Reverse();
            smoothList_P2_P1.Reverse();

            //截取相同数量
            int PCnt = Math.Min(smoothList_P1_P2.Count, smoothList_P2_P1.Count);
            thetaList_P1_P2.RemoveRange(PCnt, thetaList_P1_P2.Count - PCnt);
            thetaList_P2_P1.RemoveRange(PCnt, thetaList_P2_P1.Count - PCnt);
            smoothList_P1_P2.RemoveRange(PCnt, smoothList_P1_P2.Count - PCnt);
            smoothList_P2_P1.RemoveRange(PCnt, smoothList_P2_P1.Count - PCnt);

            List<double> thetaList = new List<double>();
            List<double> smoothList = new List<double>();
            {
                //曲线平移偏移量

                //阈值设定
                double Th_P1_P2 = (smoothList_P1_P2.Max() - smoothList_P1_P2.Min()) / 2 + smoothList_P1_P2.Min();
                double Th_P2_P1 = (smoothList_P2_P1.Max() - smoothList_P2_P1.Min()) / 2 + smoothList_P2_P1.Min();

                //总偏移量
                int OffsetCount = 0;
                for (int i = 0; i < PCnt; i++)
                {
                    double diff1 = 0;
                    if (smoothList_P1_P2[i] > Th_P1_P2) diff1 = 1;
                    double diff2 = 0;
                    if (smoothList_P2_P1[i] > Th_P2_P1) diff2 = 1;
                    if (diff1 != diff2) OffsetCount++;
                }

                //得到曲线应该的偏移量
                OffsetCount /= 4;

                var smoothList_P1_P2_L = CloneHelper.Clone<List<double>>(smoothList_P1_P2);
                var smoothList_P1_P2_R = CloneHelper.Clone<List<double>>(smoothList_P1_P2);

                var smoothList_P2_P1_L = CloneHelper.Clone<List<double>>(smoothList_P2_P1);
                var smoothList_P2_P1_R = CloneHelper.Clone<List<double>>(smoothList_P2_P1);

                double[] ZeroData = new double[OffsetCount];

                //移动数据
                smoothList_P1_P2_L.RemoveRange(0, OffsetCount);
                smoothList_P1_P2_L.AddRange(ZeroData);

                smoothList_P1_P2_R.InsertRange(0, ZeroData);
                smoothList_P1_P2_R.RemoveRange(PCnt, OffsetCount);

                smoothList_P2_P1_L.RemoveRange(0, OffsetCount);
                smoothList_P2_P1_L.AddRange(ZeroData);

                smoothList_P2_P1_R.InsertRange(0, ZeroData);
                smoothList_P2_P1_R.RemoveRange(PCnt, OffsetCount);

                //计算2组差值
                double[] ListDiff = new double[2];
                for (int i = OffsetCount; i < PCnt - OffsetCount; i++)
                {
                    ListDiff[0] += Math.Abs(smoothList_P1_P2_L[i] - smoothList_P2_P1_R[i]);
                    ListDiff[1] += Math.Abs(smoothList_P1_P2_R[i] - smoothList_P2_P1_L[i]);
                }

                //找到2组中求和最小的组
                smoothList = new List<double>();//进行数据合并
                if (ListDiff[0] <= ListDiff[1])
                {
                    smoothList_P1_P2 = smoothList_P1_P2_L;
                    smoothList_P2_P1 = smoothList_P2_P1_R;

                    int i = 0;
                    for (i = 0; i < OffsetCount; i++)
                    {
                        smoothList.Add(smoothList_P1_P2[i]);
                    }
                    for (; i < PCnt - OffsetCount; i++)
                    {
                        smoothList.Add((smoothList_P1_P2[i] + smoothList_P2_P1[i]) / 2);
                    }
                    for (; i < PCnt; i++)
                    {
                        smoothList.Add(smoothList_P2_P1[i]);
                    }
                }
                else
                {
                    smoothList_P1_P2 = smoothList_P1_P2_R;
                    smoothList_P2_P1 = smoothList_P2_P1_L;

                    int i = 0;
                    for (i = 0; i < OffsetCount; i++)
                    {
                        smoothList.Add(smoothList_P2_P1[i]);
                    }
                    for (; i < PCnt - OffsetCount; i++)
                    {
                        smoothList.Add((smoothList_P1_P2[i] + smoothList_P2_P1[i]) / 2);
                    }
                    for (; i < PCnt; i++)
                    {
                        smoothList.Add(smoothList_P1_P2[i]);
                    }
                }
            }

            foreach (var item in result_P1_P2.AxisPos)
            {
                //if (item.Key.Name == FFAxis.Name)
                //{
                thetaList.Add(item);
                //}
            }

            ////计算出平滑点数量
            int SmoothCount = 0;
            //if (this.TestModuleRecipe.MovingSmooth_Angle != 0)
            //{
            //    double C_DegC = thetaList[thetaList.Count / 2];

            //    for (int i = thetaList.Count / 2; i > 0; i--)
            //    {
            //        if (Math.Abs(C_DegC - thetaList[i]) >= this.TestModuleRecipe.MovingSmooth_Angle)
            //        {
            //            SmoothCount = Math.Abs(thetaList.Count / 2 - i);
            //            break;
            //        }
            //    }
            //}

            ////移动平均法平滑
            double[] smoothList_2;
            if (SmoothCount == 0)
            {
                smoothList_2 = smoothList.ToArray();
            }
            else
            {
                smoothList_2 = ArrayMath.CalculateMovingAverage(smoothList.ToArray(), SmoothCount);
            }


            outthetaList = thetaList.ToArray();
            outsmoothList = smoothList_2;
        }

        public Data GetData(List<List<double[]>> databook, List<double> posiList)
        {
            Data data = new Data();
            foreach (var item in databook)
            {
                foreach (var it in item)
                {
                    data.PDCurrentArr.Add(it.Average());
                }
            }
            for (int i = 0; i < posiList.Count; i++)
            {
                data.AxisPos.Add(posiList[i]);
            }

            return data;
        }
    }
    public class Data
    {

        public List<double> PDCurrentArr;
        public List<double> AxisPos;

        public Data()
        {
            PDCurrentArr = new List<double>();
            AxisPos = new List<double>();
        }
    }

}
