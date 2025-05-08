using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_I_Check : TestRecipeBase
    {
        public TestRecipe_I_Check()
        {
            this.GAIN_Drive_Parameters = "0,1,120,2.5";
            this.SOA1_Drive_Parameters = "0,1,100,2.5";
            this.SOA2_Drive_Parameters = "0,1,100,2.5";
            this.MIRROR1_Drive_Parameters = "0,1,65,2.5";
            this.MIRROR2_Drive_Parameters = "0,1,65,2.5";
            this.LP_Drive_Parameters = "0,0.5,20,2.5";
            this.PH1_Drive_Parameters = "0,0.5,20,2.5";
            this.PH2_Drive_Parameters = "0,0.5,20,2.5";
            this.ApertureTime_s = 0.01f;

        }
        [DisplayName("GAIN 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("GAIN_Drive_Parameters")]
        [PropEditable(true)]
        public string GAIN_Drive_Parameters { get; set; }

        [DisplayName("SOA1 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("SOA1_Drive_Parameters")]
        [PropEditable(true)]
        public string SOA1_Drive_Parameters { get; set; }

        [DisplayName("SOA2 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("SOA2_Drive_Parameters")]
        [PropEditable(true)]
        public string SOA2_Drive_Parameters { get; set; }

        [DisplayName("LP 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("LP_Drive_Parameters")]
        [PropEditable(true)]
        public string LP_Drive_Parameters { get; set; }

        [DisplayName("PH1 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("PH1_Drive_Parameters")]
        [PropEditable(true)]
        public string PH1_Drive_Parameters { get; set; }

        [DisplayName("PH2 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("PH2_Drive_Parameters")]
        [PropEditable(true)]
        public string PH2_Drive_Parameters { get; set; }

        [DisplayName("MIRROR1 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("MIRROR1_Drive_Parameters")]
        [PropEditable(true)]
        public string MIRROR1_Drive_Parameters { get; set; }

        [DisplayName("MIRROR2 扫描电流(mA)[start_mA,step_mA,stop_mA,complianceVoltage_V]")]
        [Description("MIRROR2_Drive_Parameters")]
        [PropEditable(true)]
        public string MIRROR2_Drive_Parameters { get; set; }

        [DisplayName("ApertureTime_s")]
        [Description("ApertureTime_s")]
        [PropEditable(true)]
        public float ApertureTime_s { get; set; }

    }

}