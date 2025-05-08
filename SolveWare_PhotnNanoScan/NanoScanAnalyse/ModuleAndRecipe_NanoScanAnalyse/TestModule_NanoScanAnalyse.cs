using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.UIComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static SolveWare_BurnInInstruments.Photon_NanoScan;

namespace SolveWare_TestPackage
{
    [SupportedCalculator(
        "TestCalculator_H_width_13p5_Deg",
        "TestCalculator_H_width_50_Deg",
        //"TestCalculator_H_width_005",
        "TestCalculator_V_width_13p5_Deg",
        "TestCalculator_V_width_50_Deg",
        //"TestCalculator_V_width_005",
        "TestCalculator_FF_Temperature",
        "TestCalculator_FF_DrivingCurrent_mA",
        "TestCalculator_FF_MoveDistance_mm",
        "TestCalculator_PrintFFTestParams"
        )]
    
    
    [ConfigurableInstrument("Keithley2602B", "Source_2602B", "用于驱动器件")]
    [ConfigurableInstrument("Photon_NanoScan", "Photon_NanoScan", "用于光束收集")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "用于获取载台当前温度")]

    [ConfigurableAxis("TestX_测试LX轴", "TestX_测试LX轴&移动测试")]

    [ConfigurablePosition("光束测试位", "用于提取测试LX轴位置")]

   
    [CustomizeRawDataUI ("Form_RawDataViewer_FF")]
    public class TestModule_NanoScanAnalyse : TestModuleBase
    {
        public TestModule_NanoScanAnalyse() : base() {   }
        #region 以get属性获取测试模块运行所需资源
        //ISourceMeter_Golight SourceMeter_Master { get { return (ISourceMeter_Golight)this.ModuleResource["SourceMeter_Master"]; } }
        Keithley2602B SourceMeter_Master { get { return (Keithley2602B)this.ModuleResource["Source_2602B"]; } }
        Photon_NanoScan NanoScananAlyse { get { return (Photon_NanoScan)this.ModuleResource["Photon_NanoScan"]; } }
        MotorAxisBase TestXAxis { get { return (MotorAxisBase)this.ModuleResource["TestX_测试LX轴"]; } }         
        AxesPosition NanoScan_Pos { get { return (AxesPosition)this.ModuleResource["光束测试位"]; } }
        MeerstetterTECController_1089 TEC { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }
        #endregion
       
        TestRecipe_NanoScanAnalyse TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_NanoScanAnalyse); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_NanoScanAnalyse>(testRecipe); }
        RawData_NanoScanAnalyse RawData { get; set; }
        double DrivingCurrent_mA { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_NanoScanAnalyse(); return RawData; }
        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            this.DrivingCurrent_mA = 0.0;
            bool Findok = dutStreamData.SummaryDataCollection.ItemCollection.Exists(t => t.Name == this.TestRecipe.RefData_Name_DrivingCurrent_mA);
            if (this.TestRecipe.UseRefData_DrivingCurrent_mA == true && Findok)
            {
                var sdata = dutStreamData.SummaryDataCollection.GetSingleItem(this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString());
                DrivingCurrent_mA = Convert.ToDouble(sdata.Value);
                DrivingCurrent_mA += this.TestRecipe.RefData_DrivingCurrent_Offset_mA;
            }
            else
            {
                DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
            }
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //if (MessageBox.Show("确定已经正确上料到光束分析仪位置?", "开始光束分析动作", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                if (SourceMeter_Master.IsOnline == false || SourceMeter_Master == null)
                {
                    Log_Global($"仪器[{SourceMeter_Master.Name}]状态为[{SourceMeter_Master.IsOnline}]");
                    return;
                }
                if (NanoScananAlyse.IsOnline == false || NanoScananAlyse == null)
                {
                    Log_Global($"仪器[{NanoScananAlyse.Name}]状态为[{NanoScananAlyse.IsOnline}]");
                    return;
                }
                if (/*TEC.IsOnline == false ||*/ TEC == null)
                {
                    Log_Global($"仪器[{TEC.Name}]状态为[{TEC.IsOnline}]");
                    return;
                }
                this.Log_Global("开始进行光束分析...");
               
                if (this.TestRecipe.MoveDistance_mm<0)
                {
                    throw new Exception(string.Format("设定的X轴移动距离为{0},不能小于0，否则会撞！", this.TestRecipe.MoveDistance_mm));             
                }
                if (DrivingCurrent_mA == 0)
                {
                    DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
                }
                #region  点位和轴校验
                if (!TestXAxis.Name.Contains("LX"))
                {
                    throw new Exception($"该点位所需的轴为[TestX_测试LX轴],加载的轴名为[{TestXAxis.Name}]!");
                }
                if (this.NanoScan_Pos.ItemCollection.Count != 3)
                {
                    throw new Exception($"点位[{this.NanoScan_Pos}]串行移动错误:该点位所需的轴数量为[{this.NanoScan_Pos.ItemCollection.Count}],实际需要一个轴!");
                }              
                bool isVerified = true;
                string ErrMsg = string.Empty;
                //得到设定的原始0度
                double NanoScan_XPos = 0;
                int count = 0;
                //校验点位所需轴和提取轴位置
                foreach (var item in this.NanoScan_Pos)// TestX_测试LX轴
                {
                    count++;
                    if (TestXAxis.Name == item.Name)
                    {
                        NanoScan_XPos = item.Position;
                        continue;
                    }
                    else if(count==3&& NanoScan_XPos==0)
                    {
                        isVerified = false;
                        ErrMsg = ($"点位[{this.NanoScan_Pos}]串行移动错误:该点位所需的轴为[{item.Name}],加载的轴名为[{TestXAxis.Name}]!");
                        break;
                    }
                }
                if (isVerified == false)
                {
                    throw new Exception(ErrMsg);
                }
               
                #endregion
                //加电让产品发光
                ///需根据实际使用条件定制
                this.Log_Global($"开始上电...[{this.DrivingCurrent_mA}]mA...");
                //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(this.TestRecipe.DrivingCurrent_mA / 1000.0);
                //this.SourceMeter_Master.IsOutputEnable = true;

                //加电前等待
                if (this.TestRecipe.BiasBeforeWait_ms > 0)
                {
                    this.Log_Global($"加电前等待 {this.TestRecipe.BiasBeforeWait_ms}ms.");
                    Thread.Sleep(this.TestRecipe.BiasBeforeWait_ms);
                }

                SourceMeter_Master.SetupSMU_LD(this.DrivingCurrent_mA, 2.5); //默认2.5V

                //加电后等待至少500ms
                if (this.TestRecipe.BiasAfterWait_ms > 0)
                {
                    int waittime = 500;
                    waittime = Math.Max(waittime, this.TestRecipe.BiasAfterWait_ms);
                    this.Log_Global($"加电后等待 {waittime}ms.");
                    Thread.Sleep(waittime);
                }

                //到达测试位置
                this.TestXAxis.MoveToV3(NanoScan_XPos, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                int err = this.TestXAxis.WaitMotionDone();
                if (err == ErrorCodes.MotorMoveTimeOutError)
                {
                    this.Log_Global($"轴[{this.TestXAxis.Name}]运动超时！");
                    throw new Exception(string.Format("轴{0}运动超时！", this.TestXAxis.Name));
                }

                NanoScananAlyse.StartControl();//开启光束仪

                List<dataKvp> xdata_1st = new List<dataKvp>();
                List<dataKvp> ydata_1st = new List<dataKvp>(); 
                List<dataKvp> xdata_2st = new List<dataKvp>();
                List<dataKvp> ydata_2st  = new List<dataKvp>();

                double BeamWidth_13p5_X_1st = 0;
                double BeamWidth_50_X_1st = 0;
                double BeamWidth_5_X_1st = 0;
                double BeamWidth_13p5_Y_1st = 0;
                double BeamWidth_50_Y_1st = 0;
                double BeamWidth_5_Y_1st = 0;

                double BeamWidth_13p5_X_2nd = 0;
                double BeamWidth_50_X_2nd = 0;
                double BeamWidth_5_X_2nd = 0;
                double BeamWidth_13p5_Y_2nd = 0;
                double BeamWidth_50_Y_2nd = 0;
                double BeamWidth_5_Y_2nd = 0;

                float[] amplitude = new float[this.TestRecipe.numOfPoints];
                float[] position = new float[this.TestRecipe.numOfPoints];


                //
                NanoScananAlyse.NsInteropGetDeviceList();
                //set
                NanoScananAlyse.SetAveragAndRotation(this.TestRecipe.finite,this.TestRecipe.rolling,this.TestRecipe.RotationFrequency);
                //
                NanoScananAlyse.NsInteropSetDataAcquisition();
                Thread.Sleep(this.TestRecipe.ScanBeforeWait_ms);               
                //3
                NanoScananAlyse.WaitForFirstData();               
                //4
                //NanoScananAlyse.ReadProfile(
                //    this.TestRecipe.aperture,
                //    this.TestRecipe.leftBound,
                //    this.TestRecipe.rightBound,
                //    this.TestRecipe.samplingRes,
                //    this.TestRecipe.decimation,
                //    this.TestRecipe.numOfPoints,
                //    ref amplitude,
                //    ref position);  //传recipe 里面的参数 要接收有返回值 
                //                    //获取到的数据
                xdata_1st = NanoScananAlyse.ReadProfile(
                            NanoScan_AxisNameEnum_20050A.X, //this.TestRecipe.aperture,
                            this.TestRecipe.leftBound,
                            this.TestRecipe.rightBound,
                            this.TestRecipe.samplingRes,
                            this.TestRecipe.decimation,
                            this.TestRecipe.numOfPoints,
                            ref BeamWidth_13p5_X_1st,
                            ref BeamWidth_50_X_1st,
                             ref BeamWidth_5_X_1st);

                 ydata_1st = NanoScananAlyse.ReadProfile(
                            NanoScan_AxisNameEnum_20050A.Y,//this.TestRecipe.aperture,
                            this.TestRecipe.leftBound,
                            this.TestRecipe.rightBound,
                            this.TestRecipe.samplingRes, 
                            this.TestRecipe.decimation,
                            this.TestRecipe.numOfPoints,
                            ref BeamWidth_13p5_Y_1st,
                            ref BeamWidth_50_Y_1st,
                            ref BeamWidth_5_Y_1st);                

                //for (int i = 0; i < amplitude.Length; i++)
                //{
                //    RawData.Add(new RawDatumItem_NanoScanAnalyse()
                //    {
                //        X_Amplitude = amplitude[i],
                //        X_Position = position[i]
                //    });
                //}

                //向后移动1mm再测试一次
                this.TestXAxis.MoveToV3(NanoScan_XPos- this.TestRecipe.MoveDistance_mm, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                err = this.TestXAxis.WaitMotionDone();
                if (err == ErrorCodes.MotorMoveTimeOutError)
                {
                    this.Log_Global($"轴[{this.TestXAxis.Name}]运动超时！");
                    throw new Exception(string.Format("轴{0}运动超时！", this.TestXAxis.Name));
                }
                //再次获取数据
                Thread.Sleep(1000);
                //1
                NanoScananAlyse.NsInteropSetDataAcquisition();
                Thread.Sleep(this.TestRecipe.ScanBeforeWait_ms);
                ////2
                //NanoScananAlyse.NsInteropGetDeviceList();
                //3
                NanoScananAlyse.WaitForFirstData();
                //4
                //NanoScananAlyse.ReadProfile(
                //    this.TestRecipe.aperture,
                //    this.TestRecipe.leftBound,
                //    this.TestRecipe.rightBound,
                //    this.TestRecipe.samplingRes,
                //    this.TestRecipe.decimation,
                //    this.TestRecipe.numOfPoints,
                //    ref amplitude,
                //    ref position);  //传recipe 里面的参数 要接收有返回值 
                //                    //获取到的数据
               xdata_2st = NanoScananAlyse.ReadProfile(
                            NanoScan_AxisNameEnum_20050A.X, //this.TestRecipe.aperture,
                            this.TestRecipe.leftBound,
                            this.TestRecipe.rightBound,
                            this.TestRecipe.samplingRes,
                            this.TestRecipe.decimation,
                            this.TestRecipe.numOfPoints,
                            ref BeamWidth_13p5_X_2nd,
                            ref BeamWidth_50_X_2nd,
                             ref BeamWidth_5_X_2nd);

                ydata_2st = NanoScananAlyse.ReadProfile(
                            NanoScan_AxisNameEnum_20050A.Y,//this.TestRecipe.aperture,
                            this.TestRecipe.leftBound,
                            this.TestRecipe.rightBound,
                            this.TestRecipe.samplingRes,
                            this.TestRecipe.decimation,
                            this.TestRecipe.numOfPoints,
                            ref BeamWidth_13p5_Y_2nd,
                            ref BeamWidth_50_Y_2nd,
                            ref BeamWidth_5_Y_2nd);


                //传RawData
                //先校验长度
                if (xdata_1st.Count!=ydata_1st.Count|| xdata_2st.Count != ydata_2st.Count||
                    xdata_1st.Count != xdata_2st.Count || ydata_1st.Count != ydata_2st.Count)
                {
                    throw new Exception($"光束分析出来的数据有误，请检查测试是否完整,xdata_1st[{xdata_1st.Count}],ydata_1st[{ydata_1st.Count}]"+
                                        $"xdata_2st[{xdata_2st.Count}],ydata_2st[{ydata_2st.Count}]");
                }
                for (int i = 0; i < xdata_1st.Count; i++)
                {
                    RawData.Add(new RawDatumItem_NanoScanAnalyse()
                    {
                        X_Amplitude_1st = xdata_1st[i].adc,
                        X_Position_1st = xdata_1st[i].pos,
                        Y_Amplitude_1st = ydata_1st[i].adc,
                        Y_Position_1st = ydata_1st[i].pos,

                        X_Amplitude_2nd = xdata_2st[i].adc,
                        X_Position_2nd = xdata_2st[i].pos,
                        Y_Amplitude_2nd = ydata_2st[i].adc,
                        Y_Position_2nd = ydata_2st[i].pos,
                    });
                }


                RawData.FF_DrivingCurrent_mA = this.DrivingCurrent_mA;
                RawData.MoveDistance_mm = this.TestRecipe.MoveDistance_mm;

                RawData.BeamWidth_13p5_X_1st = BeamWidth_13p5_X_1st;
                RawData.BeamWidth_50_X_1st = BeamWidth_50_X_1st;
               // RawData.BeamWidth_5_X_1st = BeamWidth_5_X_1st;

                RawData.BeamWidth_13p5_Y_1st = BeamWidth_13p5_Y_1st;
                RawData.BeamWidth_50_Y_1st = BeamWidth_50_Y_1st;
               // RawData.BeamWidth_5_Y_1st = BeamWidth_5_Y_1st;


                RawData.BeamWidth_13p5_X_2nd = BeamWidth_13p5_X_2nd;
                RawData.BeamWidth_50_X_2nd = BeamWidth_50_X_2nd;
                //RawData.BeamWidth_5_X_2nd = BeamWidth_5_X_2nd;

                RawData.BeamWidth_13p5_Y_2nd = BeamWidth_13p5_Y_2nd;
                RawData.BeamWidth_50_Y_2nd = BeamWidth_50_Y_2nd;
               // RawData.BeamWidth_5_Y_2nd = BeamWidth_5_Y_2nd;

                //下电
                this.Log_Global($"开始下电...[{0.0}]mA...");
                //this.SourceMeter_Master.CurrentSetpoint_A =(float) 0.0;
                //this.SourceMeter_Master.IsOutputEnable = false;
                // NanoScananAlyse.StopControl();
                ////让轴回去
                //this.TestXAxis.MoveToV3(0, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                this.SourceMeter_Master.Reset();
                if (token.IsCancellationRequested)
                {
                    this.RawData.IsAlignmentDone = false;
                }
                else
                {
                    this.RawData.IsAlignmentDone = true;

                }
                //获取温度
                RawData.FF_Temperature_degC = TEC.CurrentObjectTemperature;
                this.Log_Global("TestModule_NanoScanAnalyse 模块运行完成");
            }
            catch (Exception ex)
            {
                //this.SourceMeter_Master.CurrentSetpoint_A = (float)0.0;
                //this.SourceMeter_Master.IsOutputEnable = false;
                this.SourceMeter_Master.Reset();
                this._core.ReportException("光束分析模块运行错误", ErrorCodes.Module_FF_Failed, ex);
                ///根据模块的影响决定是否往上抛出错误
                ///throw ex;s
            }
        }
    }
}