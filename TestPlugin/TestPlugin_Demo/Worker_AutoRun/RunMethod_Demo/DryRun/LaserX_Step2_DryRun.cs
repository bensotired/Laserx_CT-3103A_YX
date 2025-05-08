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
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            dutLoad = 0;
            this.Log_Global($"[Step2]Load  dutLoad:{dutLoad}");
            Thread.Sleep(100);
            do
            {
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            this.Bridges_WithPauseFunc[ARECT40410A.Step2Finish].Set();
                            this.Log_Global("[Step2]Step2Finish");
                            return;
                        }
                        break;
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step2]用户取消运行");
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
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanTakeDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //右吸嘴至取料位取料
                                        Nozzle_Right_TakeDutUp(tokenSource);
                                        SearchProduct search = searchProducts[0];
                                        searchProducts.RemoveAt(0);
                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_TakeDutFinish].Set();//这是让线程1重新匹配物料
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //IO检测
                                        //bool isCotinue = false;
                                        //this.Check_Right_Nozzle_IsExist(tokenSource, out isCotinue);
                                        //if (isCotinue)
                                        //{
                                        //    this.Log_Global("[Step2]IO检测不合格，重新抓取");
                                        //    this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].Set();
                                        //    continue;
                                        //}
                                        //this.Log_Global("[Step2]IO检测合格，继续运行");
                                        dutLoad++;
                                        this.Log_Global($"[Step2]dutLoad:{dutLoad}");
                                        TestProduct product = new TestProduct();
                                        product.LoadIndex = dutLoad;
                                        product.LoadposX = search.PosX;
                                        product.LoadposY = search.PosY;
                                        On_N_R_Tray.Add(product);
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //至相机2开始匹配
                                        bool isNeedReLoadTec = false;
                                        //this.Nozzle_Right_Camera2_Match(tokenSource, out isNeedReLoadTec);

                                        if (isNeedReLoadTec)
                                        {
                                            if (tokenSource.IsCancellationRequested)
                                            {
                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                return;
                                            }
                                            On_N_R_Tray.RemoveAt(On_N_R_Tray.Count - 1);
                                            //应该需要关闭右吸嘴真空 
                                            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle].TurnOn(false);
                                            this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].Set();
                                            break;
                                        }

                                        while (true)
                                        {
                                            if (tokenSource.IsCancellationRequested)
                                            {
                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                return;
                                            }
                                            bool s = false;
                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanUnloadDutToDevice].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                            {
                                                case EventResult.SUCCEED:
                                                    {
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        //if (TrunOn)
                                                        //{
                                                        //左吸嘴测试激光功率
                                                        this.Step2Wavelen = "878";
                                                        //this.Nozzle_Left(tokenSource);
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        //}
                                                        //产品放置测试台,可增加相机三动作
                                                        this.Nozzle_Right_ToDevice_ToUnload_DryRun(tokenSource);
                                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_UnloadDutToDeviceFinish].Set();
                                                        //右吸嘴应该回到准备位
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        this.Nozzle_Right_RunToIdlePosition(tokenSource);
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }

                                                        while (true)
                                                        {
                                                            if (tokenSource.IsCancellationRequested)
                                                            {
                                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                                return;
                                                            }
                                                            bool s2 = false;
                                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Step3_Information_Step2_Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                            {
                                                                case EventResult.SUCCEED:
                                                                    {
                                                                        this.Log_Global("Step2已经收到step3的信息 证明已经进了step3");
                                                                        s2 = true;
                                                                    }
                                                                    break;
                                                                case EventResult.CANCEL:
                                                                    {
                                                                        this.Log_Global("[Step2]用户取消运行");
                                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                                    }
                                                                    break;
                                                                case EventResult.TIMEOUT:
                                                                    break;
                                                            }
                                                            if (s2)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                        s = true;
                                                    }
                                                    break;
                                                case EventResult.CANCEL:
                                                    {
                                                        this.Log_Global("[Step2]用户取消运行");
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                    }
                                                    break;
                                                case EventResult.TIMEOUT:
                                                    break;
                                            }
                                            if (s)
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    }

                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step2]用户取消运行");
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                    }
                                    break;
                                case EventResult.TIMEOUT:
                                    break;
                                default:
                                    break;
                            }
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.NozzeRightNeedTakeTECbackToRightBlue].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        int sindex = On_N_R_Tray.Count;
                                        if (sindex == 2)//倒二
                                        {
                                            this.Nozzle_Right_MoveToIdelPlace_DESC_Second(tokenSource);
                                            this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_NeedRuntoDescSecondPlace].Set();

                                            while (true)
                                            {
                                                switch (this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_FinishRuntoDescSecondPlace].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                {
                                                    case EventResult.SUCCEED:
                                                        {
                                                            this.Nozzle_Right_Releas_MoveToIdelPlace(tokenSource);
                                                            On_N_R_Tray.RemoveAt(0);
                                                            sindex--;
                                                        }
                                                        break;
                                                    case EventResult.CANCEL:
                                                        {
                                                            this.Log_Global("[Step2]用户取消运行");
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                        }
                                                        break;
                                                    case EventResult.TIMEOUT:
                                                        break;
                                                }
                                            }

                                        }
                                        if (sindex == 1)
                                        {
                                            //将测试台产品放回料盒
                                            this.Nozzle_Right_Add_TecFromTray_MoveToIdelPlace_DESC_First(tokenSource);
                                            this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_NeedRuntoDescFirstPlace].Set();

                                            while (true)
                                            {
                                                switch (this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_FinishRuntoDescFirstPlace].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                {
                                                    case EventResult.SUCCEED:
                                                        {
                                                            this.Nozzle_Right_Releas_MoveToIdelPlace(tokenSource);
                                                            this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_CanFinish].Set();
                                                        }
                                                        break;
                                                    case EventResult.CANCEL:
                                                        {
                                                            this.Log_Global("[Step2]用户取消运行");
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                        }
                                                        break;
                                                    case EventResult.TIMEOUT:
                                                        break;
                                                    default:
                                                        break;
                                                }

                                            }

                                        }
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step2]用户取消运行");
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
