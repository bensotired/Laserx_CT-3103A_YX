using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_SEop : CalcRecipe
    {
        //POP定功率测试电流  所以输入的是功率
        public CalcRecipe_LIV_SEop()
        {
            Power_mW = 1;
        }
        
        [DisplayName("以光功率为标定计算对应点斜率,此处为作为标定的光功率值")]
        [PropEditable(true)]
        public double Power_mW{ get; set; }

      
    }
}
