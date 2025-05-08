using LX_BurnInSolution.Utilities;
using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class Relay8CHController : InstrumentBase, IModbus, IInstrumentBase, IDigitalIOController
    {
        public bool[] OutputStatus { get; protected set; }
        public bool[] InputStatus { get; protected set; }
        private const int OutputBitsStartAddress = 20000;
        private const int InputBitsStartAddress = 10000;
        private const int ChannelCount = 8;
        protected int Slot { get; set; }

        public Modbus Modbus
        {
            get
            {
                return this._chassis.Modbus;
            }
        }

        public float[] AD_Value
        {
            get
            {
                return null;
            }
        }

        public Relay8CHController(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
            this.Slot = Convert.ToInt16(this.Address);
            OutputStatus = new bool[ChannelCount];
            InputStatus = new bool[ChannelCount];

            chassis.Modbus.SetDelay_ms(50);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            try
            {
                if (IsOnline && this._chassis.IsOnline)
                {
                    token.ThrowIfCancellationRequested();
                    this.InputStatus = GetInputStatus();

                    this.OutputStatus = GetOutputStatus();
                }
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

        private bool[] GetInputStatus()
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
                string msg = string.Format("DigitalIOController[{0}] ID[{1}] IP[{2}] GetInputStatus() Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                throw new Exception(msg);
            }

            return inputBits;
        }

        private bool[] GetOutputStatus()
        {
            bool[] status = new bool[ChannelCount];

            short[] outputBits = new short[ChannelCount];
            bool ok = true;
            for (int i = 0; i < 2; i++)
            {
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_3(this.Slot, OutputBitsStartAddress, ChannelCount, ref outputBits);
                }
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
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_3(this.Slot, channel + OutputBitsStartAddress, len, ref val);
                }
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
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_5(this.Slot, channel + OutputBitsStartAddress, 0);
                }
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
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_5(this.Slot, channel + OutputBitsStartAddress, 0xFF00);
                }
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
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_6(this.Slot, channel + OutputBitsStartAddress, (short)interval);
                }
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
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_5(this.Slot, address, val);
                }
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
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_6(this.Slot, address, (short)val);
                }
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
                if (IsOnline && this._chassis.IsOnline)
                {
                    ok = this.Modbus.Function_1(this.Slot, address, len, ref val);
                }
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

        public float[] Get_AD_Value()
        {
            throw new NotImplementedException();
        }
    }
}