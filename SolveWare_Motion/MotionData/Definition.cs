using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Manager_Motion.Base
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
        /// 回零模式
        /// </summary>
        HOME = 6,
        /// <summary>
        /// 周期同步位置模式
        /// </summary>
        MOTION = 8,
    }
}
