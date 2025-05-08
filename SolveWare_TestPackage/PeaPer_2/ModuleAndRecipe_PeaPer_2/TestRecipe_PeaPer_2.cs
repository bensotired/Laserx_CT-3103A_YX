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
    [Serializable]
    public class TestRecipe_PeaPer_2 : TestRecipeBase
    {
        public TestRecipe_PeaPer_2()
        {
            isTriggerModeEnable = true;

            isDoubleEnable = true;

            I_A = 1;
            Range1 = PDCurrentRange.档位3_2mA;
            Range2 = PDCurrentRange.档位3_2mA;
            //ComplianceVoltage_V = 3;
            //AcqTime_ms = 100;

            SweepSpeed = 150;
            CentralAngle_1 = 60;
            AcqDegStart_RoughSweep = 40;
            AcqDegEnd_RoughSweep = 40;
            AcqStepDeg_RoughSweep = 1;

            CentralAngle_2 = 150;
            AcqDegStart_FineSweep = 40;
            AcqDegEnd_FineSweep = 40;
            AcqStepDeg_FineSweep = 1;

            Factor_K_1st = 1;
            Factor_B_1st = 0;

            Factor_K_2ed = 1;
            Factor_B_2ed = 0;

        }

        [DisplayName("true：由FiberAlignment模块确定精确测试位置并运动至该位置" +
            "false：测试模块自动运动至预设测试位置")]
        [Description("SourceChannel")]
        [PropEditable(true)]
        public bool isTriggerModeEnable { get; set; }

        [DisplayName("true：双扇区(第一、第二)" +
        "false：单扇区（第一）")]
        [Description("isDoubleEnable")]
        [PropEditable(true)]
        public bool isDoubleEnable { get; set; }

        [DisplayName("源表LD加电电流(安)")]
        [Description("I_A")]
        [PropEditable(true)]
        public double I_A { get; set; }

        [DisplayName("第一扇区采集卡量程")]
        [Description("Range1")]
        [PropEditable(true)]
        public PDCurrentRange Range1 { get; set; }

        [DisplayName("第二扇区采集卡量程")]
        [Description("Range2")]
        [PropEditable(true)]
        public PDCurrentRange Range2 { get; set; }


        [DisplayName("偏振片旋转速度(°/s)")]
        [Description("SweepSpeed")]
        [PropEditable(true)]
        public int SweepSpeed { get; set; }



        [DisplayName("第一扇区偏振片旋转中心角度")]
        [Description("CentralAngle_1")]
        [PropEditable(true)]
        public int CentralAngle_1 { get; set; }

        [DisplayName("粗扫描偏振片旋转扫描起始负方向角度偏移量")]
        [Description("AcqRange_R")]
        [PropEditable(true)]
        public double AcqDegStart_RoughSweep { get; set; }


        [DisplayName("第一扇区偏振片旋转扫描结束正方向角度偏移量")]
        [Description("AcqRange_R")]
        [PropEditable(true)]
        public double AcqDegEnd_RoughSweep { get; set; }


        [DisplayName("第一扇区偏振片旋转扫描步进")]
        [Description("AcqStepAngle_Deg")]
        [PropEditable(true)]
        public double AcqStepDeg_RoughSweep { get; set; }



        [DisplayName("第二扇区偏振片旋转中心角度")]
        [Description("CentralAngle_2")]
        [PropEditable(true)]
        public int CentralAngle_2 { get; set; }
        [DisplayName("第二扇区偏振片旋转扫描起始负方向角度偏移量")]
        [Description("AcqRange_R")]
        [PropEditable(true)]
        public double AcqDegStart_FineSweep { get; set; }


        [DisplayName("第二扇区偏振片旋转扫描结束正方向角度偏移量")]
        [Description("AcqRange_R")]
        [PropEditable(true)]
        public double AcqDegEnd_FineSweep { get; set; }


        [DisplayName("第二扇区偏振片旋转扫描步进")]
        [Description("AcqStepAngle_Deg")]
        [PropEditable(true)]
        public double AcqStepDeg_FineSweep { get; set; }


        //DAQ采集卡
        [DisplayName("PD电压转电流换算系数K(y=kx+b)")]
        [Description("Factor_K_1st")]
        [PropEditable(true)]
        public float Factor_K_1st { get; set; }


        [DisplayName("PD电压转电流换算系数B(y=kx+b)")]
        [Description("Factor_B_1st")]
        [PropEditable(true)]
        public float Factor_B_1st { get; set; }



        [DisplayName("PD电流转功率换算系数K(y=kx+b)")]
        [Description("Factor_K_2ed")]
        [PropEditable(true)]
        public float Factor_K_2ed { get; set; }


        [DisplayName("PD电流转功率换算系数B(y=kx+b)")]
        [Description("Factor_B_2ed")]
        [PropEditable(true)]
        public float Factor_B_2ed { get; set; }

    }




}
