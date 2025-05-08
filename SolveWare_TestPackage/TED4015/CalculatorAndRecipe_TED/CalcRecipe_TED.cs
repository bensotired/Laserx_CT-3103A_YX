using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;


namespace SolveWare_TestPackage
{
    public class CalcRecipe_TED : CalcRecipe
    {
        public CalcRecipe_TED()
        {

        }
        [DisplayName("Demo")]
        [PropEditable(true)]
        public double Demo { get; set; }
    }
}
