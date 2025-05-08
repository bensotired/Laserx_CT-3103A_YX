using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_LiERR : CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_LIV_LiERR()
        {
            SE_PowerStart_mW = 1;
            SE_PowerEnd_mW = 2;
            CurrentSet_mA = 10;
        }
        [DisplayName("SE开始功率点")]
        [PropEditable(true)]
        public double SE_PowerStart_mW { get; set; }

        [DisplayName("SE结束功率点")]
        [PropEditable(true)]
        public double SE_PowerEnd_mW { get; set; }

        [DisplayName("设定的电流点")]
        [PropEditable(true)]
        public double CurrentSet_mA { get; set; }
    }
}
