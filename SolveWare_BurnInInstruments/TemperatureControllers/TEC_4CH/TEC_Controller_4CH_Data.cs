using MeSoft.MeCom.Core;
using MeSoft.MeCom.PhyWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
 
    public partial class TEC_Controller_4CH : InstrumentBase, IInstrumentBase
    {
        //线程对象
        protected Thread _thUpdateState; //更新当前状态线程
        //接收超时
        protected int _timeout_ms = 5000;

        //设备是否连接  true:已经连接  false:未连接
        public bool Connected
        {
            get
            {
                //if (this._isSimulation) return false;
                if (this._isOnline == false) return false;
                switch (this.PhysicalConnection)

                {
                    case PhysicalConnection.SerialPort:
                        {
                            if (this._chassis?.IsOnline == false)
                            {
                                return false;
                            }
                            if (this._chassis?.Visa == null)
                            {
                                return false;
                            }
                            else
                            {
                                return ((MeComPhySerialPort)this._chassis.Visa).IsOpen;
                            }
                        }
                        break;

                    case PhysicalConnection.TCPIP:
                        {
                            if (this._chassis?.IsOnline == false)
                            {
                                return false;
                            }
                            if (this._chassis?.Visa == null)
                            {
                                return false;
                            }
                        }
                        break;
                    default:
                        break;
                }
         
                return false;
            }
        }
        //lock 顺序操作
        public object objlock = new object();

        //是否需要连接本设备  true:连接  false:不连接
        //private bool _online = false;
        //public bool Online
        //{
        //    get { return _online; }
        //}


        #region 通讯参数

        //端口参数
        protected string _portName = null;  //端口名
        protected string _ipAddress = null;
        protected string _port = null;
        protected int _DeviceID; //板子的ID
        public int DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }
        public string IpAddress { get { return _ipAddress; } }
        public int Port { get { return int.Parse(_port); } }
        public string PortName { get { return _portName; } }

        protected int _baudRate = 57600;   //波特率

        protected string _DeviceType = "TEC"; //设备类型名称

        //DLL库运行情况
        //protected MeComPhyTcp _meComPhyTcp;
        //protected MeComPhySerialPort _meComPhySerialPort;  //物理连接meComPhySerialPort
        //protected MeComQuerySet _meComQuerySet; //通讯核心

        //protected MeComBasicCmd _meComBasicCmd; //G1指令
        //protected MeComG2Cmd _meComG2Cmd;    //G2指令

        #endregion




        #region 只读数据内容
        //当前对象即时温度
        protected double[] _CurrentObjectTemp = new double[5];//????
        public double[] CurrentObjectTemperature
        {
            get
            {
                //if (this._isSimulation == true) return _TargetObjectTemp; //如果仿真模式,直接返回设定温度
                if (this._isOnline == false) return _TargetObjectTemp; //如果离线模式,直接返回设定温度

                return _CurrentObjectTemp; //返回温度
            }
        }
      

        //TEC电流
        protected double[] _CurrentTEC_A = new double[5];
        public double[] CurrentTEC_A
        {
            get
            {
                return _CurrentTEC_A; //返回电流
            }
        }

        //TEC电压
        protected double[] _CurrentTEC_V = new double[5];
        public double[] CurrentTEC_V
        {
            get
            {
                return _CurrentTEC_V; //返回电流
            }
        }

        //温度已经稳定
        protected bool[] _TemperatureStability = new bool[5];
        public bool[] TemperatureStability
        {
            get
            {
                return _TemperatureStability;
            }
        }

        protected double[] _AutoTuneProcess = new double[5];

        public double[] AutoTuningProcess
        {
            get
            {
                return _AutoTuneProcess; //返回进度
            }
        }

        protected double[] _PID_P = new double[5];

        public double[] PID_P
        {
            get
            {
                return _PID_P; //返回
            }
        }

        protected double[] _PID_I = new double[5];

        public double[] PID_I
        {
            get
            {
                return _PID_I; //返回
            }
        }
        protected double[] _PID_D = new double[5];

        public double[] PID_D
        {
            get
            {
                return _PID_D; //返回
            }
        }
     
        #region 运行模式
        protected int?[] _OutputStageMode = new int?[3];
        public int?[] OutputStageMode
        {
            get
            {
                return _OutputStageMode;
            }
        }

        public string OutputStageModeStr(int ch)
        {
            //0: Static Current/Voltage (Uses ID 2020…)
            //1: Live Current/Voltage (Uses ID 50001…)
            //2: Temperature Controller

            switch (_OutputStageMode[ch])
            {
                case 0:
                    return "Static Current/Voltage";
                case 1:
                    return "Live Current/Voltage";
                case 2:
                    return "Temperature Controller";
            }
            return "NA";
        }
        #endregion

        #region 运行状态

        protected int?[] _GetDeviceStatus = new int?[5];
        public int?[] GetDeviceStatus
        {
            get
            {
                return _GetDeviceStatus;
            }
        }

        public string GetDeviceStatusStr(int _DeviceStatus)
        {
            //0: Init
            //1: Ready
            //2: Run
            //3: Error
            //4: Bootloader
            //5: Device will Reset within next 200ms

            switch (_DeviceStatus)
            {
                case 0:
                    return "Init";
                case 1:
                    return "Ready";
                case 2:
                    return "Run";
                case 3:
                    return "Error";
                case 4:
                    return "Bootloader";
                case 5:
                    return "Device will Reset within next 200ms";
            }
            return "NA";
        }

    

        #endregion

        #region 错误代码

        protected int?[] _GetErrorNumber = new int?[3];
        public int?[] GetErrorNumber
        {
            get
            {
                return _GetErrorNumber;
            }
        }


        #endregion
        #endregion


        #region 读写数据内容

        //对象需要达到的温度
        protected double[] _TargetObjectTemp = new double[5];
        public double[] TargetObjectTemperature
        {
            get { return _TargetObjectTemp; }
        }
        #endregion

        //当前的运行状态

        //设备是否连接



        public eDeviceStatus DeviceStatus
        {
            get
            {
                //return TECControler.eDeviceStatus.Error;
                return eDeviceStatus.Error;
                //string response = string.Empty;
                //lock (this.Chassis)
                //{
                //    string command = BuildCommand("?VR", (int)MeerstetterTECCommands.DeviceStatus, "");

                //    Chassis.Write(command);
                //    Thread.Sleep(200);
                //    response = GetResponseFrame(Chassis.ReadString()).Substring(7, 2);
                //}

                //var vale = Convert.ToInt32(response, 16);
                //return (DeviceStatus)vale;
            }
        }


        //当前的报警编号与内容

        //被控对象温度

        //温度是否已经达到需求



        //读写数据内容

        //设定被控的目标温度

        //设定目标温度的允许上下限

        //设定目标温度达到后的延迟时间
    }
}