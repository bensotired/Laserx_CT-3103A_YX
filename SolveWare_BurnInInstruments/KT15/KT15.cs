using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class KT15 : InstrumentBase, IModbus, IInstrumentBase
    {
        protected int ID = 0;
        protected const int ADDRESS = 4;
        protected const int ADDRESS_2 = 1;
        protected const int BYTE_ARRAY_DATA_LEN = 2;

        public KT15(string name, string address, IInstrumentChassis chassis)
           : base(name, address, chassis)
        {
            this.ID = int.Parse(address);
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
        }

        public double SetAngle
        {
            set
            {
                lock (new object())
                {
                    if (this._isOnline == false)
                    {
                        return;
                    }
                    var response = (value / 360) * 3200;

                    var sa = BaseDataConverter.LongToUshort((int)response);
                    byte[] re = BitConverter.GetBytes(sa[0]);
                    byte[] re1 = BitConverter.GetBytes(sa[1]);
                    byte[] byteList = new byte[4];
                    byteList[0] = re1[1];
                    byteList[1] = re1[0];
                    byteList[2] = re[1];
                    byteList[3] = re[0];
                    bool isOK = this.Modbus.Function_16(ID, ADDRESS, BYTE_ARRAY_DATA_LEN, byteList);
                    if (!isOK)
                    {
                        string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- SetTempCoeff error!.";
                        throw new Exception(msg);
                    }
                    Thread.Sleep(50);
                    this.isEnable = true;
                }

            }
        }
        private bool isEnable
        {
            set
            {

                if (this._isOnline == false)
                {
                    return;
                }
                int run = value ? 0xFF00 : 0;
                bool ok = this.Modbus.Function_5(ID, ADDRESS_2, run);
                if (ok == false)
                {
                    string msg = string.Format("KT15[{0}] ID[{1}] IP[{2}] KT15ModBusFunc5Extend(int address, int val) Modbus fails", this.Name, ID, this.Modbus.ChassisResource);
                    throw new Exception(msg);
                }
                int len = 1;
                bool[] val = new bool[len];
                while (true)
                {
                    bool isSuccess = this.Modbus.Function_1(ID, ADDRESS_2,len, ref val);
                    if (val[0] == false)
                        break;
               
                    
                }

            }
        }
        public Modbus Modbus
        {
            get
            {
                return this._chassis.Modbus;
            }
        }
    }
}
