using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_MTuning : TestRecipeBase
    {
        public TestRecipe_MTuning()
        {
            this.Inherit = true;
            this.M1M2_Start_mA = 0f;
            this.M1M2_Stop_mA = 65f;
            this.StepCount = 120;

            this.complianceVoltage_V = 2.5f;

            this.ApertureTime_s = 0.001f;
            this.IsOSA = true;

            OpticalSwitchChannel = 2;

            Algorithm4_CountLimit = 500;
        }
        [DisplayName("从QWLT2获取数值")]
        [Description("Inherit")]
        [PropEditable(true)]
        public bool Inherit { get; set; }

        [DisplayName("M1M2 扫描开始电流(mA)[start]")]
        [Description("M1M2_Start_mA")]
        [PropEditable(true)]
        public float M1M2_Start_mA { get; set; }

        [DisplayName("M1M2 扫描结束电流(mA)[stop]")]
        [Description("M1M2_Stop_mA")]
        [PropEditable(true)]
        public float M1M2_Stop_mA { get; set; }

        [DisplayName("StepCount对数取点数量")]
        [Description("StepCount")]
        [PropEditable(true)]
        public float StepCount { get; set; }

        [DisplayName("限制电压")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public float complianceVoltage_V { get; set; }


        [DisplayName("ApertureTime_s")]
        [Description("ApertureTime_s")]
        [PropEditable(true)]
        public float ApertureTime_s { get; set; }

        [DisplayName("是否使用光谱仪补点")]
        [Description("IsOSA")]
        [PropEditable(true)]
        public bool IsOSA { get; set; }

        [DisplayName("Algorithm4 光谱仪补点数量限制(1个点约耗时2.5秒)")]
        [Description("Algorithm4_CountLimit")]
        [PropEditable(true)]
        public int Algorithm4_CountLimit { get; set; }

        [DisplayName("OSwitch光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int OpticalSwitchChannel { get; set; }
    }
}