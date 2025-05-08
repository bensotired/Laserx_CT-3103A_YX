using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class PXISourceMeter_6683H : InstrumentBase
    {
       public NationalInstruments.VisaNS.PxiSession PxiSession;

        public PXISourceMeter_6683H(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {

        }
        IntPtr session66xx = IntPtr.Zero;
        public override void Initialize()
        {
            if (this.IsOnline)
            {
                var resourceName = this._chassis.Resource;
                string errorMsg = "";
                 
                int status = NiSync.init(resourceName, true, true, out session66xx);
                errorMsg = NiSync.ERROR_CHECK(status);
                if(status != 0)
                {
                    throw new Exception($"PXISourceMeter_6683H initialize error:{errorMsg}!");
                }
            }
        }
        public void ConnectTrigTerminals(string srcTerminal, string destTerminal, string syncClock, int invert, int updateEdge)
        {
            if (this.IsOnline)
            {
                string errorMsg = string.Empty;
                var resourceName = this._chassis.Resource;
                int status = NiSync.ConnectTrigTerminals(this.session66xx, srcTerminal, destTerminal, syncClock, invert, updateEdge);
                errorMsg = NiSync.ERROR_CHECK(status);
                if (status != 0)
                {
                    throw new Exception($"PXISourceMeter_6683H initialize error:{errorMsg}!");
                }
            }
        }
        public void DisconnectTrigTerminals(string srcTerminal, string destTerminal )
        {
            if (this.IsOnline)
            {
                string errorMsg = string.Empty;
                var resourceName = this._chassis.Resource;
                int status = NiSync.DisconnectTrigTerminals(this.session66xx, srcTerminal, destTerminal );
                errorMsg = NiSync.ERROR_CHECK(status);
                if (status != 0)
                {
                    throw new Exception($"PXISourceMeter_6683H initialize error:{errorMsg}!");
                }
            }
        }
        public void Close( )
        {
            if (this.IsOnline)
            {
                string errorMsg = string.Empty;
                var resourceName = this._chassis.Resource;
                int status = NiSync.close(this.session66xx );
                errorMsg = NiSync.ERROR_CHECK(status);
                if (status != 0)
                {
                    throw new Exception($"PXISourceMeter_6683H initialize error:{errorMsg}!");
                }
            }
        }

        public void TrigTerminalsStart(string TermialName)
        {
            Initialize();
            ConnectTrigTerminals(TermialName, "/PXI1Slot6/PFI0", "SyncClkAsync", 1, 0);
        }
        public void TrigTerminalsStop(string TermialName)
        {
            DisconnectTrigTerminals(TermialName, "/PXI1Slot6/PFI0");
            Close();
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }
    }

}
