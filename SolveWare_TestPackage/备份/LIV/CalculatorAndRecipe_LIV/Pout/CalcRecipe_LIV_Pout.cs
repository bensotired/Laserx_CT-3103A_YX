using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Pout : CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_LIV_Pout()
        {
            Current_mA = 1;
        }
        [DisplayName("定电流测试功率的输入电流")]
        [PropEditable(true)]
        public double Current_mA { get; set; }

      
    }
}
