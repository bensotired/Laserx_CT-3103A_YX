using SolveWare_BurnInCommon;
using System;
using System.ComponentModel;

namespace SolveWare_Motion
{
    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class MotorSpeed
    {
        // 复位专区
        [Category("Home Speed")]
        [DisplayName("复位初速")]
        [Description("Start velocity")]
        [PropEditable(true)]
        public double Home_Start_Velocity { get; set; }

        [Category("Home Speed")]
        [DisplayName("复位最大速度")]
        [Description("Max velocity")]
        [PropEditable(true)]
    
        public double Home_Max_Velocity { get; set; }

        [Category("Home Speed")]
        [DisplayName("复位加速度")]
        [Description("Acceleration")]
        [PropEditable(true)]
        public double Home_Acceleration { get; set; }

        [Category("Home Speed")]
        [DisplayName("复位减速度")]
        [Description("Deceleration")]
        [PropEditable(true)]
        public double Home_Deceleration { get; set; }

        [Category("Home Speed")]
        [DisplayName("复位拉力")]
        [Description("Jerk")]
        [PropEditable(true)]
        public double Home_Jerk { get; set; }


        //自动专区
        [Category("Auto Speed")]
        [DisplayName("自动运行初速")]
        [Description("Start velocity")]
        [PropEditable(true)]
        public double Auto_Start_Velocity { get; set; }

        [Category("Auto Speed")]
        [DisplayName("自动运行最大速度")]
        [Description("Max velocity")]
        [PropEditable(true)]
        public double Auto_Max_Velocity { get; set; }

        [Category("Auto Speed")]
        [DisplayName("自动运行加速度")]
        [Description("Acceleration")]
        [PropEditable(true)]
        public double Auto_Acceleration { get; set; }

        [Category("Auto Speed")]
        [DisplayName("自动运行减速度")]
        [Description("Deceleration")]
        [PropEditable(true)]
        public double Auto_Deceleration { get; set; }

        [Category("Auto Speed")]
        [DisplayName("自动运行平滑时间(毫秒)")]
        [Description("Auto_SmoothingTime")]
        [PropEditable(true)]
        public double Auto_SmoothingTime { get; set; }

        [Category("Auto Speed")]
        [DisplayName("自动运行拉力")]
        [Description("Jerk")]
        [PropEditable(true)]
        public double Auto_Jerk { get; set; }

        [Category("Auto Speed")]
        [DisplayName("自动运行初速_低速")]
        [Description("Start velocity")]
        [PropEditable(true)]
        public double Auto_Low_Start_Velocity { get; set; }

        [Category("Auto Speed Low Level")]
        [DisplayName("自动运行最大速度_低速")]
        [Description("Max velocity")]
        [PropEditable(true)]
        public double Auto_Low_Max_Velocity { get; set; }

        [Category("Auto Speed Low Level")]
        [DisplayName("自动运行加速度_低速")]
        [Description("Acceleration")]
        [PropEditable(true)]
        public double Auto_Low_Acceleration { get; set; }

        [Category("Auto Speed Low Level")]
        [DisplayName("自动运行减速度_低速")]
        [Description("Deceleration")]
        [PropEditable(true)]
        public double Auto_Low_Deceleration { get; set; }

        [Category("Auto Speed Low Level")]
        [DisplayName("自动运行平滑时间(毫秒)_低速")]
        [Description("Auto_SmoothingTime")]
        [PropEditable(true)]
        public double Auto_Low_SmoothingTime { get; set; }

        [Category("Auto Speed Low Level")]
        [DisplayName("自动运行拉力_低速")]
        [Description("Jerk")]
        [PropEditable(true)]
        public double Auto_Low_Jerk { get; set; }





        //Jog专区
        [Category("Jog Speed")]
        [DisplayName("Jog初速")]
        [Description("Start velocity")]
        [PropEditable(true)]
        public double Jog_Start_Velocity { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Jog最大速度")]
        [Description("Max velocity")]
        [PropEditable(true)]
        public double Jog_Max_Velocity { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Jog加速度")]
        [Description("Acceleration")]
        [PropEditable(true)]
        public double Jog_Acceleration { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Jog减速度")]
        [Description("Deceleration")]
        [PropEditable(true)]
        public double Jog_Deceleration { get; set; }


        [Category("Jog Speed")]
        [DisplayName("Jog运行平滑时间(毫秒)")]
        [Description("Jog_SmoothingTime")]
        [PropEditable(true)]
        public double Jog_SmoothingTime { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Jog拉力")]
        [Description("Jog Jerk")]
        [PropEditable(true)]
        public double Jog_Jerk { get; set; }

        #region ctor
        public MotorSpeed()
        {
            Home_Start_Velocity = 0;
            Home_Max_Velocity = 10;
            Home_Acceleration = 0.1;
            Home_Deceleration = 0.1;
            Home_Jerk = 1;

            Auto_Start_Velocity = 0;
            Auto_Max_Velocity = 10;
            Auto_Acceleration = 0.1;
            Auto_Deceleration = 0.1;
            Auto_SmoothingTime = 10.0;
            Auto_Jerk = 1;

            Jog_Start_Velocity = 0;
            Jog_Max_Velocity = 10;
            Jog_Acceleration = 0.1;
            Jog_Deceleration = 0.1;
            Jog_SmoothingTime = 0.8;
            Jog_Jerk = 1;
        }
        #endregion
    }
}
