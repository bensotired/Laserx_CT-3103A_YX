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
            userContiUnloadTec = true;


            product_Load = new List<XYCoord>();
            On_N_R_Tray = new List<TestProduct>();
            searchProducts = new List<SearchProduct>();
            do
            {

                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                WaitMessageChooseRestart(tokenSource);

                int box_In = this.parameterInformation.numberBox - 1;//上料盘料盒索引
                for (int i = box_In; i < this.parameterInformation.feedBoxes.Count; i++)
                {
                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    WaitMessageChooseRestart(tokenSource);
                    skip_in = this.parameterInformation.skip[i];
                    if (skip_in)
                    {
                        this.Log_Global($"Step1 跳过{this.parameterInformation.feedBoxes[i]}");
                        continue;
                    }
                    var _InputSettings = this.providerResourse.InPutSettings_Provider._InPutSettings;
                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    WaitMessageChooseRestart(tokenSource);
                    var inputstting = _InputSettings.Feed[this.parameterInformation.feedBoxes[i]];
                    //XYCoord xY = new XYCoord();
                    //xY.X = inputstting[ProductPosition.扫码_X];
                    //xY.Y = inputstting[ProductPosition.扫码_Y];
                    //WaitMessageChoose(tokenSource);
                    //this.MoveToSearchTECPosition(xY, tokenSource);
                    //WaitMessageChoose(tokenSource);
                    //string ScannerResult_1 = string.Empty;
                    //int scannercount = 0;

                    //do
                    //{
                    //    WaitMessageChoose(tokenSource);
                    //    ScannerResult_1 = this.LocalResource.scannerES_1.Scanning();

                    //    if (!string.IsNullOrEmpty(ScannerResult_1))
                    //    {
                    //        this.LoadScann = ScannerResult_1;
                    //        break;
                    //    }
                    //    if (scannercount >= 3)
                    //    {
                    //        //扫描不到二维码，手动输入
                    //        //创建UI来手动输入
                    //        this.SetWaitStep1();
                    //        Form_Name form_Name = new Form_Name();
                    //        form_Name.ShowDialog();
                    //        this.SetRestartStep1();
                    //        if (form_Name.DialogResult == DialogResult.Cancel)
                    //        {
                    //            //取消运行
                    //            this.Log_Global("[Step1]用户取消运行");
                    //            tokenSource.Token.ThrowIfCancellationRequested();
                    //        }
                    //        if (form_Name.DialogResult == DialogResult.OK)
                    //        {
                    //            //传递参数
                    //            //确认输入
                    //            this.LoadScann = form_Name.name;
                    //            break;
                    //        }
                    //        if (form_Name.DialogResult == DialogResult.None)
                    //        {
                    //            //重新扫
                    //            continue;
                    //        }
                    //    }
                    //    scannercount++;
                    //} while (true);

                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    WaitMessageChooseRestart(tokenSource);
                    this.Log_Global($"Step1 扫描{this.parameterInformation.feedBoxes[i]}");
                    product_Load = this.CreatePointList(inputstting[ProductPosition.左上角坐标_X], inputstting[ProductPosition.左上角坐标_Y],
                        inputstting[ProductPosition.右下角坐标_X], inputstting[ProductPosition.右下角坐标_Y],
                        inputstting[ProductPosition.行数], inputstting[ProductPosition.列数]);
                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    WaitMessageChooseRestart(tokenSource);
                    int StartRow_In = (int)inputstting[ProductPosition.起始行];
                    int StartColumn_In = (int)inputstting[ProductPosition.起始列];
                    int row = (int)inputstting[ProductPosition.行数];
                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    WaitMessageChooseRestart(tokenSource);
                    int searchIndex = (StartRow_In - 1) * row + StartColumn_In - 1;//上料起始扫描坐标索引
                    do
                    {
                        if (tokenSource.IsCancellationRequested)
                        {
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }
                        WaitMessageChooseRestart(tokenSource);
                        if (userContiUnloadTec)
                        {
                            if (searchIndex < product_Load.Count && product_Load != null && product_Load.Count > 0)
                            {
                                bool isSkipProduct = false;
                                if (!userContiUnloadTec)
                                {
                                    continue;
                                }

                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessageChooseRestart(tokenSource);
                                this.MoveToSearchTECPosition(product_Load[searchIndex], tokenSource);
                                searchIndex++;
                                if (searchIndex>= product_Load.Count)
                                {
                                    break;
                                }
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessageChooseRestart(tokenSource);
                                SearchProduct search = new SearchProduct();
                                search.PosX = this.LocalResource.Axes[AxisNameEnum_CT40410A.RX_进料_X].Get_CurUnitPos();
                                search.PosY = this.LocalResource.Axes[AxisNameEnum_CT40410A.RY_进料_Y].Get_CurUnitPos();
                                searchProducts.Add(search);

                            }
                        }
                        else if (!userContiUnloadTec && On_N_R_Tray.Count == 0)
                        {
                            this.Log_Global($"[Step1]On_N_R_Tray_1 :{On_N_R_Tray.Count}");
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessageChooseRestart(tokenSource);
                            RX_RY_Tray_Origin(tokenSource);
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessageChooseRestart(tokenSource);
                            //直接停止流转各个线程
                            this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].Set();
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessageChooseRestart(tokenSource);
                            this.Log_Global("Step1Finish");
                        }
                        else if (!userContiUnloadTec && On_N_R_Tray.Count > 0)
                        {
                            //this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource);
                            this.Log_Global($"[Step1]On_N_R_Tray_2 :{On_N_R_Tray.Count}");
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessageChooseRestart(tokenSource);
                        }

                        do
                        {
                            bool sk = false;
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            WaitMessageChooseRestart(tokenSource);
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.DeveiceEmptyAllowNozzle_Right_TakeUpDut].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessageChooseRestart(tokenSource);
                                        this.Log_Global("[Step1]线程3允许线程1运行到抓料的位置，已经进入方法了");
                                        this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_CanTakeDut].Set();
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessageChooseRestart(tokenSource);
                                        while (true)
                                        {
                                            bool skip = false;
                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_TakeDutFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                            {
                                                case EventResult.SUCCEED:
                                                    {
                                                        skip = true;
                                                        this.Log_Global("[Step1]线程1已经抓料上去了 可以开始下次匹配了");
                                                        sk = true;
                                                    }
                                                    break;
                                                case EventResult.CANCEL:
                                                    {
                                                        this.Log_Global("[Step1]用户取消操作");
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                    }
                                                    break;
                                                case EventResult.TIMEOUT:
                                                    break;
                                            }
                                            if (skip)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step1]用户取消操作");
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
                            //倒一
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_NeedRuntoDescFirstPlace].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessageChooseRestart(tokenSource);
                                        RuntoReleaseTecPositionDescFirst(On_N_R_Tray[0], tokenSource);
                                        this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_FinishRuntoDescFirstPlace].Set();
                                        while (true)
                                        {
                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_CanFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                            {
                                                case EventResult.SUCCEED:
                                                    {
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        WaitMessageChooseRestart(tokenSource);
                                                        this.Log_Global("倒一");
                                                        RX_RY_Tray_Origin(tokenSource);
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        WaitMessageChooseRestart(tokenSource);
                                                        this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].Set();
                                                        return;
                                                    }
                                                case EventResult.CANCEL:
                                                    {
                                                        this.Log_Global("[Step1]用户取消操作");
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                    }
                                                    break;
                                            }

                                        }
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step1]用户取消操作");
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                    }
                                    break;
                            }
                            //倒二
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_NeedRuntoDescSecondPlace].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessageChooseRestart(tokenSource);
                                        RuntoReleaseTecPositionDescFirst(On_N_R_Tray[0], tokenSource);
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        WaitMessageChooseRestart(tokenSource);
                                        this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_FinishRuntoDescSecondPlace].Set();
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step1]用户取消操作");
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                    }
                                    break;
                                case EventResult.TIMEOUT:
                                    break;
                            }





                        } while (true);



                    } while (true);





                }
                //是否扫描下一
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                WaitMessageChooseRestart(tokenSource);
                this.RXRY_Origin(tokenSource);
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                WaitMessageChooseRestart(tokenSource);
                //需要暂停
                //this.UserRequest_Pause(MT.上料模组);
                this.SetWaitStep1();
                Form_ChangeBox form_ChangeBox = new Form_ChangeBox();
                form_ChangeBox.ConnectToAppInteration(this);
                form_ChangeBox.ConnectToCore(this._coreInteration);
                form_ChangeBox.ShowDialog();
                //回复暂停
                //this.UserRequest_Resume(MT.上料模组);
                this.SetRestartStep1();
                if (form_ChangeBox.DialogResult == DialogResult.OK)
                {
                }
                if (form_ChangeBox.DialogResult == DialogResult.Cancel)
                {
                    this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].Set();
                    this.Log_Global("Step1Finish  用户取消继续放料");
                    return;
                }

            } while (true);
        }
    }
   
}
