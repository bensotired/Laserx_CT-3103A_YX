using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Motion
{
    public enum CustomHomeType
    {
        HomeOnNEL,
        HomeOnPEL,
        HomeMidPoint
    }
    public enum MotorMasterDriver
    {
        ACS,
        LeadSide_DMC,
        LeadSide_SMC,
        YangKong,
        GuGao
    }
    public enum LimitConfig
    {
        正限位 = 0,
        負限位 = 1,
        原點 = 2
    }
    public enum MotionStatus
    {
        MT_DONE,
        MT_ESTOP,
        MT_LIMIT,
        MT_ALARM,
        MT_CRITICAL,
        MT_HANGED,
        MT_TIMEOUT,
        MT_INHIBIT_TO_HOME,
        MT_INHIBIT_TO_MOVE,
        MT_HOMING_ERROR,
        MT_MOVING,
        MT_ENCODER_CLOSE_LOOP_FAILED
    }
    public enum SpeedType
    {
        Home,
        Auto,
        Jog
    }
    public enum SpeedLevel
    {
        High,
        Normal,
        Low
    }

    public enum IOSTATUS_DMC3600
    {
        ALM = 1,
        PEL = 2,
        NEL = 4,
        EMG = 8,
        ORG = 16,
        PSL = 64,
        MSL = 128,
        INP = 256,
        EZ = 512,
        RDY = 1024,
        DSTP = 2048
    }
    public enum SlowDownType
    {
        HalfWay,
        AllTheWay
    }

    public delegate bool BrakeDel(bool set);
    public delegate bool MotionDel();
    public delegate bool MotionDel2(ref string sErr);
    public delegate bool MotionInterlockDel(double currentPos, double newPos);
    public enum AXIS_STATUS_GUGAO : int
    {
        /// <summary>
        /// 伺服报警
        /// </summary>
        FlagAlarm = 0x2,
        /// <summary>
        /// 跟随误差越限标志 运动出错
        /// </summary>
        FlagMotionError = 0x10,
        /// <summary>
        /// 正限位触发
        /// </summary>
        FlagPosLimit = 0x20,
        /// <summary>
        /// 负限位触发
        /// </summary>
        FlagNegLimit = 0x40,
        /// <summary>
        /// 平滑停止触发
        /// </summary>
        FlagSmoothStop = 0x80,
        /// <summary>
        /// 急停触发
        /// </summary>
        FlagAbruptStop = 0x100,
        /// <summary>
        /// 伺服使能
        /// </summary>
        FlagServoOn = 0x200,
        /// <summary>
        /// 正在运动状态
        /// </summary>
        FlagMotion = 0x400,
        /// <summary>
        /// 位置到位
        /// </summary>
        FlagInPos = 0x800,
    }

    public enum RUNMODE_GUGAO : int
    {
        /// <summary>
        /// Profile position
        /// </summary>
        PROF_POS = 1,
        /// <summary>
        /// Profile velocity 
        /// </summary>
        PROF_VELO = 3,
        /// <summary>
        /// Profile torque
        /// </summary>
        PROF_TORQ = 4,
        /// <summary>
        /// 回零模式
        /// </summary>
        HOME = 6,
        /// <summary> 
        /// Cyclic sync position 
        /// 周期同步位置模式
        /// </summary>
        MOTION = 8,
        /// <summary>
        /// Cyclic sync velocity 
        /// </summary>
        VELO = 9,
        /// <summary>
        /// Cyclic sync torque
        /// </summary>
        TORQ = 10,
    }
    public enum AXIS_STATUS_LEADSHINE : uint
    {
        /// <summary>
        /// 位号0   描述:1：表示伺服报警信号 ALM 为 ON 0 OFF  二进制1001位号是3210  1*Math.Pow(2,3)+1**Math.Pow(2,0)
        /// </summary>
        FlagAlarm = 0x01,
        /// <summary>
        /// 位号1   描述:1：表示正 硬 限位信号 +EL 为 ON 0 OFF
        /// </summary>
        FlagPEL = 0x02,
        /// <summary>
        /// 位号2   描述:1：表示 负硬 限位 信号 EL 为 ON 0 OFF
        /// </summary>
        FlagNEL = 0x04,
        /// <summary>
        ///位号3   描述:1：：表示 急停 信号 EMG 为 ON 0 OFF
        /// </summary>
        FlagEMG = 0x08,
        /// <summary>
        /// 位号4   描述:1：表示原点信号 ORG 为 ON 0 OFF
        /// </summary>
        FlagORG = 0x10,
        /// <summary>
        /// 位号6   描述:1：表示正软限位信号 +SL 为 ON 0 OFF
        /// </summary>
        FlagPSL = 0x40,
        /// <summary>
        /// 位号7   描述:1：表示负软件限位信号-SL 为 ON 0 OFF
        /// </summary>
        FlagNSL = 0x80,
        /// <summary>
        /// 位号8   描述:1：表示伺服到位信号 INP 为 ON 0 OFF
        /// </summary>
        FlagINP = 0x100,
        /// <summary>
        /// 位号9   描述:1：表示 EZ 信号为 ON 0 OFF
        /// </summary>
        FlagEZ = 0x200,
        /// <summary>
        /// 位号10   描述:1：表示伺服准备信号 RDY 为 ON 0 OFF
        /// </summary>
        FlagRDY = 0x400,
        /// <summary>
        /// 位号11   描述:1：表示减速停止信号 DSTP 为 ON 0 OFF
        /// </summary>
        FlagDSTP = 0x800,
    }

    public enum STOP_MODE_LEADSHINE : ushort
    {
        DeccStop = 0,//减速停止
        ImmeStop = 1//立刻停止
    }

    public enum Sevon_Pin : int
    {
        LowLevel = 0,//减速停止
        HighLevel = 1//立刻停止
    }
    public enum MANUAL_HOMEMODE_LEADSHINE : int
    {
        //根据大佬的指示 因为没有原点的电平定义  可以自己定义新的模式
        //Negative：负方向   Positive  正方向
        //Low  低电平    High  高电平
        //Homemode0：一 次回零  1一次回零加回找  2二次回零
        //mode1 方向负方向  原点电平为0 复位方式是0  报警电平  正负限位电平
        //mode1 方向负方向  原点电平为0 复位方式是1  
        //mode1 方向负方向  原点电平为0 复位方式是2  
        Negative_Low_Homemode0_AlarmLow_PosLowNegLow = 00000,
        Negative_Low_Homemode0_AlarmLow_PosHighNegHigh = 00001,
        Negative_Low_Homemode0_AlarmLow_PosLowNegHigh = 00002,
        Negative_Low_Homemode0_AlarmLow_PosHighNegLow = 00003,
        Negative_Low_Homemode0_AlarmHigh_PosLowNegLow = 00010,
        Negative_Low_Homemode0_AlarmHigh_PosHighNegHigh = 00011,
        Negative_Low_Homemode0_AlarmHigh_PosLowNegHigh = 00012,
        Negative_Low_Homemode0_AlarmHigh_PosHighNegLow = 00013,

        Negative_Low_Homemode1_AlarmLow_PosLowNegLow = 00100,
        Negative_Low_Homemode1_AlarmLow_PosHighNegHigh = 00101,
        Negative_Low_Homemode1_AlarmLow_PosLowNegHigh = 00102,
        Negative_Low_Homemode1_AlarmLow_PosHighNegLow = 00103,
        Negative_Low_Homemode1_AlarmHigh_PosLowNegLow = 00110,
        Negative_Low_Homemode1_AlarmHigh_PosHighNegHigh = 00111,
        Negative_Low_Homemode1_AlarmHigh_PosLowNegHigh = 00112,
        Negative_Low_Homemode1_AlarmHigh_PosHighNegLow = 00113,


        Negative_Low_Homemode2_AlarmLow_PosLowNegLow = 00200,
        Negative_Low_Homemode2_AlarmLow_PosHighNegHigh = 00201,
        Negative_Low_Homemode2_AlarmLow_PosLowNegHigh = 00202,
        Negative_Low_Homemode2_AlarmLow_PosHighNegLow = 00203,
        Negative_Low_Homemode2_AlarmHigh_PosLowNegLow = 00210,
        Negative_Low_Homemode2_AlarmHigh_PosHighNegHigh = 00211,
        Negative_Low_Homemode2_AlarmHigh_PosLowNegHigh = 00212,
        Negative_Low_Homemode2_AlarmHigh_PosHighNegLow = 00213,

        //mode1 方向负方向  原点电平为1 复位方式是0  
        //mode1 方向负方向  原点电平为1 复位方式是1  
        //mode1 方向负方向  原点电平为1 复位方式是2  
        Negative_High_Homemode0_AlarmLow_PosLowNegLow = 01000,
        Negative_High_Homemode0_AlarmLow_PosHighNegHigh = 01001,
        Negative_High_Homemode0_AlarmLow_PosLowNegHigh = 01002,
        Negative_High_Homemode0_AlarmLow_PosHighNegLow = 01003,
        Negative_High_Homemode0_AlarmHigh_PosLowNegLow = 01010,
        Negative_High_Homemode0_AlarmHigh_PosHighNegHigh = 01011,
        Negative_High_Homemode0_AlarmHigh_PosLowNegHigh = 01012,
        Negative_High_Homemode0_AlarmHigh_PosHighNegLow = 01013,


        Negative_High_Homemode1_AlarmLow_PosLowNegLow = 01100,
        Negative_High_Homemode1_AlarmLow_PosHighNegHigh = 01101,
        Negative_High_Homemode1_AlarmLow_PosLowNegHigh = 01102,
        Negative_High_Homemode1_AlarmLow_PosHighNegLow = 01103,
        Negative_High_Homemode1_AlarmHigh_PosLowNegLow = 01110,
        Negative_High_Homemode1_AlarmHigh_PosHighNegHigh = 01111,
        Negative_High_Homemode1_AlarmHigh_PosLowNegHigh = 01112,
        Negative_High_Homemode1_AlarmHigh_PosHighNegLow = 01113,


        Negative_High_Homemode2_AlarmLow_PosLowNegLow = 01200,
        Negative_High_Homemode2_AlarmLow_PosHighNegHigh = 01201,
        Negative_High_Homemode2_AlarmLow_PosLowNegHigh = 01202,
        Negative_High_Homemode2_AlarmLow_PosHighNegLow = 01203,
        Negative_High_Homemode2_AlarmHigh_PosLowNegLow = 01210,
        Negative_High_Homemode2_AlarmHigh_PosHighNegHigh = 01211,
        Negative_High_Homemode2_AlarmHigh_PosLowNegHigh = 01212,
        Negative_High_Homemode2_AlarmHigh_PosHighNegLow = 01213,
        //mode1 方向正方向  原点电平为0 复位方式是0  
        //mode1 方向正方向  原点电平为0 复位方式是1  
        //mode1 方向正方向  原点电平为0 复位方式是2  
        Positive_Low_Homemode0_AlarmLow_PosLowNegLow = 10000,
        Positive_Low_Homemode0_AlarmLow_PosHighNegHigh = 10001,
        Positive_Low_Homemode0_AlarmLow_PosLowNegHigh = 10002,
        Positive_Low_Homemode0_AlarmLow_PosHighNegLow = 10003,
        Positive_Low_Homemode0_AlarmHigh_PosLowNegLow = 10010,
        Positive_Low_Homemode0_AlarmHigh_PosHighNegHigh = 10011,
        Positive_Low_Homemode0_AlarmHigh_PosLowNegHigh = 10012,
        Positive_Low_Homemode0_AlarmHigh_PosHighNegLow = 10013,

        Positive_Low_Homemode1_AlarmLow_PosLowNegLow = 10100,
        Positive_Low_Homemode1_AlarmLow_PosHighNegHigh = 10101,
        Positive_Low_Homemode1_AlarmLow_PosLowNegHigh = 10102,
        Positive_Low_Homemode1_AlarmLow_PosHighNegLow = 10103,
        Positive_Low_Homemode1_AlarmHigh_PosLowNegLow = 10110,
        Positive_Low_Homemode1_AlarmHigh_PosHighNegHigh = 10111,
        Positive_Low_Homemode1_AlarmHigh_PosLowNegHigh = 10112,
        Positive_Low_Homemode1_AlarmHigh_PosHighNegLow = 10113,


        Positive_Low_Homemode2_AlarmLow_PosLowNegLow = 10200,
        Positive_Low_Homemode2_AlarmLow_PosHighNegHigh = 10201,
        Positive_Low_Homemode2_AlarmLow_PosLowNegHigh = 10202,
        Positive_Low_Homemode2_AlarmLow_PosHighNegLow = 10203,
        Positive_Low_Homemode2_AlarmHigh_PosLowNegLow = 10210,
        Positive_Low_Homemode2_AlarmHigh_PosHighNegHigh = 10211,
        Positive_Low_Homemode2_AlarmHigh_PosLowNegHigh = 10212,
        Positive_Low_Homemode2_AlarmHigh_PosHighNegLow = 10213,

        //mode1 方向正方向  原点电平为1 复位方式是0  
        //mode1 方向正方向  原点电平为1 复位方式是1  
        //mode1 方向正方向  原点电平为1 复位方式是2  
        Positive_High_Homemode0_AlarmLow_PosLowNegLow = 11000,
        Positive_High_Homemode0_AlarmLow_PosHighNegHigh = 11001,
        Positive_High_Homemode0_AlarmLow_PosLowNegHigh = 11002,
        Positive_High_Homemode0_AlarmLow_PosHighNegLow = 11003,
        Positive_High_Homemode0_AlarmHigh_PosLowNegLow = 11010,
        Positive_High_Homemode0_AlarmHigh_PosHighNegHigh = 11011,
        Positive_High_Homemode0_AlarmHigh_PosLowNegHigh = 11012,
        Positive_High_Homemode0_AlarmHigh_PosHighNegLow = 11013,

        Positive_High_Homemode1_AlarmLow_PosLowNegLow = 11100,
        Positive_High_Homemode1_AlarmLow_PosHighNegHigh = 11101,
        Positive_High_Homemode1_AlarmLow_PosLowNegHigh = 11102,
        Positive_High_Homemode1_AlarmLow_PosHighNegLow = 11103,
        Positive_High_Homemode1_AlarmHigh_PosLowNegLow = 11110,
        Positive_High_Homemode1_AlarmHigh_PosHighNegHigh = 11111,
        Positive_High_Homemode1_AlarmHigh_PosLowNegHigh = 11112,
        Positive_High_Homemode1_AlarmHigh_PosHighNegLow = 11113,

        Positive_High_Homemode2_AlarmLow_PosLowNegLow = 11200,
        Positive_High_Homemode2_AlarmLow_PosHighNegHigh = 11201,
        Positive_High_Homemode2_AlarmLow_PosLowNegHigh = 11202,
        Positive_High_Homemode2_AlarmLow_PosHighNegLow = 11203,
        Positive_High_Homemode2_AlarmHigh_PosLowNegLow = 11210,
        Positive_High_Homemode2_AlarmHigh_PosHighNegHigh = 11211,
        Positive_High_Homemode2_AlarmHigh_PosLowNegHigh = 11212,
        Positive_High_Homemode2_AlarmHigh_PosHighNegLow = 11213,
        //____________________________________总结来说就是方向+电平+模式
    }

    public enum HOME_DIRECTION_LEADSHINE : int
    {
        Negative = 0,
        Positive = 1
    }

    public enum HOME_LOGIC_LEADSHINE : int
    {
        Low = 0,
        High = 1
    }

    public enum HOME_MODE_LEADSHINE : int
    {
        Homemode0 = 0,
        Homemode1 = 1,
        Homemode2 = 2
    }

    public enum ALARM_LOGIC : int
    {
        AlarmLow = 0,
        AlarmHigh = 1
    }
    public enum LIMIT_LOGIC : int
    {
        PosLowNegLow = 0,
        PosHighNegHigh = 1,
        PosLowNegHigh = 2,
        PosHighNegLow = 3
    }
}