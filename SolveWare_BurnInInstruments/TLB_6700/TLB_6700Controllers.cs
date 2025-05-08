using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class TLB_6700Controllers : InstrumentBase, IInstrumentBase
    {
        const int Delay_ms = 50;
        string Queryfailure = "Query failure";

        public TLB_6700Controllers(string name, string address, IInstrumentChassis chassis)
        : base(name, address, chassis)
        {
        }

        /// <summary>
        /// 标识字符串查询
        /// </summary>
        public string IND
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("*IDN? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 此命令将仪器恢复到保存在仪器的非易失性闪存中的设置状态
        /// 0  恢复出厂设置
        /// 1 ~ 5 恢复保存在指定bin中的设置
        /// *RST, *SAV
        /// </summary>
        public void RCL(int value)
        {
            if (this._isOnline == false)
            {
                return;
            }
            this._chassis.TryWrite($"*RCL {value}\r\n");
        }
        /// <summary>
        /// 该命令对仪器进行软复位
        /// </summary>
        public void RST()
        {
            if (this._isOnline == false)
            {
                return;
            }
            this._chassis.TryWrite("*RST\r\n");
        }
        /// <summary>
        /// 2 ~ 5  保存当前设置到指定bin
        /// 该命令将仪器的当前状态存储在非易失性闪存中。
        /// 然后使用*RCL命令回调该状态。请参阅*RCL命令的描述，
        /// 以获取仪器保存/召回的值列表。
        /// </summary>
        public void SAV(int value)
        {
            if (this._isOnline == false)
            {
                return;
            }
            this._chassis.TryWrite($"*SAV {value}\r\n");
        }
        /// <summary>
        /// 操作完成查询。
        /// 该激光器具有波长扫描、波长扫描复位和波长设置三种长期操作。
        /// 启动这三个操作中的任何一个都会清除OPC状态位。
        /// 当激光到达目标波长时，OPC状态位置为1。
        /// 如果通过停止/重置扫描过程或关闭波长跟踪模式来中断运动，
        /// 则该位也设置为1。此查询命令返回OPC状态位。
        /// </summary>
        public string OPC
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("*OPC? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 状态字节查询
        /// </summary>
        public string STB
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("*STB? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 蜂鸣器
        /// </summary>
        public string BEEP
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"BEEP {value}\r\n");
            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("BEEP? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 显示器的亮度
        /// 1~100
        /// </summary>
        public string BRIGHT
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"BRIGHT {value}\r\n");
            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("BRIGHT? \r\n", Delay_ms);
                return response;
            }

        }
        /// <summary>
        /// Error string query.
        /// </summary>
        public string ERRSTR
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("ERRSTR? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 设置硬件配置寄存器
        /// </summary>
        public string HWCONFIG
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"HWCONFIG {value}\r\n");
            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("HWCONFIG? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 仪表前面板锁定状态查询
        /// 0 启用前面板按钮和旋钮
        /// 1 前面板按钮和旋钮失效
        /// 2 前面板旋钮关闭
        /// </summary>
        public string LOCKOUT
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"LOCKOUT {value}\r\n");
            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("LOCKOUT? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 激光开启延迟命令。时间，以毫秒计。
        /// </summary>
        public string ONDELAY
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"ONDELAY {value}\r\n");
            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("ONDELAY? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 激光输出使能
        /// 0 或 OFF 关闭激光器
        /// 1 或 ON 开启激光
        /// </summary>
        public string OUTPut_STATe
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"OUTPut:STATe {value}\r\n");
            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("OUTPut:STATe? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 波长跟踪模式
        /// 0 或 OFF 表示关闭Track模式 
        /// 1 或 ON 表示开启Track模式
        /// </summary>
        public string OUTPut_TRACk
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"OUTPut:TRACk {value}\r\n");
            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("OUTPut:TRACk? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 启动/关闭波长扫描
        /// </summary>
        public bool OUTPut_SCAN
        {
            set
            {
                if (value == true)
                {
                    if (this._isOnline == false)
                    {
                        return;
                    }
                    this._chassis.TryWrite($"OUTPut:SCAN:START\r\n");
                }
                else
                {
                    if (this._isOnline == false)
                    {
                        return;
                    }
                    this._chassis.TryWrite($"OUTPut:SCAN:STOP\r\n");
                }

            }
        }
        /// <summary>
        /// 重置波长扫描
        /// </summary>
        public void OUTPut_SCAN_RESET()
        {
            if (this._isOnline == false)
            {
                return;
            }
            this._chassis.TryWrite($"OUTPut:SCAN:RESET\r\n");

        }
        /// <summary>
        /// 实际激光二极管电流查询(mA)
        /// </summary>
        public string SENSe_CURRent_DIODe
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:CURRent:DIODe \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 实际激光二极管功率查询(mW)
        /// </summary>
        public string SENSe_POWer_DIODe
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:POWer:DIODe \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 实际激光二极管增益元件温度查询(℃)
        /// </summary>
        public string SENSe_TEMPerature_DIODe
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:TEMPerature:DIODe \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 实际激光腔温查询(℃)
        /// </summary>
        public string SENSe_TEMPerature_CAVity
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:TEMPerature:CAVity \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 实际辅助探测器输入电压查询(V)
        /// </summary>
        public string SENSe_VOLTage_AUXiliary
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:VOLTage:AUXiliary \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 实际压电电压查询(V)
        /// </summary>
        public string SENSe_VOLTage_PIEZo
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:VOLTage:PIEZo \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 实际波长输入电压查询(V)
        /// </summary>
        public string SENSe_VOLTage_WAVEIN
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:VOLTage:WAVEIN \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 实际激光波长查询(nm)
        /// </summary>
        public string SENSe_WAVElength
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SENSe:WAVElength \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 恒功率模式设置
        /// OFF 或 0 输入“OFF”或“0” 表示将控制器设置为恒流模式。
        /// ON 或 1 当取值为“ON”或“1”时，将控制器设置为恒功率模式。
        /// </summary>
        public string SOURce_CPOWer
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:CPOWer {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:CPOWer? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 二极管激光电流设置_(mA)
        /// 如果字符串“MAX”被指定为值，则二极管电流设定值被设置为二极管的额定电流
        /// </summary>
        public string SOURce_CURRent_DIODe
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:CURRent:DIODe {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:CURRent:DIODe? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 二极管激光输出功率设置。(mW)
        /// 如果字符串“MAX”被指定为值，则二极管功率设定值被设置为二极管的额定功率
        /// </summary>
        public string SOURce_POWer_DIODe
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:POWer:DIODe {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:POWer:DIODe? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 压电换能器电压设置。
        /// 如果将“MAX”字符串指定为值，则压电电压设定值设置为100%
        /// </summary>
        public string SOURce_VOLTage_PIEZo
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:VOLTage:PIEZo {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:VOLTage:PIEZo? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 压电换能器电压增益设置。
        /// HIGH 或 1   如果指定字符串“HIGH”或“1”作为值，则压电增益设置为20倍
        /// LOW 或 0 如果指定字符串“LOW”或“0”作为值，则压电增益设置为1x
        /// </summary>
        public string SOURce_VOLTage_PZGAIN
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:VOLTage:PZGAIN {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:VOLTage:PZGAIN? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 激光二极管波长设置(nm)
        /// MAX  如果字符串“MAX”被指定为值，则二极管波长设定值被设置为二极管的最大值。
        /// MIN  如果字符串“MIN”指定为值，则二极管波长设定值设置为二极管的最小额定值。
        /// </summary>
        public string SOURce_WAVElength
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:WAVElength {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:WAVElength? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 设置所需的扫描数。
        /// 1~9999
        /// </summary>
        public int SOURce_WAVE_DESSCANS
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:WAVE:DESSCANS {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return 0;
                }
                string response = this._chassis.Query("SOURce:WAVE:DESSCANS? \r\n", Delay_ms);
                return int.Parse(response);
            }
        }
        /// <summary>
        /// 已完成查询的实际扫描次数
        /// </summary>
        public string SOURce_WAVE_ACTSCANS
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:WAVE:ACTSCANS? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 扫描配置集
        /// 0 or 1
        /// </summary>
        public int SOURce_WAVE_SCANCFG
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:WAVE:SCANCFG {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return -1;
                }
                string response = this._chassis.Query("SOURce:WAVE:SCANCFG? \r\n", Delay_ms);
                return int.Parse(response);
            }
        }
        /// <summary>
        /// 激光二极管扫描起始波长(nm)
        /// 如果将字符串“MAX”指定为值，则扫描起始波长设置为二极管的最大波长。
        /// 如果将字符串“MIN”指定为值，则扫描起始波长设置为二极管的最小波长。
        /// </summary>
        public string SOURce_WAVE_START
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:WAVE:START {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:WAVE:START? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 激光二极管扫描起始波长(nm)
        /// 如果将字符串“MAX”指定为值，则扫描停止波长设置为二极管的最大波长。
        /// 如果将字符串“MIN”指定为值，则扫描停止波长设置为二极管的最小波长。
        /// </summary>
        public string SOURce_WAVE_STOP
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:WAVE:STOP {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:WAVE:STOP? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 波长扫描正向速度设置
        /// 如果将字符串“MAX”指定为值，则波长扫描正向速度设置为二极管的最大值
        /// </summary>
        public string SOURce_WAVE_SLEW_FORWard
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:WAVE:SLEW:FORWard {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:WAVE:SLEW:FORWard? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 波长扫描返回速度设置(nm/s)
        /// 如果将字符串“MAX”指定为值，则波长扫描返回速度设置为二极管的最大值。
        /// </summary>
        public string SOURce_WAVE_SLEW_RETurn
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOURce:WAVE:SLEW:RETurn {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:WAVE:SLEW:RETurn? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 二极管温度设定值查询(℃)
        /// </summary>
        public string SOURce_TEMPerature_DIODe
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:TEMPerature:DIODe? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 空腔温度设定值查询(℃)
        /// </summary>
        public string SOURce_TEMPerature_CAVity
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:TEMPerature:CAVity? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 激光二极管扫描最大速度查询(nm/sec)
        /// </summary>
        public string SOURce_WAVE_MAXVEL
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SOURce:WAVE:MAXVEL? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 激光使能时间查询(min)
        /// </summary>
        public string SYSTem_ENTIME
        {
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SYSTem:ENTIME? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 设置控制器为远程/本地模式
        /// REM 将控制器设置为远程模式
        /// LOC 将控制器设置为本地模式
        /// </summary>
        public string SYSTem_MCONtrol
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SYSTem:MCONtrol {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SYSTem:MCONtrol? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 波长输入模式
        /// 0 表示输入模式关闭
        /// 1     打开输入模式
        /// </summary>
        public string SYSTem_WINPut
        {
            set
            {
                if (this._isOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SYSTem:WINPut {value}\r\n");

            }
            get
            {
                if (this._isOnline == false)
                {
                    return Queryfailure;
                }
                string response = this._chassis.Query("SYSTem:WINPut? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 激光头型号查询
        /// </summary>
        /// <returns></returns>
        public string GetMODEL()
        {
            if (this._isOnline == false)
            {
                return Queryfailure;
            }
            string response = this._chassis.Query("SYSTEM:LASER:MODEL? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 激光头序列号查询
        /// </summary>
        /// <returns></returns>
        public string GetSN()
        {
            if (this._isOnline == false)
            {
                return Queryfailure;
            }
            string response = this._chassis.Query("SYSTEM:LASER:SN? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 激光头修订号查询
        /// </summary>
        /// <returns></returns>
        public string GetREV()
        {
            if (this._isOnline == false)
            {
                return Queryfailure;
            }
            string response = this._chassis.Query("SYSTEM:LASER:REV? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 激光头校准日期查询
        /// </summary>
        /// <returns></returns>
        public string GetCALDATE()
        {
            if (this._isOnline == false)
            {
                return Queryfailure;
            }
            string response = this._chassis.Query("SYSTEM:LASER:CALDATE? \r\n", Delay_ms);
            return response;
        }
























        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }
    }
}
