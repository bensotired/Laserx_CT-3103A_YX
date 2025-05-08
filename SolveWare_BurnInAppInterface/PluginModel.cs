

using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInAppInterface
{
    public abstract class PluginModel
    {
        protected Task modelTask { get; set; }
        public Action<IMessage> OnSendOutMessageAction { get; set; }
        public PluginModel()
        {
            this.myMessageQueue = new BlockingCollection<IMessage>();
        }

        protected BlockingCollection<IMessage> myMessageQueue { get; set; }
        protected CancellationTokenSource _myTokenSource;
        public virtual void StartUp()
        {
            this._myTokenSource = new CancellationTokenSource();
            this.modelTask = Task.Factory.StartNew
                            (
                                () =>
                                {
                                    this.Run(ref this._myTokenSource);
                                },
                                TaskCreationOptions.LongRunning
                            );
        }
        public virtual void Close()
        {
            if (this.modelTask != null)
            {
                if (this._myTokenSource.IsCancellationRequested == false)
                {
                    this._myTokenSource.Cancel();
                }
            }
        }



        protected virtual void Cancel()
        {
            if (_myTokenSource != null && _myTokenSource.IsCancellationRequested == false)
            {
                _myTokenSource.Cancel();
            }
        }

        protected virtual void Initialize(CancellationToken token)
        {
        }

        protected virtual void Run(ref CancellationTokenSource tokenSource)
        {
            try
            {
                try
                {
                    try
                    {
                        //初始化操作 在开始消息监听之前
                        this.Initialize(tokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        var owner = this.GetType();
                        //this.HandleException(ex);
                        throw ex;
                    }
                    // 监听消息 FIFO
                    while (!tokenSource.IsCancellationRequested)
                    {
                        Thread.Sleep(5);
                        IMessage message = null;
                        try
                        {
                            message = this.myMessageQueue.Take(tokenSource.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {

                        }
                        //处理消息
                        this.HandleMessage(message);
                    }
                }
                catch (OperationCanceledException oce)
                {
                    //this.HandleException(oce);
                    this.Log_Global(oce.ToString());
                }
                finally
                {
                    //this.Clean();
                }
            }
            catch (Exception ex)
            {
                //this.HandleException(ex);
                //throw ex;
            }
        }
        public virtual void FormattedLog_File(string format, params object[] args) { }
        public virtual void FormattedLog_Global(string format, params object[] args) { }
        public virtual void Log_File(string message) { }
        public virtual void Log_Global(string message) { }
        //public virtual void ReportException(ExceptionMessage exMsg) { }
        //public virtual void ReportException(string message, ExpectedException expectedException) { }
        //public virtual void ReportException(string message, ExpectedException expectedException, Exception e) { }
        //public virtual void ReportException(string message, ExpectedException expectedException, Exception e, object context) { }
        public virtual void ReportException(string message, int errorCode) { }
        public virtual void ReportException(string message, int errorCode, Exception e) { }
        public virtual void ReportException(string message, int errorCode, Exception e, object context) { }



        //protected virtual void ClearException(ExpectedException eeType) { }
        protected virtual void HandleMessage(IMessage message)
        {

        }
        protected virtual void ReceiveMessage(IMessage message)
        {

            this.myMessageQueue.Add(message);
        }
        protected virtual void SendMessage(IMessage message)
        {
            //if(this.OnSendOutMessageAction != null)
            //{
            //    this.OnSendOutMessageAction(message);
            //}
            //this.myMessageQueue.Add(message);
        }
    }
}