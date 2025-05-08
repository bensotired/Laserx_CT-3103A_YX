using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;
namespace SolveWare_TestPackage
{
    public class TestRecipe_Optica : TestRecipeBase
    {
        public TestRecipe_Optica()
        {

        }

        [DisplayName("开始角度")]
        [PropEditable(true)]
        public double StartAngle { get; set; }
        [DisplayName("停止角度")]
        [PropEditable(true)]
        public double StopAngle { get; set; }

        [DisplayName("步进角度")]
        [PropEditable(true)]
        public double StepAngle { get; set; }

        [DisplayName("TMPL_功率计探头_产品波长")]
        [PropEditable(true)]
        public double TMPL_Wavelength { get; set; }
    }
}
