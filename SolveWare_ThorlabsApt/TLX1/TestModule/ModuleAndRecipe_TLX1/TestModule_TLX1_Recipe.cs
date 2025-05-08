using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_TLX1 : TestRecipeBase 
    {
        public TestRecipe_TLX1() 
        {
            this.SetLaserPower_SW = true;
            this.SetDither_SW = true;
            this.SetGetITUChannel = 1;
            this.SetGetFrequency = 1;
            // public string SetGetBand

            this.SetVOAPower_SW = true;
            this.SetVOAMode_SW = true;
            this.Use_dBm0rmW = true;
            this.SetGetSystemWavelength = 1310;
            this.SetOpticalAttenuationValue = 1;
            this.SetOpticalOutputPowerValue_dBm = 1;
            this.SetOpticalOutputPower_mW = 1;
        }
    

        [DisplayName("是否开启可调激光器出光")]
        [PropEditable(true)]
        public bool SetLaserPower_SW { get; set; }
        [DisplayName("是否开启噪声处理")]
        [PropEditable(true)]
        public bool SetDither_SW { get; set; }
        [DisplayName("设置ITU通道数")]
        [PropEditable(true)]
        public int SetGetITUChannel { get; set; }
        [DisplayName("设置Frequency")]
        [PropEditable(true)]
        public int SetGetFrequency { get; set; }

        [DisplayName("设置系统波长")]
        [PropEditable(true)]
        public int SetGetSystemWavelength { get; set; }
        //[DisplayName("SetGetBand")]
        //[PropEditable(true)]
        //public string SetGetBand { get; set; }



        [DisplayName("是否开启VOA控制")]
        [PropEditable(true)]
        public bool SetVOAPower_SW { get; set; }
        [DisplayName("设置VOA模式(true =out,false =Atten)")]
        [PropEditable(true)]
        public bool SetVOAMode_SW { get; set; }

       
        [DisplayName("设置衰减模式下衰减值(VOA模式=false)")]
        [PropEditable(true)]
        public float SetOpticalAttenuationValue { get; set; }

        [DisplayName("选择输出模式单位(True=dBm,False=mW)&(VOA模式=true)")]
        [PropEditable(true)]
        public bool  Use_dBm0rmW { get; set; }

        [DisplayName("设置输出功率值_dBm")]
        [PropEditable(true)]
        public float SetOpticalOutputPowerValue_dBm { get; set; }
        [DisplayName("设置输出功率值_mW")]
        [PropEditable(true)]
        public float SetOpticalOutputPower_mW { get; set; }

    }
}