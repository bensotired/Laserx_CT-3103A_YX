using MeSoft.MeCom.PhyWrapper;
using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class MeCom_TCPChassis : InstrumentChassisBase, IInstrumentChassis
    {
        private MeComPhyTcp _meComPhyTcp;
        protected ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        public int Port { get; set; }
        public string IPAddress { get; set; }
        public bool Reconnecting { get; set; }
        //public int Timeout_ms { get; set; }
        public object SyncObj = new object();
        public MeCom_TCPChassis(string name, string resource, bool isOnline )
            : base(name, resource, isOnline )
        {
            try
            {
                var arr = resource.Split(':');
                this.IPAddress = arr[0];
                this.Port = Convert.ToInt16(arr[1]);
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Constructor error, Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg, ex);
            }
        }
        public override void WaitRuning()
        {

        }

        public override object Visa
        {
            get { return this._meComPhyTcp; }
        }

        public override void Initialize()
        {
            Initialize(5000);
        }
        public override void Initialize(int timeout)
        {
            if (IsOnline == false)
            {
                return;
            }
            try
            {
                this._meComPhyTcp = new MeComPhyTcp();
                this._meComPhyTcp.OpenClient(this.IPAddress, this.Port, timeout);
                this._canAccess = true;
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]",
                                       this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);
            }
        }
        public override void ClearConnection()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                this._meComPhyTcp?.CloseClient();
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Clear connection fails. Chassis resource = [{1}].[{2}]-[{3}]",
                                                  this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
            }
        }
        public override void BuildConnection(int timeout_ms)
        {
            if (IsOnline == false  )
            {
                return;
            }
            try
            {
                this._meComPhyTcp = new MeComPhyTcp();
                this._meComPhyTcp.OpenClient(this.IPAddress, this.Port, timeout_ms);
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] BuildConnection error. Chassis resource = [{1}].[{2}]-[{3}]",
                                       this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);

            }
        }
        public override void ConnectToResource(int timeout_ms, bool forceOnline = false )
        {
         
            try
            {
                if (forceOnline)
                {
                    this._manualResetEvent.Set();
                }

                if (this._manualResetEvent.WaitOne(50))
                {
                    this._manualResetEvent.Reset();
                    this.ClearConnection();
                    this.BuildConnection(timeout_ms);
                    Thread.Sleep(5000);
                    this.OnAllocateChassisResouce();
                }
                else
                {

                    Console.WriteLine($"{this.Name} quit ConnectToResource");
                }
            }
            catch (Exception ex)
            {
              
            }
            finally
            {
                this._manualResetEvent.Set();
            }
        }
        protected override void OnTurnOnline()
        { 
            this.ConnectToResource(5000, true);
            Thread.Sleep(5000);
            base.OnTurnOnline();
        }
    }
}