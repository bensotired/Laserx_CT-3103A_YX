using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class TemperatureMonitor : InstrumentBase, IModbus, IInstrumentBase, ITemperatureMonitor
    {
        enum TempMonitor_RegAddr : int
        {
            GetTemperature = 1000,
            SetTempCoeff = 974,
            T_High = 974,
            R_High = 976,
            T_Mid = 978,
            R_Mid = 980,
            T_Low = 982,
            R_Low = 984
        }
        const int HardwareVersion = 4;                     //当前上位机运行需要的固件版本号
        const int HardwareVersion_Address = 973;           //硬件版本号地址
 
        protected const int CHANNEL_COUNT = 16;
        protected const int SHORT_ARRAY_DATA_LEN = 2;
        protected const int BYTE_ARRAY_DATA_LEN = 4;

        protected int Slot { get; set; }
        float[] _Temperatures = new float[CHANNEL_COUNT];
        public Modbus Modbus
        {
            get
            {
                return this._chassis.Modbus;
            }
        }
        public TemperatureMonitor(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
            this.Slot = Convert.ToInt16(this.Address);
        }
        public virtual float[] Temperatures
        {
            get { return _Temperatures; }
            protected set { _Temperatures = value; }
        }
        public override void CheckHardwareLatest()
        {
            int realV = Convert.ToInt16(GetHardwareVersion());
            if (realV < HardwareVersion)
            {
                throw new Exception(string.Format("警告：程序初始化失败，[TemperatureMonitor]Resource:{0} Address:{1} 当前固件版本：{2}，软件最低需要版本：{3}，版本不一致，请联系相关工程师处理！", this.Modbus.ChassisResource, this.Slot, realV, HardwareVersion));
            }
        }

        public override void Initialize()
        {
            if (this._isOnline)
            {
                //if (this._isSimulation)
                //{

                //}
                //else
                {
                    this.CheckHardwareLatest();
                }
                
            }
            base.Initialize();
        }
        public override object GetHardwareVersion()
        {
            if (this._isOnline == false /*|| this._isSimulation*/)
            {
                return int.MaxValue;
            }
            short[] val = new short[1];
            bool isok = this.Modbus.Function_3(this.Slot, HardwareVersion_Address, 1, ref val);
            if (isok == false)
            {
                throw new Exception("TemperatureMonitor GetHardwareVersion exception");
            }
            return val[0];
        }



        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            try
            {
                //Console.WriteLine("{0} start reading", this.Name);
                this._Temperatures = this.ReadTemperature();
            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }
        public override void RefreshDataLoop(CancellationToken token)
        {

            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    token.ThrowIfCancellationRequested();
                    this.RefreshDataOnceCycle(token);
                    Thread.Sleep(2000);
                }
                catch (OperationCanceledException ocex)
                {
                    //响应取消操作前把喂狗功能关掉
                    //this.EnableFeedDog(false);
                    return;
                }
                catch (Exception ex)
                {
                    //非取消操作不退出循环
                }
            }
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            Random rnd = new Random();
            for (int i = 0; i < CHANNEL_COUNT; i++)
            {
                this._Temperatures[i] = (float)Math.Round(rnd.NextDouble(), 3); Thread.Sleep(10);  
            }
        }

        float[] ReadTemperature()
        {
            if (this._isOnline == false)
            {
                return new float[CHANNEL_COUNT];
            }
            short[] values = new short[CHANNEL_COUNT * 2];
            bool isOk = this.Modbus.Function_3(this.Slot, (int)TempMonitor_RegAddr.GetTemperature, CHANNEL_COUNT * 2, ref values);

            if (isOk==false)
            {
                Thread.Sleep(200);
                isOk = this.Modbus.Function_3(this.Slot, (int)TempMonitor_RegAddr.GetTemperature, CHANNEL_COUNT * 2, ref values);
                if (isOk)
                {
                    string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- ReadTemperature exception:ModbusStatus.CRCCheckFailed.";
                    throw new Exception(msg);
                }
            }

            List<float> tempf = new List<float>();
            List<byte> temp = new List<byte>();
            for (int index = values.Length - 1; index >= 0; index--)
            {
                var bytes = BitConverter.GetBytes(values[index]);
                temp.AddRange(bytes);
            }
            byte[] sourBytes = temp.ToArray();
            byte[] destBytes = new byte[BYTE_ARRAY_DATA_LEN];
            for (int index = 0; index < temp.Count; index += BYTE_ARRAY_DATA_LEN)
            {
                Array.Copy(sourBytes, index, destBytes, 0, BYTE_ARRAY_DATA_LEN);
                var fVal = BitConverter.ToSingle(destBytes, 0);
                tempf.Add(fVal);
            }
            tempf.Reverse();
            return tempf.ToArray();
        }
        //写温度系数
        public virtual void SetTempCoeff(float tempHigh, float resHigh, float tempMid, float resMid, float tempLow, float resLow)
        {

            if (this._isOnline == false) return;

            byte[] val = new byte[6 * BYTE_ARRAY_DATA_LEN];

            int i = 0;
            byte[] chVal = new byte[BYTE_ARRAY_DATA_LEN];
            chVal = BitConverter.GetBytes(tempHigh);
            Array.Reverse(chVal);
            Array.Copy(chVal, 0, val, i * BYTE_ARRAY_DATA_LEN, chVal.Length);

            i++;
            //chVal = new byte[BYTE_ARRAY_DATA_LEN];
            chVal = BitConverter.GetBytes(resHigh);
            Array.Reverse(chVal);
            Array.Copy(chVal, 0, val, i * BYTE_ARRAY_DATA_LEN, chVal.Length);

            i++;
            //chVal = new byte[BYTE_ARRAY_DATA_LEN];
            chVal = BitConverter.GetBytes(tempMid);
            Array.Reverse(chVal);
            Array.Copy(chVal, 0, val, i * BYTE_ARRAY_DATA_LEN, chVal.Length);

            i++;
            //chVal = new byte[BYTE_ARRAY_DATA_LEN];
            chVal = BitConverter.GetBytes(resMid);
            Array.Reverse(chVal);
            Array.Copy(chVal, 0, val, i * BYTE_ARRAY_DATA_LEN, chVal.Length);

            i++;
            //chVal = new byte[BYTE_ARRAY_DATA_LEN];
            chVal = BitConverter.GetBytes(tempLow);
            Array.Reverse(chVal);
            Array.Copy(chVal, 0, val, i * BYTE_ARRAY_DATA_LEN, chVal.Length);

            i++;
            //chVal = new byte[BYTE_ARRAY_DATA_LEN];
            chVal = BitConverter.GetBytes(resLow);
            Array.Reverse(chVal);
            Array.Copy(chVal, 0, val, i * BYTE_ARRAY_DATA_LEN, chVal.Length);

            bool isOK = this.Modbus.Function_16(this.Slot, (int)TempMonitor_RegAddr.SetTempCoeff, 12, val);

            if (!isOK)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- SetTempCoeff error!.";
                throw new Exception(msg) ;
            }
            
        }

        //读温度系数
        public virtual void GetTempCoeff(out float tempHigh, out float resHigh, out float tempMid, out float resMid, out float tempLow, out float resLow)
        {

            tempHigh = 0;
            resHigh = 0;
            tempMid = 0;
            resMid = 0;
            tempLow = 0;
            resLow = 0;
            if (this._isOnline == false) return;
            tempHigh = ReadCoeffValue((int)TempMonitor_RegAddr.T_High);
            resHigh = ReadCoeffValue((int)TempMonitor_RegAddr.R_High);

            tempMid = ReadCoeffValue((int)TempMonitor_RegAddr.T_Mid);
            resMid = ReadCoeffValue((int)TempMonitor_RegAddr.R_Mid);

            tempLow = ReadCoeffValue((int)TempMonitor_RegAddr.T_Low);
            resLow = ReadCoeffValue((int)TempMonitor_RegAddr.R_Low);
        }


        private float ReadCoeffValue(int VerAddress)
        {
            short[] val = new short[2];

            bool isok = this.Modbus.Function_3(this.Slot, VerAddress, 2, ref val);
            if (isok == false)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- GetTempCoeff exception.";
                throw new Exception(msg);
            }

            return BaseDataConverter.ConvertUshortToFloatABCD(val);
        }
    }
}