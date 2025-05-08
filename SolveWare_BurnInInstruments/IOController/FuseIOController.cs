using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class FuseIOController : InstrumentBase, IModbus, IInstrumentBase, IFuseIOController
    {
        const int FUSE_IO_ADD_1 = 1000;
        const int FUSE_IO_ADD_2 = 1001;
        protected int Slot { get; set; }

        //输出状态
         bool[] bOutput = new bool[2] { false, false };
        public Modbus Modbus
        {
            get
            {
                return this._chassis.Modbus;
            }
        }

        public bool[] Output
        {
            get
            {
                return bOutput;
            }
        }

        public FuseIOController(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
            this.Slot = Convert.ToInt16(this.Address);

        }
        public void ConnectFuse(bool enable)
        {
            bool ok = true;
            if (this._isOnline == false /*|| this._isSimulation*/) return;
            //List<int> address = new List<int>();
            //2021/3/13 Jerry 为适应新的485通信继电器板，临时改变0:1为1:0
            //3/22改回0:1
            int value = enable ? 0 : 1;

            //给5次机会, 否则抛异常
            for (int i = 0; i < 5; i++)
            {
                ok &= this.Modbus.Function_6(this.Slot, FUSE_IO_ADD_1, (short)value);
                ok &= this.Modbus.Function_6(this.Slot, FUSE_IO_ADD_2, (short)value);
                if (ok)
                    return;
            }
            string msg = string.Format("FuseIOController[{0}] ID[{1}] IP[{2}] ConnectFuse({3}) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource, enable);
            throw new Exception(msg);

        }
        public void UpdataOutputValue(short[] val)
        {
            int count1 = val.Length;
            int count2 = bOutput.Length;
            int count = Math.Min(count1, count2);
            for(int i=0;i<count;i++)
            {
                if (val[i] == 0) bOutput[i] = true;
                else bOutput[i] = false;
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
                //{
                    try
                    {
                        short[] outputBits = new short[2];
                        var ret = this.Modbus.Function_3(Convert.ToInt16(this.Address), FUSE_IO_ADD_1, 2, ref outputBits);
                        if (ret == false)
                        {
                            string msg = string.Format("FuseIOController[{0}] ID[{1}] IP[{2}] Initialize() Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                            throw new Exception(msg);
                        }
                        UpdataOutputValue(outputBits);
                    }
                    catch (Exception ex)
                    {
                        string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- Initialize() exception:{ex.Message}-{ex.StackTrace}.";
                        throw new Exception(msg);
                    }
                //}

            }
            base.Initialize();
        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            try
            {
                short[] outputBits = new short[2];
                var ret = this.Modbus.Function_3(Convert.ToInt16(this.Address), FUSE_IO_ADD_1, 2, ref outputBits);
                if (ret == false)
                {
                    string msg = string.Format("FuseIOController[{0}] ID[{1}] IP[{2}] RefreshDataOnceCycle(CancellationToken token) Modbus fails", this.Name, this.Slot, this.Modbus.ChassisResource);
                    throw new Exception(msg);
                }
                UpdataOutputValue(outputBits);

                //token.ThrowIfCancellationRequested();

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
                    Thread.Sleep(3000);
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

 
    }
}