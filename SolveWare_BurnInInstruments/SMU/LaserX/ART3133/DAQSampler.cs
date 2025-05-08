using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class DAQSampler: InstrumentBase
    {

        [DllImport("msvcrt.dll")]
        public static extern int _getch();
        [DllImport("msvcrt.dll")]
        public static extern int _kbhit();

        Int32 error = 0;
        string errorInfo = new string('0', 2048);
        IntPtr taskHandle = (IntPtr)(0);
        Int32 read = 0;
        public List<double[]> DataSet { get; set; }
        string strTaskName = string.Empty;
        string strCardName = string.Empty;
        string strChannelName = string.Empty;
        string strSource = string.Empty;

        int DevID = 1;
        int AiChannel = 15;
        int PFIChannel = 0;
        //DAQSampler daqSampler = new DAQSampler(1, 15, 0);
        public DAQSampler(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
            var addr = chassis.Resource.Split(',');//1, 15, 0
            this.DevID = Convert.ToInt16(addr[0]);
            this.AiChannel = Convert.ToInt16(addr[1]);
            this.PFIChannel = Convert.ToInt16(addr[2]);
        }
        //public DAQSampler(int deviceID, int aiChannel, int pFIChannel)
        //{
        //    this.DevID = deviceID;
        //    this.AiChannel = aiChannel;
        //    this.PFIChannel = pFIChannel;
        //}
        private string GetErrorString(Int32 errorCode)
        {
            byte[] errorInfo = new byte[2048];
            ArtDAQ.ArtDAQ_GetExtendedErrorInfo(errorInfo, 2048);
            string str = Encoding.Default.GetString(errorInfo);
            return str;
        }
        public void Reset()
        {
            try
            {
                ArtDAQ.ArtDAQ_StopTask(taskHandle); 
                Thread.Sleep(5);
                ArtDAQ.ArtDAQ_ClearTask(taskHandle);
                Thread.Sleep(5);
                _getch();
                Thread.Sleep(5);
            }
            catch
            {

            }
        }
        public void SampleData(double range, double aiSamplingClockRate, int samplesPerLoop, int loopCount)
        {
            DataSet = new List<double[]>();
            strTaskName = "";
            //step1: CreateTask
            error = ArtDAQ.ArtDAQ_CreateTask(strTaskName, out taskHandle);
            strCardName = $"Dev{DevID}";
            strChannelName = $"ai{AiChannel}";
            strChannelName = strCardName + "/" + strChannelName;
            //step2: CreateChannel
            error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, strChannelName, "", ArtDAQ.ArtDAQ_Val_Diff, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
            var errmsg = GetErrorString(error);
            if (error < 0)
            {
                this._chassis.Log_Global("daqsampler error:" + errmsg);
                goto Exit;
            }
            Thread.Sleep(10);
            //step3: Configure Clock
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", aiSamplingClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, samplesPerLoop);
            errmsg = GetErrorString(error);

            if (error < 0)
            {
                this._chassis.Log_Global("daqsampler error:" + errmsg);
                goto Exit;
            }
            Thread.Sleep(10);
            //step4: Configure trigger
            strSource = $"/Dev{DevID}/PFI{PFIChannel}";
            error = ArtDAQ.ArtDAQ_CfgDigEdgeStartTrig(taskHandle, strSource, ArtDAQ.ArtDAQ_Val_Rising);
            error = ArtDAQ.ArtDAQ_SetStartTrigRetriggerable(taskHandle, 1);
            errmsg = GetErrorString(error);
            if (error < 0)
            {
                this._chassis.Log_Global("daqsampler error:" + errmsg);
                goto Exit;
            }
            Thread.Sleep(10);

            //step4: Start Task
            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            errmsg = GetErrorString(error);
            if (error < 0)
            {
                this._chassis.Log_Global("daqsampler error:" + errmsg);
                goto Exit;
            }
            Thread.Sleep(10);

            //step5: ReadData

            for (int loops = 0;loops < loopCount; loops++)
            {
                double[] data = new double[samplesPerLoop];
 
                error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, samplesPerLoop, 4.0, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)samplesPerLoop, out read, (IntPtr)(0));
                DataSet.Add(data);
                Thread.Sleep(1);
  
                errmsg =  GetErrorString(error);
                if (error < 0)
                {
                    this._chassis.Log_Global("daqsampler error:" + errmsg);
                    goto Exit;
                }                
            }
         
        //step5: Exit ReleaseTask
        Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            _getch();
            return;
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
           // throw new NotImplementedException();
        }
    }
}
