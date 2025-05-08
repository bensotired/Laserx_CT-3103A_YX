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
        int bi = 0;

        public void LaserX_Step6_DryRun(CancellationTokenSource tokenSource)
        {
            int o = 0;

            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
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
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
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
                        }
                        break;
                }
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step5Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
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
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //var data_CT40410A = this._mainStreamData.GetCurrentOne_AndDequeueOutput();
                                        if (bi > 9)
                                        {
                                            bi = 0;
                                        }
                                        string[] bin = { "档位1", "档位2", "档位3", "档位4", "档位5", "档位6", "档位7", "档位8", "档位9", "档位10" };
                                        product_UnLoad = this.parameterInformation.AllUnLoadPos[(Gear)Enum.Parse(typeof(Gear), bin[bi])];
                                        CurrentBox = this.parameterInformation.gearBox[(Gear)Enum.Parse(typeof(Gear), bin[bi])];
                                        
                                        bi++;
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        //放料盘至产品放置位
                                        this.ReceivingTray_To_Dut_Product(CurrentBox);
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
                                                    if (tokenSource.IsCancellationRequested)
                                                    {
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
                                                    }
                                                    //dutPass_UnloadCount++;
                                                    this.parameterInformation.PositinIndex[CurrentBox]++;
                                                    this.Log_Global($"[Step6] [{CurrentBox}] 成功芯片总计:{this.parameterInformation.PositinIndex[CurrentBox]}个");


                                                    if (tokenSource.IsCancellationRequested)
                                                    {
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
                                                    }
                                                    if (this.parameterInformation.PositinIndex[CurrentBox] >= this.parameterInformation.ProductNumber[CurrentBox])
                                                    {
                                                        //此处需要换下一料盒
                                                        this.ReceivingTray_To_Zero();
                                                        //需要暂停
                                                        this.UserRequest_Pause(MT.下料模组);
                                                        Form_ChangeBox_Out form_ChangeBox_Out = new Form_ChangeBox_Out();
                                                        form_ChangeBox_Out.feedBox_Out = CurrentBox;
                                                        form_ChangeBox_Out.OutInfor = this.parameterInformation.UnLoadScann[CurrentBox];
                                                        form_ChangeBox_Out.ConnectToAppInteration(this);
                                                        form_ChangeBox_Out.ConnectToCore(this._coreInteration);
                                                        form_ChangeBox_Out.ShowDialog();
                                                        //回复暂停
                                                        this.UserRequest_Resume(MT.下料模组);
                                                        if (form_ChangeBox_Out.DialogResult == DialogResult.OK)
                                                        {
                                                            this.parameterInformation.PositinIndex[CurrentBox] = this.Calindex(CurrentBox, form_ChangeBox_Out.hang, form_ChangeBox_Out.lie);
                                                            isGrab = this.parameterInformation.PositinIndex[CurrentBox] + 1 < this.parameterInformation.ProductNumber[CurrentBox];
                                                            this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep3].Set();
                                                            this.Bridges_WithPauseFunc[ARECT40410A.BlueNeedAddTec_TellStep5].Set();
                                                        }
                                                        if (form_ChangeBox_Out.DialogResult == DialogResult.Yes)
                                                        {
                                                            this.parameterInformation.PositinIndex[CurrentBox] = this.Calindex(CurrentBox, form_ChangeBox_Out.hang, form_ChangeBox_Out.lie);
                                                            isGrab = this.parameterInformation.PositinIndex[CurrentBox] + 1 < this.parameterInformation.ProductNumber[CurrentBox];
                                                            do
                                                            {
                                                                bool SKIP = false;
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
                                                                //    ScannerResult_2 = this.LocalResource.scannerES_2.Scanning();

                                                                //    if (!string.IsNullOrEmpty(ScannerResult_2))
                                                                //    {
                                                                //        this.DelScann(CurrentBox);
                                                                //        this.parameterInformation.UnLoadScann.Add(CurrentBox, ScannerResult_2);
                                                                //        this.Log_Global("[Step6]线程6已经扫描二维码并且已解析成功");

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
                                                                //            this.parameterInformation.UnLoadScann.Add(CurrentBox, form_Name.name);
                                                                //            //输入成功
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
                                                                //            tokenSource.Cancel();
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
                                                                if (tokenSource.IsCancellationRequested)
                                                                {
                                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                                    return;
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
