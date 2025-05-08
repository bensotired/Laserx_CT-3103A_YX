using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Threading;

namespace SolveWare_BurnInAppInterface
{ 
    public abstract class TesterAppPluginModel : PluginModel , ITesterAppLogHandle, ITesterAppExceptionHandle,   ITesterCoreLink, IAccessPermissionLevel
    {
        protected ITesterCoreResource _coreResource;
        protected ITesterCoreInteration _coreInteration;
        string _myName = string.Empty;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this._myName) == true)
                {
                    this._myName = this.GetType().Name;
                }
                return this._myName;
            }
            protected set
            {
                this._myName = value;
            }
        }
        public virtual AccessPermissionLevel APL
        {
            get
            {
                return AccessPermissionLevel.None;
            }
        }

        protected ITesterCoreResource CoreResource
        {
            get
            {
                return this._coreInteration as ITesterCoreResource;
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
            _coreInteration?.FormattedLog_File($"[{this.Name}]{format}", args );
        }
        public override void FormattedLog_Global(string format, params object[] args)
        {
            _coreInteration?.FormattedLog_Global($"[{this.Name}]{format}", args);
        }
        public override void Log_File(string message)
        {
            _coreInteration?.Log_File($"[{this.Name}]{message}");
        }
        public override void Log_Global(string message)
        {
            _coreInteration?.Log_Global($"[{this.Name}]{message}");
            //_coreInteration?.Log_Global(message);
        }
        public override void ReportException(string message, int errorCode)
        {
            _coreInteration?.ReportException($"[{this.Name}]{message}", errorCode);
        }
        public override void ReportException(string message, int errorCode, Exception e)
        {
            _coreInteration?.ReportException($"[{this.Name}]{message}", errorCode,e);
        }
        public override void ReportException(string message, int errorCode, Exception e, object context)
        {
            _coreInteration?.ReportException($"[{this.Name}]{message}", errorCode, e, context);
        }


        //public override void ReportException(ExceptionMessage exMsg)
        //{
        //    _coreInteration?.ReportException(exMsg);
        //}
        //public override void ReportException(string message, ExpectedException expectedException)
        //{
        //    _coreInteration?.ReportException($"[{this.GetType().Name}]{message}", expectedException);
        //}
        //public override void ReportException(string message, ExpectedException expectedException, Exception e)
        //{
        //    _coreInteration?.ReportException($"[{this.GetType().Name}]{message}", expectedException, e);
        //}

        //public override void ReportException(string message,
        //    ExpectedException expectedException,
        //    Exception e,
        //    object context)
        //{
        //    _coreInteration?.ReportException($"[{this.GetType().Name}]{message}", expectedException, e, context);
        //}
        //protected override void ClearException(ExpectedException eeType)
        //{
        //    _coreInteration?.ClearException(eeType);
        //}
    }
}