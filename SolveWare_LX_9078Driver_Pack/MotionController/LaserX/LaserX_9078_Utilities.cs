using System.Runtime.InteropServices;
using System.Text;

namespace SolveWare_BurnInInstruments //命名空间根据应用程序修改
{
    public static partial class LaserX_9078_Utilities
    {
        private const string SunTech_DLL_PATH = "PCI9078.dll";

        public const int MOT_MAX_DEVICE = 16;                   //support up to 16 devices
        public const int MOT_MAX_AXIS = 8;                      //number of axes
        public const int MOT_MAX_DIO = 16;                      //number of DIs and DOs
        public const int MOT_MAX_AIO = 4;                       //number of AOUTs

        public const int MOT_TERM_COND_STOP = 1;                //stop mode
        public const int MOT_TERM_COND_BLEND = 2;               //blend next move with tolerance constraint
        public const int MOT_TERM_COND_BLEND_PREVIOUS = 4;      //velocity blended with the velocity of previous move

        public const int MOT_COMM_OK = 0;                       //went through
        public const int MOT_COMM_ERROR_CONNECT = -1;           //can't connect realtime task
        public const int MOT_COMM_ERROR_TIMEOUT = -2;           //send command timeout
        public const int MOT_COMM_ERROR_COMMAND = -3;           //can't run command now
        public const int MOT_COMM_READ_STATUS_TIMEOUT = -4;     //read status timeout
        public const int MOT_COMM_INVALID_ARGUMENT = -5;        //invalid argument
        public const int MOT_ERROR_ALREADY_EXISTS = -6;         //device already open
        public const int MOT_ERROR_INSUFFICIENT_RESOURCE = -7;  //cannot allocate system resource

        public const int DIO_ON = 1;                            //DI/DO turn-on
        public const int DIO_OFF = 0;                           //DI/DO turn-off

        public const int TL_PVT = 8;                            //position-velocity-time profile
        public const int TL_END_FLAG = 0x10000000;              //end of profile data

        public const int TASK_FN_PATTERN = 0x50415454;          //pattern data

        public enum PN_NUMBER
        {
            PN_OutputMode = 1000,           //pulse output mode, 0 for PUL/DIR, 1 for CW/CCW, 2 for 4xAB
            PN_OutputStepLen,               //length of pulse output in nanosecond
            PN_OutputDirectionSetup,        //setup time of direction output in nanosecond
            PN_OutputDirectionHold,         //hold time of direction output in nanosecond
            PN_InvertOutputDirection,       //invert direction output, non-zero for enable, zero for disable
            PN_InputMode,                   //pulse input mode, only 4xAB mode supported
            PN_InvertInputDirection,        //invert input counter(feedback counter) direction, non-zero for enable, zero for disable
            PN_PosLimitSwitchType,          //positive hardware limit switch type, zero for normal open, non-zero for normal close
            PN_PosLimitSwitchEnable,        //enable positive hard limit input, non-zero for enable, zero for disable
            PN_NegLimitSwitchType,          //negative hardware limit switch type, zero for normal open, non-zero for normal close
            PN_NegLimitSwitchEnable,        //enable negative hard limit input, non-zero for enable, zero for disable
            PN_HomeSwitchType,              //home switch type, zero for normal open, non-zero for normal close
            PN_HomeSwitchEnable,            //reserved
            PN_IndexActiveLevel,            //index active level, zero for low level, non-zero for high level
            PN_AmpFaultType,                //amplifier fault input type,  zero for normal open, non-zero for normal close
            PN_AmpFaultEnable,              //enable amplifier fault input
            PN_AbortSwitchType,             //abort input type, zero for normal open, non-zero for normal close
            PN_AbortSwitchEnable,           //enable abort input, non-zero for enable, zero for disable

