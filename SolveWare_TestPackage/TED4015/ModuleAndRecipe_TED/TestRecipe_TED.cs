using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_TED : TestRecipeBase
    {
        public TestRecipe_TED()
        {
            OnOrOff = false;
            TemperatureSetpoint = 25;
            TemperatureTolerance = 0.03;
            Temperature_StabilizingTime_sec = 15;
            CurrentLimit_mA = 5;
            PID = "1.0,0.1,0.0,1";
            RTB = "10000,25,3930";

            Timeout_Min = 10;
            TemperaturePenetration_S = 15;
        }

        [DisplayName("开启控温或关闭控温")]
        [Description("OnOrOff")]
        [PropEditable(true)]
        public bool OnOrOff { get; set; }

        [DisplayName("目标温度")]
        [Description("TemperatureSetpoint")]
        [PropEditable(true)]
        public double TemperatureSetpoint { get; set; }

        [DisplayName("温度容差")]
        [Description("Temperature Tolerance")]
        [PropEditable(true)]
        public double TemperatureTolerance { get; set; }

        [DisplayName("温度稳定时间")]
        [Description("Temperature TempControl_StabilizingTime_sec")]
        [PropEditable(true)]
        public double Temperature_StabilizingTime_sec { get; set; }

        [DisplayName("限制电流")]
        [Description("CurrentLimit_mA")]
        [PropEditable(true)]
        public double CurrentLimit_mA { get; set; }

        [DisplayName("PID")]
        [Description("P,I,D,oscillation period")]
        [PropEditable(true)]
        public string PID { get; set; }

        [DisplayName("R0,T0,BETA")]
        [Description("R0,T0,BETA")]
        [PropEditable(true)]
        public string RTB { get; set; }

        [DisplayName("Timeout_Min")]
        [Description("Timeout_Min")]
        [PropEditable(true)]
        public double Timeout_Min { get; set; }

        [DisplayName("TemperaturePenetration_S")]
        [Description("TemperaturePenetration_S")]
        [PropEditable(true)]
        public double TemperaturePenetration_S { get; set; }
    }
}