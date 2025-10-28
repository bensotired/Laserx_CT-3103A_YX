using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_QWLT2 : TestRecipeBase
    {
        public TestRecipe_QWLT2()
        {




            DefaultSettingValue_GAIN_mA = 120;
            DefaultSettingValue_LP_mA = 4;
            DefaultSettingValue_MIRROR1_mA = 10;
            DefaultSettingValue_MIRROR2_mA = 10;
            DefaultSettingValue_PH1_mA = 1;
            DefaultSettingValue_PH2_mA = 0;
            DefaultSettingValue_SOA1_mA = 50;
            DefaultSettingValue_SOA2_mA = 40;


            this.PH_MPD = PH_MPD.MPD2;
            this.P1_P2_mA = "0,0.5,10";
            this.PCVoltage_V = 2.5F;
            this.CentralCurrent = "10,10";
            this.Offset = -5F;
            this.ScanningStep = 0.1F;

            this.LP_mA = "0,0.5,10";

            this.Bais1_V = -2;
            this.Bais2_V = -2;
            this.mPd1_V = -2.5;
            this.mPd2_V = -2.5;

            OpticalSwitchChannel = 2;
        }

        [DisplayName("扫描Section")]
        [Description("Section")]
        [PropEditable(true)]
        public PH_MPD PH_MPD { get; set; }

        [DisplayName("PH1-PH2 扫描电流(mA)[start,step,stop]")]
        [Description("P1_P2_mA")]
        [PropEditable(true)]
        public string P1_P2_mA { get; set; }
        [DisplayName("PH1-PH2 限制电压")]
        [Description("PCVoltage_V")]
        [PropEditable(true)]
        public float PCVoltage_V { get; set; }

        [DisplayName("MIRROR 中心电流点(mA)")]
        [Description("Central current mA")]
        [PropEditable(true)]
        public string CentralCurrent { get; set; }

        [DisplayName("MIRROR Offset")]
        [Description("Offset")]
        [PropEditable(true)]
        public float Offset { get; set; }

        [DisplayName("MIRROR Scanning step")]
        [Description("Scanning step")]
        [PropEditable(true)]
        public float ScanningStep { get; set; }

        [DisplayName("LP 扫描电流(mA)[start,step,stop]")]
        [Description("LP_mA")]
        [PropEditable(true)]
        public string LP_mA { get; set; }

        [DisplayName("DefaultSettingValue_GAIN_mA")]
        [Description("DefaultSettingValue_GAIN_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_GAIN_mA { get; set; }

        [DisplayName("DefaultSettingValue_LP_mA")]
        [Description("DefaultSettingValue_LP_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_LP_mA { get; set; }

        [DisplayName("DefaultSettingValue_MIRROR1_mA")]
        [Description("DefaultSettingValue_MIRROR1_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_MIRROR1_mA { get; set; }

        [DisplayName("DefaultSettingValue_MIRROR2_mA")]
        [Description("DefaultSettingValue_MIRROR2_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_MIRROR2_mA { get; set; }

        [DisplayName("DefaultSettingValue_PH1_mA")]
        [Description("DefaultSettingValue_PH1_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_PH1_mA { get; set; }

        [DisplayName("DefaultSettingValue_PH2_mA")]
        [Description("DefaultSettingValue_PH2_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_PH2_mA { get; set; }

        [DisplayName("DefaultSettingValue_SOA1_mA")]
        [Description("DefaultSettingValue_SOA1_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_SOA1_mA { get; set; }

        [DisplayName("DefaultSettingValue_SOA2_mA")]
        [Description("DefaultSettingValue_SOA2_mA")]
        [PropEditable(true)]
        public double DefaultSettingValue_SOA2_mA { get; set; }


        [DisplayName("Bais1_V")]
        [Description("Bais1_V")]
        [PropEditable(true)]
        public double Bais1_V { get; set; }
        [DisplayName("Bais2_V")]
        [Description("Bais2_V")]
        [PropEditable(true)]
        public double Bais2_V { get; set; }
        [DisplayName("mPd1_V")]
        [Description("mPd1_V")]
        [PropEditable(true)]
        public double mPd1_V { get; set; }
        [DisplayName("mPd2_V")]
        [Description("mPd2_V")]
        [PropEditable(true)]
        public double mPd2_V { get; set; }

        [DisplayName("OSwitch光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int OpticalSwitchChannel { get; set; }
    }

}