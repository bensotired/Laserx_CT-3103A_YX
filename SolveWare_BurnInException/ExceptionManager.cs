using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInLog;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInException
{
    public sealed class ExceptionManager :/* AppPluginModel, IAppPluginInteration,*/ IExceptionHandle
    {
        CommonLogger Logger { get; set; }
        static object _mutex = new object();
        static ExceptionManager _Instance;
        //public Dictionary<ExpectedException, List<string>> ExceptionBoard = new Dictionary<ExpectedException, List<string>>();
        public Dictionary<int, List<string>> ExceptionBoard = new Dictionary<int, List<string>>();
        //public List<ExceptionMessage> ExceptionMessages = new List<ExceptionMessage>();
        Dictionary<EnumMessageType, Action<IMessage>> LogActionDict { get; set; }
        public Action<IMessage> SendExceptionMessageToGuiAction { get; set; }

        long _exceptionCount = 0;
        //Form_ExceptionBoard _gui;
        //ICoreInteration _coreInteration;
        public ExceptionManager()
        {
        }
        public void Initialize(string rootFolder)
        {
            Logger = new CommonLogger();
            Logger.Initialize(rootFolder, @"\ExceptionLog\ExceptionLog.txt", true);
            InitialLogActionDict();
        }
        public static ExceptionManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_mutex)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new ExceptionManager();
                        }
                    }
                }
                return _Instance;
            }
        }
        void InitialLogActionDict()
        {
            LogActionDict = new Dictionary<EnumMessageType, Action<IMessage>>();

            Action<IMessage> allAction = null;
            allAction += IncreaseCounterAndReport;
            allAction += SendMessageToGui;
            allAction += SendMessageToPlatformLogFile;

            //allAction += SortException;
            LogActionDict[EnumMessageType.Exception] = allAction;

        }
        void Log(IMessage message)
        {
            try
            {
                if (message == null)
                {
                    throw new Exception("Invalid Null message !");
                }

                LogActionDict[message.Type]?.Invoke(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Log Manager can not execute message of type [{0}]." + message.GetType());
            }
        }

        void SendMessageToPlatformLogFile(string exlog)
        {
            Logger.Log(exlog);
        }

        void SendMessageToGui(IMessage sourceMessage)
        {
            if (SendExceptionMessageToGuiAction != null)
            {
                SendExceptionMessageToGuiAction.Invoke(sourceMessage);
            }
        }
        void SendMessageToPlatformLogFile(IMessage sourceMessage)
        {
            Logger.Log(sourceMessage.ToString());
        }
     

        public void SortException(IMessage sourceMsg)
        {
            var exMsg = sourceMsg as ExceptionMessage;
            //if (this.ExceptionBoard.ContainsKey(exMsg.ExceptionType))
            if (this.ExceptionBoard.ContainsKey(exMsg.ErrorCode))
            {
            }
            else
            {
                this.ExceptionBoard.Add(exMsg.ErrorCode, new List<string>());
                //this.ExceptionBoard.Add(exMsg.ExceptionType, new List<string>());
            }
            //this.ExceptionBoard[exMsg.ExceptionType].Add(exMsg.Message);
            this.ExceptionBoard[exMsg.ErrorCode].Add(exMsg.Message);
        }
        void IncreaseCounterAndReport(IMessage sourceMsg)
        {
            this._exceptionCount++;
            if (this._exceptionCount % 2 == 1)
            { 
                this.SendMessageToGui(new InternalMessage("", InternalOperationType.ExceptionCountChanged, this._exceptionCount));
            }
        }
        public void ReportException(ExceptionMessage exMsg)
        {
            
            Log(exMsg);
        }
        //public void ReportException(string message, ExpectedException expectedException, Exception e, object context)
        //{
        //    ExceptionMessage exMsg = new ExceptionMessage($"{message}-{GetExceptionDetails(e)}", expectedException, e, context);
 
        //    this.ReportException(exMsg);
        //}
        //public void ReportException(string message, ExpectedException expectedException, Exception e)
        //{
        //    ExceptionMessage exMsg = new ExceptionMessage($"{message}-{GetExceptionDetails(e)}", expectedException, e);
        //    this.ReportException(exMsg);
        //}
        //public void ReportException(string message, ExpectedException expectedException)
        //{
        //    ExceptionMessage exMsg = new ExceptionMessage(message, expectedException);
        //    this.ReportException(exMsg);
        //}
      
        //public void ClearException(ExpectedException eeType)
        //{

        //}
        private string GetExceptionDetails(Exception ex)
        {
            Exception currEx = ex;
            string log = string.Empty;
            while (true)
            {
                log += string.Format("{0}-{1}.", currEx.Message, currEx.StackTrace);
                if (currEx.InnerException != null)
                {
                    currEx = currEx.InnerException;
                }
                else
                {
                    break;
                }
            }
            return log;
        }

        public void ReportException(string message, int errorCode)
        {
            ExceptionMessage exMsg = new ExceptionMessage($"{message}", errorCode );
            Log(exMsg);
        }

        public void ReportException(string message, int errorCode, Exception e)
        {
            ExceptionMessage exMsg = new ExceptionMessage($"{message}-{GetExceptionDetails(e)}", errorCode, e );
            Log(exMsg);
        }

        public void ReportException(string message, int errorCode, Exception e, object context)
        {
            ExceptionMessage exMsg = new ExceptionMessage($"{message}-{GetExceptionDetails(e)}", errorCode, e , context);
            Log(exMsg); 
        }
    }
}