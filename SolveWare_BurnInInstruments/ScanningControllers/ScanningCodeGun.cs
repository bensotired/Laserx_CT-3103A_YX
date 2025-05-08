using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class ScanningCodeGun : InstrumentBase, IInstrumentBase
    {
        string ResourceName;
        SerialPort port;
        public ScanningCodeGun(string name, string address, IInstrumentChassis chassis)
        : base(name, address, chassis)
        {
        }
        public void Connect()
        {
            InitialGun();
        }

        public string Scanning()
        {
            if (this._isOnline == false)
            {
                return string.Empty;
            }
            string port_txt = "16 54 0D";
            byte[] bt = HexStringToBytes(port_txt);

            var responseList = this._chassis.Query(bt, 50);
            string ret = System.Text.Encoding.Default.GetString(responseList).Replace('\r', ' ').Replace('\n', ' ').Trim();

            string stop = "16 55 0D";
            byte[] st = HexStringToBytes(stop);
            this._chassis.TryWrite(st);

            return ret;
        }
        public string ScanCode()
        {
            string readLine = "\r";
            if (this.IsOnline)
            {
                //if (port == null)
                //{
                //    return "NR";
                //}
                if (port.IsOpen)
                {
                    port.Close();
                    Thread.Sleep(60);  //zkz20220426添加延时 防止太快
                }
                if (!port.IsOpen)
                {
                    port.Open();
                    Thread.Sleep(60);  //zkz20220426添加延时 防止太快
                }

                string port_txt = "16 54 0D";
                byte[] bt = HexStringToBytes(port_txt);
                port.Write(bt, 0, bt.Length);

                while (true)
                {
                    readLine = port.ReadLine();
                    if (readLine != "\r")
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(200);
                }
                if (port.IsOpen)
                {
                    port.Close();
                    Thread.Sleep(60);  //zkz20220426添加延时 防止太快
                }
            }
            return readLine.Replace("\r", "");
        }

        private void InitialGun()
        {
            if (this.IsOnline)
            {
                //Com3,9600,None,8,1  string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits
                string[] comstr = ResourceName.Split(',');
                string portName = comstr[0];
                int baudRate = int.Parse(comstr[1]);
                Parity parity = GetParity(comstr[2]);
                int dataBits = int.Parse(comstr[3]);
                StopBits stopBits = GetStopBits(comstr[4]);
                port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            }
        }
        private byte[] HexStringToBytes(string hs)
        {
            string[] strArr = hs.Trim().Split(' ');
            byte[] b = new byte[strArr.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < strArr.Length; i++)
            {
                b[i] = Convert.ToByte(strArr[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return b;
        }

        private Parity GetParity(string ParityValue)
        {
            Parity parity = Parity.None;
            switch (ParityValue)
            {
                case "Even":
                    {
                        parity = Parity.Even;
                    };
                    break;
                case "Mark":
                    {
                        parity = Parity.Mark;
                    };
                    break;
                case "None":
                    {
                        parity = Parity.None;
                    };
                    break;
                case "Odd":
                    {
                        parity = Parity.Odd;
                    };
                    break;
                case "Space":
                    {
                        parity = Parity.Space;
                    };
                    break;
            }
            return parity;
        }

        private StopBits GetStopBits(string GetStopBitsValue)
        {
            StopBits stopBits = StopBits.None;
            switch (GetStopBitsValue)
            {
                case "0":
                    {
                        stopBits = StopBits.None;
                    };
                    break;
                case "1":
                    {
                        stopBits = StopBits.One;
                    };
                    break;
                case "1.5":
                    {
                        stopBits = StopBits.OnePointFive;
                    };
                    break;
                case "2":
                    {
                        stopBits = StopBits.Two;
                    };
                    break;
            }
            return stopBits;
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
