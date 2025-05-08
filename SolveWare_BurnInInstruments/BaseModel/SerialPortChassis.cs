using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using SolveWare_BurnInCommon;


namespace SolveWare_BurnInInstruments
{
    public class SerialPortChassis : InstrumentChassisBase, IInstrumentChassis
    {
        //const int DefaultBufferSize = 1024;

        const int MAX_RETRIES = 3;

        private SerialPort _serialPort;
        //public SerialPort _serialPort;
        string _PortName;
        int _BaudRate;
        int _DataBits;
        Parity _Parity;
        StopBits _StopBits;
        public object SyncObj = new object();
        public SerialPortChassis(string name, string resource, bool isOnline)
            : base(name, resource, isOnline)
        {
            try
            {
                string[] props = resource.Split(',');
                _PortName = props[0].ToUpper();
                _BaudRate = Convert.ToInt32(props[1]);
                _DataBits = Convert.ToInt16(props[2]);
                _Parity = (Parity)Enum.Parse(typeof(Parity), props[3]);
                _StopBits = (StopBits)Enum.Parse(typeof(StopBits), props[4]);
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Constructor error, Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg, ex);
            }
        }
        public override void WaitRuning()
        {

        }

        public override object Visa
        {
            get { return this._serialPort; }
        }

        public override void Initialize()
        {
            Initialize(5000);
        }


        public override void Initialize(int timeout_ms)
        {
            if (IsOnline == false)
            {
                return;
            }
            try
            {
                _serialPort = new SerialPort();
                _serialPort.PortName = _PortName;
                _serialPort.BaudRate = _BaudRate;
                _serialPort.DataBits = _DataBits;
                _serialPort.Parity = _Parity;
                _serialPort.StopBits = _StopBits;
                _serialPort.WriteTimeout = timeout_ms;
                _serialPort.ReadTimeout = timeout_ms;  //20200617 增加超时设定
                if (_serialPort.IsOpen == false)
                {
                    _serialPort.Open();
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]",
                                       this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);
            }
        }
        public override void ClearConnection()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                this._serialPort?.Close();
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Clear connection fails. Chassis resource = [{1}].[{2}]-[{3}]",
                                                  this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
            }
        }
        public override void BuildConnection(int timeout_ms)
        {
            if (IsOnline == false)
            {
                return;
            }
            try
            {
                _serialPort = new SerialPort();
                _serialPort.PortName = _PortName;
                _serialPort.BaudRate = _BaudRate;
                _serialPort.DataBits = _DataBits;
                _serialPort.Parity = _Parity;
                _serialPort.StopBits = _StopBits;
                _serialPort.WriteTimeout = timeout_ms;
                _serialPort.ReadTimeout = timeout_ms;  //20200617 增加超时设定
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] BuildConnection error. Chassis resource = [{1}].[{2}]-[{3}]",
                                       this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
            }
        }

        /// <summary>
        /// write string function no retry
        /// </summary>
        /// <param name="data"></param>
        private void Write(string cmd)
        {
            if (!this.IsOnline) return;
            do
            {
                try
                {
                    Byte[] writeData = Encoding.ASCII.GetBytes(cmd);
                    this._serialPort.Write(cmd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } while (false);
        }
        /// <summary>
        /// write bytes function no retry
        /// </summary>
        /// <param name="data"></param>
        private void Write(byte[] cmd)
        {
            if (!this.IsOnline) return;
            do
            {
                try
                {
                    this._serialPort.ReadExisting();
                    this._serialPort.Write(cmd, 0, cmd.Length);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } while (false);
        }
        public override void TryWrite(string cmd)
        {
            if (!this.IsOnline) return;

            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        this.Write(cmd);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] TryWrite(string cmd), int bytesToRead) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] TryWrite(string cmd) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
        }
        public override void TryWrite(byte[] cmd)
        {
            if (!this.IsOnline) return;

            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        this.Write(cmd);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] TryWrite(byte[] cmd), int bytesToRead) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] TryWrite(byte[] cmd) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
        }
        public  int BytesToRead 
        {
            get
            {
                if (!this.IsOnline) return 0;
                return this._serialPort.BytesToRead;
            }
           
        }
        public override string Query(string cmd, int delay_ms)
        {
            if (!this.IsOnline) return string.Empty;
            var resp = "";
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        //if(_serialPort.IsOpen==false)
                        //{
                        //    _serialPort.Open();
                        //}
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = this._serialPort.ReadExisting();
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);

                return resp;
            }
        }
        public override byte[] Query(byte[] cmd, int delay_ms)
        {
            if (!this.IsOnline) return new byte[1];
            byte[] resp = new byte[1];
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = new byte[this._serialPort.BytesToRead];
                        this._serialPort.Read(resp, 0, this._serialPort.BytesToRead);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
            return resp;
        }
        public override byte[] Query(byte[] cmd, int bytesToRead, int delay_ms)
        {
            if (!this.IsOnline) return new byte[1];
            byte[] resp = new byte[1];
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        string str = this._serialPort.ReadExisting();
                        this._serialPort.Write(cmd, 0, cmd.Length);
                        System.Threading.Thread.Sleep(delay_ms);
                        resp = new byte[bytesToRead];
                        this._serialPort.Read(resp, 0, bytesToRead);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);

                            // this.ClearConnection();  //20220118 把串口资源释放掉
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
            return resp;
        }
        public override void Read(byte[] buffer, int offset, int bytesToRead)
        {
            if (!this.IsOnline) return;

            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        this._serialPort.Read(buffer, offset, bytesToRead);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Read(byte[] buffer,  int bytesToRead ) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Read(byte[] buffer,  int bytesToRead ) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);

                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
        }
        protected override void OnTurnOnline()
        {
            lock (this)
            {
                this.IsOnline = true;
               
                this.BuildConnection(5000);
            }
            base.OnTurnOnline();
        }
        protected override void OnTurnOffline()
        {
            lock (this)
            {
                this.ClearConnection();
            }
            //base.OnTurnOnline()
            base.OnTurnOffline();
        }
    }
}