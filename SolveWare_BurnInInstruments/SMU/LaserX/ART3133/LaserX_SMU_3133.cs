using SolveWare_BurnInCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SolveWare_BurnInInstruments
{
    public class LaserX_SMU_3133 : InstrumentBase, IInstrumentBase
    {
        const int SampleClockRateLimit = 500 * 1000;

        SMU3133Config config = new SMU3133Config();
        static string Properties_Resources = "SolveWare_BurnInInstruments.Properties.Resources";

        public LaserX_SMU_3133(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {

        }


        public override void Initialize()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            ResourceManager rm = new ResourceManager(Properties_Resources, asm);
            var wftRes = rm.GetString("lxsmuconfig");
            XmlSerializer se = new XmlSerializer(typeof(SMU3133Config));
            //XmlReader xr = XElement.Parse(wftRes).CreateReader();
            var element = XElement.Parse(wftRes);

            //element.FirstAttribute.Remove();
            //element.LastAttribute.Remove();
            var xr = new StringReader(element.ToString());
            //config = (SMU3133Config)se.Deserialize(xr);
            base.Initialize();
        }


        /// <summary>
        /// 多次原始数据
        /// 需要处理
        /// </summary>
        public List<double[]> DataSet { get; private set; } = new List<double[]>();


        /// <summary>
        /// 多次多通道数据
        /// 二次处理数据
        /// </summary>
        public List<List<double[]>> DataBook { get; private set; } = new List<List<double[]>>();


        /// <summary>
        /// 回读采集过程的的采样率
        /// </summary>
        public int SampleClockRate { get; private set; } = 0;




        //单硬件线路调节
        public void PDAndMPDRangeAdjust(PDCurrentRange pdRange,int CardNo)//, PDCurrentRange mpdRange)
        {
            List<int> doNos = new List<int>()
            {
                //config.PD1_Range_doCh_1,
                //config.PD1_Range_doCh_2,
                //config.MPD1_Range_doCh_1,
                //config.MPD1_Range_doCh_2
                0,
                1,
            };
            List<bool> vals = new List<bool>();
            switch (pdRange)
            {
                case PDCurrentRange.档位1_20uA:
                    {
                        vals.AddRange(new bool[] { true, true });
                    }
                    break;
                case PDCurrentRange.档位2_200uA:
                    {
                        vals.AddRange(new bool[] { false, true });
                    }
                    break;
                case PDCurrentRange.档位3_2mA:
                    {
                        vals.AddRange(new bool[] { true, false });
                    }
                    break;
                case PDCurrentRange.档位4_20mA:
                    {
                        vals.AddRange(new bool[] { false, false });
                    }
                    break;
            }
            //vals[0] = true;
            //vals[1] = false;

            //switch (mpdRange)
            //{
            //    case PDCurrentRange.档位1_20uA:
            //        {
            //            vals.AddRange(new bool[] { true, true });
            //        }
            //        break;
            //    case PDCurrentRange.档位2_200uA:
            //        {
            //            vals.AddRange(new bool[] { false, true });
            //        }
            //        break;
            //    case PDCurrentRange.档位3_2mA:
            //        {
            //            vals.AddRange(new bool[] { true, false });
            //        }
            //        break;
            //    case PDCurrentRange.档位4_20mA:
            //        {
            //            vals.AddRange(new bool[] { false, false });
            //        }
            //        break;
            //}

            //vals[2] = true;
            //vals[3] = false;


            Dictionary<int, bool> doVals = new Dictionary<int, bool>();

            for (int i = 0; i < doNos.Count; i++)
            {
                doVals.Add(doNos[i], vals[i]);
            }

            WriteMultiDO("LDAndMPDRangeAdjust", CardNo, 0, doVals, config.Timeout_s);
        }

        //双硬件线路，需要根据产品选择对应的线路
        //public void PDAndMPDRangeAdjust(DutType dut, PDCurrentRange pdRange, PDCurrentRange mpdRange)
        //{
        //    List<int> doNos = new List<int>();
        //    List<bool> vals = new List<bool>();
        //    switch (dut)
        //    {
        //        case DutType.Dut_1:
        //            {
        //                doNos.AddRange(new int[]
        //                {
        //                    config.PD1_Range_doCh_1,
        //                    config.PD1_Range_doCh_2,
        //                    config.MPD1_Range_doCh_1,
        //                    config.MPD1_Range_doCh_2
        //                });
        //            }
        //            break;
        //        case DutType.Dut_2:
        //            {
        //                doNos.AddRange(new int[]
        //                {
        //                    config.PD2_Range_doCh_1,
        //                    config.PD2_Range_doCh_2,
        //                    config.MPD2_Range_doCh_1,
        //                    config.MPD2_Range_doCh_2
        //                });
        //            }
        //            break;
        //    }

        //    switch (pdRange)
        //    {
        //        case PDCurrentRange.档位1_20uA:
        //            {
        //                vals.AddRange(new bool[] { true, true });
        //            }
        //            break;
        //        case PDCurrentRange.档位2_200uA:
        //            {
        //                vals.AddRange(new bool[] { false, true });
        //            }
        //            break;
        //        case PDCurrentRange.档位3_2mA:
        //            {
        //                vals.AddRange(new bool[] { true, false });
        //            }
        //            break;
        //        case PDCurrentRange.档位4_20mA:
        //            {
        //                vals.AddRange(new bool[] { false, false });
        //            }
        //            break;
        //    }

        //    switch (mpdRange)
        //    {
        //        case PDCurrentRange.档位1_20uA:
        //            {
        //                vals.AddRange(new bool[] { true, true });
        //            }
        //            break;
        //        case PDCurrentRange.档位2_200uA:
        //            {
        //                vals.AddRange(new bool[] { false, true });
        //            }
        //            break;
        //        case PDCurrentRange.档位3_2mA:
        //            {
        //                vals.AddRange(new bool[] { true, false });
        //            }
        //            break;
        //        case PDCurrentRange.档位4_20mA:
        //            {
        //                vals.AddRange(new bool[] { false, false });
        //            }
        //            break;
        //    }

        //    Dictionary<int, bool> doVals = new Dictionary<int, bool>();

        //    for (int i = 0; i < doNos.Count; i++)
        //    {
        //        doVals.Add(doNos[i], vals[i]);
        //    }

        //    WriteMultiDO("LDAndMPDRangeAdjust", config.CardNo, 0, doVals, config.Timeout_s);
        //}


        /// <summary>
        /// 关闭所有通道输出
        /// </summary>
        public void Reset()
        {
            try
            {
                //待添加
            }
            catch
            {

            }
        }




        public void TecVI_Acq(double acqTime_s)
        {
            int[] aiNos = new int[] { config.TEC1_aiCh };

            MulitiAI_SingleAcq_Finite("TecVI_Acq", aiNos, acqTime_s);
        }

        /// <summary>
        /// 多通道、单次、有限点
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="IntegratingPeriod_s"></param>
        void MulitiAI_SingleAcq_Finite(string taskName, int[] aiNos, double IntegratingPeriod_s)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();
            var cardNo = config.CardNo;
            //int[] aiNos = new int[]
            //{
            //    config.TEC1_aiCh
            //};
            var aITerminal = config.AITerminal;
            var range = config.Range_Tec;
            var sampleClockRate = config.SampleClockRate;
            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }

            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;

            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }
            var sampsPerChanPerLoop = Convert.ToInt32(IntegratingPeriod_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            double[] data = new double[dataCount];
            error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            else
            {
                this.DataSet.Add(data);
            }

            //单次多通道的数据
            var dataList = GetDoubleArrayListFromDoubleArray(this.DataSet.First(), sampsPerChanPerLoop);
            //多次多通道
            this.DataBook.Add(dataList);

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;

        }

        void Acq_TECVol_DoubleDut(double IntegratingPeriod_s)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();

            var taskName = "Acq_TECVol_DoubleDut";
            var cardNo = config.CardNo;
            int[] aiNos = new int[]
            {
                config.TEC1_aiCh,
                config.TEC2_aiCh
            };
            var aITerminal = config.AITerminal;
            var range = config.Range_Tec;
            var sampleClockRate = config.SampleClockRate;
            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }

            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;

            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }
            //var actualSampleClockRate = Convert.ToInt32(sampleClockRate / aiNos.Length);
            //this.SampleClockRate = actualSampleClockRate;
            var sampsPerChanPerLoop = Convert.ToInt32(IntegratingPeriod_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            this.DataSet = new List<double[]>();
            double[] data = new double[dataCount];
            error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            else
            {
                this.DataSet.Add(data);
            }

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();

            //单次多通道的数据
            var dataList = GetDoubleArrayListFromDoubleArray(this.DataSet.First(), sampsPerChanPerLoop);
            //多次多通道
            this.DataBook.Add(dataList);

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;

        }




        public void PowerStability_Acq(int acqTime_s, CancellationToken token)
        {
            Acq_PD_SingleDut("PowerStability_Acq", acqTime_s, token);
        }






        /// <summary>
        /// 采集产品pd1
        /// </summary>
        /// <param name="acqTime_s"></param>
        /// <param name="token"></param>
        void Acq_PD_SingleDut(string taskName, int acqTime_s, CancellationToken token)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();

            var cardNo = config.CardNo;
            int[] aiNos = new int[]
            {
                config.PD1_aiCh,
            };
            var aITerminal = config.AITerminal;
            var range = config.Range;

            var sampleClockRate = config.SampleClockRate;
            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }

            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;

            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }
            //var actualSampleClockRate = sampleClockRate / aiNos.Length;   //采样率



            var sampsPerChanPerLoop = Convert.ToInt32(1 * sampleClockRate);   //缓冲区大小    计划1秒种从缓冲区读取一次数据

            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_ContSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }


            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            var time = acqTime_s * 1000;

            this.DataSet = new List<double[]>();


            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                if (token.IsCancellationRequested)
                {
                    var errmsg = "用户中止读取数据";
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                double[] data = new double[dataCount];
                error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                else
                {
                    this.DataSet.Add(data);
                }
            } while (sw.ElapsedMilliseconds <= time);

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();
            foreach (var item in this.DataSet)
            {
                //单次多通道的数据
                var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
                //多次多通道
                this.DataBook.Add(dataList);
            }

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;
        }


        double[] Acq_PD_SingleDut(double acqTime_s)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();

            double[] data = new double[] { 0 };

            var taskName = "Acq_PD_SingleDut";
            var cardNo = config.CardNo;

            var aiNo = config.PD1_aiCh;

            var aITerminal = config.AITerminal;
            var range = config.Range_Tec;
            var sampleClockRate = config.SampleClockRate;
            var timeout_s = config.Timeout_s;

            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }


            var channel = $"Dev{cardNo}/ai{aiNo}";
            error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }


            var sampsPerChanPerLoop = Convert.ToInt32(acqTime_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop;

            data = new double[dataCount];
            error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return data;
        }


        /// <summary>
        /// 采集双通道pd
        /// </summary>
        /// <param name="acqTime_s"></param>
        /// <param name="token"></param>
        void Acq_PD_DoubleDut(int acqTime_s, CancellationToken token)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();


            var taskName = "Acq_PD_SingleDut";
            var cardNo = config.CardNo;
            int[] aiNos = new int[]
            {
                config.PD1_aiCh,
                config.PD2_aiCh,
            };
            var aITerminal = config.AITerminal;
            var range = config.Range;

            var sampleClockRate = config.SampleClockRate;
            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }

            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;

            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }
            //var actualSampleClockRate = sampleClockRate / aiNos.Length;   //采样率



            var sampsPerChanPerLoop = Convert.ToInt32(1 * sampleClockRate);   //缓冲区大小    计划1秒种从缓冲区读取一次数据

            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_ContSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }


            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            var time = acqTime_s * 1000;

            this.DataSet = new List<double[]>();


            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                if (token.IsCancellationRequested)
                {
                    var errmsg = "用户中止读取数据";
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                double[] data = new double[dataCount];
                error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                else
                {
                    this.DataSet.Add(data);
                }
            } while (sw.ElapsedMilliseconds <= time);

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();
            foreach (var item in this.DataSet)
            {
                //单次多通道的数据
                var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
                //多次多通道
                this.DataBook.Add(dataList);
            }

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;
        }






        public void LIV_Acq_PDMPD(double acqTime_s, int loopCount)
        {
            Acq_PDMPD_SingleDut("LIV_Acq_PDMPD", acqTime_s, loopCount);
        }


        public void LIV_Acq_PD(double acqTime_s, int loopCount)
        {
            //var pfiNo = config.ArtPFICh;
            //int[] aiNos = new int[] { config.PD1_aiCh };
            //int sampleClockRate = 100000;
            //MulitiAI_MulitiAcq_Finite_DigStart("LIV_Acq_PD", aiNos, sampleClockRate, acqTime_s, loopCount, pfiNo);
        }


        /// <summary>
        /// 采集产品pd1、mpd1
        /// </summary>
        /// <param name="IntegratingPeriod_s"></param>
        /// <param name="loopCount"></param>
        void Acq_PDMPD_SingleDut(string taskName, double IntegratingPeriod_s, int loopCount)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();
            var cardNo = config.CardNo;
            int[] aiNos = new int[]
            {
                config.PD1_aiCh,
                config.MPD1_aiCh
            };
            var pfiNo = config.ArtPFICh;
            var aITerminal = config.AITerminal;
            var range = config.Range;
            //var sampleClockRate = config.SampleClockRate;

            var sampleClockRate = 100000;

            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }
            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;
            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }


            var sampsPerChanPerLoop = Convert.ToInt32(IntegratingPeriod_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var triggerSource = $"/Dev{cardNo}/PFI{pfiNo}";
            error = ArtDAQ.ArtDAQ_CfgDigEdgeStartTrig(taskHandle, triggerSource, ArtDAQ.ArtDAQ_Val_Rising);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            error = ArtDAQ.ArtDAQ_SetStartTrigRetriggerable(taskHandle, 1);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }


            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            this.DataSet = new List<double[]>();

            for (int loops = 0; loops < loopCount; loops++)
            {
                double[] data = new double[dataCount];
                error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                else
                {
                    this.DataSet.Add(data);
                }
            }

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();
            foreach (var item in this.DataSet)
            {
                //单次多通道的数据
                var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
                //多次多通道
                this.DataBook.Add(dataList);
            }

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;
        }


        /// <summary>
        /// 采集双通道产品pd、mpd
        /// </summary>
        /// <param name="IntegratingPeriod_s"></param>
        /// <param name="loopCount"></param>
        void Acq_PDMPD_DoubleDut(double IntegratingPeriod_s, int loopCount)
        {


            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();

            var taskName = "Acq_SingleDut_PDMPD";
            var cardNo = config.CardNo;
            int[] aiNos = new int[]
            {
                config.PD1_aiCh,
                config.MPD1_aiCh,
                config.PD2_aiCh,
                config.MPD2_aiCh
            };
            var pfiNo = config.ArtPFICh;
            var aITerminal = config.AITerminal;
            var range = config.Range;
            var sampleClockRate = config.SampleClockRate;

            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }
            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;
            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }

            //var actualSampleClockRate = Convert.ToInt32(sampleClockRate / aiNos.Length);//已知总采样率，求单通道采样率
            //this.SampleClockRate = actualSampleClockRate;

            var sampsPerChanPerLoop = Convert.ToInt32(IntegratingPeriod_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var triggerSource = $"/Dev{cardNo}/PFI{pfiNo}";
            error = ArtDAQ.ArtDAQ_CfgDigEdgeStartTrig(taskHandle, triggerSource, ArtDAQ.ArtDAQ_Val_Rising);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            error = ArtDAQ.ArtDAQ_SetStartTrigRetriggerable(taskHandle, 1);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }


            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            this.DataSet = new List<double[]>();

            for (int loops = 0; loops < loopCount; loops++)
            {
                double[] data = new double[dataCount];
                error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                else
                {
                    this.DataSet.Add(data);
                }
            }

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();
            foreach (var item in this.DataSet)
            {
                //单次多通道的数据
                var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
                //多次多通道
                this.DataBook.Add(dataList);
            }

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;
        }



        public void ThermalDelay_Acq(double acqTime_s)
        {
            Acq_TecAndNtcVol_SingleDut("ThermalDelay_Acq", acqTime_s);
        }




        /// <summary>
        /// 采集单通道tec1,ntc1
        /// </summary>
        /// <param name="IntegratingPeriod_s"></param>
        void Acq_TecAndNtcVol_SingleDut(string taskName, double IntegratingPeriod_s)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();

            var cardNo = config.CardNo;

            int[] aiNos = new int[]
            {
                config.TEC1_aiCh,
                 config.NTC1_aiCh,
            };

            var aITerminal = config.AITerminal;
            var range = config.Range_Tec;

            //var sampleClockRate = config.SampleClockRate;

            var sampleClockRate = 1000;


            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }
            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;

            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }

            //var actualSampleClockRate = Convert.ToInt32(sampleClockRate / aiNos.Length);
            //this.SampleClockRate = actualSampleClockRate;
            var sampsPerChanPerLoop = Convert.ToInt32(IntegratingPeriod_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
            }

            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            this.DataSet = new List<double[]>();
            double[] data = new double[dataCount];
            error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            else
            {
                this.DataSet.Add(data);
            }

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();

            //单次多通道的数据
            var dataList = GetDoubleArrayListFromDoubleArray(this.DataSet.First(), sampsPerChanPerLoop);
            //多次多通道
            this.DataBook.Add(dataList);

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;

        }


        /// <summary>
        /// 采集双通道
        /// </summary>
        /// <param name="IntegratingPeriod_s"></param>
        void Acq_TecAndNtcVol_DoubleDut(double IntegratingPeriod_s)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();


            var taskName = "Acq_TecAndNtcVol_SingleDut";
            var cardNo = config.CardNo;

            int[] aiNos = new int[]
            {
                config.TEC1_aiCh,
                 config.NTC1_aiCh,
                  config.TEC2_aiCh,
                 config.NTC2_aiCh

            };

            var aITerminal = config.AITerminal;
            var range = config.Range_Tec;
            var sampleClockRate = config.SampleClockRate;

            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }
            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;

            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }

            //var actualSampleClockRate = Convert.ToInt32(sampleClockRate / aiNos.Length);
            //this.SampleClockRate = actualSampleClockRate;
            var sampsPerChanPerLoop = Convert.ToInt32(IntegratingPeriod_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
            }

            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            this.DataSet = new List<double[]>();
            double[] data = new double[dataCount];
            error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            else
            {
                this.DataSet.Add(data);
            }

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();

            //单次多通道的数据
            var dataList = GetDoubleArrayListFromDoubleArray(this.DataSet.First(), sampsPerChanPerLoop);
            //多次多通道
            this.DataBook.Add(dataList);

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;

        }



        public void FiberAlignment_Acq_PD(double acqTime_s, int loopCount)
        {
            //var pfiNo = config.PerPFICh;
            //int[] aiNos = new int[] { config.PD1_aiCh };
            //int sampleClockRate = 100000;
            //MulitiAI_MulitiAcq_Finite_DigStart("FiberAlignment_Acq_PD", aiNos, sampleClockRate, acqTime_s, loopCount, pfiNo);

        }


        public void PeaPer_Acq_PD(double acqTime_s, int loopCount,int triggerNo,int samplNo, int CardNo)
        {
            //var pfiNo = config.PerPFICh;
            //int[] aiNos = new int[] { config.PD1_aiCh };
            int[] aiNos = new int[] { samplNo };
            int sampleClockRate = 10000;
            MulitiAI_MulitiAcq_Finite_DigStart("PeaPer_Acq_PD", aiNos, sampleClockRate, acqTime_s, loopCount, triggerNo, CardNo);

        }



        //多通道、多次采集、有限点、数字量触发
        void MulitiAI_MulitiAcq_Finite_DigStart(string taskName, int[] aiNos, int sampleClockRate, double acqTime_s, int loopCount, int pfiNo,int CardNo)
        {
            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();

            var cardNo = CardNo;// config.CardNo;
            //int[] aiNos = new int[]
            //{
            //    config.PD1_aiCh,
            //};
            //var pfiNo = config.PerPFICh;
            var aITerminal = config.AITerminal;
            var range = config.Range;
            //var sampleClockRate = 100000;
            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }
            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;
            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }


            var sampsPerChanPerLoop = Convert.ToInt32(acqTime_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var triggerSource = $"/Dev{cardNo}/PFI{pfiNo}";
            error = ArtDAQ.ArtDAQ_CfgDigEdgeStartTrig(taskHandle, triggerSource, ArtDAQ.ArtDAQ_Val_Rising);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            error = ArtDAQ.ArtDAQ_SetStartTrigRetriggerable(taskHandle, 1);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }


            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            this.DataSet = new List<double[]>();

            for (int loops = 0; loops < loopCount; loops++)
            {
                double[] data = new double[dataCount];
                error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                else
                {
                    this.DataSet.Add(data);
                }
            }

            //多通道数据拆分
            foreach (var item in this.DataSet)
            {
                //单次多通道的数据
                var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
                //多次多通道
                this.DataBook.Add(dataList);
            }

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}] 关闭任务！");
            return;

        }




        public void Acq_Per_SingDut_demo(double acqTime_s, int sampleClockRate = 10000)
        {

            this.DataSet = new List<double[]>();
            this.DataBook = new List<List<double[]>>();

            var taskName = "Acq_Per_SingDut";
            var cardNo = config.CardNo;
            int[] aiNos = new int[]
            {
                config.PD1_aiCh,
            };
            var pfiNo = config.PerPFICh;
            var aITerminal = config.AITerminal;
            var range = config.Range;
            //var sampleClockRate = 100000;

            if (sampleClockRate * aiNos.Length > SampleClockRateLimit)
            {
                var errmsg = $"采样频率超限！采样频率：{sampleClockRate}采样通道数：{aiNos.Length}。采样卡频率上限{SampleClockRateLimit}";
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                return;
            }
            this.SampleClockRate = sampleClockRate;
            var timeout_s = config.Timeout_s;
            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            foreach (var aiNo in aiNos)
            {
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
            }


            var sampsPerChanPerLoop = Convert.ToInt32(acqTime_s * sampleClockRate);
            error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", sampleClockRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var triggerSource = $"/Dev{cardNo}/PFI{pfiNo}";
            error = ArtDAQ.ArtDAQ_CfgDigEdgeStartTrig(taskHandle, triggerSource, ArtDAQ.ArtDAQ_Val_Rising);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            //error = ArtDAQ.ArtDAQ_SetStartTrigRetriggerable(taskHandle, 1);
            //if (error < 0)
            //{
            //    var errmsg = GetErrorString(error);
            //    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
            //    goto Exit;
            //}


            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }

            var dataCount = sampsPerChanPerLoop * aiNos.Length;

            this.DataSet = new List<double[]>();

            //for (int loops = 0; loops < loopCount; loops++)
            //{
            double[] data = new double[dataCount];
            error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle,
                sampsPerChanPerLoop,
                timeout_s,
                ArtDAQ.ArtDAQ_Val_GroupByChannel,
                data,
                (uint)dataCount,
                out int read,
                (IntPtr)(0));
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            else
            {
                this.DataSet.Add(data);
            }
            //}

            //多通道数据拆分
            this.DataBook = new List<List<double[]>>();
            foreach (var item in this.DataSet)
            {
                //单次多通道的数据
                var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
                //多次多通道
                this.DataBook.Add(dataList);
            }

            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;

        }





        [DllImport("msvcrt.dll")]
        public static extern int _getch();



        public void SampleData(double range, double aiSamplingClockRate, int samplesPerLoop, int loopCount)
        {
            IntPtr taskHandle = (IntPtr)(0);

            int DevID = 0;

            int AiChannel_0 = 0;
            int AiChannel_1 = 1;
            int AiChannel_2 = 2;
            int AiChannel_3 = 3;
            int PFIChannel = 0;

            string strTaskName = string.Empty;
            string strCardName = string.Empty;
            string strChannelName = string.Empty;
            string strSource = string.Empty;

            DataSet = new List<double[]>();

            strTaskName = "SampleData";
            //step1: CreateTask
            int error = ArtDAQ.ArtDAQ_CreateTask(strTaskName, out taskHandle);
            strCardName = $"Dev{DevID}";
            strChannelName = $"ai{AiChannel_0}";
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
            strChannelName = $"ai{AiChannel_1}";
            strChannelName = strCardName + "/" + strChannelName;
            //step2: CreateChannel
            error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, strChannelName, "", ArtDAQ.ArtDAQ_Val_Diff, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
            errmsg = GetErrorString(error);
            if (error < 0)
            {
                this._chassis.Log_Global("daqsampler error:" + errmsg);
                goto Exit;
            }
            Thread.Sleep(10);
            strChannelName = $"ai{AiChannel_2}";
            strChannelName = strCardName + "/" + strChannelName;
            //step2: CreateChannel
            error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, strChannelName, "", ArtDAQ.ArtDAQ_Val_Diff, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
            errmsg = GetErrorString(error);
            if (error < 0)
            {
                this._chassis.Log_Global("daqsampler error:" + errmsg);
                goto Exit;
            }
            Thread.Sleep(10);
            strChannelName = $"ai{AiChannel_3}";
            strChannelName = strCardName + "/" + strChannelName;
            //step2: CreateChannel
            error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, strChannelName, "", ArtDAQ.ArtDAQ_Val_Diff, -range, range, ArtDAQ.ArtDAQ_Val_Volts, null);
            errmsg = GetErrorString(error);
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


            var dataCount = samplesPerLoop * 4;

            //var dataCount = samplesPerLoop ;

            DataSet = new List<double[]>();

            //step5: ReadData


            Stopwatch sw = new Stopwatch();

            for (int loops = 0; loops < loopCount; loops++)
            {
                double[] data = new double[dataCount];

                sw.Start();
                error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, samplesPerLoop, 4.0, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                sw.Stop();
                //DataSet.Add(data);
                //Thread.Sleep(1);

                var jkj = sw.ElapsedMilliseconds;

                if (error < 0)
                {
                    errmsg = GetErrorString(error);
                    this._chassis.Log_Global("daqsampler error:" + errmsg);
                    goto Exit;
                }
                else
                {
                    DataSet.Add(data);
                }
            }

            //step5: Exit ReleaseTask
            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            _getch();
            return;
        }








        void GetErrorStr(int errorCode)
        {
            if (errorCode < 0)
            {
                byte[] errorInfo = new byte[2048];
                ArtDAQ.ArtDAQ_GetExtendedErrorInfo(errorInfo, 2048);
                var str = Encoding.Default.GetString(errorInfo);
                throw new Exception(str);
            }
        }

        string GetErrorString(int errorCode)
        {
            byte[] errorInfo = new byte[2048];
            ArtDAQ.ArtDAQ_GetExtendedErrorInfo(errorInfo, 2048);
            string str = Encoding.Default.GetString(errorInfo);
            return str;
        }





        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //  throw new NotImplementedException();
        }





        /// <summary>
        /// 多DO通道单点输出
        /// 仅用于通道端口相同的情景
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="cardNo"></param>
        /// <param name="portNo"></param>
        /// <param name="doNos"></param>
        /// <param name="vals"></param>
        /// <param name="timeout_s"></param>
        void WriteMultiDO(string taskName, int cardNo, int portNo, Dictionary<int, bool> doVal, double timeout_s)
        {
            IntPtr taskHandle = (IntPtr)(0);
            int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            List<bool> vals = new List<bool>();
            foreach (var item in doVal)
            {
                var channel = $"Dev{cardNo}/port{portNo}/line{item.Key}";
                error = ArtDAQ.ArtDAQ_CreateDOChan(taskHandle, channel, "", ArtDAQ.ArtDAQ_Val_ChanForAllLines);
                if (error < 0)
                {
                    var errmsg = GetErrorString(error);
                    this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                    goto Exit;
                }
                vals.Add(item.Value);
            }
            var data = vals.ConvertAll(x => Convert.ToByte(x)).ToArray();
            error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            error = ArtDAQ.ArtDAQ_WriteDigitalLines(taskHandle, 1, 1, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, out int read, (IntPtr)0);
            if (error < 0)
            {
                var errmsg = GetErrorString(error);
                this._chassis.Log_Global($"LaserX_SMU_3133：[{taskName}]error!{errmsg}");
                goto Exit;
            }
            Exit:
            ArtDAQ.ArtDAQ_StopTask(taskHandle);
            ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            return;
        }








        //该方法要求port必须相同
        //void WriteMultiDO(string taskName, int cardNo, int portNo, bool[] vals, double timeout_s)
        //{
        //    IntPtr taskHandle = (IntPtr)(0);
        //    try
        //    {
        //        int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
        //        GetErrorStr(error);

        //        var channel = $"Dev{cardNo}/port{portNo}/line0:7";
        //        error = ArtDAQ.ArtDAQ_CreateDOChan(taskHandle, channel, "", ArtDAQ.ArtDAQ_Val_ChanForAllLines);
        //        GetErrorStr(error);

        //        var data = vals.ToList().ConvertAll(x => Convert.ToByte(x)).ToArray();

        //        error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
        //        GetErrorStr(error);

        //        error = ArtDAQ.ArtDAQ_WriteDigitalLines(taskHandle, 1, 1, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, out int read, (IntPtr)0);
        //        GetErrorStr(error);
        //    }
        //    catch (Exception ex)
        //    {
        //        this._chassis.Log_Global("ART_USB3133A error:" + ex.Message);
        //    }
        //    finally
        //    {
        //        ArtDAQ.ArtDAQ_StopTask(taskHandle);
        //        ArtDAQ.ArtDAQ_ClearTask(taskHandle);
        //    }
        //}




        /// <summary>
        /// 单通道、内部时钟、有限点采样、外部数字触发、多次轮询
        /// 多次有限点采样
        /// 应用于将DAQ当作表使用，开线程使用
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="cardNo"></param>
        /// <param name="aiNo"></param>
        /// <param name="pfiNo"></param>
        /// <param name="aITerminal"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="rate"></param>
        /// <param name="sampsPerChanPerLoop"></param>
        /// <param name="timeout_s"></param>
        /// <param name="loopTimes"></param>
        void ReadSingleAI_IntClk_Finite_DigStart(string taskName, int cardNo, int aiNo, int pfiNo,
            ART_AI_TermCfg aITerminal, double minValue, double maxValue,
            double rate, int sampsPerChanPerLoop, double timeout_s, int loopTimes)
        {
            DataSet = new List<double[]>();
            IntPtr taskHandle = (IntPtr)(0);

            try
            {
                int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
                GetErrorStr(error);
                var channel = $"Dev{cardNo}/ai{aiNo}";
                error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, minValue, maxValue, ArtDAQ.ArtDAQ_Val_Volts, null);
                GetErrorStr(error);
                error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", rate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
                GetErrorStr(error);

                var triggerSource = $"/Dev{cardNo}/PFI{pfiNo}";
                error = ArtDAQ.ArtDAQ_CfgDigEdgeStartTrig(taskHandle, triggerSource, ArtDAQ.ArtDAQ_Val_Rising);
                GetErrorStr(error);
                error = ArtDAQ.ArtDAQ_SetStartTrigRetriggerable(taskHandle, 1);
                GetErrorStr(error);
                error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
                GetErrorStr(error);
                int times = 0;
                while (true)
                {
                    //单次单通道的值
                    double[] data = new double[sampsPerChanPerLoop];
                    error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)sampsPerChanPerLoop, out int read, (IntPtr)(0));
                    if (error < 0)
                    {
                        continue;
                    }
                    //单次多通道的值
                    DataSet.Add(data);
                    times++;
                    if (times >= loopTimes)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this._chassis.Log_Global("ART_USB3133A error:" + ex.Message);
            }
            finally
            {
                ArtDAQ.ArtDAQ_StopTask(taskHandle);
                ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            }
        }

        /// <summary>
        /// 多通道
        /// 多次有限点采样
        /// 应用于将DAQ当作表使用，开线程使用
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="cardNo"></param>
        /// <param name="aiNos"></param>
        /// <param name="pfiNo"></param>
        /// <param name="aITerminal"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="rate"></param>
        /// <param name="sampsPerChanPerLoop"></param>
        /// <param name="timeout_s"></param>
        /// <param name="loopTimes"></param>
        void ReadMultiAI_IntClk_Finite_DigStart(string taskName, int cardNo, int[] aiNos, int pfiNo,
           ART_AI_TermCfg aITerminal, double minValue, double maxValue,
           double rate, int sampsPerChanPerLoop, double timeout_s, int loopTimes)
        {
            IntPtr taskHandle = (IntPtr)(0);
            Stopwatch sw = new Stopwatch();
            try
            {
                int error = ArtDAQ.ArtDAQ_CreateTask(taskName, out taskHandle);
                GetErrorStr(error);
                foreach (var aiNo in aiNos)
                {
                    var channel = $"Dev{cardNo}/ai{aiNo}";
                    error = ArtDAQ.ArtDAQ_CreateAIVoltageChan(taskHandle, channel, "", (int)aITerminal, minValue, maxValue, ArtDAQ.ArtDAQ_Val_Volts, null);
                    GetErrorStr(error);
                }
                var actualRate = rate / aiNos.Length;
                error = ArtDAQ.ArtDAQ_CfgSampClkTiming(taskHandle, "", actualRate, ArtDAQ.ArtDAQ_Val_Rising, ArtDAQ.ArtDAQ_Val_FiniteSamps, sampsPerChanPerLoop);
                GetErrorStr(error);
                var triggerSource = $"/Dev{cardNo}/PFI{pfiNo}";
                error = ArtDAQ.ArtDAQ_CfgDigEdgeStartTrig(taskHandle, triggerSource, ArtDAQ.ArtDAQ_Val_Rising);
                GetErrorStr(error);
                error = ArtDAQ.ArtDAQ_SetStartTrigRetriggerable(taskHandle, 1);
                GetErrorStr(error);
                error = ArtDAQ.ArtDAQ_StartTask(taskHandle);
                GetErrorStr(error);

                var dataCount = sampsPerChanPerLoop * aiNos.Length;
                var dataset = new List<double[]>();


                //for (int times = 0; times < loopTimes; times++)
                //{
                //    var data = new double[dataCount];
                //    error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                //    dataset.Add(data);
                //    Thread.Sleep(1);
                //}


                sw.Start();
                int times = 0;
                while (true)
                {
                    //单次多通道的数据
                    var data = new double[dataCount];
                    error = ArtDAQ.ArtDAQ_ReadAnalogF64(taskHandle, sampsPerChanPerLoop, timeout_s, ArtDAQ.ArtDAQ_Val_GroupByChannel, data, (uint)dataCount, out int read, (IntPtr)(0));
                    if (sw.ElapsedMilliseconds > 10000)
                    {
                        throw new Exception("采集时间超时！");
                    }
                    if (error < 0)
                    {
                        continue;
                    }

                    //多次多通道的数据
                    dataset.Add(data);
                    times++;
                    if (times >= loopTimes)
                    {
                        break;
                    }
                }

                //多通道数据拆分
                foreach (var item in dataset)
                {
                    //单次多通道的数据
                    var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
                    //多次多通道
                    DataBook.Add(dataList);
                }
            }
            catch (Exception ex)
            {
                this._chassis.Log_Global("ART_USB3133A error:" + ex.Message);
            }
            finally
            {
                sw.Stop();
                ArtDAQ.ArtDAQ_StopTask(taskHandle);
                ArtDAQ.ArtDAQ_ClearTask(taskHandle);
            }
        }










        /// <summary>
        /// 将数组中的值拆分成多通道的值
        /// </summary>
        /// <param name="dataArray"></param>
        /// <param name="numberOfSamples"></param>
        /// <returns></returns>
        List<double[]> GetDoubleArrayListFromDoubleArray(double[] dataArray, int numberOfSamples)
        {

            List<double[]> data_onceRead = new List<double[]>();

            var channelCount = dataArray.Length / numberOfSamples;

            var dataList = dataArray.ToList();

            for (int i = 0; i < channelCount; i++)
            {
                var start = numberOfSamples * i;

                var temp = dataList.GetRange(start, numberOfSamples);

                var data = temp.ToArray();

                data_onceRead.Add(data);
            }
            return data_onceRead;
        }


        //public List<List<double[]>> GetDAQDataAfterHandle()
        //{
        //    List<List<double[]>> data = new List<List<double[]>>();

        //    foreach (var item in this.DataSet)
        //    {
        //        //单次多通道的数据
        //        var dataList = GetDoubleArrayListFromDoubleArray(item, sampsPerChanPerLoop);
        //        //多次多通道
        //        data.Add(dataList);
        //    }
        //    return data;
        //}
    }


    public class InternalParam_DAQ
    {
        //2602
        public const int KeithleyTriggerOutChannel = 1;

        //9078板卡
        public const int LX9078TriggerOutChannel = 0;

    }
}
