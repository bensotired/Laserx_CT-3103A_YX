using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator(
        "TestCalculator_PeaPer_PEA1_Deg",
        "TestCalculator_PeaPer_Power_PEA1_mW",
        "TestCalculator_PeaPer_NullA1_Deg",
        "TestCalculator_PeaPer_Power_NullA1_mW",
        "TestCalculator_PeaPer_PER1_dB",
        //"TestCalculator_PeaPer_PEA",
        "TestCalculator_PER_Temperature",
        "TestCalculator_PER_DrivingCurrent_mA",
        "TestCalculator_PER_Factor_K",
        "TestCalculator_PER_Factor_B"
        )]
        
    [ConfigurableInstrument("Keithley2602B", "Source_2602B", "用于驱动器件")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "用于获取载台当前温度")]

    [ConfigurableAxis("PER_偏振旋转轴", "PER_偏振旋转轴")]

    [ConfigurablePosition("偏振0度位", "偏振0度位")]
    [ConfigurablePosition("偏振90度位", "偏振90度位")]

    public class TestModule_PeaPer: TestModuleBase 
    {
        public TestModule_PeaPer() : base() { }
        #region 以get属性获取测试模块运行所需资源
        Keithley2602B SourceMeter_Master { get { return (Keithley2602B)this.ModuleResource["Source_2602B"]; } }
        MotorAxisBase PERAxis { get { return (MotorAxisBase)this.ModuleResource["PER_偏振旋转轴"]; } }
        AxesPosition PeaPer_0 { get { return (AxesPosition)this.ModuleResource["偏振0度位"]; } }
        AxesPosition PeaPer_90 { get { return (AxesPosition)this.ModuleResource["偏振90度位"]; } }
        MeerstetterTECController_1089 TEC { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }

        #endregion

        TestRecipe_PeaPer TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_PeaPer); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_PeaPer>(testRecipe); }
        RawData_PeaPer RawData { get; set; }
        double DrivingCurrent_mA { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_PeaPer(); return RawData; }
        //public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        //{
        //    this.DrivingCurrent_mA = 0.0;
        //    bool Findok = dutStreamData.SummaryDataCollection.ItemCollection.Exists(t => t.Name == this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString());
        //    if (this.TestRecipe.UseRefData_DrivingCurrent_mA == true && Findok)
        //    {
        //        var sdata = dutStreamData.SummaryDataCollection.GetSingleItem(this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString());
        //        DrivingCurrent_mA = Convert.ToDouble(sdata.Value);
        //        DrivingCurrent_mA += this.TestRecipe.RefData_DrivingCurrent_Offset_mA;
        //    }
        //    else
        //    {
        //        DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
        //    }
        //}
        public override void Run(CancellationToken token)
        {
            try
            {
                //if (MessageBox.Show("确定已经正确上料到PeaPer位置?", "开始偏振动作", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                if (SourceMeter_Master.IsOnline == false || SourceMeter_Master == null)
                {
                    Log_Global($"仪器[{SourceMeter_Master.Name}]状态为[{SourceMeter_Master.IsOnline}]");
                    return;
                }
                if (/*TEC.IsOnline == false ||*/ TEC == null)
                {
                    Log_Global($"仪器[{TEC.Name}]状态为[{TEC.IsOnline}]");
                    return;
                }
                this.Log_Global("开始进行偏振态测试...");
               
                //if (DrivingCurrent_mA == 0)
                //{
                //    DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
                //}
                #region  点位和轴校验
                if (this.PeaPer_0.ItemCollection.Count != 1)
                {
                    throw new Exception($"点位[{this.PeaPer_0}]串行移动错误:该点位所需的轴数量为[{this.PeaPer_0.ItemCollection.Count}],实际需要一个轴!");
                }
                if (this.PeaPer_90.ItemCollection.Count != 1)
                {
                    throw new Exception($"点位[{this.PeaPer_90}]串行移动错误:该点位所需的轴数量为[{this.PeaPer_90.ItemCollection.Count}],实际需要一个轴!");
                }
                bool isVerified = true;
                string ErrMsg = string.Empty;
                //得到设定的原始0度
                double phyZeroAngle = 0;
                //校验点位所需轴和提取轴位置
                foreach (var item in this.PeaPer_0)// TestX_测试LX轴
                {
                    if (PERAxis.Name == item.Name)
                    {
                        phyZeroAngle = item.Position;
                        continue;
                    }                    
                    else
                    {
                        isVerified = false;
                        ErrMsg = ($"点位[{this.PeaPer_0}]串行移动错误:该点位所需的轴为[{item.Name}],加载的轴名为[{PERAxis.Name}]!");
                        break;
                    }
                }
                if (isVerified == false)
                {
                    throw new Exception(ErrMsg);
                }

                foreach (var item in this.PeaPer_90)// TestX_测试LX轴
                {
                    if (PERAxis.Name == item.Name)
                    {
                        //phyZeroAngle = item.Position;
                        continue;
                    }
                    else
                    {
                        isVerified = false;
                        ErrMsg = ($"点位[{this.PeaPer_90}]串行移动错误:该点位所需的轴为[{item.Name}],加载的轴名为[{PERAxis.Name}]!");
                        break;
                    }
                }
                if (isVerified == false)
                {
                    throw new Exception(ErrMsg);
                }
                #endregion

                ///需根据实际使用条件定制
                this.Log_Global($"开始上电...[{this.TestRecipe.DrivingCurrent_mA}]mA...");
                //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(this.TestRecipe.DrivingCurrent_mA / 1000.0);
                //this.SourceMeter_Master.IsOutputEnable = true;
                this.SourceMeter_Master.SetupSMU_LD(this.TestRecipe.DrivingCurrent_mA, 2.5);

                //传测试电流值
                this.RawData.Per_DrivingCurrent_mA = this.TestRecipe.DrivingCurrent_mA;
                ////得到设定的原始0度
                //double phyZeroAngle = PeaPer_0.ItemCollection[0].Position;
                //Peak扫描
                Stopwatch sw = new Stopwatch();
                sw.Start();
                this.Log_Global($"PeaPer开始Peak扫描,驱动电流[{this.TestRecipe.DrivingCurrent_mA}]mA," +
                    $"Peak起始角度[{this.TestRecipe.PeakSearchStart_Deg}]," +
                     $"Peak结束角度[{this.TestRecipe.PeakSearchEnd_Deg}]," +
                      $"Peak步进角度[{this.TestRecipe.StepAngle_Deg}]," +
                      $"现在时间是[{DateTime.Now}]");

                for (double angle = phyZeroAngle + this.TestRecipe.PeakSearchStart_Deg;
                  angle <= phyZeroAngle + this.TestRecipe.PeakSearchEnd_Deg;
                  angle += this.TestRecipe.StepAngle_Deg)
                {
                    //this.RawData.Points.Add(MoveAngleMeasurePower(angle, phyZeroAngle));//只测一个电流，不用做list
                    this.RawData.Add(MoveAngleMeasurePower(angle, phyZeroAngle));
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"偏振态测试被取消...");
                        break;
                    }
                }
                sw.Stop();
                this.Log_Global($"Peak[{this.TestRecipe.DrivingCurrent_mA}]mA扫描结束，耗时：[{sw.Elapsed.TotalSeconds}]");
                //Null扫描
                sw.Restart();
                this.Log_Global($"PeaPer开始Null扫描,驱动电流[{this.TestRecipe.DrivingCurrent_mA}]mA," +
                    $"Null起始角度[{this.TestRecipe.NullSearchStart_Deg}]," +
                     $"Null结束角度[{this.TestRecipe.NullSearchEnd_Deg}]," +
                      $"Null步进角度[{this.TestRecipe.StepAngle_Deg}]," +
                      $"现在时间是[{DateTime.Now}]");

                for (double angle = phyZeroAngle + this.TestRecipe.NullSearchStart_Deg;
                  angle <= phyZeroAngle + this.TestRecipe.NullSearchEnd_Deg;
                  angle += this.TestRecipe.StepAngle_Deg)
                {
                    //this.RawData.Points.Add(MoveAngleMeasurePower(angle, phyZeroAngle));
                    this.RawData.Add(MoveAngleMeasurePower(angle, phyZeroAngle));//只测一个电流，不用做list
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"偏振态测试被取消...");
                        break;
                    }
                }
                sw.Stop();
                this.Log_Global($"Null[{this.TestRecipe.DrivingCurrent_mA}]mA扫描结束，耗时：[{sw.Elapsed.TotalSeconds}]");
                RawData.PER_Factor_K =this.TestRecipe.Power_Factor_K;
                RawData.PER_Factor_B =this.TestRecipe.Power_Factor_B;
                //下电
                //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(0.0);
                //this.SourceMeter_Master.IsOutputEnable = false;
                this.SourceMeter_Master.Reset();
                RawData.Per_Temperature_degC = TEC.CurrentObjectTemperature;
                if (token.IsCancellationRequested)
                {
                    this.RawData.IsAlignmentDone = false;
                }
                else
                {
                    this.RawData.IsAlignmentDone = true;
                }
                this.Log_Global("TestModule_PeaPer 模块运行完成");
            }
            catch (Exception ex)
            {
                //下电
                //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(0.0);
                //this.SourceMeter_Master.IsOutputEnable = false;
                this.SourceMeter_Master.Reset();

                this._core.ReportException("偏振态模块运行错误", ErrorCodes.Module_PER_Failed, ex);
                ///根据模块的影响决定是否往上抛出错误
                ///throw ex;s
            }
        }
        private RawDataumItem_PeaPer MoveAngleMeasurePower(double angle, double offset)
        {

            this.PERAxis.MoveToV3(angle, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
            int err=this.PERAxis.WaitMotionDone();
            if(err==ErrorCodes.MotorMoveTimeOutError)
            {
                this.Log_Global($"轴[{this.PERAxis.Name}]运动超时！");
                throw new Exception(string.Format("轴{0}运动超时！", this.PERAxis.Name));
            }
            //myAxis.WaitForMotionComleted(token); 先不研究这个 ???
            //if (myAxis.WaitForMotionComleted_PosDone(new CancellationToken(), 180) == false)
            //{
            //    throw new Exception(string.Format("轴{0}运动超时！", myAxis.Name));
            //}
            Thread.Sleep(10);
            RawDataumItem_PeaPer rawpp = new RawDataumItem_PeaPer();
            rawpp.Angle_deg = angle - offset;
            //过程中读取功率啥的
            //rawpp.Current_mA = this.SourceMeter_Master.ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL.CH1) * 1000;
            //rawpp.Current_mA = this.SourceMeter_Master.MeasureCurrent_A(Keithley2602BChannel.CHB) * 1000;
            rawpp.Current_mA = this.SourceMeter_Master.ReadCurrent_PD_A() * 1000;
            rawpp.Power_mW = rawpp.Current_mA * this.TestRecipe.Power_Factor_K + this.TestRecipe.Power_Factor_B;
            return rawpp;
        }
    }
}