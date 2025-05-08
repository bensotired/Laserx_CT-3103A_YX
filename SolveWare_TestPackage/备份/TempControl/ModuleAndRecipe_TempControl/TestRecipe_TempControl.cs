using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_TempControl_1089 : TestRecipeBase
    {
        [DisplayName("开关TEC")]
        [Description("EnableTEC")]
        [PropEditable(true)]
        public bool EnableTEC { get; set; }

        [DisplayName("控制温度(℃)")]
        [Description("Temperature")]
        [PropEditable(true)]
        public double Temperature { get; set; }

        [DisplayName("等待温控稳定超时(Sec)")]
        [Description("TempControl_Timeout")]
        [PropEditable(true)]
        public int TempControl_Timeout { get; set; }

        [DisplayName("温控稳定范围(±℃)")]
        [Description("TempControl_Tolerance")]
        [PropEditable(true)]
        public double TempControl_Tolerance { get; set; }

        [DisplayName("温控稳定保持时间(Sec)")]
        [Description("TempControl_StabilizingTime_sec")]
        [PropEditable(true)]
        public double TempControl_StabilizingTime_sec { get; set; }

        public TestRecipe_TempControl_1089()
        {
            TempControl_Timeout = 5*60;    //分钟
            TempControl_Tolerance = 0.3;    //摄氏度
            TempControl_StabilizingTime_sec = 10;   //秒
        }

    }
}