            PN_DoTimerEnable = 1033,        //enable one-shot timer on digital output
            PN_DoTimerIndex,                //dout index for one-shot timer
            PN_DoTimerOnTime,               //ON time in nanoseconds
            PN_DoTimerOffTime,              //OFF time in nanoseconds
            PN_DoTimerCount,                //toggle count, 0~65535, 0 for always
            PN_AxisMinVel,                  //set minimum velocity for joint
            PN_TrajMinVel,                  //set minimum velocity for trajectory planner
            PN_PSOEnable,                   //enable PSO(Position Synchronized Output)
            PN_PSOMinLimit,                 //min position limit for PSO in feedback position unit, XYZ supported
            PN_PSOMaxLimit,                 //max position limit for PSO in feedback position unit, XYZ supported
            PN_PSOSpacing,                  //spacing to output pulse in feedback position unit

            PN_EnableCaptureFifo = 1200,    //enable/disable capture stream data
            PN_CapturePositionSource,       //enable capture feedback position of XYZA
            PN_AxisHandleErrorMode,         //axis handle errror mode, 0- disabling axis(no deceleration), 1-not disabling axis(with deceleration)
            PN_AxisExtEnable,               //reserved for future
            PN_AxisExtEnableType,           //external enable(servo ON) type, zero for normal open, non-zero for normal close
            PN_AxisExtEnableMode,           //servo-ON control mode, 0 - sync. with internal axis enable, 1 - independent of internal axis enable
            PN_AxisOutputScale,             //multiply internal position by output scale when pulse is output to motor
            PN_AxisInputScale,              //multiply external feedback position by input scale when external pulse is input to controller
            PN_AxisEmgDecelEnable,          //enable axis to decelerate with dedicated value in emergency
            PN_AxisEmgDecel,                //set decleration value of an axis in emergency, e.g. EL active, abort active
            PN_AxisHomeDelay,               //length of delay between homing motions in seconds
            PN_AxisAbortEnable,             //reserved for future
            PN_AxisAbortType,               //reserved for future
            PN_AxisAbortIndex,              //reserved for future
            PN_AxisPELIndex,                //axis EL+ index in dins
            PN_AxisMELIndex,                //axis EL- index in dins
            PN_AxisHomeIndex                //axis home index in dins
        }

        public enum TASK_COMMAND_CODE
        {
            TRAJ_SET_LINE = 0x5400,         //set line with target of XYZ
            TRAJ_SET_CIRCLE,                //set circle with target of XYZ
            TRAJ_SET_DELAY,                 //set delay in seconds, x - delay value in seconds
            TRAJ_SET_CENTER,                //set center of arc
            TRAJ_SET_NORMAL,                //set normalized vector of arc
            TRAJ_SET_TURN,                  //set turn, x - circle turn
            TRAJ_SET_ABC,                   //set target of ABC
            TRAJ_SET_VEL,                   //set sync. velocity, x - velocity
            TRAJ_SET_ACCEL,                 //set sync. acceleration, x - acceleration
            TRAJ_SET_ID,                    //set command ID, x - ID
            TRAJ_SET_TERM_COND,             //set termination condition, x-term. condition, y-tolerance
            TRAJ_COMBINE_AXES,              //combine axes with tp, x - bit flag of xyz

            TRAJ_SET_DO,                    //set sync. dout, index - dout index, x - dout value
            TRAJ_SET_PWM,                   //set sync. pwm,  index - pwm index, x - period in nanoseconds, y - width in nanseconds, z - pulse count
            TRAJ_SET_AOUT,                  //set sync. aout, index - aout index, x - value in volt

