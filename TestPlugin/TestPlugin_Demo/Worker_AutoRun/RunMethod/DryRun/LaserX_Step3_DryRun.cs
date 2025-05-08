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
    public sealed partial class TestPluginWorker_CT40410A
    {
        public void LaserX_Step3_DryRun(CancellationTokenSource tokenSource)
        {
            bool Forward = false;

            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.测试台_正向_θ, SequenceOrder.Normal);
            this.Bridges_WithPauseFunc[ARECT40410A.GrabEnable].Set();
            while (true)
            {

                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step3Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {

                            this.Log_Global("[Step3]Step3Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step3]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }

                //接受旋转信号
                //判断是否在进行测试，若测试未结束则等待

                switch (this.Bridges_WithPauseFunc[ARECT40410A.Turntable].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                if (!Status.isTesting)//不在测试跳出
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            if (!this.Status.UnLoadRightStation_1&&!this.Status.UnLoadRightStation_2&&!this.Status.UnLoadLeftStation_1&&!this.Status.UnLoadLeftStation_2&&this.StopSearch==true)
                            {
                                break;
                            }
                            //判断载台，决定如何旋转
                            if (Forward)
                            {
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.Log_Global("载台正向");

                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.测试台_正向_θ, SequenceOrder.Normal);
                                this.LocalResource.Axes[AxisNameEnum_CT40410A.测试台旋转].WaitMotionDone();
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                Forward = false;

                                this.Bridges_WithPauseFunc[ARECT40410A.CanTest].Set();
                            }
                            else
                            {
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.Log_Global("载台反向");

                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.测试台_反向_θ, SequenceOrder.Normal);
                                this.LocalResource.Axes[AxisNameEnum_CT40410A.测试台旋转].WaitMotionDone();
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                Forward = true;

                                this.Bridges_WithPauseFunc[ARECT40410A.CanTest].Set();
                            }
                            this.Status.isTesting = true;
                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step3]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }


                switch (this.Bridges_WithPauseFunc[ARECT40410A.EndRun].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            GantryAxisHome();
                            this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].Set();
                            this.Bridges_WithPauseFunc[ARECT40410A.Step2Finish].Set();
                            this.Bridges_WithPauseFunc[ARECT40410A.Step3Finish].Set();
                            this.Bridges_WithPauseFunc[ARECT40410A.Step4Finish].Set();
                            this.Bridges_WithPauseFunc[ARECT40410A.Step5Finish].Set();

                            Cancell();
                            this.Log_Global("[Step3]结束信号");
                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step3]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }




            }







        }
    }
   
}
