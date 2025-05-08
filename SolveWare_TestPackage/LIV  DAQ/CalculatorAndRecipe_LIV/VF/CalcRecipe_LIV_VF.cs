using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;
using System;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_VF: CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_LIV_VF()
        {
            Current_mA = 60;
        }
        [DisplayName("定电流测试VF的输入电流")]
        [PropEditable(true)]
        public double Current_mA { get; set; }

      
    }
}
