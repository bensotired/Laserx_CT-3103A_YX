using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    public class TestRecipe_LIV : TestRecipeBase
    {
        public TestRecipe_LIV()
        {
            DebugMode = true;


            FiberAlignment = false;
            PDCurrentRange = PDCurrentRange.档位3_2mA;

            this.I_Start_mA = 0.0;
            this.I_Stop_mA = 100.0;
            this.I_Step_mA = 1.0;
            this.ComplianceVoltage_V = 2.5;
            this.Period_ms = 5;
            this.SenseDelay_ms = 0.1;
            this.MPDSourceVoltage_V = 0;
            this.MPDSenseCurrentRange_mA = 0.1;

            this.PulsedMode = false;
            this.DutyRatio = double.NaN;

            PDFactor_K_1st = 1.0;
            PDFactor_B_1st = 0;
            PDFactor_K_2ed = 1.0;
            PDFactor_B_2ed = 0;

            //MPDFactor_K = 1.0;
            //MPDFactor_B = 0;
        }

        [DisplayName("调试模式")]
        [Description("DebugMode")]
        [PropEditable(true)]
        public bool DebugMode { get; set; }


        [DisplayName("true：由FiberAlignment模块确定精确测试位置并运动至该位置" +
            "false：测试模块自动运动至预设测试位置")]
        [Description("FiberAlignment")]
        [PropEditable(true)]
        public bool FiberAlignment { get; set; }


        //转接板
        [DisplayName("TIA板PD电流采集量程")]
        [Description("PDCurrentRange")]
        [PropEditable(true)]
        public PDCurrentRange PDCurrentRange { get; set; }


        [DisplayName("扫描起始电流(mA)")]
        [Description("I_Start_mA")]
        [PropEditable(true)]
        public double I_Start_mA { get; set; }

        [DisplayName("扫描结束电流(mA)")]
        [Description("I_Stop_mA")]
        [PropEditable(true)]
        public double I_Stop_mA { get; set; }

        [DisplayName("扫描步进电流(mA)")]
        [Description("I_Step_mA")]
        [PropEditable(true)]
        public double I_Step_mA { get; set; }



        [DisplayName("MPD通道输出电压(V)")]
        [Description("MPDSourceVoltage_V")]
        [PropEditable(true)]
        public double MPDSourceVoltage_V { get; set; }



        [DisplayName("MPD通道采集电流量程(mA)")]
        [Description("MPDSenseCurrentRange_mA")]
        [PropEditable(true)]
        public double MPDSenseCurrentRange_mA { get; set; }



        [DisplayName("扫描时间间隔 毫秒")]
        [Description("Period_ms")]
        [PropEditable(true)]
        public double Period_ms { get; set; }

        [DisplayName("扫描占空比")]
        [Description("DutyRatio")]
        [PropEditable(true)]
        public double DutyRatio { get; set; }


        [DisplayName("扫描延迟时间 毫秒")]
        [Description("SenseDelay_ms")]
        [PropEditable(true)]
        public double SenseDelay_ms { get; set; }


        [DisplayName("源表LD限制保护电压")]
        [Description("ComplianceVoltage_V")]
        [PropEditable(true)]
        public double ComplianceVoltage_V { get; set; }


        [DisplayName("是否启用脉冲扫描模式，默认连续扫描")]
        [Description("PulsedMode")]
        [PropEditable(true)]
        public bool PulsedMode { get; set; }




        //DAQ采集卡
        [DisplayName("PD电压转电流换算系数K(y=kx+b)")]
        [Description("PDFactor_K_1st")]
        [PropEditable(true)]
        public double PDFactor_K_1st { get; set; }


        [DisplayName("PD电压转电流换算系数B(y=kx+b)")]
        [Description("PDFactor_B_1st")]
        [PropEditable(true)]
        public double PDFactor_B_1st { get; set; }



        [DisplayName("PD电流转功率换算系数K(y=kx+b)")]
        [Description("PDFactor_K_2ed")]
        [PropEditable(true)]
        public double PDFactor_K_2ed { get; set; }


        [DisplayName("PD电流转功率换算系数B(y=kx+b)")]
        [Description("PDFactor_B_2ed")]
        [PropEditable(true)]
        public double PDFactor_B_2ed { get; set; }



        //[DisplayName("MPD电流转功率换算系数K(y=kx+b)")]
        //[Description("MPDFactor_K_2ed")]
        //[PropEditable(true)]
        //public double MPDFactor_K { get; set; }


        //[DisplayName("MPD电流转功率换算系数B(y=kx+b)")]
        //[Description("MPDFactor_B_2ed")]
        //[PropEditable(true)]
        //public double MPDFactor_B { get; set; }



    }
}
