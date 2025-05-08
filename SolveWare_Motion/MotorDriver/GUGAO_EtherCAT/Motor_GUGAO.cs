
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_Motion
{
    public class Motor_GUGAO : MotorAxisBase
    {
        const int ORG_SIGNAL_BIT = 2;
        public bool _pause = false;
        private AutoResetEvent MotionDoneEvent = new AutoResetEvent(false);

    
        public Motor_GUGAO(MotorSetting setting ) : base(setting )
        {
            this.Interation.AxisName =  setting.Name ;
            this.Interation.AxisTag = $"GUGAO Motion AxisName[{setting.Name}] CoreID ={MotorGeneralSetting.MotorTable.CardNo},AxisID ={MotorGeneralSetting.MotorTable.AxisNo}";
        }
        public void Pause()
        {
            this._pause = true;
        }
        public void Resume()
        {
            this._pause = false;
        }
        RUNMODE_GUGAO GetAxisRunMode()
        {
            ushort pValue;                 // 轴状态

            if (_interation.IsSimulation)
            {
                return RUNMODE_GUGAO.MOTION;
            }
            var ret = GUGAO_LIB.GTN_GetEcatAxisMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, out pValue);
            var mode = (RUNMODE_GUGAO)pValue;
            ThrowIfFeedbackError(ret, $"GetAxisRunMode() error");
            return mode;
        }
        public override void StartStatusReading()
        {
            if (readStatusSource != null) return;
            readStatusSource = new CancellationTokenSource();
            Task task = new Task(() =>
            {
                while (!readStatusSource.IsCancellationRequested)
                {
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsOrg = Get_Origin_Signal();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsAlarm = Get_Alarm_Signal();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsPosLimit = Get_PEL_Signal();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsNegLimit = Get_NEL_Signal();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsInPosition = Get_InPos_Signal();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    if (!_interation.IsSimulation) _interation.IsMoving = IsMotorMoving();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.CurrentPulse = Get_CurPulse();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.CurrentPosition = Get_CurUnitPos();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsServoOn = Get_ServoStatus();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                }

                cancelDoneFlag.Set();

            }, readStatusSource.Token, TaskCreationOptions.LongRunning);
            task.Start();
        }



        public override int Get_IO_sts()
        {
            int iAxisSts;   				// 轴状态
            uint uiClock;
            if (_interation.IsSimulation)
            {
                return 0;
            }
            var ret = GUGAO_LIB.GTN_GetSts(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, out iAxisSts, 1, out uiClock);
            ThrowIfFeedbackError(ret, $"Get_IO_sts() error");
            return iAxisSts;
        }

        private void ThrowIfFeedbackError(short feedback, string exMsg)
        {
            if (feedback != 0)
            {
                throw new Exception($"{this.Interation.AxisTag}{exMsg}!");
            }
        }
        private int GetMask()
        {
            return 1 << (this._setting.MotorTable.AxisNo - 1);
        }

        public override bool Clear_AlarmSignal()
        {
            if (this._interation.IsSimulation)
            {
                return true;

            }
            const short axisCount = 1;
            var ret = GUGAO_LIB.GTN_ClrSts(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, axisCount);
            ThrowIfFeedbackError(ret, $"Get_CurPulse() error");
            if(ret != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Get_Alarm_Signal()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_GUGAO)Get_IO_sts() & AXIS_STATUS_GUGAO.FlagAlarm) == AXIS_STATUS_GUGAO.FlagAlarm;
        }

        public override double Get_AnalogInputValue()
        {

            if (this._interation.IsSimulation) return 0.0;

            return 0.0;
            // throw new NotImplementedException();
        }

        public override double Get_CurPulse()
        {
            if (this._interation.IsSimulation) return 0.0;
            uint clk = 0;
            double curpos = 0.0;
            var ret = GUGAO_LIB.GTN_GetEncPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, out curpos, 1, out clk);
            ThrowIfFeedbackError(ret, $"Get_CurPulse() error");
            return curpos;
        }

        public override double Get_CurUnitPos()
        {
            double position = 0;

            if (this._interation.IsSimulation)
            {
                return _interation.CurrentPosition;
            }


            if (this._setting.MotorTable.IsFormulaAxis)
            {
                double mm = this._interation.CurrentPulse * this._setting.MotorTable.UnitOfRound / this._setting.MotorTable.Resolution;
                position = FormulaCalc_AngleToUnit(mm);
                return position;
            }
            else
            {
                position = this._interation.CurrentPulse * this._setting.MotorTable.UnitOfRound / this._setting.MotorTable.Resolution;
                return position;
            }
        }

        public override bool Get_InPos_Signal()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_GUGAO)Get_IO_sts() & AXIS_STATUS_GUGAO.FlagInPos) == AXIS_STATUS_GUGAO.FlagInPos;
        }


        public override bool Get_NEL_Signal()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_GUGAO)Get_IO_sts() & AXIS_STATUS_GUGAO.FlagNegLimit) == AXIS_STATUS_GUGAO.FlagNegLimit;
        }

        public override bool Get_Origin_Signal()
        {
            //const int HomeNormalDone = 3;
            //const int HomeError = 4;

            //ushort usHomeSts = 0;
            //var ret = GUGAO_LIB.GTN_GetEcatHomingStatus(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, out usHomeSts);
            //ThrowIfFeedbackError(ret, $" Get_Origin_Signal() error");
            //return usHomeSts == HomeNormalDone;


            uint pValue;                // 轴状态

            bool isOrgOn = false;
            if (_interation.IsSimulation)
            {
                return false;
            }
            var ret = GUGAO_LIB.GTN_GetEcatAxisDI(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, out pValue);
            isOrgOn = JuniorMath.IsBitEqualsOne(pValue, ORG_SIGNAL_BIT);
            ThrowIfFeedbackError(ret, $"Get_IO_sts() error");
            return isOrgOn;


        }

        public override bool Get_PEL_Signal()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_GUGAO)Get_IO_sts() & AXIS_STATUS_GUGAO.FlagPosLimit) == AXIS_STATUS_GUGAO.FlagPosLimit;
        }

        public override bool Get_ServoStatus()
        {

            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_GUGAO)Get_IO_sts() & AXIS_STATUS_GUGAO.FlagServoOn) == AXIS_STATUS_GUGAO.FlagServoOn;
        }

        //1：离开负限位后，第一个index标记回零
        //2：离开正限位后，第一个index标记回零 
        //3：离开回零开关后，第一个index标记回零（正行程回零）
        //4：接触回零开关后，第一个index标记回零（正行程回零）
        //5：离开回零开关后，第一个index标记回零（负行程回零）
        //6：接触回零开关后，第一个index标记回零（负行程回零） 
        //public override int HomeMove()
        //{
        //    int errorCode = ErrorCodes.NoError;
        //    string sErr = string.Empty;

        //    if (IsProhibitToHome())
        //        return ErrorCodes.MotorIsProhibitedToMove;
        //    //GTN_SetEcatHomingPrm
        //    //GTN_StartEcatHoming
        //    //GTN_GetEcatHomingStatus
        //    ///
        //    ///bit
        //    //0 0：正在回零 1：回零完成
        //    //1 0:  无意义 1:回零成功完成
        //    //2 0:  无意义 1:回零过程出错

        //    // 设置回零参数
        //    const Int32 OFFSET = 1000;
        //    const ushort PROBE_FUNC = 0;
        //    const ushort HOME_DONE_FLAG = 3;

        //    short ret = 0;
        //    ushort usHomeSts = 0;

        //    isStopReq = false;
        //    Set_Servo(false);
        //    Thread.Sleep(100);
        //    Set_Servo(true);

        //    MotionStatus flag = MotionStatus.MT_DONE;

        //    isStopReq = false;

        //    if (this._interation.IsSimulation)
        //    {

        //        this._interation.PlanPosition = 0;
        //        double DistanceToMove = 0 - currentPulse;
        //        double EstimateTimeTaken = 0.01 * (Math.Abs(DistanceToMove) / this._setting.MotorSpeed.Home_Max_Velocity);
        //        MoveTo(0, SpeedType.Home);
        //        return (int)MotionStatus.MT_DONE;
        //    }
        //    Clear_AlarmSignal();

        //    float StartVel = Convert.ToSingle(this._setting.MotorSpeed.Home_Start_Velocity);
        //    float MaxVel = Convert.ToSingle(this._setting.MotorSpeed.Home_Max_Velocity);
        //    double Acc = this._setting.MotorSpeed.Home_Acceleration;
        //    double Dec = this._setting.MotorSpeed.Home_Deceleration;

        //    ret = GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.MOTION);
        //    // 必须处于伺服使能状态，切换到回零模式
        //    ret = GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.HOME);

        //    ret = GUGAO_LIB.GTN_SetEcatHomingPrm(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, this._setting.MotorModes.Home_Mode, MaxVel, StartVel, Acc, OFFSET, PROBE_FUNC);
        //    ThrowIfFeedbackError(ret, $"HomeMove() GTN_SetEcatHomingPrm error");
        //    // 启动回零
        //    ret = GUGAO_LIB.GTN_StartEcatHoming(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo);
        //    ThrowIfFeedbackError(ret, $"HomeMove() GTN_StartEcatHoming error");
        //    do
        //    {
        //        ret = GUGAO_LIB.GTN_GetEcatHomingStatus(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, out usHomeSts);
        //        ThrowIfFeedbackError(ret, $"HomeMove() GTN_GetEcatHomingStatus error");
              
        //    } while (HOME_DONE_FLAG != usHomeSts);  //等待搜索原点完成



        //    ret = GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.MOTION); // 切换到位置控制模式
        //    ThrowIfFeedbackError(ret, $"HomeMove() GTN_SetHomingMode error");

        //    ret = GUGAO_LIB.GTN_ZeroPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, 1);
        //    ThrowIfFeedbackError(ret, $"HomeMove() GTN_SetHomingMode error");

        //    this.Clear_AlarmSignal();
        //    return errorCode;
        //}

        public override bool Init()
        {
            if (this._interation.IsSimulation) return true;

            try
            {
                //软限位设置
                //负向软限位，当规划位置小于该值时，负限位触发。 
                //默认值为：0x80000000，表示负向软限位无效。

                //正向软限位，当规划位置大于该值时，正限位触发。 
                //默认值为：0x7fffffff，表示正向软限位无效

                //20221229,改动，增加软限位使能
                bool enableSoftLimint = this._setting.MotorTable.Enable_SoftLimit;
                int nelPulse = Int32.MinValue;
                int pelPulse = Int32.MaxValue;
                if (enableSoftLimint==true)
                {
                     nelPulse = (int)(this._setting.MotorTable.MinDistance_SoftLimit * this._setting.MotorTable.Resolution / this._setting.MotorTable.UnitOfRound);
                     pelPulse = (int)(this._setting.MotorTable.MaxDistance_SoftLimit * this._setting.MotorTable.Resolution / this._setting.MotorTable.UnitOfRound);
                }

                var ret = GUGAO_LIB.GTN_SetSoftLimit(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, pelPulse, nelPulse);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public override bool IsMotorMoving()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_GUGAO)Get_IO_sts() & AXIS_STATUS_GUGAO.FlagMotion) == AXIS_STATUS_GUGAO.FlagMotion;
        }
        public override void Jog(bool isPositive)
        {
            if (this._interation.IsSimulation) return;


            float dir = isPositive ? 1.0f : -1.0f;

            //ConverToJogMMPerSec(ref minVel, ref maxVel, ref acc, ref dec);

            GUGAO_LIB.TJogPrm jog;
            var maxVel = Convert.ToSingle(_setting.MotorSpeed.Jog_Max_Velocity * dir);

            jog.acc = _setting.MotorSpeed.Jog_Acceleration;
            jog.dec = _setting.MotorSpeed.Jog_Deceleration;
            jog.smooth = _setting.MotorSpeed.Jog_SmoothingTime;


            if (this.GetAxisRunMode() != RUNMODE_GUGAO.MOTION)
            {
                GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.MOTION);
            }
            this.Clear_AlarmSignal();

            var ret = GUGAO_LIB.GTN_PrfJog(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo); // 设置为Jog运动模式

            ret = GUGAO_LIB.GTN_SetJogPrm(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, ref jog); // 设置Jog运动参数

            ret = GUGAO_LIB.GTN_SetVel(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, maxVel);  // 设置目标速度

            ret = GUGAO_LIB.GTN_Update(this._setting.MotorTable.CardNo, GetMask());    // 更新轴运动

        }
        //public override int MoveTo(double pos, SpeedType speedType, float slowFactor = 1)
        //{
        //    int errorCode = ErrorCodes.NoError;
        //    string sErr = string.Empty;

        //    if (IsProhibitToMove())
        //        return ErrorCodes.MotorIsProhibitedToMove;

        //    if (this._interation.IsSimulation) this.isMoving = true;
        //    if (this._interation.IsServoOn == false)
        //    {
        //        return ErrorCodes.MotorIsProhibitedToMove;
        //    }
        //    isStopReq = false;

        //    float startVel = 0;
        //    float maxVel = 0;
        //    double acc = 0;
        //    double dec = 0;
        //    ConvertSpeedMMPerSec(speedType, ref startVel, ref maxVel, ref acc, ref dec);

        //    startVel *= slowFactor;
        //    maxVel *= slowFactor;
        //    var currentPos =  Get_CurUnitPos();
        //    var currentPulse = Get_CurPulse();
        //    double distanceToMove = pos - currentPos;
        //    double estimateTimeTaken = 0.01 * (Math.Abs(distanceToMove) / maxVel);
        //    pos = Math.Round(pos, 5, MidpointRounding.AwayFromZero);
        //    DateTime commmandStartTime = DateTime.Now;

        //    this._interation.PlanPosition = pos;

        //    var temp = (pos * 1000.0 * 1000.0 ) / this._setting.MotorTable.UnitOfRound * this._setting.MotorTable.Resolution ;

        //    double targetPulse = Math.Round((temp / (1000.0 * 1000.0)), 5);
 
        //    if (this._interation.IsSimulation)
        //    {
        //        return errorCode;
        //    }

        //    GUGAO_LIB.TTrapPrm trap;
 

        //    maxVel = Convert.ToSingle(this._setting.MotorSpeed.Auto_Max_Velocity);//50
        //    trap.acc = Convert.ToSingle(this._setting.MotorSpeed.Auto_Acceleration);//0.5
        //    trap.dec = Convert.ToSingle(this._setting.MotorSpeed.Auto_Deceleration);//0.5
        //    trap.smoothTime = Convert.ToInt16(this._setting.MotorSpeed.Auto_SmoothingTime); //10;
        //    trap.velStart = Convert.ToSingle(this._setting.MotorSpeed.Auto_Start_Velocity);//0

        //    Clear_AlarmSignal();
        //    if (this.GetAxisRunMode() != RUNMODE_GUGAO.MOTION)
        //    {
        //        GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.MOTION);
        //    }

        //    var ret = GUGAO_LIB.GTN_PrfTrap(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo); // 设置为点位运动模式

        //    ret = GUGAO_LIB.GTN_SetTrapPrm(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, ref trap); // 设置点位运动参数

        //    ret = GUGAO_LIB.GTN_SetVel(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, maxVel);  // 设置目标速度

        //    ret = GUGAO_LIB.GTN_SetPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)targetPulse);  // 设置目标位置

        //    ret = GUGAO_LIB.GTN_Update(this._setting.MotorTable.CardNo, GetMask());    // 更新轴运动


        //    while (true)
        //    {
        //        if (IsMotorMoving() == false)
        //        {
        //            break;
        //        }
        //        Thread.Sleep(10);

        //    }
        //    MotionDoneEvent.Set();


        //    return errorCode;
        //}
        //public override int MoveToV2(double pos, SpeedType speedType, float slowFactor = 1 , Func<bool> breakWaitFunc = null)
        //{
        //    int errorCode = ErrorCodes.NoError;
        //    string sErr = string.Empty;

        //    if (IsProhibitToMove())
        //        return ErrorCodes.MotorIsProhibitedToMove;

        //    if (this._interation.IsSimulation) this.isMoving = true;
        //    if (this._interation.IsServoOn == false)
        //    {
        //        return ErrorCodes.MotorIsProhibitedToMove;
        //    }
        //    isStopReq = false;

        //    float startVel = 0;
        //    float maxVel = 0;
        //    double acc = 0;
        //    double dec = 0;
        //    ConvertSpeedMMPerSec(speedType, ref startVel, ref maxVel, ref acc, ref dec);

        //    startVel *= slowFactor;
        //    maxVel *= slowFactor;
        //    var currentPos = Get_CurUnitPos();
        //    var currentPulse = Get_CurPulse();
        //    double distanceToMove = pos - currentPos;
        //    double estimateTimeTaken = 0.01 * (Math.Abs(distanceToMove) / maxVel);
        //    pos = Math.Round(pos, 5, MidpointRounding.AwayFromZero);
        //    DateTime commmandStartTime = DateTime.Now;

        //    this._interation.PlanPosition = pos;

        //    var temp = (pos * 1000.0 * 1000.0) / this._setting.MotorTable.UnitOfRound * this._setting.MotorTable.Resolution;

        //    double targetPulse = Math.Round((temp / (1000.0 * 1000.0)), 5);

        //    if (this._interation.IsSimulation)
        //    {
        //        return errorCode;
        //    }

        //    GUGAO_LIB.TTrapPrm trap;


        //    maxVel = Convert.ToSingle(this._setting.MotorSpeed.Auto_Max_Velocity);//50
        //    trap.acc = Convert.ToSingle(this._setting.MotorSpeed.Auto_Acceleration);//0.5
        //    trap.dec = Convert.ToSingle(this._setting.MotorSpeed.Auto_Deceleration);//0.5
        //    trap.smoothTime = Convert.ToInt16(this._setting.MotorSpeed.Jog_SmoothingTime); //10;
        //    trap.velStart = Convert.ToSingle(this._setting.MotorSpeed.Auto_Start_Velocity);//0

        //    Clear_AlarmSignal();
        //    if (this.GetAxisRunMode() != RUNMODE_GUGAO.MOTION)
        //    {
        //        GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.MOTION);
        //    }

        //    var ret = GUGAO_LIB.GTN_PrfTrap(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo); // 设置为点位运动模式

        //    ret = GUGAO_LIB.GTN_SetTrapPrm(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, ref trap); // 设置点位运动参数

        //    ret = GUGAO_LIB.GTN_SetVel(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, maxVel);  // 设置目标速度

        //    ret = GUGAO_LIB.GTN_SetPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)targetPulse);  // 设置目标位置

        //    ret = GUGAO_LIB.GTN_Update(this._setting.MotorTable.CardNo, GetMask());    // 更新轴运动


        //    while (true)
        //    {
        //        if (IsMotorMoving() == false)
        //        {
        //            break;
        //        }
        //        Thread.Sleep(10);
        //        if (breakWaitFunc?.Invoke() == true)
        //        {
        //            break;
        //        }
        //        //暂停
        //        if (_pause) 
        //        {  
        //            Stop();
        //            while (true)
        //            {
                      
        //                Thread.Sleep(100);
        //                if (!_pause)
        //                {
        //                    //ret = GUGAO_LIB.GTN_PrfTrap(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo); // 设置为点位运动模式
        //                    //ret = GUGAO_LIB.GTN_SetTrapPrm(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, ref trap); // 设置点位运动参数
        //                    //ret = GUGAO_LIB.GTN_SetVel(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, maxVel);  // 设置目标速度
        //                    //ret = GUGAO_LIB.GTN_SetPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)targetPulse);  // 设置目标位置
        //                    ret = GUGAO_LIB.GTN_Update(this._setting.MotorTable.CardNo, GetMask()); // 更新轴运动
        //                    break;
        //                }
        //                if (breakWaitFunc?.Invoke() == true)
        //                {
        //                    _pause = false;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    MotionDoneEvent.Set();
        //    return errorCode;
        //}
        /// <summary>
        /// 单轴运动参数和指令
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="speedType"></param>
        /// <param name="slowFactor"></param>
        /// <returns></returns>
        public override int MoveToV3(double pos, SpeedType speedType,SpeedLevel speedLevel)
        {
            int errorCode = ErrorCodes.NoError;
            string sErr = string.Empty;

            if (IsProhibitToMove())
                return ErrorCodes.MotorIsProhibitedToMove;

            if (this._interation.IsSimulation) this.isMoving = true;
            if (this._interation.IsServoOn == false)
            {
                return ErrorCodes.MotorIsProhibitedToMove;
            }
            isStopReq = false;

            float startVel = 0;
            float maxVel = 0;
            double acc = 0;
            double dec = 0;
            //ConvertSpeedMMPerSec(speedType, ref startVel, ref maxVel, ref acc, ref dec);

            //startVel *= slowFactor;
            //maxVel *= slowFactor;
 
            pos = Math.Round(pos, 5, MidpointRounding.AwayFromZero);
            this._interation.PlanPosition = pos;
            var temp = (pos * 1000.0 * 1000.0) / this._setting.MotorTable.UnitOfRound * this._setting.MotorTable.Resolution;
            double targetPulse = Math.Round((temp / (1000.0 * 1000.0)), 5);
            //if (this._interation.IsSimulation)
            //{
            //    return errorCode;
            //}
            GUGAO_LIB.TTrapPrm trap = new GUGAO_LIB.TTrapPrm();

            switch(speedLevel)
            {
                case SpeedLevel.High:
                case SpeedLevel.Normal:
                    {
                        maxVel = Convert.ToSingle(this._setting.MotorSpeed.Auto_Max_Velocity);//50
                        trap.acc = Convert.ToSingle(this._setting.MotorSpeed.Auto_Acceleration);//0.5
                        trap.dec = Convert.ToSingle(this._setting.MotorSpeed.Auto_Deceleration);//0.5
                        trap.smoothTime = Convert.ToInt16(this._setting.MotorSpeed.Auto_SmoothingTime); //10;
                        trap.velStart = Convert.ToSingle(this._setting.MotorSpeed.Auto_Start_Velocity);//0
                    }
                    break;
                case SpeedLevel.Low:
                    {
                        maxVel = Convert.ToSingle(this._setting.MotorSpeed.Auto_Low_Max_Velocity);//50
                        trap.acc = Convert.ToSingle(this._setting.MotorSpeed.Auto_Low_Acceleration);//0.5
                        trap.dec = Convert.ToSingle(this._setting.MotorSpeed.Auto_Low_Deceleration);//0.5
                        trap.smoothTime = Convert.ToInt16(this._setting.MotorSpeed.Auto_Low_SmoothingTime); //10;
                        trap.velStart = Convert.ToSingle(this._setting.MotorSpeed.Auto_Low_Start_Velocity);//0
                    }
                    break;
            }



            Clear_AlarmSignal();
            if (this.GetAxisRunMode() != RUNMODE_GUGAO.MOTION)
            {
                GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.MOTION);
            }
            var ret = GUGAO_LIB.GTN_PrfTrap(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo); // 设置为点位运动模式
            ret = GUGAO_LIB.GTN_SetTrapPrm(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, ref trap); // 设置点位运动参数
            ret = GUGAO_LIB.GTN_SetVel(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, maxVel);  // 设置目标速度
            ret = GUGAO_LIB.GTN_SetPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)targetPulse);  // 设置目标位置
            ret = GUGAO_LIB.GTN_Update(this._setting.MotorTable.CardNo, GetMask());    // 更新轴运动
            return errorCode;
        }
        public override void Set_Servo(bool on)
        {
            do
            {
                if (this._interation.IsSimulation)
                {
                    this._interation.IsServoOn = on;

                    break;
                }
                short ret = 0;
                this.Clear_AlarmSignal();
                if (on)
                {
                    ret = GUGAO_LIB.GTN_AxisOn(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo);
                }
                else
                {
                    ret = GUGAO_LIB.GTN_AxisOff(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo);
                }
                ThrowIfFeedbackError(ret, $"Set_Servo(bool on) error");
                this._interation.IsServoOn = on;

            } while (false);
        }

        public override bool Stop()
        {
            isStopReq = true;
            if (this._interation.IsSimulation)
            {
                return true;
            }
            else
            {
                var runMode = GetAxisRunMode();
                var ret = 0;
                switch (runMode)
                {
                    case RUNMODE_GUGAO.HOME:
                        {
                            ret = GUGAO_LIB.GTN_StopEcatHoming(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo);

                        }
                        break;
                    case RUNMODE_GUGAO.MOTION:
                        {
                            ret = GUGAO_LIB.GTN_Stop(this._setting.MotorTable.CardNo, GetMask(), (ushort)this._setting.MotorModes.Stop_Mode);
                            WaitMotionDone();
                        }
                        break;
                    default:
                        {
                            ret = GUGAO_LIB.GTN_Stop(this._setting.MotorTable.CardNo, GetMask(), (ushort)this._setting.MotorModes.Stop_Mode);
                            WaitMotionDone();
                        }
                        break;
                }
                if (ret != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public override int WaitHomeDone(CancellationTokenSource tokenSource)
        {
            int errorCode = ErrorCodes.NoError;
      
            ushort HomeDoneFlag = 3;
            if (this._interation.IsSimulation)
            {
                this.isMoving = false;
                return errorCode;
            }
            Thread.Sleep(10);  
            Stopwatch st = new Stopwatch();
            st.Start();
            ushort homeStatus = 0;
            do
            {
                var ret = GUGAO_LIB.GTN_GetEcatHomingStatus
                        (
                            this._setting.MotorTable.CardNo,
                            this._setting.MotorTable.AxisNo,
                            out homeStatus
                        );
                //命令执行状态异常 退出并回报home error
                if (ret != 0)
                {
                    errorCode = ErrorCodes.MotorHomingError;
                    break;
                }
                //若状态为回零成功标志 则返回no error并成功 homeDoneFlag = 3
                if (homeStatus == HomeDoneFlag)
                {
                    break;
                }
                if(homeStatus > HomeDoneFlag)
                {
                    errorCode = ErrorCodes.MotorHomingError;
                    break;
                }
                //若马达停止了并没有回零成功表示  返回home error
                //if (this._interation.IsMoving == false)
                //{
                //    Thread.Sleep(500);
                //    //停止运动后再获得一次实时状态
                //    ret = GUGAO_LIB.GTN_GetEcatHomingStatus
                //           (
                //               this._setting.MotorTable.CardNo,
                //               this._setting.MotorTable.AxisNo,
                //               out homeStatus
                //           );
                //    if (homeStatus == HomeDoneFlag)
                //    {
                //        break;
                //    }
                //    else
                //    {
                //        errorCode = ErrorCodes.MotorHomingError;
                //    }
                //    break;
                //}
                //若马达停止了并没有回零成功表示  返回home error
                if (tokenSource.IsCancellationRequested == true)
                {
                    errorCode = ErrorCodes.UserReqestStop;
                    break;
                }
                if (st.Elapsed.TotalMilliseconds > this._setting.MotorTable.MotionTimeOut)
                {
                    errorCode = ErrorCodes.MotorMoveTimeOutError;
                    break;
                }
            } while (true);
            return errorCode;
        }

        public override int WaitMotionDone()
        {
            int errorCode = ErrorCodes.NoError;
            DateTime st = DateTime.Now;

            if (this._interation.IsSimulation)
            {
                this.isMoving = false;
                return errorCode;
            }

            while (true)
            {
                TimeSpan ts = DateTime.Now - st;

                bool isDone = MotionDoneEvent.WaitOne(10);
                if (isDone) break;

                if (isStopReq)
                    break;


                if (ts.TotalMilliseconds > this._setting.MotorTable.MotionTimeOut)
                    return ErrorCodes.MotorMoveTimeOutError;
            }


            return errorCode;
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <returns></returns>
        public override bool HomeRun()
        {
            const int OFFSET = 0;
            const ushort PROBE_FUNC = 0;
            const short axisCount = 1;

            //设置速度参数
            float StartVel = Convert.ToSingle(this._setting.MotorSpeed.Home_Start_Velocity);
            float MaxVel = Convert.ToSingle(this._setting.MotorSpeed.Home_Max_Velocity);
            double Acc = this._setting.MotorSpeed.Home_Acceleration;
            double Dec = this._setting.MotorSpeed.Home_Deceleration;
            
            var ret = GUGAO_LIB.GTN_ClrSts(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, axisCount);
            if (ret != 0)
            {
                return false;
            }
            ret = GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.HOME);
            if (ret != 0)
            {
                return false;
            }
            ret = GUGAO_LIB.GTN_SetEcatHomingPrm(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, this._setting.MotorModes.Home_Mode, MaxVel, StartVel, Acc, OFFSET, PROBE_FUNC);
            if (ret != 0)
            {
                return false;
            }
            ret = GUGAO_LIB.GTN_StartEcatHoming(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo);
            if (ret != 0)
            {
                return false;
            }
            return true;
        }
 
        public override bool PhaseSearching(int timeout_s)
        {
            if (this._interation.IsSimulation)
            {
                return true;
            }
            if (this.MotorGeneralSetting.MotorTable.IsPhaseSearchNeeded)
            {
                const ushort succeededFlag = 2;
                const ushort phaseSignalAddress = 0x20A9;
                const byte subPhaseSignalAddress = 0;
                short rtn;
                uint result_size = 0, errorCode = 0;
                uint target_size = 2;
                byte[] data = new byte[4];
                ushort phaseFindStatus = 0;

                Stopwatch sw = new Stopwatch();
                sw.Start();
                //int count = 0;
                do
                {
                    rtn = GUGAO_LIB.GTN_EcatSDOUploadEx
                    (
                        this.MotorGeneralSetting.MotorTable.CardNo,
                        (ushort)this.MotorGeneralSetting.MotorTable.AxisNo,
                        phaseSignalAddress, //(ushort)0x20A9,
                        subPhaseSignalAddress,
                        ref data[0],
                        target_size,
                        ref result_size,
                        ref errorCode
                    );

                    phaseFindStatus = BitConverter.ToUInt16(data, 0);
                    Thread.Sleep(20);
 
                } while (phaseFindStatus != succeededFlag && sw.Elapsed.TotalSeconds < timeout_s);

                if (phaseFindStatus != succeededFlag)
                {
                    //MessageBox.Show("寻相失败 " + "\t" + phaseFindStatus + "\t" + Axisid + "\t" + (endTime - startTime).TotalSeconds + "\t");
                    return false;
                }
                else
                {
                    //Log.PrintInfo("寻相成功 " + "\t" + phaseFindStatus + "\t" + Axisid + "\t" + (endTime - startTime).TotalSeconds + "\t");
                }
            }
            else
            {
                Thread.Sleep(500);
            }
            return true;
        }


        public virtual ushort IsHomeReached(out bool IsNormal)
        {
            IsNormal = true;
            ushort usHomeSts = 0;
            var ret = GUGAO_LIB.GTN_GetEcatHomingStatus
                (
                    this._setting.MotorTable.CardNo,
                    this._setting.MotorTable.AxisNo,
                    out usHomeSts
                );
            if (ret != 0)
            {
                IsNormal = false;
            }
            return  usHomeSts;
        }
        public override bool SetToHomeMode()
        {
            var ret = GUGAO_LIB.GTN_SetHomingMode
               (
                   this.MotorGeneralSetting.MotorTable.CardNo,
                   this.MotorGeneralSetting.MotorTable.AxisNo,
                   (int)RUNMODE_GUGAO.HOME
               );
            if (ret != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override bool SetToRunMode()
        {
            var ret = GUGAO_LIB.GTN_SetHomingMode
                (
                    this.MotorGeneralSetting.MotorTable.CardNo,
                    this.MotorGeneralSetting.MotorTable.AxisNo,
                    (int)RUNMODE_GUGAO.MOTION
                );
            if (ret != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 原点复位后能够正常的清零和清除状态
        /// </summary>
        /// <returns></returns>
        public virtual bool HomeClearStaus()
        {
            const short axisCount = 1;
            var ret = GUGAO_LIB.GTN_SetHomingMode(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, (int)RUNMODE_GUGAO.MOTION); // 切换到位置控制模式
            if (ret != 0)
            {
                return false;
            }
            ret = GUGAO_LIB.GTN_ZeroPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, axisCount);
            if (ret != 0)
            {
                return false;
            }
            ret = GUGAO_LIB.GTN_ClrSts(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, axisCount);
            if (ret != 0)
            {
                return false;
            }
            return true;
        }
        public override bool SetCurrentPositionToZero()
        {
            var ret = GUGAO_LIB.GTN_ZeroPos(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo, 1);
            if (ret != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public virtual void HomeStop()
        {
                var runMode = GetAxisRunMode();

                switch (runMode)
                {
                    case RUNMODE_GUGAO.HOME:
                        {
                            GUGAO_LIB.GTN_StopEcatHoming(this._setting.MotorTable.CardNo, this._setting.MotorTable.AxisNo);
                            //WaitHomeDone();
                        }
                        break;
                    case RUNMODE_GUGAO.MOTION:
                        {
                            GUGAO_LIB.GTN_Stop(this._setting.MotorTable.CardNo, GetMask(), (ushort)this._setting.MotorModes.Stop_Mode);
                            WaitMotionDone();
                        }
                        break;
                    default:
                        {
                            GUGAO_LIB.GTN_Stop(this._setting.MotorTable.CardNo, GetMask(), (ushort)this._setting.MotorModes.Stop_Mode);
                            WaitMotionDone();
                        }
                        break;
                
            }
        }
    }
}