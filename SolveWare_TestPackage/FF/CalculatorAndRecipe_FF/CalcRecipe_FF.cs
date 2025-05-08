using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;


namespace SolveWare_TestPackage
{
    public class CalcRecipe_FF : CalcRecipe
    {
        public CalcRecipe_FF()
        {

        }
        [DisplayName("Demo")]
        [PropEditable(true)]
        public double Demo { get; set; }
    }
}
