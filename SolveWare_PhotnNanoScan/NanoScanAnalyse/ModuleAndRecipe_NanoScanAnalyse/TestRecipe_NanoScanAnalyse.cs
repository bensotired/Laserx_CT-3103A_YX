using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;
using static SolveWare_BurnInInstruments.Photon_NanoScan;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_NanoScanAnalyse : TestRecipeBase
    {
        public TestRecipe_NanoScanAnalyse() 
        {
            //this.DrivingCurrent_mA = 100.0;
            this.aperture = NanoScan_AxisNameEnum_20050A.X;
            this.leftBound = 0;
            this.rightBound = 0;
            this.samplingRes = 0;
            this.decimation = 1;
            this.numOfPoints = 500;
            this.BiasBeforeWait_ms = 0;
            this.BiasAfterWait_ms = 0;
            this.ScanBeforeWait_ms = 8000;
            Default_DrivingCurrent_mA = 40.0;
            this.MoveDistance_mm = 1;
        }

        [DisplayName("是否使用前置测试结果参数作为驱动电流")]
        [Description("UseRefData_DrivingCurrent_mA")]
        [PropEditable(true)]
        public bool UseRefData_DrivingCurrent_mA { get; set; } = false;

        [DisplayName("使用前置测试结果参数作为驱动电流,该参数名称")]
        [Description("RefData_Name_DrivingCurrent_mA")]
        [PropEditable(true)]
        public string RefData_Name_DrivingCurrent_mA { get; set; } = "Ith2";
        [DisplayName("使用前置测试结果参数作为驱动电流时,电流的偏移量(区分正负值)")]
        [Description("RefData_DrivingCurrent_Offset_mA")]
        [PropEditable(true)]
        public double RefData_DrivingCurrent_Offset_mA { get; set; } = 0.0;


        [DisplayName("使用的默认驱动电流值")]
        [Description("Default_DrivingCurrent_mA")]
        [PropEditable(true)]
        public double Default_DrivingCurrent_mA { get; set; }




        [DisplayName("加电前等待时间(ms)")]
        [Description("BiasBeforeWait_ms")]
        [PropEditable(true)]
        public int BiasBeforeWait_ms { get; set; }

        [DisplayName("加电后等待时间(ms)，默认至少500ms")]
        [Description("BiasAfterWait_ms")]
        [PropEditable(true)]
        public int BiasAfterWait_ms { get; set; }

        [DisplayName("扫描前等待时间(ms)")]
        [Description("BiasBeforeWait_ms")]
        [PropEditable(true)]
        public int ScanBeforeWait_ms { get; set; }



        [DisplayName("X轴移动的距离")]
        [PropEditable(true)]
        public double MoveDistance_mm { get; set; }

        //[DisplayName("器件驱动电流(mA)")]
        //[PropEditable(true)]
        //public double DrivingCurrent_mA { get; set; }
        [DisplayName("轴名称（XY）")]
        [PropEditable(true)]
        public NanoScan_AxisNameEnum_20050A aperture { get; set; }
        [DisplayName("左边限制()")]
        [PropEditable(true)]
        public float leftBound { get; set; }
        [DisplayName("右边限制()")]
        [PropEditable(true)]
        public float rightBound { get; set; }
        [DisplayName("采样分辨率()")]
        [PropEditable(true)]
        public float samplingRes { get; set; }
        [DisplayName("decimation()")]
        [PropEditable(true)]
        public short decimation { get; set; }
        [DisplayName("numOfPoints()")]
        [PropEditable(true)]
        public int numOfPoints { get; set; }

        [DisplayName("扫描频率")]
        [PropEditable(true)]
        public float RotationFrequency { get; set; } = 20;
        [DisplayName("平滑比例")]
        [PropEditable(true)]
        public short finite { get; set; } = 5;
        [DisplayName("平滑次数")]
        [PropEditable(true)]
        public short rolling { get; set; } = 2;
        //public enum CurrentBase
        //{
        //    Null,
        //    Ith1,
        //    Ith2,
        //    Ith3,
        //    // Pop
        //}
    }
}