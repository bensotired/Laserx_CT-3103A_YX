using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class LX3012H : InstrumentBase, IInstrumentBase
    {
        const int Delay_ms = 50;
        const string eorr = "Instrument not connected";
        public LX3012H(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
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
        /// 重置配置
        /// </summary>
        public void RST()
        {
            try
            {
                if (this._isOnline == false)
                {
                    return;
                }
                string response = this._chassis.Query($"*RST\r\n", Delay_ms);
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 输出电压
        /// </summary>
        /// <param name="slot">槽位（1-20）</param>
        /// <param name="channel">通道（1-99）</param>
        /// <param name="ProtectionVoltage_V">保护电压</param>
        /// <param name="VoltageRang_V">电压量程</param>
        /// <param name="CurrentRang_A">电流量程</param>
        /// <param name="ClampingCurrent_A">钳制电流</param>
        /// <param name="OutputVolt_V">输出电压</param>
        /// <returns></returns>
        public string OutputVoltage(int slot, int channel,double ProtectionVoltage_V,double VoltageRang_V,
            double CurrentRang_A,double ClampingCurrent_A,double OutputVolt_V)
        {
            if (this._isOnline == false)
            {
                return eorr;
            }
            if (slot > 20 || slot < 0)
            {
                return $"Parameter [slot] error : {slot}";
            }
            if (channel > 99)
            {
                return $"Parameter [channel] error : {channel}";
            }
            string _channel = string.Empty;
            if (channel < 10)
            {
                _channel = "0" + channel.ToString();
            }
            else
            {
                _channel = channel.ToString();
            }

            this._chassis.Query($"*RST\r\n", Delay_ms);//重置配置
            this._chassis.Query($":SOUR:FUNC:MODE VOLT,(@{slot}{_channel})\r\n", Delay_ms);//选择电压源模式
            this._chassis.Query($":SOUR:VOLT:MODE FIX,(@{slot}{_channel})\r\n", Delay_ms);//选择电压源固定输出模式
            this._chassis.Query($":SOUR:VOLT:PROT {ProtectionVoltage_V},(@{slot}{_channel})\r\n", Delay_ms);//保护电压
            this._chassis.Query($":SOUR:VOLT:RANG {VoltageRang_V},(@{slot}{_channel})\r\n", Delay_ms);//电压量程
            this._chassis.Query($":SOUR:CURR:RANG {CurrentRang_A},(@{slot}{_channel})\r\n", Delay_ms);//电流量程
            this._chassis.Query($":SOUR:CURR:PROT {ClampingCurrent_A},(@{slot}{_channel})\r\n", Delay_ms);//钳制电流
            this._chassis.Query($":SOUR:VOLT {OutputVolt_V},(@{slot}{_channel})\r\n", Delay_ms);//输出电压
            this._chassis.Query($":OUTP 1,(@{slot}{_channel})\r\n", Delay_ms);
            string response = this._chassis.Query($":READ?\r\n", Delay_ms);

            return response;
        }

        /// <summary>
        /// 输出电流
        /// </summary>
        /// <param name="slot">槽位（1-20）</param>
        /// <param name="channel">通道（1-99）</param>
        /// <param name="ProtectionVoltage_V">保护电压</param>
        /// <param name="VoltageRang_V">电压量程</param>
        /// <param name="CurrentRang_A">电流量程</param>
        /// <param name="ClampingVoltage_V">钳制电压</param>
        /// <param name="OutputCurrent_A">输出电流</param>
        /// <returns></returns>
        public string OutputCurrent(int slot, int channel, double ProtectionVoltage_V, double VoltageRang_V,
            double CurrentRang_A, double ClampingVoltage_V, double OutputCurrent_A)
        {
            if (this._isOnline == false)
            {
                return eorr;
            }
            if (slot > 20 || slot < 0)
            {
                return $"Parameter [slot] error : {slot}";
            }
            if (channel > 99)
            {
                return $"Parameter [channel] error : {channel}";
            }
            string _channel = string.Empty;
            if (channel < 10)
            {
                _channel = "0" + channel.ToString();
            }
            else
            {
                _channel = channel.ToString();
            }

            this._chassis.Query($"*RST\r\n", Delay_ms);//重置配置
            this._chassis.Query($":SOUR:FUNC:MODE CURR,(@{slot}{_channel})\r\n", Delay_ms);//选择电流源模式
            this._chassis.Query($":SOUR:CURR:MODE FIX,(@{slot}{_channel})\r\n", Delay_ms);//选择电流源固定输出模式
            this._chassis.Query($":SOUR:VOLT:PROT {ProtectionVoltage_V},(@{slot}{_channel})\r\n", Delay_ms);//保护电压
            this._chassis.Query($":SOUR:VOLT:RANG {VoltageRang_V},(@{slot}{_channel})\r\n", Delay_ms);//电压量程
            this._chassis.Query($":SOUR:CURR:RANG {CurrentRang_A},(@{slot}{_channel})\r\n", Delay_ms);//电流量程
            this._chassis.Query($":SENS:VOLT:PROT {ClampingVoltage_V},(@{slot}{_channel})\r\n", Delay_ms);//钳制电压
            this._chassis.Query($":SOUR:CURR {OutputCurrent_A},(@{slot}{_channel})\r\n", Delay_ms);//输出电流
            this._chassis.Query($":OUTP 1,(@{slot}{_channel})\r\n", Delay_ms);
            string response = this._chassis.Query($":READ?\r\n", Delay_ms);

            return response;
        }


        
        /// <summary>
        /// 测量电压
        /// </summary>
        /// <param name="slot">槽位（1-20）</param>
        /// <param name="channel">通道（1-99）</param>
        /// <param name="ProtectionVoltage_V">保护电压</param>
        /// <param name="VoltageRang_V">电压量程</param>
        /// <param name="CurrentRang_A">电流量程</param>
        /// <param name="ClampingVoltage_V">钳制电压</param>
        /// <param name="OutputCurrent_A">输出电流</param>
        /// <returns></returns>
        public string MeasurVoltage(int slot, int channel, double ProtectionVoltage_V, double VoltageRang_V,
           double CurrentRang_A, double ClampingVoltage_V, double OutputCurrent_A)
        {
            if (this._isOnline == false)
            {
                return eorr;
            }
            if (slot > 20 || slot < 0)
            {
                return $"Parameter [slot] error : {slot}";
            }
            if (channel > 99)
            {
                return $"Parameter [channel] error : {channel}";
            }
            string _channel = string.Empty;
            if (channel < 10)
            {
                _channel = "0" + channel.ToString();
            }
            else
            {
                _channel = channel.ToString();
            }

            this._chassis.Query($"*RST\r\n", Delay_ms);//重置配置
            this._chassis.Query($":SOUR:FUNC:MODE CURR,(@{slot}{_channel})\r\n", Delay_ms);//选择电流源模式
            this._chassis.Query($":SOUR:CURR:MODE FIX,(@{slot}{_channel})\r\n", Delay_ms);//选择电流源固定输出模式
            this._chassis.Query($":SOUR:VOLT:PROT {ProtectionVoltage_V},(@{slot}{_channel})\r\n", Delay_ms);//保护电压
            this._chassis.Query($":SOUR:VOLT:RANG {VoltageRang_V},(@{slot}{_channel})\r\n", Delay_ms);//电压量程
            this._chassis.Query($":SOUR:CURR:RANG {CurrentRang_A},(@{slot}{_channel})\r\n", Delay_ms);//电流量程
            this._chassis.Query($":SENS:VOLT:PROT {ClampingVoltage_V},(@{slot}{_channel})\r\n", Delay_ms);//钳制电压
            this._chassis.Query($":SOUR:CURR {OutputCurrent_A},(@{slot}{_channel})\r\n", Delay_ms);//输出电流
            this._chassis.Query($":OUTP 1,(@{slot}{_channel})\r\n", Delay_ms);
            string response = this._chassis.Query($":READ:VOLT?\r\n", Delay_ms);

            return response;
        }

        /// <summary>
        /// 测量电流
        /// </summary>
        /// <param name="slot">槽位（1-20）</param>
        /// <param name="channel">通道（1-99）</param>
        /// <param name="ProtectionVoltage_V">保护电压</param>
        /// <param name="VoltageRang_V">电压量程</param>
        /// <param name="CurrentRang_A">电流量程</param>
        /// <param name="ClampingCurrent_A">钳制电流</param>
        /// <param name="OutputVolt_V">输出电压</param>
        /// <returns></returns>
        public string MeasurCurrent(int slot, int channel, double ProtectionVoltage_V, double VoltageRang_V,
        double CurrentRang_A, double ClampingCurrent_A, double OutputVolt_V)
        {
            if (this._isOnline == false)
            {
                return eorr;
            }
            if (slot > 20 || slot < 0)
            {
                return $"Parameter [slot] error : {slot}";
            }
            if (channel > 99)
            {
                return $"Parameter [channel] error : {channel}";
            }
            string _channel = string.Empty;
            if (channel < 10)
            {
                _channel = "0" + channel.ToString();
            }
            else
            {
                _channel = channel.ToString();
            }

            this._chassis.Query($"*RST\r\n", Delay_ms);//重置配置
            this._chassis.Query($":SOUR:FUNC:MODE VOLT,(@{slot}{_channel})\r\n", Delay_ms);//选择电压源模式
            this._chassis.Query($":SOUR:VOLT:MODE FIX,(@{slot}{_channel})\r\n", Delay_ms);//选择电压源固定输出模式
            this._chassis.Query($":SOUR:VOLT:PROT {ProtectionVoltage_V},(@{slot}{_channel})\r\n", Delay_ms);//保护电压
            this._chassis.Query($":SOUR:VOLT:RANG {VoltageRang_V},(@{slot}{_channel})\r\n", Delay_ms);//电压量程
            this._chassis.Query($":SOUR:CURR:RANG {CurrentRang_A},(@{slot}{_channel})\r\n", Delay_ms);//电流量程
            this._chassis.Query($":SOUR:CURR:PROT {ClampingCurrent_A},(@{slot}{_channel})\r\n", Delay_ms);//钳制电流
            this._chassis.Query($":SOUR:VOLT {OutputVolt_V},(@{slot}{_channel})\r\n", Delay_ms);//输出电压
            this._chassis.Query($":OUTP 1,(@{slot}{_channel})\r\n", Delay_ms);
            string response = this._chassis.Query($":READ:CURR?\r\n", Delay_ms);

            return response;
        }


    }
    public enum Mode
    {
        VOLT,
        CURR,
    }
}
