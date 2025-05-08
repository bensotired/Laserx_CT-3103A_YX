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
        public void LaserX_Step2_TemperatureControl_To_Carrier_Left(CancellationTokenSource tokenSource)
        {
            while (true)
            {
                //接受步骤一的信号
                switch (this.Bridges_WithPauseFunc[Action3103.Step2Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {

                            this.Log_Global("[Step2]Step2Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step2]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);

                switch (this.Bridges_WithPauseFunc[Action3103.TemperatureControlLeft].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            #region 空跑时用来验证的
                            //this.Status.TempControlStateLeft = Operation.Idle;
                            //var Temperature = this.TempListLeft[0];
                            //this.Log_Global($"[Step2]左载台开始控温：[{Temperature}]℃");
                            //Thread.Sleep(3000);
                            //this.Status.TempControlStateLeft = Operation.Done;
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
                            //this.Log_Global($"[Step2]左载台执行测试");
                            //this.Bridges_WithPauseFunc[Action3103.AllowTestsLeft].Set();
                            //break;


                            //if (this.TempListLeft.Count <= 0)
                            //{
                            //    this.Log_Global("[Step2]控温列表为空!!!");
                            //    this.Log_Global("[Step2]Step2Finish");
                            //    //结束信号
                            //    return;
                            //}
                            #endregion
                            this.Status.TempControlStateLeft = Operation.Idle;
                            var TargetTemperature = this.TempListLeft[0];//获取当前控制温度
                            this.Log_Global($"[Step2]左载台开始控温：[{TargetTemperature}]℃");
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            this.LocalResource.IOs[IONameEnum_CT3103.TEC_Left].TurnOn(false);//将TEC与机台的控制器连接
                            this.TEC_Controller_1(TargetTemperature, this.parameter.TemperatureTolerance);//设置控制温度和温度容差
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                Thread.Sleep(100);
                                var ispass = this.TEC_isStable_1();//获取温控是否完成
                                if (ispass)
                                {
                                    this.Status.TempControlStateLeft = Operation.Done;
                                    this.Log_Global($"[Step2]左载台控温完成，用时：{sw.ElapsedMilliseconds / 1000.0}S");
                                    sw.Stop();
                                    break;
                                }
                            }
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                Thread.Sleep(100);
                                if (this.Status.IsTesting == Operation.Idle)
                                {
                                    break;//测试未被另一个载台占用时跳出
                                }
                            }
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            this.Log_Global($"[Step2]左载台执行测试");
                            this.Bridges_WithPauseFunc[Action3103.AllowTestsLeft].Set();
                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step2]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }

            }

        }

    }

}
