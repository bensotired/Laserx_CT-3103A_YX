using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_AlternativeQWLT : TestRecipeBase
    {
        public TestRecipe_AlternativeQWLT()
        {
            this.Inherit = true;
            this.Analog_CH = 1;
            this.InitialCurrentSense_mA = 0.1;

            //===========================
            EnableAlignment = false;

            this.PowerThreshold_mA = 0.01;

            this.Fine_Radius = 0.01;
            this.Fine_Involute_Interval = 0.002;
            this.Fine_Trajspeed = 0.05;

            this.Creep_Step_um = 0.2; //蠕动步进
            this.CreepDelay_ms = 400;   //蠕动后等待多久稳定
            this.OutOfFocusDistance_um = 2;

            //===========================

            ITU_Channel = 48;
            SerachMode = eSerachMode.带宽范围;
            Bandwidth_range_Ghz = 25.0;
            Wavelength_range_nm = "1556.6 - 1556.8";

            SerachLimit = 30;

            this.P1_P2_mA = "0,0.5,10";
            this.PCVoltage_V = 2.5F;

            this.Mirror_retry_step_mA = 3;
            this.Mirror_Offset_mA = -5F;
            this.Mirror_ScanningStep_mA = 0.1F;

            this.LaserPhase_mA = "0,0.5,20";

            PDComplianceCurrent_mA = 1.0f;
            PD_K = 1.0;
            PD_B = 0.0;

            LIVOpticalSwitchChannel = 1;
            SPOpticalSwitchChannel = 2;
            
        }
        [DisplayName("从QWLT2获取数值")]
        [Description("Inherit")]
        [PropEditable(true)]
        public bool Inherit { get; set; }

        [DisplayName("模拟量通道")]
        [Description("Analog_CH")]
        [PropEditable(true)]
        public int Analog_CH { get; set; }


        //==========================
        
        [DisplayName("初始PD电流量程(mA)")]
        [Description("InitialCurrentSense_mA")]
        [PropEditable(true)]
        public double InitialCurrentSense_mA { get; set; }

        [DisplayName("耦合PD电流下限(mA)")]
        [Description("PowerThreshold_mA")]
        [PropEditable(true)]
        public double PowerThreshold_mA { get; set; }  

        //==========================

        [DisplayName("启用耦合")]
        [Description("EnableAlignment")]
        [PropEditable(true)]
        public bool EnableAlignment { get; set; }  
        
        [DisplayName("精扫插补速度(mm/s)")]
        [Description("Fine_Trajspeed")]
        [PropEditable(true)]
        public double Fine_Trajspeed { get; set; }

        [DisplayName("精扫半径(mm)")]
        [Description("Fine_Radius")]
        [PropEditable(true)]
        public double Fine_Radius { get; set; }

        [DisplayName("精扫线间隔(mm)")]
        [Description("Fine_Involute_Interval")]
        [PropEditable(true)]
        public double Fine_Involute_Interval { get; set; }

        //==========================

        [DisplayName("蠕动耦合步长(um)")]
        [Description("Creep_Step_um")]
        [PropEditable(true)]
        public double Creep_Step_um { get; set; }

        [DisplayName("蠕动后稳定等待")]
        [Description("CreepDelay_ms")]
        [PropEditable(true)]
        public double CreepDelay_ms { get; set; }

        [DisplayName("蠕动离焦长度")]
        [Description("OutOfFocusDistance_um")]
        [PropEditable(true)]
        public double OutOfFocusDistance_um { get; set; }

        //==========================

        [DisplayName("目标ITU通道")]
        [Description("ITU Channel")]
        [PropEditable(true)]
        public int ITU_Channel { get; set; }

        [DisplayName("目标波长搜索方式")]
        [Description("SerachMode")]
        [PropEditable(true)]
        public eSerachMode SerachMode { get; set; }

        [DisplayName("目标 带宽 范围(单边)")]
        [Description("ITU Channel Bandwidth_range_Ghz")]
        [PropEditable(true)]
        public double Bandwidth_range_Ghz { get; set; }

        [DisplayName("目标 波长 范围 输入 Ex: [1556.6 - 1556.8]")]
        [Description("ITU Channel Wavelength_range_nm")]
        [PropEditable(true)]
        public string Wavelength_range_nm { get; set; }


        public double Wavelength_range_nm_min
        {
            get
            {
                string[] wavlength = Wavelength_range_nm.Split('-');
                double t = 1556.6;

                if (wavlength.Length >1)
                {
                    double.TryParse(wavlength[0], out t);
                }
      
                return t;
            }
        }
        public double Wavelength_range_nm_max
        {
            get
            {
                string[] wavlength = Wavelength_range_nm.Split('-');
                double t = 1556.8;

                if (wavlength.Length ==1)
                {
                    double.TryParse(wavlength[0], out t);
                }
                if (wavlength.Length >=2)
                {
                    double.TryParse(wavlength[1], out t);
                }      
                return t;
            }
        }


        [DisplayName("目标波长搜索的最大次数")]
        [Description("SerachLimit")]
        [PropEditable(true)]
        public int SerachLimit { get; set; }

        [DisplayName("M1M2斜线扫描电流范围")]
        [Description("Offset")]
        [PropEditable(true)]
        public float Mirror_Offset_mA { get; set; }

        [DisplayName("M1M2斜线扫描电流步进")]
        [Description("Scanning step")]
        [PropEditable(true)]
        public float Mirror_ScanningStep_mA { get; set; }

        [DisplayName("M1M2电流的调整步进")]
        [Description("Mirror Retry Step(mA)")]
        [PropEditable(true)]
        public float Mirror_retry_step_mA { get; set; }

        [DisplayName("LP 扫描电流(mA)[start,step,stop]")]
        [Description("LP_mA")]
        [PropEditable(true)]
        public string LaserPhase_mA { get; set; }

        [DisplayName("PH1-PH2 扫描电流(mA)[start,step,stop]")]
        [Description("P1_P2_mA")]
        [PropEditable(true)]
        public string P1_P2_mA { get; set; }
        [DisplayName("PH1-PH2 限制电压")]
        [Description("PCVoltage_V")]
        [PropEditable(true)]
        public float PCVoltage_V { get; set; }


        [DisplayName("PD限制电流")]
        [Description("PDComplianceCurrent_mA")]
        [PropEditable(true)]
        public float PDComplianceCurrent_mA { get; set; }

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

        [DisplayName("OSwitch SP光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int SPOpticalSwitchChannel { get; set; }

    }

    public enum eSerachMode
    {
        带宽范围,
        波长范围,
    }
}