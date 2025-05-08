using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class GDM : InstrumentBase, IInstrumentBase
    {
        const int Delay_ms = 50;
        const string eorr = "Instrument not connected";
        public GDM(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
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
        /// <summary>
        /// 在主显示屏上设定 DC 电压 测量 并且指定量程
        /// </summary>
        /// <param name="volt"></param>
        public void VOLTage_V(int volt)
        {
            try
            {
                if (this._isOnline == false)
                {
                    return;
                }
                string response = this._chassis.Query($"CONF:VOLT:DC {volt} \r\n", Delay_ms);
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 在主显示屏上设定DC 电流 测量 并且指定量程
        /// </summary>
        /// <param name="curr"></param>
        public void CURRent_mA(int curr)
        {
            try
            {
                if (this._isOnline == false)
                {
                    return;
                }
                string response = this._chassis.Query($"CONF:CURR:DC {curr}e-3 \r\n", Delay_ms);
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 返回主显示屏的当前测试量程
        /// </summary>
        /// <returns></returns>
        public string RANGe()
        {
            try
            {
                if (this._isOnline == false)
                {
                    return eorr;
                }

                string response = this._chassis.Query($"CONFigure:RANGe? \r\n", Delay_ms);
                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public bool AutoRange
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                string par = string.Empty;
                if (value)
                {
                    par = "ON";
                }
                else
                {
                    par = "OFF";
                }


                string response = this._chassis.Query($"CONFigure:AUTO {par} \r\n", Delay_ms);
            }
            get
            {
                if (this._isOnline == false)
                {
                    return false;
                }

                string response = this._chassis.Query($"CONFigure:AUTO? \r\n", Delay_ms);

                response = response.Trim();
                if (response == "1")
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 返回主显示屏上 DC 电压
        /// </summary>
        /// <returns></returns>
        public string VOLTage_mV()
        {
            if (this._isOnline == false)
            {
                return eorr;
            }


            string response = this._chassis.Query($"MEAS:VOLT:DC? \r\n", Delay_ms);

            return response.Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CURRent_mA()
        {
            if (this._isOnline == false)
            {
                return eorr;
            }


            string response = this._chassis.Query($"MEAS:CURR:DC? \r\n", Delay_ms);

            return response.Trim();
        }
        /// <summary>
        /// 清除所有指令状态
        /// </summary>
        public void CLS()
        {
            if (this._isOnline == false)
            {
                return;
            }
            string response = this._chassis.Query($"*CLS \r\n", Delay_ms);
        }
        /// <summary>
        /// 返回生产厂商版本号 序列号 和系统版本号
        /// </summary>
        /// <returns></returns>
        public string IDN()
        {
            if (this._isOnline == false)
            {
                return eorr;
            }


            string response = this._chassis.Query($"*IDN? \r\n", Delay_ms);

            return response.Trim();
        }

        /// <summary>
        /// 当操作完成后，将“1” 放置在输出队列中
        /// </summary>
        public void OPC()
        {
            if (this._isOnline == false)
            {
                return;
            }
            string response = this._chassis.Query($"*OPC? \r\n", Delay_ms);
        }
    }
}
