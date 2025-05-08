using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    //[Recipe]    
    public class TestRecipe_NF : TestRecipeBase
    {

        public TestRecipe_NF()
        {
            CenterWavelength_nm = 1577;
            WavelengthSpan_nm = 20;
            OsaRbw_nm = 0.07;
            //OsaTraceLength = 1001;
            OsaTraceLength_string = "1001";
            SmsrModeDiff_dB = 2.5;
            SMSRMask_nm = 0.5;
            //strBiasCurrentList_mA = "30";
            BiasBeforeWait_ms = 0;
            BiasAfterWait_ms = 0;
            complianceVoltage_V = 2.5;
            //OsaTrace = "TRA";
            //En_EAOutput = false;
            //EAVoltage_V = 0;
            //EACurrentClampI_mA = -150;
            Default_DrivingCurrent_mA = 40.0;
            SensitivityModes = YokogawaAQ6370SensitivityModes.Mid;
        }

        //[DisplayName("是否使用前置测试结果参数作为驱动电流")]
        //[Description("UseRefData_DrivingCurrent_mA")]
        //[PropEditable(true)]
        //public bool UseRefData_DrivingCurrent_mA { get; set; } = false;

        //[DisplayName("使用前置测试结果参数作为驱动电流,该参数名称")]
        //[Description("RefData_Name_DrivingCurrent_mA")]
        //[PropEditable(true)]
        //public string RefData_Name_DrivingCurrent_mA { get; set; } = "Ith2";
        //[DisplayName("使用前置测试结果参数作为驱动电流时,电流的偏移量(区分正负值)")]
        //[Description("RefData_DrivingCurrent_Offset_mA")]
        //[PropEditable(true)]
        //public double RefData_DrivingCurrent_Offset_mA { get; set; } = 0.0;
        [DisplayName("加电前等待时间(ms)")]
        [Description("BiasBeforeWait_ms")]
        [PropEditable(true)]
        public int BiasBeforeWait_ms { get; set; }



        [DisplayName("加电后等待时间(ms)，默认至少500ms")]
        [Description("BiasAfterWait_ms")]
        [PropEditable(true)]
        public int BiasAfterWait_ms { get; set; }

        [DisplayName("加电过程中的保护电压(V)")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public double complianceVoltage_V { get; set; }


        //[DisplayName("是否启用EA输出")]
        //[Description("En_EAOutput")]
        //[PropEditable(true)]
        //public bool En_EAOutput { get; set; }

        //[DisplayName("EA设置的电压(V)")]
        //[Description("EAVoltage_V")]
        //[PropEditable(true)]
        //public double EAVoltage_V { get; set; }

        //[DisplayName("EA保护电流(mA)")]
        //[Description("EACurrentClampI_mA")]
        //[PropEditable(true)]
        //public double EACurrentClampI_mA { get; set; }

        [DisplayName("使用的默认驱动电流值")]
        [Description("Default_DrivingCurrent_mA")]
        [PropEditable(true)]
        public double Default_DrivingCurrent_mA { get; set; }





        [DisplayName("芯片中心波长")]
        [Description("CenterWavelength_nm")]
        [PropEditable(true)]
        public double CenterWavelength_nm { get; set; }


        [DisplayName("扫描范围")]
        [Description("WavelengthSpan_nm")]
        [PropEditable(true)]
        public double WavelengthSpan_nm { get; set; }


        [DisplayName("分辨率")]
        [Description("OsaRbw_nm")]
        [PropEditable(true)]
        public double OsaRbw_nm { get; set; }



        [DisplayName("采集点的数目(可设置AUTO或点数)")]
        [Description("OsaTraceLength_string")]
        [PropEditable(true)]
        public string OsaTraceLength_string { get; set; }

        [DisplayName("主峰和次峰最小相隔的功率差")]
        [Description("SmsrModeDiff_dB")]
        [PropEditable(true)]
        public double SmsrModeDiff_dB { get; set; }

        [DisplayName("测试灵敏度")]
        [Description("SmsrModeDiff_dB")]
        [PropEditable(true)]
        public YokogawaAQ6370SensitivityModes SensitivityModes { get; set; }


        [DisplayName("主峰和次峰最小相隔的距离")]
        [Description("SMSRMask_nm")]
        [PropEditable(true)]
        public double SMSRMask_nm { get; set; }

        //[DisplayName("光谱仪的曲线类型")]
        //[Description("OsaTrace")]
        //[PropEditable(true)]
        //public string OsaTrace { get; set; }

        [DisplayName("TraceA_前光开关对应光谱的通道")]
        [Description("TraceA_OpticalF_SPECT_ChannelNO")]
        [PropEditable(true)]
        public int TraceA_OpticalF_SPECT_ChannelNO { get; set; } = 1;
        [DisplayName("TraceA_后光开关对应可调激光器的通道")]
        [Description("TraceA_OpticalB_TLX1_ChannelNO")]
        [PropEditable(true)]
        public int TraceA_OpticalB_TLX1_ChannelNO { get; set; } = 1;

        [DisplayName("TraceB_前光开关对应光谱的通道")]
        [Description("TraceB_OpticalF_SPECT_ChannelNO")]
        [PropEditable(true)]
        public int TraceB_OpticalF_SPECT_ChannelNO { get; set; } = 1;
        [DisplayName("TraceB_后光开关对应可调激光器的通道")]
        [Description("TraceB_OpticalB_TLX1_ChannelNO")]
        [PropEditable(true)]
        public int TraceB_OpticalB_TLX1_ChannelNO { get; set; } = 1;

        //计算参数
        [DisplayName("选择ASE功率测量的算法：0=AUTO_FIX；1=MANUAL_FIX；2=AUTO_CENTER；3=MANUAL_CENTER")]
        [Description("AALGo")]
        [PropEditable(true)]
        public int AALGo { get; set; } = 0;
        [DisplayName("选择求取ASE功率的插补算法：0=LINEAR；1=GAUSS；2=LORENZ；3=3RD_POLY；4=4YH_POLY；5=5TH_POLY")]
        [Description("FALGo")]
        [PropEditable(true)]
        public int FALGo { get; set; } = 0;
        [DisplayName("运用插补算法计算ASE功率时，用此参数设置波形数据范围，只有在ASE ALGO设为MANUAL-FIX时才可设置：0.01_10.00nm")]
        [Description("FARea")]
        [PropEditable(true)]
        public double FARea { get; set; } = 1;
        [DisplayName("对信号光波形(曲线A)设置功率偏置，不需要功率偏置时设置成0.00，设置范围:-99.9_99.9_dB")]
        [Description("IOFFset")]
        [PropEditable(true)]
        public double IOFFset { get; set; } = 0;
        [DisplayName("设置积分范围，用于求取信号光功率, 当SIGNAL_POWER设为INTEGRAL时有效,设置范围:1.0_999.9_GHz")]
        [Description("IRANge")]
        [PropEditable(true)]
        public double IRANge { get; set; } = 10;
        [DisplayName("运用插补算法计算ASE功率时，用此参数设置信号光掩盖范围：0.01_10.00nm")]
        [Description("MARea")]
        [PropEditable(true)]
        public double MARea { get; set; } = 0.4;
        [DisplayName("设置通道峰值检测的最小峰谷差：0.0_50.0_dB")]
        [Description("MDIFF")]
        [PropEditable(true)]
        public double MDIFF { get; set; } = 3.0;
        [DisplayName("对输出光波形(曲线B)设置功率偏置，不需要功率偏置时设置成0.00,设置范围：-99.9_99.9_dB")]
        [Description("OOFFset")]
        [PropEditable(true)]
        public double OOFFset { get; set; } = 0;
        [DisplayName("显示插补的数据范围,用来求取噪声功率:0=off；1=on")]
        [Description("PDISplay")]
        [PropEditable(true)]
        public int PDISplay { get; set; } = 1;
        [DisplayName("设置通道检测的阈值：0.1_99.9_dB")]
        [Description("TH")]
        [PropEditable(true)]
        public double TH { get; set; } = 20;
        [DisplayName("设置测量分辨率RBi的计算方法，用于计算各通道的NF值: 0=Measured_:从TRACE_B_的波形计算各通道的THRESH_3dB的带宽，并设为RBi：1=CAL_DATA_将仪器内存储的实际分辨率设为RBi")]
        [Description("RBWidth")]
        [PropEditable(true)]
        public int RBWidth { get; set; } = 1;
        [DisplayName("设置NF值计算是否包含Shot_Noise成分：0=off；1=on")]
        [Description("SNOISE")]
        [PropEditable(true)]
        public int SNOISE { get; set; } = 1;
        [DisplayName("设置信号光功率的计算方法: 0=Peak_峰值功率；1=Integral_每次累积计算的功率值")]
        [Description("SPOWer")]
        [PropEditable(true)]
        public int SPOWer { get; set; } = 0;


        //[DisplayName("Use_dBmormW(True=dBm,False=mW)")]
        //[PropEditable(true)]
        //public bool Use_dBm0rmW { get; set; }

        //[DisplayName("设置可调激光器VOAAttenModePower(_dBm)")]
        //[Description("VOAAttenModePower_dBm")]
        //[PropEditable(true)]
        //public float VOAAttenModePower_dBm { get; set; } = 0.1f;

        //[DisplayName("设置可调激光器VOAAttenModePower(_mW)")]
        //[Description("VOAAttenModePower_mW")]
        //[PropEditable(true)]
        //public float VOAAttenModePower_mW { get; set; } = 0.1f;


    }
}