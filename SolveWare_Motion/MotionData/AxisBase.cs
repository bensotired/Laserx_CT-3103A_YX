using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using SolveWare_Business_Manager_Motion.Base;

namespace SolveWare_Business_Motion.Base
{
    public abstract class AxisBase : DispatcherObject, INotifyPropertyChanged, IDisposable
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        #region ctor
        public AxisBase(ref MtrConfig mtrConfig, ref MtrTable mtrTable, ref MtrSpeed mtrSpeed, bool simulation)
        {
            this.MtrTable = mtrTable;
            this.MtrSpeed = mtrSpeed;
            this.MtrConfig = mtrConfig;
            this.MtrMisc = null;
            this.IsSimulation = simulation;
            AxisBase.MotorsList.Add(this);
        }
        public AxisBase(ref MtrConfig mtrConfig, ref MtrTable mtrTable, ref MtrSpeed mtrSpeed, ref MtrMisc mtrMisc, bool simulation)
        {
            this.MtrTable = mtrTable;
            this.MtrSpeed = mtrSpeed;
            this.MtrConfig = mtrConfig;
            this.MtrMisc = mtrMisc;
            this.IsSimulation = simulation;
            AxisBase.MotorsList.Add(this);
        }
        #endregion

        protected MtrTable mtrTable;
        protected MtrSpeed mtrSpeed;
        protected MtrConfig mtrConfig;
        protected MtrMisc mtrMisc = null;
        protected MotionDel _inhibitThread = null;
        protected Thread thread = null;
        protected Thread customHomingThrd = null;
        protected Thread readMotorStatusThrd = null;
        protected List<PosMap> tPosMap = new List<PosMap>();
        public static Dictionary<string, double> LastUseStepDict = new Dictionary<string, double>();
        protected MotionStatus customHomingFlag = MotionStatus.MT_DONE;


        protected string servoOnCaption = "Enable";
        protected int mapSize = 300;
        protected int analogInputValue = 0;
        protected int NumberOfAxisPerCard;
        protected float lastUsedSpeedPercent = 0f;
        protected double currentPhysicalPos = 0.0;
        protected double currentPulse = 0.0;
        protected double tempPositionOffset = 0.0;
        protected double statusReadingTime = 0.0;
        protected double targetPos = 0.0;


        protected bool disableMotionInterlock = false;
        protected bool endInhibitThread = false;
        protected bool isAlarm = false;
        protected bool isBusy = false;
        protected bool isCustomHomingStarted = false;
        protected bool inhibit = false;
        protected bool isInPosition = false;
        protected bool isNegLimit = false;
        protected bool isOrg = false;
        protected bool isPosLimit = false;
        protected bool isStandby = false;
        protected bool isServoOn = false;
        protected bool isStopReq = false;
        protected bool isShowPulse = true;
        protected bool isStatusReading = false;
        protected bool isSimulation = true;
        protected bool isForceStop = false;
        protected bool isHoming = false;
        protected bool isProhibitActivated = false;

        protected volatile bool endThread = false;
        protected volatile bool endreadMotorStatusThrd = false;


        protected double desiredPos = 0.0;
        protected static double disableInterLockAllowDistance = 10.0;
        protected string sErr = "";
        protected string strErr = "";
        protected string interlockWaringMsg = "";
        protected MtrMotionProfile longDistanceProf = null;
        protected DateTime commmandStartTime = DateTime.Now;
        protected AxisBase.LastMoveDirection lastDirection = AxisBase.LastMoveDirection.Unknown;
        protected IMotionProfile lastMotionProfile = null;


        //protected bool hasHome = false;
        //public bool HasHome
        //{
        //    get
        //    {
        //        return this.hasHome;
        //    }
        //    protected set
        //    {
        //        if (this.hasHome != value)
        //        {
        //            this.hasHome = value;
        //            this.OnPropertyChanged("HasHome");
        //            this.OnPropertyChanged("CanMotorMove");
        //        }
        //    }
        //}

        public MtrTable MtrTable
        {
            get
            {
                return this.mtrTable;
            }
            private set
            {
                this.mtrTable = value;
                if (value != null)
                {
                    this.mtrTable.SetAxisID(this.AxisID);
                }
            }
        }
        public MtrSpeed MtrSpeed
        {
            get
            {
                return this.mtrSpeed;
            }
            private set
            {
                this.mtrSpeed = value;
            }
        }
        public MtrConfig MtrConfig
        {
            get
            {
                return this.mtrConfig;
            }
            private set
            {
                this.mtrConfig = value;
            }
        }
        public MtrMisc MtrMisc
        {
            get
            {
                return this.mtrMisc;
            }
            private set
            {
                this.mtrMisc = value;
            }
        }
        public AxisBase.InitDelegate initDel = null;
        public static ObservableCollection<AxisBase> MotorsList = new ObservableCollection<AxisBase>();

        public double StepPermm = 1.0;
        public double mmPerStep = 1.0;
        public double MaxStep = 0.0;
        public double MinStep = 0.0;
        public double HmRevStep = 0.0;
        public bool BypassReadyCheck = false;
        public bool BypassAlarmCheck = false;
        public bool BypassLimitCheck = false;
        public bool DisableHomingInterlock = false;
        public string AxisID
        {
            get
            {
                return string.Concat(new string[]
                {
                    base.GetType().Name,
                    "[",
                    this.MtrTable.CardNo.ToString(),
                    "][",
                    this.MtrTable.AxisNo.ToString(),
                    "]"
                });
            }
        }

        public float LastUsedSpeedPercent
        {
            get
            {
                return this.lastUsedSpeedPercent;
            }
            set
            {
                this.lastUsedSpeedPercent = value;
                this.OnPropertyChanged("LastUsedSpeedPercent");
            }
        }
        public double TargetPos
        {
            get
            {
                return this.targetPos;
            }
            set
            {
                this.targetPos = value;
                this.OnPropertyChanged("TargetPos");
            }
        }

