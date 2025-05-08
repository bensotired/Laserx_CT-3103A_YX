using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class CalcRecipe_DeltaUniversal : CalcRecipe
    {
        [DisplayName("需要计算Delta的参数名")]
        [PropEditable(true)]
        //public double Power_mW { get; set; }
        public string ParamName { get; set; }


    }
}
