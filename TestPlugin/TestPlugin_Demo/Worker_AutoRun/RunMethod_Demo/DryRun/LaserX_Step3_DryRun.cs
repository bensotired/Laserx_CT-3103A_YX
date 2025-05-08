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

            //中转
            this.Log_Global("[Step3]Load");
            this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].Set();

            do
            {
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step2Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Bridges_WithPauseFunc[ARECT40410A.Step3Finish].Set();
                            this.Log_Global("[Step3]Step3Finish");
                            return;
                        }
                        break;
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step3]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                        }
                        break;
                    case EventResult.TIMEOUT:
                        {
                            //Nozzle_Left_PositionToCamera4

                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_UnloadDutToDeviceFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        this.Bridges_WithPauseFunc[ARECT40410A.Step3_Information_Step2_Finish].Set();
                                        while (true)
                                        {
                                            bool sk = false;
                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                            {
                                                case EventResult.SUCCEED:
                                                    {
                                                        if (!userContiUnloadTec)
                                                        {
                                                            break;
                                                        }
                                                        this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].Set();

                                                        TestProduct testProduct = On_N_R_Tray[0];
                                                        this.TransTec(ref On_N_R_Tray, ref _step4, tokenSource);

                                                        //测试需要左吸嘴到达才能进行测试，移至Step5的ARECT40410A.Nozzle_Left_CanRunToPickTecIdlePosition
                                                        this.Bridges_WithPauseFunc[ARECT40410A.CanTest].Set();

                                                        //检测右吸嘴在安全位置后，左吸嘴才能过来
                                                        //bool safeResult = this.Right_Nozzle_IsInSafePosition(tokenSource);

                                                        //if (safeResult)
                                                        //{
                                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanRunToPickTecIdlePosition].Set();
                                                        //}

                                                        while (true)
                                                        {
                                                            bool s2 = false;
                                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.FinishTest].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                            {
                                                                case EventResult.SUCCEED:
                                                                    {
                                                                        this.Log_Global("[Step3]测试完成");

                                                                        //testProduct = _step4[0];
                                                                        //_step4.RemoveAt(0);

                                                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanTakeDut].Set();

                                                                        while (true)
                                                                        {
                                                                            bool s = false;
                                                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_TakingDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                                            {
                                                                                case EventResult.SUCCEED:
                                                                                    {
                                                                                        this.Log_Global("[Step3]Step5已经收到Step3的信号 Nozzle_Left正在准备运行取料");
                                                                                        s = true;
                                                                                    }
                                                                                    break;
                                                                                case EventResult.CANCEL:
                                                                                    {
                                                                                        this.Log_Global("[Step3]用户取消运行");
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
                                                                        s2 = true;
                                                                    }
                                                                    break;
                                                                case EventResult.CANCEL:
                                                                    {
                                                                        this.Log_Global("[Step3]用户取消运行");
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
                                                        sk = true;
                                                    }
                                                    break;
                                                case EventResult.CANCEL:
                                                    {
                                                        this.Log_Global("[Step3]用户取消运行");
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                    }
                                                    break;
                                                case EventResult.TIMEOUT:
                                                    break;
                                            }
                                            if (sk)
                                            {
                                                break;
                                            }
                                            bool s3 = false;
                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.UserCancelBlueAddTec_TellStep3].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                            {
                                                case EventResult.SUCCEED:
                                                    {
                                                        this.Bridges_WithPauseFunc[ARECT40410A.NozzeRightNeedTakeTECbackToRightBlue].Set();
                                                        s3 = true;
                                                    }
                                                    break;
                                                case EventResult.CANCEL:
                                                    {
                                                        this.Log_Global("[Step3]用户取消运行");
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                    }
                                                    break;
                                                case EventResult.TIMEOUT:
                                                    break;
                                            }
                                            if (s3)
                                            {
                                                break;
                                            }


                                        }
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step3]用户取消运行");
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
