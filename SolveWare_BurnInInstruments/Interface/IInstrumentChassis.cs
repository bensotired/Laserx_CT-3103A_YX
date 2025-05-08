using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;

namespace SolveWare_BurnInInstruments
{
    public interface IInstrumentChassis ///: IExceptionHandle//://ILogHandle, 
    {
        int InitTimeout_ms { get; set; }
        int DefaultBufferSize { get; set; }
        void SetupLogger(ILogHandle logHandler, IExceptionHandle exceptionHandler);
        event InstrumentChassisEventHandler InstrumentChassisEvent;
        Modbus Modbus { get; }
        object Visa { get; }
        void Initialize();
        void Initialize(int timeout);
        void WaitRuning();  
        string Resource { get; }
        string Name { get;  } 
        bool IsOnline { get;  }
        void TurnOnline(bool isOnline);
        //void TurnOnSimulation();
        void TryWrite(string cmd);
        void TryWrite(byte[] msg);
        byte[] Query(byte[] cmd, int bytesToRead, int delay_ms);
        int BytesToRead { get; }
        byte[] Query(byte[] cmd, int delay_ms);
        string Query(string cmd, int delay_ms);

        byte[] QueryWithLongResponTime(byte[] cmd, int bytesToRead, int delay_ms, int respon_ms);
        byte[] QueryWithLongResponTime(byte[] cmd, int delay_ms, int respon_ms);
        string QueryWithLongResponTime(string cmd, int delay_ms, int respon_ms);

        void ClearConnection();
        void BuildConnection(int timeout_ms);
        void ConnectToResource(int timeout_ms, bool forceOnline = false);
        bool CanAccess { get; }
        int Timeout_ms { get; set; }

        void FormattedLog_Global(string format, params object[] args);
        void Log_Global(string log);
        //void ReportException(string errorMsg);
        //void ReportException(string errorMsg, ExpectedException eeType);
        //void ReportException(string errorMsg, ExpectedException eeType, Exception ex);
        void ReportException(string message, int errorCode);
        void ReportException(string message, int errorCode, Exception e);
        void ReportException(string message, int errorCode, Exception e, object context);
        void Read(byte[] buffer, int offset, int bytesToRead);
    }
}
