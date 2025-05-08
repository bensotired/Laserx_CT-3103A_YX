using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;


namespace SolveWare_TestPackage
{
    public class CalcRecipe_I_Check : CalcRecipe
    {
        public CalcRecipe_I_Check()
        {
            GAIN_Parameter = "0,1";
            SOA1_Parameter = "0,1";
            SOA2_Parameter = "0,1";
            MIRROR1_Parameter = "0,1";
            MIRROR2_Parameter = "0,1";
            PH1_Parameter = "0,1";
            PH2_Parameter = "0,1";
            LP_Parameter = "0,1";

        }
        [DisplayName("GAIN_Parameter mA")]
        [PropEditable(true)]
        public string GAIN_Parameter { get; set; }
        [DisplayName("SOA1_Parameter mA")]
        [PropEditable(true)]
        public string SOA1_Parameter { get; set; }
        [DisplayName("SOA2_Parameter mA")]
        [PropEditable(true)]
        public string SOA2_Parameter { get; set; }
         [DisplayName("MIRROR1_Parameter mA")]
        [PropEditable(true)]
        public string MIRROR1_Parameter { get; set; }
         [DisplayName("MIRROR2_Parameter mA")]
        [PropEditable(true)]
        public string MIRROR2_Parameter { get; set; }
         [DisplayName("PH1_Parameter mA")]
        [PropEditable(true)]
        public string PH1_Parameter { get; set; }
         [DisplayName("PH2_Parameter mA")]
        [PropEditable(true)]
        public string PH2_Parameter { get; set; }
         [DisplayName("LP_Parameter mA")]
        [PropEditable(true)]
        public string LP_Parameter { get; set; }

    }
}
