using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Rs_V : CalcRecipe
    {
        public CalcRecipe_LIV_Rs_V()
        {
            StartVoltage_V = 6;
            StopVoltage_V = 7;
        }

        [DisplayName("Rs_V开始电流点")]
        [PropEditable(true)]
        public double StartVoltage_V { get; set; }

        [DisplayName("Rs_V结束电流点")]
        [PropEditable(true)]
        public double StopVoltage_V { get; set; }
    }
}
