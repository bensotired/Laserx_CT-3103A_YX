using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    class PhotoelectricSwitch : InstrumentBase, IInstrumentBase
    {
        const int Delay_ms = 50;
        const string eorr = "Instrument not connected";
        public PhotoelectricSwitch(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
        }

        public void ChannelOnOFF(channel channel)
        {
            if (this._isOnline == false)
            {
                return;
            }
            string response = this._chassis.Query($"*SW{channel}\r\n", Delay_ms);
        }

        public string QueryPN()
        {
            if (this._isOnline == false)
            {
                return eorr;
            }
            string response = this._chassis.Query($"*PN\r\n", Delay_ms);

            return response;
        }
        public string QuerySN()
        {
            if (this._isOnline == false)
            {
                return eorr;
            }
            string response = this._chassis.Query($"*SN\r\n", Delay_ms);

            return response;
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
    public enum channel
    {
        off = 000,
        channel_1 = 001,
        channel_2 = 002,
        channel_3 = 003,
        channel_4 = 004,
    }
}
