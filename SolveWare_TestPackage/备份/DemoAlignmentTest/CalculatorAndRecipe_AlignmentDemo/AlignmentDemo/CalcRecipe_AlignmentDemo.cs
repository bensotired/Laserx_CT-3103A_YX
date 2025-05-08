using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class CalcRecipe_AlignmentDemo : CalcRecipe
    {
 
        public CalcRecipe_AlignmentDemo()
        {
            AnyParamYouWantToUseForCalculatingDueToAlignmentDemo = 1;
        }
        [DisplayName("你想通过耦合Demo计算的参数")]
        [PropEditable(true)]
        public double AnyParamYouWantToUseForCalculatingDueToAlignmentDemo  { get; set; }
    }
}
