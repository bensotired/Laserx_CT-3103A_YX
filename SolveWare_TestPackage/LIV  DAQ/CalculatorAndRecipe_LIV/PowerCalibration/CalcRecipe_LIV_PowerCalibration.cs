using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_PowerCalibration : CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_LIV_PowerCalibration()
        {
            Coeff_K = 41742.28675;
            Coeff_B = 0;
        }
        [DisplayName("功率校正(斜率)系数 K")]
        [PropEditable(true)]
        public double Coeff_K { get; set; }
        [DisplayName("功率校正(截距)系数 B")]
        [PropEditable(true)]
        public double Coeff_B { get; set; }

    }
}
