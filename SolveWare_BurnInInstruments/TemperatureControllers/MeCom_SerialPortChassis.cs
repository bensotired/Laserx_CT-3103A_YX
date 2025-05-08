using MeSoft.MeCom.PhyWrapper;
 
using System;
using System.IO.Ports;

namespace SolveWare_BurnInInstruments
{
    public class MeCom_SerialPortChassis : InstrumentChassisBase, IInstrumentChassis
    {
        private MeComPhySerialPort _meComPhySerialPort;
        string _PortName;
        int _BaudRate;
        int _DataBits;
        Parity _Parity;
        StopBits _StopBits;
        public object SyncObj = new object();
        public MeCom_SerialPortChassis(string name, string resource, bool isOnline )
            : base(name, resource, isOnline )
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
            get { return this._meComPhySerialPort; }
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
                _meComPhySerialPort = new MeComPhySerialPort();
                _meComPhySerialPort.PortName = _PortName;
                _meComPhySerialPort.BaudRate = _BaudRate;
                _meComPhySerialPort.DataBits = _DataBits;
                _meComPhySerialPort.Parity = _Parity;
                _meComPhySerialPort.StopBits = _StopBits;
                _meComPhySerialPort.WriteTimeout = timeout_ms;
                _meComPhySerialPort.ReadTimeout = timeout_ms;  //20200617 增加超时设定
                _meComPhySerialPort.Open();
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
                this._meComPhySerialPort?.Close();
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
                _meComPhySerialPort = new MeComPhySerialPort();
                _meComPhySerialPort.PortName = _PortName;
                _meComPhySerialPort.BaudRate = _BaudRate;
                _meComPhySerialPort.DataBits = _DataBits;
                _meComPhySerialPort.Parity = _Parity;
                _meComPhySerialPort.StopBits = _StopBits;
                _meComPhySerialPort.WriteTimeout = timeout_ms;
                _meComPhySerialPort.ReadTimeout = timeout_ms;  //20200617 增加超时设定
                _meComPhySerialPort.Open();
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] BuildConnection error. Chassis resource = [{1}].[{2}]-[{3}]",
                                       this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);

            }
        }
    }
}