using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Kink1_zh : CalcRecipe
    {
        public CalcRecipe_LIV_Kink1_zh() 
        {
            Ith2_StartP_mW = 0;
            Ith2_StopP_mW = 1;
            SEMax_Start_AboveIthCurrent = 10;
            SEMax_End_AboveIthCurrent = 20;
        }

        [DisplayName("ith开始的功率点")]
        [PropEditable(true)]
        public double Ith2_StartP_mW { get; set; }

        [DisplayName("ith结束的功率点")]
        [PropEditable(true)]
        public double Ith2_StopP_mW { get; set; }

        [DisplayName("SE起始电流点(比ith对应电流的增量，例如ith=1，值设定2，起始电流为3)")]
        [PropEditable(true)]
        public double SEMax_Start_AboveIthCurrent { get; set; }

        [DisplayName("SE结束电流点(比ith对应电流的增量，例如ith=1，值设定3，结束电流为4)")]
        [PropEditable(true)]
        public double SEMax_End_AboveIthCurrent { get; set; }

        //[DisplayName("SE0的功率点")]
        //[PropEditable(true)]
        //public double SE0_Power { get; set; }
        [DisplayName("A2点指定的功率")]
        [PropEditable(true)]
        public double A2_Power_mW { get; set; } = 2;
        [DisplayName("A1点指定的电流点(比ith对应电流的增量，例如ith=1，值设定3，结束电流为4)")]
        [PropEditable(true)]
        public double A1_AboveIthCurrent { get; set; } = 30;
        [DisplayName("SE_Line1Y轴的差值")]
        [PropEditable(true)]
        public double SE_Line_YDiff { get; set; } = 0.001;
    }
}
