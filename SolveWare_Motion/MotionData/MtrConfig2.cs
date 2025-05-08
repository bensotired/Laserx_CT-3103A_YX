using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Manager_Motion.Base
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MtrConfig2
    {
        //复位模式
        [Category("Home Configuration 复位设定")]
        [DisplayName("Home Mode 复位模式 (参考手册)")]
        [Description("Home Mode")]
        public short Home_mode { get; set; }

        //原点  (有效电平，0：低有效，1：高有效)
        [Category("ORG Configuration 原点设定")]
        [DisplayName("ORG Logic 原点讯号")]
        [Description("ORG Logic")]
        public short ORG_Logic { get; set; }


        //零位  (有效电平，0：低有效，1：高有效)
        [Category("EZ Configuration 零位设定")]
        [DisplayName("EZ Logic 零位讯号")]
        [Description("EZ Logic")]
        public short EZ_Logic { get; set; }

        [Category("EZ Configuration 零位设定")]
        [DisplayName("EZ Count")]
        [Description("EZ Count")]
        public short EZ_Count { get; set; }


        //驱动报警复位
        [Category("ERC Configuration 驱动报警复位设定")]
        [DisplayName("ERC Output")]
        [Description("ERC Output")]
        public short ERC_Output { get; set; }


        //报警  (有效电平，0：低有效，1：高有效)
        [Category("ALM Configuration 报警设定")]
        [DisplayName("ALM Enable 报警使能")]
        [Description("ALM Enable")]
        public short Alm_Enable { get; set; }

        [Category("ALM Configuration 报警设定")]
        [DisplayName("ALM Logic 报警讯号")]
        [Description("ALM Logic")]
        public short Alm_Logic { get; set; }

        [Category("ALM Configuration 报警设定")]
        [DisplayName("ALM Mode 报警执行模式")]
        [Description("ALM Mode")]
        public short Alm_Mode { get; set; }


        //到位信号 (有效电平，0：低有效，1：高有效)
        [Category("INP Configuration 到位设定")]
        [DisplayName("INP Enable 到位使能")]
        [Description("INP Enable")]
        public short INP_Enable { get; set; }

        [Category("INP Configuration 到位设定")]
        [DisplayName("INP Logic 到位讯号")]
        [Description("INP Logic")]
        public short INP_Logic { get; set; }


        //停止模式  (0 减速停；1 急停)
        [Category("Stop Configuration 停止设定")]
        [DisplayName("Stop Mode 停止模式")]
        [Description("Stop Mode")]
        public short SD_Mode { get; set; }


        //EL 限位信号
        //Enable (0：正负限位禁止       1：正负限位允许       2：正限位禁止、负限位允许         3：正限位允许、负限位禁止
        //Logic  (0：正负限位低电平有效 1：正负限位高电平有效 2：正限位低有效，负限位高有效     3：正限位高有效，负限位低有效)
        //Mode   (0：正负限位立即停止   1：正负限位减速停止   2：正限位立即停止，负限位减速停止 3：正限位减速停止，负限位立即停止)
        [Category("EL Configuration 限位设定")]
        [DisplayName("EL Enable 限位使能")]
        [Description("EL Enable")]
        public short EL_Enable { get; set; }

        [Category("EL Configuration 限位设定")]
        [DisplayName("EL Logic 限位讯号")]
        [Description("EL Logic")]
        public short EL_Logic { get; set; }

        [Category("EL Configuration 限位设定")]
        [DisplayName("EL Mode 限位模式")]
        [Description("EL Mode")]
        public short EL_Mode { get; set; }


        //脉冲输出模式
        [Category("Pulse Configuration 脉冲输出模式设定")]
        [DisplayName("Pulse Mode 脉冲输出模式")]
        [Description("Pulse Mode")]
        public short Pulse_Mode { get; set; }

        //專用IO映射
        [Description("Limit IO  Configuration")]
        [DisplayName("正限位 設定")]
        [Category("Limit Configuration")]
        public LimitConfig PELConfig { get; set; }

        [Description("Limit IO  Configuration")]
        [DisplayName("負限位 設定")]
        [Category("Limit Configuration")]
        public LimitConfig MELConfig { get; set; }

        [Description("Limit IO  Configuration")]
        [DisplayName("原點 設定")]
        [Category("Limit Configuration")]
        public LimitConfig ORGConfig { get; set; }


        //軟限位啟用
        [Description("Soft Limit Configuration")]
        [DisplayName("軟限位 啟用")]
        [Category("Soft Limit Configuration")]
        public int Enable_SoftLimit { get; set; }

        [Description("Soft Limit Configuration")]
        [DisplayName("最大值")]
        [Category("Soft Limit Configuration")]
        public double Max_SoftLimit { get; set; }

        [Description("Soft Limit Configuration")]
        [DisplayName("最小值")]
        [Category("Soft Limit Configuration")]
        public double Min_SoftLimit { get; set; }
    }
}
