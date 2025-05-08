using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_AlignmentDemo")]

    [ConfigurableInstrument("ISourceMeter_Golight", "SourceMeter_Master", "用于驱动器件")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TEC", "用于控温")]
    [ConfigurableInstrument("IDigitalIOController", "IO", "用于控制通道")]

    public class TestModule_AlignmentDemo : TestModuleBase
    {
        public TestModule_AlignmentDemo() : base() { }
        #region 以get属性获取测试模块运行所需资源
        ISourceMeter_Golight SourceMeter_Master { get { return (ISourceMeter_Golight)this.ModuleResource["SourceMeter_Master"]; } }
        MeerstetterTECController_1089 TEC_Collection { get { return (MeerstetterTECController_1089)this.ModuleResource["TEC"]; } }
        IDigitalIOController IO_Collection { get { return (IDigitalIOController)this.ModuleResource["IO"]; } }
        #endregion

        TestRecipe_AlignmentDemo TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_AlignmentDemo); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_AlignmentDemo>(testRecipe); }
        RawData_AlignmentDemo RawData { get; set; }
        RawDataMenu_Demo RawDataMenu { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawDataMenu = new RawDataMenu_Demo(); return RawDataMenu; }

        public override void Run(CancellationToken token)
        {
            try
            {
                List<ushort[]> vs = new List<ushort[]>();
                vs.Add(new ushort[] { 00 });
                vs.Add(new ushort[] { 01 });
                vs.Add(new ushort[] { 10 });
                vs.Add(new ushort[] { 11 });

                this.Log_Global($"开始测试!");

                //if (IO_Collection.IsOnline)
                //{
                //    this._core.Log_Global("IO连接错误！");
                //    return;
                //}


                if (SourceMeter_Master == null | TEC_Collection == null)
                {
                    this._core.Log_Global("仪器/TEC连接错误！");
                    return;
                }

                if (!SourceMeter_Master.IsOnline)
                {
                    this.Log_Global("仪表异常，取消测试！");
                    SourceMeter_Master.Timeout_ms = 1000;
                    SourceMeter_Master.CurrentSetpoint_A = 0;
                    SourceMeter_Master.VoltageSetpoint_V = 0;
                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    SourceMeter_Master.IsOutputEnable = false;
                    TEC_Collection.IsOutputEnabled = false;
                    return;
                }
                SourceMeter_Master.Timeout_ms = 60 * 1000;
                //var pd_1_range = SourceMeter_Master.GetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1);


                //SourceMeter_Master.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range2);
                //var pd_1_range = SourceMeter_Master.GetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1);


                TEC_Collection.TemperatureSetPoint_DegreeC = this.TestRecipe.Temp;
                TEC_Collection.IsOutputEnabled = true;
                this.Log_Global($"开始控温!");
                while (Math.Abs(TEC_Collection.CurrentObjectTemperature - this.TestRecipe.Temp) > 0.5)
                {
                    Thread.Sleep(1000);
                }
                //等待时间
                Thread.Sleep(this.TestRecipe.TempWait * 1000);

                if (this.TestRecipe.convert)
                {
                    //扫描
                    for (int j = 1; j <= 4; j++)
                    {

                        //用来切换通道
                        TemporarilySetChannel(j);
                        this.Log_Global($"通道[{j}]...");
                        Thread.Sleep(this.TestRecipe.ChWait);
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global("取消测试！");
                            SourceMeter_Master.Timeout_ms = 1000;
                            SourceMeter_Master.CurrentSetpoint_A = 0;
                            SourceMeter_Master.VoltageSetpoint_V = 0;
                            SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                            SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                            SourceMeter_Master.IsOutputEnable = false;
                            Thread.Sleep(3000);
                            TEC_Collection.IsOutputEnabled = false;
                            return;
                        }
                        this.RawData = new RawData_AlignmentDemo();
                        this.RawData.TestStepStartTime = DateTime.Now;
                        //建立数组表
                        int sweepPoints = Convert.ToInt32((this.TestRecipe.StopCurrent_mA - this.TestRecipe.StartCurrent_mA) / this.TestRecipe.StepCurrent_mA) + 1;
                        float[] ldCurrs_mA = new float[sweepPoints];
                        float[] ldVolt_V = new float[sweepPoints];
                        float[] pdCurrs_mA = new float[sweepPoints];
                        float[] mpdCurrs_mA = new float[sweepPoints];
                        float[] eaCurrs_mA = new float[sweepPoints];
                        List<double> temperature = new List<double>();

                        this.Log_Global($"Start LIV sweep ...{this.TestRecipe.StartCurrent_mA}~{this.TestRecipe.StopCurrent_mA}mA step {this.TestRecipe.StepCurrent_mA}mA ");

                        //this.SourceMeter_Master.IsOutputEnable = true;
                        bool Temp = true;

                        Parallel.Invoke(
                        () =>
                        {
                            while (Temp)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    this.Log_Global("取消测试！");
                                    SourceMeter_Master.Timeout_ms = 1000;
                                    SourceMeter_Master.CurrentSetpoint_A = 0;
                                    SourceMeter_Master.VoltageSetpoint_V = 0;
                                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                                    SourceMeter_Master.IsOutputEnable = false;
                                    Thread.Sleep(3000);
                                    TEC_Collection.IsOutputEnabled = false;
                                    break;
                                }
                                //temperature.Add(TEC_Collection.CurrentObjectTemperature);
                                Thread.Sleep(500);
                                temperature.Add(0);
                            }
                        },
                        () =>
                        {
                            SourceMeter_Master.Sweep_LD_PD(Convert.ToSingle(this.TestRecipe.StartCurrent_mA),
                                              Convert.ToSingle(this.TestRecipe.StepCurrent_mA),
                                              Convert.ToSingle(this.TestRecipe.StopCurrent_mA),
                                              Convert.ToSingle(this.TestRecipe.CompliaceVoltage_V),
                                              Convert.ToSingle(this.TestRecipe.PdBiasVoltage_V), //PD偏置电压, 默认0V
                                              Convert.ToSingle(this.TestRecipe.PdComplianceCurrent_mA),
                                              this.TestRecipe.K2400_NPLC);
                            Temp = false;
                        });



                        while (SourceMeter_Master.IsSweeping)
                        {
                            Thread.Sleep(100);
                        }
                        this.Log_Global("Fetch LIV sweep data.");
                        ldCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Current_mA);
                        ldVolt_V = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Voltage_V);
                        pdCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.EA_Drive_Voltage_V);





                        if (!SourceMeter_Master.IsOnline)
                        {
                            this.Log_Global("仪表异常，取消测试！");
                            SourceMeter_Master.Timeout_ms = 1000;
                            SourceMeter_Master.CurrentSetpoint_A = 0;
                            SourceMeter_Master.VoltageSetpoint_V = 0;
                            SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                            SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                            SourceMeter_Master.IsOutputEnable = false;
                            Thread.Sleep(3000);
                            TEC_Collection.IsOutputEnabled = false;
                            return;
                        }
                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global("取消测试！");
                            SourceMeter_Master.Timeout_ms = 1000;
                            SourceMeter_Master.CurrentSetpoint_A = 0;
                            SourceMeter_Master.VoltageSetpoint_V = 0;
                            SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                            SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                            SourceMeter_Master.IsOutputEnable = false;
                            Thread.Sleep(3000);
                            TEC_Collection.IsOutputEnabled = false;
                            return;
                        }
                        this.Log_Global("LIV sweep finished..");

                        double SphereResponsivity = 1;
                        double PowerOffset_mW = 0;


                        for (int i = 0; i < ldCurrs_mA.Length; i++)
                        {
                            if (i < temperature.Count)
                            {
                                this.RawData.Add(new RawDatumItem_AlignmentDemo()
                                {
                                    Current_mA = ldCurrs_mA[i],
                                    Voltage_V = ldVolt_V[i],
                                    Power_mW = (pdCurrs_mA[i] * SphereResponsivity + PowerOffset_mW),
                                    Temperature = temperature[i]

                                });
                            }
                            else
                            {
                                this.RawData.Add(new RawDatumItem_AlignmentDemo()
                                {
                                    Current_mA = ldCurrs_mA[i],
                                    Voltage_V = ldVolt_V[i],
                                    Power_mW = (pdCurrs_mA[i] * SphereResponsivity + PowerOffset_mW),

                                });
                            }

                        }
                        this.RawData.IO_Channel = j;
                        this.RawData.TestStepEndTime = DateTime.Now;
                        this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                        this.RawDataMenu.Add(this.RawData);

                    }

                }
                else
                {
                    //string driver = this.TestRecipe.DrivingCurrent_mA;
                    //string[] Driver_I = driver.Split(",".ToCharArray());
                    float startc = Convert.ToSingle(this.TestRecipe.StartCurrent_mA);
                    float stepc = Convert.ToSingle(this.TestRecipe.StepCurrent_mA);
                    float stopc = Convert.ToSingle(this.TestRecipe.StopCurrent_mA);

                    for (int n = 1; n <= 4; n++)
                    {
                        TemporarilySetChannel(n);
                        //用来切换通道
                        this.Log_Global($"通道[{n}]...");
                        Thread.Sleep(this.TestRecipe.ChWait);


                        List<float> ldCurrs_mA = new List<float>();
                        List<float> ldVolt_V = new List<float>();
                        List<float> pdCurrs_mA = new List<float>();
                        List<double> temperature = new List<double>();


                        for (float i = startc; i <= stopc; i += stepc)
                        {
                            if (!SourceMeter_Master.IsOnline)
                            {
                                this.Log_Global("仪表异常，取消测试！");
                                SourceMeter_Master.Timeout_ms = 1000;
                                SourceMeter_Master.CurrentSetpoint_A = 0;
                                SourceMeter_Master.VoltageSetpoint_V = 0;
                                SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                                SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                                SourceMeter_Master.IsOutputEnable = false;
                                Thread.Sleep(3000);
                                TEC_Collection.IsOutputEnabled = false;
                                return;
                            }
                            if (token.IsCancellationRequested)
                            {
                                this.Log_Global("取消测试！");
                                SourceMeter_Master.Timeout_ms = 1000;
                                SourceMeter_Master.CurrentSetpoint_A = 0;
                                SourceMeter_Master.VoltageSetpoint_V = 0;
                                SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                                SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                                SourceMeter_Master.IsOutputEnable = false;
                                Thread.Sleep(3000);
                                TEC_Collection.IsOutputEnabled = false;
                                return;
                            }
                            this.Log_Global($"开始上电...[{i}]mA...");

                            this.RawData = new RawData_AlignmentDemo();
                            this.RawData.TestStepStartTime = DateTime.Now;

                            this.SourceMeter_Master.IsOutputEnable = true;
                            this.SourceMeter_Master.CurrentSetpoint_A = i / 1000;

                            Thread.Sleep(250);

                            #region 5次取平均
                            //float vsc = 0;
                            //float vsv = 0;
                            //float vspdc = 0;
                            //for (int m = 0; m < 5; m++)
                            //{
                            //    vsc += Convert.ToSingle(this.SourceMeter_Master.ReadCurrent_A());
                            //    vsv += Convert.ToSingle(this.SourceMeter_Master.ReadVoltage_V());
                            //    vspdc += this.SourceMeter_Master.ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL.CH1);
                            //}
                            //ldCurrs_mA.Add(vsc / 5);
                            //ldVolt_V.Add(vsv / 5);
                            //pdCurrs_mA.Add(vspdc / 5);
                            #endregion

                            ldCurrs_mA.Add(Convert.ToSingle(this.SourceMeter_Master.ReadCurrent_A()));
                            ldVolt_V.Add(Convert.ToSingle(this.SourceMeter_Master.ReadVoltage_V()));
                            pdCurrs_mA.Add(this.SourceMeter_Master.ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL.CH1));

                            //var T = 0;

                            var T = TEC_Collection.CurrentObjectTemperature;
                            if (T > this.TestRecipe.UpperTemp)
                            {
                                int count1 = 0;
                                while (true)
                                {
                                    T = TEC_Collection.CurrentObjectTemperature;
                                    if (T <= this.TestRecipe.UpperTemp)
                                    {
                                        count1++;
                                    }
                                    if (count1 >= 4)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(500);
                                }
                            }

                            temperature.Add(T);
                        }

                        double SphereResponsivity = 1;
                        double PowerOffset_mW = 0;
                        for (int m = 0; m < ldCurrs_mA.Count; m++)
                        {
                            this.RawData.Add(new RawDatumItem_AlignmentDemo()
                            {
                                Current_mA = ldCurrs_mA[m],
                                Voltage_V = ldVolt_V[m],
                                Power_mW = (pdCurrs_mA[m] * SphereResponsivity + PowerOffset_mW),
                                Temperature = temperature[m]
                            });
                        }
                        this.RawData.IO_Channel = n;
                        this.RawData.TestStepEndTime = DateTime.Now;
                        this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                        this.RawDataMenu.Add(RawData);

                    }
                }






                SourceMeter_Master.IsOutputEnable = false;
                Thread.Sleep(3000);
                TEC_Collection.IsOutputEnabled = false;
                this.Log_Global("测试结束！");
            }
            catch (Exception ex)
            {
                this._core.ReportException("运行错误", ErrorCodes.TestModuleRuntimeExceptionRaised, ex);
                ///根据模块的影响决定是否往上抛出错误
                ///throw ex;s
            }
        }
        public void TemporarilySetChannel(int channel)
        {
            switch (channel)
            {
                case 1:
                    IO_Collection.OffChannel(0);
                    Thread.Sleep(100);
                    IO_Collection.OffChannel(1);
                    break;
                case 2:
                    IO_Collection.OffChannel(0);
                    Thread.Sleep(100);
                    IO_Collection.OnChannel(1);
                    break;
                case 3:
                    IO_Collection.OnChannel(0);
                    Thread.Sleep(100);
                    IO_Collection.OffChannel(1);
                    break;
                case 4:
                    IO_Collection.OnChannel(0);
                    Thread.Sleep(100);
                    IO_Collection.OnChannel(1);
                    break;
                default:
                    break;
            }
        }
    }
}