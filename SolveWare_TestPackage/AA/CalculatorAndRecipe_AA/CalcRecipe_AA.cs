using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;


namespace SolveWare_TestPackage
{
    public class CalcRecipe_AA : CalcRecipe
    {
        public CalcRecipe_AA()
        {

        }
        [DisplayName("Demo")]
        [PropEditable(true)]
        public double Demo { get; set; }
    }
}
