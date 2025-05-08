using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_SE_mWpermA : CalcRecipe
    {
        public CalcRecipe_LIV_SE_mWpermA()
        {
            StartCurrent_mA = 0;
            StopCurrent_mA = 10;
        }

        [DisplayName("SE开始电流点")]
        [PropEditable(true)]
        public double StartCurrent_mA { get; set; }

        [DisplayName("SE结束电流点")]
        [PropEditable(true)]
        public double StopCurrent_mA { get; set; }
    }
}
