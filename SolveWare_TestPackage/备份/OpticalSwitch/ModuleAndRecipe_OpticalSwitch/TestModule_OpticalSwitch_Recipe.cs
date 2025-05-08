using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_OpticalSwitch : TestRecipeBase
    {
        public TestRecipe_OpticalSwitch()
        {
            this.OpticalSwitchEnable = true;
            this.ChangeNo = 1;
            this.IOSwitchEnable = false;
            this.IOSwitchStatus = false;
        }
        [DisplayName("是否使用1*4光路控制器")]
        [Description("OpticalSwitchEnable")]
        [PropEditable(true)]
        public bool OpticalSwitchEnable { get; set; } 
        [DisplayName("需要切换的通道")]
        [Description("ChangeNo")]
        [PropEditable(true)]
        public int  ChangeNo { get; set; }

        [DisplayName("是否使用IO控制右侧六轴光路")]
        [Description("OpticalSwitchEnable")]
        [PropEditable(true)]
        public bool IOSwitchEnable { get; set; }
        
        [DisplayName("需要切换IO的状态")]
        [Description("ChangeNo")]
        [PropEditable(true)]
        public bool IOSwitchStatus{ get; set; }
    }
}