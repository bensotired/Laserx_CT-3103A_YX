using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Rs_I : CalcRecipe
    {
        public CalcRecipe_LIV_Rs_I()
        {
            StartCurrent_mA = 0;
            StopCurrent_mA = 1;
        }

        [DisplayName("Rs_I开始电流点")]
        [PropEditable(true)]
        public double StartCurrent_mA { get; set; }

        [DisplayName("Rs_I结束电流点")]
        [PropEditable(true)]
        public double StopCurrent_mA { get; set; }
    }
}
