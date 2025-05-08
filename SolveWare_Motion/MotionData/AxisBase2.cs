using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_Business_Manager_Motion.Base
{
    public abstract class AxisBase2: IModel, ITool
    {
        #region ctor
        public AxisBase2(MtrTable2 mtrTable, MtrConfig2 mtrConfig, MtrSpeed2 mtrSpeed, bool isSimulate)
        {
            this.mtrTable = mtrTable;
            this.mtrConfig = mtrConfig;
            this.mtrSpeed = mtrSpeed;
            this.isSimulate = isSimulate;
            this.Name = mtrTable.Name;
        }
        #endregion

        protected MtrTable2 mtrTable;
        protected MtrConfig2 mtrConfig;
        protected MtrSpeed2 mtrSpeed;
        protected CancellationTokenSource readStatusSource;
        protected bool isSimulate;
        protected bool hasHome;
        protected AutoResetEvent cancelDoneFlag = new AutoResetEvent(false);

        protected bool isServoOn;
        protected bool isInPosition;
        protected bool isAlarm;
        protected bool isOrg;
        protected bool isPosLimit;
        protected bool isNegLimit;
        protected bool isProhibitActivated = false;
        protected bool isMoving;
        protected bool isStopReq = false;
        protected double currentPulse;
        protected double currentPhysicalPos;
        protected double analogInputValue;
        protected string interlockWaringMsg;

        public string Name { get; set; }
        public MtrTable2 MtrTable
        {
            get => mtrTable;
            set
            {
                mtrTable = value;
                this.Name = mtrTable.Name;
                OnPropertyChanged(nameof(MtrTable));
            }
        }
        public MtrConfig2 MtrConfig
        {
            get => mtrConfig;
            set => UpdateProper(ref mtrConfig, value);
        }
        public MtrSpeed2 MtrSpeed
        {
            get => mtrSpeed;
            set => UpdateProper(ref mtrSpeed, value);
        }

        public bool IsSimulation
        {
            get => isSimulate;
            private set => UpdateProper(ref isSimulate, value);
        }
        public bool IsServoOn
        {
            get
            {
                return this.isServoOn;
            }
            protected set
            {
                if (value != isServoOn)
                {
                    isServoOn = value;
                }
                OnPropertyChanged(nameof(IsServoOn));
            }
        }
        public bool IsInPosition
        {
            get => isInPosition;
            set => UpdateProper(ref isInPosition, value);
        }
        public bool IsAlarm
        {
            get => isAlarm;
            set => UpdateProper(ref isAlarm, value);
        }
        public bool IsOrg
        {
            get => isOrg;
            set => UpdateProper(ref isOrg, value);
        }
        public bool IsPosLimit
        {
            get => isPosLimit;
            set => UpdateProper(ref isPosLimit, value);
        }
        public bool IsNegLimit
        {
            get => isNegLimit;
            set => UpdateProper(ref isNegLimit, value);
        }
        public bool HasHome
        {
            get => hasHome;
            private set => UpdateProper(ref hasHome, value);
        }
        public bool IsMoving
        {
            get => isMoving;
            protected set => UpdateProper(ref isMoving, value);
        }
        public double CurrentPulse
        {
            get => currentPulse;
            set => UpdateProper(ref currentPulse, value);
        }
        public double CurrentPhysicalPos
        {
            get => currentPhysicalPos;
            set => UpdateProper(ref currentPhysicalPos, value);
        }
        public double AnalogInputValue
        {
            get => analogInputValue;
            set => UpdateProper(ref analogInputValue, value);
        }
        public string InterlockWaringMsg
        {
            get => interlockWaringMsg;
            set => UpdateProper(ref interlockWaringMsg, value);
        }

        //公用部份
        public virtual void StartStatusReading()
        {
            if (readStatusSource != null) return;
            readStatusSource = new CancellationTokenSource();
            Task task = new Task(() =>
            {
                while (!readStatusSource.IsCancellationRequested)
                {
                    IsOrg = Get_Origin_Signal();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    IsAlarm = Get_Alarm_Signal();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    IsPosLimit = Get_PEL_Signal();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    IsNegLimit = Get_NEL_Signal();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    IsInPosition = Get_InPos_Signal();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    if(!IsSimulation) IsMoving = IsMotorMoving();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    CurrentPulse = Get_CurPulse();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    CurrentPhysicalPos = Get_CurUnitPos();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    AnalogInputValue = Get_AnalogInputValue();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                    IsServoOn = Get_ServoStatus();
                    Thread.Sleep(mtrTable.StatusReadTiming);
                }

                cancelDoneFlag.Set();

            }, readStatusSource.Token, TaskCreationOptions.LongRunning);
            task.Start();
        }
        public void StopStatusReading()
        {
            if (readStatusSource == null) return;
            readStatusSource.Cancel();
            cancelDoneFlag.WaitOne();
            readStatusSource = null;
        }
        public bool IsProhibitToHome()
        {
            string sErr = string.Empty;
            this.isProhibitActivated = false;
            bool result = false;

            if (mtrTable.pIsInhibitToHome == null) return false;

            if (mtrTable.pIsInhibitToHome(ref sErr))
            {
                this.isProhibitActivated = true;
                this.InterlockWaringMsg = sErr;
                result = true;
            }

            return result;
        }
        public bool IsProhibitToMove()
        {
            this.InterlockWaringMsg = "";
            this.isProhibitActivated = false;
            bool result = false;
            string sErr = string.Empty;

            if (!IsSimulation && IsMoving) return true;

            if (mtrTable.pIsInhibitToMove == null) return false;
            if (mtrTable.pIsInhibitToMove(ref sErr))
            {
                this.isProhibitActivated = true;
                this.InterlockWaringMsg = sErr;
                return true;
            }

            return result;
        }     
        public int InPositionCheck(double targetPos)
        {
            int errorCode = ErrorCodes.NoError;
            double curPos = IsSimulation ? MtrTable.CurPos : Get_CurUnitPos();
            double realOffset = Math.Abs(curPos) - Math.Abs(targetPos);

            errorCode = Math.Abs(realOffset) > mtrTable.AcceptableInPositionOffset ?
                        ErrorCodes.MotorNotReachToPos: ErrorCodes.NoError;

            return errorCode;           
        }
        public void SetHomeDone(bool homedone)
        {
            this.HasHome = homedone;
        }

        //重写部份
        public abstract bool Init();
        public abstract bool Get_PEL_Signal();
        public abstract bool Get_NEL_Signal();
        public abstract bool Get_InPos_Signal();
        public abstract bool Get_Alarm_Signal();
        public abstract bool Get_Origin_Signal();
        public abstract bool IsMotorMoving();
        public abstract double Get_CurPulse();
        public abstract double Get_CurUnitPos();
        public abstract double Get_AnalogInputValue();
        public abstract bool Get_ServoStatus();
        public abstract int MoveTo(double pos, SpeedType speedType, float slowFactor = 1);
        public abstract int WaitMotionDone();
        public abstract int WaitHomeDone();
        public abstract void Stop();
        public abstract int HomeMove();
        public abstract void Jog(bool isPositive);
        public abstract int Get_IO_sts();

        public abstract void Set_Servo(bool on);

        public void ConverToAutoMMPerSec(ref float startVel, ref float maxVel, ref double acc, ref double dec)
        {
            double unitPerSec = MtrTable.PulsePerRevolution / MtrTable.UnitPerRevolution;
            double acc_Unit = MtrSpeed.Auto_Acceleration * unitPerSec;
            double dec_Unit = MtrSpeed.Auto_Deceleration * unitPerSec;

            startVel = (float)(unitPerSec * MtrSpeed.Auto_Start_Velocity);
            maxVel = (float)(unitPerSec * MtrSpeed.Auto_Max_Velocity);


            double factor = maxVel == startVel ? 1 : maxVel - startVel;
            acc = factor / acc_Unit;
            dec = factor / dec_Unit;
        }
        public void ConverToHomeMMPerSec(ref float startVel, ref float maxVel, ref double acc, ref double dec)
        {
            double unitPerSec = MtrTable.PulsePerRevolution / MtrTable.UnitPerRevolution;
            double acc_Unit = MtrSpeed.Home_Acceleration * unitPerSec;
            double dec_Unit = MtrSpeed.Home_Deceleration * unitPerSec;

            startVel = (float)(unitPerSec * MtrSpeed.Home_Start_Velocity);
            maxVel = (float)(unitPerSec * MtrSpeed.Home_Max_Velocity);


            double factor = maxVel == startVel ? 1 : maxVel - startVel;
            acc = factor / acc_Unit;
            dec = factor / dec_Unit;
        }
        public void ConverToJogMMPerSec(ref float startVel, ref float maxVel, ref double acc, ref double dec)
        {
            double unitPerSec = MtrTable.PulsePerRevolution / MtrTable.UnitPerRevolution;
            double acc_Unit = MtrSpeed.Jog_Acceleration * unitPerSec;
            double dec_Unit = MtrSpeed.Jog_Deceleration * unitPerSec;

            startVel = (float)(unitPerSec * MtrSpeed.Jog_Start_Velocity);
            maxVel = (float)(unitPerSec * MtrSpeed.Jog_Max_Velocity);


            double factor = maxVel == startVel ? 1 : maxVel - startVel;
            acc = factor / acc_Unit;
            dec = factor / dec_Unit;
        }

   
        protected void ConvertSpeedMMPerSec(SpeedType sType, ref float startVel, ref float maxVel, ref double acc, ref double dec)
        {
            switch (sType)
            {
                case SpeedType.Home:
                    ConverToHomeMMPerSec(ref startVel, ref maxVel, ref acc, ref dec);
                    break;
                case SpeedType.Auto:
                    ConverToAutoMMPerSec(ref startVel, ref maxVel, ref acc, ref dec);
                    break;
                case SpeedType.Jog:
                    ConverToJogMMPerSec(ref startVel, ref maxVel, ref acc, ref dec);
                    break;
            }
        }
        public double FormulaCalc_AngleToUnit(double Angle)
        {
            //"InsideAngDeg=180-(-1*Angle);
            /* 电机旋转角度转三角形内角 */
            //AngArc=InsideAngDeg*Math.PI/180;
            /* 三角形内角转弧度 */
            //L1=2.5;  
            /*旋转臂长度*/
            //L2 =55; /*连杆臂长度*/
            //InitLen=L2-L1;  
            /* 初始连杆长度,归零用 */
            /*核心计算函数*/
            //a =1; b=-2*L1*Math.cos(AngArc);
            //c =L1*L1-L2*L2;\nDistance=(-b+Math.sqrt(b*b-4*a*c))/2-InitLen; 
            /*算出位置*/
            //-1*Distance  /* 向下运动为负数 */"
            Angle *= -1;
            double InsideAngDeg = 180 - (-1 * Angle);
            double AngArc = InsideAngDeg * Math.PI / 180;
            double L1 = 2.5;
            double L2 = 55;
            double InitLen = L2 - L1;
            double a = 1;
            double b = -2 * L1 * Math.Cos(AngArc);
            double c = L1 * L1 - L2 * L2;
            double Distance = (-b + Math.Sqrt(b * b - 4 * a * c)) / 2 - InitLen;

            double reulst = Distance * 1;
            return reulst;

        }
        public double FormulaCalc_UnitToAngle(double Unit)
        {

            //"Distance=-1*Unit 
            /* 向下运动为负数 */
            //L1 =2.5;  
            /*旋转臂长度*/
            //L2 =55; 
            /*连杆臂长度*/
            //InitLen =L2-L1;  
            /* 初始连杆长度,归零用 */
            //L3 =Distance+InitLen;  
            /*三角形可变边长*/
            //if (Unit>=0)
            //{0}v
            //else if(L3>L1+L2){-180}
            //else{/*核心计算函数 计算出三角形内角 */
            //InsideAngArc =Math.acos((L2*L2-L1*L1-L3*L3)/(-2*L1*L3));
            //InsideAngDeg = InsideAngArc * 180 / Math.PI;/* 
            //三角形内角转角度 */(-1)*(180-InsideAngDeg); /*算出角度*/\n}"

            Unit *= -1;
            double Distance = -1 * Unit;
            double L1 = 2.5;
            double L2 = 55;
            double InitLen = L2 - L1;
            double L3 = Distance + InitLen;
            double InsideAngArc = 0;
            double InsideAngDeg = 0;

            if (Unit >= 0)
            {
                return 0;
            }
            else if (L3 > L1 + L2)
            {
                return -180;
            }
            else
            {
                InsideAngArc = Math.Acos((L2 * L2 - L1 * L1 - L3 * L3) / (-2 * L1 * L3));
                InsideAngDeg = InsideAngArc * 180 / Math.PI;
            }
            double result = (1) * (180 - InsideAngDeg);
            return result;
        }
        public bool IsMoveTimeOut(DateTime st)
        {
            TimeSpan ts = DateTime.Now - st;
            return ts.TotalMilliseconds > mtrTable.MotionTimeOut;
        }
        public bool IsHomeTimeOut(DateTime st)
        {
            TimeSpan ts = DateTime.Now - st;
            return ts.TotalMilliseconds > mtrTable.HomeTimeOut;
        }

    }
}
