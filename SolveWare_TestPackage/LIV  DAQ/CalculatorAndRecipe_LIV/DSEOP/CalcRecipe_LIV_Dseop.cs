using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Dseop : CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_LIV_Dseop()
        {
            SE_Power1 = 10;
            SE_Power2 = 20;
        }
        [DisplayName("SE功率点1(mW)")]
        [PropEditable(true)]
        public double SE_Power1 { get; set; }

        [DisplayName("SE功率点2(mW)")]
        [PropEditable(true)]
        public double SE_Power2 { get; set; }
    }
}
