using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Lipo2 : CalcRecipe
    {
        public CalcRecipe_LIV_Lipo2()
        {
            PowerStart1_mW = 1;
            PowerEnd1_mW = 2;
            PowerStart2_mW = 4;
            PowerEnd2_mW = 5;
        }

        [DisplayName("第一个SE开始功率(mW)")]
        [PropEditable(true)]
        public double PowerStart1_mW { get; set; }

        [DisplayName("第一个SE结束功率(mW)")]
        [PropEditable(true)]
        public double PowerEnd1_mW { get; set; }

        [DisplayName("第二个SE开始功率(mW)")]
        [PropEditable(true)]
        public double PowerStart2_mW { get; set; }

        [DisplayName("第二个SE结束功率(mW)")]
        [PropEditable(true)]
        public double PowerEnd2_mW{ get; set; }
    }
}
