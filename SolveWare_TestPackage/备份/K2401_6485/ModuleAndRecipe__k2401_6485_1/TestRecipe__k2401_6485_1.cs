using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage.K2401_6485.ModuleAndRecipe__k2401_6485_1
{
    public class TestRecipe_k2401_6485_1 : TestRecipeBase
    {

        public TestRecipe_k2401_6485_1()
        {
            NPLC = 0.1;
            I_A = 0.08;
            Delayafter_ms = 50;
            Samplinginterval_s = 2;
            IsPass = false;
            Samplingduration_m = 20;
            //I_Start_A = 0;
            //I_Stop_A = 0.1;
            //I_Step_A = 0.001;
            ldcomplianceVoltage_V = 2;
            triggerInputLine = 3;
            triggerOutputLine = 4;
            IsCurrentSenseAutoRangeOn = false;
            CurrentSenseRange_A = 0.01;
        }


        [DisplayName("交流电一个周期是50HZ 1代表一个完整的1HZ  为两点间采集时间2ms 可配置1,0.1,0.01")]
        [Description("NPLC")]
        [PropEditable(true)]
        public double NPLC { get; set; }

        [DisplayName("K2401扫描电流(A)")]
        [Description("I_A")]
        [PropEditable(true)]
        public double I_A { get; set; }

        [DisplayName("加电后采样前延时(ms)")]
        [Description("Delayafter_ms")]
        [PropEditable(true)]
        public int Delayafter_ms { get; set; }

        [DisplayName("采样时间间隔(S)")]
        [Description("Samplinginterval_s")]
        [PropEditable(true)]
        public int Samplinginterval_s { get; set; }

        [DisplayName("是否启用循环")]
        [Description("IsPass")]
        [PropEditable(true)]
        public bool IsPass { get; set; }
        [DisplayName("总采样时长(Min)")]
        [Description("Samplingduration_m")]
        [PropEditable(true)]
        public double Samplingduration_m { get; set; }

        //[DisplayName("K2401扫描起始电流(A)")]
        //[Description("I_Start_A")]
        //[PropEditable(true)]
        //public double I_Start_A { get; set; }

        //[DisplayName("K2401扫描结束电流(A)")]
        //[Description("I_Stop_A")]
        //[PropEditable(true)]
        //public double I_Stop_A { get; set; }

        //[DisplayName("K2401扫描步进电流(A)")]
        //[Description("I_Step_A")]
        //[PropEditable(true)]
        //public double I_Step_A { get; set; }


        [DisplayName("K2401保护电压(V)")]
        [Description("ldcomplianceVoltage_V")]
        [PropEditable(true)]
        public double ldcomplianceVoltage_V { get; set; }

        [DisplayName("K2401触发输入线路")]
        [Description("triggerInputLine")]
        [PropEditable(true)]
        public int triggerInputLine { get; set; }


        [DisplayName("K2401触发输出线路")]
        [Description("triggerOutputLine")]
        [PropEditable(true)]
        public int triggerOutputLine { get; set; }


        [DisplayName("K6485自动设置电流范围")]
        [Description("IsCurrentSenseAutoRangeOn")]
        [PropEditable(true)]
        public bool IsCurrentSenseAutoRangeOn { get; set; }


        [DisplayName("K6485设置电流上限")]
        [Description("CurrentSenseRange_A")]
        [PropEditable(true)]
        public double CurrentSenseRange_A { get; set; }



    }
}