        public bool IsProhibitActivated
        {
            get
            {
                return this.isProhibitActivated;
            }
        }
        public bool IsSimulation
        {
            get
            {
                return this.isSimulation;
            }
            set
            {
                if (this.isSimulation != value)
                {
                    this.isSimulation = value;
                    this.OnPropertyChanged("Simulation");
                }
            }
        }
        public bool IsShowPulse
        {
            get
            {
                return this.isShowPulse;
            }
            set
            {
                this.isShowPulse = value;
                this.OnPropertyChanged("IsShowPulse");
            }
        }
        public bool DisableMotionInterlock
        {
            get
            {
                return this.disableMotionInterlock;
            }
            set
            {
                this.disableMotionInterlock = value;
                this.OnPropertyChanged("DisableMotionInterlock");
            }
        }

        public void ResetTargetPos()
        {
            this.TargetPos = this.get_mm(this.currentPulse);
        }
        public void KeepLastUseStepSize(double step)
        {
            if (step >= 0.0)
            {
                if (AxisBase.LastUseStepDict.ContainsKey(this.AxisID))
                {
                    AxisBase.LastUseStepDict[this.AxisID] = step;
                }
                else
                {
                    AxisBase.LastUseStepDict.Add(this.AxisID, step);
                }
            }
        }
        public double GetLastUseStepSize()
        {
            double num = this.mtrTable.StepSize;
            if (this.mtrTable.KeepLastUseStepSize)
            {
                if (AxisBase.LastUseStepDict.ContainsKey(this.AxisID))
                {
                    num = AxisBase.LastUseStepDict[this.AxisID];
                }
                if (num == 0.0)
                {
                    num = this.mtrTable.StepSize;
                }
            }
            if (num <= 0.0)
            {
                num = 1.0;
            }
            return num;
        }
        public bool Init()
        {
            this.StepPermm = this.MtrTable.StepPerRevolution / this.MtrTable.mmPerRevolution;
            this.mmPerStep = this.MtrTable.mmPerRevolution / this.MtrTable.StepPerRevolution;
            this.MaxStep = this.MtrTable.Maxmm * this.StepPermm;
            this.MinStep = this.MtrTable.Minmm * this.StepPermm;
            this.HmRevStep = this.MtrTable.HmRevmm * this.StepPermm;
            return this.isSimulation || this.initialization((int)this.MtrTable.CardNo, false);
        }
        public void UpdateMtrEntry()
        {
            this.StepPermm = this.MtrTable.StepPerRevolution / this.MtrTable.mmPerRevolution;
            this.mmPerStep = this.MtrTable.mmPerRevolution / this.MtrTable.StepPerRevolution;
            this.MaxStep = this.MtrTable.Maxmm * this.StepPermm;
            this.MinStep = this.MtrTable.Minmm * this.StepPermm;
            this.HmRevStep = this.MtrTable.HmRevmm * this.StepPermm;
        }
        public static string InitAllMotors()
        {
            string text = "";
            for (int i = 0; i < AxisBase.MotorsList.Count; i++)
            {
                if (!AxisBase.MotorsList[i].Init())
                {
                    text = string.Format("{0} {1} [{2}] card=[{3}] axis[{4}]", new object[]
                    {
                        "Init error at ",
                        AxisBase.MotorsList[i].MtrTable.Name,
                        AxisBase.MotorsList[i].GetType().ToString(),
                        AxisBase.MotorsList[i].MtrTable.CardNo,
                        AxisBase.MotorsList[i].MtrTable.AxisNo
                    });
                    break;
                }
            }
            if (text == "")
            {
                for (int i = 0; i < AxisBase.MotorsList.Count; i++)
                {
                    AxisBase.MotorsList[i].StartStatusReading();
                }
            }
            return text;
        }

        public void StartStatusReading()
        {
            if (this.readMotorStatusThrd == null)
            {
                this.endreadMotorStatusThrd = false;
                this.readMotorStatusThrd = new Thread(new ThreadStart(this.StatusReadOperation));
                this.readMotorStatusThrd.IsBackground = true;
                this.readMotorStatusThrd.Start();
            }
        }


