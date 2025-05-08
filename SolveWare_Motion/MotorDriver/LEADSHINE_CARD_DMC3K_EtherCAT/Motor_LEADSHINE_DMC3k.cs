
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_Motion
{
    //问题点记录
    //1：点位运动的时候写入配置文件的是脉冲还是脉冲当量，如果是脉冲当量就需要重新计算脉冲   
    //回答：全部都是走脉冲的  距离进行换算即可
    //2:MoveToV3中的位置是脉冲
    //3：脉冲输出模式需要先用运动软件调试完成后确定脉冲+方向
    //4:雷赛的控制卡的轴号是从0开始的 这怎么处理 跟固高的后面对比一下  
    //回答:根据底层来设置轴
    public class Motor_LEADSHINE_DMC3k : MotorAxisBase
    {

        const int ORG_SIGNAL_BIT = 2;
        public bool _pause = false;

        public Motor_LEADSHINE_DMC3k(MotorSetting setting) : base(setting)
        {
            this.Interation.AxisName = setting.Name;
            this.Interation.AxisTag = $"LEADSHINE Motion AxisName[{setting.Name}] CardNo ={MotorGeneralSetting.MotorTable.CardNo},AxisID ={MotorGeneralSetting.MotorTable.AxisNo}";
        }

        public override void StartStatusReading()
        {
            if (readStatusSource != null) return;
            readStatusSource = new CancellationTokenSource();
            Task task = new Task(() =>
            {
                while (!readStatusSource.IsCancellationRequested)
                {
                    //dmc_axis_io_status(WORD CardNo,WORD axis)
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsOrg = Get_Origin_Signal();//原点信号
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsAlarm = Get_Alarm_Signal();//报警信号
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsPosLimit = Get_PEL_Signal();//正限位
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsNegLimit = Get_NEL_Signal();//负限位
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsInPosition = Get_InPos_Signal();//位置到位信号
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    if (!_interation.IsSimulation) _interation.IsMoving = IsMotorMoving();//是否在运行的状态
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.CurrentPulse = Get_CurPulse();//现在当前的脉冲位置
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.CurrentPosition = Get_CurUnitPos();//当前的位置mm
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    _interation.IsServoOn = Get_ServoStatus();//获取使能状态
                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                }
                cancelDoneFlag.Set();

            }, readStatusSource.Token, TaskCreationOptions.LongRunning);
            task.Start();
        }

        public override bool Init()//初始化设置点平 需要根据参数来进行设置
        {
            if (this._interation.IsSimulation)
            {
                return true;
            }
            try
            {
                ushort cardNo = (ushort)this._setting.MotorTable.CardNo;
                ushort axis = (ushort)this._setting.MotorTable.AxisNo;

                ushort outmode = (ushort)this._setting.MotorModes.Pulse_Mode;

                ushort el_enable = 1;//正负限位允许
                ushort el_logic = 0;//正负限位低电平有效
                ushort el_mode = 0;//正负限位立即停止

                ushort org_logic = 1;//0低有效
                double org_filter = 0;//0：固定值

                ushort alm_enable = 1;//1：允许
                ushort alm_logic = 1;//1：高有效
                ushort alm_action = 0;//0 立即停止 （只支 持该方式）

                //string manualHomeModeStr = Enum.Parse(typeof(MANUAL_HOMEMODE_LEADSHINE), this._setting.MotorModes.Home_Mode.ToString()).ToString();
                //org_logic = (ushort)Convert.ToInt32(Enum.Parse(typeof(HOME_LOGIC_LEADSHINE), manualHomeModeStr.Split('_')[1]));
                //alm_logic = (ushort)Convert.ToInt32(Enum.Parse(typeof(ALARM_LOGIC), manualHomeModeStr.Split('_')[3]));
                //el_logic = (ushort)Convert.ToInt32(Enum.Parse(typeof(LIMIT_LOGIC), manualHomeModeStr.Split('_')[4]));

                //outmode是脉冲加方向 所以需要用雷赛的软件先进行调试
                //LTDMC.dmc_set_pulse_outmode(cardNo, axis, outmode);//设置脉冲输出模式
                //LTDMC.dmc_set_el_mode(cardNo, axis, el_enable, el_logic, el_mode);//设置EL模式
                //LTDMC.dmc_set_home_pin_logic(cardNo, axis, org_logic, org_filter);//设置原点电平
                //LTDMC.dmc_set_alm_mode(cardNo, axis, alm_enable, alm_logic, alm_action); //设置报警电平
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public override void Jog(bool isPositive)//这里只是下发指令  界面上有mouseup和mousedown
        {
            //所有的运动都是以远离点击方向为正
            const ushort s_mode = 0;
            double s_para = this._setting.MotorSpeed.Jog_SmoothingTime;
            ushort cardNo = (ushort)this._setting.MotorTable.CardNo;
            ushort axis = (ushort)this._setting.MotorTable.AxisNo;
            double min_Vel = this._setting.MotorSpeed.Jog_Start_Velocity;
            double max_Vel = this._setting.MotorSpeed.Jog_Max_Velocity;
            double tacc = this._setting.MotorSpeed.Jog_Acceleration;
            double tdec = this._setting.MotorSpeed.Jog_Deceleration;
            double stop_Vel = this._setting.MotorSpeed.Jog_Start_Velocity;
            ushort dir = Convert.ToUInt16(isPositive);

            //计算加减速时间
            var Acc_s = (max_Vel - min_Vel) / tacc;
            var Dec_s = Acc_s;

            //设置运行参数
            var minPulse = ConvertPositionToPulse(min_Vel);
            var maxPulse = ConvertPositionToPulse(max_Vel);

            //加减速的总时间是s_para+tacc
            LTDMC.dmc_set_s_profile(cardNo, axis, s_mode, s_para);
            LTDMC.dmc_set_profile(cardNo, axis, minPulse, maxPulse, Acc_s, Dec_s, ConvertPositionToPulse(stop_Vel));
            LTDMC.dmc_set_dec_stop_time(cardNo, axis, Dec_s);
            LTDMC.dmc_vmove(cardNo, axis, dir);//0 ：负方向 1 ：正方向
        }

        /// <summary>
        /// 单轴运动参数和指令 单纯的发送指令
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="speedType"></param>
        /// <param name="slowFactor"></param>
        /// <returns></returns>
        public override int MoveToV3(double pos, SpeedType speedType, SpeedLevel speedLevel)
        {
            int errorCode = ErrorCodes.NoError;

            //所有的运动都是以远离点击方向为正    0 ：相对坐标模式 1 ：绝对坐标模式
            const ushort s_mode = 0;
            const ushort posi_mode = 1;

            ushort cardNo = (ushort)this._setting.MotorTable.CardNo;
            ushort axis = (ushort)this._setting.MotorTable.AxisNo;

            double s_para = 0.0;
            double min_Vel = 0.0;
            double max_Vel = 0.0;
            double tacc = 0.0;
            double tdec = 0.0;
            double stop_Vel = 0.0;

            switch (speedLevel)
            {
                case SpeedLevel.High:
                case SpeedLevel.Normal:
                    {
                        s_para = this._setting.MotorSpeed.Auto_SmoothingTime;  //范围 0---0.5 s
                        min_Vel = this._setting.MotorSpeed.Auto_Start_Velocity;
                        max_Vel = this._setting.MotorSpeed.Auto_Max_Velocity;
                        tacc = this._setting.MotorSpeed.Auto_Acceleration;
                        tdec = this._setting.MotorSpeed.Auto_Deceleration;
                        stop_Vel = this._setting.MotorSpeed.Auto_Start_Velocity;
                    }
                    break;
                case SpeedLevel.Low:
                    {
                        s_para = this._setting.MotorSpeed.Auto_Low_SmoothingTime;  //范围 0---0.5 s
                        min_Vel = this._setting.MotorSpeed.Auto_Low_Start_Velocity;
                        max_Vel = this._setting.MotorSpeed.Auto_Low_Max_Velocity;
                        tacc = this._setting.MotorSpeed.Auto_Low_Acceleration;
                        tdec = this._setting.MotorSpeed.Auto_Low_Deceleration;
                        stop_Vel = this._setting.MotorSpeed.Auto_Low_Start_Velocity;
                    }
                    break;
            }

            pos = Math.Round(pos, 5, MidpointRounding.AwayFromZero);
            this._interation.PlanPosition = pos;
            var temp = (pos * 1000.0 * 1000.0) / this._setting.MotorTable.UnitOfRound * this._setting.MotorTable.Resolution;
            double targetPulse = Math.Round((temp / (1000.0 * 1000.0)), 5);
            int dist = Convert.ToInt32(targetPulse);


            //计算加减速时间
            var Acc_s = (max_Vel - min_Vel) / tacc;
            var Dec_s = Acc_s;

            //设置运行参数
            var minPulse = ConvertPositionToPulse(min_Vel);
            var maxPulse = ConvertPositionToPulse(max_Vel);

            //加减速的总时间是s_para+tacc
            LTDMC.dmc_set_s_profile(cardNo, axis, s_mode, s_para);
            LTDMC.dmc_set_profile(cardNo, axis, minPulse, maxPulse, Acc_s, Dec_s, ConvertPositionToPulse(stop_Vel));
            LTDMC.dmc_pmove(cardNo, axis, dist, posi_mode);//0 ：负方向 1 ：正方向

            return errorCode;
        }


        /// <summary>
        /// 等待原点回零结束 其实就是用checkdone检测是否到位没
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <returns></returns>
        public override int WaitHomeDone(CancellationTokenSource tokenSource)
        {
            int errorCode = ErrorCodes.NoError; ;
            if (this._interation.IsSimulation)
            {
                this.isMoving = false;
                return errorCode;
            }
            Thread.Sleep(10);
            Stopwatch st = new Stopwatch();
            st.Start();

            short ret = 0;
            do
            {
                //0 指定轴正在运行， 1 指定轴 已停止
                ret = LTDMC.dmc_check_done((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo);
                if (ret == 1)
                {
                    errorCode = ErrorCodes.NoError;
                    break;
                }
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
            }
            while (true);
            return errorCode;
        }

        public override int WaitMotionDone()//等待运动完成
        {
            int errorCode = ErrorCodes.NoError;
            DateTime st = DateTime.Now;

            if (this._interation.IsSimulation)
            {
                this.isMoving = false;
                return errorCode;
            }
            short ret = 0;
            while (true)
            {
                TimeSpan ts = DateTime.Now - st;

                ret = LTDMC.dmc_check_done((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo);
                if (ret == 1)
                {
                    errorCode = ErrorCodes.NoError;
                    break;
                }

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
            ushort cardNo = (ushort)this._setting.MotorTable.CardNo;
            ushort axis = (ushort)this._setting.MotorTable.AxisNo;

            ushort org_logic = 0;//默认远地点电平都是负的
            double filter = 0;

            ushort home_dir = 0;//0 负向 1 正向  根据大佬对轴的定义  所有轴都是负方向回零点  而且复位速度都是用最大的速度
            double homeVel = 1;//0为低速 1为高速
            ushort homeMode = (ushort)this._setting.MotorModes.Home_Mode;
            ushort EZ_count = 0;

            double min_Vel = this._setting.MotorSpeed.Home_Start_Velocity;
            double max_Vel = this._setting.MotorSpeed.Home_Max_Velocity;
            double tacc = this._setting.MotorSpeed.Home_Acceleration;
            double tdec = this._setting.MotorSpeed.Home_Deceleration;
            double stop_Vel = 0;// this._setting.MotorSpeed.Home_Start_Velocity;

            string manualHomeModeStr = Enum.Parse(typeof(MANUAL_HOMEMODE_LEADSHINE), this._setting.MotorModes.Home_Mode.ToString()).ToString();
            home_dir = (ushort)Convert.ToInt32(Enum.Parse(typeof(HOME_DIRECTION_LEADSHINE), manualHomeModeStr.Split('_')[0]));
            homeMode = (ushort)Convert.ToInt32(Enum.Parse(typeof(HOME_MODE_LEADSHINE), manualHomeModeStr.Split('_')[2]));

            //计算加减速时间
            var Acc_s = 0.05;// (max_Vel - min_Vel) / tacc;
            var Dec_s = 0.05;//Acc_s;

            //设置运行参数
            var minPulse = 0;// ConvertPositionToPulse(min_Vel);
            var maxPulse = ConvertPositionToPulse(max_Vel);

            LTDMC.dmc_set_dec_stop_time(cardNo, axis, 0.1);

            //LTDMC.dmc_set_pulse_outmode(cardNo, axis, homeMode);
            LTDMC.dmc_set_homemode(cardNo, axis, home_dir, homeVel, homeMode, EZ_count);
            LTDMC.dmc_set_profile(cardNo, axis, minPulse, maxPulse, Acc_s, Dec_s, ConvertPositionToPulse(stop_Vel));
            LTDMC.dmc_set_s_profile(cardNo, axis, 0, 0);//T形运行
            LTDMC.dmc_home_move(cardNo, axis);

            return true;
        }
        public override bool Stop() //单轴停止 此函数适用于单轴、 PVT 运动  如果减速停止的时候 一般都是执行的是运动前速度的参数
        {
            if (this._interation.IsSimulation)
            {
                return true;
            }
            else
            {
                LTDMC.dmc_stop((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo, (ushort)this._setting.MotorModes.Stop_Mode);
                return true;
            }
        }
        public override void Set_Servo(bool on)//使能指定的轴
        {
            do
            {
                if (this._interation.IsSimulation)
                {
                    this._interation.IsServoOn = on;

                    break;
                }
                //从配置中知道的使能初始化的
                ushort ServoOff_Logic = 0;
                if (this._setting.MotorTable.ServoOn_Logic == (short)Sevon_Pin.HighLevel)
                {
                    ServoOff_Logic = (ushort)Sevon_Pin.LowLevel;
                }
                else
                {
                    ServoOff_Logic = (ushort)Sevon_Pin.HighLevel;
                }
                if (on)
                {
                    LTDMC.dmc_write_sevon_pin((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo, (ushort)this._setting.MotorTable.ServoOn_Logic);
                }
                else
                {
                    LTDMC.dmc_write_sevon_pin((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo, ServoOff_Logic);
                }
                this._interation.IsServoOn = on;

            } while (false);
        }
        public override bool PhaseSearching(int timeout_s)
        {
            return true;
        }

 
        public override bool SetCurrentPositionToZero()//位置清零 一般都是复位完成之后在进行清零的
        {
            short a = LTDMC.dmc_set_position((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo, 0);
            return true;
        }

        //**************************************************
        public override int Get_IO_sts()//获取轴的IO状态  但是返回值不是Uint格式
        {
            return 0;
        }
        public override double Get_AnalogInputValue()//得到模拟输入值
        {
            if (this._interation.IsSimulation) return 0.0;
            return 0.0;
        }

        public override bool Clear_AlarmSignal()//清除报警标志  因为雷赛的报警都是自动清除 
        {
            return true;
        }

        public override bool Get_Alarm_Signal()//获取报警标志  因为雷赛的报警都是自动清除
        {
            return true;
        }
        private uint Get_IO_sts_LEADSHINE()
        {
            uint ret = LTDMC.dmc_axis_io_status((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo);
            return ret;
        }
        public override bool IsMotorMoving()
        {
            if (this._interation.IsSimulation)
                return false;
            return IsRunning();
        }
        private bool IsRunning()
        {
            //返回值： 0 指定轴正在运行， 1 指定轴 已停止
            short ret = LTDMC.dmc_check_done((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo);
            return ret == 0 ? true : false;
        }
        public override bool Get_ServoStatus()
        {
            if (this._interation.IsSimulation)
                return false;
            //伺服使能端 口 电平 0 低电平 1 高电平
            short sevon_pin_Logic = LTDMC.dmc_read_sevon_pin((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo);
            return sevon_pin_Logic == 0 ? true : false;
        }
        public override double Get_CurPulse()
        {
            if (this._interation.IsSimulation)
                return 0.0;
            //指令脉冲位置，单位： pulse
            int pulsePosition = LTDMC.dmc_get_position((ushort)this._setting.MotorTable.CardNo, (ushort)this._setting.MotorTable.AxisNo);
            return pulsePosition;
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
            return ((AXIS_STATUS_LEADSHINE)Get_IO_sts_LEADSHINE() & AXIS_STATUS_LEADSHINE.FlagINP) == AXIS_STATUS_LEADSHINE.FlagINP;
        }


        public override bool Get_NEL_Signal()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_LEADSHINE)Get_IO_sts_LEADSHINE() & AXIS_STATUS_LEADSHINE.FlagNEL) == AXIS_STATUS_LEADSHINE.FlagNEL;
        }

        public override bool Get_Origin_Signal()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_LEADSHINE)Get_IO_sts_LEADSHINE() & AXIS_STATUS_LEADSHINE.FlagORG) == AXIS_STATUS_LEADSHINE.FlagORG;
        }

        public override bool Get_PEL_Signal()
        {
            if (this._interation.IsSimulation)
                return false;
            return ((AXIS_STATUS_LEADSHINE)Get_IO_sts_LEADSHINE() & AXIS_STATUS_LEADSHINE.FlagPEL) == AXIS_STATUS_LEADSHINE.FlagPEL;
        }
        #region 脉冲与位置之间转换
        private int ConvertPositionToPulse(double distance_mm)
        {
            return Convert.ToInt32(distance_mm * (this._setting.MotorTable.Resolution / this._setting.MotorTable.UnitOfRound));
        }
        private double ConvertPulseToPosition(double pulse)
        {
            return Math.Round((pulse * this._setting.MotorTable.UnitOfRound) / this._setting.MotorTable.Resolution, 3);
        }
        #endregion
    }
}