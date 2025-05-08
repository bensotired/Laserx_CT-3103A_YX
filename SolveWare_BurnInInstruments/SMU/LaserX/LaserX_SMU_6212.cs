using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using LX_BurnInSolution.Utilities;
//using System.Threading.Tasks;
using NationalInstruments.DAQmx;

namespace SolveWare_BurnInInstruments
{


    public class LaserX_SMU_6212 : InstrumentBase
    {
        static string Properties_Resources = "SolveWare_BurnInInstruments.Properties.Resources";
        private Task LDDriverTask;
        private Task MultiChannelReadTask;

        private AnalogMultiChannelReader MultiChannelReader;



        private LinearScale LDCurrentSourceScale;
        private LinearScale LDCurrentSenseScale;
        private LinearScale LDVoltageSenseScale;
        private LinearScale PDCurrentSenseScale;
        private LinearScale MPDCurrentSenseScale;

        public SMUConfiguration config { get; set; }


        public LaserX_SMU_6212(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                ResourceManager rm = new ResourceManager(Properties_Resources, asm);
                var wftRes = rm.GetString("smuconfig");
                XmlSerializer se = new XmlSerializer(typeof(SMUConfiguration));
                XmlReader xr = XElement.Parse(wftRes).CreateReader();
                config = (SMUConfiguration)se.Deserialize(xr);

                //using (StreamReader reader = new  StreamReader(wftRes))
                //{
                    
                //}
                InitializeCustomScale();
            }
            catch(Exception ex)
            {
            }
        }

        
        private string[] GetDevices()
        {
            List<string> devices = new List<string>();
            string[] daqChannels = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External);
            foreach (var daq in daqChannels)
            {
                if (devices.Contains(daq.Split('/')[0]))
                {
                    continue;
                }
                else
                {
                    devices.Add(daq.Split('/')[0]);
                }
            }
            return devices.ToArray();
        }
        public void InitializeCustomScale()
        {
            LDCurrentSourceScale = new LinearScale("LD_Current_Source_Scale", config.LD_Current_Source_K, config.LD_Current_Source_B);
            LDCurrentSenseScale = new LinearScale("LD_Current_Sense_Scale", config.LD_Current_Sense_K, config.LD_Current_Sense_B);
            LDVoltageSenseScale = new LinearScale("LD_Voltage_Sense_Scale", config.LD_Voltage_Sense_K, config.LD_Voltage_Sense_B);
            PDCurrentSenseScale = new LinearScale("PD_Current_Sense_Scale", config.PD_Current_Sense_K, config.PD_Current_Sense_B);
            MPDCurrentSenseScale = new LinearScale("MPD_Current_Sense_Scale", config.MPD_Current_Sense_K, config.MPD_Current_Sense_B);
        }

        public void SetOutputOn(bool isOn)
        {
            using (Task digitalWriteTask = new Task())
            {
                string channel = $"Dev{config.DeviceID}/port1/line{config.Enable_Output_IOChannel}";
                digitalWriteTask.DOChannels.CreateChannel(
                    channel,
                    "",
                ChannelLineGrouping.OneChannelForEachLine);
                DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                writer.WriteSingleSampleSingleLine(true, isOn);
                System.Threading.Thread.Sleep(100);
            }
            using (Task digitalWriteTask = new Task())
            {
                string channel = $"Dev{config.DeviceID}/port1/line{config.Discharge_IOChannel}";
                digitalWriteTask.DOChannels.CreateChannel(
                    channel,
                    "",
                ChannelLineGrouping.OneChannelForEachLine);
                DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                writer.WriteSingleSampleSingleLine(true, isOn);
                System.Threading.Thread.Sleep(100);
            }
        }
        public void SetLDOutputCurrent_A(double current)
        {
            using (LDDriverTask = new Task())
            {
                LDDriverTask.AOChannels.CreateVoltageChannel(
                    $"Dev{config.DeviceID}/ao{config.LD_Current_Drive_Channel}",
                    "LD_Current_Source",
                    -config.LD_Current_Source_Range,
                    config.LD_Current_Source_Range,
                    "LD_Current_Source_Scale");
                //CLOCK CONFIG
                AnalogSingleChannelWriter writer = new AnalogSingleChannelWriter(LDDriverTask.Stream);
                writer.WriteSingleSample(true, current);
            }
        }
        public double GetLDActualCurrent_A(double integratingTime_s)
        {
            StopAsyncReadMultipleChannel();

            using (Task ReadTask = new Task())
            {
                // Create channels
                string SenseChannel = $"Dev{config.DeviceID}/ai{config.LD_Current_Sense_Channel}";
                ReadTask.AIChannels.CreateVoltageChannel(SenseChannel,
                    "",
                    config.LD_Current_Sense_Channel_Type,
                    -config.LD_Current_Sense_Range,
                    config.LD_Current_Sense_Range,
                    "LD_Current_Sense_Scale");

                ReadTask.Timing.ConfigureSampleClock("",
                config.AISampleClockRate,
                SampleClockActiveEdge.Rising,
                SampleQuantityMode.FiniteSamples, (int)(config.AISampleClockRate * integratingTime_s));

                // Verify the task
                ReadTask.Control(TaskAction.Verify);
                AnalogSingleChannelReader reader = new AnalogSingleChannelReader(ReadTask.Stream);
                double[] readings = reader.ReadMultiSample((int)(config.AISampleClockRate * integratingTime_s));
                double curr = readings.Average();// * config.LD_Current_Sense_K + config.LD_Current_Sense_B;
                return curr;
            }
        }
        public double GetLDActualVoltage_V(double integratingTime_s)
        {
            StopAsyncReadMultipleChannel();
            using (Task ReadTask = new Task())
            {

                // Create channels
                string SenseChannel = $"Dev{config.DeviceID}/ai{config.LD_Voltage_Sense_Channel}";
                ReadTask.AIChannels.CreateVoltageChannel(
                    SenseChannel,
                    "",
                     config.LD_Voltage_Sense_Channel_Type,
                    -config.LD_Voltage_Sense_Range,
                    config.LD_Voltage_Sense_Range,
                    "LD_Voltage_Sense_Scale");

                ReadTask.Timing.ConfigureSampleClock("",
                config.AISampleClockRate,
                SampleClockActiveEdge.Rising,
                SampleQuantityMode.FiniteSamples, (int)(config.AISampleClockRate * integratingTime_s));

                // Verify the task
                ReadTask.Control(TaskAction.Verify);

                AnalogSingleChannelReader reader = new AnalogSingleChannelReader(ReadTask.Stream);
                double[] readings = reader.ReadMultiSample((int)(config.AISampleClockRate * integratingTime_s));
                double volt = readings.Average();
                return volt;
            }
        }
        public double GetPDActualCurrent_mA(double integraingTime_s)
        {
            StopAsyncReadMultipleChannel();
            using (Task ReadTask = new Task())
            {
                // Create channels
                string SenseChannel = $"Dev{config.DeviceID}/ai{config.PD_Current_Sense_Channel}";
                ReadTask.AIChannels.CreateVoltageChannel(SenseChannel,
                    "",
                    config.PD_Current_Sense_Channel_Type,
                    -config.PD_Current_Sense_Range,
                    config.PD_Current_Sense_Range,
                    "PD_Current_Sense_Scale");

                ReadTask.Timing.ConfigureSampleClock("",
                config.AISampleClockRate,
                SampleClockActiveEdge.Rising,
                SampleQuantityMode.FiniteSamples, (int)(config.AISampleClockRate * integraingTime_s));

                // Verify the task
                ReadTask.Control(TaskAction.Verify);

                AnalogSingleChannelReader reader = new AnalogSingleChannelReader(ReadTask.Stream);
                double[] readings = reader.ReadMultiSample((int)(config.AISampleClockRate * integraingTime_s));
                double curr = readings.Average();// * config.PD_Current_Sense_K + config.PD_Current_Sense_B;
                return curr;
            }
        }
        public double GetMPDActualCurrent_mA(double integratingTime_s)
        {
            StopAsyncReadMultipleChannel();
            using (Task ReadTask = new Task())
            {

                // Create channels
                string SenseChannel = $"Dev{config.DeviceID}/ai{config.MPD_Current_Sense_Channel}";
                ReadTask.AIChannels.CreateVoltageChannel(SenseChannel,
                          "",
                    config.MPD_Current_Sense_Channel_Type,
                    -config.MPD_Current_Sense_Range,
                    config.MPD_Current_Sense_Range,
                    "MPD_Current_Sense_Scale");

                ReadTask.Timing.ConfigureSampleClock("",
                config.AISampleClockRate,
                SampleClockActiveEdge.Rising,
                SampleQuantityMode.FiniteSamples, (int)(config.AISampleClockRate * integratingTime_s));

                // Verify the task
                ReadTask.Control(TaskAction.Verify);

                AnalogSingleChannelReader reader = new AnalogSingleChannelReader(ReadTask.Stream);
                double[] readings = reader.ReadMultiSample((int)(config.AISampleClockRate * integratingTime_s));
                double curr = readings.Average();// * config.MPD_Current_Sense_K + config.MPD_Current_Sense_B;
                return curr;
            }
        }

        private double CalculateRange(double limit)
        {
            if (limit >= -0.2 && limit <= 0.2)
            {
                return 0.2;
            }
            if (limit >= -1 && limit <= 1)
            {
                return 1;
            }
            if (limit >= -5 && limit <= 5)
            {
                return 5;
            }
            if (limit >= -10 && limit <= 10)
            {
                return 10;
            }
            return 0;
        }

        //public void StartAsyncReadMultipleChannel(double integratingTime_s)
        //{
        //    if (MultiChannelReadTask != null)
        //    {
        //        MultiChannelReadTask.Dispose();
        //    }
        //    MultiChannelReadTask = new Task();
        //    // Create channels
        //    MultiChannelReadTask.AIChannels.CreateVoltageChannel(
        //        $"Dev{config.DeviceID}/ai{config.LD_Current_Sense_Channel}",
        //         "LD_Current_Sense",
        //         config.LD_Current_Sense_Channel_Type,
        //         -config.LD_Current_Sense_Range,
        //         config.LD_Current_Sense_Range,
        //         "LD_Current_Sense_Scale"
        //         );
        //    MultiChannelReadTask.AIChannels.CreateVoltageChannel(
        //        $"Dev{config.DeviceID}/ai{config.LD_Voltage_Sense_Channel}",
        //         "LD_Voltge_Sense",
        //             config.LD_Voltage_Sense_Channel_Type,
        //            -config.LD_Voltage_Sense_Range,
        //            config.LD_Voltage_Sense_Range,
        //            "LD_Voltage_Sense_Scale");
        //    MultiChannelReadTask.AIChannels.CreateVoltageChannel(
        //        $"Dev{config.DeviceID}/ai{config.PD_Current_Sense_Channel}",
        //        "PD_Current_Sense",
        //             config.PD_Current_Sense_Channel_Type,
        //            -config.PD_Current_Sense_Range,
        //            config.PD_Current_Sense_Range,
        //            "PD_Current_Sense_Scale");
        //    MultiChannelReadTask.AIChannels.CreateVoltageChannel(
        //        $"Dev{config.DeviceID}/ai{config.MPD_Current_Sense_Channel}",
        //            "MPD_Current_Sense",
        //            config.MPD_Current_Sense_Channel_Type,
        //            -config.MPD_Current_Sense_Range,
        //            config.MPD_Current_Sense_Range,
        //            "MPD_Current_Sense_Scale");

        //    // Set up timing
        //    MultiChannelReadTask.Timing.ConfigureSampleClock("",
        //        config.AISampleClockRate,
        //        SampleClockActiveEdge.Rising,
        //        SampleQuantityMode.ContinuousSamples,
        //        (int)(config.AISampleClockRate * integratingTime_s));

        //    // Verify the task
        //    MultiChannelReadTask.Control(TaskAction.Verify);

        //    MultiChannelReader = new AnalogMultiChannelReader(MultiChannelReadTask.Stream);
        //    MultiChannelReader.SynchronizeCallbacks = true;
        //    MultiChannelReader.BeginReadMultiSample((int)(config.AISampleClockRate * integratingTime_s), new AsyncCallback(MultiChannelRead), MultiChannelReadTask);
        //}

        public void SwitchToExternalSource(bool enable)
        {
            using (Task digitalWriteTask = new Task())
            {
                string channel = $"Dev{config.DeviceID}/port1/line{config.ExternalSourceSwitch_IOChannel}";
                digitalWriteTask.DOChannels.CreateChannel(
                    channel,
                    "",
                ChannelLineGrouping.OneChannelForEachLine);
                DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                writer.WriteSingleSampleSingleLine(true, enable);
                System.Threading.Thread.Sleep(100);
            }
        }

        public void StopAsyncReadMultipleChannel()
        {
            if (MultiChannelReadTask != null)
            {
                MultiChannelReadTask.Dispose();
                MultiChannelReadTask = null;
            }

        }
        private void MultiChannelRead(IAsyncResult ar)
        {
            try
            {
                if (MultiChannelReadTask != null && MultiChannelReadTask == ar.AsyncState)
                {
                    // Read the data
                    double[,] data = MultiChannelReader.EndReadMultiSample(ar);

                    double[] returnData = new double[data.GetLength(0)];
                    int n = data.GetLength(1);
                    double[] ldCurrent = new double[n];
                    double[] ldVoltage = new double[n];
                    double[] pdCurrent = new double[n];
                    double[] mpdCurrent = new double[n];


                    for (int i = 0; i < n; i++)
                    {
                        ldCurrent[i] = data[0, i];
                        ldVoltage[i] = data[1, i];
                        pdCurrent[i] = data[2, i];
                        mpdCurrent[i] = data[3, i];
                    }
                    returnData[0] = ldCurrent.Average();// * config.LD_Current_Sense_K + config.LD_Current_Sense_B;
                    returnData[1] = ldVoltage.Average();
                    returnData[2] = pdCurrent.Average();// * config.PD_Current_Sense_K + config.PD_Current_Sense_B;
                    returnData[3] = mpdCurrent.Average();

                    //MultiChannelDataAcquired?.Invoke(returnData);
                    MultiChannelReader.BeginReadMultiSample(n, new AsyncCallback(MultiChannelRead), MultiChannelReadTask);

                }
            }
            catch (Exception ex)
            {
                MultiChannelReadTask.Dispose();
                MultiChannelReadTask = null;
                MessageBox.Show(ex.Message);
            }
        }


        LIVSweepSetting LIVSweepSetting;
        public Dictionary<ReadingSection_6212, double[]> LIVSweepSync(LIVSweepSetting livSweepSetting)
        {
            Dictionary<ReadingSection_6212, double[]> result = new Dictionary<ReadingSection_6212, double[]>();
            try
            {
                this.LIVSweepSetting = livSweepSetting;
                double[] output = new double[1];
                if (livSweepSetting.SweepMode == SweepMode.Pulse)
                {
                    output = this.GeneratePulseStair(
                                livSweepSetting.Period_s,
                                livSweepSetting.DutyCycle,
                                livSweepSetting.StartCurrent_A,
                                livSweepSetting.StopCurrent_A,
                                livSweepSetting.StepCurrent_A,
                                0.001,
                                config.AOSampleClockRate);
                }
                else if (livSweepSetting.SweepMode == SweepMode.CW)
                {
                    output = this.GenerateStaircaseWave(
                                livSweepSetting.Period_s,
                                livSweepSetting.StartCurrent_A,
                                livSweepSetting.StopCurrent_A,
                                livSweepSetting.StepCurrent_A,
                                config.AOSampleClockRate);
                }
                //if (LDDriverTask != null)
                //{
                //    LDDriverTask.Dispose();
                //}
                //if (MultiReadTask != null)
                //{
                //    MultiReadTask.Dispose();
                //}
                // Create the master and slave tasks
                LDDriverTask = new Task("LDDriverTask");
                MultiChannelReadTask = new Task("MultiChannelReadTask");

                // Configure both tasks with the values selected on the UI.
                MultiChannelReadTask.AIChannels.CreateVoltageChannel(
                  $"Dev{config.DeviceID}/ai{config.LD_Current_Sense_Channel}",
                  "LDCurrentSense",
                  config.LD_Current_Sense_Channel_Type,
                  -config.LD_Current_Sense_Range,
                  config.LD_Current_Sense_Range,
                  "LD_Current_Sense_Scale");
                MultiChannelReadTask.AIChannels.CreateVoltageChannel(
                     $"Dev{config.DeviceID}/ai{config.LD_Voltage_Sense_Channel}",
                    "LDVoltageSense",
                      config.LD_Voltage_Sense_Channel_Type,
                    -config.LD_Voltage_Sense_Range,
                    config.LD_Voltage_Sense_Range,
                    "LD_Voltage_Sense_Scale");
                MultiChannelReadTask.AIChannels.CreateVoltageChannel(
                    $"Dev{config.DeviceID}/ai{config.PD_Current_Sense_Channel}",
                    "PDCurrentSense",
                   config.PD_Current_Sense_Channel_Type,
                    -config.PD_Current_Sense_Range,
                    config.PD_Current_Sense_Range,
                    "PD_Current_Sense_Scale");
                MultiChannelReadTask.AIChannels.CreateVoltageChannel(
                    $"Dev{config.DeviceID}/ai{config.MPD_Current_Sense_Channel}",
                    "MPDCurrentSense",
                    config.MPD_Current_Sense_Channel_Type,
                    -config.MPD_Current_Sense_Range,
                    config.MPD_Current_Sense_Range,
                    "MPD_Current_Sense_Scale");

                LDDriverTask.AOChannels.CreateVoltageChannel(
                     $"Dev{config.DeviceID}/aO{config.LD_Current_Drive_Channel}",
                    "LDCurrentSource",
                   -config.LD_Current_Source_Range,
                    config.LD_Current_Source_Range,
                    "LD_Current_Source_Scale");

                // Set up the timing
                MultiChannelReadTask.Timing.ConfigureSampleClock("",
                    config.AISampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples,
                    output.Length);
                LDDriverTask.Timing.ConfigureSampleClock("",
                    config.AOSampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples,
                    output.Length);

                //Config Trigger
                string deviceName = $"/Dev{config.DeviceID}/";
                MultiChannelReadTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(deviceName + "ao/StartTrigger", DigitalEdgeStartTriggerEdge.Rising);

                //Verify Tasks
                MultiChannelReadTask.Control(TaskAction.Verify);
                LDDriverTask.Control(TaskAction.Verify);

                AnalogSingleChannelWriter writer = new AnalogSingleChannelWriter(LDDriverTask.Stream);
                writer.WriteMultiSample(false, output);

                // Officially start the task
                MultiChannelReadTask.Start();
                LDDriverTask.Start();

                //System.Threading.Thread.Sleep(30000);
                MultiChannelReader = new AnalogMultiChannelReader(MultiChannelReadTask.Stream);
                MultiChannelReader.SynchronizeCallbacks = true;

                var arr_2d = MultiChannelReader.ReadMultiSample(output.Length);
                //MultiChannelReader.BeginReadMultiSample(output.Length, new AsyncCallback(LIVSweepDoneCallback), null);
                result = Decode_LIVSweepData(arr_2d);


            }

            catch (Exception exception)
            {
                throw exception;
                //MessageBox.Show(ex.Message);
            }
            return result;
        }
        private Dictionary<ReadingSection_6212, double[]> Decode_LIVSweepData(double[,] rawData)
        {
            Dictionary<ReadingSection_6212, double[]> result = new Dictionary<ReadingSection_6212, double[]>();
            try
            {
                // Read the available data from the channels   
                //double[,] rawData = MultiChannelReader.EndReadMultiSample(ar);
                MultiChannelReadTask.Dispose();
                LDDriverTask.Dispose();
                SetLDOutputCurrent_A(0);
                int channels = rawData.GetLength(0);
                int dataCount = rawData.GetLength(1);
                double[] ldCurrentRaw = new double[dataCount];
                double[] ldVoltageRaw = new double[dataCount];
                double[] pdCurrentRaw = new double[dataCount];
                double[] mpdCurrentRaw = new double[dataCount];
                //double[] tempRaw = new double[dataCount];
                for (int i = 0; i < dataCount; i++)
                {
                    ldCurrentRaw[i] = rawData[0, i];
                    ldVoltageRaw[i] = rawData[1, i];
                    pdCurrentRaw[i] = rawData[2, i];
                    mpdCurrentRaw[i] = rawData[3, i];
                    //tempRaw[i] = rawData[3, i];
                }
                int steps = Convert.ToInt16((LIVSweepSetting.StopCurrent_A - LIVSweepSetting.StartCurrent_A) / LIVSweepSetting.StepCurrent_A + 1);

                double[] ldCurr = new double[steps];
                double[] ldVolt = new double[steps];
                double[] pdCurr = new double[steps];
                double[] mpdCurr = new double[steps];
                if (LIVSweepSetting.SweepMode == SweepMode.Pulse)
                {
                    ldCurr = ProcessPulseSweepData(ldCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                    ldVolt = ProcessPulseSweepData(ldVoltageRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                    pdCurr = ProcessPulseSweepData(pdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                    mpdCurr = ProcessPulseSweepData(mpdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                }
                else if (LIVSweepSetting.SweepMode == SweepMode.CW)
                {
                    ldCurr = ProcessCWSweepData(ldCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                    ldVolt = ProcessCWSweepData(ldVoltageRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                    pdCurr = ProcessCWSweepData(pdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                    mpdCurr = ProcessCWSweepData(mpdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
                }


                result.Add(ReadingSection_6212.RawLD_Current, ldCurrentRaw);
                result.Add(ReadingSection_6212.RawLD_Voltage, ldVoltageRaw);
                result.Add(ReadingSection_6212.RawPD_Current_Rng80uA, pdCurrentRaw);
                result.Add(ReadingSection_6212.RawMPD_Current_Rng300uA, mpdCurrentRaw);
                result.Add(ReadingSection_6212.LD_Current, ldCurr);
                result.Add(ReadingSection_6212.LD_Voltage, ldVolt);
                result.Add(ReadingSection_6212.PD_Current_Rng80uA, pdCurr); //80uA
                result.Add(ReadingSection_6212.MPD_Current_Rng300uA, mpdCurr); //300uA
            }
            catch (DaqException exception)
            {
                //  MessageBox.Show(exception.Message);
                throw exception;
            }
            finally
            {


            }
            return result;
        }
        //private void LIVSweepDoneCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Read the available data from the channels   
        //        double[,] rawData = MultiChannelReader.EndReadMultiSample(ar);
        //        MultiChannelReadTask.Dispose();
        //        LDDriverTask.Dispose();
        //        SetLDOutputCurrent_A(0);
        //        int channels = rawData.GetLength(0);
        //        int dataCount = rawData.GetLength(1);
        //        double[] ldCurrentRaw = new double[dataCount];
        //        double[] ldVoltageRaw = new double[dataCount];
        //        double[] pdCurrentRaw = new double[dataCount];
        //        double[] mpdCurrentRaw = new double[dataCount];
        //        //double[] tempRaw = new double[dataCount];
        //        for (int i = 0; i < dataCount; i++)
        //        {
        //            ldCurrentRaw[i] = rawData[0, i];
        //            ldVoltageRaw[i] = rawData[1, i];
        //            pdCurrentRaw[i] = rawData[2, i];
        //            mpdCurrentRaw[i] = rawData[3, i];
        //            //tempRaw[i] = rawData[3, i];
        //        }
        //        int steps = Convert.ToInt16((LIVSweepSetting.StopCurrent_A - LIVSweepSetting.StartCurrent_A) / LIVSweepSetting.StepCurrent_A + 1);

        //        double[] ldCurr = new double[steps];
        //        double[] ldVolt = new double[steps];
        //        double[] pdCurr = new double[steps];
        //        double[] mpdCurr = new double[steps];
        //        if (LIVSweepSetting.SweepMode == SweepMode.Pulse)
        //        {
        //            ldCurr = ProcessPulseSweepData(ldCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //            ldVolt = ProcessPulseSweepData(ldVoltageRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //            pdCurr = ProcessPulseSweepData(pdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //            mpdCurr = ProcessPulseSweepData(mpdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.DutyCycle, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //        }
        //        else if (LIVSweepSetting.SweepMode == SweepMode.CW)
        //        {
        //            ldCurr = ProcessCWSweepData(ldCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //            ldVolt = ProcessCWSweepData(ldVoltageRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //            pdCurr = ProcessCWSweepData(pdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //            mpdCurr = ProcessCWSweepData(mpdCurrentRaw, steps, LIVSweepSetting.Period_s, LIVSweepSetting.MeasureDelay_s, LIVSweepSetting.IntegratingPeriod_s, config.AISampleClockRate);
        //        }

        //        Dictionary<string, double[]> result = new Dictionary<string, double[]>();
        //        result.Add("LDCurrRaw", ldCurrentRaw);
        //        result.Add("LDVoltRaw", ldVoltageRaw);
        //        result.Add("PDCurrRaw", pdCurrentRaw);
        //        result.Add("MPDCurrRaw", mpdCurrentRaw);
        //        result.Add("LDCurr", ldCurr);
        //        result.Add("LDVolt", ldVolt);
        //        result.Add("PDCurr", pdCurr);
        //        result.Add("MPDCurr", mpdCurr);
        //        LIVSweepDataAcquired?.Invoke(result);

        //    }
        //    catch (DaqException exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //    }
        //    finally
        //    {


        //    }
        //}

        private double[] GeneratePulseStair(double period, double dutyCycle, double startCurrent, double stopCurrent, double stepCurrent, double pulseBase, double aOSampleClockRate)
        {
            double pulseWidth = dutyCycle * period;
            int points = Convert.ToInt32((stopCurrent - startCurrent) / stepCurrent + 1);
            int totalSamples = Convert.ToInt32(aOSampleClockRate * points * period);
            int periodSamples = Convert.ToInt32(aOSampleClockRate * period);
            int pulseOnSamples = Convert.ToInt32(aOSampleClockRate * dutyCycle * period);
            double[] rVal = new double[totalSamples];

            for (int i = 0; i < points; i++)
            {
                for (int j = 0; j < pulseOnSamples; j++)
                {
                    rVal[i * periodSamples + j] = i * stepCurrent;
                }
                for (int j = pulseOnSamples; j < periodSamples; j++)
                {
                    rVal[i * periodSamples + j] = pulseBase;
                }
            }
            return rVal;
        }
        private double[] GenerateStaircaseWave(
        double period,
        double startCurrent,
        double stopCurrent,
        double stepCurrent,
        double aOSampleClockRate
        )
        {
            int steps = Convert.ToInt32((stopCurrent - startCurrent) / stepCurrent + 1);
            int intSamplesPerBuffer = Convert.ToInt32(period * steps * aOSampleClockRate);
            int stepSamples = (int)(period * aOSampleClockRate);
            double[] rVal = new double[intSamplesPerBuffer];

            for (int i = 0; i < steps; i++)
            {
                for (int j = 0; j < stepSamples; j++)
                    rVal[i * stepSamples + j] = i * stepCurrent;
            }
            return rVal;
        }

        private double[] ProcessPulseSweepData(double[] data, int steps, double period, double dutyCycle, double measureDelay, double integratingPeriod, double sampleClockRate)
        {
            List<double> processedData = new List<double>();
            int periodPoints = (int)(period * sampleClockRate);
            int ignorePoints = (int)(measureDelay * sampleClockRate);
            int integratingPoints = (int)(integratingPeriod * sampleClockRate);
            int dutyPoints = (int)(period * dutyCycle * sampleClockRate);
            for (int i = 0; i < steps; i++)
            {
                double sum = 0;
                double points = 0;
                for (int j = 0; j < periodPoints; j++)
                {
                    if (j >= ignorePoints && j < ignorePoints + integratingPoints && j < dutyPoints)
                    {
                        double datum = data[i * periodPoints + j];
                        sum += datum;
                        points++;
                    }
                }
                processedData.Add(sum / points);
            }
            return processedData.ToArray();
        }
        private double[] ProcessCWSweepData(double[] data, int steps, double period, double measureDelay, double integratingPeriod, double sampleClockRate)
        {
            List<double> processedData = new List<double>();
            int periodPoints = (int)(period * sampleClockRate);
            int ignorePoints = (int)(measureDelay * sampleClockRate);
            int integratingPoints = (int)(integratingPeriod * sampleClockRate);
            for (int i = 0; i < steps; i++)
            {
                double sum = 0;
                double points = 0;
                for (int j = 0; j < periodPoints; j++)
                {
                    if (j > ignorePoints && j <= ignorePoints + integratingPoints)
                    {
                        sum += data[i * periodPoints + j];
                        points++;
                    }
                }
                processedData.Add(sum / points);
            }
            return processedData.ToArray();
        }
        internal void StartExternalTriggerSampling(int loopCount, double integratingTime_s)
        {
            
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }
    }

}
