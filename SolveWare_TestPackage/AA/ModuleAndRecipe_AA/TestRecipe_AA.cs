using SolveWare_BurnInCommon;
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
    public class TestRecipe_AA : TestRecipeBase
    {
        public TestRecipe_AA()
        {
            this.Inherit = true;
            this.Analog_CH = 1;
            this.InitialCurrentSense_mA = 0.1;
            this.PowerThreshold_mA = 0.01;
            //this.EnableLD = true;
            //this.Current_LD_mA = 80.0;
            //this.ComplianceVoltage_LD_V = 2.5;
            //this.EnableEA = true;
            //this.Voltage_EA_V = -2.0;
            //this.ComplianceCurrent_EA_mA = 120;
            //this.EnableSOA = false;
            //this.Current_SOA_mA = 50.0;
            //this.ComplianceVoltage_SOA_V = 2.5;

            this.Rough_Trajspeed = 2;
            this.Rough_Radius = 0.5;
            this.Rough_Involute_Interval = 0.01;
            this.Layer_Step = 0.005;

            //this.DoubleSizeline_Trajspeed = 1;
            //this.DoubleSizeline_Radius_Inside = 0;
            //this.DoubleSizeline_Radius = 0.07;
            //this.DoubleSizeline_Interval = 0.002;

            this.Fine_Trajspeed = 0.05;
            this.Fine_Radius = 0.01;
            this.Fine_Involute_Interval = 0.001;

            this.Creep_Step_um = 0.2; //蠕动步进
            this.CreepDelay_ms = 400;   //蠕动后等待多久稳定
            this.OutOfFocusDistance_um = 2;//离焦距离




            //this.Layer_Range = 0.2;
            //this.ScanDirByLayerPower = 3;
            //this.Involute_Enable = false;
            this.CrossScanCount = 2;

            OpticalSwitchChannel = 1;

        }
        [DisplayName("从QWLT2获取数值")]
        [Description("Inherit")]
        [PropEditable(true)]
        public bool Inherit { get; set; }

        [DisplayName("模拟量通道")]
        [Description("Analog_CH")]
        [PropEditable(true)]
        public int Analog_CH { get; set; }

        [DisplayName("初始PD电流量程(mA)")]
        [Description("InitialCurrentSense_mA")]
        [PropEditable(true)]
        public double InitialCurrentSense_mA { get; set; }

        //[DisplayName("启用LD加电")]
        //[Description("EnableLD")]
        //[PropEditable(true)]
        //public bool EnableLD { get; set; }

        //[DisplayName("LD电流(mA)")]
        //[Description("Current_LD_mA")]
        //[PropEditable(true)]
        //public double Current_LD_mA { get; set; }

        //[DisplayName("LD钳制电压(V)")]
        //[Description("ComplianceVoltage_LD_V")]
        //[PropEditable(true)]
        //public double ComplianceVoltage_LD_V { get; set; }

        //[DisplayName("启用EA加电")]
        //[Description("EnableEA")]
        //[PropEditable(true)]
        //public bool EnableEA { get; set; }

        //[DisplayName("EA电压(V)")]
        //[Description("Voltage_EA_V")]
        //[PropEditable(true)]
        //public double Voltage_EA_V { get; set; }

        //[DisplayName("EA钳制电流(mA)")]
        //[Description("ComplianceCurrent_EA_mA")]
        //[PropEditable(true)]
        //public double ComplianceCurrent_EA_mA { get; set; }

        //[DisplayName("启用SOA加电")]
        //[Description("EnableSOA")]
        //[PropEditable(true)]
        //public bool EnableSOA { get; set; }

        //[DisplayName("SOA电流(mA)")]
        //[Description("Current_SOA_mA")]
        //[PropEditable(true)]
        //public double Current_SOA_mA { get; set; }

        //[DisplayName("SOA钳制电压(V)")]
        //[Description("ComplianceVoltage_SOA_V")]
        //[PropEditable(true)]
        //public double ComplianceVoltage_SOA_V { get; set; }

        //[DisplayName("中心波长(nm)")]
        //[Description("CenterWavelength_nm")]
        //[PropEditable(true)]
        //public double CenterWavelength_nm { get; set; }

        [DisplayName("耦合PD电流下限(mA)")]
        [Description("PowerThreshold_mA")]
        [PropEditable(true)]
        public double PowerThreshold_mA { get; set; }

        //==========================

        [DisplayName("粗扫插补速度(mm/s)")]
        [Description("Rough_Trajspeed")]
        [PropEditable(true)]
        public double Rough_Trajspeed { get; set; }


        [DisplayName("粗扫半径(mm)")]
        [Description("Rough_Radius")]
        [PropEditable(true)]
        public double Rough_Radius { get; set; }

        [DisplayName("粗扫线间隔(mm)")]
        [Description("Rough_Involute_Interval")]
        [PropEditable(true)]
        public double Rough_Involute_Interval { get; set; }

        [DisplayName("粗扫寻光层步长(mm)")]
        [Description("Layer_Step")]
        [PropEditable(true)]
        public double Layer_Step { get; set; }

        //==========================


        //[DisplayName("Double粗扫插补速度(mm/s)")]
        //[Description("DoubleSizeline_Trajspeed")]
        //[PropEditable(true)]
        //public double DoubleSizeline_Trajspeed { get; set; }

        ////[DisplayName("Double粗扫内圈半径(mm)")]
        ////[Description("DoubleSizeline_Radius_Inside")]
        ////[PropEditable(true)]
        //public double DoubleSizeline_Radius_Inside { get; set; }

        //[DisplayName("Double粗扫半径(mm)")]
        //[Description("DoubleSizeline_Radius")]
        //[PropEditable(true)]
        //public double DoubleSizeline_Radius { get; set; }

        //[DisplayName("Double粗扫线间隔(mm)")]
        //[Description("DoubleSizeline_Interval")]
        //[PropEditable(true)]
        //public double DoubleSizeline_Interval { get; set; }

        //==========================

        [DisplayName("精扫线扫插补速度(mm/s)")]
        [Description("Fine_Trajspeed")]
        [PropEditable(true)]
        public double Fine_Trajspeed { get; set; }

        [DisplayName("精扫线扫半径(mm)")]
        [Description("Fine_Radius")]
        [PropEditable(true)]
        public double Fine_Radius { get; set; }


        [DisplayName("精扫线间隔(mm)")]
        [Description("Fine_Involute_Interval")]
        [PropEditable(true)]
        public double Fine_Involute_Interval { get; set; }

        //[DisplayName("是否执行粗扫")]
        //[Description("Rough_Enable")]
        //[PropEditable(true)]
        //public bool Rough_Enable { get; set; }

        //[DisplayName("寻光层范围(mm)")]
        //[Description("Layer_Range")]
        //[PropEditable(true)]
        //public double Layer_Range { get; set; }



        [DisplayName("蠕动耦合步长(um)")]
        [Description("Creep_Step")]
        [PropEditable(true)]
        public double Creep_Step_um { get; set; }

        [DisplayName("蠕动后稳定等待")]
        [Description("CreepDelay_ms")]
        [PropEditable(true)]
        public double CreepDelay_ms { get; set; }

        [DisplayName("蠕动离焦长度")]
        [Description("OutOfFocusDistance_um")]
        [PropEditable(true)]
        public double OutOfFocusDistance_um { get; set; }

        //[DisplayName("确认搜索方向层数")]
        //[Description("ScanDirByLayerPower")]
        //[PropEditable(true)]
        //public int ScanDirByLayerPower { get; set; }

        //[DisplayName("是否执行渐开线扫描")]
        //[Description("Involute_Enable")]
        //[PropEditable(true)]
        //public bool Involute_Enable { get; set; }

        [DisplayName("十字扫描次数")]
        [Description("CrossScanCount")]
        [PropEditable(true)]
        public int CrossScanCount { get; set; }

        [DisplayName("OSwitch光开关通道")]
        [Description("OpticalSwitchChannel")]
        [PropEditable(true)]
        public int OpticalSwitchChannel { get; set; }
    }
}