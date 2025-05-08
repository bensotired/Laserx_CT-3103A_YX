using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.ResourceProvider;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestPlugin_Demo
{
    public class TestPluginResourceProvider_CT3103 : TestPluginResourceProvider, ITesterCoreLink//单元运行状态
    {
        //public IBinSortResourceProvider Local_BinSort_ResourceProvider { get; private set; }
        public IBinSortListResourceProvider Local_BinSortList_ResourceProvider { get; private set; }
        //轴运动方法
        public Dictionary<AxisNameEnum_CT3103, MotionActionV2> AxesMotionAction = new Dictionary<AxisNameEnum_CT3103, MotionActionV2>();
        //轴运动参数
        public Dictionary<AxisNameEnum_CT3103, MotorAxisBase> Axes = new Dictionary<AxisNameEnum_CT3103, MotorAxisBase>();
        //io
        public Dictionary<IONameEnum_CT3103, IOBase> IOs = new Dictionary<IONameEnum_CT3103, IOBase>();
        //运动工位坐标
        public Dictionary<AxesPositionEnum_CT3103, AxesPosition> Positions = new Dictionary<AxesPositionEnum_CT3103, AxesPosition>();
        //视觉校准工位坐标
        public Dictionary<VisionCalibrationEnum_CT3103, AxesPosition> VCALPositions = new Dictionary<VisionCalibrationEnum_CT3103, AxesPosition>();
        public VisionController_LaserX_Image VisionController { get; private set; }

        //public TLB_6700Controllers TLB_6700 { get; set; }
        //public TMPL_Controllers TMPL_Master { get; set; }
        //public ScannerES4650 scannerES_1 { get; set; }
        //public ScannerES4650 scannerES_2 { get; set; }

        //public IDigitalIOController IO { get; set; }

        //public ISourceMeter_Golight sourceMeter { get; set; }

        public MeerstetterTECController_1089 TC_1 { get; set; }//左1
        public MeerstetterTECController_1089 TC_2 { get; set; }//右2
        //public MeerstetterTECController_1089 TC_3 { get; set; }//右1
        //public MeerstetterTECController_1089 TC_4 { get; set; }//右2
        public FWM8612 fWM { get; set; }
        public ScpiOsa oSA { get; set; }
        public TED4015 tED4015 { get; set; }

        public PXISourceMeter_4143 PD { get; set; }
        public PXISourceMeter_4143 BIAS2 { get; set; }
        public PXISourceMeter_4143 SOA1 { get; set; }
        public PXISourceMeter_4143 SOA2 { get; set; }
        public PXISourceMeter_4143 MIRROR2 { get; set; }
        public PXISourceMeter_4143 LP { get; set; }
        public PXISourceMeter_4143 MPD1 { get; set; }
        public PXISourceMeter_4143 MIRROR1 { get; set; }
        public PXISourceMeter_4143 BIAS1 { get; set; }
        public PXISourceMeter_4143 MPD2 { get; set; }
        public PXISourceMeter_4143 PH2 { get; set; }
        public PXISourceMeter_4143 PH1 { get; set; }
        public PXISourceMeter_4143 GAIN { get; set; }
        public PXISourceMeter_6683H S_6683H { get; set; }

        public OpticalSwitch OSwitch { get; set; }

        public override void ClearResource()
        {
            base.ClearResource();
            AxesMotionAction.Clear();
            Axes.Clear(); ;
            IOs.Clear();
            Positions.Clear();
            VCALPositions.Clear();
            Local_BinSortList_ResourceProvider = null;
            //Local_BinSort_ResourceProvider = null;
            VisionController = null;
        }
        public override void LocalizeResource()
        {
            this.Log_Global($"开始导入平台资源");
            this.Log_Global($"正在导入轴资源...");
            Axes.Clear();
            var axisNameEnum_Values = Enum.GetNames(typeof(AxisNameEnum_CT3103));
            foreach (var axisName in axisNameEnum_Values)
            {
                MotorAxisBase axisInstance = this.GetLocalResource<MotorAxisBase>(ResourceItemType.AXIS, axisName);
                if (axisInstance == null)
                {
                    throw new Exception($"不存在测试组件所需轴[{axisName}]的资源!");
                }
                else
                {
                    this.Axes.Add((AxisNameEnum_CT3103)Enum.Parse(typeof(AxisNameEnum_CT3103), axisName), axisInstance);
                    this.AxesMotionAction.Add((AxisNameEnum_CT3103)Enum.Parse(typeof(AxisNameEnum_CT3103), axisName), new MotionActionV2());
                    this.Log_Global($"导入轴[{axisName}]资源成功!");
                }
            }
             
            this.Log_Global($"正在导入位置资源...");
            Positions.Clear();
            var AxesPositionEnum_Values = Enum.GetNames(typeof(AxesPositionEnum_CT3103));
            foreach (var posName in AxesPositionEnum_Values)
            {
                AxesPosition positionInstance = this.GetLocalResource<AxesPosition>(ResourceItemType.POS, posName);
                if (positionInstance == null)
                {
                    throw new Exception($"不存在测试组件所需位置[{posName}]的资源!");
                }
                else
                {
                    this.Positions.Add((AxesPositionEnum_CT3103)Enum.Parse(typeof(AxesPositionEnum_CT3103), posName), positionInstance);
                    this.Log_Global($"导入位置[{posName}]资源成功!");
                }
            }


            this.Log_Global($"正在导入IO资源...");
            IOs.Clear();
            var IONameEnum_Values = Enum.GetNames(typeof(IONameEnum_CT3103));
            foreach (var ioName in IONameEnum_Values)
            {
                IOBase IoInstance = this.GetLocalResource<IOBase>(ResourceItemType.IO, ioName);
                if (IoInstance == null)
                {
                    throw new Exception($"不存在测试组件所需IO[{ioName}]的资源!");
                }
                else
                {
                    this.IOs.Add((IONameEnum_CT3103)Enum.Parse(typeof(IONameEnum_CT3103), ioName), IoInstance);
                    this.Log_Global($"导入IO[{ioName}]资源成功!");
                }
            }
            this.Log_Global($"正在导入视觉校准 Vision calibration axes position 资源...");
            VCALPositions.Clear();
            var VcalEnum_Values = Enum.GetNames(typeof(VisionCalibrationEnum_CT3103));
            foreach (var VcalName in VcalEnum_Values)
            {
                AxesPosition vcalPos = this.GetLocalResource<AxesPosition>(ResourceItemType.VICAL, VcalName);
                if (vcalPos == null)
                {
                    throw new Exception($"不存在测试组件所需视觉校准 Vision calibration axes position [{VcalName}]的资源!");
                }
                else
                {
                    this.VCALPositions.Add((VisionCalibrationEnum_CT3103)Enum.Parse(typeof(VisionCalibrationEnum_CT3103), VcalName), vcalPos);
                    this.Log_Global($"导入视觉校准 VCAL AP [{VcalName}]资源成功!");
                }
            }


            var VisionEnum_Values = Enum.GetNames(typeof(VisionControllerEnum_CT3103));
            this.VisionController = this.GetLocalResource<VisionController_LaserX_Image>(ResourceItemType.VISION, VisionControllerEnum_CT3103.VisionController_LaserX.ToString());

            this.TC_1 = this.GetLocalResource<MeerstetterTECController_1089>(ResourceItemType.INSTR, "TC_1");
            this.TC_2 = this.GetLocalResource<MeerstetterTECController_1089>(ResourceItemType.INSTR, "TC_2");
            //this.TC_3 = this.GetLocalResource<MeerstetterTECController_1089>(ResourceItemType.INSTR, "TC_3");
            //this.TC_4 = this.GetLocalResource<MeerstetterTECController_1089>(ResourceItemType.INSTR, "TC_4");
            this.fWM= this.GetLocalResource<FWM8612>(ResourceItemType.INSTR, "FWM8612");
            this.oSA= this.GetLocalResource<ScpiOsa>(ResourceItemType.INSTR, "OSA");
            this.tED4015= this.GetLocalResource<TED4015>(ResourceItemType.INSTR, "TED4015");
            this.OSwitch = this.GetLocalResource<OpticalSwitch>(ResourceItemType.INSTR, "OSwitch");

            this.PD = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "PD");
            this.BIAS2 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "BIAS2");
            this.SOA1 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "SOA1");
            this.SOA2 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "SOA2");
            this.MIRROR2 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "MIRROR2");
            this.LP = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "LP");
            this.MPD1 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "MPD1");
            this.MIRROR1 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "MIRROR1");
            this.BIAS1 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "BIAS1");
            this.MPD2 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "MPD2");
            this.PH2 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "PH2");
            this.PH1 = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "PH1");
            this.GAIN = this.GetLocalResource<PXISourceMeter_4143>(ResourceItemType.INSTR, "GAIN");
            this.S_6683H = this.GetLocalResource<PXISourceMeter_6683H>(ResourceItemType.INSTR, "6683H");

            Merged_PXIe_4143.MergedSource(PD, BIAS2, SOA1, SOA2, MIRROR2, LP, MPD1, MIRROR1, BIAS1, MPD2, PH2, PH1, GAIN, S_6683H);
            this.Log_Global($"导入资源完成!");
        }

        public override void Setup(TestPluginConfigItem resourceItems)
        {
            base.Setup(resourceItems);
            foreach (var item in resourceItems.ResourcePluginItems)
            {
                switch (item.ItemType)
                {
                    case ResourceItemType.BIN:

                        var app = this.CoreResource.GetAppPlugin(item.AppPluginName);
                        if (app == null)
                        {
                            throw new Exception($"未能为测试组件加载APP插件[{item.AppPluginName}]!");
                        }
                        //Local_BinSort_ResourceProvider = (IBinSortResourceProvider)app;
                        Local_BinSortList_ResourceProvider = (IBinSortListResourceProvider)app;
                        break;
                }
            }

        }

        public override bool MonitorKeyResourceStatus(CancellationTokenSource tokenSource)
        {
            return true;
        }
        public override void Dispose()
        {
            base.Dispose();
            //Local_BinSort_ResourceProvider = null;
            Local_BinSortList_ResourceProvider = null;
            //轴运动方法
            AxesMotionAction = null;
            //轴运动参数
            //io
            Axes = null; IOs = null;
            //运动工位坐标
            Positions = null;
            //视觉校准工位坐标
            VCALPositions = null;
            VisionController = null;
            //TMPL_Master = null;
            //scannerES_1 = null;
            //scannerES_2 = null;
        }
    }
}