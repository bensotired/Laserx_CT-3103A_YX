namespace SolveWare_BurnInInstruments
{
    public enum TH2810D_SPEED
    {
        FAST,
        MEDium,
        SLOW
    }
    public enum TH2810D_DISPlay
    {
        DIRect,
        PERcent,
        ABSolute
    }
    public enum TH2810D_FREQ
    {
        Hz_100,
        Hz_120,
        Hz_1K,
        Hz_10K
    }
    public enum TH2810D_PARA
    {
        CD,
        RQ,
        ZQ,
        LQ
    }
    public enum TH2810D_LEV
    {
        V1,
        V2,
        V3
    }
    public enum TH2810D_SR
    {
        R_30,
        R_100
    }
    /// <summary>
    /// INTernal 设定为内部触发方式。
    /// EXTernal 设定为外部触发方式。
    /// IMMediate 立即触发一次测量。
    /// </summary>
    public enum TH2810D_TRIG
    {
        INTernal,
        EXTernal,
        IMMediate
    }
    /// <summary>
    /// OPEN 在当前测试电压和信号源内阻下对所有测试频率进行OPEN 清零
    /// OPEN_ALL 在当前信号源内阻下对所有测试电压和频率进行OPEN 清零
    /// SHORt 在当前测试电压和信号源内阻下对所有测试频率进行SHORt 清零
    /// SHORt_ALL 在当前信号源内阻下对所有测试电压和频率进行SHOTt 清零
    /// </summary>
    public enum TH2810D_CORR
    {
        OPEN,
        OPEN_ALL,
        SHORt,
        SHORt_ALL
    }
    public enum TH2810D_EQU
    {
        //串联等效电路
        SERial,
        //并联等效电路
        PARallel
    }
    public enum TH2810D_RANG
    {
        AUTO,
        HOLD,
        ZERO,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE
    }
    public enum TH2810D_ALAR
    {
        OFF,
        AUX,
        P3,
        P2,
        P1,
        NG
    }
}
