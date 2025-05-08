using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;
using System;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Pop : CalcRecipe
    {
        //POP定功率测试电流  所以输入的是功率
        public CalcRecipe_LIV_Pop()
        {
            Power_mW = 1;
        }
        
        [DisplayName("定功率测试电流的输入功率")]
        [PropEditable(true)]
        public double Power_mW{ get; set; }

      
    }
}
