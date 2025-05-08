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
        public void LaserX_Step1_DryRun(CancellationTokenSource tokenSource)
        {
            isEndRun = false;
            StopSearch = false;
            while (true)
            {

                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Log_Global("[Step1]Step1Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step1]用户取消运行");
                            Cancell();
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }

                //接收开始抓料信号
                //搜索产品、抓取产品
                switch (this.Bridges_WithPauseFunc[ARECT40410A.GrabEnable].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            if (isEndRun || StopSearch)
                            {
                                if (this.Status.StageProducts<=0)
                                {
                                    this.Bridges_WithPauseFunc[ARECT40410A.EndRun].Set();
                                }
                                else
                                {
                                    Thread.Sleep(3000);
                                    this.Bridges_WithPauseFunc[ARECT40410A.LoadGradFinish].Set();
                                }
                                
                                break;
                            }
                            DataBook<ProductPosition, double> inputstting = null;

                            VisionResult_LaserX_Image_Universal visionMatch1 = new VisionResult_LaserX_Image_Universal(); visionMatch1.Success = false;
                            VisionResult_LaserX_Image_Universal visionMatch2 = new VisionResult_LaserX_Image_Universal(); visionMatch2.Success = false;
                            if (this.parameterInformation.LeftNozzle)//判断吸嘴1能否抓取
                            {
                                int unSuccess = 0;

                                while (true)
                                {
                                    if (loadboxindex >= this.parameterInformation.feedBoxes.Count)
                                    {
                                        this.UserRequest_Pause(MT.上料模组);
                                        Form_ChangeBox _ChangBox = new Form_ChangeBox();
                                        _ChangBox.ConnectToAppInteration(this);
                                        _ChangBox.ConnectToCore(_coreInteration);
                                        _ChangBox.ShowDialog();
                                        this.UserRequest_Resume(MT.上料模组);
                                        if (_ChangBox.DialogResult == DialogResult.OK)
                                        {
                                            searchIndex = 0;
                                            loadboxindex = 0;
                                            continue;
                                        }
                                        if (_ChangBox.DialogResult == DialogResult.Cancel)
                                        {
                                            isEndRun = true;
                                            this.Bridges_WithPauseFunc[ARECT40410A.EndRun].Set();
                                            break;
                                        }
                                        if (_ChangBox.DialogResult == DialogResult.Yes)
                                        {
                                            StopSearch = true;
                                        }
                                    }
                                    if (isEndRun || StopSearch)
                                    {
                                        break;
                                    }

                                    LoadMatrix(tokenSource, out inputstting);

                                    this.MoveToPosition(tokenSource, product_Load[searchIndex]);
                                    this.Log_Global("[Step1]开始相机识别");

                                    visionMatch1.Success = true;

                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    var Xshaft2 = this.LocalResource.Axes[AxisNameEnum_CT40410A.龙门_X].Get_CurUnitPos();
                                    var Yshaft2 = this.LocalResource.Axes[AxisNameEnum_CT40410A.龙门_Y].Get_CurUnitPos();
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_左吸嘴真空电磁阀].TurnOn(false);
                                    //设定左吸嘴点位后更改
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.左吸嘴_待机点_Z, SequenceOrder.Normal);

                                    var OffsetX = this.providerResourse.RunSettings_Provider._RunParamSettings.左吸嘴相机间距_X;
                                    var OffsetY = this.providerResourse.RunSettings_Provider._RunParamSettings.左吸嘴相机间距_Y;
                                    xY1 = new XYCoord(Xshaft2 - OffsetX, Yshaft2 - OffsetY);

                                    searchIndex++;
                                    if (searchIndex >= inputstting[ProductPosition.行数] * inputstting[ProductPosition.列数])
                                    {
                                        searchIndex = 0;
                                        loadboxindex++;
                                    }
                                    break;
                                }


                            }
                            if (isEndRun || StopSearch)
                            {
                                Thread.Sleep(3000);
                                this.Bridges_WithPauseFunc[ARECT40410A.LoadGradFinish].Set();
                                break;
                            }
                            if (this.parameterInformation.MiddleNozzle)
                            {
                                int unSuccess = 0;

                                while (true)
                                {
                                    if (loadboxindex >= this.parameterInformation.feedBoxes.Count)
                                    {
                                        this.UserRequest_Pause(MT.上料模组);
                                        Form_ChangeBox _ChangBox = new Form_ChangeBox();
                                        _ChangBox.ConnectToAppInteration(this);
                                        _ChangBox.ConnectToCore(_coreInteration);
                                       _ChangBox.ShowDialog();
                                        this.UserRequest_Resume(MT.上料模组);
                                        if (_ChangBox.DialogResult == DialogResult.OK)
                                        {
                                            searchIndex = 0;
                                            loadboxindex = 0;
                                            continue;
                                        }
                                        if (_ChangBox.DialogResult == DialogResult.Cancel)
                                        {
                                            isEndRun = true;
                                            this.Bridges_WithPauseFunc[ARECT40410A.EndRun].Set();
                                            break;
                                        }
                                        if (_ChangBox.DialogResult == DialogResult.Yes)
                                        {
                                            StopSearch = true;
                                        }
                                    }
                                    if (isEndRun || StopSearch)
                                    {
                                        break;
                                    }

                                    LoadMatrix(tokenSource, out inputstting);

                                    this.MoveToPosition(tokenSource, product_Load[searchIndex]);
                                    this.Log_Global("[Step1]开始相机识别");

                                    visionMatch2.Success = true;

                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                   
                                    var Xshaft2 = this.LocalResource.Axes[AxisNameEnum_CT40410A.龙门_X].Get_CurUnitPos();
                                    var Yshaft2 = this.LocalResource.Axes[AxisNameEnum_CT40410A.龙门_Y].Get_CurUnitPos();
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_中吸嘴真空电磁阀].TurnOn(false);
                                    //设定左吸嘴点位后更改
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.中吸嘴_待机点_Z, SequenceOrder.Normal);

                                    var OffsetX = this.providerResourse.RunSettings_Provider._RunParamSettings.中吸嘴相机间距_X;
                                    var OffsetY = this.providerResourse.RunSettings_Provider._RunParamSettings.中吸嘴相机间距_Y;
                                    xY2 = new XYCoord(Xshaft2 - OffsetX, Yshaft2 - OffsetY);

                                    searchIndex++;
                                    if (searchIndex >= inputstting[ProductPosition.行数] * inputstting[ProductPosition.列数])
                                    {
                                        searchIndex = 0;
                                        loadboxindex++;
                                    }
                                    break;
                                    //}
                                    //else
                                    //{
                                    //    unSuccess++;
                                    //    if (unSuccess > 3)
                                    //    {
                                    //        //多次未找到匹配产品，需判断是否测试完成需要抓取产品
                                    //        searchIndex++;
                                    //        break;
                                    //    }
                                    //}
                                }


                            }
                            if (isEndRun || StopSearch)
                            {
                                Thread.Sleep(3000);
                                this.Bridges_WithPauseFunc[ARECT40410A.LoadGradFinish].Set();
                                break;
                            }
                            if (visionMatch1.Success)
                            {
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.Log_Global("[Step1]左吸嘴开始抓取");
                                this.MoveToPosition(tokenSource, xY1);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.左吸嘴_上料盘_取料点_Z, SequenceOrder.Normal);
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.LocalResource.IOs[IONameEnum_CT40410A.Output_左吸嘴真空电磁阀].TurnOn(true);
                                var delaytime1 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_1;
                                Thread.Sleep(delaytime1);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.左吸嘴_待机点_Z, SequenceOrder.Normal);
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.左吸嘴旋转_待机点_θ, SequenceOrder.Normal);
                                //可加IO判断
                                //bool isCotinue = false;
                                //this.Check_Left_Nozzle_IsExist(tokenSource, out isCotinue);
                                //if (isCotinue)
                                //{
                                //    this.Log_Global("[Step1]左吸嘴IO检测不合格，重新抓取");
                                //    continue;
                                //}
                                this.Status.LoadStatusPro[0] = NozzlePorduct.Yes;
                                this.Log_Global("[Step1]左吸嘴抓取完成");
                            }
                            if (visionMatch2.Success)
                            {
                                this.Log_Global("[Step1]中吸嘴开始抓取");
                                this.MoveToPosition(tokenSource, xY2);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.中吸嘴_上料盘_取料点_Z, SequenceOrder.Normal);
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.LocalResource.IOs[IONameEnum_CT40410A.Output_中吸嘴真空电磁阀].TurnOn(true);
                                var delaytime1 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_1;
                                Thread.Sleep(delaytime1);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.中吸嘴_待机点_Z, SequenceOrder.Normal);
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.中吸嘴旋转_待机点_θ, SequenceOrder.Normal);
                                //可加IO判断
                                //bool isCotinue = false;
                                //this.Check_Left_Nozzle_IsExist(tokenSource, out isCotinue);
                                //if (isCotinue)
                                //{
                                //    this.Log_Global("[Step1]中吸嘴IO检测不合格，重新抓取");
                                //    continue;
                                //}
                                this.Status.LoadStatusPro[1] = NozzlePorduct.Yes;
                                this.Log_Global("[Step1]中吸嘴抓取完成");
                            }

                            this.Bridges_WithPauseFunc[ARECT40410A.LoadGradFinish].Set();



                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step1]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }


                //向步骤二发送转运信号

            }

        }
    }

}
