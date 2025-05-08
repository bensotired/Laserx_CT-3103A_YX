using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_LIV_Tap_PD : TestRecipeBase
    {
        public TestRecipe_LIV_Tap_PD()
        {
            this.Inherit = true;
            //this.Gain_DrivingCurrent_Of_PhaseScan_mA = 120.0f;

            //this.Phase_Start_mA = 0.0F;
            //this.Phase_Stop_mA = 20F;
            //this.Phase_Step_mA = 0.1F;
            //this.PhaseComplianceVoltage_V = 2.5f;

            this.ApertureTime_s = 0.04f;

            this.Gain_Start_mA = 0.0F;
            this.Gain_Stop_mA = 120F;
            this.Gain_Step_mA = 1F;

            this.GainComplianceVoltage_V = 2.5F;

            this.PDBiasVoltage_V = 0f;
            this.PDComplianceCurrent_mA = 1f;
            this.PD_K = 12.8565;
            this.PD_B = 0;

            LIVOpticalSwitchChannel = 1;


            CenterWavelength_nm = 1550;
            WavelengthSpan_nm = 50;
            Resolution_nm = 0.1;

            OsaTraceLength_string = "1001";

            SPOpticalSwitchChannel = 2;
        }

        [DisplayName("从QWLT2获取数值")]
        [Description("Inherit")]
        [PropEditable(true)]
        public bool Inherit { get; set; }

        //[DisplayName("PhaseScan的 GAIN 驱动电流(mA)")]
        //[Description("Gain_DrivingCurrent_Of_PhaseScan_mA")]
        //[PropEditable(true)]
        //public float Gain_DrivingCurrent_Of_PhaseScan_mA { get; set; }
        //[DisplayName("PH1-PH2 扫描起始电流(mA)")]
        //[Description("Phase_Start_mA")]
        //[PropEditable(true)]
        //public float Phase_Start_mA { get; set; }
 
        //[DisplayName("PH1-PH2 扫描结束电流(mA)")]
        //[Description("Phase_Stop_mA")]
        //[PropEditable(true)]
        //public float Phase_Stop_mA { get; set; }
 
        //[DisplayName("PH1-PH2 扫描步进电流(mA)")]
        //[Description("Phase_Step_mA")]
        //[PropEditable(true)]
        //public float Phase_Step_mA { get; set; }

        //[DisplayName("PH1-PH2 限制电压")]
        //[Description("PhaseComplianceVoltage_V")]
        //[PropEditable(true)]
        //public float PhaseComplianceVoltage_V { get; set; }

        [DisplayName("GAIN 扫描起始电流(mA)")]
        [Description("I_Start_A")]
        [PropEditable(true)]
        public float Gain_Start_mA { get; set; }

        [DisplayName("GAIN 扫描结束电流(mA)")]
        [Description("I_Stop_A")]
        [PropEditable(true)]
        public float Gain_Stop_mA { get; set; }

        [DisplayName("GAIN 扫描步进电流(mA)")]
        [Description("I_Step_A")]
        [PropEditable(true)]
        public float Gain_Step_mA { get; set; }
        [DisplayName("限制电压")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public float GainComplianceVoltage_V { get; set; }

        [DisplayName("PD偏置电压")]
        [Description("PDBiasVoltage_V")]
        [PropEditable(true)]
        public float PDBiasVoltage_V { get; set; }

        [DisplayName("PD限制电流")]
        [Description("PDComplianceCurrent_mA")]
        [PropEditable(true)]
        public float PDComplianceCurrent_mA { get; set; }

        [DisplayName("ApertureTime_s")]
        [Description("ApertureTime_s")]
        [PropEditable(true)]
        public float ApertureTime_s { get; set; }
        [DisplayName("PD_K")]
        [Description("PD_K")]
        [PropEditable(true)]
        public double PD_K { get; set; }
        [DisplayName("PD_B")]
        [Description("PD_B")]
        [PropEditable(true)]
        public double PD_B { get; set; }

        [DisplayName("OSwitch LIV光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int LIVOpticalSwitchChannel { get; set; }

        //光谱
        [DisplayName("中心波长")]
        [Description("CenterWavelength_nm")]
        [PropEditable(true)]
        public double CenterWavelength_nm { get; set; }

        [DisplayName("扫描范围")]
        [Description("WavelengthSpan_nm")]
        [PropEditable(true)]
        public double WavelengthSpan_nm { get; set; }

        [DisplayName("分辨率")]
        [Description("Resolution_nm")]
        [PropEditable(true)]
        public double Resolution_nm { get; set; }

        [DisplayName("采集点的数目(可设置AUTO或点数)")]
        [Description("OsaTraceLength_string")]
        [PropEditable(true)]
        public string OsaTraceLength_string { get; set; }


        [DisplayName("OSwitch SP光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int SPOpticalSwitchChannel { get; set; }
    }
}