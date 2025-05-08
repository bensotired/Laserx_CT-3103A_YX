using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_Motion
{
    public class Motor_LaserX_9078 : MotorAxisBase
    {
        //设置脉冲宽度 重要参数, 窄脉宽步进驱动器无法响应
        //2024-8-2 yxw 控制轴未完全停止时设置PN_OutputStepLen参数反而会导致问题
        //private const int StepLen_ns = 500000; //us  默认值是 5000ns -> 5us
        private int DigitalDigits = 4;   //Double数值的精度,控制位数0.0001

        //private Dictionary<int, Stopwatch[]> Home_sw = new Dictionary<int, Stopwatch[]>();  //每个轴都有自己的计时器

        public Motor_LaserX_9078(MotorSetting setting) : base(setting)
        {
            this.Interation.AxisName = setting.Name;
            this.Interation.AxisTag = $"LaserX Motion AxisName[{setting.Name}] CardNo ={MotorGeneralSetting.MotorTable.CardNo},AxisID ={MotorGeneralSetting.MotorTable.AxisNo}";

            //foreach(var id in LaserX_9078_Utilities.CardIDList)
            //{
            //    Stopwatch[] sw = new Stopwatch[LaserX_9078_Utilities.MOT_MAX_AXIS];
            //    for(int i=0;i<sw.Length;i++)
            //    {
            //        sw[i] = new Stopwatch();
            //    }
            //    Home_sw.Add(id, sw);
            //}
        }

        public override void StartStatusReading()
        {
            if (readStatusSource != null) return;
            readStatusSource = new CancellationTokenSource();
            Task task = new Task(() =>
            {
                while (!readStatusSource.IsCancellationRequested)
                {
                    RefreshAxisStatus();

                    //dmc_axis_io_status(WORD CardNo,WORD axis)
                    _interation.IsOrg = Get_Origin_Signal();//原点信号
                    _interation.HasHome = Get_Homed_Signal();//是否回过原点

                    _interation.IsAlarm = Get_Alarm_Signal();//报警信号
                    _interation.IsPosLimit = Get_PEL_Signal();//正限位
                    _interation.IsNegLimit = Get_NEL_Signal();//负限位
                    //Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                    //_interation.IsInPosition = Get_InPos_Signal();//位置到位信号

                    if (!_interation.IsSimulation) _interation.IsMoving = IsMotorMoving();//是否在运行的状态

                    _interation.CurrentPulse = Get_CurPulse();//现在当前的位置位置
                    _interation.CurrentPosition = Get_CurUnitPos();///当前转换后的位置mm

                    _interation.IsServoOn = Get_ServoStatus();//获取使能状态

                    Thread.Sleep(this._setting.MotorTable.StatusReadTiming);
                }
                cancelDoneFlag.Set();
            }, readStatusSource.Token, TaskCreationOptions.LongRunning);
            task.Start();
        }

        //卡号, 轴号
        private int cardNo = 0;

        private int axis = 0;

        public int CardNo
        { get { return cardNo; } }

        public int AxisNo
        { get { return axis; } }

        public MotorSpeed Speed
        { get { return this._setting.MotorSpeed; } }

        /// <summary>
        /// 轴初始化函数
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            if (this._interation.IsSimulation)
            {
                return true;
            }
            try
            {
                int rc = 0;
                string exMsg = "";

                cardNo = this._setting.MotorTable.CardNo;
                axis = this._setting.MotorTable.AxisNo;

                if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
                {
                    int ServoOn_Logic = 0;
                    if (this._setting.MotorTable.ServoOn_Logic == (short)Sevon_Pin.HighLevel)
                    {
                        ServoOn_Logic = (ushort)Sevon_Pin.HighLevel;
                    }
                    else
                    {
                        ServoOn_Logic = (ushort)Sevon_Pin.LowLevel;
                    }

                    //设置伺服使能极性 [ 0:常开（表示关闭输出电压）  1:常闭（输出电压） ]
                    rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_AxisExtEnableType, ServoOn_Logic);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 伺服使能极性失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }

                    ////启动第一步下伺服
                    //rc = LaserX_9078_Utilities.P9078_AxisDisable(cardNo, axis);
                    //if (rc != 0)
                    //{
                    //    exMsg = $"控制卡ID[{cardNo}] P9078_AxisDisable设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    //    throw new Exception($"{exMsg}!");
                    //}
                }
                else
                {
                    //不存在的卡和轴
                    return false;
                }

                //pls/mm

                //                              pls/转 / mm/转  *
                double plsEquiv = this._setting.MotorTable.Resolution / this._setting.MotorTable.UnitOfRound;
                double outmode = this._setting.MotorModes.Pulse_Mode;

                rc = LaserX_9078_Utilities.P9078_AxisSetRealParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_AxisOutputScale, plsEquiv);//脉冲当量
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 脉冲当量设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //20231019 编码器相关
                {
                    //如果使用外部编码器
                    if (this._setting.MotorTable.EncoderSource == MotorTable.eEncoderType.Encoder)
                    {
                        rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_InputMode, 1);//4xAB
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 外部编码器 类型设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            throw new Exception($"{exMsg}!");
                        }                    
                        
                        double EncoderEquiv = this._setting.MotorTable.EncoderResolution / this._setting.MotorTable.UnitOfRound;

                        rc = LaserX_9078_Utilities.P9078_AxisSetRealParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_AxisInputScale, 1/EncoderEquiv);//编码器脉冲当量
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 外部编码器 脉冲当量设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            throw new Exception($"{exMsg}!");
                        }
                    }
                    //如果是使用脉冲方向到编码器, 则设定编码器脉冲当量为脉冲方向的脉冲当量
                    else if (this._setting.MotorTable.EncoderSource == MotorTable.eEncoderType.PulseLoop)
                    {
                        rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_InputMode, 0);//PLS
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 外部编码器 类型设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            throw new Exception($"{exMsg}!");
                        }

                        rc = LaserX_9078_Utilities.P9078_AxisSetRealParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_AxisInputScale, 1/plsEquiv);//脉冲当量
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 回环编码器 脉冲当量设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            throw new Exception($"{exMsg}!");
                        }
                    }
                }


                //20230817 忽略脉冲当量以下的小数位数
                {
                    float v = (float)(this._setting.MotorTable.UnitOfRound / this._setting.MotorTable.Resolution);
                    int t = (int)(1 / v);
                    int m = t.ToString().Length - 1;
                    if (m < 3) m = 3;
                    DigitalDigits = m;

                    //尝试消除位置偏差
                    //2）PN_OutputDirectionSetup、PN_OutputDirectionHold参数设置为0；
                    rc = LaserX_9078_Utilities.P9078_AxisSetRealParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputDirectionSetup, 0);//
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] PN_OutputDirectionSetup设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }
                    rc = LaserX_9078_Utilities.P9078_AxisSetRealParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputDirectionHold, 0);//
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] PN_OutputDirectionHold设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }

                }
                //编码器当量不知道在哪找
                //使能状态 驱动暂无 后续有需要补
                rc = LaserX_9078_Utilities.P9078_AxisSetBacklash(cardNo, axis, _setting.MotorTable.Backlash);// 丝杆回差mm
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 回差设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //设置最大速度和最大加速度(与设置中的最大保持一致)
                rc = LaserX_9078_Utilities.P9078_AxisSetMaxVelocity(cardNo, axis, this._setting.MotorSpeed.Auto_Max_Velocity);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 最大速度[Auto_Max_Velocity]设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                rc = LaserX_9078_Utilities.P9078_AxisSetMaxAcceleration(cardNo, axis, this._setting.MotorSpeed.Auto_Acceleration);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 最大加速度[Auto_Acceleration]设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //2024-8-2 yxw 控制轴未完全停止时设置PN_OutputStepLen参数反而会导致问题
                //设置脉冲宽度 重要参数, 窄脉宽步进驱动器无法响应
                int StepLen_ns = 500000;
                rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputStepLen, StepLen_ns);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 脉冲宽度设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                ////设置脉冲模式
                //switch ((int)outmode)
                //{
                //    case 0:  //下降沿脉冲
                //    case 1:  //上升沿脉冲
                //    case 2:  //下降沿脉冲
                //    case 3:  //上升沿脉冲
                //        rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputMode, 0);
                //        break;
                //    case 4:  //下降沿双脉冲
                //    case 5:  //上升沿双脉冲
                //        rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputMode, 1);
                //        break;
                //}
                //switch ((int)outmode)
                //{
                //    case 0:  //下降沿脉冲
                //    case 1:  //上升沿脉冲
                //        //Dir=H 正转
                //        rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_InvertOutputDirection, 0);
                //        break;
                //    case 2:  //下降沿脉冲
                //    case 3:  //上升沿脉冲
                //        //Dir=L 正转
                //        rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_InvertOutputDirection, 1);
                //        break;
                //    case 4:  //下降沿双脉冲
                //    case 5:  //上升沿双脉冲
                //        break;
                //}
                //if (rc != 0)
                //{
                //    exMsg = $"控制卡ID[{cardNo}] 脉冲模式设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                //    throw new Exception($"{exMsg}!");
                //}

                //软极限设置
                if (this._setting.MotorTable.Enable_SoftLimit)
                {
                    double minpos = this._setting.MotorTable.MinDistance_SoftLimit;
                    double maxpos = this._setting.MotorTable.MaxDistance_SoftLimit;

                    if (this._setting.MotorTable.IsFormulaAxis)
                    {
                        minpos = this.FormulaCalc_UnitToAngle(this._setting.MotorTable.MinDistance_SoftLimit);
                        maxpos = this.FormulaCalc_UnitToAngle(this._setting.MotorTable.MaxDistance_SoftLimit);
                    }
                    else
                    {
                        minpos = this._setting.MotorTable.MinDistance_SoftLimit;
                        maxpos = this._setting.MotorTable.MaxDistance_SoftLimit;
                    }

                    rc = LaserX_9078_Utilities.P9078_AxisSetMinPositionLimit(cardNo, axis, minpos);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 最小软极限设定失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }

                    rc = LaserX_9078_Utilities.P9078_AxisSetMaxPositionLimit(cardNo, axis, maxpos);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 最大软极限设定失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }

                    //将当前坐标位置定义为已回零
                    HomeFlagRun();
                }
                else
                {
                    rc = LaserX_9078_Utilities.P9078_AxisSetMinPositionLimit(cardNo, axis, -8e15);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 最小软极限设定失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }

                    rc = LaserX_9078_Utilities.P9078_AxisSetMaxPositionLimit(cardNo, axis, +8e15);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 轴ID[{axis}] 最大软极限设定失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool HomeFlagRun()
        {
            int rc = 0;
            string exMsg = "";

            //cardNo = this._setting.MotorTable.CardNo;
            //axis = this._setting.MotorTable.AxisNo;
            var speed = this._setting.MotorSpeed.Home_Max_Velocity;
            var dir = this._setting.MotorModes.Home_Mode;

            if (dir > 0)
            {
            }
            else
            {
                speed *= -1;
            }

            cardNo = this._setting.MotorTable.CardNo;
            axis = this._setting.MotorTable.AxisNo;
            if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
            {
                rc = LaserX_9078_Utilities.P9078_AxisEnable(cardNo, axis);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] P9078_AxisEnable设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputStepLen, StepLen_ns);

                double currentpos = this.Get_CurUnitPos();
                //设置参数
                rc = LaserX_9078_Utilities.P9078_AxisSetHomingParams(cardNo, axis, currentpos, currentpos, 100, 0, 0, 0);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 设置Home参数失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                _Homed = false;

                //启动回原点运动
                rc = LaserX_9078_Utilities.P9078_AxisHome(cardNo, axis);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 启动Home失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //等等
                //Thread.Sleep(500);

                //Home_sw.Reset();
                //Home_sw.Start();
            }
            return true;
        }

        #region 传感器

        public bool _Senvo { get; private set; }  //伺服状态
        public bool _HomeSensor { get; private set; } //原点传感器
        public bool _Homed { get; private set; } = false; //是否上使能后回过原点

        public bool _Alm { get; private set; }  //报警状态
        public bool _Orign { get; private set; } //外部Home传感器
        public bool _EL_P { get; private set; } //外部+极限
        public bool _EL_N { get; private set; } //外部-极限
        public bool _SL_P { get; private set; } //内部+软极限
        public bool _SL_N { get; private set; } //外部-软极限

        public bool _INSP { get; private set; } //外部到位
        public bool _MOVING { get; private set; } //运动中

        #endregion 传感器

        public LaserX_9078_Utilities.MOT_AXIS_STAT RefreshAxisStatus()
        {
            int rc = 0;
            string exMsg = "";

            LaserX_9078_Utilities.MOT_AXIS_STAT axisInfo = new LaserX_9078_Utilities.MOT_AXIS_STAT();
            if (this._interation.IsSimulation)
            {
                return axisInfo;
            }
            int cardNo = this._setting.MotorTable.CardNo;
            int axis = this._setting.MotorTable.AxisNo;

            if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
            {
                rc = LaserX_9078_Utilities.P9078_MotionUpdate(cardNo);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 刷新更新数据失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                rc = LaserX_9078_Utilities.P9078_MotionGetAxisStatus(cardNo, axis, ref axisInfo, Marshal.SizeOf(axisInfo));
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 获取轴状态失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                _Senvo = (axisInfo.enabled != 0 ? true : false);

                _HomeSensor = (axisInfo.homeSwitch != 0 ? true : false);

                //_Homed = (axisInfo.homed != 0 ? true : false);

                _Orign = (axisInfo.homeSwitch != 0 ? true : false);
                _Orign = (axisInfo.homeSwitch != 0 ? true : false);

                _EL_P = (axisInfo.maxHardLimit != 0 ? true : false);
                _EL_N = (axisInfo.minHardLimit != 0 ? true : false);

                _SL_P = (axisInfo.maxSoftLimit != 0 ? true : false);
                _SL_N = (axisInfo.minSoftLimit != 0 ? true : false);

                _Alm = (axisInfo.error != 0 ? true : false);

                _MOVING = (axisInfo.inpos != 0 ? false : true);    //运动中
                _INSP = (axisInfo.inpos != 0 ? true : false);   //模拟外部到位信号

                //Dictionary<uint, uint> address = new Dictionary<uint, uint>();
                //for(uint i=0;i<=128;i+=4)
                //{
                //    uint d = 0;
                //    LaserX_9078_Utilities.P9078_MotionInd(cardNo, i, ref d);

                //    address.Add(i, d);
                //}

                //string msg = "";
                //foreach(var item in address)
                //{
                //    msg += $"[{item.Key}][0x{item.Value.ToString("X8")}][{item.Value}]\r\n";
                //}
                //Debug.Print(msg);
            }

            return axisInfo;
        }

        public double ConvertPulseToPosition(double pulse)
        {
            //                              pls/转 / mm/转  *

            //return (float)(pulse * this._setting.MotorTable.UnitOfRound) / this._setting.MotorTable.Resolution;
             return Math.Round((pulse * this._setting.MotorTable.UnitOfRound) / this._setting.MotorTable.Resolution, DigitalDigits);
       }

        public override int MoveToV3(double UnitPos, SpeedType speedType, SpeedLevel speedLevel)
        {
            int rc = 0;
            string exMsg = "";

            int errorCode = ErrorCodes.NoError;
            //int cardNo = this._setting.MotorTable.CardNo;
            //axis = this._setting.MotorTable.AxisNo;
            var speed = this._setting.MotorSpeed.Auto_Max_Velocity;

            switch (speedLevel)
            {
                case SpeedLevel.High:
                case SpeedLevel.Normal:
                    {
                        speed = this._setting.MotorSpeed.Auto_Max_Velocity;
                    }
                    break;

                case SpeedLevel.Low:
                    {
                        speed = this._setting.MotorSpeed.Auto_Low_Max_Velocity;
                    }
                    break;
            }

            return MoveToV3(UnitPos, speed);

        }
        public int MoveToV3(double UnitPos, double speed)
        {
            int rc = 0;
            string exMsg = "";

            int errorCode = ErrorCodes.NoError;
            //int cardNo = this._setting.MotorTable.CardNo;
            //axis = this._setting.MotorTable.AxisNo;
            if( speed > this._setting.MotorSpeed.Auto_Max_Velocity)
            {
                speed = this._setting.MotorSpeed.Auto_Max_Velocity;
            }
;
            var Currentpos = Get_CurUnitPos();

            UnitPos = Math.Round(UnitPos, DigitalDigits, MidpointRounding.AwayFromZero);

            cardNo = this._setting.MotorTable.CardNo;
            axis = this._setting.MotorTable.AxisNo;

            Debug.WriteLine($"cardNo={cardNo} axis={axis} MoveToV3 Currentpos={Currentpos} pos={UnitPos}");

            double pos = UnitPos;

            if (this._setting.MotorTable.IsFormulaAxis)
            {
                pos = this.FormulaCalc_UnitToAngle(UnitPos);
            }

            this._interation.PlanPosition = pos;


            if (Currentpos != pos)   
            {
                if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
                {
                    if (Math.Abs(Currentpos - pos) < ConvertPulseToPosition(1))  //如果要运动不足1个脉冲的要求
                    {
                        if (this._setting.MotorTable.IsFormulaAxis ==false) //20230627 修复错误
                        {
                            //运行一个脉冲
                            if (pos > Currentpos)
                                pos = Currentpos + ConvertPulseToPosition(1);
                            else
                                pos = Currentpos - ConvertPulseToPosition(1);
                        }
                    }

                    ///2024-8-2 yxw 控制轴未完全停止时设置PN_OutputStepLen参数反而会导致问题
                    //rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputStepLen, StepLen_ns);
                    //if (rc != 0)
                    //{
                    //    exMsg = $"控制卡ID[{cardNo}] 绝对运动失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    //    throw new Exception($"{exMsg}!");
                    //}
                    
                    rc = LaserX_9078_Utilities.P9078_AxisAbsJog(cardNo, axis, pos, speed);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] 绝对运动失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }
                }
            }
            return errorCode;
        }

        //减速停止
        public override bool Stop()
        {
            int rc = 0;
            string exMsg = "";

            cardNo = this._setting.MotorTable.CardNo;
            axis = this._setting.MotorTable.AxisNo;

            if (this._interation.IsSimulation)
            {
                return true;
            }
            else
            {
                if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
                {
                    rc = LaserX_9078_Utilities.P9078_AxisAbort(cardNo, this._setting.MotorTable.AxisNo);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] P9078_AxisAbort失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }
                }
                return true;
            }
        }

        //立即停止
        public bool StopImmediately()
        {
            int rc = 0;
            string exMsg = "";

            //int cardNo = this._setting.MotorTable.CardNo;
            //int axis = this._setting.MotorTable.AxisNo;

            if (this._interation.IsSimulation)
            {
                return true;
            }
            else
            {
                cardNo = this._setting.MotorTable.CardNo;
                axis = this._setting.MotorTable.AxisNo;

                if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
                {
                    rc = LaserX_9078_Utilities.P9078_AxisAbort(cardNo, this._setting.MotorTable.AxisNo);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{cardNo}] P9078_AxisAbort失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                        throw new Exception($"{exMsg}!");
                    }

                    DateTime st = DateTime.Now;

                    //等待轴停止
                    while (true)
                    {
                        TimeSpan ts = DateTime.Now - st;

                        RefreshAxisStatus();
                        if (_INSP)
                        {
                            return true;
                            break;
                        }
                        if (ts.TotalMilliseconds > this._setting.MotorTable.MotionTimeOut)
                            return false;
                    }
                }
                return true;
            }
        }

        public override void Set_Servo(bool on)
        {
            int rc = 0;
            string exMsg = "";

            do
            {
                if (this._interation.IsSimulation)
                {
                    this._interation.IsServoOn = on;

                    break;
                }

                cardNo = this._setting.MotorTable.CardNo;
                axis = this._setting.MotorTable.AxisNo;

                if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
                {
                    if (on)
                    {//上使能
                        rc = LaserX_9078_Utilities.P9078_AxisEnable(cardNo, axis);
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{cardNo}] P9078_AxisEnable设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            throw new Exception($"{exMsg}!");
                        }
                    }
                    else
                    {//下使能
                        _Homed = false;
                        //20221205 平台总是下伺服, 导致电动机出现自由的状态, 由于平台在多设备使用, 因此此处想禁止下伺服
                        if (true)
                        {
                            rc = LaserX_9078_Utilities.P9078_AxisDisable(cardNo, axis);
                            if (rc != 0)
                            {
                                exMsg = $"控制卡ID[{cardNo}] P9078_AxisDisable设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                                throw new Exception($"{exMsg}!");
                            }
                        }
                    }
                }
            }
            while (false);
        }

        #region 信号量

        public override bool Get_PEL_Signal()
        {
            //+极限传感器
            return (_EL_P || _SL_P);
        }

        public override bool Get_NEL_Signal()
        {
            //-极限传感器
            return (_EL_N || _SL_N);
        }

        public override bool Get_InPos_Signal()
        {
            return _INSP;
        }

        public override bool Get_Alarm_Signal()
        {
            return _Alm;
        }

        public override bool Get_Origin_Signal()
        {
            return _Orign;
        }

        public bool Get_Homed_Signal()
        {
            return _Homed;
        }

        public override bool IsMotorMoving()
        {
            return _MOVING & _Senvo; //20221208 供应商说要这样改
        }

        #endregion 信号量

        public override double Get_CurPulse()
        {
            //得到脉冲位置
            LaserX_9078_Utilities.MOT_AXIS_STAT axisInfo = RefreshAxisStatus();

            double UnitPos = 0;
            double pos = Math.Round(axisInfo.actPos, DigitalDigits, MidpointRounding.AwayFromZero);      //物理位置0.000000
            //double pos = Math.Round(axisInfo.cmdPos, DigitalDigits, MidpointRounding.AwayFromZero);      //物理位置0.000000

            UnitPos = pos; //当前位置, 无需公式转换

            return UnitPos;
        }

        public override double Get_CurUnitPos()
        {
            //得到脉冲位置
            LaserX_9078_Utilities.MOT_AXIS_STAT axisInfo = RefreshAxisStatus();

            double UnitPos = 0;
            double pos = Math.Round(axisInfo.cmdPos, DigitalDigits, MidpointRounding.AwayFromZero);      //物理位置0.000000

            //根据公式轴, 需要再转换一次
            if (this._setting.MotorTable.IsFormulaAxis)//   _CurrentAxisConfig.AxisType == AxisTypeTable.Linear_Formula_Axis)
            {
                UnitPos = this.FormulaCalc_AngleToUnit(pos);
            }
            else
            {
                UnitPos = pos;
            }

            return UnitPos;
        }

        public override double Get_AnalogInputValue()
        {
            //无功能
            return 0.0;
        }

        public override bool Get_ServoStatus()
        {
            return _Senvo;
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

                RefreshAxisStatus();
                if (_INSP)
                {
                    errorCode = ErrorCodes.NoError;
                    break;
                }
                if (isStopReq)
                {
                    break;
                }
                if (ts.TotalMilliseconds > this._setting.MotorTable.MotionTimeOut)
                    return ErrorCodes.MotorMoveTimeOutError;
            }
            return errorCode;
        }

        #region Home功能

        private Stopwatch Home_sw = new Stopwatch();

        public override bool HomeRun()
        {
            int rc = 0;
            string exMsg = "";

            //cardNo = this._setting.MotorTable.CardNo;
            //axis = this._setting.MotorTable.AxisNo;
            var speed = this._setting.MotorSpeed.Home_Max_Velocity;
            var dir = this._setting.MotorModes.Home_Mode;
            var Edge = this._setting.MotorModes.ORG_JumpEdge;

            if (dir > 0)
            {
            }
            else
            {
                speed *= -1;
            }

            
            if (Edge > 0)
            {
                //Home传感器上升沿
                Edge = 1;
            }
            else
            {
                //Home传感器下降沿
                Edge = -1;
            }

            cardNo = this._setting.MotorTable.CardNo;
            axis = this._setting.MotorTable.AxisNo;
            double offset = this._setting.MotorModes.Home_NewPos;
            if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
            {
#if true
                //2024-8-2 yxw 确认控制轴完全停止后再设置PN_OutputStepLen参数
                LaserX_9078_Utilities.MOT_AXIS_STAT axisStat = default(LaserX_9078_Utilities.MOT_AXIS_STAT);
                int stepLen_ns = 10000;

                //PlsEquiv  - 脉冲/mm
                double PlsEquiv = this._setting.MotorTable.Resolution / this._setting.MotorTable.UnitOfRound;
                if (PlsEquiv != 0)
                {
                    stepLen_ns = (int)(0.5 * 1000000000.0 / (Math.Abs(speed) * PlsEquiv));
                }

                rc = LaserX_9078_Utilities.P9078_MotionUpdate(cardNo);
                if (rc == 0)
                {
                    LaserX_9078_Utilities.P9078_MotionGetAxisStatus(cardNo, axis, ref axisStat, System.Runtime.InteropServices.Marshal.SizeOf(axisStat));
                    if (axisStat.inpos != 0)
                    {
                rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputStepLen, stepLen_ns);
                    }
                }
#endif
                //设置参数
                rc = LaserX_9078_Utilities.P9078_AxisSetHomingParams(cardNo, axis, 0, offset, speed * 0.5, speed, speed * 0.1* Edge, 0);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 设置Home参数失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                _Homed = false;

                //启动回原点运动
                rc = LaserX_9078_Utilities.P9078_AxisHome(cardNo, axis);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] 启动Home失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //等等
                Thread.Sleep(500);

                Home_sw.Reset();
                Home_sw.Start();
            }
            return true;
        }

        public override int WaitHomeDone(CancellationTokenSource tokenSource)
        {
            int errorCode = ErrorCodes.NoError;

            if (this._interation.IsSimulation)
            {
                this.isMoving = false;
                return errorCode;
            }
            if (tokenSource.IsCancellationRequested == true)
            {
                errorCode = ErrorCodes.UserReqestStop;
                return errorCode;
            }
            Thread.Sleep(10);


            cardNo = this._setting.MotorTable.CardNo;
            axis = this._setting.MotorTable.AxisNo;

            if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
            {
                //读取HOME状态
                LaserX_9078_Utilities.MOT_AXIS_STAT axisInfo = RefreshAxisStatus();
                var timeout = this._setting.MotorTable.HomeTimeOut;
                do
                {
                    axisInfo = RefreshAxisStatus();

                    if (Home_sw.ElapsedMilliseconds > timeout)
                    {
                        Stop();
                        errorCode = ErrorCodes.MotorMoveTimeOutError;
                        return errorCode;
                    }

                    if (tokenSource.IsCancellationRequested == true)
                    {
                        StopImmediately();
                        errorCode = ErrorCodes.UserReqestStop;
                        return errorCode;
                    }
                    //System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(10);
                }
                while (axisInfo.homing == 1);   //Home中

                //读取回零状态
                axisInfo = RefreshAxisStatus();
                int homeresult = axisInfo.homed;

                if (homeresult == 0)
                {
                    errorCode = ErrorCodes.MotorHomingError;
                    return errorCode;
                }

                //this.WaitForMotionComleted(token);

                //没有退出
                if (tokenSource.IsCancellationRequested == false)
                {
                    SetCurrentPositionToZero();

                    _Homed = true;
                    return errorCode;
                }
            }
            return errorCode;
        }

        public override bool SetCurrentPositionToZero()
        {
            int rc = 0;
            string exMsg = "";
            double Setpostion = 0;

            cardNo = this._setting.MotorTable.CardNo;
            axis = this._setting.MotorTable.AxisNo;

            if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
            {
                Thread.Sleep(5);
                rc = LaserX_9078_Utilities.P9078_AxisSetCommandPosition(cardNo, axis, Setpostion);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] P9078_AxisSetCommandPosition设置[{Setpostion}]失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";

                    //throw new Exception($"{exMsg}!");
                    return false;
                }
				if (Setpostion == 0)
	            {
	                //编码器位置设定到0
	                rc = LaserX_9078_Utilities.P9078_AxisSetActualPosition(cardNo, axis, 0);
					if (rc != 0)
	                {
	                    exMsg = $"控制卡ID[{cardNo}] P9078_AxisSetActualPosition设置[{Setpostion}]失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";

	                    //throw new Exception($"{exMsg}!");
	                    return false;
	                }
	            }
            }
            return true;
        }

        #endregion Home功能

        //无寻相功能
        public override bool PhaseSearching(int timeout_s)
        {
            return false;
        }

        public override void Jog(bool isPositive)
        {
            int rc = 0;
            string exMsg = "";


            cardNo = this._setting.MotorTable.CardNo;
            axis = this._setting.MotorTable.AxisNo;

            if (this._interation.IsSimulation) return;

            float dir = isPositive ? 1.0f : -1.0f;

            var maxVel = Convert.ToSingle(_setting.MotorSpeed.Jog_Max_Velocity * dir);

            if (LaserX_9078_Utilities.CardIDList.Contains(cardNo) && (0 <= axis && axis < LaserX_9078_Utilities.MOT_MAX_AXIS))
            {
                //2024-8-2 yxw 控制轴未完全停止时设置PN_OutputStepLen参数反而会导致问题
                //rc = LaserX_9078_Utilities.P9078_AxisSetParameter(cardNo, axis, (int)LaserX_9078_Utilities.PN_NUMBER.PN_OutputStepLen, StepLen_ns);

                //速度模式运动
                rc = LaserX_9078_Utilities.P9078_AxisContJog(cardNo, axis, maxVel);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{cardNo}] JOG运动失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    //throw new Exception($"{exMsg}!");
                }
            }
        }

        public override int Get_IO_sts()
        {
            return 0;
        }

        public override bool Clear_AlarmSignal()
        {
            return true;
        }
    }
}