            PSO_SET_ENABLE = 0x5000,        //set PSO enable/disable, x - spacing, flag - bit flag for axis enable
            PSO_SET_CONDITION_MIN_POS,      //set min position(x,y,z) for later use
            PSO_SET_CONDITION_TRIGGER_ON,   //set conditon as triggered ON and max position(x,y,z)
            PSO_SET_CONDITION_TRIGGER_OFF,  //set conditon as triggered OFF and max position(x,y,z), flag - bit flag for limit enable
            PSO_SET_CONDITION_INRANGE_ON    //set conditon as in-range ON and max position(x,y,z), flag - bit flag for limit enable
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct PmCartesian
        {
            public void SetAxis(int i, double val)
            {
                switch (i)
                {
                    case 0: x = val; break;
                    case 1: y = val; break;
                    case 2: z = val; break;
                }
            }

            public double x;
            public double y;
            public double z;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct MotPose
        {
            public void SetAxis(int i, double val)
            {
                switch (i)
                {
                    case 0: x = val; break;
                    case 1: y = val; break;
                    case 2: z = val; break;
                    case 3: a = val; break;
                    case 4: b = val; break;
                    case 5: c = val; break;
                }
            }

            public double GetAxis(int i)
            {
                switch (i)
                {
                    case 0: return x; break;
                    case 1: return y; break;
                    case 2: return z; break;
                    case 3: return a; break;
                    case 4: return b; break;
                    case 5: return c; break;
                }
                return 0;
            }

            public double x;
            public double y;
            public double z;
            public double a;
            public double b;
            public double c;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct MOT_AXIS_STAT
        {
            public double cmdPos;      //内部寄存器位置    current command position
            public double cmdVel;      //内部寄存器速度    current command velocity
            public double cmdAcc;      //reserved
            public double actPos;      //外部编码器位置    actual position(feedback position)
            public double actVel;      //外部编码器速度    actual velocity(feedback velocity)
            public double actAcc;      //reserved
            public double reserved1;   //reserved
            public double reserved2;   //reserved
            public int homeSwitch;     //原点的输入状态    non-zero means home switch is tripped
            public int minHardLimit;   //硬极限    non-zero means min hard limit tripped
            public int maxHardLimit;   //硬极限    non-zero means max hard limit tripped
            public int fault;          //告警输入   non-zero means servo amplifier fault
            public int abort;          //non-zero means abort active
            public int minSoftLimit;   //软极限    non-zero means min soft limit exceeded
            public int maxSoftLimit;   //软极限    non-zero means max soft limit exceeded
            public int inpos;          //控制轴到位(停止)      non-zero means in position
            public int enabled;        //控制轴使能状态        non-zero means enabled
            public int homing;         //Home中                  non-zero means homing
            public int homed;          //Home完成                 non-zero means axis has been homed
            public int error;          //错误                     non-zero means error
            public int ferror;         //reserved
            public int queue;          //number of pending time-locked profile motion
            public int id;             //当前正在执行的 id号码       id of the currently time-locked profile motion
            public int gear;           //non-zero means a master or slave axis engaged in gear
            public int ingear;         //non-zero means slave axis is locked to master axis
            public int reserved3;      //reserved
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct MOT_TRAJ_STAT
        {
            public MotPose cmdPos;     //current command position
            public MotPose actPos;     //reserved
            public double vel;         //current command velocity
            public double accel;       //reserved
            public double reserved1;   //reserved
            public double reserved2;   //reserved
            public int inpos;          //non-zero means in position
            public int enabled;        //non-zero means enabled
            public int queue;          //number of pending motions
            public int queueFull;      //non-zero means queue is full
            public int id;             //id of the currently executing motion
            public int paused;         //non-zero means motion paused
            public uint axes;          //axes combined with trajectory planner
            public int error;          //non-zero means error
            public sbyte axisIndex0;   //axis index of traj's X, negative for no axis combined
            public sbyte axisIndex1;   //axis index of traj's Y, negative for no axis combined
            public sbyte axisIndex2;   //axis index of traj's Z, negative for no axis combined
            public sbyte axisIndex3;   //axis index of traj's A, negative for no axis combined
            public sbyte axisIndex4;   //axis index of traj's B, negative for no axis combined
            public sbyte axisIndex5;   //axis index of traj's C, negative for no axis combined
            public sbyte reserved3;    //
            public sbyte reserved4;    //
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct MOT_CMP_STAT
        {
            public int queFull;         //nonzero means reference queue full
            public int queEmpty;        //nonzero means reference queue empty
            public int queItem;         //queued items
            public int curRef;          //current reference
            public int matchCount;      //match count
            public int reserved1;       //reserved
            public int reserved2;       //reserved
            public int reserved3;       //reserved
        }
        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct MOT_CMP_STAT_EX
        {
            public double queFull;         //nonzero means reference queue full
            public double queEmpty;        //nonzero means reference queue empty
            public double queItem;         //queued items
            public double curRef;          //current reference
            public double matchCount;      //match count
            public double reserved1;       //reserved
            public double reserved2;       //reserved
            public double reserved3;       //reserved
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct TL_PAIR
        {
            public double p;            //target position
            public double v;            //target velocity
            public double t;            //duration in seconds
            public uint type;           //instruction type
            public uint id;             //instruction ID
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct TASK_COMMAND
        {
            public ushort code;   //command code, refer to TASK_COMMAND_CODE
            public ushort flag;   //reserved for future
            public ushort index;  //index of dout, aout, pwm
            public ushort rsv;    //reserved for future
            public double x;      //target of x
            public double y;      //target of y
            public double z;      //target of z
        }

        /// <summary>
        /// serialize TASK_COMMAND array into byte array
        /// </summary>
        /// <param name="items">task command array</param>
        /// <param name="Length">array length</param>
        /// <returns></returns>
        public static byte[] getBytes(TASK_COMMAND[] items, int Length)
        {
            int index = (Length > items.Length) ? items.Length : Length;
            int size = Marshal.SizeOf(items[0]);
            byte[] buff = new byte[size * index];
            byte[] b = new byte[size];
            System.IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                for (int i = 0; i < index; i++)
                {
                    Marshal.StructureToPtr(items[i], ptr, true);
                    Marshal.Copy(ptr, b, 0, size);
                    b.CopyTo(buff, i * size);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return buff;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Pack = 4)]
        public struct FIFO_ITEM
        {
            public double[] GetPos()
            {
                double[] a = new double[]
                {
                    pos0,
                    pos1,
                    pos2,
                    pos3,
                    pos4,
                    pos5,
                    pos6,
                    pos7,
                };
                return a;
            }

            public short[] GetAin()
            {
                short[] a = new short[]
                {
                    ain0,
                    ain1,
                    ain2,
                    ain3,
                };
                return a;
            }

            public double[] GetVoltage_mV()
            {
                double[] a = new double[]
                {
                    P9078_AdcToVoltage_mV(ain0),
                    P9078_AdcToVoltage_mV(ain1),
                    P9078_AdcToVoltage_mV(ain2),
                    P9078_AdcToVoltage_mV(ain3),
                };
                return a;
            }

            public double[] GetCurrent_mA(int id)
            {
                double[] a = new double[]
                {
                    P9078_AdcToCurrent_mA(ain0,AnalogCard_ResList[id][0]),
                    P9078_AdcToCurrent_mA(ain1,AnalogCard_ResList[id][1]),
                    P9078_AdcToCurrent_mA(ain2,AnalogCard_ResList[id][2]),
                    P9078_AdcToCurrent_mA(ain3,AnalogCard_ResList[id][3]),
                };
                return a;
            }

            public double pos0;         //axis position
            public double pos1;         //axis position
            public double pos2;         //axis position
            public double pos3;         //axis position
            public double pos4;         //axis position
            public double pos5;         //axis position
            public double pos6;         //axis position
            public double pos7;         //axis position
            public short ain0;          //analog input0
            public short ain1;          //analog input1
            public short ain2;          //analog input2
            public short ain3;          //analog input3
            public int id;              //motion command id
            public int rsv;             //reserved
        }

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetBacklash(int dev, int axis, double backlash);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetMinPositionLimit(int dev, int axis, double limit);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetMaxPositionLimit(int dev, int axis, double limit);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetHomingParams(int dev, int axis, double home, double offset, double final_vel, double search_vel, double latch_vel, int use_index);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetMaxVelocity(int dev, int axis, double vel);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetMaxAcceleration(int dev, int axis, double acc);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetJerk(int dev, int axis, double jerk);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetParameter(int dev, int axis, int number, int value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetRealParameter(int dev, int axis, int number, double value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetCommandPosition(int dev, int axis, double pos);    //deprecated interface

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetActualPosition(int dev, int axis, double pos);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisEnable(int dev, int axis);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisDisable(int dev, int axis);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisSetServoON(int dev, int axis, int _on);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisAbort(int dev, int axis);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisHome(int dev, int axis);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisUnhome(int dev, int axis);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisContJog(int dev, int axis, double vel);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisIncrJog(int dev, int axis, double incr, double vel);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisAbsJog(int dev, int axis, double pos, double vel);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_AxisJogTP(int dev, int axis, TL_PAIR[] pair, int count);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajSetMaxVelocity(int dev, double vel);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajCombineAxes(int dev, uint flag);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajCombineAxesEx(int dev, int[] axis, int count);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajAddAxis(int dev, int axis);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajRemoveAxis(int dev, int axis);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajSetMotionId(int dev, int id);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajSetTermCond(int dev, int cond, double tolerance);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajEnable(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajDisable(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajAbort(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajPause(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajResume(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajDelay(int dev, double delay);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajLinearMove(int dev, ref MotPose end, double vel, double acc);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajLinearMoveEx(int dev, ref MotPose end, double vel, double acc, int type);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajCircularMove(int dev, ref MotPose end, ref PmCartesian center, ref PmCartesian nv, int turn, double vel, double acc);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajCircularMoveEx(int dev, ref MotPose end, ref PmCartesian center, ref PmCartesian nv, int turn, double vel, double acc, int type);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TrajSetSyncDout(int dev, int index, int value, double offset, int mode);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_GearIn(int dev, int master, int slave, int ratio_numerator, int ratio_denominator, int source);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_GearOut(int dev, int slave);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionInit(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionDeinit(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionLoadCfg(int dev, [MarshalAs(UnmanagedType.LPStr)] string filename, int isSingleFile);  //isSingleFile 1表示共用， 0表示不共用

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetDevIDs(int[] ID, int count, ref int actCnt);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionAbort();

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionSetAout(int dev, int index, double value, int now);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionSetDout(int dev, int index, int value, int now);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionSetDoutEx(int dev, uint value, int now);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionSetPWM(int dev, int index, int period, int width, int now);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionSetPWMEx(int dev, int index, int period, int width, int count, int now);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_CmpEnable(int dev, int index, int enable, int active_level, int ref_source, int length);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_CmpSetRef(int dev, int index, int pos);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_CmpSetRefEx(int dev, int index, double pos);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_CmpGetStatus(int dev, int index, ref MOT_CMP_STAT status);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_CmpGetStatusEx(int dev, int index, ref MOT_CMP_STAT_EX status);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_PsoEnable(int dev, int spacing, int flag);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_PsoSetCondition(int dev, ref PmCartesian _min, ref PmCartesian _max, int flag, int mode, int operation);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TaskRun(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TaskAbort(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TaskSetData(int dev, int fn, byte[] buffer, int size);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TaskSetParameter(int dev, int number, int value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_TaskSetRealParameter(int dev, int number, double value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionUpdate(int dev);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetAxisStatus(int dev, int axis, ref MOT_AXIS_STAT stat, int size);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetTrajStatus(int dev, ref MOT_TRAJ_STAT stat, int size);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetDin(int dev, int index, ref int value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetAin(int dev, int index, ref double value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetAout(int dev, int index, ref double value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetPWMReady(int dev, int index, ref int value);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetTaskStatus(int dev, uint[] value, int count);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetPsoStatus(int dev, uint[] value, int count);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetFifoStatus(int dev, [In, Out] uint[] value, int count);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionReadFifo(int dev, [In, Out] FIFO_ITEM[] value, int count, ref int actCount);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetDevInfo(int dev, [In, Out] uint[] value, int count);

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetCfgFileNameA(int dev, [OutAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] StringBuilder buff, int size);
        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionGetCfgFileNameW(int dev, [OutAttribute()] [MarshalAsAttribute(UnmanagedType.LPWStr)] StringBuilder buff, int size);


        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionInd(int dev, uint offset, ref uint data);  //寄存器读

        [DllImport(SunTech_DLL_PATH)]
        public static extern int P9078_MotionOutd(int dev, uint offset, uint data);  //寄存器写
    }
}