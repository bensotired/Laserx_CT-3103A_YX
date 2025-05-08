namespace SolveWare_BurnInInstruments
{
    public enum SweepDataType
    {
        LD_Drive_Current_mA = 0x01,
        LD_Drive_Voltage_V = 0x02,
        EA_Drive_Current_mA = 0x03,
        EA_Drive_Voltage_V = 0x04,
        PD_Ch1_Current_mA = 0x05,
        PD_Ch2_Current_mA = 0x06,
        PD_Ch3_Current_mA = 0x07,
        PD_Ch4_Current_mA = 0x08,

        LD_Sense_Current_mA = 0x09,

        APD_Voltage_V = 0x03,
        APD_Current_mA = 0x83,
    }
    public enum SweepData
    {
        Sense_Current_mA,
        Sense_Voltage_V,
    }
}
