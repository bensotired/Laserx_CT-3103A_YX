using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface ITesterAppExceptionHandle
    {
 
        //void ReportException(ExceptionMessage exMsg);
        //void ReportException(string message, ExpectedException expectedException);
        //void ReportException(string message, ExpectedException expectedException, Exception e);
        //void ReportException(string message, ExpectedException expectedException, Exception e, object context);

        void ReportException(string message, int errorCode );
        void ReportException(string message, int errorCode, Exception e);
        void ReportException(string message, int errorCode, Exception e, object context);
        //void ClearException(ExpectedException eeType);
    }
}