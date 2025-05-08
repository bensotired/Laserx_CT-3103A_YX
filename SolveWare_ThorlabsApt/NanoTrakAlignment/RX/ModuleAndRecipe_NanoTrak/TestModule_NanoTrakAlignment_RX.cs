using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_NanoTrakAlignment_RX_Power_mW",
        "TestCalculator_NanoTrakAlignment_RX_Current_mA",
        "TestCalculator_NanoTrak_RX_Temperature"
        )]
  
    //[ConfigurableInstrument("Keithley2602B", "Source_2602B", "用于驱动器件")]
    
    [ConfigurableInstrument("Thorlabs_NanoTrak", "NanoTrak_RX", "用于RX端耦合")]

    [StaticResource(ResourceItemType.IO, "OpticaFiberSW_OutPut", "光纤切换继电器")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "用于获取载台当前温度")]

    public class TestModule_NanoTrakAlignment_RX : TestModuleBase
    {
        public TestModule_NanoTrakAlignment_RX() : base() { }
        #region 以get属性获取测试模块运行所需资源
       // Keithley2602B SourceMeter_Master { get { return (Keithley2602B)this.ModuleResource["Source_2602B"]; } }
        Thorlabs_NanoTrak NanoTrak { get { return (Thorlabs_NanoTrak)this.ModuleResource["NanoTrak_RX"]; } }
        IOBase OpticaFiberSW { get { return (IOBase)this.ModuleResource["OpticaFiberSW_OutPut"]; } }
        MeerstetterTECController_1089 TEC { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }

        #endregion

        TestRecipe_NanoTrakAlignment_RX TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_NanoTrakAlignment_RX); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_NanoTrakAlignment_RX>(testRecipe); }
        RawData_NanoTrakAlignment_RX RawData { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_NanoTrakAlignment_RX(); return RawData; }
        public override void Run(CancellationToken token)
        {
            try
            {
                OpticaFiberSW.TurnOn(true);
                this.Log_Global("光IO开关已经为true");
                if (MessageBox.Show("确定已经正确上料到耦合位置?", "开始耦合动作", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Log_Global("开始进行耦合...");
                    ///需根据实际使用条件定制
                    //this.Log_Global($"开始上电...[{this.TestRecipe.DrivingCurrent_mA}]mA...");
                    

                    // SourceMeter_Master.SetupSMU_LD(this.TestRecipe.DrivingCurrent_mA, 2.5); //默认2.5V
                    //OpticaFiberSW.TurnOn(false);
                    int pos_index = 0;
                    List<double> NanoTrakCurrentList = new List<double>() { 0, 0, 0 };
                    bool Alignment_ok = false;
                    double average = 0;
                    double NanoTrakCurrent = 0;
                    //你的耦合动作
                    for (int step_mm = 0; step_mm <= 18; step_mm++)
                    {
                        NanoTrak.Track();

                        NanoTrak.SetCircleDiameter(2.0);
                        Thread.Sleep(200);

                        NanoTrak.SetCircleDiameter(1.0);

                        Thread.Sleep(200);
                        NanoTrak.SetCircleDiameter(0.5);

                        Thread.Sleep(200);
                        NanoTrak.Latch();


                        ///过程中读取功率啥的
                        //var pdCurr = this.SourceMeter_Master.ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL.CH1);
                        //var pdCurr = this.SourceMeter_Master.MeasureCurrent_A(Keithley2602BChannel.CHA);                   
                         NanoTrakCurrent = NanoTrak.ReadCurrent();
                        if (NanoTrakCurrent>this.TestRecipe.Threshold_Factor_Current_mA)
                        {
                            this.Log_Global($"右侧耦合动作完成最终光电流为[{NanoTrakCurrent}]，设定的光电流门槛值为[{this.TestRecipe.Threshold_Factor_Current_mA}]...");
                            break;
                        }
                        //if (step_mm % 3 == 0) NanoTrakCurrentList[0] = NanoTrakCurrent;
                        //if (step_mm % 3 == 1) NanoTrakCurrentList[1] = NanoTrakCurrent;
                        //if (step_mm % 3 == 2) NanoTrakCurrentList[2] = NanoTrakCurrent;

                        //if (step_mm > 1/*&& step_mm % 3 == 0*/)
                        //{
                        //    average = NanoTrakCurrentList.Average();
                        //    if (Math.Abs(average - NanoTrakCurrentList[0]) < this.TestRecipe.Three_Current_mA_Diff
                        //        && Math.Abs(average - NanoTrakCurrentList[1]) < this.TestRecipe.Three_Current_mA_Diff
                        //        && Math.Abs(average - NanoTrakCurrentList[2]) < this.TestRecipe.Three_Current_mA_Diff)
                        //    {
                        //        this.Log_Global($"耦合动作完成最终光电流平均值为[{average}]...");
                        //        Alignment_ok = true;
                        //        break;
                        //    }
                        //    else
                        //        continue;
                        //}
                        //if (NanoTrakCurrent >= this.TestRecipe.Threshold_PD_Current_mA)
                        //{
                        //    this.Log_Global($"耦合动作完成最终光电流为[{NanoTrakCurrent}]...");
                        //    break;
                        //}
                        //this.RawData.Add(new RawDatumItem_NanoTrakAlignment_LX
                        //{
                        //    Position_Index = ++pos_index,
                        //    Position_X_mm = step_mm,
                        //    Position_Y_mm = Math.Round(step_mm * Math.PI, 2),
                        //    Position_Z_mm = Math.Round(step_mm * Math.E, 2),
                        //    Power_mW = NanoTrakCurrent * this.TestRecipe.Power_Factor_K + this.TestRecipe.Power_Factor_B
                        //});
                    }
                    //if (Alignment_ok == false)
                    //{
                    //    throw new Exception($"耦合失败，三次平均值为[{NanoTrakCurrentList.Average()}],设置差值为[{this.TestRecipe.Three_Current_mA_Diff}]轴{0}运动超时！" +
                    //         $"耦合列表值1为[{NanoTrakCurrentList[0]}],耦合列表值2为[{NanoTrakCurrentList[1]}],耦合列表值3为[{NanoTrakCurrentList[2]}],");
                    //}
                    //this.RawData.NanoTrakAverageCurrent_mA = average;
                    //var finalData = this.RawData.DataCollection.Last();
                    //this.RawData.Final_Power_mW = finalData.Power_mW;

                    this.RawData.NanoTrak_RX_Current_mA = NanoTrakCurrent; 
                    // var finalData = this.RawData.DataCollection.Last();
                    this.RawData.NanoTrak_RX_Power_mW = NanoTrakCurrent * this.TestRecipe.Power_Factor_K + this.TestRecipe.Power_Factor_B;
                    //获取温度
                    this.RawData.NanoTrak_RX_Temperature_degC = TEC.CurrentObjectTemperature;
                   // OpticaFiberSW.TurnOn(false);
                    //this.RawData.Final_Position_X_mm = finalData.Position_X_mm;
                    //this.RawData.Final_Position_Y_mm = finalData.Position_Y_mm;
                    //this.RawData.Final_Position_Z_mm = finalData.Position_Z_mm;
                    //下电
                    //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(0.0);
                    //this.SourceMeter_Master.IsOutputEnable = false;
                    //SourceMeter_Master.Reset();

                    if (token.IsCancellationRequested)
                    {
                        this.RawData.IsAlignmentDone = false;
                    }
                    else
                    {
                        this.RawData.IsAlignmentDone = true;

                    }
                    this.Log_Global("TestModule_NanoTrakAlignment_RX 模块运行完成");
                }
                OpticaFiberSW.TurnOn(false);
                this.Log_Global("光IO开关已经为false");
            }
            catch (Exception ex)
            {
                //下电
                //this.SourceMeter_Master.CurrentSetpoint_A = Convert.ToSingle(0.0);
                //this.SourceMeter_Master.IsOutputEnable = false;
                //this.SourceMeter_Master.Reset();
                this._core.ReportException("耦合模块运行错误", ErrorCodes.TestModuleRuntimeExceptionRaised, ex);
                ///根据模块的影响决定是否往上抛出错误
                ///throw ex;s
            }
        }
    }
}