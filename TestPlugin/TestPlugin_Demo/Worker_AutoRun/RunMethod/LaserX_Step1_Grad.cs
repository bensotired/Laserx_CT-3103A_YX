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
    public sealed partial class TestPluginWorker_CT3103
    {
        public ParameterInformation parameter = new ParameterInformation();
        public Status Status = new Status();

        List<double> TempListLeft = new List<double>();
        List<double> TempListRight = new List<double>();

        List<string> LeftChipNumberlist = new List<string>();
        List<string> RightChipNumberlist = new List<string>();

        string CarrierNumberLeft = string.Empty;
        string CarrierNumberRight = string.Empty;
        int CarrierCount = 0;
        int LeftTestCount = 0;
        int RightTestCount = 0;
        int UnLoadCarrier = 0;
        public void LaserX_Step1_Grad(CancellationTokenSource tokenSource)
        {
            this.Status.LoadAndUnLoad = Operation.Idle;
            this.Status.LeftStationTest = TestStatusOnBoard.未测试;
            this.Status.RightStationTest = TestStatusOnBoard.未测试;
            this.Status.TempControlStateLeft = Operation.Idle;
            this.Status.TempControlStateRight = Operation.Idle;
            this.Status.IsTesting = Operation.Idle;
            TempListLeft = new List<double>();
            TempListRight = new List<double>();
            LeftChipNumberlist = new List<string>();
            RightChipNumberlist = new List<string>();
            CarrierNumberLeft = string.Empty;
            CarrierNumberRight = string.Empty;
            CarrierCount = 0;
            LeftTestCount = 0;
            RightTestCount = 0;
            UnLoadCarrier = 0;


            while (true)
            {

                switch (this.Bridges_WithPauseFunc[Action3103.Step1Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Log_Global("[Step1]Step1Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step1]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }
                switch (this.Bridges_WithPauseFunc[Action3103.LoadAndUnLoad].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Status.LoadAndUnLoad = Operation.Occupancy;
                            if (this.Status.LeftStation)//左载台启用
                            {
                                if (this.Status.UnLoadLeftStation)//左载台允许卸料
                                {
                                    /*
                                     * 左载台LX轴运动到避让位
                                     * 判断右载台RX的位置是否影响左载台LY轴的移动
                                     * 
                                     * LY运动装卸点，LX、LZ运动到装卸点
                                     */
                                    this.Log_Global("[Step1]左载台开始卸料");
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);

                                    if (this.Status.IsTesting == Operation.Idle)//没有进行测试时另一载台需要避让
                                    {
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_测试, SequenceOrder.Normal);
                                    }

                                    //var pos = this.LocalResource.Axes[AxisNameEnum_CT3103.RX].Get_CurUnitPos();
                                    //var poslimit = this.LocalResource.Positions[AxesPositionEnum_CT3103.RX_避让界限].ItemCollection[0].Position;
                                    //if (pos <= poslimit)
                                    //{
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_装卸, SequenceOrder.Normal);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT3103.LXZ_装卸);

                                    //CheckIO_FixedJaw_Left_Up(tokenSource);

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_装卸, SequenceOrder.Normal);

                                    CheckIO_Tongs_Loosen(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Dn(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Tighten(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Vacuum_L(false, tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                    Parallel.Invoke(() =>
                                    {
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                    },
                                    () =>
                                    {
                                        //T_OUT_Z轴下降预定高度
                                        var pistion = this.LocalResource.Positions[AxesPositionEnum_CT3103.T_OUT_Z_最高位].ItemCollection[0].Position;
                                        //int number = this.parameter.CarrierNumber;
                                        var motion = pistion - UnLoadCarrier * 14.5;
                                        this.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_OUT_Z].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z], motion);
                                        //出料弹夹从最高点（只有一个夹具时抓手能够抓取和放置的高度为最高点）随夹具数量增加而下降，单个夹具理论高度为14.5
                                    });

                                    CheckIO_Tongs_Dn(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Loosen(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);


                                    //成功放置的夹具数量
                                    UnLoadCarrier++;
                                    if (UnLoadCarrier >= this.parameter.UnLoadCarrierNumber)//放置的夹具数量等于放置的最大数量时结束允许
                                    {
                                        //已无夹具，结束信号
                                        this.Bridges_WithPauseFunc[Action3103.EndRun].Set();
                                        break;
                                    }
                                    this.Log_Global("[Step1]左载台卸料完成");
                                    this.Status.UnLoadLeftStation = false;
                                    //卸料完成，允许上料
                                    this.Status.LoadLeftStation = true;
                                }
                                if (this.Status.LoadLeftStation)//左载台允许上料
                                {
                                    if (this.parameter.CarrierNumber > 0)//可用夹具数量大于0则允许上料，抓手抓取家具后自减
                                    {
                                        this.Log_Global("[Step1]左载台开始上料");
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                                        LeftTestCount = 0;  //20241107 上一个夹具归零一次 
                                        if (this.Status.IsTesting == Operation.Idle)//没有进行测试时另一载台需要避让
                                        {
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_测试, SequenceOrder.Normal);
                                        }
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_装卸, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT3103.LXZ_装卸);

                                        //CheckIO_FixedJaw_Left_Up(tokenSource);

                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);

                                        Parallel.Invoke(() =>
                                        {
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_进料, SequenceOrder.Normal);
                                        },
                                        () =>
                                        {
                                            var pistion = this.LocalResource.Positions[AxesPositionEnum_CT3103.T_IN_Z_最高位].ItemCollection[0].Position;
                                            int number = this.parameter.CarrierNumber - 1;
                                            var motion = pistion - number * 14.5;
                                            this.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_IN_Z].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z], motion);

                                        });

                                        CheckIO_Tongs_Loosen(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Dn(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Tighten(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Up(tokenSource);

                                        CarrierNumberLeft = this.parameter.CarrierNumberlist[CarrierCount];//左载台上夹具编号
                                        LeftChipNumberlist = this.GetCarrierChipNumber(this.parameter.CarrierNumberlist[CarrierCount]);//获取该夹具中所有产品编号
                                        bool P_Test = true;
                                        bool N_Test = true;
                                        bool PandNTest = DetectingPNTest(LeftChipNumberlist, out P_Test, out N_Test);//判断该夹具正反面是否都能测试


                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_装卸, SequenceOrder.Normal);

                                        CheckIO_Tongs_Dn(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Loosen(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Vacuum_L(true, tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        PandNDetection_L(PandNTest, P_Test, N_Test, tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Up(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        //CheckIO_FixedJaw_Left_Dn(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);

                                        Parallel.Invoke(() =>
                                        {
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LZ_待机, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_待机, SequenceOrder.Normal);
                                        },
                                        () =>
                                        {
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            //这个可以不用动
                                            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                        });

                                        this.Log_Global("[Step1]左载台上料完成");
                                        //LeftTestCount = 0;  //20240820 归零  20241101 邓远明让注释掉


                                        //获取界面设置的控制温度，最初是温度list，后改为单个温度，任然可以用list进行多温度的测试
                                        foreach (var item in this.parameter.TemperatureListLeft)
                                        {
                                            this.TempListLeft.Add(item);
                                        }



                                        CarrierCount++;//已抓取的夹具数量
                                        this.parameter.CarrierNumber--;//可用夹具数量

                                        if (PandNTest)
                                        {
                                            //夹具两面都要测试时
                                            this.Status.LeftStationTest = TestStatusOnBoard.未测试;
                                        }
                                        else
                                        {
                                            //夹具只需要单面测试时
                                            this.Status.LeftStationTest = TestStatusOnBoard.一次测试;
                                        }

                                        this.Status.LoadLeftStation = false;//右载台不允许上料
                                        //左载台控温信号
                                        this.Bridges_WithPauseFunc[Action3103.TemperatureControlLeft].Set();
                                    }
                                    else
                                    {
                                        this.Status.LeftStationTest = TestStatusOnBoard.未测试;
                                        Parallel.Invoke(() =>
                                        {
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LZ_待机, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_待机, SequenceOrder.Normal);
                                        });
                                    }
                                }
                            }
                            if (this.Status.RightStation)//右载台启用
                            {
                                if (this.Status.UnLoadRightStation)//右载台卸料
                                {
                                    /*
                                     * 右载台RX轴运动到避让位
                                     * 判断左载台LX的位置是否影响右载台RY轴的移动
                                     * 
                                     * RY运动装卸点，RX、RZ运动到装卸点
                                     */
                                    this.Log_Global("[Step1]右载台开始卸料");
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                  
                                    if (this.Status.IsTesting == Operation.Idle)
                                    {
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_测试, SequenceOrder.Normal);
                                    }
                                    //var pos = this.LocalResource.Axes[AxisNameEnum_CT3103.LX].Get_CurUnitPos();
                                    //var poslimit = this.LocalResource.Positions[AxesPositionEnum_CT3103.LX_避让界限].ItemCollection[0].Position;
                                    //if (pos <= poslimit)
                                    //{
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_装卸, SequenceOrder.Normal);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT3103.RXZ_装卸);

                                    //CheckIO_FixedJaw_Right_Up(tokenSource);

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_装卸, SequenceOrder.Normal);

                                    CheckIO_Tongs_Loosen(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Dn(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Tighten(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Vacuum_R(false, tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);

                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    Parallel.Invoke(() =>
                                    {
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                    },
                                   () =>
                                   {
                                       var pistion = this.LocalResource.Positions[AxesPositionEnum_CT3103.T_OUT_Z_最高位].ItemCollection[0].Position;
                                       //int number = this.parameter.CarrierNumber;
                                       var motion = pistion - UnLoadCarrier * 14.5;
                                       this.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_OUT_Z].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT3103.T_OUT_Z], motion);

                                   });

                                    CheckIO_Tongs_Dn(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Loosen(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);
                                    CheckIO_Tongs_Up(tokenSource);
                                    if (tokenSource.IsCancellationRequested) return;
                                    WaitMessage(tokenSource);

                                    //T_OUT_Z轴下降预定高度

                                    UnLoadCarrier++;
                                    if (UnLoadCarrier >= this.parameter.UnLoadCarrierNumber)
                                    {
                                        //已无夹具，结束信号
                                        this.Bridges_WithPauseFunc[Action3103.EndRun].Set();
                                        break;
                                    }
                                    this.Log_Global("[Step1]右载台卸料完成");
                                    this.Status.UnLoadRightStation = false;
                                    //卸料完成，允许上料
                                    this.Status.LoadRightStation = true;
                                    //}



                                }
                                if (this.Status.LoadRightStation)//右载台上料
                                {
                                    if (this.parameter.CarrierNumber > 0)
                                    {
                                        this.Log_Global("[Step1]右载台开始上料");
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                        RightTestCount = 0;  //20241107 上一个夹具归零一次
                                        if (this.Status.IsTesting == Operation.Idle)
                                        {
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_测试, SequenceOrder.Normal);
                                        }
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_装卸, SequenceOrder.Normal);
                                        this.LocalResource.Axes[AxisNameEnum_CT3103.RY].WaitMotionDone();
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        this.Parellel_MoveToAxesPosition(AxesPositionEnum_CT3103.RXZ_装卸);

                                        //CheckIO_FixedJaw_Right_Up(tokenSource);

                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        Parallel.Invoke(() =>
                                        {
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_进料, SequenceOrder.Normal);
                                        },
                                       () =>
                                       {
                                           var pistion = this.LocalResource.Positions[AxesPositionEnum_CT3103.T_IN_Z_最高位].ItemCollection[0].Position;
                                           int number = this.parameter.CarrierNumber - 1;
                                           var motion = pistion - number * 14.5;
                                           this.LocalResource.AxesMotionAction[AxisNameEnum_CT3103.T_IN_Z].SingleAxisMotion(this.LocalResource.Axes[AxisNameEnum_CT3103.T_IN_Z], motion);

                                       });

                                        CheckIO_Tongs_Loosen(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Dn(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Tighten(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Up(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);

                                        CarrierNumberRight = this.parameter.CarrierNumberlist[CarrierCount];//右载台上夹具编号
                                        RightChipNumberlist = this.GetCarrierChipNumber(this.parameter.CarrierNumberlist[CarrierCount]);//对应夹具上所有位置的产品编号
                                        bool P_Test = true;
                                        bool N_Test = true;
                                        bool PandNTest = DetectingPNTest(RightChipNumberlist, out P_Test, out N_Test);//判断该夹具正反面是否都能测试



                                        this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_装卸, SequenceOrder.Normal);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);

                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Dn(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Loosen(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Vacuum_R(true, tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        PandNDetection_R(PandNTest, P_Test, N_Test, tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        CheckIO_Tongs_Up(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);
                                        //CheckIO_FixedJaw_Right_Dn(tokenSource);
                                        if (tokenSource.IsCancellationRequested) return;
                                        WaitMessage(tokenSource);

                                        Parallel.Invoke(() =>
                                        {
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RZ_待机, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_待机, SequenceOrder.Normal);
                                        },
                                        () =>
                                        {
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            //这个可以不用动
                                            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.Y_出料, SequenceOrder.Normal);
                                        });

                                        this.Log_Global("[Step1]右载台上料完成");
                                        //RightTestCount= 0;  //20240820 归零  20241101 邓远明让注释掉


                                        //TempListRight = this.parameter.TemperatureListRight;
                                        foreach (var item in this.parameter.TemperatureListRight)
                                        {
                                            this.TempListRight.Add(item);
                                        }


                                        CarrierCount++;

                                        this.parameter.CarrierNumber--;

                                        if (PandNTest)
                                        {
                                            this.Status.RightStationTest = TestStatusOnBoard.未测试;
                                        }
                                        else
                                        {
                                            this.Status.RightStationTest = TestStatusOnBoard.一次测试;
                                        }

                                        this.Status.LoadRightStation = false;
                                        //右载台控温信号
                                        this.Bridges_WithPauseFunc[Action3103.TemperatureControlRight].Set();
                                    }
                                    else
                                    {
                                        this.Status.RightStationTest = TestStatusOnBoard.未测试;
                                        Parallel.Invoke(() =>
                                        {
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RZ_待机, SequenceOrder.Normal);
                                            if (tokenSource.IsCancellationRequested) return;
                                            WaitMessage(tokenSource);
                                        });
                                    }
                                }
                            }

                            this.Log_Global("[Step1]释放 LoadAndUnLoad_MotionDone 信号");
                            this.Bridges_WithPauseFunc[Action3103.LoadAndUnLoad_MotionDone].Set();


                            this.Status.LoadAndUnLoad = Operation.Idle;
                            break;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step1]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }

            }
        }
        /// <summary>
        /// 检测抓手是否上升
        /// </summary>
        public void CheckIO_Tongs_Up(CancellationTokenSource tokenSource)
        {
            int count = 0;
            while (true)
            {
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_UP].TurnOn(true);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_DN].TurnOn(false);
                Thread.Sleep(500);
                if (!this.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_UP].Interation.IsActive)
                {
                    count++;
                }
                else
                {
                    break;
                }
                if (count > 4)
                {
                    //检测抓手是否上升
                    this.UserRequest_Pause(MT.抓手模组);
                    Form_CheckIO _CheckIO = new Form_CheckIO("抓手上升信号检测异常");
                    _CheckIO.ShowDialog();
                    this.UserRequest_Resume(MT.抓手模组);
                    if (_CheckIO.DialogResult == DialogResult.OK)
                    {
                        break;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Yes)
                    {
                        continue;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("抓手上升信号检测异常；用户取消运行");
                        tokenSource.Cancel();
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 检测抓手是否下降
        /// </summary>
        public void CheckIO_Tongs_Dn(CancellationTokenSource tokenSource)
        {
            int count = 0;
            while (true)
            {
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_UP].TurnOn(false);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_DN].TurnOn(true);
                Thread.Sleep(500);
                if (!this.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_DN].Interation.IsActive)
                {
                    count++;
                }
                else
                {
                    break;
                }
                if (count > 4)
                {
                    //检测抓手是否下降
                    this.UserRequest_Pause(MT.抓手模组);
                    Form_CheckIO _CheckIO = new Form_CheckIO("抓手下降信号检测异常");
                    _CheckIO.ShowDialog();
                    this.UserRequest_Resume(MT.抓手模组);
                    if (_CheckIO.DialogResult == DialogResult.OK)
                    {
                        break;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Yes)
                    {
                        continue;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("抓手下降信号检测异常；用户取消运行");
                        tokenSource.Cancel();
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 检测抓手是否放松
        /// </summary>
        public void CheckIO_Tongs_Loosen(CancellationTokenSource tokenSource)
        {
            int count = 0;
            while (true)
            {
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_OFF].TurnOn(true);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_ON].TurnOn(false);
                Thread.Sleep(800);
                if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_GRAB_ON].Interation.IsActive)
                {
                    count++;
                }
                else
                {
                    break;
                }
                if (count > 4)
                {
                    //检测抓手是否放松
                    this.UserRequest_Pause(MT.抓手模组);
                    Form_CheckIO _CheckIO = new Form_CheckIO("抓手松开信号检测异常");
                    _CheckIO.ShowDialog();
                    this.UserRequest_Resume(MT.抓手模组);
                    if (_CheckIO.DialogResult == DialogResult.OK)
                    {
                        break;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Yes)
                    {
                        continue;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("抓手松开信号检测异常；用户取消运行");
                        tokenSource.Cancel();
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 检测抓手是否收紧
        /// </summary>
        public void CheckIO_Tongs_Tighten(CancellationTokenSource tokenSource)
        {
            int count = 0;
            while (true)
            {
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_OFF].TurnOn(false);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_GRAB_ON].TurnOn(true);
                Thread.Sleep(500);
                if (!this.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_GRAB_ON].Interation.IsActive)
                {
                    count++;
                }
                else
                {
                    break;
                }
                if (count > 4)
                {
                    //检测抓手是否收紧
                    this.UserRequest_Pause(MT.抓手模组);
                    Form_CheckIO _CheckIO = new Form_CheckIO("抓手收紧信号检测异常");
                    _CheckIO.ShowDialog();
                    this.UserRequest_Resume(MT.抓手模组);
                    if (_CheckIO.DialogResult == DialogResult.OK)
                    {
                        break;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Yes)
                    {
                        continue;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("抓手收紧信号检测异常；用户取消运行");
                        tokenSource.Cancel();
                        return;
                    }
                }
            }
        }
        #region 不在使用

        /// <summary>
        /// 检测左载台固定夹爪是否松开
        /// </summary>
        //public void CheckIO_FixedJaw_Left_Up(CancellationTokenSource tokenSource)
        //{
        //while (true)
        //{
        //    if (tokenSource.IsCancellationRequested) return;
        //    WaitMessage(tokenSource);
        //    this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_UP].TurnOn(true);
        //    this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_DN].TurnOn(false);
        //    Thread.Sleep(200);
        //    //if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_HOLDER_UP].Interation.IsActive)
        //    //{
        //    //    //检测左载台固定夹爪是否松开
        //    //    this.UserRequest_Pause(MT.左载台);
        //    //    Form_CheckIO _CheckIO = new Form_CheckIO("左载台固定夹爪松开信号检测异常");
        //    //    _CheckIO.ShowDialog();
        //    //    this.UserRequest_Resume(MT.左载台);
        //    //    if (_CheckIO.DialogResult == DialogResult.OK)
        //    //    {
        //    //        break;
        //    //    }
        //    //    if (_CheckIO.DialogResult == DialogResult.Yes)
        //    //    {
        //    //        continue;
        //    //    }
        //    //    if (_CheckIO.DialogResult == DialogResult.Cancel)
        //    //    {
        //    //        this.Log_Global("左载台固定夹爪松开信号检测异常；用户取消运行");
        //    //        tokenSource.Token.ThrowIfCancellationRequested();
        //    //        return;
        //    //    }
        //    //}
        //    break;
        //}
        //}
        /// <summary>
        /// 检测左载台固定夹爪是否收紧
        /// </summary>
        //public void CheckIO_FixedJaw_Left_Dn(CancellationTokenSource tokenSource)
        //{
        //    while (true)
        //    {
        //        if (tokenSource.IsCancellationRequested) return;
        //        WaitMessage(tokenSource);
        //        this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_UP].TurnOn(false);
        //        this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_L_HOLDER_DN].TurnOn(true);
        //        Thread.Sleep(200);
        //        //if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_HOLDER_DN].Interation.IsActive)
        //        //{
        //        //    //检测左载台固定夹爪是否收紧
        //        //    this.UserRequest_Pause(MT.左载台);
        //        //    Form_CheckIO _CheckIO = new Form_CheckIO("左载台固定夹爪收紧信号检测异常");
        //        //    _CheckIO.ShowDialog();
        //        //    this.UserRequest_Resume(MT.左载台);
        //        //    if (_CheckIO.DialogResult == DialogResult.OK)
        //        //    {
        //        //        break;
        //        //    }
        //        //    if (_CheckIO.DialogResult == DialogResult.Yes)
        //        //    {
        //        //        continue;
        //        //    }
        //        //    if (_CheckIO.DialogResult == DialogResult.Cancel)
        //        //    {
        //        //        this.Log_Global("左载台固定夹爪收紧信号检测异常；用户取消运行");
        //        //        tokenSource.Token.ThrowIfCancellationRequested();
        //        //        return;
        //        //    }
        //        //}
        //        break;
        //    }
        //}
        /// <summary>
        /// 检测右载台固定夹爪是否松开
        /// </summary>
        //public void CheckIO_FixedJaw_Right_Up(CancellationTokenSource tokenSource)
        //{
        //    while (true)
        //    {
        //        if (tokenSource.IsCancellationRequested) return;
        //        WaitMessage(tokenSource);
        //        this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_UP].TurnOn(true);
        //        this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_DN].TurnOn(false);
        //        Thread.Sleep(200);
        //        //if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_HOLDER_UP].Interation.IsActive)
        //        //{
        //        //    //检测右载台固定夹爪是否松开
        //        //    this.UserRequest_Pause(MT.右载台);
        //        //    Form_CheckIO _CheckIO = new Form_CheckIO("右载台固定夹爪松开信号检测异常");
        //        //    _CheckIO.ShowDialog();
        //        //    this.UserRequest_Resume(MT.右载台);
        //        //    if (_CheckIO.DialogResult == DialogResult.OK)
        //        //    {
        //        //        break;
        //        //    }
        //        //    if (_CheckIO.DialogResult == DialogResult.Yes)
        //        //    {
        //        //        continue;
        //        //    }
        //        //    if (_CheckIO.DialogResult == DialogResult.Cancel)
        //        //    {
        //        //        this.Log_Global("右载台固定夹爪松开信号检测异常；用户取消运行");
        //        //        tokenSource.Token.ThrowIfCancellationRequested();
        //        //        return;
        //        //    }
        //        //}
        //        break;
        //    }
        //}
        /// <summary>
        /// 检测左载台固定夹爪是否收紧
        /// </summary>
        //public void CheckIO_FixedJaw_Right_Dn(CancellationTokenSource tokenSource)
        //{
        //    while (true)
        //    {
        //        if (tokenSource.IsCancellationRequested) return;
        //        WaitMessage(tokenSource);
        //        this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_UP].TurnOn(false);
        //        this.LocalResource.IOs[IONameEnum_CT3103.CYL_STG_R_HOLDER_DN].TurnOn(true);
        //        Thread.Sleep(200);
        //        //if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_HOLDER_DN].Interation.IsActive)
        //        //{
        //        //    //检测左载台固定夹爪是否收紧
        //        //    this.UserRequest_Pause(MT.右载台);
        //        //    Form_CheckIO _CheckIO = new Form_CheckIO("右载台固定夹爪收紧信号检测异常");
        //        //    _CheckIO.ShowDialog();
        //        //    this.UserRequest_Resume(MT.右载台);
        //        //    if (_CheckIO.DialogResult == DialogResult.OK)
        //        //    {
        //        //        break;
        //        //    }
        //        //    if (_CheckIO.DialogResult == DialogResult.Yes)
        //        //    {
        //        //        continue;
        //        //    }
        //        //    if (_CheckIO.DialogResult == DialogResult.Cancel)
        //        //    {
        //        //        this.Log_Global("右载台固定夹爪收紧信号检测异常；用户取消运行");
        //        //        tokenSource.Token.ThrowIfCancellationRequested();
        //        //        return;
        //        //    }
        //        //}
        //        break;
        //    }
        //}
        #endregion
        /// <summary>
        /// 检测抓手是否旋转到正面
        /// </summary>
        public void CheckIO_Tongs_Turn_P(CancellationTokenSource tokenSource)
        {
            int count = 0;
            while (true)
            {
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_L].TurnOn(true);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_R].TurnOn(false);
                Thread.Sleep(1200);
                if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_ROTATE_R].Interation.IsActive)
                {
                    count++;
                }
                else
                {
                    break;
                }
                if (count > 4)
                {
                    //检测抓手是否旋转到正面
                    this.UserRequest_Pause(MT.抓手模组);
                    Form_CheckIO _CheckIO = new Form_CheckIO("抓手旋转正面信号检测异常");
                    _CheckIO.ShowDialog();
                    this.UserRequest_Resume(MT.抓手模组);
                    if (_CheckIO.DialogResult == DialogResult.OK)
                    {
                        break;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Yes)
                    {
                        continue;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("抓手旋转正面信号检测异常；用户取消运行");
                        tokenSource.Cancel();
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 检测抓手是否旋转到反面
        /// </summary>
        public void CheckIO_Tongs_Turn_N(CancellationTokenSource tokenSource)
        {
            int count = 0;
            while (true)
            {
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_L].TurnOn(false);
                this.LocalResource.IOs[IONameEnum_CT3103.CYL_PICKER_ROTATE_R].TurnOn(true);
                Thread.Sleep(1200);
                if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_PICKER_ROTATE_L].Interation.IsActive)
                {
                    count++;
                }
                else
                {
                    break;
                }
                if (count > 4)
                {
                    //检测抓手是否旋转到反面
                    this.UserRequest_Pause(MT.抓手模组);
                    Form_CheckIO _CheckIO = new Form_CheckIO("抓手旋转反面信号检测异常");
                    _CheckIO.ShowDialog();
                    this.UserRequest_Resume(MT.抓手模组);
                    if (_CheckIO.DialogResult == DialogResult.OK)
                    {
                        break;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Yes)
                    {
                        continue;
                    }
                    if (_CheckIO.DialogResult == DialogResult.Cancel)
                    {
                        this.Log_Global("抓手旋转反面信号检测异常；用户取消运行");
                        tokenSource.Cancel();
                        return;
                    }
                }
            }
        }
        public void CheckIO_Vacuum_L(bool turnon, CancellationTokenSource tokenSource)
        {
            int count = 0;
            if (turnon)
            {
                this.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_L].TurnOn(true);
                while (true)
                {
                    Thread.Sleep(200);
                    if (!this.LocalResource.IOs[IONameEnum_CT3103.SEN_Vacuum_L].Interation.IsActive)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                    if (count > 4)
                    {
                        //检测左载台真空
                        this.UserRequest_Pause(MT.抓手模组);
                        Form_CheckIO _CheckIO = new Form_CheckIO("左载台真空信号检测异常");
                        _CheckIO.ShowDialog();
                        this.UserRequest_Resume(MT.抓手模组);
                        if (_CheckIO.DialogResult == DialogResult.OK)
                        {
                            break;
                        }
                        if (_CheckIO.DialogResult == DialogResult.Yes)
                        {
                            continue;
                        }
                        if (_CheckIO.DialogResult == DialogResult.Cancel)
                        {
                            this.Log_Global("左载台真空信号检测异常；用户取消运行");
                            tokenSource.Cancel();
                            return;
                        }
                    }
                }


            }
            else
            {
                this.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_L].TurnOn(false);
                Thread.Sleep(200);
            }
        }
        public void CheckIO_Vacuum_R(bool turnon, CancellationTokenSource tokenSource)
        {
            int count = 0;
            if (turnon)
            {
                this.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_R].TurnOn(true);
                while (true)
                {
                    Thread.Sleep(200);
                    if (!this.LocalResource.IOs[IONameEnum_CT3103.SEN_Vacuum_R].Interation.IsActive)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                    if (count > 4)
                    {
                        //检测右载台真空
                        this.UserRequest_Pause(MT.抓手模组);
                        Form_CheckIO _CheckIO = new Form_CheckIO("右载台真空信号检测异常");
                        _CheckIO.ShowDialog();
                        this.UserRequest_Resume(MT.抓手模组);
                        if (_CheckIO.DialogResult == DialogResult.OK)
                        {
                            break;
                        }
                        if (_CheckIO.DialogResult == DialogResult.Yes)
                        {
                            continue;
                        }
                        if (_CheckIO.DialogResult == DialogResult.Cancel)
                        {
                            this.Log_Global("右载台真空信号检测异常；用户取消运行");
                            tokenSource.Cancel();
                            return;
                        }
                    }
                }


            }
            else
            {
                this.LocalResource.IOs[IONameEnum_CT3103.Pedestal_Vacuum_R].TurnOn(false);
                Thread.Sleep(200);
            }
        }
        /// <summary>
        /// 左载台正反检测
        /// </summary>
        public void PandNDetection_L(bool PandNTest, bool P_Test, bool N_Test, CancellationTokenSource tokenSource)
        {
            if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_L_CARRIER_LOAD].Interation.IsActive)//返回true时夹具反置
            {
                if (P_Test)//夹具反置时正面需要测试则需要翻转夹具
                {
                    LeftTestCount = 0;//正面需要测试时从索引0开始
                    this.Log_Global("[Step1]左载台夹具反向，进行翻转");
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Tighten(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_L(false, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_N(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_L(true, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    //CheckIO_FixedJaw_Left_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_P(tokenSource);
                }
                else
                {
                    LeftTestCount = 4;//反面需要测试时从索引4开始
                }
            }
            else
            {
                //夹具正面放置，切只有反面需要测试时夹具需要翻转
                if (!PandNTest && N_Test)
                {
                    LeftTestCount = 4;//反面需要测试时从索引4开始
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Tighten(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_L(false, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_N(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_L(true, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    //CheckIO_FixedJaw_Left_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_P(tokenSource);
                }
            }

        }
        /// <summary>
        /// 右载台正反检测
        /// </summary>
        public void PandNDetection_R(bool PandNTest, bool P_Test, bool N_Test, CancellationTokenSource tokenSource)
        {
            if (this.LocalResource.IOs[IONameEnum_CT3103.SEN_STG_R_CARRIER_LOAD].Interation.IsActive)//返回true时夹具反置
            {
                if (P_Test)//夹具反置时正面需要测试则需要翻转夹具
                {
                    RightTestCount = 0;//正面需要测试时从索引0开始
                    this.Log_Global("[Step1]右载台夹具反向，进行翻转");
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Tighten(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_R(false, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_N(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_R(true, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    //CheckIO_FixedJaw_Left_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_P(tokenSource);
                }
                else
                {
                    //夹具反置时反面需要测试
                    RightTestCount = 4;//反面需要测试时从索引4开始
                }
            }
            else
            {
                //夹具正面放置，切只有反面需要测试时夹具需要翻转
                if (!PandNTest && N_Test)
                {
                    RightTestCount = 4;//反面需要测试时从索引4开始
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Tighten(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_R(false, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_N(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Loosen(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Vacuum_R(true, tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Up(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    //CheckIO_FixedJaw_Left_Dn(tokenSource);
                    if (tokenSource.IsCancellationRequested) return;
                    WaitMessage(tokenSource);
                    CheckIO_Tongs_Turn_P(tokenSource);
                }
            }

        }

        private bool DetectingPNTest(List<string> ChipNumberlist, out bool P_Test, out bool N_Test)
        {
            //通过获取对应夹具中的产品编号来判断夹具的那一面需要测试，没有产品时规定再表格中输入“NA”
            P_Test = false;
            for (int i = 0; i < 4; i++)
            {
                if (ChipNumberlist[i] != "NA" && string.IsNullOrEmpty(ChipNumberlist[i]) == false)
                {
                    P_Test = true;
                    break;
                }
            }
            N_Test = false;
            for (int i = 4; i < 8; i++)
            {
                if (ChipNumberlist[i] != "NA" && string.IsNullOrEmpty(ChipNumberlist[i]) == false)
                {
                    N_Test = true;
                    break;
                }
            }
            return P_Test && N_Test;
        }
    }

}