        private void StatusReadOperation()
        {
            this.IsStatusReading = true;
            try
            {
                int num = this.mtrTable.StatusReadTiming;
                if (num < 300)
                {
                    num = 300;
                }
                else if (num > 2000)
                {
                    num = 2000;
                }
                DateTime now = DateTime.Now;
                TimeSpan timeSpan = DateTime.Now - now;
                while (!this.endreadMotorStatusThrd)
                {
                    now = DateTime.Now;
                    this.IsInPosition = this.get_inpos_signal();
                    Thread.Sleep(5);
                    this.IsAlarm = this.get_alarm_signal();
                    Thread.Sleep(5);
                    this.IsOrg = this.get_origin_signal();
                    Thread.Sleep(5);
                    this.IsPosLimit = this.get_pel_signal();
                    Thread.Sleep(5);
                    this.IsNegLimit = this.get_nel_signal();
                    Thread.Sleep(5);
                    this.CurrentPulse = this.get_current_pos();
                    this.CurrentPhysicalPos = this.get_mm(this.currentPulse);
                    Thread.Sleep(5);
                    this.AnalogInputValue = this.GetAnalogValue();
                    Thread.Sleep(5);
                    this.ReadServoStatus();
                    this.StatusReadingTime = (DateTime.Now - now).TotalMilliseconds;


                    now = DateTime.Now;
                    timeSpan = DateTime.Now - now;
                    while (timeSpan.TotalMilliseconds < (double)num)
                    {
                        if (this.endreadMotorStatusThrd)
                        {
                            break;
                        }
                        timeSpan = DateTime.Now - now;
                        Thread.Sleep(5);     //关闭程序这里无法退出
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.IsStatusReading = false;
            }
        }

        public double TempPositionOffset
        {
            get
            {
                return this.tempPositionOffset;
            }
            set
            {
                if (value != this.tempPositionOffset)
                {
                    this.tempPositionOffset = value;
                    this.OnPropertyChanged("TempPositionOffset");
                    this.OnPropertyChanged("TempOffsetStr");
                }
            }
        }

        public string TempOffsetStr
        {
            get
            {
                return "Biased Offset=" + this.tempPositionOffset.ToString("F4") + " " + this.mtrMisc.UnitName;
            }
        }

        public void SetInhibit(bool set)
        {
            this.inhibit = set;
        }

        public double get_mm(double step)
        {
            double factor = this.MtrTable.mmPerRevolution / this.MtrTable.StepPerRevolution;
            return step * factor;
        }

        public double get_step(double mm)
        {

            double factor = this.MtrTable.StepPerRevolution / this.MtrTable.mmPerRevolution;
            return mm * factor / this.MtrTable.PulseFactor;//this.StepPermm;
        }

        public double CurrentPhysicalPos
        {
            get
            {
                return this.currentPhysicalPos;
            }
            protected set
            {
                if (value != this.currentPhysicalPos)
                {
                    this.currentPhysicalPos = value;
                    this.OnPropertyChanged("CurrentPhysicalPos");
                }
            }
        }

        public double CurrentPulse
        {
            get
            {
                return this.currentPulse;
            }
            protected set
            {
                if (value != this.currentPulse)
                {
                    this.currentPulse = value;
                    this.OnPropertyChanged("CurrentPulse");
                }
            }
        }

        public bool IsOrg
        {
            get
            {
                return this.isOrg;
            }
            protected set
            {
                if (value != this.isOrg)
                {
                    this.isOrg = value;
                    this.OnPropertyChanged("IsOrg");
                }
            }
        }

        public bool IsInPosition
        {
            get
            {
                return this.isInPosition;
            }
            protected set
            {
                if (value != this.isInPosition)
                {
                    this.isInPosition = value;
                    this.OnPropertyChanged("IsInPosition");
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            protected set
            {
                if (value != this.isBusy)
                {
                    this.isBusy = value;
                    this.OnPropertyChanged("IsBusy");
                    this.OnPropertyChanged("CanMotorMove");
                }
            }
        }

        public string ServoOnCaption
        {
            get
            {
                return this.servoOnCaption;
            }
            protected set
            {
                this.servoOnCaption = value;
                this.OnPropertyChanged("ServoOnCaption");
            }
        }

        public bool IsAlarm
        {
            get
            {
                return this.isAlarm;
            }
            protected set
            {
                if (value != this.isAlarm)
                {
                    this.isAlarm = value;
                    this.OnPropertyChanged("IsAlarm");
                    this.OnPropertyChanged("CanMotorMove");
                }
            }
        }

        public bool IsPosLimit
        {
            get
            {
                return this.isPosLimit;
            }
            protected set
            {
                if (value != this.isPosLimit)
                {
                    this.isPosLimit = value;
                    this.OnPropertyChanged("IsPosLimit");
                }
            }
        }

        public bool IsNegLimit
        {
            get
            {
                return this.isNegLimit;
            }
            protected set
            {
                if (value != this.isNegLimit)
                {
                    this.isNegLimit = value;
                    this.OnPropertyChanged("IsNegLimit");
                }
            }
        }

        public double StatusReadingTime
        {
            get
            {
                return this.statusReadingTime;
            }
            set
            {
                this.statusReadingTime = value;
                this.OnPropertyChanged("StatusReadingTime");
            }
        }

        public bool IsStatusReading
        {
            get
            {
                return this.isStatusReading;
            }
            protected set
            {
                if (value != this.isStatusReading)
                {
                    this.isStatusReading = value;
                    this.OnPropertyChanged("IsStatusReading");
                }
            }
        }

        public bool CanMotorMove
        {
            get
            {
                return !this.isBusy && !this.isAlarm;
                //return !this.isBusy && !this.isAlarm && this.hasHome;
            }
        }

      

        public bool IsServoOn
        {
            get
            {
                return this.isServoOn;
            }
            protected set
            {
                if (value != this.isServoOn)
                {
                    this.isServoOn = value;
                    this.ServoOnCaption = (value ? "Disable" : "Enable");
                    this.OnPropertyChanged("IsServoOn");
                    this.OnPropertyChanged("CanMotorMove");
                }
            }
        }

        protected virtual void ReadServoStatus()
        {
        }

        public double GetCommandPos(double desiredPos)
        {
            double result = desiredPos;
            double num = 20000.0;
            int num2 = -1;
            for (int i = 0; i < this.tPosMap.Count; i++)
            {
                double num3 = Math.Abs(this.tPosMap[i].TargetPos - desiredPos);
                if (num3 >= num)
                {
                    num2 = i - 1;
                    break;
                }
                num2 = i;
                num = num3;
            }
            if (num2 >= 0 && num2 < this.tPosMap.Count && num < 0.5)
            {
                result = this.tPosMap[num2].CommandPos;
            }
            return result;
        }

        public void RegisterPosMap(double commadPos, double desiredPos)
        {
            bool flag = false;
            PosMap item = new PosMap
            {
                TargetPos = desiredPos,
                CommandPos = commadPos
            };
            for (int i = 0; i < this.tPosMap.Count; i++)
            {
                if (Math.Abs(this.tPosMap[i].TargetPos - desiredPos) < 0.01)
                {
                    this.tPosMap[i].CommandPos = commadPos;
                    flag = true;
                    break;
                }
                if (this.tPosMap[i].TargetPos > desiredPos && this.tPosMap.Count < this.mapSize)
                {
                    flag = true;
                    this.tPosMap.Insert(i, item);
                    break;
                }
            }
            if (!flag && this.tPosMap.Count < this.mapSize)
            {
                this.tPosMap.Add(item);
            }
        }

        private int TryCloseWithEncoderAxis(int motionTimeOut)
        {
            int num = 0;
            int num2 = 0;
            double num3 = this.GetCommandPos(this.desiredPos);
            double num4 = this.desiredPos;
            for (int i = 0; i < this.MtrTable.EncoderAxisCloseTryCount; i++)
            {
                num = this.wait_motion_done(motionTimeOut);
                if (num != 0)
                {
                    break;
                }
                Thread.Sleep(1);
                double current_pos_main_axis_f = this.get_current_pos_main_axis_f();
                double current_pos_f = this.get_current_pos_f();
                double num5 = current_pos_f / current_pos_main_axis_f;
                double num6 = num4 - current_pos_f;
                if (Math.Abs(num6) < this.MtrTable.InpositionRadius)
                {
                    num2++;
                }
                else
                {
                    num2 = 0;
                }
                if (num2 > 2)
                {
                    break;
                }
                num3 += num5 * num6;
                this.MoveTo(num3, this.lastMotionProfile, 1f, true);
            }
            if (num == 0)
            {
                double num6 = num4 - this.get_current_pos_f();
                num = ((Math.Abs(num6) < this.MtrTable.InpositionRadius) ? 0 : 11);
                if (num == 0)
                {
                    this.RegisterPosMap(num3, num4);
                }
            }
            return num;
        }

        public int wait_motion_done()
        {
            int result;
            if (this.MtrTable.EncoderAxisNo >= 0 && this.MtrTable.CloseLoopWithEncoderAxis)
            {
                result = this.TryCloseWithEncoderAxis(this.MtrTable.MotionTimeOut);
            }
            else
            {
                result = this.wait_motion_done(this.MtrTable.MotionTimeOut);
            }
            return result;
        }

        public string DirectionStr
        {
            get
            {
                string result;
                if (this.lastDirection == AxisBase.LastMoveDirection.Unknown)
                {
                    result = "NA";
                }
                else
                {
                    result = ((this.lastDirection == AxisBase.LastMoveDirection.Forward) ? "Fw" : "Bw");
                }
                return result;
            }
        }

        public int AnalogInputValue
        {
            get
            {
                return this.analogInputValue;
            }
            set
            {
                this.analogInputValue = value;
                this.OnPropertyChanged("AnalogInputValue");
            }
        }

        public AxisBase.LastMoveDirection LastDirection
        {
            get
            {
                return this.lastDirection;
            }
            protected set
            {
                this.lastDirection = value;
                this.OnPropertyChanged("LastDirection");
                this.OnPropertyChanged("DirectionStr");
            }
        }

        public int wait_home_done()
        {
            return this.wait_home_done(this.MtrTable.HomeTimeOut);
        }

        public void start_ta_move_f(double pos, double strvel, double maxvel)
        {
            this.start_ta_move(this.get_step(pos), strvel, maxvel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec);
        }

        public void start_ta_move_f(double pos, double strvel, double maxvel, double tacc, double tdec)
        {
            this.start_ta_move(this.get_step(pos), strvel, maxvel, tacc, tdec);
        }

        public void start_ta_move_f(double pos)
        {
            this.start_ta_move(this.get_step(pos), this.MtrSpeed.StrVel, this.MtrSpeed.MaxVel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec);
        }

        public void AssignLongDistanceProf(MtrMotionProfile longDistanceProf)
        {
            this.longDistanceProf = longDistanceProf;
        }

        public MtrMotionProfile TryGetLongDistanceProf(double pos)
        {
            if (this.longDistanceProf != null)
            {
                double current_pos_f = this.get_current_pos_f();
                double value = pos - current_pos_f;
                if (Math.Abs(value) > this.MtrSpeed.LongDistanceThreshold)
                {
                    return this.longDistanceProf;
                }
            }
            return null;
        }

        private void ApplyBacklashCompensation(ref double pos, double currentPos)
        {
            if (this.mtrTable.EnableBackLashCompensation)
            {
                if (this.lastDirection != AxisBase.LastMoveDirection.Unknown)
                {
                    AxisBase.LastMoveDirection lastMoveDirection = (currentPos < pos) ? AxisBase.LastMoveDirection.Forward : AxisBase.LastMoveDirection.Backward;
                    if (lastMoveDirection != this.lastDirection)
                    {
                        if (this.mtrTable.BackLashCompenstationForworkAddUp != 0f)
                        {
                            double num = (double)((lastMoveDirection == AxisBase.LastMoveDirection.Forward) ? this.mtrTable.BackLashCompenstationForworkAddUp : (-(double)this.mtrTable.BackLashCompenstationBackwardAddUp));
                            pos += num;
                        }
                    }
                }
            }
        }

        public void MoveTo(double pos)
        {
            this.MoveTo(pos, false);
        }

        private void MoveTo(double pos, bool trunOffCloseLoopGuessing = false)
        {
            this.lastMotionProfile = null;
            double current_pos_f = this.get_current_pos_f();
            this.ApplyBacklashCompensation(ref pos, current_pos_f);
            this.desiredPos = pos;
            if (!trunOffCloseLoopGuessing && this.MtrTable.CloseLoopWithEncoderAxis && this.MtrTable.EncoderAxisNo >= 0)
            {
                pos = this.GetCommandPos(pos);
            }
            this.start_ta_move_f(pos);
            if (this.interlockWaringMsg == "")
            {
                this.LastDirection = ((current_pos_f < pos) ? AxisBase.LastMoveDirection.Forward : AxisBase.LastMoveDirection.Backward);
            }
        }

        public void MoveTo(double pos, IMotionProfile pf, float speedFactor = 1f)
        {
            this.MoveTo(pos, pf, speedFactor, false);
        }

        private void MoveTo(double pos, IMotionProfile pf, float speedFactor = 1f, bool trunOffCloseLoopGuessing = false)
        {
            if (pf == null)
            {
                this.MoveTo(pos);
            }
            else
            {
                if (speedFactor < 0f)
                {
                    speedFactor = 0.01f;
                }
                if (speedFactor > 1f)
                {
                    speedFactor = 1f;
                }
                double current_pos_f = this.get_current_pos_f();
                this.ApplyBacklashCompensation(ref pos, current_pos_f);
                this.desiredPos = pos;
                if (!trunOffCloseLoopGuessing && this.MtrTable.CloseLoopWithEncoderAxis && this.MtrTable.EncoderAxisNo >= 0)
                {
                    pos = this.GetCommandPos(pos);
                }
                float startVel = pf.StartVel;
                float maxVel = pf.MaxVel;
                double acc = pf.Acc;
                double dec = pf.Dec;
                double jerk = pf.Jerk;
                pos *= this.MtrTable.PulseFactor;
                if (pf is MtrMotionProfile)
                {
                    MtrMotionProfile mtrMotionProfile = pf as MtrMotionProfile;
                    IMotionProfile distanceProf = mtrMotionProfile.GetDistanceProf(pos - current_pos_f);
                    if (distanceProf != null)
                    {
                        startVel = distanceProf.StartVel;
                        maxVel = distanceProf.MaxVel;
                        acc = distanceProf.Acc;
                        dec = distanceProf.Dec;
                        jerk = distanceProf.Jerk;
                    }
                }
                this.lastMotionProfile = pf;
                if (pf.IsRequiredJerk)
                {
                    this.start_ta_move(this.get_step(pos), (double)(startVel * speedFactor), (double)(maxVel * speedFactor), acc * (double)speedFactor, dec * (double)speedFactor, jerk);
                }
                else
                {
                    this.start_ta_move(this.get_step(pos), (double)(startVel * speedFactor), (double)(maxVel * speedFactor), acc, pf.Dec);
                }
                if (this.interlockWaringMsg == "")
                {
                    this.LastDirection = ((current_pos_f < pos) ? AxisBase.LastMoveDirection.Forward : AxisBase.LastMoveDirection.Backward);
                }
            }
        }

        public void MoveLineBy(double distance1, double distance2, double startVel, double maxVel, double acc, double dec, AxisBase secondaryAxis)
        {
            double num = this.get_current_pos_f() + distance1;
            double num2 = secondaryAxis.get_current_pos_f() + distance2;
            double num3 = this.get_step(num);
            double num4 = secondaryAxis.get_step(num2);
            this.start_ta_move_xy_line_to_pos(secondaryAxis, num, num2, startVel, maxVel, acc, dec);
        }

        public void MoveLineTo(double pos1, double pos2, double startVel, double maxVel, double acc, double dec, AxisBase secondaryAxis)
        {
            double posX = this.get_step(pos1);
            double posY = secondaryAxis.get_step(pos2);
            this.start_ta_move_xy_line_to_pos(secondaryAxis, posX, posY, startVel, maxVel, acc, dec);
        }

        public void MoveBy(double distance)
        {
            this.ApplyMinDistanceRoundOff(ref distance);
            double pos = this.get_current_pos_f() + distance;
            this.MoveTo(pos);
        }

        private void ApplyMinDistanceRoundOff(ref double distance)
        {
            if (this.MtrTable.RoundOffNearestMinStepSize)
            {
                double num = (double)((distance < 0.0) ? -1 : 1);
                if (Math.Abs(distance) < this.MtrTable.MinStepSize)
                {
                    distance = this.MtrTable.MinStepSize;
                    distance *= num;
                }
            }
        }

        public void MoveBy(double distance, MtrMotionProfile pf, float speedFactor = 1f)
        {
            this.ApplyMinDistanceRoundOff(ref distance);
            double pos = this.get_current_pos_f() + distance;
            this.MoveTo(pos, pf, speedFactor);
        }

        public double get_current_pos_main_axis_f()
        {
            return this.get_mm(this.get_current_pos_main_axis());
        }

        public double get_current_pos_f()
        {
            return this.get_mm(this.get_current_pos());
        }

        public void set_current_pos_f(double pos)
        {
            this.set_current_pos(this.get_step(pos));
        }

        public double get_new_pos_f()
        {
            return this.get_mm(this.MtrTable.NewPos);
        }


        public void start_ta_move_f(MtrPosSpeed posSd)
        {
            this.start_ta_move(this.get_step(posSd.Pos), (double)posSd.StartVel, (double)posSd.MaxVel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec);
        }
        public void start_ta_move(double pos)
        {
            this.start_ta_move(pos, this.MtrSpeed.StrVel, this.MtrSpeed.MaxVel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec);
        }
        public void start_ta_move(double pos, double strvel, double maxvel)
        {
            this.start_ta_move(pos, strvel, maxvel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec);
        }
        public void start_ta_move_xy(ref AxisBase pxmtr, ref AxisBase pymtr)
        {
            this.start_ta_move_xy(ref pxmtr, ref pymtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec);
        }
        public void start_ta_move_xy(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel)
        {
            this.start_ta_move_xy(ref pxmtr, ref pymtr, strvel, maxvel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec);
        }

        public void start_tr_move(double dist, double strvel, double maxvel)
        {
            this.start_ta_move(dist, strvel, maxvel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec);
        }

        public void start_sa_move(double pos)
        {
            this.start_sa_move(pos, this.MtrSpeed.StrVel, this.MtrSpeed.MaxVel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec, this.MtrSpeed.VSacc, this.MtrSpeed.VSdec);
        }
        public void start_sa_move(double pos, double strvel, double maxvel)
        {
            this.start_sa_move(pos, strvel, maxvel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec, this.MtrSpeed.VSacc, this.MtrSpeed.VSdec);
        }
        public void start_sr_move(double dist, double strvel, double maxvel)
        {
            this.start_sa_move(dist, strvel, maxvel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec, this.MtrSpeed.VSacc, this.MtrSpeed.VSdec);
        }

        public void start_sa_move_f(MtrPosSpeed posSd)
        {
            this.start_sa_move(this.get_step(posSd.Pos), (double)posSd.StartVel, (double)posSd.MaxVel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec, this.MtrSpeed.VSacc, this.MtrSpeed.VSdec);
        }
        public void start_sa_move_f(double pos)
        {
            this.start_sa_move(this.get_step(pos), this.MtrSpeed.StrVel, this.MtrSpeed.MaxVel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec, this.MtrSpeed.VSacc, this.MtrSpeed.VSdec);
        }
        public void start_sa_move_f(double pos, double strvel, double maxvel)
        {
            this.start_sa_move(this.get_step(pos), strvel, maxvel, this.MtrSpeed.Tacc, this.MtrSpeed.Tdec, this.MtrSpeed.VSacc, this.MtrSpeed.VSdec);
        }
        public void start_sa_move_f(double pos, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec)
        {
            this.start_sa_move(this.get_step(pos), strvel, maxvel, tacc, tdec, vsacc, vsdec);
        }









        public void start_ta_move_zu(ref AxisBase pxmtr, ref AxisBase pymtr)
        {
            this.start_ta_move_zu(ref pymtr, ref pymtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec);
        }

        public void start_ta_move_zu(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel)
        {
            this.start_ta_move_zu(ref pymtr, ref pymtr, strvel, maxvel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec);
        }

        public void start_sa_move_xy(ref AxisBase pxmtr, ref AxisBase pymtr)
        {
            this.start_sa_move_xy(ref pxmtr, ref pymtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec, pxmtr.MtrSpeed.IntVSacc, pxmtr.MtrSpeed.IntVSdec);
        }

        public void start_sa_move_xy(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel)
        {
            this.start_sa_move_xy(ref pxmtr, ref pymtr, strvel, maxvel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec, pxmtr.MtrSpeed.IntVSacc, pxmtr.MtrSpeed.IntVSdec);
        }

        public void start_sa_move_zu(ref AxisBase pxmtr, ref AxisBase pymtr)
        {
            this.start_sa_move_zu(ref pxmtr, ref pymtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec, pxmtr.MtrSpeed.IntVSacc, pxmtr.MtrSpeed.IntVSdec);
        }

        public void start_sa_move_zu(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel)
        {
            this.start_sa_move_zu(ref pxmtr, ref pymtr, strvel, maxvel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec, pxmtr.MtrSpeed.IntVSacc, pxmtr.MtrSpeed.IntVSdec);
        }

        public void start_ta_line2(ref AxisBase pxmtr, ref AxisBase pymtr)
        {
            this.start_ta_line2(ref pxmtr, ref pymtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec);
        }

        public void start_sa_line2(ref AxisBase pxmtr, ref AxisBase pymtr)
        {
            this.start_sa_line2(ref pxmtr, ref pymtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec, pxmtr.MtrSpeed.IntVSacc, pxmtr.MtrSpeed.IntVSdec);
        }

        public void start_ta_line3(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr)
        {
            this.start_ta_line3(ref pxmtr, ref pymtr, ref pzmtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec);
        }

        public void start_sa_line3(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr)
        {
            this.start_sa_line3(ref pxmtr, ref pymtr, ref pzmtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec, pxmtr.MtrSpeed.IntVSacc, pxmtr.MtrSpeed.IntVSdec);
        }

        public void start_ta_line4(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr, ref AxisBase pumtr)
        {
            this.start_ta_line4(ref pxmtr, ref pymtr, ref pzmtr, ref pumtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec);
        }

        public void start_sa_line4(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr, ref AxisBase pumtr)
        {
            this.start_sa_line4(ref pxmtr, ref pymtr, ref pzmtr, ref pumtr, pxmtr.MtrSpeed.IntStrVel, pxmtr.MtrSpeed.IntMaxVel, pxmtr.MtrSpeed.IntTacc, pxmtr.MtrSpeed.IntTdec, pxmtr.MtrSpeed.IntVSacc, pxmtr.MtrSpeed.IntVSdec);
        }

        public void start_jog_positive(float velocity)
        {
            this.sv_jog((double)velocity, (double)velocity, this.mtrSpeed.Tacc, this.mtrSpeed.Tdec);
        }

        public virtual int jog_till_neg_limit(float velocity, int timeout = 30000)
        {
            return 0;
        }

        public virtual int jog_till_pos_limit(float velocity, int timeout = 30000)
        {
            return 0;
        }

        public int PollUserEvents()
        {
            MotionStatus result = MotionStatus.MT_DONE;
            if (this.MtrTable.pCriticalEvent != null)
            {
                if (this.MtrTable.pCriticalEvent())
                {
                    this.SetInhibit(true);
                    result = MotionStatus.MT_CRITICAL;
                    this.MtrTable.HomeFlag = 0;
                    this.emg_stop();
                }
                else
                {
                    this.SetInhibit(false);
                }
            }
            if (this.MtrTable.pFwdSensing != null)
            {
                if (this.MtrTable.pFwdSensing())
                {
                    this.SetInhibit(true);
                    this.sd_stop(0.0);
                    result = MotionStatus.MT_LIMIT;
                }
                else
                {
                    this.SetInhibit(false);
                }
            }
            if (this.MtrTable.pRevSensing != null)
            {
                if (this.MtrTable.pRevSensing())
                {
                    this.SetInhibit(true);
                    this.sd_stop(0.0);
                    result = MotionStatus.MT_LIMIT;
                }
                else
                {
                    this.SetInhibit(false);
                }
            }
            return (int)result;
        }

        public bool StartUserEventThread()
        {
            if (this.thread == null)
            {
                this.thread = new Thread(new ThreadStart(this.UserEventsThreadProc));
                this.thread.Name = "AxisBase UserEventsThread";
                this.thread.Start();
            }
            return true;
        }

        public bool StopUserThread()
        {
            if (this.thread != null)
            {
                this.thread.Abort();
                this.endThread = true;
            }
            this.thread = null;
            return true;
        }

        public bool StopStatusReadThread()
        {
            if (this.readMotorStatusThrd != null)
            {
                this.endreadMotorStatusThrd = true;
                Thread.Sleep(50);
            }
            this.IsStatusReading = false;
            this.readMotorStatusThrd = null;
            return true;
        }

        public bool Standby
        {
            get
            {
                return this.isStandby;
            }
            set
            {
                this.isStandby = value;
            }
        }

        private void UserEventsThreadProc()
        {
            while (this.endThread)
            {
                if (this.MtrTable.pCriticalEvent != null)
                {
                    if (this.MtrTable.pCriticalEvent())
                    {
                        this.MtrTable.HomeFlag = 0;
                        this.emg_stop();
                        break;
                    }
                }
                if (this.MtrTable.pHangedEvent != null)
                {
                    while (this.MtrTable.pHangedEvent())
                    {
                        Thread.Sleep(10);
                    }
                }
                if (this.MtrTable.pFwdSensing != null)
                {
                    if (this.MtrTable.pFwdSensing())
                    {
                        this.sd_stop(0.0);
                        break;
                    }
                }
                if (this.MtrTable.pRevSensing != null)
                {
                    if (this.MtrTable.pRevSensing())
                    {
                        this.sd_stop(0.0);
                        break;
                    }
                }
                Thread.Sleep(0);
            }
        }

        public static void SetDisableInterLockAllowDistnace(double val)
        {
            if (val < 0.1 && val > 30.0)
            {
                throw new Exception("Ouf of range for vale (0.1 to 30 (mm or deg))");
            }
            AxisBase.disableInterLockAllowDistance = val;
        }

        public bool IsProhibitToMove(double distance)
        {
            this.InterlockWaringMsg = "";
            this.isProhibitActivated = false;
            if (this.mtrTable.EnableWriteACSDistacneVar)
            {
                string str = "";
                if (!this.WriteDistanceToBuffer(distance, ref str))
                {
                    this.InterlockWaringMsg = "Failed To Write ACS Distance Value to VarName=" + this.MtrTable.ACSDistacneVarName + Environment.NewLine + str;
                    return true;
                }
            }
            bool result;
            if (this.disableMotionInterlock)
            {
                bool flag = false;
                if (this.MtrTable.pIsInhibitToMove != null)
                {
                    flag = !this.MtrTable.pIsInhibitToMove(ref this.strErr);
                }
                if (!flag && Math.Abs(distance) > AxisBase.disableInterLockAllowDistance)
                {
                    this.InterlockWaringMsg = "Distance must be less than Allowable move [" + AxisBase.disableInterLockAllowDistance.ToString("F3") + "]";
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (this.MtrTable.pIsInhibitToMove != null)
                {
                    string text = "";
                    if (this.MtrTable.pIsInhibitToMove(ref text))
                    {
                        this.isProhibitActivated = true;
                        this.InterlockWaringMsg = text;
                        return true;
                    }
                }
                result = false;
            }
            return result;
        }

        protected virtual bool WriteDistanceToBuffer(double distance, ref string serr)
        {
            return true;
        }

        public string InterlockWaringMsg
        {
            get
            {
                return this.interlockWaringMsg;
            }
            set
            {
                this.interlockWaringMsg = value;
                this.OnPropertyChanged("InterlockWaringMsg");
            }
        }

        public bool IsProhibitToHome()
        {
            this.isProhibitActivated = false;
            string str = "";
            bool result = false;
            if (this.mtrTable.EnableWriteACSDistacneVar)
            {
                if (!this.WriteDistanceToBuffer(0.0, ref str))
                {
                    this.InterlockWaringMsg = "Failed To Write ACS Distance Value to VarName=" + this.MtrTable.ACSDistacneVarName + Environment.NewLine + str;
                    return true;
                }
            }
            this.InterlockWaringMsg = "";
            if (this.DisableHomingInterlock)
            {
                result = false;
            }
            else if (this.MtrTable.pIsInhibitToHome == null)
            {
                result = false;
            }
            else if (this.MtrTable.pIsInhibitToHome(ref this.sErr))
            {
                this.isProhibitActivated = true;
                this.InterlockWaringMsg = this.sErr;
                result = true;
            }
            return result;
        }
        public void StartInhibitThread()
        {
            if (this._inhibitThread == null && this.MtrTable.pIsInhibitToMove != null)
            {
                string strErr = "";
                this.endInhibitThread = false;
                Thread.MemoryBarrier();
                this._inhibitThread = delegate ()
                {
                    while (!this.endInhibitThread)
                    {
                        this.MtrTable.pIsInhibitToMove(ref strErr);
                        this.InterlockWaringMsg = this.sErr;
                        Thread.Yield();
                    }
                    return true;
                };
                this._inhibitThread.BeginInvoke(null, null);
            }
        }

        public void ForceToSetHomeDone()
        {
            //this.hasHome = true;
        }

        protected MotionStatus SimulateMotionStatusCheck()
        {
            MotionStatus result = MotionStatus.MT_DONE;
            TimeSpan timeSpan = (this.MtrTable.EstimateTimeTaken < 0.2) ? TimeSpan.FromSeconds(0.2) : TimeSpan.FromSeconds(this.MtrTable.EstimateTimeTaken);
            TimeSpan timeSpan2 = DateTime.Now - this.commmandStartTime;
            double curPos = this.MtrTable.CurPos;
            if (this.isStopReq)
            {
                result = MotionStatus.MT_DONE;
            }
            else if (timeSpan2.TotalMilliseconds < timeSpan.TotalMilliseconds)
            {
                double num = (DateTime.Now - this.commmandStartTime).TotalMilliseconds / timeSpan.TotalMilliseconds;
                if (num > 1.0)
                {
                    num = 1.0;
                }
                double num2 = this.MtrTable.NewPos - this.MtrTable.CurPos;
                this.MtrTable.CurPos = curPos + num * num2;
                result = MotionStatus.MT_MOVING;
            }
            else if (!this.isStopReq)
            {
                this.MtrTable.CurPos = this.MtrTable.NewPos;
                result = MotionStatus.MT_DONE;
            }
            return result;
        }

        public virtual bool IsMoving()
        {
            return false;
        }

        public virtual MotionStatus check_motion_status()
        {
            return MotionStatus.MT_ALARM;
        }

        public void StopInhibitThread()
        {
            this.endInhibitThread = true;
            Thread.MemoryBarrier();
            Thread.Sleep(50);
            this._inhibitThread = null;
        }

        public abstract void Dispose();
        public abstract bool IsRunning();
        public abstract bool IsHome();
        public virtual bool IsExceedMaxStep(double step)
        {
            bool result;
            if (step > this.MaxStep + 0.0001)
            {
                this.InterlockWaringMsg = this.get_mm(step).ToString("F4") + " is more than allow max = " + this.get_mm(this.MaxStep);
                result = true;
            }
            else if (step < this.MinStep - 0.0001)
            {
                this.InterlockWaringMsg = this.get_mm(step).ToString("F4") + " is more than allow min = " + this.get_mm(this.MinStep);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public abstract void set_servo(bool set);
        public abstract void set_newpos(double newPos);
        public abstract int wait_home_done(int waitDelay);
        public abstract int wait_motion_done(int waitDelay);
        protected abstract int wait_motion_complete();
        public abstract void set_zero_pos();
        public abstract void clear_home_flag();
        public abstract void abnormal_stop();
        public abstract int get_motion_sts();
        public abstract int get_io_sts();
        public abstract int home_move(double revstep, double strvel, double maxvel, double tacc, int WaitDelay = 0);
        public abstract int home_move(int waitDelay = 0);
        public abstract void tv_jog(double strvel, double maxvel, double tacc);
        public abstract void tv_jog();
        public abstract void sv_jog(double strvel, double maxvel, double tacc, double vsacc);
        public abstract void sv_jog();
        public abstract void emg_stop();
        public abstract void sd_stop(double tdec);
        public abstract void sd_stop();
        public virtual void start_ta_move(double pos, double strvel, double maxvel, double tacc, double tdec, double jerk)
        {
            this.start_ta_move(pos, strvel, maxvel, tacc, tdec);
        }
        public abstract void start_ta_move(double pos, double strvel, double maxvel, double tacc, double tdec);
        public virtual void start_sa_move(double pos, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec, double jerk)
        {
            this.start_sa_move(pos, strvel, maxvel, tacc, tdec, vsacc, vsdec);
        }
        public abstract void start_sa_move(double pos, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec);
        public abstract void start_tr_move(double dist, double strvel, double maxvel, double tacc, double tdec);
        public abstract void start_sr_move(double dist, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec);
        public virtual double get_current_pos_main_axis()
        {
            return this.get_current_pos();
        }
        public abstract double get_current_pos();
        public abstract void set_current_pos(double pos);
        public abstract void brake_free(bool set);
        public abstract bool get_origin_signal();
        public abstract bool get_pel_signal();
        public abstract bool get_nel_signal();
        public abstract bool get_inpos_signal();
        public abstract bool get_alarm_signal();
        public abstract bool initialization(int card, bool autoAddress);
        public abstract bool uninitialization(int card);


        public virtual void start_ta_move_xy_line_to_pos(AxisBase yAxis, double posX, double posY, double strvel, double maxvel, double tacc, double tdec)
        {
        }
        public abstract void start_ta_move_xy(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel, double tacc, double tdec);
        public abstract void start_ta_move_zu(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel, double tacc, double tdec);
        public abstract void start_sa_move_xy(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec);
        public abstract void start_sa_move_zu(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec);
        public abstract void start_ta_line2(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel, double tacc, double tdec);
        public abstract void start_sa_line2(ref AxisBase pxmtr, ref AxisBase pymtr, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec);
        public abstract void start_ta_line3(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr, double strvel, double maxvel, double tacc, double tdec);
        public abstract void start_sa_line3(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec);
        public abstract void start_ta_line4(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr, ref AxisBase pumtr, double strvel, double maxvel, double tacc, double tdec);
        public abstract void start_sa_line4(ref AxisBase pxmtr, ref AxisBase pymtr, ref AxisBase pzmtr, ref AxisBase pumtr, double strvel, double maxvel, double tacc, double tdec, double vsacc, double vsdec);
        public abstract void start_a_arc_xy(ref AxisBase pxmtr, ref AxisBase pymtr, short Dir);
        public abstract void start_a_arc_zu(ref AxisBase pxmtr, ref AxisBase pymtr, short Dir);
        public abstract void start_a_arc2(ref AxisBase pxmtr, ref AxisBase pymtr, short Dir);
        public abstract int GetAnalogValue();
        public abstract void GoInVelocityMode(ushort CardNo, ushort axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec, double stop_vel, ushort dir);
        public abstract int MoveContinuousStopByIOTrigger(double pos, bool isPositive);

        protected double _assign_StartVelocity;
        protected double _assign_MaxVelocity;
        protected double _assign_Acceleration;
        protected double _assign_Deceleration;

        public double _Assign_StartVelocity
        {
            get { return _assign_StartVelocity; }
            set
            {

                _assign_StartVelocity = value; OnPropertyChanged(nameof(_Assign_StartVelocity));
            }
        }
        public double _Assign_MaxVelocity
        {
            get { return _assign_MaxVelocity; }
            set { _assign_MaxVelocity = value; OnPropertyChanged(nameof(_Assign_MaxVelocity)); }
        }
        public double _Assign_Acceleration
        {
            get { return _assign_Acceleration; }
            set { _assign_Acceleration = value; OnPropertyChanged(nameof(_Assign_Acceleration)); }
        }
        public double _Assign_Deceleration
        {
            get { return _assign_Deceleration; }
            set { _assign_Deceleration = value; OnPropertyChanged(nameof(_Assign_Deceleration)); }
        }



        public enum LastMoveDirection
        {
            Unknown,
            Forward,
            Backward
        }
       
        public delegate bool InitDelegate(int card);
        public void SetForceStop()
        {
            this.isForceStop = true;
        }
        public bool _IsHoming
        {
            get { return isHoming; }
        }
        public void SetIsHoming()
        {
            this.isHoming = true;
        }
        public void ReleaseIsHoming()
        {
            this.isHoming = false;
        }
    }
}
