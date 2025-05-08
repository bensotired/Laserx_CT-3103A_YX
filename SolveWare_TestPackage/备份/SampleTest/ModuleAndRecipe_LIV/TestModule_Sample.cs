using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_Sample")]

    [ConfigurableInstrument("ISourceMeter_Golight", "SourceMeter_Master", "用于LIV扫描")]
    [ConfigurableInstrument("ISourceMeter_Golight", "SourceMeter_Slaver", "用于LIV扫描2")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC1", "用于LIV扫描2")]

    [ConfigurableAxis("Axis_H", "远场扫描H轴")]
    [ConfigurableAxis("Axis_V", "远场扫描V轴")]
    [ConfigurableAxis("Axis_X", "主导轨X轴")]
    [ConfigurableAxis("Axis_Y", "主导轨Y轴")]

    [ConfigurablePosition("Position_H", "远场扫描H轴中点")]
    [ConfigurablePosition("Position_V", "远场扫描V轴中点")]
    [ConfigurablePosition("Position_X", "主导轨X轴中点")]
    [ConfigurablePosition("Position_Y", "主导轨Y轴中点")]



    [StaticResource(ResourceItemType.AXIS, "SWRY_进料Y", "Y轴")]
    [StaticResource(ResourceItemType.AXIS, "SWRX_进料X", "X轴")]
    [StaticResource(ResourceItemType.IO, "demo_io_OutPut_1", "范例输出IO_1")]
    [StaticResource(ResourceItemType.IO, "demo_io_OutPut_2", "范例输出IO_2")]
    [StaticResource(ResourceItemType.IO, "demo_io_OutPut_3", "范例输出IO_3")]
    [StaticResource(ResourceItemType.IO, "demo_io_inPut_1", "范例输入IO_1")]
    [StaticResource(ResourceItemType.IO, "demo_io_inPut_2", "范例输入IO_2")]
    [StaticResource(ResourceItemType.IO, "demo_io_inPut_3", "范例输入IO_3")]
    [StaticResource(ResourceItemType.POS, "demo_pos_1", "范例点位_1")]
    [StaticResource(ResourceItemType.POS, "demo_pos_2", "范例点位_2")]
    [StaticResource(ResourceItemType.POS, "demo_pos_3", "范例点位_3")]
    [StaticResource(ResourceItemType.VICAL, "demo_vical_1", "范例视觉点位_1")]
    [StaticResource(ResourceItemType.VICAL, "demo_vical_2", "范例视觉点位_2")]
    [StaticResource(ResourceItemType.VICAL, "demo_vical_3", "范例视觉点位_3")]
    [StaticResource(ResourceItemType.VISION, "VisionController_LaserX", "视觉控制器")]
    public class TestModule_Sample : TestModuleBase
    {
        public TestModule_Sample() : base() { }
        #region 以get属性获取测试模块运行所需资源
        ISourceMeter_Golight SourceMeter_Master { get { return (ISourceMeter_Golight)this.ModuleResource["SourceMeter_Master"]; } }
        ISourceMeter_Golight SourceMeter_Slaver { get { return (ISourceMeter_Golight)this.ModuleResource["SourceMeter_Slaver"]; } }
        MotorAxisBase SWRY_进料Y { get { return (MotorAxisBase)this.ModuleResource["SWRY_进料Y"]; } }
        MotorAxisBase SWRX_进料X { get { return (MotorAxisBase)this.ModuleResource["SWRX_进料X"]; } }
        IOBase demo_io_OutPut_1 { get { return (IOBase)this.ModuleResource["demo_io_OutPut_1"]; } }
        IOBase demo_io_inPut_1 { get { return (IOBase)this.ModuleResource["demo_io_inPut_1"]; } }
        AxesPosition demo_pos_1 { get { return (AxesPosition)this.ModuleResource["demo_pos_1"]; } }
        AxesPosition demo_pos_2 { get { return (AxesPosition)this.ModuleResource["demo_pos_2"]; } }
        AxesPosition demo_vical_1 { get { return (AxesPosition)this.ModuleResource["demo_pos_1"]; } }
        AxesPosition demo_vical_2 { get { return (AxesPosition)this.ModuleResource["demo_pos_2"]; } }

        VisionController_LaserX_Image VisionController { get { return (VisionController_LaserX_Image)this.ModuleResource["VisionController_LaserX"]; } }
        #endregion

        TestRecipe_Sample TestRecipe { get; set; }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_Sample); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_Sample>(testRecipe); }
        RawData_Sample RawData { get; set; }
        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_Sample(); return RawData; }
        public override void Run(CancellationToken token)
        {
            try
            {
                //将输出IO设置为逻辑上的高电平
                demo_io_OutPut_1.TurnOn(true);
                //获取io逻辑电平状态
                var io_op_1 = demo_io_OutPut_1.Interation.IsActive;
                //新建测试模块内的单轴运动action - x轴
                MotionActionV2 Action_SWRX_进料X = new MotionActionV2();
                //单轴回零
                Action_SWRX_进料X.SingleAxisHome(this.SWRX_进料X);
                //单轴运动 - 绝对位置
                Action_SWRX_进料X.SingleAxisMotion(this.SWRX_进料X, 1.0);

                //新建测试模块内的单轴运动action - y轴
                MotionActionV2 Action_SWRY_进料Y = new MotionActionV2();
                //建立多轴运动 轴实例 vs 轴action 字典
                Dictionary<MotorAxisBase, MotionActionV2> axesDict = new Dictionary<MotorAxisBase, MotionActionV2>();
                axesDict.Add(this.SWRX_进料X, Action_SWRX_进料X);
                axesDict.Add(this.SWRY_进料Y, Action_SWRY_进料Y);
                //多轴并行运动
                MultipleAxisAction.Parallel_MoveToAxesPosition(axesDict, demo_pos_1);
                //多轴顺序运动 如点位设置了  x - y - z三轴位置 则先后按 x - y - z 轴运动
                MultipleAxisAction.Sequence_MoveToAxesPosition(axesDict, demo_pos_1, SequenceOrder.Normal);
                //多轴倒序运动 如点位设置了  x - y - z三轴位置 则先后按 z - y - x 轴运动
                MultipleAxisAction.Sequence_MoveToAxesPosition(axesDict, demo_pos_1, SequenceOrder.Reverse);

                //X轴运动停止 -  action
                Action_SWRX_进料X.Cancel();
                //Y轴运动停止 -  action
                Action_SWRY_进料Y.Cancel();
                //X轴运动停止 -  实例
                this.SWRX_进料X.Stop();
                //X轴运动停止 -  实例
                this.SWRY_进料Y.Stop();
                //点位运算 若具有相同的轴 则该轴位置作差  单个点位有的轴按原值保留  遍历顺序从 pos1 到 pos2
                var pos1_minus_pos2 = demo_pos_1 - demo_pos_2;
                //点位运算 若具有相同的轴 则该轴位置作和  单个点位有的轴按原值保留  遍历顺序从 pos1 到 pos2
                var pos1_plus_pos2 = demo_pos_1 + demo_pos_2;
                //向视觉控制器发送命令并返回通用数据结果
                VisionResult_LaserX_Image_Universal result =  this.VisionController.GetVisionResult_Universal("视觉命令", 100);
                //与仪器通讯
                var idn = this.SourceMeter_Master.InstrumentIDN;
                //将数据保存到raw data
                for (double i = this.TestRecipe.I_Start_mA; i < this.TestRecipe.I_Stop_mA; i += this.TestRecipe.I_Step_mA)
                {
                    RawData.Add(new RawDatumItem_Sample()
                    {
                        Current_mA = i,
                        Voltage_V = i * Math.E,
                        Power_mW = i * Math.PI * 2,
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}