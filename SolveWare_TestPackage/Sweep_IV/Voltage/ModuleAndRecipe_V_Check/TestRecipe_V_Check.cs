using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_V_Check : TestRecipeBase
    {
        public TestRecipe_V_Check()
        {
            this.GAIN_Drive_Parameters = "-2,0.1,1,20";
            this.SOA1_Drive_Parameters = "-2,0.1,1,20";
            this.SOA2_Drive_Parameters = "-2,0.1,1,20";
            this.MIRROR1_Drive_Parameters = "-2,0.1,1,20"; 
            this.MIRROR2_Drive_Parameters = "-2,0.1,1,20";
            this.PH1_Drive_Parameters = "-2,0.1,1,20";
            this.PH2_Drive_Parameters = "-2,0.1,1,20";
            this.LP_Drive_Parameters = "-2,0.1,1,20";

            this.MPD1_Drive_Parameters = "-5,0.1,1.5,20";
            this.MPD2_Drive_Parameters = "-5,0.1,1.5,20";
            this.BIAS1_Drive_Parameters = "-5,0.1,1.5,20";
            this.BIAS2_Drive_Parameters = "-5,0.1,1.5,20";

            this.CurrentAutoRange = true;

            //this.IsFourWireOn = true;
        }
        [DisplayName("GAIN 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("GAIN_Drive_Parameters")]
        [PropEditable(true)]
        public string GAIN_Drive_Parameters { get; set; }

        [DisplayName("SOA1 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("SOA1_Drive_Parameters")]
        [PropEditable(true)]
        public string SOA1_Drive_Parameters { get; set; }

        [DisplayName("SOA2 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("SOA2_Drive_Parameters")]
        [PropEditable(true)]
        public string SOA2_Drive_Parameters { get; set; }

        [DisplayName("MIRROR1 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("MIRROR1_Drive_Parameters")]
        [PropEditable(true)]
        public string MIRROR1_Drive_Parameters { get; set; }

        [DisplayName("MIRROR2 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("MIRROR2_Drive_Parameters")]
        [PropEditable(true)]
        public string MIRROR2_Drive_Parameters { get; set; }

        [DisplayName("PH1 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("PH1_Drive_Parameters")]
        [PropEditable(true)]
        public string PH1_Drive_Parameters { get; set; }

        [DisplayName("PH2 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("PH2_Drive_Parameters")]
        [PropEditable(true)]
        public string PH2_Drive_Parameters { get; set; }

        [DisplayName("LP 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("LP_Drive_Parameters")]
        [PropEditable(true)]
        public string LP_Drive_Parameters { get; set; }

        [DisplayName("MPD1 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("MPD1_Drive_Parameters")]
        [PropEditable(true)]
        public string MPD1_Drive_Parameters { get; set; }

        [DisplayName("MPD2 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("MPD2_Drive_Parameters")]
        [PropEditable(true)]
        public string MPD2_Drive_Parameters { get; set; }

        [DisplayName("BIAS1/MZM1 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("BIAS1/MZM1_Drive_Parameters")]
        [PropEditable(true)]
        public string BIAS1_Drive_Parameters { get; set; }

        [DisplayName("BIAS2/MZM2 扫描电压(mA)[start_V,step_V,stop_V,complianceCurrent_mA]")]
        [Description("BIAS2/MZM2_Drive_Parameters")]
        [PropEditable(true)]
        public string BIAS2_Drive_Parameters { get; set; }


        [DisplayName("CurrentAutoRange")]
        [Description("CurrentAutoRange")]
        [PropEditable(true)]
        public bool CurrentAutoRange { get; set; }


    }
}