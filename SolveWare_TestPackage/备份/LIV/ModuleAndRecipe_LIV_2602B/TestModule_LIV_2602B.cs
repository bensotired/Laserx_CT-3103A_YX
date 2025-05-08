using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     (
        "TestCalculator_PrintLIVTestParams",
         "TestCalculator_LIV_VF",
         "TestCalculator_LIV_Iop",
         "TestCalculator_LIV_Vop",
         "TestCalculator_LIV_Ith1",
         "TestCalculator_LIV_Ith2",
         "TestCalculator_LIV_Ith3",
         "TestCalculator_LIV_Kink_Current",
         "TestCalculator_LIV_Kink_Percentage",
         "TestCalculator_LIV_Kink_Power",
         "TestCalculator_LIV_Kink1_Percent_zh",
         "TestCalculator_LIV_Kink2_Percent_zh",
         "TestCalculator_LIV_Pout",
         "TestCalculator_LIV_Rs",
         "TestCalculator_LIV_Rs_2Point",
         "TestCalculator_LIV_SE_mWpermA",
         "TestCalculator_LIV_SE_mWpermW",
         "TestCalculator_LIV_Temperature",
         "TestCalculator_LIV_Imax",
         "TestCalculator_LIV_PowerMax",
         "TestCalculator_LIV_SEmax",
         "TestCalculator_LIV_Frequency_Hz",
         "TestCalculator_LIV_DutyCycle",
         "TestCalculator_LIV_SE_Ref_IOP_mWpermA",
         "TestCalculator_LIV_ResDiff_Ref_IOP",
         "TestCalculator_LIV_PCE_Ref_IOP",
         "TestCalculator_LIV_MaxPCE"
       
     // "TestCalculator_LIV_Factor_K",
     // "TestCalculator_LIV_Factor_B",
     //"TestCalculator_LIV_I_Start_mA",
     //"TestCalculator_LIV_I_Stop_mA",
     //"TestCalculator_LIV_I_Step_mA",
     //"TestCalculator_LIV_PDComplianceCurrent_mA",
     //"TestCalculator_LIV_Period_ms",
     //"TestCalculator_LIV_SourceDelay_ms",
     //"TestCalculator_LIV_SenseDelay_ms",
     //"TestCalculator_LIV_NPLC_ms",
     //"TestCalculator_LIV_PulsedMode",
     //"TestCalculator_LIV_pulseWidth_ms"
     )]
    [ConfigurableInstrument("Keithley2602B", "SourceMeter_2602B", "用于LIV扫描")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "用于获取载台当前温度")]


    public class TestModule_LIV_2602B : TestModuleBase
    {

        public TestModule_LIV_2602B() : base()
        {
        }

        Keithley2602B SourceMeter_2602B { get { return (Keithley2602B)this.ModuleResource["SourceMeter_2602B"]; } }
        MeerstetterTECController_1089 TEC { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }

        TestRecipe_LIV_2602B TestRecipe { get; set; }
        RawData_LIV RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV_2602B);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV_2602B>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //根据1100A
                this.Log_Global($"开始测试!");
                

                if (SourceMeter_2602B.IsOnline == false|| SourceMeter_2602B== null)
                {
                    Log_Global($"仪器[{SourceMeter_2602B.Name}]状态为[{SourceMeter_2602B.IsOnline}]");
                    return;
                }
                if (/*TEC.IsOnline == false ||*/ TEC == null)
                {
                    Log_Global($"仪器[{TEC.Name}]状态为[{TEC.IsOnline}]");
                    return;
                }
                if (SourceMeter_2602B.IsOnline)
                {
                    SourceMeter_2602B.Reset();                   
                }
                //this.SourceMeter_GS820.Reset();//下面方法中有

                double startValue = this.TestRecipe.I_Start_mA / 1000;
                double stopValue = this.TestRecipe.I_Stop_mA / 1000;
                double stepValue = this.TestRecipe.I_Step_mA / 1000;
                double complianceVoltage_V = this.TestRecipe.complianceVoltage_V;
                //用万用表确认PD的正负极，如果正极接的源表的Hi负极接Lo，那么要加负电压
                //如果PD正极接源表Lo，PD负极接源表Hi，那么要加正电压
                //zh现场加正电
                if (this.TestRecipe.PDBiasVoltage_V < 0)
                {
                    this.Log_Global($"偏置电压{this.TestRecipe.PDBiasVoltage_V}为负,现场最开始接线为PD正极接源表Lo，PD负极接源表Hi，应该为正偏压");
                    return;
                }
                double PDBiasVoltage_V = this.TestRecipe.PDBiasVoltage_V;
                double PDComplianceCurrent_A = this.TestRecipe.PDComplianceCurrent_mA / 1000;
                double Period_s = this.TestRecipe.Period_ms / 1000;
                double pulseWidth = this.TestRecipe.pulseWidth_ms / 1000;
                double SourceDelay_s = this.TestRecipe.SourceDelay_ms / 1000;
                double SenseDelay_s = this.TestRecipe.SenseDelay_ms / 1000;
                double NPLC_s = this.TestRecipe.NPLC_ms / 20; /// 1000;
                bool PulsedMode = this.TestRecipe.PulsedMode;
                int pulseCount = this.TestRecipe.PulseCount;
                double DutyRatio = double.NaN;
                int sweepPoints =Convert.ToInt16((this.TestRecipe.I_Stop_mA - this.TestRecipe.I_Start_mA) / this.TestRecipe.I_Step_mA)+1;
                this.Log_Global($"Start TestModule_LIV_2602B sweep sweep ...{this.TestRecipe.I_Start_mA}~{this.TestRecipe.I_Stop_mA}mA " +
                                      $"step  {this.TestRecipe.I_Step_mA}mA");
                //PD校准系数 应该从主配置文件中导入的*************
                double SphereResponsivity = this.TestRecipe.Power_Factor_K;
                double PowerOffset_mW = this.TestRecipe.Power_Factor_B;

                if (PulsedMode)
                {
                    this.Log_Global($"Start TestModule_LIV_2602B Pluse sweep ...pulseCount [{pulseCount}]");
                    for (int i = 0; i < sweepPoints; i++)
                    {
                        double smua_pulseLevel = i * stepValue + startValue;
                        var cRaw = SourceMeter_2602B.PulseTrainSyncSampling(
                                                            smua_pulseLevel,//电流点
                                                            complianceVoltage_V,//配置参数的限压 2.5
                                                            PDBiasVoltage_V,
                                                            PDComplianceCurrent_A,
                                                            SenseDelay_s,//粗扫测量延迟秒    默认值0.001
                                                            NPLC_s,//粗略扫描NPLC  默认是0.02秒一个扫描周期   默认值0.001
                                                            pulseCount,
                                                            pulseWidth,
                                                            Period_s);
                        var sour1curr =cRaw[0].Split(',');
                        var sour1volt = cRaw[1].Split(',');
                        var sour2curr = cRaw[2].Split(',');
                        var sour2volt = cRaw[3].Split(',');

                        List<double> _sour1curr = new List<double>();
                        List<double> _sour1volt = new List<double>();
                        List<double> _sour2curr = new List<double>();
                        List<double> _sour2volt = new List<double>();
                        if (sour1curr.Length == sour1volt.Length && sour1volt.Length == sour2curr.Length && sour2curr.Length == sour2volt.Length)
                        {
                            for (int j = 0; j < sour1curr.Length; j++)
                            {
                                _sour1curr.Add(Convert.ToDouble(sour1curr[j]));
                                _sour1volt.Add(Convert.ToDouble(sour1volt[j]));
                                _sour2curr.Add(Convert.ToDouble(sour2curr[j]));
                                _sour2volt.Add(Convert.ToDouble(sour2volt[j]));
                            }

                            double _pce = 0;
                            if (i < this.TestRecipe.PCESort)
                            {
                                _pce = 0;
                            }
                            else
                            {
                                _pce = ((CalculateAVG(_sour2curr) * 1000 * SphereResponsivity + PowerOffset_mW) / CalculateAVG(_sour1volt) /
                                        (i * this.TestRecipe.I_Step_mA + this.TestRecipe.I_Start_mA)) * 100;  //*100变成% 
                            }

                            RawData.Add(new RawDatumItem_LIV()
                            {
                                // Current_mA = Math.Round(Convert.ToDouble(sour1curr[i]) * 1000,2),
                                Current_mA = i * this.TestRecipe.I_Step_mA + this.TestRecipe.I_Start_mA,
                                Voltage_V = CalculateAVG(_sour1volt),
                                Power_mW = CalculateAVG(_sour2curr) * 1000 * SphereResponsivity + PowerOffset_mW,
                                PDCurrent_mA = CalculateAVG(_sour2curr) * 1000,
                                PDVoltage_V = CalculateAVG(_sour2volt),
                                //PCE = ((CalculateAVG(_sour2curr) * 1000 * SphereResponsivity + PowerOffset_mW) / CalculateAVG(_sour1volt) / 
                                //       (i * this.TestRecipe.I_Step_mA + this.TestRecipe.I_Start_mA)) * 100  //*100变成% 
                                PCE = _pce
                            });                           
                        }
                        else
                        {
                            this.Log_Global($" LIV Pulse扫描电流点[{smua_pulseLevel}]回读的点数不对，请检查" +
                                                                   $"\r\nsour1curr.Length == [{sour1curr.Length}]" +
                                                                   $"\r\nsour1volt.Length==[{sour1volt.Length}]" +
                                                                   $"\r\nsour2curr.Length==[{sour2curr.Length}]" +
                                                                   $"\r\nsour2volt.Length==[{sour2volt.Length}]" );
                        }
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                } 
                else
                {
                    var cRaw = SourceMeter_2602B.SweepDualChannelsUsingTimer
                                                         (startValue,//配置参数的开始电流 0.001
                                                          stopValue,//配置参数的结束电流   1.2
                                                          complianceVoltage_V,//配置参数的限压 2.5
                                                          PDBiasVoltage_V,
                                                          PDComplianceCurrent_A,
                                                          SourceDelay_s, //粗扫源延迟秒      默认值0.001
                                                          SenseDelay_s,//粗扫测量延迟秒    默认值0.001
                                                          NPLC_s,//粗略扫描NPLC  默认是0.02秒一个扫描周期   默认值0.001
                                                          sweepPoints,
                                                          PulsedMode,
                                                          pulseWidth,
                                                          Period_s);//粗扫定时器周期秒    默认值0.005


                    this.Log_Global("TestModule_LIV_2602B CW sweep finished..");

                    this.SourceMeter_2602B.Reset();
                    var sour1curr = cRaw[0].Split(',');
                    var sour1volt = cRaw[1].Split(',');
                    var sour2curr = cRaw[2].Split(',');
                    var sour2volt = cRaw[3].Split(',');
                                                           

                    if (sour1curr.Length == sour1volt.Length && sour1curr.Length == sour2curr.Length && sour1curr.Length == sour2volt.Length && sweepPoints == sour1curr.Length)
                    {
                        var len = sour1curr.Length;
                        for (int i = 0; i < len; i++)
                        {
                            double _pce = 0;
                            if (i<this.TestRecipe.PCESort)
                            {
                                _pce = 0;
                            }
                            else
                            {
                                _pce = (Convert.ToDouble(sour2curr[i]) * 1000 * SphereResponsivity + PowerOffset_mW) / Convert.ToDouble(sour1volt[i]) /
                                      (i * this.TestRecipe.I_Step_mA + this.TestRecipe.I_Start_mA) * 100; //*100变成%
                            }
                            RawData.Add(new RawDatumItem_LIV()
                            {
                                // Current_mA = Math.Round(Convert.ToDouble(sour1curr[i]) * 1000,2),
                                Current_mA = i * this.TestRecipe.I_Step_mA + this.TestRecipe.I_Start_mA,
                                Power_mW = (Convert.ToDouble(sour2curr[i]) * 1000 * SphereResponsivity + PowerOffset_mW),
                                Voltage_V = Convert.ToDouble(sour1volt[i]),
                                PDCurrent_mA = Convert.ToDouble(sour2curr[i]) * 1000,
                                PDVoltage_V = Convert.ToDouble(sour2volt[i]),
                                PCE = _pce
                                //PCE = (Convert.ToDouble(sour2curr[i]) * 1000 * SphereResponsivity + PowerOffset_mW) / Convert.ToDouble(sour1volt[i]) / 
                                //  (i * this.TestRecipe.I_Step_mA + this.TestRecipe.I_Start_mA) * 100  //*100变成%
                                //PCE = Power_mW / Voltage_V/ Current_mA//
                            });
                        }
                    }
                    else
                    {
                        this.Log_Global($" LIV扫描的点数不对，请检查" +
                                        $"\r\nsour1curr.Length == [{sour1curr.Length}]" +
                                        $"\r\nsour1volt.Length==[{sour1volt.Length}]" +
                                        $"\r\nsour2curr.Length==[{sour2curr.Length}]" +
                                        $"\r\nsour2volt.Length==[{sour2volt.Length}]" +
                                        $"\r\nsweepPoints==[{sweepPoints}]");
                    }
                }
               


                RawData.I_Start_mA = this.TestRecipe.I_Start_mA;
                RawData.I_Stop_mA = this.TestRecipe.I_Stop_mA;
                RawData.I_Step_mA = this.TestRecipe.I_Step_mA;
                RawData.PDComplianceCurrent_mA = this.TestRecipe.PDComplianceCurrent_mA;
                RawData.Period_ms = this.TestRecipe.Period_ms;
                RawData.pulseWidth_ms = this.TestRecipe.pulseWidth_ms;
                RawData.SourceDelay_ms = this.TestRecipe.SourceDelay_ms;
                RawData.SenseDelay_ms = this.TestRecipe.SenseDelay_ms;
                RawData.NPLC_ms = this.TestRecipe.NPLC_ms;
                RawData.PulsedMode = this.TestRecipe.PulsedMode;
                RawData.PulseCount = this.TestRecipe.PulseCount;
                RawData.LIV_Factor_K = this.TestRecipe.Power_Factor_K;
                RawData.LIV_Factor_B = this.TestRecipe.Power_Factor_B;
                //做完后必须要reset 带电操作会不好
                if (SourceMeter_2602B.IsOnline)
                {
                    SourceMeter_2602B.Reset();                
                }
                //获取温度
                RawData.LIV_Temperature_degC = TEC.CurrentObjectTemperature;

                this.Log_Global("TestModule_LIV_2602B 模块运行完成");
            }
            catch (Exception ex)
            {
                if (SourceMeter_2602B.IsOnline)
                {
                    SourceMeter_2602B.Reset();
                }
                this._core.ReportException("LIV模块运行错误", ErrorCodes.Module_LIV_Failed, ex);
            }
        }
        private double CalculateAVG(List<double> DataList) //去掉一个最大值，去掉一个最小值，取平均
        {
            double ret = 0;
            try
            {
                if (DataList.Count>2)
                {
                    double Min = DataList.Min();
                    double Max = DataList.Max();
                    double Sum = DataList.Sum();
                    //去除最大值和最小值的和值
                    double sumWithoutMaxAndMin = Sum - Max - Min;
                    //去除最大值和最小值的和值的平均值
                    double avg = sumWithoutMaxAndMin / (DataList.Count - 2);
                    ret = avg;
                    return ret;
                }
                else
                {
                    this.Log_Global($"TestModule_LIV_2602B Ppulse 数组数量为[{DataList.Count}],要大于3才能计算");
                    return ret;
                }
              
            }
            catch (Exception ex)
            {
                return ret;
                throw new Exception($"TestModule_LIV_2602B Ppulse 数组计算平均值异常");
            }
        }
    }
}