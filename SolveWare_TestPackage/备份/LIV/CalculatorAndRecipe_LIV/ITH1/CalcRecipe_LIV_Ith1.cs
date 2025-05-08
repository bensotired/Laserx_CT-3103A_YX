using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Ith1 : CalcRecipe
    {
        public CalcRecipe_LIV_Ith1()
        {
            IthLimitMax_mA = 20;
            Ith1_Cal_Left_Offset_mA = 0;
            Ith1_Cal_Right_Offset_mA = 1;
        }

        [DisplayName("Ith限定最大值")]
        [PropEditable(true)]
        public double IthLimitMax_mA { get; set; }

        [DisplayName("左侧偏移的电流量")]
        [PropEditable(true)]
        public double Ith1_Cal_Left_Offset_mA { get; set; }

        [DisplayName("右侧偏移的电流量")]
        [PropEditable(true)]
        public double Ith1_Cal_Right_Offset_mA { get; set; }
    }
}
