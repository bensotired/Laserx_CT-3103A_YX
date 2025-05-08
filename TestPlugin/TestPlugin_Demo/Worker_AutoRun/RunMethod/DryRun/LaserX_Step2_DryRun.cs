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
        public void LaserX_Step2_DryRun(CancellationTokenSource tokenSource)
        {
            int count = 1;
            while (true)
            {
                //接受步骤一的信号
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step2Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
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


                switch (this.Bridges_WithPauseFunc[ARECT40410A.LoadGradFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            if (false)
                            {
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                //下相机识别
                                Camera2_Match(tokenSource);
                            }
                            if (true)
                            {
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                //触水动作
                                WaterContact(tokenSource);
                            }
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessage(tokenSource);

                            int Judgment = 0;
                            if (!this.parameterInformation.LeftNozzle || !this.parameterInformation.MiddleNozzle)
                            {
                                Judgment = Single;
                            }
                            else
                            {
                                Judgment = Double;
                            }
                            //if (this.Status.StageProducts == 0 && this.StopSearch == true)
                            //{
                            //    this.Log_Global("所有产品均已取走");
                            //    break;
                            //}
                            if (this.Status.StageProducts <= Judgment && this.StopSearch == false)
                            {
                                //载台无需要取走的，直接上料
                                PlaceAction(tokenSource);
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.Status.isTesting = false;
                                //this.Bridges_WithPauseFunc[ARECT40410A.Turntable].Set();
                                this.Bridges_WithPauseFunc[ARECT40410A.GrabEnable].Set();
                            }
                            else
                            {
                                bool ispass = false;
                                while (true)
                                {
                                    switch (this.Bridges_WithPauseFunc[ARECT40410A.AllowInterchange].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                    {
                                        case EventResult.SUCCEED:
                                            {
                                                if (!running)
                                                {
                                                    //载台置换产品
                                                    Interchange(tokenSource);
                                                    if (tokenSource.IsCancellationRequested)
                                                    {
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
                                                    }
                                                    WaitMessage(tokenSource);
                                                    this.Status.isTesting = false;
                                                    //this.Bridges_WithPauseFunc[ARECT40410A.Turntable].Set();
                                                    this.Bridges_WithPauseFunc[ARECT40410A.InterchangeAndUnload].Set();
                                                    ispass = true;
                                                }
                                            }
                                            break;
                                    }
                                    if (ispass)
                                    {
                                        break;
                                    }
                                }
                                
                            }

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
