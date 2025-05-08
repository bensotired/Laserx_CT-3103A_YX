using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public static class Merged_KeithleyPulser
    {
        static float range1_A = 20 / 1000.0f;
        static float range2_A = 80 / 1000.0f / 1000.0f;
        static double[] _last_sweepData_EA_Currents { get; set; }
        static double[] _last_sweepData_EA_Voltages { get; set; }
        static double[] _last_sweepData_LD_Sense_Currents { get; set; }
        static double[] _last_sweepData_LD_Currents { get; set; }
        static double[] _last_sweepData_LD_Voltages { get; set; }
        static double[] _last_sweepData_PD_Currents { get; set; }
        static Keithley2601B_PULSE SourceLd { get; set; }//Keithley2601B_PULSE
        static Keithley2602B SourceEa { get; set; }//Keithley2602B
        static KeithleyDMM6500 SourcePd { get; set; }//KeithleyDMM6500
        public static void Merged_Keithley(Keithley2601B_PULSE LD, Keithley2602B EA, KeithleyDMM6500 PD)
        {
            SourceLd = LD;
            SourceEa = EA;
            SourcePd = PD;
        }
        public static void StopPulseTrain()
        {
            SourceLd.Reset();
        }

        public static void Sweep_LD_EA_PD(
            float startCurrent_mA, float stepCurrent_mA, float endCurrent_mA,
            float ldVoltageUpperLimit_V,
            float eaVoltage_V, float eaComplianceCurrent_mA,    //EA
            float pdBiasVoltage_V, float pdCurrentRange_mA,
            double period_ms)
        {
            if (!SourceLd.IsOnline || !SourceEa.IsOnline || !SourcePd.IsOnline)
            {
                return;
            }
            //PD档位
            double tpdCurrentRange_mA = pdCurrentRange_mA;

            bool PDInRange = true;

            int TestCount = 3;

            do
            {
                PDInRange = true;
                if (float.IsNaN(eaVoltage_V) == false)
                {
                    return;
                }

                //CW扫描,每个台阶至少应该有0.5ms
                if (period_ms < 0.5) period_ms = 0.5;
                #region 检查参数是否正确
                double measureDelay_s = 50E-6;

                int pdSamplesPerStep = Convert.ToInt32(period_ms * 0.6 / 1000.0 * 1000000); //采集整个脉宽范围内的数据，后续处理
                if (pdSamplesPerStep > 100) pdSamplesPerStep = 100;
                #endregion


                _last_sweepData_LD_Currents = new double[0];
                _last_sweepData_LD_Voltages = new double[0];
                _last_sweepData_EA_Currents = new double[0];
                _last_sweepData_EA_Voltages = new double[0];
                _last_sweepData_PD_Currents = new double[0];
                _last_sweepData_LD_Sense_Currents = new double[0];

                if (float.IsNaN(eaVoltage_V) == false)
                {
                    #region Set EA
                    SourceEa.ResetTriggerLine(TriggerLine.Line14);
                    SourceEa.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceVoltageSenceCurrent);
                    SourceEa.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, true);
                    SourceEa.SetVoltage_V(Keithley2602BChannel.CHA, eaVoltage_V);
                    SourceEa.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, true);
                    SourceEa.SetComplianceCurrent_A(Keithley2602BChannel.CHA, eaComplianceCurrent_mA / 1000.0);
                    SourceEa.SetIsOutputOn(Keithley2602BChannel.CHA, true);
                    #endregion
                }
                TriggerLine TrigOutLine = TriggerLine.Line14;
                double[] ldDrivingCurrs = ArrayMath.CalculateArray(startCurrent_mA / 1000.0, endCurrent_mA / 1000.0, stepCurrent_mA / 1000.0);
                int steps = ldDrivingCurrs.Length;
                int dataCount = steps * pdSamplesPerStep;

                SourcePd.AbortTriggerModel();
                SourcePd.SetBufferSize(dataCount);


                double pdRealRange_A = SourcePd.SetTriggeredDigitizeCurrent(tpdCurrentRange_mA / 1000.0, steps, pdSamplesPerStep, TriggerLine.Line1);

                SourcePd.EnableScreen(false);
                SourcePd.InitiateTriggerModel();

                Thread.Sleep(100);

                //这个复位还是 比较重要
                SourceLd.Reset(Keithley2602BChannel.CHA);
                Thread.Sleep(2000);

                var ldVals = SourceLd.SingleChannelSweepOutputTrigger
                 (
                    Keithley2602BChannel.CHA,
                    SourceMeterMode.SourceCurrentSenceVoltage,
                    startCurrent_mA / 1000.0,
                    endCurrent_mA / 1000.0,
                    steps,
                    ldVoltageUpperLimit_V,
                    period_ms / 1000.0,
                    TrigOutLine,
                    measureDelay_s
                 );
                if (float.IsNaN(eaVoltage_V) == false)
                {
                    SourceEa.SetIsOutputOn(Keithley2602BChannel.CHA, false);
                }
                var pdVals = SourcePd.ReadData(dataCount).ToList();

                SourcePd.AbortTriggerModel();
                SourcePd.EnableScreen(true);

                const int currArrayIndex = 0;
                const int voltArrayIndex = 1;

                _last_sweepData_LD_Currents = ldDrivingCurrs;
                _last_sweepData_LD_Voltages = ldVals[voltArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
                _last_sweepData_LD_Sense_Currents = ldVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();

                int pdReadingSkipPoints = 8;
                List<double> sorted_dmmData = new List<double>();
                for (int i = 0; i < steps; i++)
                {
                    var sumList = pdVals.Skip(i * pdSamplesPerStep).Skip(pdReadingSkipPoints).Take(pdSamplesPerStep - pdReadingSkipPoints);
                    //如果超量程, 就控制到量程 pdRealRange_A
                    double tRange = Math.Abs(pdRealRange_A * 1.2);  //允许超20%量程
                    double maxval = sumList.Max();
                    double minval = sumList.Min();

                    if (maxval > tRange)
                    {
                        sorted_dmmData.Add(tRange);
                        PDInRange = false;
                    }
                    else if (minval < -tRange)
                    {
                        sorted_dmmData.Add(-tRange);
                        PDInRange = false;
                    }
                    else
                    {
                        sorted_dmmData.Add(sumList.Average());
                    }
                }
                _last_sweepData_PD_Currents = sorted_dmmData.ToArray();// pdVals.ToArray();

                if (float.IsNaN(eaVoltage_V) == false)
                {
                    List<double> eaVolts = new List<double>();
                    for (int i = 0; i < ldDrivingCurrs.Length; i++)
                    {
                        eaVolts.Add(eaVoltage_V);
                    }
                    _last_sweepData_EA_Voltages = eaVolts.ToArray();
                }

                if (TestCount-- < 0)
                {
                    string str = ("Sweep_LD_EA_PD 测试PD范围超过量程");
                    break;
                }

                if (PDInRange == false)
                {
                    tpdCurrentRange_mA *= 10;
                }

            } while (PDInRange == false);

        }

        public static void Sweep_LD_MPD_PD(float startCurrent_mA, float stepCurrent_mA, float endCurrent_mA, float ldVoltageUpperLimit_V, float mpdVoltage_V, float mpdComplianceCurrent_mA, float pdBiasVoltage_V, float pdCurrentRange_A, double period_s)
        {
            if (!SourceLd.IsOnline || !SourceEa.IsOnline || !SourcePd.IsOnline)
            {
                return;
            }
            _last_sweepData_LD_Currents = new double[0];
            _last_sweepData_LD_Voltages = new double[0];
            _last_sweepData_EA_Currents = new double[0];
            _last_sweepData_EA_Voltages = new double[0];
            _last_sweepData_PD_Currents = new double[0];
            _last_sweepData_LD_Sense_Currents = new double[0];

            double[] ldDrivingCurrs = ArrayMath.CalculateArray(startCurrent_mA, endCurrent_mA, stepCurrent_mA);
            int steps = ldDrivingCurrs.Length;

            SourceLd.Reset();
            SourceEa.Reset();

            double measureDelay_s = 50E-6;

            double nplc = (period_s - measureDelay_s) * 0.8 / 0.02;

            SourcePd.SetTriggeredMeasureCurrent(pdCurrentRange_A, steps, nplc, TriggerLine.Line1);
            SourcePd.InitiateTriggerModel();

            //这个复位还是 比较重要
            SourceLd.Reset(Keithley2602BChannel.CHA);
            Thread.Sleep(2000);

            var ldVals = SourceLd.SingleChannelSweepOutputTrigger
             (
                Keithley2602BChannel.CHA,
                SourceMeterMode.SourceCurrentSenceVoltage,
                startCurrent_mA,
                endCurrent_mA,
                steps,
                ldVoltageUpperLimit_V,
                period_s,
                TriggerLine.Line14,
                measureDelay_s
             );

            var pdVals = SourcePd.ReadData(steps).ToList();
            const int currArrayIndex = 0;
            const int voltArrayIndex = 1;

            _last_sweepData_LD_Currents = ldDrivingCurrs;
            _last_sweepData_LD_Voltages = ldVals[voltArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
            _last_sweepData_LD_Sense_Currents = ldVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
            _last_sweepData_PD_Currents = pdVals.ToArray();




        }

        public static void Sweep_LD_PD(
            float startCurrent_mA, float stepCurrent_mA, float endCurrent_mA,
            float ldVoltageUpperLimit_V,
            float pdBiasVoltage_V, float pdCurrentRange_mA,
            double period_ms)
        {
            Sweep_LD_EA_PD(
            startCurrent_mA, stepCurrent_mA, endCurrent_mA,
            ldVoltageUpperLimit_V,
            float.NaN, 0,    //EA
            pdBiasVoltage_V, pdCurrentRange_mA,
            period_ms);
        }

        public static void Sweep_Pulse_LD_EA_PD(
            float startCurrent_mA, float stepCurrent_mA, float endCurrent_mA,
            float ldVoltageUpperLimit_V,
           float eaVoltage_V, float eaComplianceCurrent_mA,     //EA
           float pdBiasVoltage_V, float pdCurrentRange_mA,
           double apertureTime_ms, double measureDelay_ms, double period_ms, double duty_rate_percent)
        {
            if (!SourceLd.IsOnline || !SourceEa.IsOnline || !SourcePd.IsOnline)
            {
                return;
            }
            //PD档位
            double tpdCurrentRange_mA = pdCurrentRange_mA;

            bool PDInRange = true;

            int TestCount = 3;

            do
            {
                PDInRange = true;

                if (float.IsNaN(eaVoltage_V) == false)
                {
                    return;
                }

                #region 检查参数是否正确
                double pulseWidth_ms = period_ms * (duty_rate_percent / 100.0);
                if (apertureTime_ms + measureDelay_ms > pulseWidth_ms) throw new Exception("脉冲参数配置错误");
                if (measureDelay_ms == 0) throw new Exception("Measure delay 不能为0");
                //按照周期采样, 最大采样100us
                int period_ms_pdSamplesPerStep = Convert.ToInt32(period_ms * 0.7 / 1000.0 * 1000000); //采集整个脉宽范围内的数据，后续处理
                if (period_ms_pdSamplesPerStep > 100) period_ms_pdSamplesPerStep = 100;
                //如果脉宽采样, 计算出需要的点
                int pulseWidth_pdSamplesPerStep = Convert.ToInt32(pulseWidth_ms / 1000.0 * 1000000); //采集整个脉宽范围内的数据，后续处理
                if (pulseWidth_pdSamplesPerStep > 100) pulseWidth_pdSamplesPerStep = 100;
                #endregion

                _last_sweepData_LD_Currents = new double[0];
                _last_sweepData_LD_Voltages = new double[0];
                _last_sweepData_EA_Currents = new double[0];
                _last_sweepData_EA_Voltages = new double[0];
                _last_sweepData_PD_Currents = new double[0];
                _last_sweepData_LD_Sense_Currents = new double[0];

                SourceLd.Reset();
                SourceEa.Reset();

                if (float.IsNaN(eaVoltage_V) == false)
                {
                    #region Set EA
                    SourceEa.ResetTriggerLine(TriggerLine.Line14);
                    SourceEa.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceVoltageSenceCurrent);
                    SourceEa.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, true);
                    SourceEa.SetVoltage_V(Keithley2602BChannel.CHA, eaVoltage_V);
                    SourceEa.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, true);
                    SourceEa.SetComplianceCurrent_A(Keithley2602BChannel.CHA, eaComplianceCurrent_mA / 1000.0);
                    SourceEa.SetIsOutputOn(Keithley2602BChannel.CHA, true);
                    #endregion
                }
                TriggerLine TrigOutLine = TriggerLine.Line14;
                double[] ldDrivingCurrs = ArrayMath.CalculateArray(startCurrent_mA / 1000.0, endCurrent_mA / 1000.0, stepCurrent_mA / 1000.0);
                int steps = ldDrivingCurrs.Length;
                int dataCount = steps * period_ms_pdSamplesPerStep;

                SourcePd.SetBufferSize(dataCount);

                SourcePd.AbortTriggerModel();
                double pdRealRange_A = SourcePd.SetTriggeredDigitizeCurrent(tpdCurrentRange_mA / 1000.0, steps, period_ms_pdSamplesPerStep, TriggerLine.Line1);
                SourcePd.EnableScreen(false);
                SourcePd.InitiateTriggerModel();
                Thread.Sleep(100);
                bool isOK = SourceLd.PulsedSweepCurrent
                 (
                    startCurrent_mA / 1000.0,
                    endCurrent_mA / 1000.0,
                    ldVoltageUpperLimit_V,
                    measureDelay_ms / 1000.0,
                    apertureTime_ms / 1000.0,
                    steps,
                    pulseWidth_ms / 1000.0,
                    period_ms / 1000.0
                    );
                if (float.IsNaN(eaVoltage_V) == false)
                {
                    SourceEa.SetIsOutputOn(Keithley2602BChannel.CHA, false);
                }
                if (isOK)
                {
                    //SourceLd.InitiateTrigger();
                    var ldVals = SourceLd.ReadData(steps);
                    var pdVals = SourcePd.ReadData(dataCount).ToList();

                    SourcePd.AbortTriggerModel();
                    SourcePd.EnableScreen(true);

                    const int currArrayIndex = 0;
                    const int voltArrayIndex = 1;

                    _last_sweepData_LD_Currents = ldDrivingCurrs;
                    _last_sweepData_LD_Voltages = ldVals[voltArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
                    //20240202 电压超限报错, 避免再次出现
                    for (int i = 0; i < _last_sweepData_LD_Voltages.Length; i++)
                    {
                        if (Math.Abs(_last_sweepData_LD_Voltages[i]) > 9999)
                        {
                            if (_last_sweepData_LD_Voltages[i] > 0) _last_sweepData_LD_Voltages[i] = 9999;
                            if (_last_sweepData_LD_Voltages[i] < 0) _last_sweepData_LD_Voltages[i] = -9999;

                        }
                    }

                    _last_sweepData_LD_Sense_Currents = ldVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();

                    //DEBUG
                    if (false)
                    {

                        string sweepdata = "";
                        for (int i = 0; i < steps; i++)
                        {
                            string line = "";
                            for (int j = 0; j < period_ms_pdSamplesPerStep; j++)
                            {
                                line += $"{pdVals[i * period_ms_pdSamplesPerStep + j]},";
                            }
                            sweepdata += line + "\r\n";
                        }

                        System.IO.File.WriteAllText(@"D:\test_to300.csv", sweepdata, Encoding.ASCII);
                    }

                    //10us脉冲表示10个点, 从第10个点向前取一半脉冲宽度
                    //20us脉冲表示20个点, 从第20个点向前取一般脉冲宽度
                    int pdReadingHalfpulseWidthPoints = pulseWidth_pdSamplesPerStep / 2;  //跳过一半的采样点
                    List<double> sorted_dmmData = new List<double>();
                    for (int i = 0; i < steps; i++)
                    {
                        //单位是安培(A)
                        var sumList = pdVals.Skip(i * period_ms_pdSamplesPerStep).Skip(pdReadingHalfpulseWidthPoints).Take(pdReadingHalfpulseWidthPoints);

                        //如果超量程, 就控制到量程 pdRealRange_A
                        double tRange = Math.Abs(pdRealRange_A * 1.2);  //允许超20%量程
                        double maxval = sumList.Max();
                        double minval = sumList.Min();

                        if (maxval > tRange)
                        {
                            sorted_dmmData.Add(tRange);
                            PDInRange = false;
                        }
                        else if (minval < -tRange)
                        {
                            sorted_dmmData.Add(-tRange);
                            PDInRange = false;
                        }
                        else
                        {
                            sorted_dmmData.Add(sumList.Average());
                        }
                    }

                    //平滑
                    List<double> Smooth_dmmData = new List<double>();

                    //计算出平滑点数量
                    double Smooth_mA = 3.0;
                    int HalfSmoothCount = (int)(Math.Abs(Smooth_mA / stepCurrent_mA / 2.0));
                    if (HalfSmoothCount > 1)
                    {
                        for (int i = 0; i < steps; i++)
                        {
                            int p1 = i - HalfSmoothCount;
                            int p2 = i + HalfSmoothCount;

                            p1 = Math.Max(p1, 0);
                            p2 = Math.Min(p2, steps - 1);

                            var sumList = sorted_dmmData.GetRange(p1, p2 - p1);// Skip(i * period_ms_pdSamplesPerStep).Skip(pdReadingHalfpulseWidthPoints).Take(pdReadingHalfpulseWidthPoints);
                            Smooth_dmmData.Add(sumList.Average());
                        }
                    }
                    else
                    {
                        Smooth_dmmData = sorted_dmmData;
                    }

                    _last_sweepData_PD_Currents = Smooth_dmmData.ToArray();

                    //int pdReadingSkipPoints = 8;
                    //List<double> sorted_dmmData = new List<double>();
                    //for (int i = 0; i < steps; i++)
                    //{
                    //    var sumList = pdVals.Skip(i * pdSamplesPerStep).Skip(pdReadingSkipPoints).Take(pdSamplesPerStep - pdReadingSkipPoints);
                    //    sorted_dmmData.Add(sumList.Average());
                    //}
                    //this._last_sweepData_PD_Currents = sorted_dmmData.ToArray();
                }
                else
                {
                    SourceLd.Reset();
                    SourcePd.AbortTriggerModel();
                    SourcePd.EnableScreen(true);
                    SourcePd.Reset();
                    if (float.IsNaN(eaVoltage_V) == false)
                    {
                        SourceEa.Reset();
                    }
                }
                if (TestCount-- < 0)
                {
                    string str = ("Sweep_Pulse_LD_EA_PD 测试PD范围超过量程");
                    break;
                }

                if (PDInRange == false)
                {
                    tpdCurrentRange_mA *= 10;
                }

            } while (PDInRange == false);
        }

        public static void Sweep_Pulse_LD_PD(
            float startCurrent_mA, float stepCurrent_mA, float endCurrent_mA,
            float ldVoltageUpperLimit_V,
            float pdBiasVoltage_V, float pdRange_mA,
            double apertureTime_ms, double measureDelay_ms, double period_ms, double duty_rate_percent)
        {

            Sweep_Pulse_LD_EA_PD(
                startCurrent_mA, stepCurrent_mA, endCurrent_mA,
                ldVoltageUpperLimit_V,
                float.NaN, 0,     //EA
                pdBiasVoltage_V, pdRange_mA,
                apertureTime_ms, measureDelay_ms, period_ms, duty_rate_percent);
        }

        public static void SweepEA(float ldCurrent_mA, float ldComplianceVoltage_V, float startVoltage_V, float stepVoltage_V, float stopVoltage_V, float eaComplianceCurrent_mA, float pdBiasVoltage_V, float pdCurrentRange_mA, double period_ms)
        {
            if (!SourceLd.IsOnline || !SourceEa.IsOnline || !SourcePd.IsOnline)
            {
                return;
            }
            //PD档位
            double tpdCurrentRange_mA = pdCurrentRange_mA;

            bool PDInRange = true;

            int TestCount = 3;

            do
            {
                PDInRange = true;

                _last_sweepData_EA_Currents = new double[0];
                _last_sweepData_EA_Voltages = new double[0];
                _last_sweepData_PD_Currents = new double[0];

                if (startVoltage_V > stopVoltage_V)
                    stepVoltage_V = -Math.Abs(stepVoltage_V);
                else
                    stepVoltage_V = Math.Abs(stepVoltage_V);
                double[] eaDrivingVolts = ArrayMath.CalculateArray(startVoltage_V, stopVoltage_V, stepVoltage_V);
                int steps = eaDrivingVolts.Length;
                double measureDelay_s = 50E-6;

                double nplc = Math.Round((period_ms / 1000.0 - measureDelay_s) * 0.5 / 0.02, 4);

                int pdSamplesPerStep = Convert.ToInt32(period_ms * 0.5 / 1000.0 * 1000000); //采集整个脉宽范围内的数据，后续处理
                if (pdSamplesPerStep > 100) pdSamplesPerStep = 100;
                int dataCount = steps * pdSamplesPerStep;

                SourceLd.Reset();
                SourceEa.Reset();

                //这个复位还是 比较重要
                SourceEa.Reset(Keithley2602BChannel.CHA);
                Thread.Sleep(2000);

                //EA开始要加上电
                #region Set EA
                SourceEa.ResetTriggerLine(TriggerLine.Line14);
                SourceEa.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceVoltageSenceCurrent);
                SourceEa.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, true);
                SourceEa.SetVoltage_V(Keithley2602BChannel.CHA, startVoltage_V);
                SourceEa.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, true);
                SourceEa.SetComplianceCurrent_A(Keithley2602BChannel.CHA, eaComplianceCurrent_mA / 1000.0);
                SourceEa.SetIsOutputOn(Keithley2602BChannel.CHA, true);
                #endregion
                SourceLd.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);
                SourceLd.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, true);
                SourceLd.SetCurrent_A(Keithley2602BChannel.CHA, ldCurrent_mA / 1000.0);
                SourceLd.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, true);
                SourceLd.SetComplianceVoltage_V(Keithley2602BChannel.CHA, ldComplianceVoltage_V);
                SourceLd.SetIsOutputOn(Keithley2602BChannel.CHA, true);

                SourcePd.SetBufferSize(dataCount);

                SourcePd.AbortTriggerModel();
                double pdRealRange_A = SourcePd.SetTriggeredDigitizeCurrent(tpdCurrentRange_mA / 1000.0, steps, pdSamplesPerStep, TriggerLine.Line1);
                SourcePd.EnableScreen(false);
                SourcePd.InitiateTriggerModel();
                Thread.Sleep(100);

                var eaVals = SourceEa.SingleChannelSweepOutputTrigger
                    (
                       Keithley2602BChannel.CHA,
                       SourceMeterMode.SourceVoltageSenceCurrent,
                       startVoltage_V,
                       stopVoltage_V,
                       eaDrivingVolts.Length,
                       eaComplianceCurrent_mA / 1000.0,
                       period_ms / 1000.0,
                       TriggerLine.Line14,
                       measureDelay_s
                    );
                SourceLd.SetIsOutputOn(Keithley2602BChannel.CHA, false);
                var pdVals = SourcePd.ReadData(dataCount).ToList();

                SourcePd.AbortTriggerModel();
                SourcePd.EnableScreen(true);

                const int currArrayIndex = 0;
                const int voltArrayIndex = 1;

                _last_sweepData_EA_Currents = eaVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
                _last_sweepData_EA_Voltages = eaDrivingVolts;

                int pdReadingSkipPoints = 8;
                List<double> sorted_dmmData = new List<double>();
                for (int i = 0; i < steps; i++)
                {
                    var sumList = pdVals.Skip(i * pdSamplesPerStep).Skip(pdReadingSkipPoints).Take(pdSamplesPerStep - pdReadingSkipPoints);
                    //如果超量程, 就控制到量程 pdRealRange_A
                    double tRange = Math.Abs(pdRealRange_A * 1.2);  //允许超20%量程
                    double maxval = sumList.Max();
                    double minval = sumList.Min();

                    if (maxval > tRange)
                    {
                        sorted_dmmData.Add(tRange);
                        PDInRange = false;
                    }
                    else if (minval < -tRange)
                    {
                        sorted_dmmData.Add(-tRange);
                        PDInRange = false;
                    }
                    else
                    {
                        sorted_dmmData.Add(sumList.Average());
                    }
                }
                _last_sweepData_PD_Currents = sorted_dmmData.ToArray();
                if (TestCount-- < 0)
                {
                    string str = ("SweepEA 测试PD范围超过量程");
                    break;
                }

                if (PDInRange == false)
                {
                    tpdCurrentRange_mA *= 10;
                }
            } while (PDInRange == false);

        }

        public static void SweepEA_SourceVoltage_SenseCurrent(float startVoltage_V, float stepVoltage_V, float stopVoltage_V, float eaComplianceCurrent_mA)
        {
            if (!SourceLd.IsOnline || !SourceEa.IsOnline || !SourcePd.IsOnline)
            {
                return;
            }

            _last_sweepData_EA_Currents = new double[0];
            _last_sweepData_EA_Voltages = new double[0];

            const int TrigOutLine = 14;
            double apertureTime_s = 100 / 1000.0 / 1000.0;
            double measureDelay_s = 50E-6;
            bool pulsedMode = false;
            double[] eaDrivingVolts = ArrayMath.CalculateArray(startVoltage_V, stopVoltage_V, stepVoltage_V);
            int steps = eaDrivingVolts.Length;

            SourceLd.Reset();
            SourceEa.Reset();

            //这个复位还是 比较重要
            SourceEa.Reset(Keithley2602BChannel.CHA);
            Thread.Sleep(2000);
            var eaVals = SourceEa.SingleChannelSweepOutputTrigger
            (
               Keithley2602BChannel.CHA,
               SourceMeterMode.SourceVoltageSenceCurrent,
               startVoltage_V,
               stopVoltage_V,
               eaDrivingVolts.Length,
               eaComplianceCurrent_mA,
               apertureTime_s,
               TriggerLine.Line14,
               measureDelay_s,
               pulsedMode
            );
            const int currArrayIndex = 0;
            const int voltArrayIndex = 1;
            _last_sweepData_EA_Currents = eaVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
            _last_sweepData_EA_Voltages = eaDrivingVolts;

        }

        public static void SweepLD(float startCurrent_mA, float stepCurrent_mA, float ldCurrentUpperLimit_mA,
            float ldVoltageUpperLimit_V, bool enableEAOutput, float eaVoltage_V,
            float pdTestCh1_complianceCurrent_mA, float pdTestCh2_complianceCurrent_mA,
            float pdTestCh3_complianceCurrent_mA, float pdTestCh4_complianceCurrent_mA, double K2400_NPLC)
        {
            if (!SourceLd.IsOnline || !SourceEa.IsOnline || !SourcePd.IsOnline)
            {
                return;
            }
            _last_sweepData_LD_Currents = new double[0];
            _last_sweepData_LD_Voltages = new double[0];
            _last_sweepData_EA_Currents = new double[0];
            _last_sweepData_EA_Voltages = new double[0];
            _last_sweepData_PD_Currents = new double[0];
            _last_sweepData_LD_Sense_Currents = new double[0];

            double[] ldDrivingCurrs = ArrayMath.CalculateArray(startCurrent_mA, ldCurrentUpperLimit_mA, stepCurrent_mA);
            int steps = ldDrivingCurrs.Length;
            double apertureTime_s = 0.02 * K2400_NPLC;
            SourceLd.Reset();
            SourceEa.Reset();
            double measureDelay_s = 50E-6;
            bool pulsedMode = false;

            if (enableEAOutput == true)
            {
                SourceEa.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceVoltageSenceCurrent);
                SourceEa.SetVoltage_V(Keithley2602BChannel.CHA, eaVoltage_V);
                SourceEa.SetComplianceCurrent_A(Keithley2602BChannel.CHA, 5 / 1000.0);
                SourceEa.SetIsOutputOn(Keithley2602BChannel.CHA, true);
            }
            //这个复位还是 比较重要
            SourceLd.Reset(Keithley2602BChannel.CHA);
            Thread.Sleep(2000);

            var ldVals = SourceLd.SingleChannelSweepOutputTrigger
             (
                Keithley2602BChannel.CHA,
                SourceMeterMode.SourceCurrentSenceVoltage,
                startCurrent_mA,
                ldCurrentUpperLimit_mA,
                steps,
                ldVoltageUpperLimit_V,
                apertureTime_s,
                TriggerLine.Line14,
                measureDelay_s,
                pulsedMode
             );
            if (enableEAOutput == true)
            {
                SourceEa.SetIsOutputOn(Keithley2602BChannel.CHA, false);
            }
            const int currArrayIndex = 0;

            const int voltArrayIndex = 1;

            _last_sweepData_LD_Currents = ldDrivingCurrs;
            _last_sweepData_LD_Voltages = ldVals[voltArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
            _last_sweepData_LD_Sense_Currents = ldVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();

            List<double> eaVolts = new List<double>();
            for (int i = 0; i < ldDrivingCurrs.Length; i++)
            {
                eaVolts.Add(eaVoltage_V);
            }
            _last_sweepData_EA_Voltages = eaVolts.ToArray();

        }

        public static void SweepLD_SourceVoltage_SenseCurrent(float startVoltage_V, float stepVoltage_V, float stopVoltage_V, float ldCurrentUpperLimit_mA)
        {
            if (!SourceLd.IsOnline || !SourceEa.IsOnline || !SourcePd.IsOnline)
            {
                return;
            }
            _last_sweepData_LD_Currents = new double[0];
            _last_sweepData_LD_Voltages = new double[0];

            double[] ldDrivingVolts = ArrayMath.CalculateArray(startVoltage_V, stopVoltage_V, stepVoltage_V);
            int steps = ldDrivingVolts.Length;
            double period_s = 5 / 1000.0;// 时间加长
            SourceLd.Reset();
            SourceEa.Reset();
            double measureDelay_s = 50E-6;
            bool pulsedMode = false;

            //这个复位还是 比较重要
            SourceLd.Reset(Keithley2602BChannel.CHA);
            Thread.Sleep(2000);

            var ldVals = SourceLd.SingleChannelSweepOutputTrigger
             (
                Keithley2602BChannel.CHA,
                SourceMeterMode.SourceVoltageSenceCurrent,
                startVoltage_V,
                stopVoltage_V,
                steps,
                ldCurrentUpperLimit_mA / 1000.0,
                period_s,
                TriggerLine.Line14,
                measureDelay_s,
                pulsedMode
             );

            const int currArrayIndex = 0;
            const int voltArrayIndex = 1;

            _last_sweepData_LD_Currents = ldVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
            _last_sweepData_LD_Sense_Currents = ldVals[currArrayIndex].Split(',').Select(item => Convert.ToDouble(item)).ToArray();
            _last_sweepData_LD_Voltages = ldDrivingVolts;

        }

        public static float[] FetchLDSweepData(SweepDataType dataType)
        {
            if (dataType == SweepDataType.LD_Drive_Current_mA ||
                    dataType == SweepDataType.LD_Drive_Voltage_V ||
                    dataType == SweepDataType.EA_Drive_Current_mA ||
                    dataType == SweepDataType.PD_Ch1_Current_mA ||

                    dataType == SweepDataType.LD_Sense_Current_mA)
            {

            }
            else
            {
                //this.FormatedLog("Invalid sweep data type [{0}] for FetchLDSweepData", dataType);
                return null;
            }
            var dataCount = 0;
            if (_last_sweepData_LD_Currents == null ||
                _last_sweepData_LD_Currents.Length <= 0)
            {
                return new float[0];
            }
            else
            {
                dataCount = _last_sweepData_LD_Currents.Length;
            }
            switch (dataType)
            {
                case SweepDataType.LD_Sense_Current_mA:
                    {
                        if (SourceLd.IsOnline)
                        {

                            List<float> ret = new List<float>();
                            foreach (var val in _last_sweepData_LD_Sense_Currents)
                            {
                                ret.Add(Convert.ToSingle(val * 1000.0));
                            }

                            return ret.ToArray();
                        }
                    }
                    break;
                case SweepDataType.LD_Drive_Current_mA:
                    {
                        List<float> ret = new List<float>();
                        foreach (var val in _last_sweepData_LD_Currents)
                        {
                            ret.Add(Convert.ToSingle(val * 1000.0));
                        }

                        return ret.ToArray();
                    }
                    break;
                case SweepDataType.LD_Drive_Voltage_V:
                    {
                        if (SourceLd.IsOnline)
                        {
                            List<float> ret = new List<float>();
                            foreach (var val in _last_sweepData_LD_Voltages)
                            {
                                ret.Add(Convert.ToSingle(val));
                            }

                            return ret.ToArray();
                        }
                    }
                    break;
                case SweepDataType.EA_Drive_Current_mA:
                    {
                        if (SourceEa.IsOnline)
                        {

                            List<float> ret = new List<float>();
                            foreach (var val in _last_sweepData_EA_Currents)
                            {
                                ret.Add(Convert.ToSingle(val * 1000.0));
                            }

                            return ret.ToArray();
                        }
                    }
                    break;
                case SweepDataType.PD_Ch1_Current_mA:
                    {
                        if (SourcePd.IsOnline)
                        {
                            List<float> ret = new List<float>();
                            foreach (var val in _last_sweepData_PD_Currents)
                            {
                                ret.Add(Convert.ToSingle(val * 1000.0));
                            }

                            return ret.ToArray();
                        }
                    }
                    break;

            }
            return null;


        }

        public static float[] FetchEASweepData(SweepDataType dataType)
        {
            if (dataType == SweepDataType.EA_Drive_Current_mA ||
                      dataType == SweepDataType.EA_Drive_Voltage_V ||

                      dataType == SweepDataType.PD_Ch1_Current_mA ||
                      dataType == SweepDataType.PD_Ch2_Current_mA)
            {

            }
            else
            {
                //this.FormatedLog("Invalid sweep data type [{0}] for FetchEASweepData", dataType);
                return null;
            }
            var dataCount = 0;
            if (_last_sweepData_EA_Voltages == null ||
                _last_sweepData_EA_Voltages.Length <= 0)
            {
                return new float[0];
            }
            else
            {
                dataCount = _last_sweepData_EA_Voltages.Length;
            }
            switch (dataType)
            {
                case SweepDataType.EA_Drive_Current_mA:
                    {
                        List<float> ret = new List<float>();
                        foreach (var val in _last_sweepData_EA_Currents)
                        {
                            ret.Add(Convert.ToSingle(val));
                        }
                        return ret.ToArray();

                    }
                    break;
                case SweepDataType.EA_Drive_Voltage_V:
                    {
                        List<float> ret = new List<float>();
                        foreach (var val in _last_sweepData_EA_Voltages)
                        {
                            ret.Add(Convert.ToSingle(val));
                        }
                        return ret.ToArray();
                    }
                    break;
                case SweepDataType.PD_Ch1_Current_mA:
                    {
                        if (SourcePd.IsOnline)
                        {
                            List<float> ret = new List<float>();
                            foreach (var val in _last_sweepData_PD_Currents)
                            {
                                ret.Add(Convert.ToSingle(val) * 1000);
                            }
                            return ret.ToArray();
                        }
                    }
                    break;

            }
            return new float[0];
        }
    }
}
