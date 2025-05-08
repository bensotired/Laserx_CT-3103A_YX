using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    //20210510 增加温度传感器的获取功能
    public class TempMonitor_DS18B20 : InstrumentBase, IModbus, IInstrumentBase, ITemperatureMonitor
    {
        public enum TempMonitor_RegAddr : uint
        {
            //ModbusID
            ModbusID_Address = 256,
            //9600 -> 0
            //1200
            //2400
            //4800
            //9600
            //19200
            //38400
            //57600
            //115200  -> 8
            BPSIndex_Address = 257,
            //0.1Deg  -> 0
            //1Deg  ->1
            TempResolution_Address = 258,
            //0x01 刷新一下总线上的传感器ID
            ReRefreshSensor_Address = 259,

            //温度Offset
            TempOffset_StartAddress = 512,  //CH0: 1024  CH1:1025

            //扫描出的传感器ID
            Sensor_IDStartAddress = 768,

            //温度获取
            Temp_StartAddress = 1024,  //CH0: 1024  CH1:1025

        }
        protected const int CHANNEL_COUNT = 2;
        protected const int SHORT_ARRAY_DATA_LEN = 2;
        protected const int BYTE_ARRAY_DATA_LEN = 4;
        float[] _Temperatures = new float[CHANNEL_COUNT];

        protected int Slot { get; set; }
        //public string Name { get; set; }
        public virtual float[] Temperatures
        {
            get { return _Temperatures; }
            protected set { _Temperatures = value; }
        }
        public Modbus Modbus
        {
            get
            {
                return this._chassis.Modbus;
            }
        }

        public TempMonitor_DS18B20(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
            this.Slot = Convert.ToInt16(this.Address);

        }
        public override void Initialize()
        {
            base.Initialize();

        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            try
            {
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
                this._Temperatures[i] = (float)Math.Round(rnd.NextDouble(), 3);
            }
        }
        public virtual float[] ReadTemperature()
        {
            short[] values = new short[CHANNEL_COUNT];
            float[] tempf = new float[CHANNEL_COUNT];
            bool isOk = this.Modbus.Function_3(this.Slot, (int)TempMonitor_RegAddr.Temp_StartAddress, CHANNEL_COUNT, ref values);

            if (isOk)
            {
                for (int index = 0; index < values.Length; index++)
                {
                    tempf[index] = (float)(((values[index])) / 10.0);
                    if (tempf[index]>3000)//无穷大的原因是因为没接这个IO
                    {
                        tempf[index] = float.NaN;
                    }
                }
            }
            return tempf;
        }
    }
}