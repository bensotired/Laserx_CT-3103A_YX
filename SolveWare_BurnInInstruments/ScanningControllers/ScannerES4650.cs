using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public sealed class ScannerES4650 : InstrumentBase, IInstrumentBase, IScanner
    {
        const int Delay_ms = 500;
        const string startCommand = "16 54 0D";
        const string stopCommand = "16 55 0D";
        public ScannerES4650(string name, string address, IInstrumentChassis chassis)
        : base(name, address, chassis)
        {
        }
        
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            // throw new NotImplementedException();
        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //  throw new NotImplementedException();
        }

        public string Scanning()
        {
            if (this._isOnline == false)
            {
                return string.Empty;
            }
            string[] stringstart = startCommand.Split(' ');
            string[] stringstop = stopCommand.Split(' ');
            byte[] byteListstart = new byte[stringstart.Length];
            byte[] byteListstop = new byte[stringstop.Length];
            for (int i = 0; i < byteListstart.Length; i++)
            {
                byteListstart[i] = Convert.ToByte(stringstart[i],16);
            }
            for (int j = 0; j < byteListstop.Length; j++)
            {
                byteListstop[j] = Convert.ToByte(stringstop[j], 16);
            }
            var responseList = this._chassis.Query(byteListstart, Delay_ms);
            string ret = System.Text.Encoding.Default.GetString(responseList).Replace('\r',' ').Replace('\n', ' ').Trim();
            //停止解码
            this._chassis.TryWrite(byteListstop);
            return ret;
        }
    }
}
