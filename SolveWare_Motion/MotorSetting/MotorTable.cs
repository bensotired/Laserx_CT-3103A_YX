using SolveWare_BurnInCommon;
using System;
using System.ComponentModel;

namespace SolveWare_Motion
{
    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class MotorTable
    {
        //马达名称
        [Category("Axis Table")]
        [DisplayName("马达名称")]
        [Description("Axis Name")]
        [PropEditable(true)]
        public string Name { get; set; }

        [Category("Axis Table")]
        [DisplayName("控制卡号")]
        [Description("Card #No")]
        [PropEditable(true)]
        public short CardNo { get; set; }

        [Category("Axis Table")]
        [DisplayName("马达轴号")]
        [Description("Axis #No")]
        [PropEditable(true)]
        public short AxisNo { get; set; }

        [Category("Axis Table")]
        [DisplayName("马达使能逻辑")]
        [Description("Servo On Logic")]
        [PropEditable(true)]
        public short ServoOn_Logic { get; set; }

        [Category("Axis Table")]
        [DisplayName("丝杆导程(mm)")]
        [Description("导程(马达每圈所移动的导轨距离)")]
        [PropEditable(true)]
        public double UnitOfRound { get; set; }

        [Category("Axis Table")]
        [DisplayName("驱动器分辨率(脉冲/圈)")]
        [Description("分辨率 (驱动器驱动马达走一圈所需脉冲数)")]
        [PropEditable(true)]
        public double Resolution { get; set; }

        [Category("Axis Table")]
        [DisplayName("编码器源 [0]忽略 [1]脉冲回环 [2]编码器")]
        [Description("Is Formula Axis")]
        [PropEditable(true)]
        public eEncoderType EncoderSource { get; set; }

        [Category("Axis Table")]
        [DisplayName("编码器分辨率(脉冲/圈)")]
        [Description("分辨率 (驱动器驱动马达走一圈编码器脉冲数)")]
        [PropEditable(true)]
        public double EncoderResolution { get; set; }

        //[Category("Axis Table")]
        //[DisplayName("PulseFactor")]
        //[Description("PulseFactor")]
        //[PropEditable(true)]
        //public double PulseFactor { get; set; }

        [Category("Axis Table")]
        [DisplayName("类型(保留参数)")]
        [Description("AxisType")]
        [PropEditable(true)]
        public eAxisType AxisType { get; set; }


        [Category("Axis Table")]
        [DisplayName("是否需要使用公式换算")]
        [Description("Is Formula Axis")]
        [PropEditable(true)]
        public bool IsFormulaAxis { get; set; }

        [Category("Axis Table")]
        [DisplayName("转换公式(Angle->Unit)")]
        [Description("电机旋转角(Deg) -> 单位(mm/Deg)")]
        [PropEditable(true)]
        public string Formula_AngleToUnit { get; set; }

        [Category("Axis Table")]
        [DisplayName("转换公式(Angle<-Unit)")]
        [Description("电机旋转角(Deg) <- 单位(mm/Deg)")]
        [PropEditable(true)]
        public string Formula_UnitToAngle { get; set; }

        [Category("Axis Table")]
        [DisplayName("需做寻相动作")]
        [Description("IsPhaseSearchNeeded")]
        [PropEditable(true)]
        public bool IsPhaseSearchNeeded { get; set; }

        [Category("Axis Table")]
        [DisplayName("丝杆回差(mm)")]
        [Description("Backlash")]
        [PropEditable(true)]
        public double Backlash { get; set; }

        //[Category("Axis Table")]
        //[DisplayName("Decimal Digit 小数点位数")]
        //[Description("Decimal Digit")]
        //[PropEditable(true)]
        //public int Decimal_Digit { get; set; }

        ////马达控制器
        //[Category("Motor Master Driver")]
        //[DisplayName("马达总控类型")]
        //[Description("MotorMasterDriver")]
        //[PropEditable(true)]
        //public MotorMasterDriver MotorMasterDriver { get; set; }


        ////马达数值
        //[Category("Motor Step Unit")]
        //[DisplayName("MM 马达显示单位")]
        //[Description("Motor Step Unit")]
        //public bool IsMM { get; set; }


        //In Position Check
        [Category("In Position Check")]
        [DisplayName("到位置允许偏差")]
        [Description("AcceptableInPositionOffset")]
        [PropEditable(true)]
        public double AcceptableInPositionOffset { get; set; }


        //移动方向
        //[Category("Direction")]
        //[DisplayName("Direction 移动方向 (0:负 1:正)")]
        //[Description("Direction")]
        //[PropEditable(true)]
        //public short Dir { get; set; }

        //[Category("Direction")]
        //[DisplayName("HomeDirection 复位方向 (0:负 1:正)")]
        //[Description("Home Direction")]
        //[PropEditable(true)]
        //public short HomeDir { get; set; }


        ////复位选择
        //[Category("On Board")]
        //[DisplayName("OnBoardHome 使用板卡复位")]
        //[Description("On board home (0:No/1:Yes)")]
        //public short OnBoardHome { get; set; }


        //状态读取间隔
        [Category("Status Read Timing(毫秒)")]
        [DisplayName("状态读取时间间隔")]
        [Description("Timing")]
        [PropEditable(true)]
        public int StatusReadTiming { get; set; }


        ////自定义 复位设定
        //[Category("Custom Home Setting")]
        //[Description("Custom Home Type (0:NEL/1:PEL/2:MID)")]
        //[DisplayName("CustomHomeType")]
        //[PropEditable(true)]
        //public CustomHomeType CustomHomeType { get; set; }

        //[Category("Custom Home Setting")]
        //[DisplayName("Custome Home StepSize")]
        //[Description("Custom Home Type (0:NEL/1:PEL/2:MID)")]
        //[PropEditable(true)]
        //public double CustomHomeStepSize { get; set; }


        //超时设定
        [Category("Time Out")]
        [DisplayName("移动超时(毫秒)")]
        [Description("Motion Time Out in mili-second")]
        [PropEditable(true)]
        public int MotionTimeOut { get; set; }

        [Category("Time Out")]
        [DisplayName("复位超时(毫秒)")]
        [Description("Home Time Out in mili-second")]
        [PropEditable(true)]
        public int HomeTimeOut { get; set; }


        //软限位
        [Category("Soft Limit")]
        [DisplayName("启用软限位")]
        [Description("Enable Soft Limit")]
        [PropEditable(true)]
        public bool Enable_SoftLimit { get; set; }

        [Category("Soft Limit")]
        [DisplayName("最大软限位(绝对位置mm)")]
        [Description("Enable Soft Limit")]
        [PropEditable(true)]
        public double MaxDistance_SoftLimit { get; set; }

        [Category("Soft Limit")]
        [DisplayName("最小软限位(绝对位置mm)")]
        [Description("Enable Soft Limit")]
        [PropEditable(true)]
        public double MinDistance_SoftLimit { get; set; }


        [PropEditable(false)]
        public int Dir { get; set; }


        [PropEditable(false)]
        public ushort MotorRatedCurrent_mA { get; set; }

        #region ctor
        public MotorTable()
        {
            Backlash = 0;
            Resolution = 1000;
            UnitOfRound = 1;
            //OnBoardHome = 1;
            StatusReadTiming = 1;

            //轴类型
            AxisType = eAxisType.Linear_Axis;

            //编码器源
            EncoderSource = eEncoderType.Ignore;

            //公式轴
            Formula_AngleToUnit = "";
            Formula_UnitToAngle = "";
        }
        #endregion
        public enum eAxisType
        {
            Linear_Axis,                    //线性运动轴
            Rotation_Axis,                  //旋转轴

            Linear_Formula_Axis,            //线性公式轴
        }

        public enum eEncoderType
        {
            Ignore,     //忽略                   
            PulseLoop,  //脉冲回环  
            Encoder,    //真实编码器    
        }
    }
}
