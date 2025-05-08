using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class CalcRecipe_Sample : CalcRecipe
    {
        //IOP定电流测试功率  所以输入的是电流
        public CalcRecipe_Sample()
        {
            Current_mA = 1;
        }
        [DisplayName("定电流测试功率的输入电流")]

        [PropEditable(true)]
        public double Current_mA { get; set; }
    }
}
