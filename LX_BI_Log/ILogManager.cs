using System;
using SolveWare_BurnInMessage;

namespace SolveWare_BurnInLog
{
    public interface ILogManager
    {
        Action<IMessage> SendMessageToGuiAction { get; set; }

        void FormattedLog_File(string format, params object[] args);
        void FormattedLog_Global(string format, params object[] args);
        void Initialize(string rootFolder);
        void Log_File(string message);
        void Log_Global(string message);
    }
}