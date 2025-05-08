using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class TestRecipe_ChartDemo : TestRecipeBase
    {
        public TestRecipe_ChartDemo()
        {

            X_LimitLower = 0;
            X_LimitUpper = 1200;


            Y1_LimitLower = 0;
            Y1_LimitUpper = 40;

            Y11_LimitLower = -6;
            Y11_LimitUpper = 100;

            Y12_LimitLower = 1;
            Y12_LimitUpper = 30;


            Y2_LimitLower = 100;
            Y2_LimitUpper = 1200;

            Y21_LimitLower = 100;
            Y21_LimitUpper = 1200;

            Y22_LimitLower = 10;
            Y22_LimitUpper = 1200;

        }


        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int X_LimitLower { get; set; }


        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int X_LimitUpper { get; set; }




        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y1_LimitLower { get; set; }

        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y1_LimitUpper { get; set; }



        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y11_LimitLower { get; set; }


        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y11_LimitUpper { get; set; }



        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y12_LimitLower { get; set; }

        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y12_LimitUpper { get; set; }




        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y2_LimitLower { get; set; }

        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y2_LimitUpper { get; set; }



        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y21_LimitLower { get; set; }


        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y21_LimitUpper { get; set; }



        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y22_LimitLower { get; set; }

        [DisplayName("源表LD加电电流(毫安)")]
        [Description("I_mA")]
        [PropEditable(true)]
        public int Y22_LimitUpper { get; set; }



    }
}