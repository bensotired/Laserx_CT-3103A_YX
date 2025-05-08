using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;


namespace SolveWare_TestPackage
{
    public class CalcRecipe_MZMRdiff : CalcRecipe
    {
        public CalcRecipe_MZMRdiff() 
        {
            FirstVoltageSet = -1;
            SecondVoltageSet = 0;
        }
        [DisplayName("计算的第一个点_V")]
        [PropEditable(true)]
        public double FirstVoltageSet { get; set; }
        [DisplayName("计算的第二个点_V")]
        [PropEditable(true)]
        public double SecondVoltageSet { get; set; } 
       
    }
}
