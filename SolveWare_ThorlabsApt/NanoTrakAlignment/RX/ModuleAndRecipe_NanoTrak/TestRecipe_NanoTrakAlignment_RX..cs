using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_NanoTrakAlignment_RX : TestRecipeBase
    {
        public TestRecipe_NanoTrakAlignment_RX()
        {
            //this.DrivingCurrent_mA = 50.0;
            this.Threshold_Factor_Current_mA = 0;
            this.Power_Factor_K = 1;
            this.Power_Factor_B = 0;
            //this.Three_Current_mA_Diff = 0.005;
        }
        //[DisplayName("器件驱动电流(mA)")]
        //[PropEditable(true)]
        //public double DrivingCurrent_mA { get; set; }
        [DisplayName("耦合成功门限光电流I(mA)")]
        [PropEditable(true)]
        public double Threshold_Factor_Current_mA { get; set; }
        //[DisplayName("三次耦合成功与平均差值光电流I(mA)")]
        //[PropEditable(true)]
        //public double Three_Current_mA_Diff { get; set; }
        [DisplayName("耦合成功门限光功率K值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_K { get; set; }
        [DisplayName("耦合成功门限光功率B值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_B { get; set; }
    }
}