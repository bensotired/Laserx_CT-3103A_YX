using SolveWare_BurnInCommon;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Threading;
using SolveWare_TestPlugin;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using SolveWare_Vision;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103
    {
        public void LaserX_Step5_TurningOfMaterial(CancellationTokenSource tokenSource)
        {
            this.Bridges_WithPauseFunc[Action3103.LoadAndUnLoad].Set();
            while (true)
            {
                switch (this.Bridges_WithPauseFunc[Action3103.Step5Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {

                            this.Log_Global("[Step5]Step5Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step5]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);

                switch (this.Bridges_WithPauseFunc[Action3103.TestCompleteLeft].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                if (this.Status.LoadAndUnLoad == Operation.Idle)
                                {
                                    break;//上下料模块未被占用时跳出
                                }
                            }
 
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            //运行到装卸夹具的位置
                            //Parallel.Invoke(() =>
                            //{
                            //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_装卸, SequenceOrder.Normal);
                            //},
                            //() =>
                            //{
                            //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_测试, SequenceOrder.Normal);
                            //});


                            //Ben 2024/9/11==============================================================
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                            this.Log_Global($"[Step5]TestCompleteLeft，轴执行<RX_避让>点位移动完成");
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                            this.Log_Global($"[Step5]TestCompleteLeft，轴执行<LX_避让>点位移动完成");
                            //Ben 2024/9/11==============================================================

                          


                            if (this.TempListLeft.Count <= 0)//控温列表数量=0时，卸载夹具
                            {
                                while (true)
                                {
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    if (this.Status.LoadAndUnLoad == Operation.Idle)
                                    {
                                        break;
                                    }
                                }
                                this.Log_Global($"[Step5]控温结束，左载台切换夹具");
                                this.Status.UnLoadLeftStation = true;
                                this.Bridges_WithPauseFunc[Action3103.LoadAndUnLoad].Set();


                                //Ben 2024/9/11==============================================================
                                //线程1负责 下面4点顺序运动
                                //1.RX_避让
                                //2.RY_测试
                                //3.LY_装卸
                                //4.LXZ_装卸
                                //Ben 2024/9/11==============================================================

 
                                //Ben 2024/9/11==============================================================
                                var isLoadAndUnLoad_MotionDone = this.Bridges_WithPauseFunc[Action3103.LoadAndUnLoad_MotionDone].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.SuperLongTimeout_ms, tokenSource);

                                switch (isLoadAndUnLoad_MotionDone)
                                {
                                    case EventResult.TIMEOUT:
                                        {
                                            this.Log_Global("[Step5] isLoadAndUnLoad_MotionDone 超时等待");
                                            throw new TimeoutException("[Step4] isLoadAndUnLoad_MotionDone 超时等待");
                                        }
                                        break;
                                    case EventResult.SUCCEED:
                                        {
                                            //什么都不做  往下do while
                                        }
                                        break;
                                    case EventResult.CANCEL:
                                        {
                                            this.Log_Global("[Step5]用户取消运行");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                }

                                break;
                                //Ben 2024/9/11==============================================================


                            }
                            else
                            {

                                if (this.Status.LeftStationTest == TestStatusOnBoard.一次测试)//当夹具在对应温度下单面测试完成时翻转夹具
                                {
                                    //转料位夹具转向
                                    this.Log_Global($"[Step5]左载台夹具翻转");
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    //Ben 2024/9/11==============================================================
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_测试, SequenceOrder.Normal);
                                    this.Log_Global($"[Step5]TestCompleteLeft，轴执行<RY_测试>点位移动完成");
                                    if (tokenSource.IsCancellationRequested) return;

                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_装卸, SequenceOrder.Normal);
                                    this.Log_Global($"[Step5]TestCompleteLeft，轴执行<LY_装卸>点位移动完成");
                                    if (tokenSource.IsCancellationRequested) return;
                                    //Ben 2024/9/11==============================================================

                                    Parallel.Invoke(() =>
                                    {
                                        this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT3103.LXZ_装卸);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_装卸, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    });


                                    //if (tokenSource.IsCancellationRequested) return;
                                    //WaitMessage(tokenSource);
                                    //CheckIO_FixedJaw_Left_Up(tokenSource);

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Turn_P(tokenSource);      //抓手转到正面

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Loosen(tokenSource);  //抓手放松

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Dn(tokenSource);      //抓手下降
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Tighten(tokenSource);      //抓手收紧
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Vacuum_L(false, tokenSource);   //关闭左真空
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);          //抓手上升
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Turn_N(tokenSource);      //旋转到反面
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Dn(tokenSource);          //抓手下降
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Loosen(tokenSource);      //抓手放松
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Vacuum_L(true, tokenSource);    //开真空
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);          //抓手上升
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    //CheckIO_FixedJaw_Left_Dn(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);

                                    CheckIO_Tongs_Turn_P(tokenSource);      //抓手转到正面
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);

                                    Parallel.Invoke(() =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LZ_待机, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    });

                                    //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_待机, SequenceOrder.Normal);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Log_Global($"[Step5]左载台夹具翻转后，控温");
                                    this.Bridges_WithPauseFunc[Action3103.TemperatureControlLeft].Set();

                                }
                                else
                                {
                                    Parallel.Invoke(() =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LZ_待机, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                   () =>
                                   {
                                       if (tokenSource.IsCancellationRequested) return;
                                       WaitMessage(tokenSource);
                                       this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                                       if (tokenSource.IsCancellationRequested) return;
                                       WaitMessage(tokenSource);
                                   },
                                   () =>
                                   {
                                       if (tokenSource.IsCancellationRequested) return;
                                       WaitMessage(tokenSource);
                                       this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                       if (tokenSource.IsCancellationRequested) return;
                                       WaitMessage(tokenSource);
                                   });
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Bridges_WithPauseFunc[Action3103.TemperatureControlLeft].Set();
                                }

                            }
                            //Ben 2024/9/11==============================================================
                            this.Log_Global("[Step5]释放 TestCompleteLeft_MotionDone 信号");

                            this.Bridges_WithPauseFunc[Action3103.TestCompleteLeft_MotionDone].Set();   //设置了
                            //Ben 2024/9/11==============================================================

                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step5]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                switch (this.Bridges_WithPauseFunc[Action3103.TestCompleteRight].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                if (this.Status.LoadAndUnLoad == Operation.Idle)
                                {
                                    break;
                                }
                            }
 
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);

                            //Parallel.Invoke(() =>
                            //{
                            //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_装卸, SequenceOrder.Normal);
                            //},
                            //() =>
                            //{
                            //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_测试, SequenceOrder.Normal);
                            //});

                            //Ben 2024/9/11==============================================================
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                            this.Log_Global($"[Step5]TestCompleteLeft，轴执行<RX_避让>点位移动完成");
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                            this.Log_Global($"[Step5]TestCompleteLeft，轴执行<LX_避让>点位移动完成");
                            //Ben 2024/9/11==============================================================


                            if (this.TempListRight.Count <= 0)
                            {
                                while (true)
                                {
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    if (this.Status.LoadAndUnLoad == Operation.Idle)
                                    {
                                        break;
                                    }
                                }
                                this.Log_Global($"[Step5]控温结束，右载台切换夹具");
                                this.Status.UnLoadRightStation = true;
                                this.Bridges_WithPauseFunc[Action3103.LoadAndUnLoad].Set();


                                //Ben 2024/9/11==============================================================
                                //线程1负责 下面4点顺序运动
                                //1.LX_避让
                                //2.LY_测试
                                //3.RY_装卸
                                //4.RXZ_装卸
                                //Ben 2024/9/11==============================================================

                                //Ben 2024/9/11==============================================================
                                var isLoadAndUnLoad_MotionDone = this.Bridges_WithPauseFunc[Action3103.LoadAndUnLoad_MotionDone].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.SuperLongTimeout_ms, tokenSource);

                                switch (isLoadAndUnLoad_MotionDone)
                                {
                                    case EventResult.TIMEOUT:
                                        {
                                            this.Log_Global("[Step5] isLoadAndUnLoad_MotionDone 超时等待");
                                            throw new TimeoutException("[Step4] isLoadAndUnLoad_MotionDone 超时等待");
                                        }
                                        break;
                                    case EventResult.SUCCEED:
                                        {
                                            //什么都不做  往下do while
                                        }
                                        break;
                                    case EventResult.CANCEL:
                                        {
                                            this.Log_Global("[Step5]用户取消运行");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                }

                                break;
                                //Ben 2024/9/11==============================================================

                            }
                            else
                            {
                                if (this.Status.RightStationTest == TestStatusOnBoard.一次测试)
                                {
                                    //转料位夹具转向
                                    this.Log_Global($"[Step5]右载台夹具翻转");
                                    this.LocalResource.Axes[AxisNameEnum_CT3103.RY].WaitMotionDone();
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);



                                    //Ben 2024/9/11==============================================================
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_测试, SequenceOrder.Normal);
                                    this.Log_Global($"[Step5]TestCompleteLeft，轴执行<LY_测试>点位移动完成");
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_装卸, SequenceOrder.Normal);
                                    this.Log_Global($"[Step5]TestCompleteLeft，轴执行<RY_装卸>点位移动完成");
                                    if (tokenSource.IsCancellationRequested) return; 
                                    WaitMessage(tokenSource);
                                    //Ben 2024/9/11==============================================================




                                    Parallel.Invoke(() =>
                                    {
                                        this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT3103.RXZ_装卸);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_装卸, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    });

                                    //if (tokenSource.IsCancellationRequested) return;
                                    //WaitMessage(tokenSource);
                                    //CheckIO_FixedJaw_Left_Up(tokenSource);

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Turn_P(tokenSource);      //抓手转到正面

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Loosen(tokenSource);  //抓手放松

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Dn(tokenSource);      //抓手下降
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Tighten(tokenSource);      //抓手收紧
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Vacuum_L(false, tokenSource);   //关闭左真空
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);          //抓手上升
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Turn_N(tokenSource);      //旋转到反面
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Dn(tokenSource);          //抓手下降
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Loosen(tokenSource);      //抓手放松
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Vacuum_L(true, tokenSource);    //开真空
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);          //抓手上升
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    //CheckIO_FixedJaw_Left_Dn(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);

                                    CheckIO_Tongs_Turn_P(tokenSource);      //抓手转到正面
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);

                                    Parallel.Invoke(() =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RZ_待机, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    });

                                    //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_待机, SequenceOrder.Normal);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);

                                    this.Log_Global($"[Step5]右载台夹具翻转后，控温");
                                    this.Bridges_WithPauseFunc[Action3103.TemperatureControlRight].Set();

                                }
                                else
                                {
                                    Parallel.Invoke(() =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RZ_待机, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    },
                                    () =>
                                    {
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                    });
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);

                                    this.Bridges_WithPauseFunc[Action3103.TemperatureControlRight].Set();
                                }
                            }

                            //Ben 2024/9/11==============================================================
                            this.Log_Global("[Step5]释放 TestCompleteRight_MotionDone 信号");

                            this.Bridges_WithPauseFunc[Action3103.TestCompleteRight_MotionDone].Set();   //设置了
                            //Ben 2024/9/11==============================================================




                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step5]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }





            }
        }
    }

}
