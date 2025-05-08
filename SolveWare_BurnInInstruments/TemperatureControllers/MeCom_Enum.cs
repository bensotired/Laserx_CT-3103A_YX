using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public enum PhysicalConnection
    {
        TCPIP,
        SerialPort
    }

    public enum eDeviceStatus
    {
        Init = 0,
        Ready = 1,
        Run = 2,
        Error = 3,
        Bootloader = 4,
        UnderReset = 5,
    }

    //Command for Static
    public enum eMeerstetterTECAddress : int
    {

        //=============使用项目 与1089兼容==============

        //0: Init
        //1: Ready   使用
        //2: Run     使用
        //3: Error   使用
        //4: Bootloader
        //5: Device will Reset within next 200ms
        GetDeviceStatus = 104,          //状态(只读)

        Save = 108,              //Save Data to Flash

        TemperatureSetPoint = 3000,  //设定和获取目标温度

        //0: Static OFF   使用
        //1: Static ON    使用
        //2: Live OFF/ON (See ID 50000)
        //3: HW Enable (Check GPIO Config)
        OutputEnabled = 2010,

        GetTemperature = 1000,          //获取目标温度(只读)
        GetResistance = 1042,          //获取热敏电阻(只读)

        GetTECCurrent = 1020,           //电流(只读)
        GetTECVoltage = 1021,           //电压(只读

        //0: Temperature regulation is not active
        //1: Is not stable
        //2: Is stable       使用
        GetStableStatus = 1200,

        CurrentProtectionMaxLimit = 2030,  //+limit
        VoltageProtectionMaxLimit = 2031,  //+limit

        CurrentProtectionMinLimit = 2032,  //-limit
        VoltageProtectionMinLimit = 2033,  //-limit

        PIDGain = 3010,         //写入
        PIDIntegral = 3011,
        PIDDerivative = 3012,

        PID_P = 51014,         //获取
        PID_I = 51015,
        PID_D = 51016,

        //温度上下限
        TempUpperErr = 4011,
        TempLowerErr = 4012,

        //NTC参数
        NTCTempU = 5024,
        NTCRU = 5025,
        NTCTempM = 5022,
        NTCRM = 5023,
        NTCTempL = 5020,
        NTCRL = 5021,

        //温度校准系数 K,B
        TempGain = 4002,
        TempOffset = 4001,
 
        //Tuning PID
        AutoTuneStart = 51000,
        AutoTuneProcess = 51021,

        //温度达到目标的条件
        TemperatureDeviation = 4040,  //温度平滑范围
        MinTimeInWindow = 4041,       //温度平滑稳定的窗口时间
         
        //未使用
        GetDeviceType = 100,
        GetSerialNumber = 102,
        GetTemperatureSetPoint = 1010,  //获取设定的目标温度(只读) 
        GetErrorNumber = 105, //错误编码
        TemperatureSourceMode = 50011,
        DeviceAddress = 2051, //TEC设备地址

        //控制板运行模式
        OutputStageMode = 2000,
        //0: Static Current/Voltage (Uses ID 2020…)
        //1: Live Current/Voltage (Uses ID 50001…)
        //2: Temperature Controller

        //静态电流电压模式
        //0: Static Current/Voltage (Uses ID 2020…)
        StaticModeCurrent = 2020, //静态模式电流
        StaticModeVoltage = 2021, //静态模式电压

        //喂看门狗
        FeedDod = 52005,
        EnableDog = 52000,


        #region 2022.04.24新增 BT-2700A用

        //原 Temperature => VoltageCtl
        //原 Temp => VolCtl
        //温度 => 控制电压

        //设定和获取目标控制电压
        VoltageCtlSetPoint = 3000,

        //控制电压上下限
        VolCtlUpperErr = 4011,
        VolCtlLowerErr = 4012,

        //电压校准系数 (是否需要?）
        VolCtlGain = 4002,
        VolCtlOffset = 4001,

        //电压达到目标的条件 (是否需要?)
        VoltageCtlDeviation = 4040,  //电压平滑范围
        VolCtlMinTimeInWindow = 4041,//电压平滑稳定的窗口时间

        GetVoltageCtl = 1000,  //获取目标控制电压(只读)

        GetVoltageCtlSetPoint = 1010,  //获取设定的目标控制电压(只读)

        #endregion

        /// <summary>
        /// Object Temperature Stability Indicator Settings
        /// Temperature Deviation
        /// </summary>

    }
}