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

            List<FeedBox_Out> box_out = new List<FeedBox_Out> { FeedBox_Out.UnLoad_1, FeedBox_Out.UnLoad_2, FeedBox_Out.UnLoad_3, FeedBox_Out.UnLoad_4, FeedBox_Out.UnLoad_5, FeedBox_Out.UnLoad_6,
            FeedBox_Out.UnLoad_7,FeedBox_Out.UnLoad_8,FeedBox_Out.UnLoad_9,FeedBox_Out.UnLoad_10,FeedBox_Out.UnLoad_11,FeedBox_Out.UnLoad_12};
            int unloadboxindex = 0;
            int unloadIndex = 0;
            while (true)
            {
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step4Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {

                            this.Log_Global("[Step4]Step4Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step4]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }


                switch (this.Bridges_WithPauseFunc[ARECT40410A.InterchangeAndUnload].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            //现不做分选处理，统一放置
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessage(tokenSource);
                                var loadbox = box_out[unloadboxindex];
                                var _OutputSettings = this.providerResourse.OutPutSettings_Provider.OutputSettings.OutFeed[loadbox];
                                var product_Load = this.CreatePointList(_OutputSettings[ProductPosition.左上角坐标_X], _OutputSettings[ProductPosition.左上角坐标_Y],
                                        _OutputSettings[ProductPosition.右下角坐标_X], _OutputSettings[ProductPosition.右下角坐标_Y],
                                        _OutputSettings[ProductPosition.行数], _OutputSettings[ProductPosition.列数]);
                                double number = _OutputSettings[ProductPosition.行数] * _OutputSettings[ProductPosition.列数];
                                if (number <= unloadIndex)
                                {
                                    unloadboxindex++;
                                    unloadIndex = 0;
                                    continue;
                                }

                                if (this.Status.LoadStatusPro[2] == NozzlePorduct.Yes)
                                {
                                    if (unloadboxindex >= box_out.Count)
                                    {
                                        this.UserRequest_Pause(MT.下料模组);
                                        Form_ChangeBox_Out _ChangeBox_Out = new Form_ChangeBox_Out();
                                        _ChangeBox_Out.ConnectToAppInteration(this);
                                        _ChangeBox_Out.ConnectToCore(_coreInteration);
                                        _ChangeBox_Out.ShowDialog();
                                        this.UserRequest_Resume(MT.下料模组);
                                        if (_ChangeBox_Out.DialogResult == DialogResult.OK)
                                        {
                                            unloadboxindex = 0;
                                        }
                                        if (_ChangeBox_Out.DialogResult == DialogResult.Cancel)
                                        {

                                            this.Bridges_WithPauseFunc[ARECT40410A.EndRun].Set();
                                            break;
                                        }
                                    }
                                    var Xshaft = product_Load[unloadIndex].X;//this._plugin.LocalResource.Axes[AxisNameEnum_CT40410A.龙门_X].Get_CurUnitPos();
                                    var Yshaft = product_Load[unloadIndex].Y;// this._plugin.LocalResource.Axes[AxisNameEnum_CT40410A.龙门_Y].Get_CurUnitPos();
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.右吸嘴_待机点_Z, SequenceOrder.Normal);
                                    var OffsetX = this.providerResourse.RunSettings_Provider._RunParamSettings.右吸嘴相机间距_X;
                                    var OffsetY = this.providerResourse.RunSettings_Provider._RunParamSettings.右吸嘴相机间距_Y;
                                    XYCoord xY = new XYCoord(Xshaft - OffsetX, Yshaft - OffsetY);
                                    this.MoveToPosition(tokenSource, xY);
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.右吸嘴_下料盘_放料点_Z, SequenceOrder.Normal);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_右吸嘴真空电磁阀].TurnOn(false);
                                    var delaytime5 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_5;
                                    Thread.Sleep(delaytime5);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.右吸嘴_待机点_Z, SequenceOrder.Normal);
                                    this.Status.LoadStatusPro[2] = NozzlePorduct.No;

                                    this.Log_Global($"[Step4]放料到{loadbox} {unloadIndex + 1}");
                                    unloadIndex++;
                                }
                                if (this.Status.LoadStatusPro[1] == NozzlePorduct.Yes)
                                {
                                    if (unloadboxindex >= box_out.Count)
                                    {
                                        this.UserRequest_Pause(MT.下料模组);
                                        Form_ChangeBox_Out _ChangeBox_Out = new Form_ChangeBox_Out();
                                        _ChangeBox_Out.ConnectToAppInteration(this);
                                        _ChangeBox_Out.ConnectToCore(_coreInteration);
                                        _ChangeBox_Out.ShowDialog();
                                        this.UserRequest_Resume(MT.下料模组);
                                        if (_ChangeBox_Out.DialogResult == DialogResult.OK)
                                        {
                                            unloadboxindex = 0;
                                        }
                                        if (_ChangeBox_Out.DialogResult == DialogResult.Cancel)
                                        {

                                            this.Bridges_WithPauseFunc[ARECT40410A.EndRun].Set();
                                            break;
                                        }
                                    }
                                    var Xshaft = product_Load[unloadIndex].X;
                                    var Yshaft = product_Load[unloadIndex].Y;
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.中吸嘴_待机点_Z, SequenceOrder.Normal);
                                    var OffsetX = this.providerResourse.RunSettings_Provider._RunParamSettings.中吸嘴相机间距_X;
                                    var OffsetY = this.providerResourse.RunSettings_Provider._RunParamSettings.中吸嘴相机间距_Y;
                                    XYCoord xY = new XYCoord(Xshaft - OffsetX, Yshaft - OffsetY);
                                    this.MoveToPosition(tokenSource, xY);
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.中吸嘴_下料盘_放料点_Z, SequenceOrder.Normal);
                                    this.LocalResource.IOs[IONameEnum_CT40410A.Output_中吸嘴真空电磁阀].TurnOn(false);
                                    var delaytime5 = this.providerResourse.DelayTimeSettings_Provider._DelayTimeSettings.DelayTime_5;
                                    Thread.Sleep(delaytime5);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.中吸嘴_待机点_Z, SequenceOrder.Normal);
                                    this.Status.LoadStatusPro[1] = NozzlePorduct.No;
                                    this.Log_Global($"[Step4]放料到{loadbox} {unloadIndex + 1}");
                                    unloadIndex++;
                                }

                                this.Bridges_WithPauseFunc[ARECT40410A.GrabEnable].Set();
                                break;
                            }



                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step4]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }







            }


        }
    }

}
