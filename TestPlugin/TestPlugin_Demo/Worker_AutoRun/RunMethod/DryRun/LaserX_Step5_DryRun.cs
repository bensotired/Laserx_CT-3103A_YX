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
        public void LaserX_Step5_DryRun(CancellationTokenSource tokenSource)
        {
            this.Bridges_WithPauseFunc[ARECT40410A.Turntable].Set();

            while (true)
            {
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step5Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            //this.Bridges_WithPauseFunc[ARECT40410A.Step5Finish].Set();
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

                switch (this.Bridges_WithPauseFunc[ARECT40410A.CanTest].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            //开始测试
                            //this.Status.isTesting = true;
                            //判断对应测试台是否有产品，可以进行测试
                            //若可以测试，则更改对应测试台状态后开始测试
                            Parallel.Invoke(() =>
                            {
                                this.Log_Global($"LoadRightStation:{this.Status.LoadRightStation_1},{this.Status.LoadRightStation_2}");
                                if (this.Status.LoadRightStation_1 || this.Status.LoadRightStation_2)//对应工站一
                                {
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.Log_Global("工站一");
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_左探针加电气缸电磁阀_上升].TurnOn(false);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_左探针加电气缸电磁阀_下降].TurnOn(true);
                                    this.Log_Global("工站一,左探针下降");
                                    int count = 0;
                                    while (true)
                                    {
                                        count++;
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessage(tokenSource);
                                        if (this.LocalResource.IOs[IONameEnum_CT40410A.Input_左探针加电气缸动作].Interation.IsActive)
                                        {
                                            count = 0;
                                            this.Log_Global("工站一,左探针下降到位");
                                            break;
                                        }
                                        if (count > 100)
                                        {
                                            this.Log_Global("工站一,左探针下降超时!");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                    }
                                    Thread.Sleep(30000);//RunStation1(tokenSource);

                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_左探针加电气缸电磁阀_下降].TurnOn(false);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_左探针加电气缸电磁阀_上升].TurnOn(true);
                                    this.Log_Global("工站一,左探针上升");
                                    while (true)
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessage(tokenSource);
                                        if (this.LocalResource.IOs[IONameEnum_CT40410A.Input_左探针加电气缸复位].Interation.IsActive)
                                        {
                                            this.Log_Global("工站一,左探针上升到位");
                                            break;
                                        }
                                        if (count > 100)
                                        {
                                            this.Log_Global("工站一,左探针上升超时!");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                    }
                                    //this.Status.LoadRightStation = false;
                                    var test1 = this.LocalResource.Axes[AxisNameEnum_CT40410A.测试台旋转].Get_CurUnitPos();
                                    if (test1 < TesterPosition)
                                    {
                                        //this.Status.UnLoadLeftStation_2 = true;
                                        this.Status.UnLoadRightStation_2 = true;
                                        this.Status.LoadRightStation_2 = false;
                                    }
                                    else
                                    {
                                        //this.Status.UnLoadLeftStation_1 = true;
                                        this.Status.UnLoadRightStation_1 = true;
                                        this.Status.LoadRightStation_1 = false;
                                    }
                                }
                            },
                            () =>
                            {
                                this.Log_Global($"LoadLeftStation:{this.Status.LoadLeftStation_1},{this.Status.LoadLeftStation_2}");
                                if (this.Status.LoadLeftStation_1 || this.Status.LoadLeftStation_2)//对应工站二
                                {
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.Log_Global("工站二");
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_右探针加电气缸电磁阀_上升].TurnOn(false);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_右探针加电气缸电磁阀_下降].TurnOn(true);
                                    this.Log_Global("工站二,右探针下降");
                                    int count = 0;
                                    while (true)
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessage(tokenSource);
                                        if (this.LocalResource.IOs[IONameEnum_CT40410A.Input_右探针加电气缸动作].Interation.IsActive)
                                        {
                                            count = 0;
                                            this.Log_Global("工站二,右探针下降到位");
                                            break;
                                        }
                                        if (count > 100)
                                        {
                                            this.Log_Global("工站一,右探针下降超时!");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                    }
                                    Thread.Sleep(30000);//RunStation2(tokenSource);

                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_右探针加电气缸电磁阀_下降].TurnOn(false);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_右探针加电气缸电磁阀_上升].TurnOn(true);
                                    this.Log_Global("工站二,右探针上升");
                                    while (true)
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessage(tokenSource);
                                        if (this.LocalResource.IOs[IONameEnum_CT40410A.Input_右探针加电气缸复位].Interation.IsActive)
                                        {
                                            count = 0;
                                            this.Log_Global("工站二,右探针上升到位");
                                            break;
                                        }
                                        if (count > 100)
                                        {
                                            this.Log_Global("工站一,右探针上升超时!");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                    }
                                    //this.Status.LoadLeftStation = false;
                                    var test1 = this.LocalResource.Axes[AxisNameEnum_CT40410A.测试台旋转].Get_CurUnitPos();
                                    if (test1 < TesterPosition)
                                    {
                                        //this.Status.UnLoadRightStation_2 = true;
                                        this.Status.UnLoadLeftStation_2 = true;
                                        this.Status.LoadLeftStation_2 = false;
                                    }
                                    else
                                    {
                                        //this.Status.UnLoadRightStation_1 = true;
                                        this.Status.UnLoadLeftStation_1 = true;
                                        this.Status.LoadLeftStation_1 = false;
                                    }
                                }
                            });
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessage(tokenSource);

                            this.Bridges_WithPauseFunc[ARECT40410A.Turntable].Set();
                            this.Bridges_WithPauseFunc[ARECT40410A.AllowInterchange].Set();
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessage(tokenSource);
                            //this.Status.isTesting = false;


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
