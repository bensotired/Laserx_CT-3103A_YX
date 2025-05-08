using System.Threading;
using SolveWare_TestPlugin;
using SolveWare_Motion;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using SolveWare_Vision;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT40410A
    {

        void Run线程3方法(CancellationTokenSource tokenSource)
        {
            this.Log_Global($"Run线程3方法 1");
            this.Log_Global($"Run线程3方法 2");
            this.Log_Global($"Run线程3方法 3");
            this.Bridges_WithPauseFunc[ARECT40410A.演示用_3].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.MiddumTimeout_ms, tokenSource);
            this.Log_Global($"Run线程3方法 4");
            this.Log_Global($"Run线程3方法 5");
            this.Log_Global($"Run线程3方法 6");
            this.Log_Global($"Run线程3方法 7");
            this.Log_Global($"Run线程3方法 8");
            for (int i = 0; i < 3; i++)
            {

                switch (this.Bridges_WithPauseFunc[ARECT40410A.演示用_2].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.MiddumTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Log_Global($"Run线程3方法 收到了[{ARECT40410A.演示用_2}]信号  并释放[{ARECT40410A.演示用_3}]信号");
                            this.Bridges_WithPauseFunc[ARECT40410A.演示用_3].Set();
                        }
                        break;
                    case EventResult.TIMEOUT:
                        {
                            for (int a = 0; a < 3; a++)
                            {
                                this.Log_Global($"Run线程3方法 超时 {a}");
                            }
                        }
                        break;
                    case EventResult.CANCEL:
                        {
                            this.Log_Global($"Run线程3方法 收到了取消信号");
                            tokenSource.Token.ThrowIfCancellationRequested();
                        }
                        break;
                }
                Thread.Sleep(2000);
            }
        }
    }
}