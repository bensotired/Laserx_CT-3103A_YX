using LX_BurnInSolution.Utilities;
using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{

    public partial class TEC_Controller_4CH : InstrumentBase, IInstrumentBase
    {

        //显示窗口
        public void ShowParameterWindow()
        {

        }

        #region 看门狗 喂狗和使能

        public void EnableFeedDog(bool isEnable)
        {
            //if (this._isSimulation) { return; }
            if (this._isOnline == false) return;
            try
            {
                //20201214 加锁
                lock (this._chassis)
                {
                    //20211029 TEC增加失败的机会
                    for (int rty = 1; rty <= MAX_RETRIES; rty++)
                    {
                        try
                        {

                            //isEnable = false; //调试关闭看门狗
                            if (isEnable)
                            {
                                //20201204 如果看门狗已经打开，就不再使能
                                int value =this._family.MeComBasicCmd  .GetINT32Value(null, (ushort)eMeerstetterTECAddress.EnableDog, 1);
                                if ((value & 0x10) == 0)
                                {
                                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.EnableDog, 1, isEnable ? 16 : 0);
                                }
                            }
                            else
                            {
                                this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.EnableDog, 1, isEnable ? 16 : 0);
                            }
                            Thread.Sleep(50);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(100);
                            if (rty == MAX_RETRIES)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC看门狗使能失败:" + ex));
            }
        }

        public void FeedDog_S(int value)
        {
            //if (this._isSimulation) { return; }
            if (this._isOnline == false) return;
            lock (this._chassis)
            {
                //20211029 TEC增加失败的机会
                for (int rty = 1; rty <= MAX_RETRIES; rty++)
                {
                    try
                    {
                        this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.FeedDod, 1, value);
                        Thread.Sleep(50);
                        break;

                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(100);
                        if (rty == MAX_RETRIES)
                        {
                            throw ex;
                        }

                    }
                }
            }
        }

        #endregion



        public void SetTempUpperLimit(int ch, double temperature)
        {
            throw new NotImplementedException();
        }


        #region 设定通讯参数




        //返回连接
        public PhysicalConnection PhysicalConnection
        {
            get;
            set;
        }



        //枚举串口,根据ID连接
        //protected bool ReConnect()
        //{
        //    lock (this.Chassis)
        //    {
        //        Console.WriteLine("Enter  {0}", this.DeviceID);
        //        //离线模式
        //        if (this._isOnline == false) return true;

        //        MeComPhySerialPort meComPhySerialPort = null; //物理端口
        //        //MeComPhyTcp meComPhyTcp = null;
        //        MeComQuerySet meComQuerySet; //通讯
        //        MeComBasicCmd meComBasicCmd; //G1指令
        //        MeComG2Cmd meComG2Cmd;    //G2指令
        //        try
        //        {
        //            if (this.PhysicalConnection == PhysicalConnection.SerialPort)
        //            {
        //                _meComPhySerialPort.Close();
        //            }
        //            else if (this.PhysicalConnection == PhysicalConnection.TCPIP)
        //            {
        //                //(this.Chassis as MeComChassis).Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        #region serial port reconnect
        //        if (this.PhysicalConnection == PhysicalConnection.SerialPort)
        //        {
        //            //列出所有COM口
        //            string[] portList = System.IO.Ports.SerialPort.GetPortNames();

        //            foreach (string portnamestr in portList)
        //            {
        //                try
        //                {
        //                    try
        //                    {
        //                        //打开
        //                        meComPhySerialPort = new MeComPhySerialPort();

        //                        //超时设定
        //                        meComPhySerialPort.ReadTimeout = _timeout_ms;
        //                        meComPhySerialPort.WriteTimeout = _timeout_ms;

        //                        meComPhySerialPort.OpenWithDefaultSettings(portnamestr, _baudRate);
        //                        if (meComPhySerialPort.IsOpen == false)
        //                        {
        //                            throw (new Exception());
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw ex;
        //                    }

        //                    try
        //                    {
        //                        meComQuerySet = new MeComQuerySet(meComPhySerialPort);
        //                        meComQuerySet.SetDefaultDeviceAddress((byte)this._DeviceID);
        //                        meComQuerySet.SetIsReady(true);  //将IsReady = ON

        //                        meComBasicCmd = new MeComBasicCmd(meComQuerySet);
        //                        meComG2Cmd = new MeComG2Cmd(meComQuerySet);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw ex;
        //                    }


        //                    //获取型号 + 判断ID
        //                    try
        //                    {

        //                        //型号
        //                        string identString = meComBasicCmd.GetIdentString(null);
        //                        //如果不是TEC的话
        //                        if (identString.ToUpper().Contains(_DeviceType) == false)
        //                        {
        //                            throw (new Exception());
        //                        }

        //                        //如果ID=Add(2051_DeviceAddress)不是需求的话
        //                        int DeviceAddress = meComBasicCmd.GetINT32Value(null, (ushort)eMeerstetterTECAddress.DeviceAddress, 1);

        //                        //如果ID不对
        //                        if (_DeviceID != DeviceAddress)
        //                        {
        //                            throw (new Exception());
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw ex;
        //                    }

        //                    //能够到此处,说明已经成功, 记录当前的信息
        //                    _portName = portnamestr;
        //                    _meComPhySerialPort = meComPhySerialPort;
        //                    _meComQuerySet = meComQuerySet;
        //                    _meComBasicCmd = meComBasicCmd;
        //                    _meComG2Cmd = meComG2Cmd;

        //                    return true;

        //                }
        //                catch (Exception ex)
        //                {
        //                    try { meComPhySerialPort.Close(); }
        //                    catch (Exception) { }
        //                    meComPhySerialPort = null;
        //                    meComQuerySet = null;
        //                    meComBasicCmd = null;
        //                    meComG2Cmd = null;

        //                    continue;
        //                }
        //            }
        //        }
        //        #endregion
        //        else if (this.PhysicalConnection == PhysicalConnection.TCPIP)
        //        {
        //            //meComPhyTcp = new MeComPhyTcp();
        //            //meComPhyTcp.OpenClient(this.IpAddress, this.Port, 5000);

        //            //(this.Chassis as MeComChassis).Initialize();
        //            try
        //            {
        //                this.Chassis.Initialize();
        //            }
        //            catch
        //            {


        //                return false;
        //            }

        //            this.meComPhy = this.Chassis.Visa as IMeComPhy;
        //            meComQuerySet = new MeComQuerySet(this.meComPhy);
        //            meComQuerySet.SetDefaultDeviceAddress((byte)this._DeviceID);
        //            meComQuerySet.SetIsReady(true);  //启动的时候将IsReady = ON
        //            meComBasicCmd = new MeComBasicCmd(meComQuerySet);
        //            meComG2Cmd = new MeComG2Cmd(meComQuerySet);
        //            //string identString = meComBasicCmd.GetIdentString(null);


        //            _meComQuerySet = meComQuerySet;
        //            _meComBasicCmd = meComBasicCmd;
        //            _meComG2Cmd = meComG2Cmd;

        //            Console.WriteLine("Out pass  {0}", this.DeviceID);
        //            return true;
        //        }
        //        Console.WriteLine("Out fail  {0}", this.DeviceID);
        //        return false;
        //    }
        //}
        public TECValues[] GetAllReadings()
        {
            TECValues[] tecValues = new TECValues[4];
            for (int instance = 0; instance < 4; instance++)
            {
                string respon = string.Empty;
                lock (this._chassis)
                {
                    respon = this._family.MeComBasicCmd.GetStringValue((byte)this._DeviceID, 50009, (byte)(instance + 1));
                    Thread.Sleep(50);
                }
                //00000001 00000001 C29E7C69 00000000 BC0C08C0 C0B1C71C\0\0

                string statusStr = respon.Substring(0, 8);
                tecValues[instance].Status = (int)BaseDataConverter.ConvertBytesToInt(BaseDataConverter.ConvertHexStringToByteArray(statusStr, false));
                string isStableStr = respon.Substring(8, 8);
                tecValues[instance].IsStable = (int)BaseDataConverter.ConvertBytesToInt(BaseDataConverter.ConvertHexStringToByteArray(isStableStr, false));
                string tempStr = respon.Substring(16, 8);
                tecValues[instance].Temperature = BitConverter.ToSingle(BaseDataConverter.ConvertHexStringToByteArray(tempStr, true), 0);
                string tempSpStr = respon.Substring(24, 8);
                tecValues[instance].TempSetpoint = BitConverter.ToSingle(BaseDataConverter.ConvertHexStringToByteArray(tempSpStr, true), 0);
                string currentStr = respon.Substring(32, 8);
                tecValues[instance].Current = BitConverter.ToSingle(BaseDataConverter.ConvertHexStringToByteArray(currentStr, true), 0);
                string voltageStr = respon.Substring(40, 8);
                tecValues[instance].Voltage = BitConverter.ToSingle(BaseDataConverter.ConvertHexStringToByteArray(voltageStr, true), 0);
            }
            return tecValues;
        }

        #endregion

        int pcnt = 0;

        public void GetTargetObjectPID(int ch)
        {
            if (this._isOnline == false) { return; }
            lock (this._chassis)
            {
                _PID_P[ch] = this._family.MeComBasicCmd.GetFloatValue(null, (ushort)eMeerstetterTECAddress.PID_P, (byte)ch);
                _PID_I[ch] = this._family.MeComBasicCmd.GetFloatValue(null, (ushort)eMeerstetterTECAddress.PID_I, (byte)ch);
                _PID_D[ch] = this._family.MeComBasicCmd.GetFloatValue(null, (ushort)eMeerstetterTECAddress.PID_D, (byte)ch);
                string msg = $"[{DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss.fff")}]{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- GetTargetObjectPID time = [{pcnt++}].";
                Console.WriteLine(msg);
            }
        }
        int scnt = 0;
        //设定目标温度
        public void SetTargetObjectTemperature(int ch, double Value)
        {
            //if (this._isSimulation) return;
            if (this._isOnline == false) return;

            if (double.IsNaN(Value))
            {
                throw (new Exception("数据异常"));
            }
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            _TargetObjectTemp[ch] = Value;
            int rty = 0;
            for (rty = 1; rty <= MAX_RETRIES; rty++)
            {
                try
                {
                    lock (this._chassis)
                    {
                        this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.TemperatureSetPoint, (byte)ch, (float)Value);
                        //设定温度
                        string msg = $"[{DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss.fff")}]{this.Name} address[{this.Address}] chassis [{this._chassis?.Name}] resource [{this._chassis?.Resource}]- SetTargetObjectTemperature time = [{scnt++}].";
                        Console.WriteLine(msg);
                        Thread.Sleep(100);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (rty == MAX_RETRIES)
                    {
                        throw (new Exception("温度设定失败:" + ex));
                    }

                }
            }

        }
        public virtual void SetTargetObjectTemperatureEnable(int ch, int en)
        {
            //if (this._isSimulation ) return;
            if (this._isOnline == false) return;
            int rty = 0;
            for (rty = 1; rty <= MAX_RETRIES; rty++)
            {
                try
                {
                    //启动温度控制
                    //0: Static OFF
                    //1: Static ON
                    //2: Live OFF/ON (See ID 50000)
                    //3: HW Enable (Check GPIO Config)

                    lock (this._chassis)
                    {
                        this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.OutputEnabled, (byte)ch, en);
                        //设定温度

                        Thread.Sleep(100);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (rty == MAX_RETRIES)
                    {
                        throw (new Exception("启动控制失败:" + ex));
                    }
                }
            }
        }
        public void SetTargetObjectTemperatureError(int ch, double Value_up, double Value_low)
        {
            //首先判断是否需要连接
            if (this._isOnline == false) return;

            if (double.IsNaN(Value_up) ||
                double.IsNaN(Value_low))
            {
                throw (new Exception("数据异常"));
            }

            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            try
            {
                lock (this._chassis)
                {
                    //设定温度上限
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.TempUpperErr, (byte)ch, (float)Value_up);
                }

            }
            catch (Exception ex)
            {
                throw (new Exception("温度上限设定失败:" + ex));
            }
            try
            {
                lock (this._chassis)
                {
                    //设定温度下限
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.TempLowerErr, (byte)ch, (float)Value_low);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("温度下限设定失败:" + ex));
            }
        }

        public void SetTargetObjectLimitCurrent(int ch, double Value)
        {
            //首先判断是否需要连接
            if (this._isOnline == false) return;
            if (double.IsNaN(Value))
            {
                throw (new Exception("数据异常"));
            }

            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            float CurrentValue_Max_A = (float)Value;
            float CurrentValue_Min_A = -1 * CurrentValue_Max_A;

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.CurrentProtectionMaxLimit, (byte)ch, (float)CurrentValue_Max_A); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电流上限设定失败:" + ex));
            }
            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.CurrentProtectionMinLimit, (byte)ch, (float)CurrentValue_Min_A); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电流下限设定失败:" + ex));
            }
        }

        public void SetTargetObjectLimitVoltage(int ch, double Value)
        {
            //首先判断是否需要连接
            if (this._isOnline == false) return;
            if (double.IsNaN(Value))
            {
                throw (new Exception("数据异常"));
            }


            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }

            float VoltageValue_Max_V = (float)Value;
            float VoltageValue_Min_V = -1 * VoltageValue_Max_V;

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.VoltageProtectionMaxLimit, (byte)ch, (float)VoltageValue_Max_V); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电压上限设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.VoltageProtectionMinLimit, (byte)ch, (float)VoltageValue_Min_V); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电压下限设定失败:" + ex));
            }

        }

        public void SetTargetObjectPID(int ch, double ValueP, double ValueI, double ValueD)
        {
            //首先判断是否需要连接
            if (this._isOnline == false) return;


            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            try
            {
                lock (this._chassis)
                {
                    //设定PIDGain
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.PIDGain, (byte)ch, (float)ValueP);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("PIDGain设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    //设定PIDIntegral
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.PIDIntegral, (byte)ch, (float)ValueI);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("PIDIntegral设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    //设定PIDIntegral
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.PIDDerivative, (byte)ch, (float)ValueD);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("PIDDerivative设定失败:" + ex));
            }
        }
        public void SetTargetObjectTempertureCalibration(int ch, double ValueGain, double ValueOffset)
        {
            //首先判断是否需要连接
            if (this._isOnline == false) return;

            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            try
            {
                lock (this._chassis)
                {
                    //设定TempGain
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.TempGain, (byte)ch, (float)ValueGain);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TempGain设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    //设定TempOffset
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.TempOffset, (byte)ch, (float)ValueOffset);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TempOffset设定失败:" + ex));
            }

        }
        public bool IsChannelTemperatureStabled(int ch)
        {
            //if (this._isSimulation) return true;
            if (this._isOnline == false) return false;
            bool isStable = false;
            lock (this._chassis)
            {
                //温度是否已经稳定
                if (this._family.MeComBasicCmd.GetINT32Value(null, (ushort)eMeerstetterTECAddress.GetStableStatus, (byte)ch) == 2)
                {
                    isStable = true;
                }
                else
                {
                    isStable = false;
                }
            }
            return isStable;
        }

        #region NTC参数
        public void SetNTCSensorPara(int ch,
            double ValueTempU_DegC, double ValueRU_KOhm,
            double ValueTempM_DegC, double ValueRM_KOhm,
            double ValueTempL_DegC, double ValueRL_KOhm)
        {
            //if (this._isSimulation) return;

            if (this._isOnline == false) return;

            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.NTCTempU, (byte)ch, (float)ValueTempU_DegC);
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("NTC高温温度设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.NTCRU, (byte)ch, (float)ValueRU_KOhm);
                    Thread.Sleep(50);
                }

            }
            catch (Exception ex)
            {
                throw (new Exception("NTC高温电阻设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.NTCTempM, (byte)ch, (float)ValueTempM_DegC);
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("NTC中温温度设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.NTCRM, (byte)ch, (float)ValueRM_KOhm);
                    Thread.Sleep(50);
                }

            }
            catch (Exception ex)
            {
                throw (new Exception("NTC中温电阻设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.NTCTempL, (byte)ch, (float)ValueTempL_DegC);
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("NTC低温温度设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.NTCRL, (byte)ch, (float)ValueRL_KOhm);
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("NT低温电阻设定失败:" + ex));
            }

            //保存参数
            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.Save, (byte)ch, 1);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("保存失败:" + ex));
            }
        }
        public void GetNTCSensorPara(int ch,
            out double ValueTempU_DegC, out double ValueRU_KOhm,
            out double ValueTempM_DegC, out double ValueRM_KOhm,
            out double ValueTempL_DegC, out double ValueRL_KOhm)
        {

            ValueTempU_DegC = 0;
            ValueTempM_DegC = 0;
            ValueTempL_DegC = 0;

            ValueRU_KOhm = 0;
            ValueRM_KOhm = 0;
            ValueRL_KOhm = 0;

            //if (this._isSimulation) return;
            if (this._isOnline == false) return;

            lock (this._chassis)
            {
                ValueTempU_DegC = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.NTCTempU, (byte)ch);
                ValueTempM_DegC = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.NTCTempM, (byte)ch);
                ValueTempL_DegC = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.NTCTempL, (byte)ch);

                ValueRU_KOhm = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.NTCRU, (byte)ch);
                ValueRM_KOhm = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.NTCRM, (byte)ch);
                ValueRL_KOhm = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.NTCRL, (byte)ch);
            }
        }

        #endregion

        #region TEC极限电流电压
        public void SetTECLimit(int ch, double CurrentValue_A, double VoltageValue_V)
        {
            //if (this._isSimulation) return;
            if (this._isOnline == false) return;


            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }

            float CurrentValue_Max_A = (float)CurrentValue_A;
            float VoltageValue_Max_V = (float)VoltageValue_V;

            float CurrentValue_Min_A = -1 * CurrentValue_Max_A;
            float VoltageValue_Min_V = -1 * VoltageValue_Max_V;

            try
            {
                lock (this._chassis)
                {

                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.CurrentProtectionMaxLimit, (byte)ch, (float)CurrentValue_Max_A); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电流上限设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.VoltageProtectionMaxLimit, (byte)ch, (float)VoltageValue_Max_V); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电压上限设定失败:" + ex));
            }


            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.CurrentProtectionMinLimit, (byte)ch, (float)CurrentValue_Min_A); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电流下限设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.VoltageProtectionMinLimit, (byte)ch, (float)VoltageValue_Min_V); Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("TEC电压下限设定失败:" + ex));
            }


            //保存参数
            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.Save, (byte)ch, 1);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("保存失败:" + ex));
            }
        }

        //得到TEC电流电压
        public void GetTECLimit(int ch, out double CurrentValue_A, out double VoltageValue_V)
        {
            CurrentValue_A = 0;
            VoltageValue_V = 0;
            //if (this._isSimulation) return;
            if (this._isOnline == false) return;
            lock (this._chassis)  
            {
                CurrentValue_A = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.CurrentProtectionMaxLimit, (byte)ch);
                VoltageValue_V = this._family.MeComBasicCmd.GetFloatValue((byte)this._DeviceID, (ushort)eMeerstetterTECAddress.VoltageProtectionMaxLimit, (byte)ch);
            }
        }

        #endregion

        //设定温度稳定上下限,稳定时间
        public void SetTargetObjectStabilityParamerter(int ch, double DeviationValue, double WindowValue)
        {
            //if (this._isSimulation) return;
            //首先判断是否需要连接
            if (this._isOnline == false) return;

        
            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            try
            {
                lock (this._chassis)
                {
                    //设定
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.TemperatureDeviation, (byte)ch, (float)DeviationValue);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("温度稳定上下限设定失败:" + ex));
            }

            try
            {
                lock (this._chassis)
                {
                    //设定
                    this._family.MeComBasicCmd.SetFloatValue(null, (ushort)eMeerstetterTECAddress.MinTimeInWindow, (byte)ch, (float)WindowValue);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("温度稳定稳定时间设定失败:" + ex));
            }
        }
        public void AutoTuneStart(int ch)
        {
            //if (this._isSimulation) return;
            //首先判断是否需要连接
            if (this._isOnline == false) return;

        
            //最多2通道
            if (!(ch == 1 || ch == 2 || ch == 3 || ch == 4))
            {
                throw (new Exception("通道号异常"));
            }
            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.AutoTuneStart, (byte)ch, 1);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("自整定开启失败:" + ex));
            }
        }

        public void Save(int ch)
        {
            //if (this._isSimulation) return;
            //首先判断是否需要连接
            if (this._isOnline == false) return;

        
            //最多2通道
            if (!(ch == 1 || ch == 2))
            {
                throw (new Exception("通道号异常"));
            }
            try
            {
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.Save, (byte)ch, 1);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("保存失败:" + ex));
            }
        }

        //模式转换为静态电流电压模式
        public void ChangeToStaticCurrentVoltageMode(int ch)
        {
            //if (this._isSimulation) return;
            //首先判断是否需要连接
            if (this._isOnline == false) return;


            //最多2通道
            if (!(ch == 1 || ch == 2))
            {
                throw (new Exception("通道号异常"));
            }


            try
            {
                //关闭输出控制
                //0: Static OFF
                //1: Static ON
                //2: Live OFF/ON (See ID 50000)
                //3: HW Enable (Check GPIO Config)
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.OutputEnabled, (byte)ch, 0);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("关闭控制失败:" + ex));
            }

            try
            {
                _OutputStageMode[ch] = 0;
                //设定
                //0: Static Current/Voltage (Uses ID 2020…)
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.OutputStageMode, (byte)ch, 0);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("转换为静态电流电压模式失败:" + ex));
            }
        }

        //模式转换为温度控制器模式
        public void ChangeToTemperatureControllerMode(int ch)
        {
            //if (this._isSimulation) { return; }
            //首先判断是否需要连接
            if (this._isOnline == false) return;
 
            //最多2通道
            if (!(ch == 1 || ch == 2))
            {
                throw (new Exception("通道号异常"));
            }

            try
            {
                //启动温度控制
                //0: Static OFF
                //1: Static ON
                //2: Live OFF/ON (See ID 50000)
                //3: HW Enable (Check GPIO Config)
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.OutputEnabled, (byte)ch, 0);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("停止控制失败:" + ex));
            }


            try
            {
                _OutputStageMode[ch] = 2;
                //设定
                //2: Temperature Controller
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.OutputStageMode, (byte)ch, 2);
                }
          
            }
            catch (Exception ex)
            {
                throw (new Exception("转换为静态电流电压模式失败:" + ex));
            }

        }

        //关闭输出
        public void OutputOFF(int ch)
        {
            //if (this._isSimulation) { return; }
            //首先判断是否需要连接
            if (this._isOnline == false) return;
 
            //最多2通道
            if (!(ch == 1 || ch == 2))
            {
                throw (new Exception("通道号异常"));
            }

            try
            {
                //关闭输出控制
                //0: Static OFF
                //1: Static ON
                //2: Live OFF/ON (See ID 50000)
                //3: HW Enable (Check GPIO Config)
                lock (this._chassis)
                {
                    this._family.MeComBasicCmd.SetINT32Value(null, (ushort)eMeerstetterTECAddress.OutputEnabled, (byte)ch, 0);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("关闭控制失败:" + ex));
            }

        }

        //保存参数
        public void SavePara()
        {
            //if (this._isSimulation) { return; }
            //首先判断是否需要连接
            if (this._isOnline == false) return;

            lock (this._chassis)
            {
                this._family.MeComBasicCmd.ResetDevice(null); Thread.Sleep(2000);
            }
        }

        //重启
        public void ResetDevice()
        {
            //if (this._isSimulation) { return; }
            //首先判断是否需要连接
            if (this._isOnline == false) return;

            lock (this._chassis)
            {
                this._family.MeComBasicCmd.ResetDevice(null); Thread.Sleep(2000);
            }

        }
        #region function for pump coc burn-in
        public float GetSingleChannelTemperature(int ch)
        {
            //if (this._isSimulation) { return SIMU_VAL_FLOAT; }
            //首先判断是否需要连接
            if (this._isOnline == false) return OFFLINE_VAL_FLOAT;

            lock (this._chassis)
            {
                return this._family.MeComBasicCmd.GetFloatValue(null, (ushort)eMeerstetterTECAddress.GetTemperature, (byte)ch);
            }
        }

        public float[] GetAllChannelTemperature()
        {
            float[] retArr = new float[4];
            //if (this._isSimulation) { return retArr; }
            //首先判断是否需要连接
            if (this._isOnline == false) return retArr;
            lock (this._chassis)
            {
                for (int ch = 1; ch <= CHANNEL_COUNT; ch++)
                {
                    retArr[ch - 1] = this._family.MeComBasicCmd.GetFloatValue(null, (ushort)eMeerstetterTECAddress.GetTemperature, (byte)ch);
                    Thread.Sleep(50);
                }
            }
            return retArr;
        }
        public float GetSingleChannelTemperatureSetPoint(int ch)
        {
            //if (this._isSimulation) { return SIMU_VAL_FLOAT; }
            //首先判断是否需要连接
            if (this._isOnline == false) return OFFLINE_VAL_FLOAT; 
            lock (this._chassis)
            {
                return this._family.MeComBasicCmd.GetFloatValue(null, (ushort)eMeerstetterTECAddress.GetTemperatureSetPoint, (byte)ch);
            }
        }
        public float[] GetAllChannelTemperatureSetPoint()
        {
            float[] retArr = new float[4];
            //if (this._isSimulation) { return retArr; }
            //首先判断是否需要连接
            if (this._isOnline == false) return retArr;
            lock (this._chassis)
            {
                for (int ch = 1; ch <= CHANNEL_COUNT; ch++)
                {
                    retArr[ch - 1] = this._family.MeComBasicCmd.GetFloatValue(null, (ushort)eMeerstetterTECAddress.GetTemperatureSetPoint, (byte)ch);
                    Thread.Sleep(50);
                }
            }
            return retArr;
        }

        public void SetAllChannelTemperatureControlEnable(bool isEnable)
        {
            //if (this._isSimulation) return;
            if (this._isOnline == false) return;
            int en = isEnable ? 1 : 0;
            //复合函数 锁在下一层
            for (int ch = 1; ch <= CHANNEL_COUNT; ch++)
            {
                this.SetTargetObjectTemperatureEnable(ch, en); Thread.Sleep(50);
            }
        }
        public void SetAllChannelTemperature(double Value)
        {
            //if (this._isSimulation) return;
            if (this._isOnline == false) return;
            //复合函数 锁在下一层
            for (int ch = 1; ch <= CHANNEL_COUNT; ch++)
            {
                this.SetTargetObjectTemperature(ch, Value); Thread.Sleep(50);
            }
        }
        public bool IsSingleChannelTemperatureStable(int ch)
        {
            //if (this._isSimulation) return true;
            if (this._isOnline == false) return false;
            lock (this._chassis)
            {
                if (this._family.MeComBasicCmd.GetINT32Value(null, (ushort)eMeerstetterTECAddress.GetStableStatus, (byte)ch) == 2)
                { return true; }
                else
                { return false; }
            }
        }
        public bool IsAllChannelTemperatureStable()
        {
            //if (this._isSimulation) return true;
            if (this._isOnline == false) return false;
            bool isStable = true;
            //复合函数 锁在下一层
                for (int ch = 1; ch <= CHANNEL_COUNT; ch++)
                {
                    if (this.IsSingleChannelTemperatureStable(ch))
                    {
                        continue;
                    }
                    else
                    {
                        isStable = false;
                        break;
                    }
                }
          
            return isStable;
        }
        public void SetTempOffset(int ch, float offset)
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}