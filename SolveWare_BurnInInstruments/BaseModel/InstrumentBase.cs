using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public abstract class InstrumentBase : IInstrumentBase  
    {
        public string Name { get; protected set; }
        public string Address { get; protected set; }
        protected Task _nonstopTask;
        protected volatile CancellationTokenSource _tokenSource = new CancellationTokenSource();
        protected ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        protected bool _isOnline = false;
        //protected bool _isSimulation = false;
        //protected bool _orgIsSimulation = false;
        protected IInstrumentChassis _chassis;
        public InstrumentBase( )
        { 
        }
        public InstrumentBase(string name)
        {
            this.Name = name;
        }
        public InstrumentBase(string name, string address, IInstrumentChassis chassis)
        {
            this.Name = name;
            this.Address = address;
            this._chassis = chassis;
            this._chassis.InstrumentChassisEvent += InstrumentChassisEventHandler;
        }
        public virtual int Timeout_ms { get; set; }
        public virtual object GetHardwareVersion()
        {
            return 0;
        }
        public virtual void CheckHardwareLatest()
        {
    
        }
        protected virtual object InstrumentChassisEventHandler(object sender, InstrumentChassisArgs e)
        {
            try
            {
                switch (e.EventType)
                {
                    case InstrumentChassisArgsType.TurnOffline:
                        {
                            this._isOnline = false;
                            //this._isSimulation = this._orgIsSimulation;
                            return this.HandleChassisOffline();
                        }
                        break;
                    case InstrumentChassisArgsType.TurnOnline:
                        {
                            this._isOnline = true;
                            //this._orgIsSimulation = this._isSimulation;
                            //this._isSimulation = false;
                            return this.HandleChassisOnline();
                        }
                        break;
                    //case InstrumentChassisArgsType.TurnOnSimnulation:
                    //    {
                    //        this._isOnline = true;
                    //        this._isSimulation = true;
                    //        return this.HandleChassisOnSimulation() ;
                    //    }
                    //    break;
                    case InstrumentChassisArgsType.AllocateChassisResouce:
                        {
                            return this.HandleAllocateChassisResouce();
                        }
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public virtual  bool IsOnline
        {
            get
            {
                return this._isOnline;
            }
        }
        //public virtual bool IsSimulation
        //{
        //    get
        //    {
        //        return this._isSimulation;
        //    }
        //}
        public virtual void TurnOnline(bool isOnline)
        {
            this._isOnline = isOnline;
        }
        //public virtual void EnableSimulation(bool isEnable)
        //{
        //    this._isSimulation = isEnable;
        //    this._orgIsSimulation = isEnable;
        //}
        public virtual void Initialize()
        {
            if (this._tokenSource.IsCancellationRequested)
            {
                this._tokenSource = new CancellationTokenSource();
            }
            this._nonstopTask = Task.Factory.StartNew(() =>
            {
                do
                {
                    if (this._tokenSource.IsCancellationRequested)
                    {
                        //SuspendRefreshing()操作后在此处空转
                        //在此操作chassis的offline  
                        Thread.Sleep(100);
                        continue;
                    }
                    if (this._isOnline)
                    {
                        if (this._chassis != null)
                        {
                            if (this._chassis.IsOnline == true)
                            {
                                this.RefreshDataLoop(this._tokenSource.Token);
                            }
                        }
                    }
                    else
                    {
                        this.GenerateFakeDataLoop(this._tokenSource.Token);
                    }
                    Thread.Sleep(100);
                } while (true);

            }, TaskCreationOptions.LongRunning);
        }

        protected virtual void ResumeRefreshing()
        {
            if (this._tokenSource.IsCancellationRequested)
            {
                this._tokenSource = new CancellationTokenSource();
            }

        }
        public virtual void WaitRuning()
        {
            // do nothing
        }
        protected virtual void SuspendRefreshing()
        {
            if (this._tokenSource.IsCancellationRequested == false)
            {
                this._tokenSource.Cancel();
            }
        }

        public virtual void HandleGroupOperations()
        {

        }

        public abstract void RefreshDataOnceCycle(CancellationToken token);
        public virtual void RefreshDataLoop(CancellationToken token)
        {
            do
            {
                try
                {

                    this.RefreshDataOnceCycle(token);
                    Thread.Sleep(2000);
                    token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException oce)
                {
                    string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataLoop is cancelled.";
                    return;
                }
                catch (Exception ex)
                {
                    string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataLoop exception:{ex.Message}-{ex.StackTrace}.";
                }
            }
            while (true);
        }
        public abstract void GenerateFakeDataOnceCycle(CancellationToken token);
        public virtual void GenerateFakeDataLoop(CancellationToken token)
        {
            do
            {
                try
                {
                    this.GenerateFakeDataOnceCycle(token);
                    Thread.Sleep(2000);
                    token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException oce)
                {
                    string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataLoop is cancelled.";
                    return;
                }
                catch (Exception ex)
                {
                    string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataLoop exception:{ex.Message}-{ex.StackTrace}.";
                }
            }
            while (true);
        }
        public virtual object HandleChassisOnSimulation()
        {
 
            this.ResumeRefreshing();
            return true;
        }
        public virtual object HandleChassisOnline()
        {
            this.ResumeRefreshing();
            return true;
        }
        public virtual object HandleChassisOffline()
        {
            this.SuspendRefreshing();
            return true;
        }
        public virtual object HandleAllocateChassisResouce()
        {
            return true;
        }
        public virtual void ReportError(string errorMsg)
        {
            this._chassis?.ReportException(errorMsg ,ErrorCodes.InstrumentException);
        }

        public virtual void HandleGroupOperations(GroupOperation operaType)
        {
            switch (operaType)
            {
                case GroupOperation.F1:
                    {
                        this.SuspendRefreshing();
                    }
                    break;
                case GroupOperation.F2:
                    {
                        this.ResumeRefreshing();
                    }
                    break;
                case GroupOperation.F3:
                    break;
                default:
                    break;
            }
   
        }
    }
}