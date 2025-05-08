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
        public ParameterInformation parameterInformation = new ParameterInformation();//运行中料盘参数
        List<XYCoord> product_Load = new List<XYCoord>();//上料矩阵
        List<TestProduct> On_N_R_Tray = new List<TestProduct>();
        List<TestProduct> _step4 = new List<TestProduct>();
        List<SearchProduct> searchProducts = null; //上料寻找到的芯片的位置
        List<XYCoord> Cm2 = new List<XYCoord>();//相机2补偿


        string loadbox = string.Empty;
        List<int> index = new List<int>();
        //string LoadScann = string.Empty;
        bool skip_in = false;//跳过本次
        int searchIndex = 0;//扫描索引
                            //int dutPass_UnloadCount = 0;//TEC成功下料 可以设置现在有多少个已经执行完成的料了

        int delayTime = 50;//延时
        int dutLoad = 0;//吸上去的料的索引
        public volatile bool userContiUnloadTec = true;//用户继续放料?
        public void LaserX_Step1_SearchProduct(CancellationTokenSource tokenSource)
        {

            userContiUnloadTec = true;


            product_Load = new List<XYCoord>();
            On_N_R_Tray = new List<TestProduct>();


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
                    #region 扫码
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
                    //    if (scannercount >= 1)
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
                    //            tokenSource.Cancel();
                    //            return;
                    //        }
                    //        if (form_Name.DialogResult == DialogResult.OK)
                    //        {
                    //            //传递参数
                    //            //确认输入
                    //            if (string.IsNullOrEmpty(form_Name.name))
                    //            {
                    //                this.LoadScann = this.parameterInformation.feedBoxes[i].ToString();
                    //                break;
                    //            }
                    //            else
                    //            {
                    //                this.LoadScann = form_Name.name;
                    //                break;
                    //            }

                    //        }
                    //        if (form_Name.DialogResult == DialogResult.None)
                    //        {
                    //            //重新扫
                    //            continue;
                    //        }
                    //    }
                    //    scannercount++;
                    //} while (true);
                    #endregion
                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    WaitMessageChooseRestart(tokenSource);
                    this.Log_Global($"Step1 扫描{this.parameterInformation.feedBoxes[i]}");
                    loadbox = this.parameterInformation.feedBoxes[i].ToString();
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
                    int colum = (int)inputstting[ProductPosition.列数];
                    if (tokenSource.IsCancellationRequested)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    WaitMessageChooseRestart(tokenSource);
                    searchIndex = (StartRow_In - 1) * colum + StartColumn_In - 1;//上料起始扫描坐标索引
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
                                if (tokenSource.IsCancellationRequested)
                                {
                                    tokenSource.Token.ThrowIfCancellationRequested();
                                    return;
                                }
                                WaitMessageChooseRestart(tokenSource);

                                VisionResult_LaserX_Image_Universal visionMatch1 = null;
                                do
                                {
                                    
                                    WaitMessageChooseRestart(tokenSource);
                                    this.providerResourse.LoadProduct_VisionComboCommand_Config();
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessageChooseRestart(tokenSource);
                                    var cmdList = this.providerResourse.VisionComboCommand_Provider[VisionCMD_CT40410A.相机1_产品_搜索];
                                    if (tokenSource.IsCancellationRequested)
                                    {
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    WaitMessageChooseRestart(tokenSource);
                                    foreach (var item in cmdList)
                                    {
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        int count = 0;
                                        while (true)
                                        {
                                            if (tokenSource.IsCancellationRequested)
                                            {
                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                return;
                                            }
                                            count++;
                                            if (count > 3 )
                                            {
                                                visionMatch1.Success = false;
                                                break;
                                            }
                                            if (tokenSource.IsCancellationRequested)
                                            {
                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                return;
                                            }
                                            WaitMessageChooseRestart(tokenSource);
                                            visionMatch1 = this.LocalResource.VisionController.GetVisionResult_Universal(item, delayTime);
                                            if (visionMatch1.Success)
                                            {
                                                var Xshaft1 = this.LocalResource.Axes[AxisNameEnum_CT40410A.RX_进料_X].Get_CurUnitPos();
                                                var Yshaft1 = this.LocalResource.Axes[AxisNameEnum_CT40410A.RY_进料_Y].Get_CurUnitPos();
                                                var Xppm1 = this.providerResourse.PPM_Provider[PPM_Enum_CT40410A.相机1_PPM].X_ppm;
                                                var Yppm1 = this.providerResourse.PPM_Provider[PPM_Enum_CT40410A.相机1_PPM].Y_ppm;
                                                if (tokenSource.IsCancellationRequested)
                                                {
                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                    return;
                                                }
                                                WaitMessageChooseRestart(tokenSource);
                                                XYCoord coord1 = this.CalTargetPosition(Xshaft1, Yshaft1, Xppm1, Yppm1, visionMatch1.ImageWidth / 2, visionMatch1.ImageHeight / 2,
                                                                                        visionMatch1.PeekCenterX_Pix, visionMatch1.PeekCenterY_Pix);
                                                if (tokenSource.IsCancellationRequested)
                                                {
                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                    return;
                                                }
                                                WaitMessageChooseRestart(tokenSource);
                                                this.MoveToSearchTECPosition(coord1, tokenSource);
                                                if (tokenSource.IsCancellationRequested)
                                                {
                                                    tokenSource.Token.ThrowIfCancellationRequested();
                                                    return;
                                                }
                                                WaitMessageChooseRestart(tokenSource);
                                                visionMatch1 = this.LocalResource.VisionController.GetVisionResult_Universal(item, delayTime);
                                                if (Math.Abs(visionMatch1.PeekCenterX_Pix - visionMatch1.ImageWidth / 2) < this.providerResourse.RunSettings_Provider._RunParamSettings.相机1允许像素误差_X &&
                                                    Math.Abs(visionMatch1.PeekCenterY_Pix - visionMatch1.ImageHeight / 2) < this.providerResourse.RunSettings_Provider._RunParamSettings.相机1允许像素误差_Y)
                                                {
                                                    var Xshaft = this.LocalResource.Axes[AxisNameEnum_CT40410A.RX_进料_X].Get_CurUnitPos();
                                                    var Yshaft = this.LocalResource.Axes[AxisNameEnum_CT40410A.RY_进料_Y].Get_CurUnitPos();
                                                    var Xppm = this.providerResourse.PPM_Provider[PPM_Enum_CT40410A.相机1_PPM].X_ppm;
                                                    var Yppm = this.providerResourse.PPM_Provider[PPM_Enum_CT40410A.相机1_PPM].Y_ppm;
                                                    if (tokenSource.IsCancellationRequested)
                                                    {
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
                                                    }
                                                    WaitMessageChooseRestart(tokenSource);
                                                    XYCoord coord = this.CalTargetPosition(Xshaft, Yshaft, Xppm, Yppm, visionMatch1.ImageWidth / 2, visionMatch1.ImageHeight / 2,
                                                                                            visionMatch1.PeekCenterX_Pix, visionMatch1.PeekCenterY_Pix);
                                                    if (tokenSource.IsCancellationRequested)
                                                    {
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
                                                    }
                                                    WaitMessageChooseRestart(tokenSource);
                                                    this.MoveToSearchTECPosition(coord, tokenSource);
                                                    if (tokenSource.IsCancellationRequested)
                                                    {
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
                                                    }
                                                    WaitMessageChooseRestart(tokenSource);
                                                    SearchProduct search = new SearchProduct();
                                                    search.PosX = Xshaft;
                                                    search.PosY = Yshaft;
                                                    searchProducts = new List<SearchProduct>();
                                                    searchProducts.Add(search);
                                                    searchIndex++;
                                                    index.Add(searchIndex);
                                                    break;
                                                }

                                            }
                                            Thread.Sleep(20);
                                        }
                                        if (visionMatch1.Success)
                                        {
                                            break;
                                        }
                                    }
                                    if (visionMatch1.Success)
                                    {
                                        break;
                                    }
                                    //所有模板失败，新增模板，需暂停
                                    if (!visionMatch1.Success)
                                    {
                                        isSkipProduct = true;
                                        searchIndex++;
                                        break;


                                        ////需要暂停
                                        ////this.UserRequest_Pause(MT.上料模组);//全线程
                                        //this.SetWaitStep1();
                                        //Frm_ExHandle_HighRes_Cam frm_Camera1 = new Frm_ExHandle_HighRes_Cam();
                                        //frm_Camera1.cmdList = cmdList;
                                        //frm_Camera1.ConnectToAppInteration(this);
                                        //frm_Camera1.ConnectToCore(this._coreInteration);
                                        //frm_Camera1.ShowDialog();
                                        ////回复暂停
                                        ////this.UserRequest_Resume(MT.上料模组);//全线程
                                        //this.SetRestartStep1();
                                        //if (frm_Camera1.DialogResult == DialogResult.OK)
                                        //{
                                        //    //GoOn
                                        //}
                                        //if (frm_Camera1.DialogResult == DialogResult.No)
                                        //{
                                        //    isSkipProduct = true;
                                        //    searchIndex++;
                                        //    break;
                                        //}
                                        //if (frm_Camera1.DialogResult == DialogResult.Cancel)
                                        //{
                                        //    this.Log_Global("Step1Finish  用户取消继续搜索");
                                        //    tokenSource.Cancel();
                                        //return;
                                        //}
                                    }


                                } while (true);
                                if (isSkipProduct)
                                {
                                    continue;
                                }

                            }
                            if (searchIndex >= product_Load.Count)
                            {
                                break;
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
                                            bool skip = false;
                                            switch (this.Bridges_WithPauseFunc[ARECT40410A.Nozzle_Right_TakeDutFinish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                                            {
                                                case EventResult.SUCCEED:
                                                    {
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        skip = true;
                                                        this.Log_Global("[Step1]线程1已经抓料上去了 可以开始下次匹配了");
                                                        this.MoveToSearchTECPosition(product_Load[searchIndex-1], tokenSource);
                                                        var cmdList = this.providerResourse.VisionComboCommand_Provider[VisionCMD_CT40410A.相机1_产品_搜索];
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        WaitMessageChooseRestart(tokenSource);
                                                        foreach (var item in cmdList)
                                                        {
                                                            if (tokenSource.IsCancellationRequested)
                                                            {
                                                                tokenSource.Token.ThrowIfCancellationRequested();
                                                                return;
                                                            }
                                                            VisionResult_LaserX_Image_Universal visionMatch3 = this.LocalResource.VisionController.GetVisionResult_Universal(item, delayTime);
                                                            if (visionMatch3.Success)
                                                            {
                                                                if (searchIndex > 0)
                                                                {
                                                                    searchIndex--;
                                                                }
                                                            }
                                                        }
                                                            
                                                            sk = true;
                                                    }
                                                    break;
                                                case EventResult.CANCEL:
                                                    {
                                                        this.Log_Global("[Step1]用户取消操作");
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
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
                                                        if (tokenSource.IsCancellationRequested)
                                                        {
                                                            tokenSource.Token.ThrowIfCancellationRequested();
                                                            return;
                                                        }
                                                        this.Bridges_WithPauseFunc[ARECT40410A.Step1Finish].Set();
                                                        return;
                                                    }
                                                case EventResult.CANCEL:
                                                    {
                                                        this.Log_Global("[Step1]用户取消操作");
                                                        tokenSource.Token.ThrowIfCancellationRequested();
                                                        return;
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
                                        return;
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
                                        if (tokenSource.IsCancellationRequested)
                                        {
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                        this.Bridges_WithPauseFunc[ARECT40410A.Signal_Nozzel_Right_FinishRuntoDescSecondPlace].Set();
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step1]用户取消操作");
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
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

        /// <summary>
        /// 进料盘扫料点位
        /// </summary>
        /// <param name="product_Load"></param>
        public void MoveToSearchTECPosition(XYCoord product_Load, CancellationTokenSource tokenSource)//运行到LoadTEC的位置
        {
            double x = product_Load.X;
            double y = product_Load.Y;
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
            Parallel.Invoke(
                () =>
                {
                    this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RX_进料_X].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RX_进料_X], x);
                },
                () =>
                {
                    this.LocalResource.AxesMotionAction[AxisNameEnum_CT40410A.RY_进料_Y].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT40410A.RY_进料_Y], y);
                });
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
        }
        /// <summary>
        /// 进料盘、收料盘原点位
        /// </summary>
        public void RX_RY_Tray_Origin(CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
            Parallel.Invoke(
                () =>
                {
                    this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT40410A.RXY_上料盘原点位_XY);
                },
                () =>
                {
                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LY_收料盘_原点位_Y, SequenceOrder.Normal);
                });
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
        }
        /// <summary>
        /// 进料盘原点位
        /// </summary>
        public void RXRY_Origin(CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
            this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT40410A.RXY_上料盘原点位_XY);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
        }
        /// <summary>
        /// 运行到放回上料盒的坐标
        /// </summary>
        /// <param name="tokenSource"></param>
        private void RuntoReleaseTecPositionDescFirst(TestProduct coordinate, CancellationTokenSource tokenSource)
        {
            XYCoord pointsUsing = new XYCoord();
            pointsUsing.X = coordinate.LoadposX;
            pointsUsing.Y = coordinate.LoadposY;
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
            this.MoveToSearchTECPosition(pointsUsing, tokenSource);
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
                return;
            }
            WaitMessageChooseRestart(tokenSource);
        }




    }

}
