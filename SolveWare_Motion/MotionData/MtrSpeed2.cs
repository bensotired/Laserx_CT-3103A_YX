using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Manager_Motion.Base
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MtrSpeed2
    {
        // 复位专区
        [Category("Home Speed")]
        [DisplayName("Start Velocity 复位初速")]
        [Description("Start velocity")]
        public double Home_Start_Velocity { get; set; }

        [Category("Home Speed")]
        [DisplayName("Max Velocity 复位最大速度")]
        [Description("Max velocity")]
        public double Home_Max_Velocity { get; set; }

        [Category("Home Speed")]
        [DisplayName("Acceleration 复位加速度")]
        [Description("Acceleration")]
        public double Home_Acceleration { get; set; }

        [Category("Home Speed")]
        [DisplayName("Deceleration 复位减速度")]
        [Description("Deceleration")]
        public double Home_Deceleration { get; set; }

        [Category("Home Speed")]
        [DisplayName("Jerk 拉力")]
        [Description("Jerk")]
        public double Home_Jerk { get; set; }


        //自动专区
        [Category("Auto Speed")]
        [DisplayName("Start Velocity 自动初速")]
        [Description("Start velocity")]
        public double Auto_Start_Velocity { get; set; }

        [Category("Auto Speed")]
        [DisplayName("Max Velocity 自动最大速度")]
        [Description("Max velocity")]
        public double Auto_Max_Velocity { get; set; }

        [Category("Auto Speed")]
        [DisplayName("Acceleration 复位加速度")]
        [Description("Acceleration")]
        public double Auto_Acceleration { get; set; }

        [Category("Auto Speed")]
        [DisplayName("Deceleration 复位减速度")]
        [Description("Deceleration")]
        public double Auto_Deceleration { get; set; }

        [Category("Auto Speed")]
        [DisplayName("Jerk 拉力")]
        [Description("Jerk")]
        public double Auto_Jerk { get; set; }


        //Jog专区
        [Category("Jog Speed")]
        [DisplayName("Start Velocity Jog初速")]
        [Description("Start velocity")]
        public double Jog_Start_Velocity { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Max Velocity 自动最大速度")]
        [Description("Max velocity")]
        public double Jog_Max_Velocity { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Acceleration 复位加速度")]
        [Description("Acceleration")]
        public double Jog_Acceleration { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Deceleration 复位减速度")]
        [Description("Deceleration")]
        public double Jog_Deceleration { get; set; }

        [Category("Jog Speed")]
        [DisplayName("Jerk 拉力")]
        [Description("Jerk")]
        public double Jog_Jerk { get; set; }

        #region ctor
        public MtrSpeed2()
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
            Auto_Jerk = 1;

            Jog_Start_Velocity = 0;
            Jog_Max_Velocity = 10;
            Jog_Acceleration = 0.1;
            Jog_Deceleration = 0.1;
            Jog_Jerk = 1;
        }
        #endregion
    }
}
