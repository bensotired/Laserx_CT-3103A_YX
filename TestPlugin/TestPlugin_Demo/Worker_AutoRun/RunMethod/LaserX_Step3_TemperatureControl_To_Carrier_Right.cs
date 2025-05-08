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
using System.Diagnostics;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103
    {
        public void LaserX_Step3_TemperatureControl_To_Carrier_Right(CancellationTokenSource tokenSource)
        {
            while (true)
            {

                switch (this.Bridges_WithPauseFunc[Action3103.Step3Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
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
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                switch (this.Bridges_WithPauseFunc[Action3103.TemperatureControlRight].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            #region 空跑时用来验证的
                            //this.Status.TempControlStateRight = Operation.Idle;
                            //var Temperature = this.TempListRight[0];
                            //this.Log_Global($"[Step3]右载台开始控温：[{Temperature}]℃");
                            //Thread.Sleep(3000);
                            //this.Status.TempControlStateRight = Operation.Done;
                            //if (tokenSource.IsCancellationRequested) return;
                            //WaitMessage(tokenSource);
                            //while (true)
                            //{
                            //    if (tokenSource.IsCancellationRequested) return;
                            //    WaitMessage(tokenSource);
                            //    Thread.Sleep(100);
                            //    if (this.Status.IsTesting == Operation.Idle)
                            //    {
                            //        break;
                            //    }
                            //}
                            //if (tokenSource.IsCancellationRequested) return;
                            //WaitMessage(tokenSource);
                            //this.Log_Global($"[Step3]右载台执行测试");
                            //this.Bridges_WithPauseFunc[Action3103.AllowTestsRight].Set();
                            //break;
                            #endregion
                            if (this.TempListRight.Count <= 0)
                            {
                                this.Log_Global("[Step3]控温列表为空!!!");
                                this.Log_Global("[Step3]Step3Finish");
                                //结束信号
                                return;
                            }
                            this.Status.TempControlStateRight = Operation.Idle;
                            var TargetTemperature = this.TempListRight[0];//获取当前控制温度
                            this.Log_Global($"[Step3]右载台开始控温：[{TargetTemperature}]℃");
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            this.LocalResource.IOs[IONameEnum_CT3103.TEC_Right].TurnOn(false);//将TEC与机台的控制器连接
                            this.TEC_Controller_2(TargetTemperature, this.parameter.TemperatureTolerance);//设置控制温度和温度容差
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                Thread.Sleep(100);
                                var ispass = this.TEC_isStable_2();//获取温控是否完成
                                if (ispass)
                                {
                                    this.Status.TempControlStateRight = Operation.Done;
                                    this.Log_Global($"[Step3]右载台控温完成，用时：{sw.ElapsedMilliseconds / 1000.0}S");
                                    sw.Stop();
                                    break;
                                }
                            }
                            if (this.Status.LeftStation)//当两个载台同时使用时，左载台要比右载台早进入测试
                            {
                                while (true)
                                {
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    Thread.Sleep(100);
                                    if (this.Status.TempControlStateLeft == Operation.Done)
                                    {
                                        break;//当左载台控温完成进入测试后跳出
                                    }
                                }
                            }
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                Thread.Sleep(100);
                                if (this.Status.IsTesting == Operation.Idle)
                                {
                                    break;
                                }
                            }
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            this.Log_Global($"[Step3]右载台执行测试");
                            this.Bridges_WithPauseFunc[Action3103.AllowTestsRight].Set();
                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step3]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }

                switch (this.Bridges_WithPauseFunc[Action3103.EndRun].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Bridges_WithPauseFunc[Action3103.Step1Finish].Set();
                            this.Bridges_WithPauseFunc[Action3103.Step2Finish].Set();
                            this.Bridges_WithPauseFunc[Action3103.Step3Finish].Set();
                            this.Bridges_WithPauseFunc[Action3103.Step4Finish].Set();
                            this.Bridges_WithPauseFunc[Action3103.Step5Finish].Set();
                            this.Log_Global("结束信号");
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
