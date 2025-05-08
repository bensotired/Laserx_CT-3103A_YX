using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;


namespace SolveWare_TestPackage
{
    public class CalcRecipe_V_Check : CalcRecipe
    {
        public CalcRecipe_V_Check()
        {
            GAIN_Parameter = "0,2";
            SOA1_Parameter = "0,2";
            SOA2_Parameter = "0,2";
            MIRROR1_Parameter = "0,2";
            MIRROR2_Parameter = "0,2";
            PH1_Parameter = "0,2";
            PH2_Parameter = "0,2";
            LP_Parameter = "0,2";

            MPD1_Parameter = "0,2";
            MPD2_Parameter = "0,2";
            BIAS1_Parameter = "0,2";
            BIAS2_Parameter = "0,2";
        }
        [DisplayName("GAIN_Parameter V")]
        [PropEditable(true)]
        public string GAIN_Parameter { get; set; }
        [DisplayName("SOA1_Parameter V")]
        [PropEditable(true)]
        public string SOA1_Parameter { get; set; }
        [DisplayName("SOA2_Parameter V")]
        [PropEditable(true)]
        public string SOA2_Parameter { get; set; }
        [DisplayName("MIRROR1_Parameter V")]
        [PropEditable(true)]
        public string MIRROR1_Parameter { get; set; }
        [DisplayName("MIRROR2_Parameter V")]
        [PropEditable(true)]
        public string MIRROR2_Parameter { get; set; }
        [DisplayName("PH1_Parameter V")]
        [PropEditable(true)]
        public string PH1_Parameter { get; set; }
        [DisplayName("PH2_Parameter V")]
        [PropEditable(true)]
        public string PH2_Parameter { get; set; }
        [DisplayName("LP_Parameter")]
        [PropEditable(true)]
        public string LP_Parameter { get; set; }
        [DisplayName("MPD1_Parameter V")]
        [PropEditable(true)]
        public string MPD1_Parameter { get; set; }
        [DisplayName("MPD2_Parameter V")]
        [PropEditable(true)]
        public string MPD2_Parameter { get; set; }
        [DisplayName("BIAS1_Parameter V")]
        [PropEditable(true)]
        public string BIAS1_Parameter { get; set; }
        [DisplayName("BIAS2_Parameter V")]
        [PropEditable(true)]
        public string BIAS2_Parameter { get; set; }
    }
}
