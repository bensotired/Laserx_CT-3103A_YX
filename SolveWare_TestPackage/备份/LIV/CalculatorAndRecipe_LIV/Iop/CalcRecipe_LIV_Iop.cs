using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Iop : CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_LIV_Iop()
        {
            Power_mW = 1;
        }
        [DisplayName("计算指定功率下的电流  输入值:功率")]
        [PropEditable(true)]
        public double Power_mW { get; set; }

      
    }
}
