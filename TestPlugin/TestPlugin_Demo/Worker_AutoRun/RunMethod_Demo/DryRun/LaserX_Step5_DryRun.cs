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
            this.Log_Global("[Step5]Load");

            //吸嘴移动到扫码的位置
            //this.Nozzle_Left_Run_To_ScannerPosition(tokenSource);
            //this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_Run_To_ScannerPlace_Done].Set();

            //do
            //{
            //    bool skip = false;
            //    switch (this.Bridges_WithPauseFunc[ARECT40410A.ScannerFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
            //    {
            //        case EventResult.SUCCEED:
            //            {
            //                skip = true;
            //            }
            //            break;
            //        case EventResult.CANCEL:
            //            {
            //                this.Log_Global("[Step5]用户取消运行");
            //                tokenSource.Token.ThrowIfCancellationRequested();
            //            }
            //            break;
            //        case EventResult.TIMEOUT:
            //            break;
            //    }
            //    if (skip)
            //    {
            //        break;
            //    }

            //} while (true);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanUnloadDutToDevice].Set();
            do
            {
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanRunToPickTecIdlePosition].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            //左吸嘴到等待位或测试位
                            //this.Nozzle_Left_Run_To_LoadDut_PreparePosition(tokenSource);
                            //测试需要左吸嘴到达才能进行测试
                            //this.Bridges_WithPauseFunc[ARECT40410A.CanTest].Set();
                        }
                        break;
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step5]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                        }
                        break;
                    case EventResult.TIMEOUT:
                        break;
                }
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step4Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Bridges_WithPauseFunc[ARECT40410A.Step5Finish].Set();
                            this.Log_Global("[Step5]Step5Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step5]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                        }
                        break;
                    case EventResult.TIMEOUT:
                        {
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanTakeDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_TakingDut].Set();
                                        this.Nozzle_Left_Run_To_Tray_Catch(tokenSource);
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        bool isFail = false;//不知道是否会用到，先放着
                                                            //this.Check_Nozzle_IsExist(tokenSource, out isFail);

                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanUnloadDutToDevice].Set();

                                        //Parallel.Invoke(
                                        //   () =>
                                        //   {
                                        //       //this.Nozzle_Left_Run_To_Safe(tokenSource);//左侧吸嘴运行到不影响右吸嘴放料到测试台的位置
                                        //   },
                                        //   () =>
                                        //   {
                                        //       //Thread.Sleep(50);
                                        //       this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanUnloadDutToDevice].Set();
                                        //   });

                                        //dutPass_UnloadCount++;

                                        //if (this.parameterInformation.PositinIndex[CurrentBox] +1 < this.parameterInformation.ProductNumber[CurrentBox])//isGrab)
                                        //{
                                        //    this.Log_Global("go_test");
                                        this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].Set();
                                        //}
                                        //dutPass_UnloadCount--;



                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_RunToUnloadPlace].Set();

                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }


                                        //左吸嘴运行到下料X
                                        //this.Nozzle_Left_Run_To_UnloadPlace(tokenSource);

                                        switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanUnloadDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                        {
                                            case EventResult.SUCCEED:
                                                {
                                                    //左吸嘴下料位下料
                                                    //this.Nozzle_Left_UnloadPlace_Down_Unload_UP(tokenSource);
                                                    if (tokenSource.IsCancellationRequested)
                                                    {
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
                                                    }
                                                    this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_UnloadDut].Set();

                                                    do
                                                    {
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        bool exit = false;
                                                        switch (this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep5].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                        {
                                                            case EventResult.SUCCEED:
                                                                {
                                                                    exit = true;//增加物料，直接跳出
                                                                }
                                                                break;
                                                            case EventResult.CANCEL:
                                                                {
                                                                    this.Log_Global("[Step5]用户取消运行");
                                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                                }
                                                                break;
                                                            case EventResult.TIMEOUT:
                                                                break;
                                                        }
                                                        if (exit)
                                                        {
                                                            break;
                                                        }
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }

                                                        //switch (this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Left_NeedRuntoScannerPlace].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                        //{
                                                        //    case EventResult.SUCCEED:
                                                        //        {
                                                        //            //左吸嘴到扫码位置
                                                        //            //this.Nozzle_Left_Run_To_ScannerPosition(tokenSource);
                                                        //            this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_Run_To_ScannerPlace_Done].Set();
                                                        //            do
                                                        //            {
                                                        //                bool skip = false;
                                                        //                switch (this.Bridges_WithPauseFunc[ARECT40410A.ScannerFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                        //                {
                                                        //                    case EventResult.SUCCEED:
                                                        //                        {
                                                        //                            skip = true;
                                                        //                        }
                                                        //                        break;
                                                        //                }
                                                        //                if (skip)
                                                        //                {
                                                        //                    break;
                                                        //                }
                                                        //            } while (true);
                                                        //        }
                                                        //        break;
                                                        //    case EventResult.CANCEL:
                                                        //        {
                                                        //            this.Log_Global("[Step5]用户取消运行");
                                                        //            tokenSource.Token.ThrowIfCancellationRequested();
                                                        //        }
                                                        //        break;
                                                        //}



                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }

                                                        switch (this.Bridges_WithPauseFunc[ARECT40410A.UserCancelBlueAddTec_TellStep5].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                        {
                                                            case EventResult.SUCCEED:
                                                                {
                                                                    exit = true;//取消了，也直接跳出
                                                                }
                                                                break;
                                                            case EventResult.CANCEL:
                                                                {
                                                                    this.Log_Global("[Step5]用户取消运行");
                                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                                }
                                                                break;
                                                            case EventResult.TIMEOUT:
                                                                break;
                                                        }
                                                        if (exit)
                                                        {
                                                            break;
                                                        }
                                                    } while (true);

                                                }
                                                break;
                                            case EventResult.CANCEL:
                                                {
                                                    this.Log_Global("[Step5]用户取消运行");
                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                }
                                                break;
                                            case EventResult.TIMEOUT:
                                                break;
                                        }




                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step5]用户取消运行");
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
