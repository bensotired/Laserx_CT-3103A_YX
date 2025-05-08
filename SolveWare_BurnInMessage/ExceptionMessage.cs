
using SolveWare_BurnInCommon;
using System;

namespace SolveWare_BurnInMessage
{
    public class ExceptionMessage : MessageBase, IMessage
    {
        public int ErrorCode { get; private set; }
        //public ExpectedException ExceptionType { get; private set; }
        public Exception Exception { get; private set; }
        public string DateTimeString { get; private set; }
        public ExceptionMessage(string message, int errorCode, Exception e, object context)
        //public ExceptionMessage(string message, ExpectedException expectedException, Exception e, object context)
            : base(message, context)
        {
            this.DateTimeString = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            this.Type = EnumMessageType.Exception;
            this.ErrorCode = errorCode;
            this.Exception = e;
        }
        public ExceptionMessage(string message, int errorCode, Exception e)
        //public ExceptionMessage(string message, ExpectedException expectedException, Exception e)
         : base(message, null)
        {
            this.DateTimeString = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            this.Type = EnumMessageType.Exception;
            this.ErrorCode = errorCode;
            this.Exception = e;
        }
        public ExceptionMessage(string message, int errorCode)
        //public ExceptionMessage(string message, ExpectedException expectedException)
        : base(message, null)
        {
            this.DateTimeString = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            this.Type = EnumMessageType.Exception;
            this.ErrorCode = errorCode;
        }
        public override string ToString()
        {
            return GetDatetimeMessage(this);
        }

        string GetDatetimeMessage(IMessage msg)
        {
            var sourceMessage = msg as ExceptionMessage;
            string exLog = $"[{this.DateTimeString}][{sourceMessage.ErrorCode}][{sourceMessage?.Message}]";
            //string exLog = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}][{sourceMessage.ExceptionType}][{sourceMessage?.Message}][{sourceMessage.Exception?.StackTrace}]";
            return exLog;
        }

    }
}