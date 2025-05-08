using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInLog
{

    public sealed class LogManager : ILogManager
    {
        CommonLogger Logger { get; set; }
        static object _mutex = new object();
        static LogManager _Instance;
        Dictionary<EnumMessageType, Action<IMessage>> LogActionDict { get; set; }
        public static LogManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_mutex)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new LogManager();
                        }
                    }
                }
                return _Instance;
            }
        }
        public Action<IMessage> SendMessageToGuiAction   { get; set; }

        public void Initialize(string rootFolder)
        {
            Logger = new CommonLogger();
            Logger.Initialize(rootFolder, @"\Log\TestStationLog.txt", true);
            InitialLogActionDict();
        }
        void InitialLogActionDict()
        {
            LogActionDict = new Dictionary<EnumMessageType, Action<IMessage>>();

            Action<IMessage> allAction = null;
            allAction += SendMessageToGui;
            allAction += SendMessageToPlatformLogFile;
            allAction += SendMessageToConsole;
            LogActionDict[EnumMessageType.Global] = allAction;

            Action<IMessage> logFileAction = null;
            logFileAction += SendMessageToPlatformLogFile;
            allAction += SendMessageToConsole;
            LogActionDict[EnumMessageType.LogOnly] = logFileAction;
        }

        /// <summary>
        /// Write format massage to log file
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void FormattedLog_File(string format, params object[] args)
        {
            Log(new LogMessage(string.Format(format, args)));
        }
        /// <summary>
        /// Write message to log file
        /// </summary>
        /// <param name="message"></param>
        public void Log_File(string message)
        {
            Log(new LogMessage(message));
        }
        /// <summary>
        /// Write format global message 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void FormattedLog_Global(string format, params object[] args)
        {
            Log(new GlobalMessage(string.Format(format, args)));
        }
        /// <summary>
        /// Write  global message
        /// </summary>
        /// <param name="message"></param>
        public void Log_Global(string message)
        {
            Log(new GlobalMessage(message));
        }

        void Log(IMessage message)
        {
            try
            {
                if (message == null)
                {
                    throw new Exception("Invalid Null message !");
                }
                Action<IMessage> action = LogActionDict[message.Type];
                if (action != null)
                {
                    action(GetDatetimeMessage(message));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Log Manager can not execute message of type [{0}]." + message.GetType());
            }
        }
        void SendMessageToGui(IMessage sourceMessage)
        {
            if (SendMessageToGuiAction != null)
            {
                SendMessageToGuiAction.Invoke(sourceMessage);
            }
        }
        void SendMessageToPlatformLogFile(IMessage sourceMessage)
        {
            Logger.Log(sourceMessage.Message);
        }
        void SendMessageToConsole(IMessage sourceMessage)
        {
            Console.WriteLine(sourceMessage.Message);
        }
        IMessage GetDatetimeMessage(IMessage sourceMessage)
        {
            sourceMessage.Message = string.Format("[{0:yyyy/MM/dd HH:mm:ss}]{1}", DateTime.Now, sourceMessage.Message);
            return sourceMessage;
        }
    }
}