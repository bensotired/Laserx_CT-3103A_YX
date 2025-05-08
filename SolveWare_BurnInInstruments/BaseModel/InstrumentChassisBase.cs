using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;

namespace SolveWare_BurnInInstruments
{
    public   class InstrumentChassisBase : IInstrumentChassis
    {
        
        //ExpectedException defaultAlarmType = ExpectedException.INSTU_CHASSIS_ALRAM;
        protected readonly object mutex = new object();
        protected  volatile bool _canAccess;
        Modbus _modbus;
        ILogHandle _logHandler;
        IExceptionHandle _exceptionHandler;
        public event InstrumentChassisEventHandler InstrumentChassisEvent;
        public int DefaultBufferSize { get; set; } = 1024;
        public Modbus Modbus
        {
            get
            {
                if (_modbus == null)
                {
                    lock (mutex)
                    {
                        if (_modbus == null)
                        {
                            _modbus = new Modbus(this);
                        }
                    }
                }
                return _modbus;
            }
        }

        public virtual object Visa
        {
            get
            {
                return this;
            }
        }
        public virtual int Timeout_ms
        {
            get;
            set;
        }
        public int InitTimeout_ms
        {
            get;
            set;
        }
        //Action<ExpectedException, string, string> ErrorReportAction
        //{
        //    get;
        //    set;
        //}
        public string Resource { get; protected set; }
        public string Name { get; protected set; }
        public bool IsOnline { get; protected set; }
        //public InstrumentChassisBase(string name, string resource, bool isOnline, Action<string> errorReportAction)
        public InstrumentChassisBase(string name, string resource, bool isOnline/*, Action<ExpectedException, string, string> errorReportAction*/)
        {
            this.Name = name;
            this.Resource = resource;
            this.IsOnline = isOnline;
            //this.ErrorReportAction = errorReportAction;
        }
        public virtual void SetupLogger(ILogHandle logHandler, IExceptionHandle exceptionHandler)
        {
            this._logHandler = logHandler;
            this._exceptionHandler = exceptionHandler;
        }

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public virtual void Initialize(int timeout)
        {
            throw new NotImplementedException();
        }
        public virtual void WaitRuning()
        {
            throw new NotImplementedException();
        }
        protected virtual void OnTurnOffline()
        {
            if (this.InstrumentChassisEvent != null)
            {
                var ret = this.InstrumentChassisEvent(this, new InstrumentChassisArgs(InstrumentChassisArgsType.TurnOffline));
            }
            this.IsOnline = false;
        }
        protected virtual void OnTurnOnline()
        {
            this.IsOnline = true;
            if (this.InstrumentChassisEvent != null)
            {
                var ret = this.InstrumentChassisEvent(this, new InstrumentChassisArgs(InstrumentChassisArgsType.TurnOnline));
            }

        }
        //protected virtual void OnTurnOnSimulation()
        //{
        //    this.IsOnline = true;
        //    if (this.InstrumentChassisEvent != null)
        //    {
        //        var ret = this.InstrumentChassisEvent(this, new InstrumentChassisArgs(InstrumentChassisArgsType.TurnOnSimnulation));
        //    }

        //}
        protected virtual void OnAllocateChassisResouce()
        {
            if (this.InstrumentChassisEvent != null)
            {
                var ret = this.InstrumentChassisEvent(this, new InstrumentChassisArgs(InstrumentChassisArgsType.AllocateChassisResouce));
            }

        }
        public virtual void TurnOnline(bool isOnline)
        {
            if (isOnline)
            {
                this.OnTurnOnline();
            }
            else
            {
                OnTurnOffline();
            }
        }
        //public virtual void TurnOnSimulation()
        //{
        //    OnTurnOnSimulation();
        //}


        public virtual bool CanAccess
        {
            get { return _canAccess; }
        }

        public int BytesToRead
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual void ReportException(string errorMsg)
        {
            this._exceptionHandler?.ReportException(errorMsg, ErrorCodes.InstrumentChassisException);
        }
        public void ReportException(string message, int errorCode)
        {
            this._exceptionHandler?.ReportException(message, errorCode);
        }

        public void ReportException(string message, int errorCode, Exception e)
        {
            this._exceptionHandler?.ReportException(  message,   errorCode,   e);
            //throw new NotImplementedException();
        }

        public void ReportException(string message, int errorCode, Exception e, object context)
        {
            this._exceptionHandler?.ReportException(message, errorCode, e, context);
            //throw new NotImplementedException();
        }

  

        public virtual void FormattedLog_Global(string format, params object[] args)
        {
            this._logHandler?.FormattedLog_Global(format, args);
        }

        public virtual void Log_Global(string log)
        {
            this._logHandler?.Log_Global(log);
        }
        public virtual byte[] Query(byte[] cmd, int delay_ms)
        {
            throw new NotImplementedException();
        }
        public virtual byte[] Query(byte[] cmd, int bytesToRead, int delay_ms)
        {
            throw new NotImplementedException();
        }
        public virtual string Query(string cmd, int delay_ms)
        {
            throw new NotImplementedException();
        }
        public virtual void TryWrite(byte[] cmd)
        {
            throw new NotImplementedException();
        }
        public virtual void TryWrite(string cmd)
        {
            throw new NotImplementedException();
        }

        public virtual void ClearConnection()
        {
            throw new NotImplementedException();
        }
        public virtual void BuildConnection(int timeout_ms)
        {
            throw new NotImplementedException();
        }
        public virtual void ConnectToResource(int timeout_ms, bool forceOnline = false)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] QueryWithLongResponTime(byte[] cmd, int bytesToRead, int delay_ms, int respon_ms)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] QueryWithLongResponTime(byte[] cmd, int delay_ms, int respon_ms)
        {
            throw new NotImplementedException();
        }

        public virtual string QueryWithLongResponTime(string cmd, int delay_ms, int respon_ms)
        {
            throw new NotImplementedException();
        }
        public virtual void Read(byte[] buffer, int offset, int bytesToRead)
        {
            throw new NotImplementedException();
        }
    }
}