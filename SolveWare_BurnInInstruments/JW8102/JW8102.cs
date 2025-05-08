using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    //115200bps
    public class JW8102 : InstrumentBase, IInstrumentBase
    {
        const int Delay_ms = 50;
        const string eorr = "Instrument not connected";
        const int Lenth = 7;

        public JW8102(string name, string address, IInstrumentChassis chassis)
       : base(name, address, chassis)
        {
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public void Connection()
        {
            if (this._isOnline == false)
            {
                return;
            }
            byte[] cmdtop = { 0x7b, 0xff };//, 0x05, 0x01, 0x40, 0x3e, 0x7d };

            byte[] cmdmain = { 0x01, 0x40 };

            byte[] cmddata = { };

            var cmd = Checkbit(cmdtop, cmdmain, cmddata);
            var responseList = this._chassis.Query(cmd, Delay_ms);


        }


        public double[] GetFulPower()
        {
            if (this._isOnline == false)
            {
                return new double[] { 0, 0, 0, 0 };
            }
            //byte[] cmd = { 0x7b, 0xff , 0x05, 0x01, 0x42, 0x3e, 0x7d };
            byte[] cmdtop = { 0x7b, 0xff };

            byte[] cmdmain = { 0x01, 0x42 };

            byte[] cmddata = { };

            var cmd = Checkbit(cmdtop, cmdmain, cmddata);

            var responseList = this._chassis.Query(cmd, Delay_ms);

            double[] allPower = new double[4];

            allPower[0] = BitConverter.ToDouble(new byte[] { responseList[6], responseList[5] }, 0);
            allPower[1] = BitConverter.ToDouble(new byte[] { responseList[8], responseList[7] }, 0);
            allPower[2] = BitConverter.ToDouble(new byte[] { responseList[10], responseList[9] }, 0);
            allPower[3] = BitConverter.ToDouble(new byte[] { responseList[12], responseList[11] }, 0);
            return allPower;
        }



        public byte[] Checkbit(byte[] top, byte[] main, byte[] data)
        {
            int len = Lenth + data.Length - 2;
            byte[] Len = { Convert.ToByte(len.ToString("X"), 16) };

            int index = 0;
            if (data.Length <= 0)
            {
                index = top.Length + Len.Length + main.Length;
            }
            else
            {
                index = top.Length + Len.Length + main.Length + data.Length;
            }

            byte[] ComputationalCheck = new byte[index];
            Array.Copy(top, 0, ComputationalCheck, 0, top.Length);
            Array.Copy(Len, 0, ComputationalCheck, top.Length, Len.Length);
            Array.Copy(main, 0, ComputationalCheck, top.Length + Len.Length, main.Length);
            if (data.Length > 0)
            {
                Array.Copy(data, 0, ComputationalCheck, top.Length + Len.Length + main.Length, data.Length);
            }

            byte sum = 0;
            foreach (var item in ComputationalCheck)
            {
                sum += item;
            }
            var Check = (byte)(~sum + 0x01);
            byte[] cmdend = { Check, 0x7d };
            byte[] Cmd = new byte[ComputationalCheck.Length + cmdend.Length];
            Array.Copy(ComputationalCheck, 0, Cmd, 0, ComputationalCheck.Length);
            Array.Copy(cmdend, 0, Cmd, ComputationalCheck.Length, cmdend.Length);
            return Cmd;
        }


        public void SwitchWavelength(channel channel, int waveindex)
        {
            if (this._isOnline == false)
            {
                return;
            }
            byte[] data = new byte[2];
            switch (channel)
            {
                case channel.all:
                    {
                        data[0] = 0xff; data[1] = (byte)waveindex;
                    }
                    break;
                case channel._1:
                    {
                        data[0] = 0x01; data[1] = (byte)waveindex;
                    }
                    break;
                case channel._2:
                    {
                        data[0] = 0x02; data[1] = (byte)waveindex;
                    }
                    break;
                case channel._3:
                    {
                        data[0] = 0x03; data[1] = (byte)waveindex;
                    }
                    break;
                case channel._4:
                    {
                        data[0] = 0x04; data[1] = (byte)waveindex;
                    }
                    break;
            }
            byte[] cmdtop = { 0x7b, 0xff };

            byte[] cmdmain = { 0x01, 0x44 };

            var cmd = Checkbit(cmdtop, cmdmain, data);

            var responseList = this._chassis.Query(cmd, Delay_ms);
        }

        public void SetREF(channel_ref channel, double refvalue)
        {
            if (this._isOnline == false)
            {
                return;
            }
            byte[] chan = new byte[1];
            switch (channel)
            {
                case channel_ref._1:
                    {
                        chan[0] = 0x01;
                    }
                    break;
                case channel_ref._2:
                    {
                        chan[0] = 0x02;
                    }
                    break;
                case channel_ref._3:
                    {
                        chan[0] = 0x03;
                    }
                    break;
                case channel_ref._4:
                    {
                        chan[0] = 0x04;
                    }
                    break;
            }
            var REF = BitConverter.GetBytes(refvalue);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(REF);
            }
            byte[] cmddata = new byte[chan.Length + REF.Length];
            Array.Copy(chan, 0, cmddata, 0, chan.Length);
            Array.Copy(REF, 0, cmddata, chan.Length, REF.Length);

            byte[] cmdtop = { 0x7b, 0xff };

            byte[] cmdmain = { 0x01, 0x48 };

            var cmd = Checkbit(cmdtop, cmdmain, cmddata);

            var responseList = this._chassis.Query(cmd, Delay_ms);
        }

        public void GetTheScreen()//未解析
        {
            if (this._isOnline == false)
            {
                return;
            }

            byte[] cmdtop = { 0x7b, 0xff };//7b ff 05 01 4a 36 7d

            byte[] cmdmain = { 0x01, 0x4a };

            byte[] cmddata = { };

            var cmd = Checkbit(cmdtop, cmdmain, cmddata);
            var responseList = this._chassis.Query(cmd, Delay_ms);

        }

        public void ModificationID(int id)//不要改ID
        {
            if (this._isOnline == false)
            {
                return;
            }
            byte[] cmdtop = { 0x7b, 0xff };//0x 7b ID LEN 0x0 1 0x de 新ID check 0x 7d

            byte[] cmdmain = { 0x01, 0xde };

            byte[] cmddata = { (byte)id };

            var cmd = Checkbit(cmdtop, cmdmain, cmddata);

            var responseList = this._chassis.Query(cmd, Delay_ms);
        }

        public enum channel
        {
            all,
            _1,
            _2,
            _3,
            _4,
        }
        public enum channel_ref
        {
            _1,
            _2,
            _3,
            _4,
        }



    }
}
