

using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{
    public abstract class TesterPluginModel : PluginModel , ITesterCoreLink, IAccessPermissionLevel
    {
        protected ITesterCoreInteration _coreInteration;

        public virtual AccessPermissionLevel APL
        {
            get
            {
                return AccessPermissionLevel.None;
            }
        }
        public virtual void ConnectToCore(ITesterCoreInteration core)
        {
            _coreInteration = core;
            _coreInteration.SendOutFormCoreEvent -= this.ReceiveMessage;
            _coreInteration.SendOutFormCoreEvent += this.ReceiveMessage;
        }
        public virtual void DisconnectFromCore(ITesterCoreInteration core)
        {
            _coreInteration.SendOutFormCoreEvent -= this.ReceiveMessage;
        }
        protected override void SendMessage(IMessage message)
        {
            try
            {
                _coreInteration?.SendToCore(message);
            }
            catch
            {

            }
        }
        public override void FormattedLog_File(string format, params object[] args)
        {
            _coreInteration?.FormattedLog_File(format, args);
        }
        public override void FormattedLog_Global(string format, params object[] args)
        {
            _coreInteration?.FormattedLog_Global(format, args);
        }
        public override void Log_File(string message)
        {
            _coreInteration?.Log_File(message);
        }
        public override void Log_Global(string message)
        {
            _coreInteration?.Log_Global(message);
        }
        //public override void ReportException(ExceptionMessage exMsg)
        //{
        //    _coreInteration?.ReportException(exMsg);
        //}
        //public override void ReportException(string message, ExpectedException expectedException)
        //{
        //    _coreInteration?.ReportException(message, expectedException);
        //}
        //public override void ReportException(string message, ExpectedException expectedException, Exception e)
        //{
        //    _coreInteration?.ReportException(message, expectedException, e);
        //}
        //public override void ReportException(string message,
        //    ExpectedException expectedException,
        //    Exception e,
        //    object context)
        //{
        //    _coreInteration?.ReportException(message, expectedException, e, context);
        //}
        //protected override void ClearException(ExpectedException eeType)
        //{
        //    _coreInteration?.ClearException(eeType);
        //}
    }
}