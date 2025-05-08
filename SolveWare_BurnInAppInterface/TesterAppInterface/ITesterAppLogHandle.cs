using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface ITesterAppLogHandle
    {
   
        void FormattedLog_File(string format, params object[] args);
        void FormattedLog_Global(string format, params object[] args);
        void Log_File(string message);
        void Log_Global(string message);
 
    }
}