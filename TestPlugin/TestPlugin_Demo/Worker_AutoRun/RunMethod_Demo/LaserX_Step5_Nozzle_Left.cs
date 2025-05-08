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
        public void LaserX_Step5_Nozzle_Left(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step5]Load");

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
                            return;
                        }
                        break;
                    case EventResult.TIMEOUT:
                        break;
                }
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
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
                            return;
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
                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_TakingDut].Set();
                                        this.Nozzle_Left_Run_To_Tray_Catch(tokenSource);

                                        bool isFail = false;//不知道是否会用到，先放着
                                        this.Check_Nozzle_IsExist(tokenSource, out isFail);

                                        
                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanUnloadDutToDevice].Set();
                                        //Parallel.Invoke(
                                        //   () =>
                                        //   {
                                        //       this.Nozzle_Left_Run_To_Safe(tokenSource);//左侧吸嘴运行到不影响右吸嘴放料到测试台的位置
                                        //   },
                                        //   () =>
                                        //   {
                                        //       Thread.Sleep(50);
                                        //       this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanUnloadDutToDevice].Set();
                                        //   });

                                        //dutPass_UnloadCount++;
                                        //if (this.parameterInformation.PositinIndex[CurrentBox] + 1 <= this.parameterInformation.ProductNumber[CurrentBox])
                                        //{
                                        this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].Set();
                                        //}
                                        //dutPass_UnloadCount--;

                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_RunToUnloadPlace].Set();
                                        //左吸嘴运行到下料X
                                        //this.Nozzle_Left_Run_To_UnloadPlace(tokenSource);
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanUnloadDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                        {
                                            case EventResult.SUCCEED:
                                                {
                                                    ////左吸嘴下料位下料
                                                    //this.Nozzle_Left_UnloadPlace_Down_Unload_UP(tokenSource);

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
                                                                    return;
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
                                                                    return;
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
                                                    return;
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
        /// 左吸嘴至测试位
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Left_Run_To_LoadDut_PreparePosition(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step5]左侧吸嘴移动到测试位");
            //左吸嘴Z原点
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSZ_左吸嘴_原点位_Z, SequenceOrder.Normal);
            //左吸嘴X测试位
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSX_左吸嘴_测试位_X, SequenceOrder.Normal);

        }
        /// <summary>
        /// 左吸嘴至测试台抓取产品
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Left_Run_To_Tray_Catch(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step5] 左吸嘴至取料位X");

            var position = this.LocalResource.Positions[AxesPositionEnum_CT40410A.LSX_左吸嘴_取料位_X].ItemCollection[0].Position;
            var MotionOffset = this.providerResourse.MotionOffset_Provider._MotionOffsetSettings.Motion_1_Offset;


            Parallel.Invoke(
                () =>
                {
                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LZR_左吸嘴_旋转_零, SequenceOrder.Normal);
                },
                () =>
                {
                    this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X], position + MotionOffset);
                });


            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.TY_载物台_等待位_Y, SequenceOrder.Normal);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.MTR_载物台_旋转_水平, SequenceOrder.Normal);
            this.Log_Global("[Step5] 左吸嘴至取料位Z");
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Nozzle].TurnOn(false);
            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Cylinder].TurnOn(false);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSZ_左吸嘴_取料位_Z, SequenceOrder.Normal);
            this.Log_Global("[Step5] 左吸嘴开启真空");
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Nozzle].TurnOn(true);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Test_Bench].TurnOn(false);
            var time1 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_1;
            Thread.Sleep(time1);
            this.Log_Global("[Step5] 左吸嘴至原点Z");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSZ_左吸嘴_原点位_Z, SequenceOrder.Normal);
            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Cylinder].TurnOn(true);

        }
        /// <summary>
        /// 左吸嘴真空检测
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <param name="isFail"></param>
        public void Check_Nozzle_IsExist(CancellationTokenSource tokenSource, out bool isFail)
        {
            int checkCount = 0;
            isFail = false;
            var lefttime1 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_2;
            Thread.Sleep(lefttime1);
            do
            {
                if (!this.LocalResource.IOs[IONameEnum_CT40410A.Input_Flow_Left].Interation.IsActive)
                {
                    checkCount++;
                    if (checkCount < 3)
                    {
                        continue;
                    }

                    //暂停所有线程
                    //this.UserRequest_MasterControl_Pause();
                    this.UserRequest_Pause(MT.吸嘴2模组);
                    Form_Nozzle_Left _Nozzle_Left = new Form_Nozzle_Left();
                    _Nozzle_Left.ShowDialog();
                    //恢复暂停
                    //this.UserRequest_MasterControl_Resume();
                    this.UserRequest_Resume(MT.吸嘴2模组);
                    if (_Nozzle_Left.DialogResult == DialogResult.OK)
                    {
                        this.Nozzle_Left_Run_To_Tray_Catch(tokenSource);//重新抓
                        Thread.Sleep(200);
                    }
                    else if (_Nozzle_Left.DialogResult == DialogResult.Cancel)
                    {
                        tokenSource.Cancel();
                        break;
                    }
                    else if (_Nozzle_Left.DialogResult == DialogResult.Yes)
                    {
                        //继续执行
                        break;
                    }
                }
                break;
            } while (true);
        }
        /// <summary>
        /// 左侧吸嘴上升,X轴到不影响右吸嘴放料到测试台的位置
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Left_Run_To_Safe(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step5]左侧吸嘴上升,X轴到不影响右吸嘴放料到测试台的位置");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSX_左吸嘴_等待位_X, SequenceOrder.Normal);
        }
        /// <summary>
        /// 左吸嘴X运行到下料位 此动作移至线程六执行
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Left_Run_To_UnloadPlace(CancellationTokenSource tokenSource)
        {
            //此处是否需要左吸嘴Z上升到原点
            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_Demo.左侧单元吸嘴_安全位_Z, SequenceOrder.Normal);

            //左吸嘴X到达下料位
            //this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X], product_UnLoad[dutPass_UnloadCount].X);


            //
        }
        /// <summary>
        /// 左吸嘴下料位下料
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Left_UnloadPlace_Down_Unload_UP(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step5]左侧吸嘴下降,放下料，上升");
            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Cylinder].TurnOn(false);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSZ_左吸嘴_放料位_Z, SequenceOrder.Normal);
            //IO 控制
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Nozzle].TurnOn(false);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Nozzle_Vac].TurnOn(true);
            var lefttime1 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_1;
            Thread.Sleep(lefttime1);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSZ_左吸嘴_原点位_Z, SequenceOrder.Normal);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Left_Nozzle_Vac].TurnOn(false);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSX_左吸嘴_等待位_X, SequenceOrder.Normal);
        }
        /// <summary>
        /// 左吸嘴到扫码位置
        /// </summary>
        /// <param name="tokenSource"></param>
        //public void Nozzle_Left_Run_To_ScannerPosition(CancellationTokenSource tokenSource)
        //{
        //    this.Log_Global("[Step5]左侧吸嘴移动到扫码位置");
        //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LX_左侧扫码位_X, SequenceOrder.Normal);

        //}

    }

}
