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
        public void LaserX_Step3_DeviceLoad(CancellationTokenSource tokenSource)
        {
            //中转
            this.Log_Global("[Step3]Load");
            this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].Set();

            do
            {
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
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
                            return;
                        }
                        break;
                    case EventResult.TIMEOUT:
                        {
                            //Nozzle_Left_PositionToCamera4
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_UnloadDutToDeviceFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        this.Bridges_WithPauseFunc[ARECT40410A.Step3_Information_Step2_Finish].Set();

                                        while (true)
                                        {
                                            if (tokenSource.IsCancellationRequested)
                                            {
                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                return;
                                            }
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
                                                            if (tokenSource.IsCancellationRequested)
                                                            {
                                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                                return;
                                                            }
                                                            bool s2 = false;
                                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.FinishTest].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                            {
                                                                case EventResult.SUCCEED:
                                                                    {
                                                                        this.Log_Global("[Step3]测试完成");

                                                                        testProduct = _step4[0];
                                                                        _step4.RemoveAt(0);

                                                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanTakeDut].Set();

                                                                        while (true)
                                                                        {
                                                                            if (tokenSource.IsCancellationRequested)
                                                                            {
                                                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                                                return;
                                                                            }
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
                                                                                        return;
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
                                                                        return;
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
                                                        return;
                                                    }
                                                    break;
                                                case EventResult.TIMEOUT:
                                                    break;
                                            }
                                            if (sk)
                                            {
                                                break;
                                            }
                                            if (tokenSource.IsCancellationRequested)
                                            {
                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                return;
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
                                                        return;
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
                                        return;
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
        /// <summary>
        /// 检测右吸嘴是否在安全位置
        /// </summary>
        /// <param name="tokenSource"></param>
        public bool Right_Nozzle_IsInSafePosition(CancellationTokenSource tokenSource)
        {
            int startIndex = 0;
            //获取右吸嘴的安全位置
            double cameraPosition = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X].ItemCollection[0].Position;

            do
            {
                Thread.Sleep(500);
                double currentPos = this.LocalResource.Axes[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X].Get_CurUnitPos();
                if (currentPos <= cameraPosition)
                {
                    this.Log_Global($"[step3]右侧吸嘴位置:{currentPos}，下相机位置:{cameraPosition}，success！");
                    return true;
                }
                if (startIndex >= 10)
                {
                    this.UserRequest_Pause(MT.吸嘴1模组);
                    Form_Nozzle_Right_IsInSafe _Nozzle_Right_IsInSafe = new Form_Nozzle_Right_IsInSafe();
                    _Nozzle_Right_IsInSafe.ShowDialog();
                    this.UserRequest_Resume(MT.吸嘴1模组);
                    if (_Nozzle_Right_IsInSafe.DialogResult == DialogResult.OK)//继续读取
                    {
                        this.Log_Global($"[step3]右侧吸嘴位置:{currentPos}，下相机位置:{cameraPosition}，用户继续读取坐标，读取次数为{startIndex}");
                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);
                        continue;
                    }
                    else if (_Nozzle_Right_IsInSafe.DialogResult == DialogResult.None)//忽略警告
                    {
                        this.Log_Global($"[step3]右侧吸嘴位置:{currentPos}，下相机位置:{cameraPosition}，用户选择忽略警告");
                        return true;
                    }
                    else if (_Nozzle_Right_IsInSafe.DialogResult == DialogResult.Cancel)//取消
                    {
                        this.Log_Global($"[step3]右侧吸嘴位置:{currentPos}，下相机位置:{cameraPosition}，10秒钟右侧吸嘴未在下相机右侧，用户取消操作");
                        tokenSource.Cancel();
                        return true;
                    }

                }
                startIndex++;

            } while (true);

        }

        /// <summary>
        /// //拿出一个 放到下一步,DeleteTecs:拿出的数组,AddTecs
        /// </summary>
        /// <param name="DeleteTecs"></param>
        /// <param name="AddTecs"></param>
        /// <param name="tokenSource"></param>
        public void TransTec(ref List<TestProduct> DeleteTecs, ref List<TestProduct> AddTecs, CancellationTokenSource tokenSource)
        {
            try
            {
                TestProduct tec = DeleteTecs[0];
                DeleteTecs.RemoveAt(0);
                AddTecs.Add(tec);
            }
            catch (Exception ex)
            {
                tokenSource.Cancel();
                throw ex;
            }
        }
    }

}
