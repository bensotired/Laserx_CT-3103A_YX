using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_Motion
{
    /// <summary>
    /// to do 序列化 反序列化
    /// </summary>
    public abstract class MotorAxisBase// : IModel, ITool
    {
        #region ctor
        public MotorAxisBase(MotorSetting setting)
        {
            this._setting = setting;
            this._interation = new MotorRuntimeInteration();
        }
        #endregion
        protected MotorSetting _setting;
        protected MotorRuntimeInteration _interation;
        protected CancellationTokenSource readStatusSource;
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

        public string Name
        {
            get { return this._setting.Name; }
        }
        public MotorRuntimeInteration Interation
        {
            get
            {
                return this._interation;
            }
        }
        public MotorSetting MotorGeneralSetting
        {
            get
            {
                return this._setting;
            }
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
                    _interation. IsOrg = Get_Origin_Signal();
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

                    _interation.AnalogInputValue = Get_AnalogInputValue();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);

                    _interation.IsServoOn = Get_ServoStatus();
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
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

            if (this._interation.pIsInhibitToHome == null) return false;

            if (this._interation.pIsInhibitToHome(ref sErr))
            {
                this.isProhibitActivated = true;
                this._interation.InterlockWaringMsg = sErr;
                result = true;
            }

            return result;
        }
        public bool IsProhibitToMove()
        {
            this._interation.InterlockWaringMsg = "";
            this.isProhibitActivated = false;
            bool result = false;
            string sErr = string.Empty;

            if (!this._interation.IsSimulation && this._interation.IsMoving) return true;

            if (this._interation.pIsInhibitToMove == null) return false;
            if (this._interation.pIsInhibitToMove(ref sErr))
            {
                this.isProhibitActivated = true;
                this._interation.InterlockWaringMsg = sErr;
                return true;
            }

            return result;
        }     
        public int InPositionCheck(double targetPos)
        {
            int errorCode = ErrorCodes.NoError;
            double curPos = this._interation.IsSimulation ? this._interation.CurrentPosition : Get_CurUnitPos();
            double realOffset = Math.Abs(curPos - targetPos);

            if (Math.Abs(realOffset) > _setting.MotorTable.AcceptableInPositionOffset)
            {
                errorCode = ErrorCodes.MotorNotReachToPos;

            }
            else
            {
                errorCode = ErrorCodes.NoError;
            }
           
         
            return errorCode;           
        }
        public void SetHomeDone(bool homedone)
        {
             this._interation.HasHome = homedone;
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
        public abstract int MoveToV3(double pos, SpeedType speedType, SpeedLevel speedLevel);
        public abstract int WaitMotionDone();
        public abstract int WaitHomeDone(CancellationTokenSource tokenSource);
        public abstract bool Stop();
        public abstract bool HomeRun();
        public abstract bool PhaseSearching(int timeout_s);
        public abstract void Jog(bool isPositive);
        public abstract int Get_IO_sts();
        public abstract bool SetCurrentPositionToZero();
        public abstract bool Clear_AlarmSignal();
        public abstract void Set_Servo(bool on);
        public virtual bool SetToRunMode()
        {
            return true;
        }
        public virtual bool SetToHomeMode()
        {
            return true;
        }

        #region 公式计算

        private double FormulaCalc(string Formula, Dictionary<string, double> x)
        {
            string CurrentFormula = Formula.ToUpper();

            //参数输入
            string XInput = "";

            foreach (var item in x)
            {
                XInput += item.Key.ToUpper() + "=" + item.Value.ToString() + ";";
            }

            string CurrentFormula2 = XInput + CurrentFormula + ";";

            //计算结果
            try
            {
                MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControlClass();
                sc.Language = "JavaScript";
                object t = sc.Eval(CurrentFormula2);
                double result = 0;
                double.TryParse(t.ToString(), out result);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //64位用V8
        //private double FormulaCalc(string Formula, Dictionary<string, double> x)
        //{
        //    string CurrentFormula = Formula;

        //    //计算结果
        //    try
        //    {
        //        V8Engine engine = new V8Engine();
        //        engine.RunMarshallingTests();

        //        //参数输入
        //        foreach (var item in x)
        //        {
        //            //定义可以在JS中使用的全局变量，string/value
        //            engine.GlobalObject.SetProperty(item.Key, item.Value);
        //        }

        //        //js中return object
        //        string t = engine.ConsoleExecute(CurrentFormula).AsString;
        //        double result = 0;
        //        double.TryParse(t.ToString(), out result);
        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        public double FormulaCalc_AngleToUnit(double Angle)
        {

            //电机旋转角度转位置
            Dictionary<string, double> para = new Dictionary<string, double>();
            para.Add("Angle", Angle);

            double res;
            try
            {
                res = FormulaCalc(this._setting.MotorTable.Formula_AngleToUnit, para);
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public double FormulaCalc_UnitToAngle(double Unit)
        {
            //电机旋转角度转位置
            Dictionary<string, double> para = new Dictionary<string, double>();
            para.Add("Unit", Unit);

            double res;
            try
            {
                res = FormulaCalc(this._setting.MotorTable.Formula_UnitToAngle, para);
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }            
        }
 
    }
}
