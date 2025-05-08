using SolveWare_BurnInCommon;
using SolveWare_BinSorter;
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
using LX_BurnInSolution.Utilities;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT40410A
    {
        public void LaserX_Step4_DryRun(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step4]Load");
            do
            {

                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step3Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Bridges_WithPauseFunc[ARECT40410A.Step4Finish].Set();
                            this.Log_Global("[Step4]Step4Finish");
                            return;
                        }
                        break;
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step4]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                        }
                        break;
                    case EventResult.TIMEOUT:
                        {
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.CanTest].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //测试及所需动作
                                        this.Log_Global("[Step4]测试及所需动作");
                                        this.Nozzle_Left_Run_To_LoadDut_PreparePosition(tokenSource);
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //this.Pro_Light(tokenSource);
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //this.RunMain(tokenSource);
                                        Thread.Sleep(2000);
                                        this.Bridges_WithPauseFunc[ARECT40410A.FinishTest].Set();
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step4]用户取消运行");
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                    }
                                    break;
                                case EventResult.TIMEOUT:
                                    break;
                            }
                        }
                        break;
                }

            } while (true);

        }
    }
   
}
