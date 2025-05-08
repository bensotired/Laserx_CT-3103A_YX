using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class CalcRecipe_IRev : CalcRecipe
    {
        public CalcRecipe_IRev()
        {

        }
        [DisplayName("Demo")]
        [PropEditable(true)]
        public double Demo { get; set; }
    }
}
