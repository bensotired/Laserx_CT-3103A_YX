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
using Thorlabs.TLPM_32.Interop;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT40410A
    {
        public void LaserX_Step2_Nozzle_Right(CancellationTokenSource tokenSource)
        {
            dutLoad = 0;
            this.Log_Global($"[Step2]Load  dutLoad:{dutLoad}");
            Thread.Sleep(100);
            do
            {
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Bridges_WithPauseFunc[ARECT40410A.Step2Finish].Set();
                            this.Log_Global("[Step2]Step2Finish");
                            return;
                        }
                        break;
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step2]用户取消运行");
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
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanTakeDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        //右吸嘴至取料位取料
                                        Nozzle_Right_TakeDutUp(tokenSource);
                                        SearchProduct search = searchProducts[searchProducts.Count - 1];
                                        //searchProducts.RemoveAt(0);
                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_TakeDutFinish].Set();//这是让线程1重新匹配物料

                                        //IO检测
                                        bool isCotinue = false;
                                        //this.Check_Right_Nozzle_IsExist(tokenSource, out isCotinue);if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        if (isCotinue)
                                        {
                                            this.Log_Global("[Step2]IO检测不合格，重新抓取");
                                            if (searchIndex > 0)
                                            {
                                                searchIndex--;
                                            }

                                            index.RemoveAt(index.Count - 1);
                                            this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].Set();
                                            continue;
                                        }
                                        this.Log_Global("[Step2]IO检测合格，继续运行");
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
                                        this.Nozzle_Right_Camera2_Match(tokenSource, out isNeedReLoadTec);
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        if (isNeedReLoadTec)
                                        {
                                            if (tokenSource.IsCancellationRequested)
                                            {
                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                return;
                                            }
                                            On_N_R_Tray.RemoveAt(On_N_R_Tray.Count - 1);
                                            if (searchIndex > 0)
                                            {
                                                searchIndex--;
                                            }
                                            index.RemoveAt(index.Count - 1);
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
                                                        if (TrunOn)
                                                        {
                                                            //左吸嘴测试激光功率
                                                            //this.Nozzle_Left(tokenSource);
                                                        }
                                                        //产品放置测试台,可增加相机三动作
                                                        this.Nozzle_Right_ToDevice_ToUnload(tokenSource);
                                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_UnloadDutToDeviceFinish].Set();
                                                        //右吸嘴应该回到准备位
                                                        this.Nozzle_Right_RunToIdlePosition(tokenSource);

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
                                        break;
                                    }

                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step2]用户取消运行");
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
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
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
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
                                                            return;
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
                                                            return;
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
        /// 右吸嘴至取料位取料
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Right_TakeDutUp(CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            this.Log_Global("[Step2]右侧吸嘴移动到取料位，取料");
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle].TurnOn(false);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RZR_右吸嘴_旋转_零, SequenceOrder.Normal);
            var position = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_取料位_X].ItemCollection[0].Position;
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            var MotionOffset = this.providerResourse.MotionOffset_Provider._MotionOffsetSettings.Motion_1_Offset;

            this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X], position + MotionOffset);

            var offset = this.providerResourse.RunSettings_Provider._RunParamSettings.右吸嘴相机间距_X;
            var Yshaft = this.LocalResource.Axes[AxisNameEnum_CT40410A.RY_进料_Y].Get_CurUnitPos();
            this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RY_进料_Y].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RY_进料_Y], Yshaft + offset);

            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Load_Tray].TurnOn(true);
            //吸嘴下降前需要IO 控制
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_取料位_Z, SequenceOrder.Normal);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle].TurnOn(true);
            var righttime1 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_1;
            Thread.Sleep(righttime1);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //吸嘴上升前需要IO 控制
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Load_Tray].TurnOn(false);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Cylinder].TurnOn(true);
            this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RY_进料_Y].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RY_进料_Y], Yshaft);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);

        }
        /// <summary>
        /// 右吸嘴IO检测
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <param name="isCotinue"></param>
        public void Check_Right_Nozzle_IsExist(CancellationTokenSource tokenSource, out bool isCotinue)
        {
            int checkCount = 0;
            isCotinue = false;
            do
            {
                //暂不知道判定条件
                if (!this.LocalResource.IOs[IONameEnum_CT40410A.Input_Flow_Right].Interation.IsActive)
                {
                    checkCount++;
                    if (checkCount < 3)
                    {
                        continue;
                    }
                    //暂停所有线程
                    //this.UserRequest_MasterControl_Pause();
                    this.UserRequest_Pause(MT.吸嘴1模组);
                    Form_Nozzle_Right _Right = new Form_Nozzle_Right();
                    _Right.ShowDialog();
                    //恢复暂停
                    //this.UserRequest_MasterControl_Resume();
                    this.UserRequest_Resume(MT.吸嘴1模组);
                    if (_Right.DialogResult == DialogResult.OK)
                    {
                        isCotinue = true;//重新抓
                        break;
                    }
                    else if (_Right.DialogResult == DialogResult.Yes)
                    {
                        break;
                    }
                    else if (_Right.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("[Step2]右吸嘴流量计判定未通过，用户取消运行");
                        tokenSource.Cancel();
                        return;
                    }
                }
                break;
            } while (true);
        }
        /// <summary>
        /// 至相机2 匹配
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <param name="isNeedReLoadTec"></param>
        public void Nozzle_Right_Camera2_Match(CancellationTokenSource tokenSource, out bool isNeedReLoadTec)
        {
            isNeedReLoadTec = false;//是否重新抓料
            int count = 0;
            this.Log_Global("[Step2]相机2匹配");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_相机2_Z, SequenceOrder.Normal);
            bool AngleOerrun = false;

            VisionResult_LaserX_Image_Universal visionMatch2 = null;
            VisionResult_LaserX_Image_Universal visionMatch3 = null;
            while (true)
            {
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                count++;
                var cmdList = this.providerResourse.VisionComboCommand_Provider[VisionCMD_CT40410A.相机2_产品_搜索];

                foreach (var item in cmdList)
                {
                    for (int j = 0; j < 3; j++)
                    {

                        visionMatch2 = this.LocalResource.VisionController.GetVisionResult_Universal(item, delayTime);
                        if (visionMatch2.Success)
                        {

                            double pos_mm_current = this.LocalResource.Axes[AxisNameEnum_CT40410A.RZR_右侧单元吸嘴_旋转].Get_CurUnitPos();
                            double pos_mm_next = pos_mm_current - visionMatch2.PeekAngle / 2;
                            this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RZR_右侧单元吸嘴_旋转].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RZR_右侧单元吸嘴_旋转], pos_mm_next);
                            break;
                        }
                    }
                    if (visionMatch2.Success)
                    {
                        break;
                    }
                }
                if (visionMatch2.Success)
                {
                    while (true)
                    {
                        if (tokenSource.IsCancellationRequested)
                        {
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }
                        foreach (var item in cmdList)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                visionMatch3 = this.LocalResource.VisionController.GetVisionResult_Universal(item, delayTime);
                                if (visionMatch3.Success)
                                {
                                    if (Math.Abs(visionMatch3.PeekAngle) < this.providerResourse.RunSettings_Provider._RunParamSettings.相机2允许角度误差)
                                    {
                                        var Xshaft = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_放料位_X].ItemCollection[0].Position;
                                        var Yshaft = this.LocalResource.Positions[AxesPositionEnum_CT40410A.TY_载物台_工作位_Y].ItemCollection[0].Position;
                                        var Xppm = this.providerResourse.PPM_Provider[PPM_Enum_CT40410A.相机2_PPM].X_ppm;
                                        var Yppm = this.providerResourse.PPM_Provider[PPM_Enum_CT40410A.相机2_PPM].Y_ppm;
                                        XYCoord coord = this.CalPosition(Xshaft, Yshaft, Xppm, Yppm, visionMatch3.ImageWidth / 2, visionMatch3.ImageHeight / 2,
                                                                                            visionMatch3.PeekCenterX_Pix, visionMatch3.PeekCenterY_Pix);
                                        Cm2.Add(coord);
                                        this.Log_Global($"匹配角度为:{visionMatch3.PeekAngle}");
                                        return;
                                    }
                                    else
                                    {
                                        AngleOerrun = true;
                                    }

                                }
                            }
                            if (AngleOerrun)
                            {
                                break;
                            }
                        }
                        if (AngleOerrun)
                        {
                            double pos_mm_current = this.LocalResource.Axes[AxisNameEnum_CT40410A.RZR_右侧单元吸嘴_旋转].Get_CurUnitPos();
                            double pos_mm_next = pos_mm_current - visionMatch3.PeekAngle / 2;
                            this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RZR_右侧单元吸嘴_旋转].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RZR_右侧单元吸嘴_旋转], pos_mm_next);
                            continue;
                        }
                    }

                }
                if (count > 10)
                {
                    //暂停
                    //this.UserRequest_MasterControl_Pause();//全线程
                    this.UserRequest_Pause(MT.吸嘴1模组);
                    Form_Camera2_Match camera2_Match = new Form_Camera2_Match();
                    camera2_Match.cmdList = cmdList;
                    camera2_Match.ShowDialog();
                    //回复暂停
                    //this.UserRequest_MasterControl_Resume();//全线程
                    this.UserRequest_Resume(MT.吸嘴1模组);
                    if (camera2_Match.DialogResult == DialogResult.OK)
                    {
                        isNeedReLoadTec = true;
                        //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.右吸嘴Z_零位, SequenceOrder.Normal);
                        //右吸嘴X轴需要移动吗

                        return;
                    }
                    else if (camera2_Match.DialogResult == DialogResult.Yes)
                    {
                        continue;
                    }
                    else if (camera2_Match.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("相机2匹配失败  用户取消继续搜索");
                        tokenSource.Cancel();
                        return;
                    }
                    else if (camera2_Match.DialogResult == DialogResult.None)
                    {
                        this.providerResourse.VisionComboCommand_Provider[VisionCMD_CT40410A.相机2_产品_搜索].Add(camera2_Match.NewVisionCmd);
                        this.providerResourse.SaveProduct_VisionComboCommand_Config();
                    }
                }

            }

            //右吸嘴是否需要走到安全Z
        }
        /// <summary>
        /// 右吸嘴将产品放置测试台
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Right_ToDevice_ToUnload(CancellationTokenSource tokenSource)
        {
            if (!TrunOn)
            {
                while (true)
                {
                    Ting(tokenSource);
                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    var LeftPosion = this.LocalResource.Axes[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X].Get_CurUnitPos();
                    var LP = this.LocalResource.Positions[AxesPositionEnum_CT40410A.LSX_左吸嘴_等待位_X].ItemCollection[0].Position;
                    if (LeftPosion <= 1)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
            this.Log_Global("[Step2]右侧吸嘴移动到平台并且卸载料，然后回到初始位置");
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            var position = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_放料位_X].ItemCollection[0].Position;

            var MotionOffset = this.providerResourse.MotionOffset_Provider._MotionOffsetSettings.Motion_1_Offset;
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            Parallel.Invoke(
              () =>
              {
                  this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X], Cm2[0].X);
              },
              () =>
              {
                  //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.TY_载物台_工作位_Y, SequenceOrder.Normal);
                  this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.TY_载物台_Y].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.TY_载物台_Y], Cm2[0].Y);
              },
              () =>
              {
                  this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.MTR_载物台_旋转_水平, SequenceOrder.Normal);
              });
            Cm2.RemoveAt(0);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Cylinder].TurnOn(false);

            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_放料位_Z, SequenceOrder.Normal);
            
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //IO
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Test_Bench].TurnOn(true);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle].TurnOn(false);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle_Vac].TurnOn(true);
            var righttime1 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_2;
            Thread.Sleep(righttime1);

            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            Parallel.Invoke(
               () =>
               {
                   this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
                   this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle_Vac].TurnOn(false);
               },
               () =>
               {
                   this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RZR_右吸嘴_旋转_零, SequenceOrder.Normal);
               });
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.TY_载物台_工作位_Y, SequenceOrder.Normal);
        }
        /// <summary>
        /// 右吸嘴将产品放置测试台---空跑
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Right_ToDevice_ToUnload_DryRun(CancellationTokenSource tokenSource)
        {
            //Thread.Sleep(4000);
            this.Log_Global("[Step2]右侧吸嘴移动到平台并且卸载料，然后回到初始位置");
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            var position = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_放料位_X].ItemCollection[0].Position;

            var MotionOffset = this.providerResourse.MotionOffset_Provider._MotionOffsetSettings.Motion_1_Offset;
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            Parallel.Invoke(
              () =>
              {
                  this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_放料位_X, SequenceOrder.Normal);
              },
              () =>
              {
                  this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.TY_载物台_工作位_Y, SequenceOrder.Normal);
              },
              () =>
              {
                  this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.MTR_载物台_旋转_水平, SequenceOrder.Normal);
              });
            //Cm2.RemoveAt(0);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Cylinder].TurnOn(false);

            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_放料位_Z, SequenceOrder.Normal);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.TY_载物台_工作位_Y, SequenceOrder.Normal);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //IO
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Test_Bench].TurnOn(true);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle].TurnOn(false);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle_Vac].TurnOn(true);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            Parallel.Invoke(
               () =>
               {
                   this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
                   this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle_Vac].TurnOn(false);
               },
               () =>
               {
                   this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RZR_右吸嘴_旋转_零, SequenceOrder.Normal);
               });
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
        }
        /// <summary>
        /// 右吸嘴运行至准备位
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Right_RunToIdlePosition(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step2]右侧吸嘴卸载料后，回到准备位置");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);
        }
        /// <summary>
        /// 准备将倒二放回
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Right_MoveToIdelPlace_DESC_Second(CancellationTokenSource tokenSource)
        {
            //右吸嘴Z回零位
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
            //右吸嘴X回准备位
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);
        }
        /// <summary>
        /// 将右吸嘴上产品放回料盒
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Right_Releas_MoveToIdelPlace(CancellationTokenSource tokenSource)
        {
            //this.Log_Global("[Step2]吸嘴右Z上升到零点位");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
            //this.Log_Global("[Step2]吸嘴右X侧移动到抓料位置");
            var position = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_取料位_X].ItemCollection[0].Position;
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            var MotionOffset = this.providerResourse.MotionOffset_Provider._MotionOffsetSettings.Motion_1_Offset;

            this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X], position + MotionOffset);
            //this.Log_Global("[Step2]吸嘴右Z侧下降放料");
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_取料位_Z, SequenceOrder.Normal);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Load_Tray].TurnOn(true);
            //this.Log_Global("[Step2]吸嘴关闭吸气，打开吹气");
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle].TurnOn(false);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle_Vac].TurnOn(true);
            //this.Log_Global("[Step2]吸嘴右Z运行到准备位");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //this.Log_Global("[Step2]吸嘴关闭吹气");
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle_Vac].TurnOn(false);
            //this.Log_Global("[Step2]吸嘴右X运行到准备位");
            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);
        }
        /// <summary>
        /// 将测试台产品放回料盒
        /// 倒数第一个
        /// </summary>
        /// <param name="tokenSource"></param>
        public void Nozzle_Right_Add_TecFromTray_MoveToIdelPlace_DESC_First(CancellationTokenSource tokenSource)
        {
            //this.Log_Global("[Step2]吸嘴右Z上升到零点位_最后一次");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RZR_右吸嘴_旋转_零, SequenceOrder.Normal);
            //this.Log_Global("[Step2]吸嘴右X移动到测试平台_最后一次");
            var position = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_放料位_X].ItemCollection[0].Position;
            var MotionOffset = this.providerResourse.MotionOffset_Provider._MotionOffsetSettings.Motion_1_Offset;
            this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X], position + MotionOffset);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //var position1 = this.LocalResource.Positions[AxesPositionEnum_CT40410A.TY_载物台_工作位_Y].ItemCollection[0].Position;
            //var MotionOffset1 = this.providerResourse.MotionOffset_Provider._MotionOffsetSettings.Taiwan_Work_Y_Offset;
            //this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.TY_载物台_Y].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.TY_载物台_Y], position1 + MotionOffset1);

            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.TY_载物台_工作位_Y, SequenceOrder.Normal);

            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.MTR_载物台_旋转_水平, SequenceOrder.Normal);
            //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Cylinder].TurnOn(false);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //this.Log_Global("[Step2]吸嘴右Z下降吸料_最后一次");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_放料位_Z, SequenceOrder.Normal);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            //this.Log_Global("[Step2]吸嘴打开，平台关闭吸气_最后一次");
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Test_Bench].TurnOn(false);
            this.LocalResource.IOs[IONameEnum_CT40410A.Output_Right_Nozzle].TurnOn(true);
            //this.Log_Global("[Step2]吸嘴右Z上升到零点位_最后一次");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSZ_右吸嘴_原点位_Z, SequenceOrder.Normal);
            //this.Log_Global("[Step2]吸嘴右X运行到准备位_最后一次");
            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);

        }

        //public void Nozzle_Left(CancellationTokenSource tokenSource)
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            Ting(tokenSource);
        //            if (tokenSource.IsCancellationRequested)
        //            {
        //                tokenSource.Token.ThrowIfCancellationRequested();
        //                return;
        //            }
        //            var LeftPosion = this.LocalResource.Axes[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X].Get_CurUnitPos();
        //            var LP = this.LocalResource.Positions[AxesPositionEnum_CT40410A.LSX_左吸嘴_等待位_X].ItemCollection[0].Position;
        //            if (LeftPosion <= 1)
        //            {
        //                break;
        //            }
        //            Thread.Sleep(1000);
        //        }
        //        var posion = this.LocalResource.Axes[AxisNameEnum_CT40410A.RSX_右侧单元吸嘴_X].Get_CurUnitPos();
        //        var P = this.LocalResource.Positions[AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X].ItemCollection[0].Position;
        //        if (tokenSource.IsCancellationRequested)
        //        {
        //            tokenSource.Token.ThrowIfCancellationRequested();
        //            return;
        //        }
        //        Ting(tokenSource);
        //        if (posion < P)
        //        {
        //            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.RSX_右吸嘴_相机2_X, SequenceOrder.Normal);
        //        }
        //        if (tokenSource.IsCancellationRequested)
        //        {
        //            tokenSource.Token.ThrowIfCancellationRequested();
        //            return;
        //        }
        //        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSZ_左吸嘴_原点位_Z, SequenceOrder.Normal);
        //        if (tokenSource.IsCancellationRequested)
        //        {
        //            tokenSource.Token.ThrowIfCancellationRequested();
        //            return;
        //        }
        //        Ting(tokenSource);
        //        Parallel.Invoke(
        //         () =>
        //         {
        //             if (tokenSource.IsCancellationRequested)
        //             {
        //                 tokenSource.Token.ThrowIfCancellationRequested();
        //                 return;
        //             }
        //             this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSX_左吸嘴_测试位_X, SequenceOrder.Normal);
        //         },
        //         () =>
        //         {
        //             this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.TY_载物台_工作位_Y, SequenceOrder.Normal);
        //         },
        //         () =>
        //         {
        //             this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.MTR_载物台_旋转_水平, SequenceOrder.Normal);
        //         });
        //        Ting(tokenSource);
        //        TLPM tlpm = null;
        //        double CalibPower = 0;
        //        double wavelen = 1;
        //        double power = 0;
        //        this.LocalResource.IOs[IONameEnum_CT40410A.Output_Middle_ShadCylinder].TurnOn(true);
        //        if (this.LocalResource.TMPL_Master.InitializePM(ref tlpm) == false)
        //        {
        //            while (true)
        //            {
        //                this.UserRequest_Pause(MT.吸嘴1模组);
        //                Form_TLPM form_TLPM = new Form_TLPM();
        //                form_TLPM.ShowDialog();
        //                this.UserRequest_Resume(MT.吸嘴1模组);
        //                if (form_TLPM.DialogResult == DialogResult.OK)
        //                {
        //                    if (!this.LocalResource.TMPL_Master.InitializePM(ref tlpm) == false)
        //                    {
        //                        break;
        //                    }
        //                }
        //                else if (form_TLPM.DialogResult == DialogResult.Cancel)
        //                {
        //                    this.Log_Global("[Step2]未发现 Thorlabs 功率计探头");
        //                    tokenSource.Cancel();
        //                    return;
        //                }
        //            }
                   
                    
        //        }
        //        if (double.TryParse(this.Step2Wavelen, out wavelen) == false)
        //        {
        //            MessageBox.Show("目标波长值格式错误!");
        //            tokenSource.Cancel();
        //            return;
        //        }
        //        for (int i = 0; i < 5; i++)
        //        {
        //            Ting(tokenSource);
        //            if (tokenSource.IsCancellationRequested)
        //            {
        //                tokenSource.Token.ThrowIfCancellationRequested();
        //                return;
        //            }
        //            tlpm.setWavelength(wavelen);
        //            tlpm.setPowerAutoRange(true);

        //            //读取功率
        //            tlpm.measPower(out CalibPower);
        //            CalibPower *= 1000000;  //W->uW
        //            power += CalibPower;
        //            Thread.Sleep(100);
        //        }
        //        this.Light_uW = power / 5;
        //        tlpm.Dispose();
        //        this.LocalResource.IOs[IONameEnum_CT40410A.Output_Middle_ShadCylinder].TurnOn(false);
        //        if (tokenSource.IsCancellationRequested)
        //        {
        //            tokenSource.Token.ThrowIfCancellationRequested();
        //            return;
        //        }
        //        Ting(tokenSource);
        //        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSX_左吸嘴_等待位_X, SequenceOrder.Normal);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Log_Global($"{ex.Message}-{ex.StackTrace}");
        //        throw new Exception($"{ex.Message}-{ex.StackTrace}");
        //    }

        //}

        public void Ting(CancellationTokenSource tokenSource)
        {
            this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_FinishRuntoDescFirstPlace].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource);
        }
    }

}
