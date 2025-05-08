using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Dsex : CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_LIV_Dsex()
        {
            SE_Current1 = 10;
            SE_Current2 = 20;
        }
        [DisplayName("SE电流点1(mA)")]
        [PropEditable(true)]
        public double SE_Current1 { get; set; }

        [DisplayName("SE电流点2(mA)")]
        [PropEditable(true)]
        public double SE_Current2 { get; set; }
    }
}
