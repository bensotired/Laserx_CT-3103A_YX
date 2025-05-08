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
using LX_BurnInSolution.Utilities;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT40410A
    {
        List<XYCoord> product_UnLoad = new List<XYCoord>();//下料矩阵


        FeedBox_Out CurrentBox = FeedBox_Out.料盒_1;
        //int productCount = 0;


        bool isGrab = true;


        public void LaserX_Step6_UnLoadProduct(CancellationTokenSource tokenSource)
        {
            int o = 0;


            this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].Set();

            do
            {
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                switch (this.Bridges_WithPauseFunc[ARECT40410A.UserCancelBlueAddTec_TellStep6].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            //下料盘至零点位
                            this.ReceivingTray_To_Zero();
                            this.Bridges_WithPauseFunc[ARECT40410A.Step6Finish].Set();
                            this.Log_Global("[Step6] Step6Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step6]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }
                        break;
                }
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step5Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            //下料盘至零点位
                            this.ReceivingTray_To_Zero();
                            this.Bridges_WithPauseFunc[ARECT40410A.Step6Finish].Set();
                            this.Log_Global("[Step6] Step6Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step6]用户取消运行");
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
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_RunToUnloadPlace].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        var data_CT40410A = this._mainStreamData.GetCurrentOne_Station_1();
                                        product_UnLoad = this.parameterInformation.AllUnLoadPos[(Gear)Enum.Parse(typeof(Gear), data_CT40410A.BIN)];
                                        CurrentBox = this.parameterInformation.gearBox[(Gear)Enum.Parse(typeof(Gear), data_CT40410A.BIN)];


                                        //放料盘至产品放置位
                                        this.ReceivingTray_To_Dut_Product(CurrentBox);
                                        //左吸嘴下料位下料
                                        this.Nozzle_Left_UnloadPlace_Down_Unload_UP(tokenSource);
                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_CanUnloadDut].Set();
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_UnloadDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                        {
                                            case EventResult.SUCCEED:
                                                {
                                                    //dutPass_UnloadCount++;
                                                    this.parameterInformation.PositinIndex[CurrentBox]++;
                                                    this.Log_Global($"[Step6] [{CurrentBox}] 成功芯片总计:{this.parameterInformation.PositinIndex[CurrentBox]}个");

                                                    //if (dutPass_UnloadCount >= productCount)
                                                    //{
                                                    //    //此处下料盘已满
                                                    //    isFull = true;
                                                    //    break;
                                                    //}

                                                    //isGrab = this.parameterInformation.PositinIndex[CurrentBox]  < this.parameterInformation.ProductNumber[CurrentBox];
                                                    if (this.parameterInformation.PositinIndex[CurrentBox] >= this.parameterInformation.ProductNumber[CurrentBox])
                                                    {
                                                        //此处需要换下一料盒
                                                        //dutPass_UnloadCount = 0;
                                                        this.ReceivingTray_To_Zero();
                                                        //需要暂停
                                                        //this.UserRequest_MasterControl_Pause();//全线程
                                                        this.UserRequest_Pause(MT.下料模组);
                                                        Form_ChangeBox_Out form_ChangeBox_Out = new Form_ChangeBox_Out();
                                                        form_ChangeBox_Out.feedBox_Out = CurrentBox;
                                                        form_ChangeBox_Out.OutInfor = data_CT40410A.OutPutScann;
                                                        //form_ChangeBox_Out.OutputSettings = this.providerResourse.OutPutSettings_Provider.OutputSettings;
                                                        form_ChangeBox_Out.ConnectToAppInteration(this);
                                                        form_ChangeBox_Out.ConnectToCore(this._coreInteration);
                                                        form_ChangeBox_Out.ShowDialog();
                                                        //回复暂停
                                                        //this.UserRequest_MasterControl_Resume();//全线程
                                                        this.UserRequest_Resume(MT.下料模组);
                                                        if (form_ChangeBox_Out.DialogResult == DialogResult.OK)
                                                        {
                                                            this.parameterInformation.PositinIndex[CurrentBox] = this.Calindex(CurrentBox, form_ChangeBox_Out.hang, form_ChangeBox_Out.lie);
                                                            this.parameterInformation.UnLoadScann[CurrentBox] = CurrentBox.ToString() + "_" + BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now);
                                                            //isGrab = this.parameterInformation.PositinIndex[CurrentBox] + 1 < this.parameterInformation.ProductNumber[CurrentBox];
                                                            this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].Set();
                                                            this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep5].Set();
                                                        }
                                                        if (form_ChangeBox_Out.DialogResult == DialogResult.Yes)
                                                        {
                                                            this.parameterInformation.PositinIndex[CurrentBox] = this.Calindex(CurrentBox, form_ChangeBox_Out.hang, form_ChangeBox_Out.lie);
                                                            //isGrab = this.parameterInformation.PositinIndex[CurrentBox] + 1 < this.parameterInformation.ProductNumber[CurrentBox];
                                                            do
                                                            {
                                                                bool SKIP = false;
                                                                //this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Left_NeedRuntoScannerPlace].Set();
                                                                ////this.ReceivingTray_To_ScannerPlace();
                                                                //switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Left_Run_To_ScannerPlace_Done].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                                                //{
                                                                //    case EventResult.SUCCEED:
                                                                //        {
                                                                //            do
                                                                //            {
                                                                OutputSettingsItem OutputSettings = this.providerResourse.OutPutSettings_Provider.OutputSettings;
                                                                var _output = OutputSettings.OutFeed[CurrentBox];
                                                                XYCoord xYCoord = new XYCoord();
                                                                //xYCoord.X = _output[ProductPosition.扫码_X];
                                                                //xYCoord.Y = _output[ProductPosition.扫码_Y];

                                                                this.MoveToLeftScannPosition(xYCoord);
                                                                bool skip = false;
                                                                string ScannerResult_2 = string.Empty;
                                                                int scannercount = 0;
                                                                //do
                                                                //{
                                                                //    if (tokenSource.IsCancellationRequested)
                                                                //    {
                                                                //        tokenSource.Token.ThrowIfCancellationRequested();
                                                                //        return;
                                                                //    }
                                                                //    ScannerResult_2 = this.LocalResource.scannerES_2.Scanning();

                                                                //    if (!string.IsNullOrEmpty(ScannerResult_2))
                                                                //    {
                                                                //        this.DelScann(CurrentBox);
                                                                //        this.parameterInformation.UnLoadScann.Add(CurrentBox, ScannerResult_2);
                                                                //        this.Log_Global("[Step6]线程6已经扫描二维码并且已解析成功");

                                                                //        //this.Bridges_WithPauseFunc[ARECT40410A.ScannerFinish].Set();
                                                                //        this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].Set();
                                                                //        this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep5].Set();
                                                                //        skip = true;
                                                                //        SKIP = true;
                                                                //        break;


                                                                //    }
                                                                //    if (scannercount >= 3)
                                                                //    {
                                                                //        //扫描不到二维码，手动输入
                                                                //        this.UserRequest_Pause(MT.下料模组);
                                                                //        Form_Name form_Name = new Form_Name();
                                                                //        form_Name.ShowDialog();
                                                                //        //创建UI来手动输入二维码信息
                                                                //        this.UserRequest_Resume(MT.下料模组);
                                                                //        if (form_Name.DialogResult == DialogResult.OK)
                                                                //        {
                                                                //            //传递参数
                                                                //            this.DelScann(CurrentBox);
                                                                //            if (string.IsNullOrEmpty(form_Name.name))
                                                                //            {
                                                                //                this.parameterInformation.UnLoadScann.Add(CurrentBox, $"Dome_{CurrentBox}");
                                                                //            }
                                                                //            else
                                                                //            {
                                                                //                this.parameterInformation.UnLoadScann.Add(CurrentBox, form_Name.name);
                                                                //            }
                                                                            
                                                                //            //输入成功
                                                                //            //this.Bridges_WithPauseFunc[ARECT40410A.ScannerFinish].Set();
                                                                //            this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].Set();
                                                                //            this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep5].Set();
                                                                //            this.Log_Global("[Step6]线程6用户采用手输形式进行对二维码信息的录入");
                                                                //            skip = true;
                                                                //            SKIP = true;
                                                                //            break;
                                                                //        }
                                                                //        if (form_Name.DialogResult == DialogResult.Cancel)
                                                                //        {
                                                                //            //取消运行
                                                                //            this.Log_Global("[Step6]用户取消运行");
                                                                //            tokenSource.Token.ThrowIfCancellationRequested();
                                                                //            return;
                                                                //        }
                                                                //        if (form_Name.DialogResult == DialogResult.None)
                                                                //        {
                                                                //            //重新扫描
                                                                //            continue;
                                                                //        }

                                                                //    }
                                                                //    scannercount++;
                                                                //} while (true);
                                                                if (skip)
                                                                {
                                                                    break;
                                                                }

                                                            } while (true);

                                                        }
                                                        if (form_ChangeBox_Out.DialogResult == DialogResult.Cancel)
                                                        {
                                                            userContiUnloadTec = false;
                                                            this.Bridges_WithPauseFunc[ARECT40410A.UserCancelBlueAddTec_TellStep3].Set();
                                                            this.Bridges_WithPauseFunc[ARECT40410A.UserCancelBlueAddTec_TellStep5].Set();
                                                            this.Bridges_WithPauseFunc[ARECT40410A.UserCancelBlueAddTec_TellStep6].Set();

                                                            this.Log_Global("Step6 用户取消继续放料");
                                                            return;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep5].Set();
                                                    }

                                                }
                                                break;
                                            case EventResult.CANCEL:
                                                {
                                                    this.Log_Global("[Step6]用户取消运行");
                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                    return;
                                                }
                                                break;
                                            case EventResult.TIMEOUT:
                                                {
                                                    if (o == 0)
                                                    {
                                                        this.parameterInformation.PositinIndex[CurrentBox]++;
                                                        this.Log_Global($"[Step6] [{CurrentBox}] 成功芯片总计:{this.parameterInformation.PositinIndex[CurrentBox]}个   TimeOut");
                                                        isGrab = this.parameterInformation.PositinIndex[CurrentBox] < this.parameterInformation.ProductNumber[CurrentBox];
                                                        o++;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step6]用户取消运行");
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
        /// 收料盘到原点位
        /// </summary>
        public void ReceivingTray_To_Zero()
        {
            this.Log_Global("[Step6]下料Y运行到零点位");
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LY_收料盘_原点位_Y, SequenceOrder.Normal);
        }
        /// <summary>
        /// 放料盘至产品放置位
        /// </summary>
        public void ReceivingTray_To_Dut_Product(FeedBox_Out box_Out)
        {
            this.Log_Global("[Step6]左吸嘴X到达下料位,下料Y运行到产品放置位");
            int index = this.parameterInformation.PositinIndex[box_Out];
            Parallel.Invoke(
                () =>
                {
                    //左吸嘴X到达下料位
                    this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X], product_UnLoad[index].X);
                },
                () =>
                {
                    this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.LY_收料盘_Y].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.LY_收料盘_Y], product_UnLoad[index].Y);
                });

        }
        /// <summary>
        /// 放料盘到扫码位置
        /// </summary>
        //public void ReceivingTray_To_ScannerPlace()
        //{
        //    this.Log_Global("[Step6]下料Y运行到扫码位");
        //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LY_左侧扫码位_Y, SequenceOrder.Normal);
        //}
        /// <summary>
        /// 放料盘到扫码位置
        /// </summary>
        public void MoveToLeftScannPosition(XYCoord product_Load)
        {
            double x = product_Load.X;
            double y = product_Load.Y;
            Parallel.Invoke(
                () =>
                {
                    this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.LSX_左侧单元吸嘴_X], x);
                },
                () =>
                {
                    this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.LY_收料盘_Y].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.LY_收料盘_Y], y);
                });
        }


        public void DelScann(FeedBox_Out feed)
        {
            int index = 0;
            List<string> vs = new List<string>();
            string SN = feed.ToString();
            foreach (var item in this.parameterInformation.UnLoadScann)
            {
                vs.Add(item.Key.ToString());
            }
            for (int i = 0; i < vs.Count; i++)
            {
                if (SN == vs[i].ToString())
                {
                    index = i;
                }
            }
            this.parameterInformation.UnLoadScann.RemoveAtIndex(index);
        }
    }

}
