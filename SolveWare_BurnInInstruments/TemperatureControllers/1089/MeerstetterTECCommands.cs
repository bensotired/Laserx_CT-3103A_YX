namespace SolveWare_BurnInInstruments
{
    public enum MeerstetterTECSourceFunction : int
    {
        StaticMode = 0,
        LiveMode = 1,
        TemperatureControllerMode = 2
    }
    public enum DeviceStatus
    {
        Init = 0,
        Ready = 1,
        Run = 2,
        Error = 3,
        Bootloader = 4,
        UnderReset = 5,
    }

    //Command for Static
    public enum MeerstetterTECCommands : int
    {
        GetDeviceType = 100,
        GetSerialNumber = 102,
        DeviceStatus = 104,
        OutputEnabled = 2010,
        SetTemperatureSetPoint = 3000,
        GetTemperatureSetPoint = 1010,

        GetTemperature = 1000,
        GetSinkTemperature = 1044,
        GetTECCurrent = 1020,
        GetTECVoltage = 1021,
        GetDeviceStatus = 104,
        Mode = 2000,
        GetStableStatus = 1200,
        CurrentProtectionLimit = 2030,
        VoltageProtectionLimit = 2031,
        PIDGain = 3010,
        PIDIntegral = 3011,
        PIDDerivative = 3012,
        TemperatureSourceMode = 50011,

        /// <summary>
        /// Object Temperature Stability Indicator Settings
        /// Temperature Deviation
        /// </summary>
        TemperatureDeviation = 4040,
        MinTimeInWindow = 4041,
        Obj_Upper_Point_Temperature = 4024,
        Obj_Upper_Point_Resistance = 4025,
        Obj_Middle_Point_Temperature = 4022,
        Obj_Middle_Point_Resistance = 4023,
        Obj_Lower_Point_Temperature = 4020,
        Obj_Lower_Point_Resistance = 4021,

    }
}
