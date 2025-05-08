using SolveWare_BurnInCommon;
using System;
using System.ComponentModel;

namespace SolveWare_Motion
{
    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class MotorModes
    {
        public MotorModes()
        {
            this.ORG_JumpEdge  = -1;
        }
        //脉冲输出模式
        [Category("Pulse Configuration 脉冲输出模式设定")]
        [DisplayName("脉冲输出模式")]
        [Description("Pulse Mode")]
        [PropEditable(true)]
        public short Pulse_Mode { get; set; }
        //复位模式
        [Category("Home Configuration 复位设定")]
        [DisplayName("复位模式")]
        [Description("Home Mode")]
        [PropEditable(true)]
        public short Home_Mode { get; set; }

        [Category("ORG [-1]Home到ON->OFF状态 [1]Home到OFF->ON状态 ")]
        [DisplayName("ORG 原点跳变沿")]
        [Description("ORG JumpEdge")]
        [PropEditable(true)]
        public short ORG_JumpEdge { get; set; }

        [Category("Home Configuration 复位后运动")]
        [DisplayName("复位后运动到某位置")]
        [Description("Home NewPos")]
        [PropEditable(true)]
        public double Home_NewPos { get; set; }

        ////原点  (有效电平，0：低有效，1：高有效)
        //[Category("ORG Configuration 原点设定")]
        //[DisplayName("ORG Logic 原点讯号")]
        //[Description("ORG Logic")]
        //[PropEditable(true)]
        //public short ORG_Logic { get; set; }


        ////零位  (有效电平，0：低有效，1：高有效)
        //[Category("EZ Configuration 零位设定")]
        //[DisplayName("EZ Logic 零位讯号")]
        //[Description("EZ Logic")]
        //[PropEditable(true)]
        //public short EZ_Logic { get; set; }

        //[Category("EZ Configuration 零位设定")]
        //[DisplayName("EZ Count")]
        //[Description("EZ Count")]
        //[PropEditable(true)]
        //public short EZ_Count { get; set; }


        ////驱动报警复位
        //[Category("ERC Configuration 驱动报警复位设定")]
        //[DisplayName("ERC Output")]
        //[Description("ERC Output")]
        //[PropEditable(true)]
        //public short ERC_Output { get; set; }


        ////报警  (有效电平，0：低有效，1：高有效)
        //[Category("ALM Configuration 报警设定")]
        //[DisplayName("ALM Enable 报警使能")]
        //[Description("ALM Enable")]
        //[PropEditable(true)]
        //public short Alm_Enable { get; set; }

        //[Category("ALM Configuration 报警设定")]
        //[DisplayName("ALM Logic 报警讯号")]
        //[Description("ALM Logic")]
        //[PropEditable(true)]
        //public short Alm_Logic { get; set; }

        //[Category("ALM Configuration 报警设定")]
        //[DisplayName("ALM Mode 报警执行模式")]
        //[Description("ALM Mode")]
        //[PropEditable(true)]
        //public short Alm_Mode { get; set; }


        ////到位信号 (有效电平，0：低有效，1：高有效)
        //[Category("INP Configuration 到位设定")]
        //[DisplayName("到位使能(0：低有效，1：高有效)")]
        //[Description("INP Enable")]
        //[PropEditable(true)]
        //public short INP_Enable { get; set; }

        //[Category("INP Configuration 到位设定")]
        //[DisplayName("到位信号逻辑")]
        //[Description("INP Logic")]
        //[PropEditable(true)]
        //public short INP_Logic { get; set; }


        //停止模式  (0 减速停；1 急停)
        [Category("Stop Configuration 停止设定")]
        [DisplayName("停止模式(0 减速停；1 急停)")]
        [Description("Stop Mode")]
        [PropEditable(true)]
        public short Stop_Mode { get; set; }


        //EL 限位信号
        //Enable (0：正负限位禁止       1：正负限位允许       2：正限位禁止、负限位允许         3：正限位允许、负限位禁止
        //Logic  (0：正负限位低电平有效 1：正负限位高电平有效 2：正限位低有效，负限位高有效     3：正限位高有效，负限位低有效)
        //Mode   (0：正负限位立即停止   1：正负限位减速停止   2：正限位立即停止，负限位减速停止 3：正限位减速停止，负限位立即停止)

        //[Category("EL Configuration 限位设定")]
        //[DisplayName("EL Enable 限位使能")]
        //[Description("EL Enable")]
        //[PropEditable(true)]
        //public short EL_Enable { get; set; }

        //[Category("EL Configuration 限位设定")]
        //[DisplayName("EL Logic 限位讯号")]
        //[Description("EL Logic")]
        //[PropEditable(true)]
        //public short EL_Logic { get; set; }

        //[Category("EL Configuration 限位设定")]
        //[DisplayName("EL Mode 限位模式")]
        //[Description("EL Mode")]
        //[PropEditable(true)]
        //public short EL_Mode { get; set; }



        ////專用IO映射
        //[Description("Limit IO  Configuration")]
        //[DisplayName("正限位 設定")]
        //[Category("Limit Configuration")]
        //[PropEditable(true)]
        //public LimitConfig PELConfig { get; set; }

        //[Description("Limit IO  Configuration")]
        //[DisplayName("負限位 設定")]
        //[Category("Limit Configuration")]
        //[PropEditable(true)]
        //public LimitConfig MELConfig { get; set; }

        //[Description("Limit IO  Configuration")]
        //[DisplayName("原点 設定")]
        //[Category("Limit Configuration")]
        //[PropEditable(true)] 
        //public LimitConfig ORGConfig { get; set; }


        //軟限位啟用
        [Description("Soft Limit Configuration")]
        [DisplayName("软限位 使能")]
        [Category("Soft Limit Configuration")]
        [PropEditable(true)]
        public int Enable_SoftLimit { get; set; }

        [Description("Soft Limit Configuration")]
        [DisplayName("软限位 最大值")]
        [Category("Soft Limit Configuration")]
        [PropEditable(true)]
        public double Max_SoftLimit { get; set; }

        [Description("Soft Limit Configuration")]
        [DisplayName("软限位 最小值")]
        [Category("Soft Limit Configuration")]
        [PropEditable(true)]
        public double Min_SoftLimit { get; set; }
    }
}
