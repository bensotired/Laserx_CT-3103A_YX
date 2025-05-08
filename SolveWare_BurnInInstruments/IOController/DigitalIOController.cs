using LX_BurnInSolution.Utilities;
using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class DigitalIOController : InstrumentBase, IModbus, IInstrumentBase, IDigitalIOController
    {
        public float[] AD_Value { get; protected set; } //AD收到的电压值
        public bool[] OutputStatus { get; protected set; }
        public bool[] InputStatus { get; protected set; }
        const int OutputBitsStartAddress = 40040;
        const int AD_StartAddress = 40060;   //AD0:40060 40061  AD1: 40062  40063
        const int DA_StartAddress = 40064;   //DA0:40064 40065  DA1: 40066  40067
        const int InputBitsStartAddress = 10000;
        const int SpecificDefineFuncAddress = 20000;
        const int ChannelCount = 16;
        protected int Slot { get; set; }

        public Modbus Modbus
        {
            get
            {
                return this._chassis.Modbus;
            }
        }

        public DigitalIOController(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
            this.Slot = Convert.ToInt16(this.Address);
            OutputStatus = new bool[ChannelCount];
            InputStatus = new bool[ChannelCount];
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            try
            {
 
                token.ThrowIfCancellationRequested();
                this.InputStatus = GetInputStatus();
       
                this.OutputStatus = GetOutputStatus();
    
                this.AD_Value = Get_AD_Value();    //20210222 AD电压值
            }
            catch (OperationCanceledException oex)
            {
                throw oex;
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
                    Thread.Sleep(1000);
                }
                catch (OperationCanceledException ocex)
                {
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
            
        }

        bool[] GetInputStatus()
        {
            bool[] inputBits = new bool[ChannelCount];
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_2(this.Slot, InputBitsStartAddress, ChannelCount, ref inputBits);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] GetInputStatus() Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource );
                throw new Exception(msg);
            }

            return inputBits;
        }
        bool[] GetOutputStatus()
        {
            bool[] status = new bool[ChannelCount];

            short[] outputBits = new short[ChannelCount];
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_3(this.Slot, OutputBitsStartAddress, ChannelCount, ref outputBits);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] GetOutputStatus() Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
            for (int i = 0; i < ChannelCount; i++)
            {
                status[i] = Convert.ToBoolean(outputBits[i]);
            }

            return status;
        }
        public int GetChannelValue(int channel)
        {
            int len = 1;
            short[] val = new short[len];

            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_3(this.Slot, channel + OutputBitsStartAddress, len, ref val);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] GetChannelValue(int channel) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
            return val[0];
        }
        public void OffChannel(int channel)
        {
            if (this._isOnline == false /*|| this._isSimulation*/)
            {
                return;
            }
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_6(this.Slot, channel + OutputBitsStartAddress, 0);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] OffChannel(int channel) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
        }
        public void OnChannel(int channel)
        {
            if (this._isOnline == false /*|| this._isSimulation*/)
            {
                return;
            }
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_6(this.Slot, channel + OutputBitsStartAddress, 1);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] OnChannel(int channel) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
        }
        public void FlashChannel(int channel, int interval)
        {
            if (this._isOnline == false /*|| this._isSimulation*/)
            {
                return;
            }
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_6(this.Slot, channel + OutputBitsStartAddress, (short)interval);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] FlashChannel(int channel, int interval) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
        }

        public void DIOModBusFunc5Extend(int address, int val)
        {
            if (this._isOnline == false /*|| this._isSimulation*/)
            {
                return;
            }

            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_5(this.Slot, address, val);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] DIOModBusFunc5Extend(int address, int val) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
        }
        //给某个地址写某个值
        public void DIOModBusFunc6Extend(int address, int val)
        {
            if (this._isOnline == false /*|| this._isSimulation*/)
            {
                return;
            }
 
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_6(this.Slot, address, (short)val);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] DIOModBusFunc6Extend(int address, int val) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
        }
        //读某个地址的值
        public bool DIOModBusFunc1Extend(int address)
        {
            if (this._isOnline == false/* || this._isSimulation*/)
            {
                return true;
            }
            int len = 1;
            bool[] val = new bool[len];
  
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_1(this.Slot, address, len, ref val);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] DIOModBusFunc1Extend(int address) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
            return val[0];
        }

        //2个AD都读回来
        public float[] Get_AD_Value()
        {
            int len = 2;
            float[] rtn = new float[len];
            if (this._isOnline == false /*|| this._isSimulation*/)
            {
                rtn[0] = 0f;
                rtn[1] = 0f;
                return rtn;
            }
            bool ok = true;
            short[] val = new short[len * 2];
            for (int i = 0; i < 2; i++)
            {
                ok = this.Modbus.Function_3(this.Slot, AD_StartAddress, val.Length, ref val);
                if (ok)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            if (ok == false)
            {
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] Get_AD_Value() Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }
            short[] temp = new short[2];

            for (int i = 0; i < len; i++)
            {
                Array.Copy(val, i * 2, temp, 0, 2);
                rtn[i] = BaseDataConverter.ConvertUshortToFloatCDAB(temp);
            }

            return rtn;
        }
    }
}