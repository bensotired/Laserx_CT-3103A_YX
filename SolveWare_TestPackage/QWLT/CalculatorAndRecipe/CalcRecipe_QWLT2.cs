using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;


namespace SolveWare_TestPackage
{
    public class CalcRecipe_QWLT2 : CalcRecipe
    {
        public CalcRecipe_QWLT2()
        {

        }
        [DisplayName("Demo")]
        [PropEditable(true)]
        public double Demo { get; set; }
    }
}
