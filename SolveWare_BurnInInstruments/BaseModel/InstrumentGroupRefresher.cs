using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class InstrumentGroupRefresher : InstrumentBase, IInstrumentBase//, IRefreshableInstrument
    {
        List<IInstrumentBase> myGroup { get; set; }
        public InstrumentGroupRefresher(string name, List<IInstrumentBase> instGrp):base(name)
        {
            this.myGroup = instGrp;
        }
        public override void Initialize()
        {
            if (this._tokenSource.IsCancellationRequested)
            {
                this._tokenSource = new CancellationTokenSource();
            }
         
            if (/*this._isSimulation ||*/ this._isOnline)
            {  
                //把所有仪器内部刷新全部暂停
                this.SuspendAllMemberSelfRefreshing();
            }
            this._nonstopTask = Task.Factory.StartNew(() =>
            {
                do
                {
                    if (this._tokenSource.IsCancellationRequested)
                    {
                        //SuspendRefreshing()操作后在此处空转
                        Thread.Sleep(100);
                        continue;
                    }
                    if (this._isOnline)
                    {
                        //if (this._isSimulation)
                        //{
                        //    this.GenerateFakeDataLoop(this._tokenSource.Token);
                        //}
                        //else
                        {
                            this.RefreshDataLoop(this._tokenSource.Token);
                        }
                    }
                    Thread.Sleep(100);
                } while (true);

            }, TaskCreationOptions.LongRunning);
        }

        public void SuspendAllMemberSelfRefreshing()
        {
            foreach (var inst in this.myGroup)
            {
                inst.HandleGroupOperations(GroupOperation.F1);
            }
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            foreach (var inst in this.myGroup)
            {
                inst.GenerateFakeDataOnceCycle(token);
                token.ThrowIfCancellationRequested();
            }
        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            foreach (var inst in this.myGroup)
            {
                inst.RefreshDataOnceCycle(token);
                token.ThrowIfCancellationRequested();
            }
        }
     
        public override void GenerateFakeDataLoop(CancellationToken token)
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
      
        public override void RefreshDataLoop(CancellationToken token)
        {
            do
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    this.RefreshDataOnceCycle(token);
                    Thread.Sleep(2000);
                }
                catch (OperationCanceledException oce)
                {
                    //string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- RefreshDataLoop is cancelled.";
                    //Console.WriteLine(msg);
                    return;
                }
                catch (Exception ex)
                {
                    //string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- RefreshDataLoop exception:{ex.Message}-{ex.StackTrace}.";
                    //Console.WriteLine(msg);
                }
            }
            while (true);
        }

 

       
    }
}
