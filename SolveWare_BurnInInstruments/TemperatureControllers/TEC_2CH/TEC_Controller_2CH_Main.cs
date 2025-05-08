
using MeSoft.MeCom.Core;
using MeSoft.MeCom.PhyWrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
 
    public partial class TEC_Controller_2CH : InstrumentBase, ITemperatureController, IInstrumentBase
    {
        protected const float SIMU_VAL_FLOAT = -66f;
        protected const float OFFLINE_VAL_FLOAT = -66f;
        protected const int CHANNEL_COUNT = 4;
        protected MecomFamily _family;
        protected const int MAX_RETRIES = 3;
        public TEC_Controller_2CH(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {

            if ((chassis is MeCom_SerialPortChassis) == false &&
                (chassis is MeCom_TCPChassis) == false)
            {
                throw new Exception("Unable to find expected chassis type [MeCom_TCPChassis] or [MeCom_SerialPortChassis]!");
            }
            else if (chassis is MeCom_SerialPortChassis)
            {
                this.PhysicalConnection = PhysicalConnection.SerialPort;
            }
            else if (chassis is MeCom_TCPChassis)
            {
                this.PhysicalConnection = PhysicalConnection.TCPIP;
            }
          
            this._DeviceID = Convert.ToInt16(address);
 
        }
        ~TEC_Controller_2CH()
        {
            #region 杀线程
            try
            {
                if (_thUpdateState != null)
                {
                    _thUpdateState.Abort();
                    while (_thUpdateState.ThreadState != System.Threading.ThreadState.Aborted)
                    {
                        //当调用Abort方法后，如果thread线程的状态不为Aborted，线程就一直在这里做循环，直到thread线程的状态变为Aborted为止
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            #endregion
        }


        public override void Initialize()
        {
            try
            {
                lock (this._chassis)
                {
                    this._family = new MecomFamily(this._chassis.Visa as IMeComPhy, (byte)this._DeviceID);
                }
                base.Initialize();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override object HandleAllocateChassisResouce()
        {
            try
            {
                lock (this._chassis)
                {
                    this._family = new MecomFamily(this._chassis.Visa as IMeComPhy, (byte)this._DeviceID);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false; 
            }
        }
        public override object HandleChassisOffline()
        {
            try
            {
                //string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- HandleChassisOffline.";
                //Console.WriteLine(msg);
                SuspendRefreshing();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public override object HandleChassisOnline()
        {
            try
            {
                //string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- HandleChassisOnline.";
                //Console.WriteLine(msg);
                ResumeRefreshing(); return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public override void WaitRuning()
        {
        }
        
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var allParams = GetAllReadings();

                for (int i = 1; i <= CHANNEL_COUNT; i++)
                {
                    _CurrentObjectTemp[i] = allParams[i - 1].Temperature;
                    _TemperatureStability[i] = allParams[i - 1].IsStable == 1 ? true : false;
                    _TargetObjectTemp[i] = allParams[i - 1].TempSetpoint;
                    _CurrentTEC_V[i] = allParams[i - 1].Voltage;
                    _CurrentTEC_A[i] = allParams[i - 1].Current;
                    _GetDeviceStatus[i] = allParams[i - 1].Status;
                }
                //string msg = $"[{DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss.fff")}] {this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- RefreshDataOnceCycle time = [{rcnt++}].";
                //Console.WriteLine(msg);
            }
            catch (OperationCanceledException oce)
            {

            }
            catch (Exception ex)
            {
                if (ex.Source.Contains("MeSoft.MeCom.Core"))
                {
                    throw new MeComException("RefreshDataOnceCycle", ex);
                }
            }
        }
        public override void RefreshDataLoop(CancellationToken token)
        {
            do
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    this.RefreshDataOnceCycle(token);
                    Thread.Sleep(2000);
                }
                catch (OperationCanceledException oce)
                {
                    //string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- RefreshDataLoop is cancelled.";
                    //Console.WriteLine(msg);
                    return;
                }
                catch (MeComException mcex)
                {
                    //string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- RefreshDataLoop exception:{mcex.Message}-{mcex.StackTrace}.";
                    //Console.WriteLine(msg);
                    try
                    {
                        this._chassis.ConnectToResource(2000);
                    }
                    catch (Exception reEx)
                    {

                    }
                }
                catch (Exception ex)
                {
                
        
                }
            }
            while (true);
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
            try
            {
                token.ThrowIfCancellationRequested();

                Random rnd = new Random();
                for (int i = 1; i <= CHANNEL_COUNT; i++)
                {
                    _CurrentObjectTemp[i] = rnd.NextDouble(); Task.Delay(50);
                    _TemperatureStability[i] = true; Task.Delay(50);
                    _TargetObjectTemp[i] = rnd.NextDouble(); Task.Delay(50);
                    _CurrentTEC_V[i] = rnd.NextDouble(); Task.Delay(50);
                    _CurrentTEC_A[i] = rnd.NextDouble(); Task.Delay(50);
                    _GetDeviceStatus[i] = rnd.Next(); Task.Delay(50);
                }
            }
            catch (OperationCanceledException oce)
            {

            }
            catch (Exception ex)
            {
                if (ex.Source.Contains("MeSoft.MeCom.Core"))
                {
                    throw new MeComException("RefreshDataOnceCycle", ex);
                }
            }
        }

      
    }
}