using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_AA_FineTuning : TestRecipeBase
    {
        public TestRecipe_AA_FineTuning()
        {
            this.Inherit = true;
            this.Analog_CH = 1;
            this.InitialCurrentSense_mA = 0.1;

        

            this.PowerThreshold_mA = 0.01;

            this.Fine_Radius = 0.01;
            this.Fine_Involute_Interval = 0.002;
            this.Fine_Trajspeed = 0.05;

            this.Creep_Step_um = 0.2; //蠕动步进
            this.CreepDelay_ms = 400;   //蠕动后等待多久稳定
            this.OutOfFocusDistance_um = 2;
 

            LIVOpticalSwitchChannel = 1;
            SPOpticalSwitchChannel = 2;

        }
        [DisplayName("从QWLT2获取数值")]
        [Description("Inherit")]
        [PropEditable(true)]
        public bool Inherit { get; set; }

        [DisplayName("模拟量通道")]
        [Description("Analog_CH")]
        [PropEditable(true)]
        public int Analog_CH { get; set; }


        //==========================

        [DisplayName("初始PD电流量程(mA)")]
        [Description("InitialCurrentSense_mA")]
        [PropEditable(true)]
        public double InitialCurrentSense_mA { get; set; }

        [DisplayName("耦合PD电流下限(mA)")]
        [Description("PowerThreshold_mA")]
        [PropEditable(true)]
        public double PowerThreshold_mA { get; set; }

 

        [DisplayName("精扫插补速度(mm/s)")]
        [Description("Fine_Trajspeed")]
        [PropEditable(true)]
        public double Fine_Trajspeed { get; set; }

        [DisplayName("精扫半径(mm)")]
        [Description("Fine_Radius")]
        [PropEditable(true)]
        public double Fine_Radius { get; set; }

        [DisplayName("精扫线间隔(mm)")]
        [Description("Fine_Involute_Interval")]
        [PropEditable(true)]
        public double Fine_Involute_Interval { get; set; }

        //==========================

        [DisplayName("蠕动耦合步长(um)")]
        [Description("Creep_Step_um")]
        [PropEditable(true)]
        public double Creep_Step_um { get; set; }

        [DisplayName("蠕动后稳定等待")]
        [Description("CreepDelay_ms")]
        [PropEditable(true)]
        public double CreepDelay_ms { get; set; }

        [DisplayName("蠕动离焦长度")]
        [Description("OutOfFocusDistance_um")]
        [PropEditable(true)]
        public double OutOfFocusDistance_um { get; set; }

        [DisplayName("OSwitch LIV光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int LIVOpticalSwitchChannel { get; set; }

        [DisplayName("OSwitch SP光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int SPOpticalSwitchChannel { get; set; }
    }
}