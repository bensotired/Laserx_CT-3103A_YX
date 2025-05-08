using LX_BurnInSolution.Utilities;
using SolveWare_Analog;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SolveWare_BurnInInstruments.LaserX_9078_Utilities;
using static SolveWare_TestPackage.LaserX_9078_Traj_Function;
using static SolveWare_TestPackage.TestModule_AA;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     ("TestCalculator_QWLT2")]


    #region  轴、位置、IO、仪器
    //[StaticResource(ResourceItemType.AXIS, "左短摆臂旋转", "左短摆臂")]
    //[StaticResource(ResourceItemType.IO, "Input_左PER前后移动气缸动作", "左PER前后动作")]
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [StaticResource(ResourceItemType.AXIS, "LNX", "LNX")] // 耦合模块X轴
    [StaticResource(ResourceItemType.AXIS, "LNY", "LNY")] // 耦合模块Y轴
    [StaticResource(ResourceItemType.AXIS, "LNZ", "LNZ")] // 耦合模块Z轴
    [ConfigurableInstrument("PXISourceMeter_4143", "PD", "PD")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA1", "SOA1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA2", "SOA2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "LP", "LP")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH1", "PH1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH2", "PH2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR1", "MIRROR1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR2", "MIRROR2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS1", "BIAS1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS2", "BIAS2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MPD1", "MPD1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MPD2", "MPD2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "GAIN", "GAIN")]
    [ConfigurableInstrument("PXISourceMeter_6683H", "6683H", "6683H")]
    [ConfigurableInstrument("FWM8612", "FWM8612", "波长计")]
    [ConfigurableInstrument("ScpiOsa", "OSA", "OSA")]
    [ConfigurableInstrument("OSA_AQ67370", "OSA_6370", "OSA_6370")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]

    #endregion
    public class TestModule_AlternativeQWLT : TestModuleBase
    {

        string ModuleName = "AlternativeQWLT";
        public TestModule_AlternativeQWLT() : base() { }

        #region 以Get获取资源
        IOBase SwitchPD { get { return (IOBase)this.ModuleResource["PD_3"]; } }
        private MotorAxisBase X2 { get { return (MotorAxisBase)this.ModuleResource["LNX"]; } }
        private MotorAxisBase Y2 { get { return (MotorAxisBase)this.ModuleResource["LNY"]; } }
        private MotorAxisBase Z2 { get { return (MotorAxisBase)this.ModuleResource["LNZ"]; } }

        PXISourceMeter_4143 PD { get { return (PXISourceMeter_4143)this.ModuleResource["PD"]; } }
        PXISourceMeter_4143 SOA1 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA1"]; } }
        PXISourceMeter_4143 SOA2 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA2"]; } }
        PXISourceMeter_4143 LP { get { return (PXISourceMeter_4143)this.ModuleResource["LP"]; } }
        PXISourceMeter_4143 PH1 { get { return (PXISourceMeter_4143)this.ModuleResource["PH1"]; } }
        PXISourceMeter_4143 PH2 { get { return (PXISourceMeter_4143)this.ModuleResource["PH2"]; } }
        PXISourceMeter_4143 MIRROR1 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR1"]; } }
        PXISourceMeter_4143 MIRROR2 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR2"]; } }
        PXISourceMeter_4143 BIAS1 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS1"]; } }
        PXISourceMeter_4143 BIAS2 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS2"]; } }
        PXISourceMeter_4143 MPD1 { get { return (PXISourceMeter_4143)this.ModuleResource["MPD1"]; } }
        PXISourceMeter_4143 MPD2 { get { return (PXISourceMeter_4143)this.ModuleResource["MPD2"]; } }
        PXISourceMeter_4143 GAIN { get { return (PXISourceMeter_4143)this.ModuleResource["GAIN"]; } }
        PXISourceMeter_6683H S_6683H { get { return (PXISourceMeter_6683H)this.ModuleResource["6683H"]; } }

        FWM8612 FWM8612 { get { return (FWM8612)this.ModuleResource["FWM8612"]; } }

        ScpiOsa OSA_86142B { get { return (ScpiOsa)this.ModuleResource["OSA"]; } }
        OSA_AQ67370 OSA_6370 { get { return (OSA_AQ67370)this.ModuleResource["OSA_6370"]; } }
        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }

        #endregion

        TestRecipe_AlternativeQWLT TestRecipe { get; set; }
        RawData_AlternativeQWLT RawData { get; set; }

        RawDataMenu_AlternativeQWLT RawDataMenu { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_AlternativeQWLT);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawDataMenu = new RawDataMenu_AlternativeQWLT();
            return RawDataMenu;
        }
        public void Choose(string section, out PXISourceMeter_4143 pXISource)
        {
            var source = (Section)Enum.Parse(typeof(Section), section);
            switch (source)
            {
                case Section.PD:
                    pXISource = PD;
                    break;
                case Section.SOA1:
                    pXISource = SOA1;
                    break;
                case Section.SOA2:
                    pXISource = SOA2;
                    break;
                case Section.LP:
                    pXISource = LP;
                    break;
                case Section.PH1:
                    pXISource = PH1;
                    break;
                case Section.PH2:
                    pXISource = PH2;
                    break;
                case Section.MIRROR1:
                    pXISource = MIRROR1;
                    break;
                case Section.MIRROR2:
                    pXISource = MIRROR2;
                    break;
                case Section.BIAS1:
                    pXISource = BIAS1;
                    break;
                case Section.BIAS2:
                    pXISource = BIAS2;
                    break;
                case Section.MPD1:
                    pXISource = MPD1;
                    break;
                case Section.MPD2:
                    pXISource = MPD2;
                    break;
                case Section.GAIN:
                    pXISource = GAIN;
                    break;
                default:
                    pXISource = null;
                    break;
            }
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_AlternativeQWLT>(testRecipe);
        }
        
        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            MaskName = dutStreamData.MaskName;
            SerialNumber = dutStreamData.SerialNumber;
            var dtype = dutStreamData.GetType();
            var pProp = dtype.GetProperty("CoarseTuningPath");
            this.CoarseTuningPath = pProp.GetValue(dutStreamData).ToString();
        }
        string MaskName { get; set; }
        string SerialNumber { get; set; }

        private AxesPosition t_Start_Pos;

        int retestcount = 5;//最大测试次数

        string CoarseTuningPath { get; set; }
        const string Deviations_csv = "Deviations.csv";
        const string Midlines_csv = "Midlines.csv";
        //D:\CT-3103\LaserX_TesterLibrary\Data\1_4_K-02-28_Post BI_20240708_111118\Coarse_tuning
        public override void Run(CancellationToken token)
        {
            try
            {
                string path = Application.StartupPath + $@"\Data\AlternativeQWLT\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\AlternativeQWLT";
                }

                if (string.IsNullOrEmpty(MaskName))
                {
                    MaskName = "DO721";
                }

                int Ch = this.TestRecipe.ITU_Channel; //目标通道

                List<string> strline = new List<string>();
                string strHead = "";

                //string CurrentFile = @"D:\CT-3103\LaserX_TesterLibrary\Data\98_1_TEST_Pre BI_20240722_172307\Coarse_tuning\Deviations.csv";
                string CurrentFile = this.CoarseTuningPath;

                #region 读取文件处理文件
                if (File.Exists(CurrentFile))
                {
                    for (int retry = 0; retry < 100; retry++)
                    {
                        try
                        {
                            bool Head = false;


                            //读取文件行
                            List<string> fileline = new List<string>();

                            using (StreamReader sr = new StreamReader(CurrentFile, Encoding.Default))
                            {
                                //逐行读取文件处理至文件结束
                                string str = "";

                                while ((str = sr.ReadLine()) != null)
                                {
                                    var col = str.Split(',');

                                    int tCh = 0;

                                    if (Head == false)
                                    {
                                        //输出文件头
                                        if (col[0].ToUpper() == "CH")
                                        {
                                            strHead = str;
                                            Head = true;
                                        }
                                    }

                                    if (int.TryParse(col[0], out tCh) == true)
                                    {
                                        if (tCh == Ch)//发现目标通道
                                        {
                                            strline.Add(str);
                                            //break;
                                        }
                                    }
                                }

                            }

                            if (strline.Count() == 0)
                            {
                                throw new Exception("目标通道不存在");
                            }

                            break;


                        }
                        catch
                        {

                        }

                        Thread.Sleep(100);
                    }
                }
                else
                {
                    throw new Exception("目标文件不存在");
                }

                List<Dictionary<string, double>> tdic = new List<Dictionary<string, double>>();
                string[] lsthead = strHead.Split(',');
                for (int tdici = 0; tdici < strline.Count(); tdici++)
                {
                    string[] lstdata = strline[tdici].Split(',');
                    if (lstdata.Count() >= lsthead.Count())
                    {
                        Dictionary<string, double> t = new Dictionary<string, double>();

                        for (int tdici2 = 0; tdici2 < lsthead.Count(); tdici2++)
                        {
                            t.Add(lsthead[tdici2], double.Parse(lstdata[tdici2]));
                        }
                        tdic.Add(t);
                    }

                }
                #endregion

                foreach (var chitem in tdic)
                {
                    //通道字典项


                    //CH Itu_WL  Itu_Freq Gain[mA]    SOA1[mA]    SOA2[mA]    Mirror1[mA] Mirror2[mA] L_Phase[mA] Phase1[mA]  Phase2[mA]  Wavelength[nm]  Deviation[nm]   SMSR[dB]    Power[dBm]  MZM1[V] MZM2[V] MidlineIndex


                    //string path = Application.StartupPath + $@"\Data\QWLT2\{DateTime.Now:yyyyMMdd_HHmmss}";
                    //if (!string.IsNullOrEmpty(SerialNumber))
                    //{
                    //    path = Application.StartupPath + $@"\Data\{SerialNumber}\QWLT2";
                    //}
                    //Turn on : Gain ? L-phase ?Mirror#1?Mirror#2?Phase#1?Phase#2?SOA#1?SOA#2?others
                    //Turn off : others? SOA#2 ? SOA#1 ?Phase#2?Phase#1?Mirror#2?Mirror#1?L-phase?Gain
                    int SerachDir = 1;


                    #region  尝试耦合
                    ///历史从此开始
                    if (TestRecipe.EnableAlignment == true)
                    {
                        if (TestRecipe.Creep_Step_um > 0)
                        {
                            this.Log_Global($"重新快速找光");
                                                        
                            Merged_PXIe_4143.Reset();

                            this.Log_Global("开始加电.");

                            //OSwitch切换:
                            {
                                var och = Convert.ToByte(this.TestRecipe.LIVOpticalSwitchChannel);
                                if (OSwitch.SetCH(och) == false)
                                {
                                    string msg = "光开关通道切换失败！";
                                    this.Log_Global(msg);
                                    throw new Exception(msg);
                                }
                            }

                            if (this.TestRecipe.Inherit)
                            {
                                GAIN.AssignmentMode_Current(chitem["Gain[mA]"], 2.5);
                                LP.AssignmentMode_Current(chitem["L_Phase[mA]"], 2.5);
                                MIRROR1.AssignmentMode_Current(chitem["Mirror1[mA]"], 2.5);
                                MIRROR2.AssignmentMode_Current(chitem["Mirror2[mA]"], 2.5);
                                PH1.AssignmentMode_Current(chitem["Phase1[mA]"], 2.5);
                                PH2.AssignmentMode_Current(chitem["Phase2[mA]"], 2.5);
                                SOA1.AssignmentMode_Current(chitem["SOA1[mA]"], 2.5);
                                SOA2.AssignmentMode_Current(chitem["SOA2[mA]"], 2.5);
                            }
                            else
                            {
                                //临时设定为定制  以下数据需要从qwlt2获取
                                GAIN.AssignmentMode_Current(120, 2.5);
                                LP.AssignmentMode_Current(1.25, 2.5);
                                MIRROR1.AssignmentMode_Current(9.3, 1.6);
                                MIRROR2.AssignmentMode_Current(10.7, 1.6);

                                //PH1.AssignmentMode_Current(6, 2.5);
                                //PH2.AssignmentMode_Current(0, 2.5);
                                SOA1.AssignmentMode_Current(50, 2.5);
                                SOA2.AssignmentMode_Current(40, 2.5);


                                MPD1.AssignmentMode_Voltage(-2, 20);
                                MPD2.AssignmentMode_Voltage(-2, 20);
                                BIAS1.AssignmentMode_Voltage(-3, 20);
                                BIAS2.AssignmentMode_Voltage(-3, 20);
                            }

                            //string LogDataMsg = string.Empty;
                            //string path = Application.StartupPath + $@"\Data\AlignmentResult\{DateTime.Now:yyyyMMdd_HHmmss}";
                            //if (!string.IsNullOrEmpty(SerialNumber))
                            //{
                            //    path = Application.StartupPath + $@"\Data\AlignmentResult\{SerialNumber}";
                            //}
                            //if (!Directory.Exists(path))
                            //{
                            //    Directory.CreateDirectory(path);
                            //}
                            //StringBuilder strb = new StringBuilder();
                            //StreamWriter sw;

                            //初始位
                            var actlnx = X2 as Motor_LaserX_9078;
                            var actlny = Y2 as Motor_LaserX_9078;
                            var actlnz = Z2 as Motor_LaserX_9078;
                            var ThreeAxisList = new List<Motor_LaserX_9078>() { actlnz, actlnx, actlny };

                            //初始位置
                            t_Start_Pos = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                            };

                            var P1 = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                            };

                            var LastP = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                            };

                            var t1 = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                            };

                            var t2 = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                            };

                            var StartPos = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                            };

                            foreach (var axis in ThreeAxisList)
                            {
                                axis.WaitMotionDone();
                            }

                            // Dictionary<int, PointResult> maxList = new Dictionary<int, PointResult>();

                            double size = TestRecipe.Fine_Radius;  //左右两边的空间

                            var ch = this.TestRecipe.Analog_CH - 1;


                            int id = 100;


                            double xSize = size;
                            double zSize = size;
                            double ySize = size;

                            double pd_Max = 0;

                            double current_PD_max = Analog_LaserX_9078.GetCurrent_mA(X2 as Motor_LaserX_9078, ch);
                            double Creep_step = TestRecipe.Creep_Step_um / 1000;//蠕动步进

                            //最大次数
                            int xserachcount = (int)(xSize / Creep_step);
                            int yserachcount = (int)(ySize / Creep_step);
                            int zserachcount = (int)(zSize / Creep_step);

                            int xyzserachcount = xserachcount;
                            int serachPointMax = 1;//需要2个点进行判断是否找到最大
                            int serachPointCount = 0;//当前搜索失败点

                            Motor_LaserX_9078 ln_axis = actlnx;
                            SerachDir = 1;
                            bool UpdatePosition;
                            int GetdataDelay_ms = (int)TestRecipe.CreepDelay_ms;

                            double axisspeed = TestRecipe.Fine_Trajspeed;

                            #region  先粗耦合一次

                            SwitchPD.TurnOn(false); //使用耦合通道
                            Thread.Sleep(100);

                            TrajResultItem result;
                            var UsedPlane = LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW;
                            //初始位
                            //var actlnx = X2 as Motor_LaserX_9078;
                            //var actlny = Y2 as Motor_LaserX_9078;
                            //var actlnz = Z2 as Motor_LaserX_9078;
                            //var ThreeAxisList = new List<Motor_LaserX_9078>() { actlnz, actlnx, actlny };
                            Dictionary<AxesPosition, double> retPoint = new Dictionary<AxesPosition, double>();
                            Dictionary<int, PointResult> maxList = new Dictionary<int, PointResult>();
                            string LogDataMsg = string.Empty;
                            string path_aa = Application.StartupPath + $@"\Data\AlignmentResult\{DateTime.Now:yyyyMMdd_HHmmss}";
                            if (!string.IsNullOrEmpty(SerialNumber))
                            {
                                path_aa = Application.StartupPath + $@"\Data\AlignmentResult\{SerialNumber}";
                            }
                            if (!Directory.Exists(path_aa))
                            {
                                Directory.CreateDirectory(path_aa);
                            }
                            StringBuilder strb = new StringBuilder();
                            StreamWriter sw_aa;
                            t_Start_Pos = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                            };
                            var P1_AA = new AxesPosition()
                            {
                                ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name = actlny.Name,
                                        CardNo = actlny.CardNo.ToString(),
                                        AxisNo = actlny.AxisNo.ToString(),
                                        Position = actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name = actlnz.Name,
                                        CardNo = actlnz.CardNo.ToString(),
                                        AxisNo = actlnz.AxisNo.ToString(),
                                        Position = actlnz.Get_CurUnitPos()
                                    },
                                }
                            };


                            {
                                //阈值停止
                                TrajThresholdStop thresholdStop = new TrajThresholdStop()
                                {
                                    En = true,
                                    ThCurrent_mA = new Dictionary<int, double>(),
                                    ThVoltage_mV = new Dictionary<int, double>()
                                };

                                //增加阈值
                                thresholdStop.ThCurrent_mA.Add(ch, TestRecipe.PowerThreshold_mA);

                                //设置挡位
                                Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, 0.1);

                                Log_Global($"开始AQWLT耦合");
                                result = Run_Involute(eRunSize_Table.Fine_Double, P1, UsedPlane, thresholdStop, token);

                                this.CheckCancellationRequested(token);

                                while (!DataAnalyze(result, false, out retPoint))
                                {
                                    this.CheckCancellationRequested(token);

                                    LogDataMsg = path_aa + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_AQWLT耦合超量程.csv";
                                    this.WriteCSCVFile(LogDataMsg, out strb, out sw_aa, result);

                                    result = Run_Involute(eRunSize_Table.Fine_Double, P1, UsedPlane, thresholdStop, token);

                                    this.CheckCancellationRequested(token);
                                }
                                LogDataMsg = path_aa + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_AQWLT耦合.csv";
                                this.WriteCSCVFile(LogDataMsg, out strb, out sw_aa, result);

                                this.JudgeThreshold_mW(retPoint);

                                P1_AA = this.FindP1(ThreeAxisList, maxList, 0, retPoint, false);


                                //id++;
                                //Log_Global($"开始精耦合[{id}]");

                                //this.Updatet1t2Pos(ThreeAxisList, P1, t1, t2);

                                //this.Updatet1t2Pos(ThreeAxisList, P1, StartPos, t2);

                                //double size = GetSerachSize(eRunSize_Table.Fine);  //左右两边的空间

                                //StartPos.ItemCollection.FirstOrDefault(item => item.Name == "LNX").Position -= size * 1;
                                //StartPos.ItemCollection.FirstOrDefault(item => item.Name == "LNZ").Position -= size * 1;
                                ////StartPos[MonIC.GetAxis(Axis.X2)] -= 0.3;
                                ////StartPos[MonIC.GetAxis(Axis.Z2)] -= 0.3;

                                ////此处需要运行到外部初始位置
                                //for (int i = 0; i < this.TestRecipe.CrossScanCount; i++)
                                //{
                                //    P1 = HorizontalLine(Alignmentpath, i, id, size * 3,
                                //                      ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine,
                                //                      out sw, out strb, out LogDataMsg, out result, out retPoint, token);

                                //    P1 = VerticalLine(Alignmentpath, i, id, size * 3,
                                //                      ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine,
                                //                      out sw, out strb, out LogDataMsg, out result, out retPoint, token);
                                //}
                                ////增加点
                                //P1 = MaxListAdd(ThreeAxisList, maxList, id, retPoint, true);
                                //firstFineScan = true;









                                //运行到P1点
                                this.MoveToAxesPosition(ThreeAxisList, P1_AA, token);
                                Thread.Sleep(300);
                                this.CheckCancellationRequested(token);
                                Log_Global($"结束QWLT耦合");
                            }

                            #endregion



                            this.Log_Global($"恢复Gain电流,进行光电流耦合");

                            GAIN.AssignmentMode_Current(chitem["Gain[mA]"], 2.5);

                            SwitchPD.TurnOn(true); //使用源表

                            double pdSenseCurrentRange_mA = 10;// Math.Round(pd_Max * 5, 6);

                            PD.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(0, pdSenseCurrentRange_mA);

                            Thread.Sleep(400);

                            pd_Max = PD.ReadCurrent_A() * 1000.0;
                            Log_Global($"源表读取当前光电流为[{pd_Max}]mA");

                            //最大轮
                            int maxStep = 1;
                            int SerachStep = 0;

                            //3方向搜索
                            for (int iSerach = 0; iSerach <= maxStep;)
                            {

                                switch (SerachStep)
                                {
                                    case 0:  // X+
                                        Log_Global($"开始蠕动扫描[{id}]");
                                        if (iSerach == maxStep) //只离焦一次
                                        {
                                            //远离焦点0.003mm
                                            double fd_um = this.TestRecipe.OutOfFocusDistance_um;
                                            if (fd_um < 0) fd_um = 0;
                                            if (fd_um > 1000) fd_um = 1000;
                                            Log_Global($"离焦[{fd_um}]um");
                                            actlny.MoveToV3(actlny.Get_CurUnitPos() - fd_um / 1000.0, axisspeed);  // 离焦
                                            actlny.WaitMotionDone();
                                            Thread.Sleep(GetdataDelay_ms);
                                        }

                                        ln_axis = actlnx;
                                        break;

                                    case 1:  // Z+
                                        ln_axis = actlnz;
                                        break;

                                    case 2:  // Y+
                                        ln_axis = actlny;
                                        break;

                                    case 3:

                                        Log_Global($"蠕动扫描[{id}] 最大光电流为[{pd_Max}_mA]");

                                        id++;
                                        iSerach++;
                                        SerachStep = 0;
                                        continue;
                                        break;

                                }

                                SerachStep++;

                                //最后2次搜索时候, 只判断一个点
                                if (iSerach >= maxStep)
                                {
                                    serachPointMax = 1;

                                    if (ln_axis == actlny)
                                    {
                                        continue;
                                    }
                                }

                                #region 进行搜索

                                //+方向
                                SerachDir = 1;
                                serachPointCount = 1;
                                Log_Global($"DEBUG 蠕动扫描中[{ln_axis.Name}] 方向[{SerachDir}]");
                                pd_Max = CreepGetPDCurrent_mA(ln_axis, ch);
                                UpdatePosition = false;
                                for (int i = 0; i < xyzserachcount; i++)
                                {

                                    ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + SerachDir * Creep_step, axisspeed);
                                    ln_axis.WaitMotionDone();
                                    Thread.Sleep(GetdataDelay_ms);

                                    //得到当前电流                            
                                    current_PD_max = CreepGetPDCurrent_mA(ln_axis, ch);

                                    if (pd_Max > current_PD_max)
                                    {
                                        if (serachPointCount >= serachPointMax)
                                        {
                                            ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + (-1) * serachPointMax * SerachDir * Creep_step, axisspeed);
                                            ln_axis.WaitMotionDone();
                                            Thread.Sleep(GetdataDelay_ms);
                                            break;
                                        }
                                        else
                                        {
                                            serachPointCount++;
                                        }


                                    }
                                    else
                                    {
                                        pd_Max = current_PD_max;
                                        P1.ItemCollection.FirstOrDefault(axis => axis.Name == ln_axis.Name).Position = ln_axis.Get_CurUnitPos(); ;
                                        UpdatePosition = true;
                                        serachPointCount = 1;
                                    }
                                }

                                if (UpdatePosition == false)
                                {
                                    SerachDir = -1;
                                    serachPointCount = 1;
                                    Log_Global($"DEBUG 蠕动扫描中[{ln_axis.Name}] 方向[{SerachDir}]");
                                    pd_Max = CreepGetPDCurrent_mA(ln_axis, ch);

                                    for (int i = 0; i < xyzserachcount; i++)
                                    {

                                        ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + SerachDir * Creep_step, axisspeed);
                                        ln_axis.WaitMotionDone();
                                        Thread.Sleep(GetdataDelay_ms);

                                        //得到当前电流
                                        current_PD_max = CreepGetPDCurrent_mA(ln_axis, ch);

                                        if (pd_Max > current_PD_max)
                                        {
                                            if (serachPointCount >= serachPointMax)
                                            {
                                                ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + (-1) * serachPointMax * SerachDir * Creep_step, axisspeed);
                                                ln_axis.WaitMotionDone();
                                                Thread.Sleep(GetdataDelay_ms);
                                                break;
                                            }
                                            else
                                            {
                                                serachPointCount++;
                                            }

                                        }
                                        else
                                        {
                                            pd_Max = current_PD_max;
                                            P1.ItemCollection.FirstOrDefault(axis => axis.Name == ln_axis.Name).Position = ln_axis.Get_CurUnitPos(); ;
                                            UpdatePosition = true;
                                            serachPointCount = 1;
                                        }
                                    }
                                }

                                #endregion


                            }
                            #region 

                            #endregion
                            //得到当前电流
                            //Log_Global($"蠕动耦合到的最大光电流（模拟量值）为[{pd_Max}_mA]");

                        }
                    }
 
                    #endregion

                    this.Log_Global($"开始测试!");

                    SwitchPD.TurnOn(false);
                    Merged_PXIe_4143.Reset();

                    //OSwitch切换:
                    {
                        var och = Convert.ToByte(this.TestRecipe.SPOpticalSwitchChannel);
                        if (OSwitch.SetCH(och) == false)
                        {
                            string msg = "光开关通道切换失败！";
                            this.Log_Global(msg);
                            throw new Exception(msg);
                        }
                    }

                    if (this.TestRecipe.Inherit)
                    {
                        GAIN.AssignmentMode_Current(chitem["Gain[mA]"], 2.5);
                        LP.AssignmentMode_Current(chitem["L_Phase[mA]"], 2.5);
                        MIRROR1.AssignmentMode_Current(chitem["Mirror1[mA]"], 2.5);
                        MIRROR2.AssignmentMode_Current(chitem["Mirror2[mA]"], 2.5);
                        PH1.AssignmentMode_Current(chitem["Phase1[mA]"], 2.5);
                        PH2.AssignmentMode_Current(chitem["Phase2[mA]"], 2.5);
                        SOA1.AssignmentMode_Current(chitem["SOA1[mA]"], 2.5);
                        SOA2.AssignmentMode_Current(chitem["SOA2[mA]"], 2.5);
                    }
                    else
                    {
                        //临时设定为定制  以下数据需要从qwlt2获取
                        GAIN.AssignmentMode_Current(100, 2.5);
                        LP.AssignmentMode_Current(1.25, 2.5);
                        MIRROR1.AssignmentMode_Current(9.3, 1.6);
                        MIRROR2.AssignmentMode_Current(10.7, 1.6);

                        //PH1.AssignmentMode_Current(6, 2.5);
                        //PH2.AssignmentMode_Current(0, 2.5);
                        SOA1.AssignmentMode_Current(50, 2.5);
                        SOA2.AssignmentMode_Current(40, 2.5);


                        MPD1.AssignmentMode_Voltage(-2, 20);
                        MPD2.AssignmentMode_Voltage(-2, 20);
                        BIAS1.AssignmentMode_Voltage(-3, 20);
                        BIAS2.AssignmentMode_Voltage(-3, 20);
                    }

                    double sourceDelay_s = 0.001;
                    double ApertureTime_s = 0.001;

                    float complianceVoltage_V = 2.5f;
                    double timeout_ms_fetchdata = 10 * 1000;

                    //Alternative quick WL tuning
                    //1.新建测试模块 AlternativeQuickWlTuning
                    //2.module recipe包含参数
                    //2.1 itu channel(如 CH48)
                    //2.2 bandwidth_range_Ghz(如 25 Ghz)
                    //2.3 Mirror_offset_mA
                    //2.4 Mirror_scaning_step_mA
                    //2.5 Mirror_retry_step_mA
                    //2.5 LaserPhase_start_mA , LaserPhase_stop_mA , LaserPhase_step_mA
                    //2.6 Phase1_2_start_mA , Phase1_2 _stop_mA, Phase1_2 _step_mA


                    ////48  1546.917    193.8
                    double itu_channel_wl_nm = chitem["Itu_WL"];// Convert.ToDouble("1546.917");
                    double itu_channel_freq_Thz = chitem["Itu_Freq"];//Convert.ToDouble("193.8");
                    //4.通过 193.8Thz ± bandwidth_range_Ghz(如 25 Ghz) 计算出目标波长区间
                    //下限频率：(193.8 * 10 ^ 12 - 25 * 10 ^ 9 = 193775 * 10 ^ 9 Hz 对应波长 C / 下限频率 = 1548.4nm
                    //-上限频率：(193.8 * 10 ^ 12 + 25 * 10 ^ 9 = 193825 * 10 ^ 9 Hz对应波长 C/ 上限频率 = 1548.0nm
                    var temp_itu_ch_freq_val = itu_channel_freq_Thz * Math.Pow(10, 12);
                    var temp_range_freq_val = this.TestRecipe.Bandwidth_range_Ghz * Math.Pow(10, 9);

                    //带宽范围
                    double upper_limit_wl_nm = OpticalMath.VelocityOfLight_mPerSec / (temp_itu_ch_freq_val - temp_range_freq_val) * 1e+9;
                    double lower_limit_wl_nm = OpticalMath.VelocityOfLight_mPerSec / (temp_itu_ch_freq_val + temp_range_freq_val) * 1e+9;

                    if (this.TestRecipe.SerachMode == eSerachMode.带宽范围)
                    {                        
                        this.Log_Global($"[{this.TestRecipe.Bandwidth_range_Ghz}]Ghz 带宽搜索目标范围 [{lower_limit_wl_nm} - {upper_limit_wl_nm}]nm");
                    }
                    else if (this.TestRecipe.SerachMode == eSerachMode.波长范围)
                    {
                        upper_limit_wl_nm = this.TestRecipe.Wavelength_range_nm_max;
                        lower_limit_wl_nm = this.TestRecipe.Wavelength_range_nm_min;
                        this.Log_Global($"波长搜索目标范围 [{lower_limit_wl_nm} - {upper_limit_wl_nm}]nm");
                    }

                    this.Log_Global($"-----MIRROR-----");

                    var RawData_m1m2 = new RawData_AlternativeQWLT();
                    RawData_m1m2.TestStepStartTime = DateTime.Now;
                    Thread.Sleep(5 * 1000);

                    //3.从产品mirror map结果文件 Deviations.csv 文件中读取目标itu channel 各section信息，  
                    // 其中(Mirror1[mA], Mirror2[mA])作为mirror diag center 扫描中心 搭配上面mirror_offset_mA使用

                    var M1central = chitem["Mirror1[mA]"];//onvert.ToDouble("15.3347");
                    var M2central = chitem["Mirror2[mA]"];//Convert.ToDouble("4.9936");

                    //循环用M1 M2
                    double tM1central = M1central;
                    double tM2central = M2central;

                    var retry_step_mA = this.TestRecipe.Mirror_retry_step_mA;           //变化m1 m2的步进
                    var normal_scan_offset_mA = this.TestRecipe.Mirror_Offset_mA;       //斜线扫描电流范围
                    var normal_scan_step_mA = this.TestRecipe.Mirror_ScanningStep_mA;   //斜线扫描电流步进
                    bool isAny_m1m2_wl_inRange = false;

                    this.Log_Global($"搜索步长 [{retry_step_mA}]mA");

                    //循环结束的输出结果
                    double last_chosen_wl_val = 0;
                    double last_chosen_m1_val = 0;
                    double last_chosen_m2_val = 0;

                    List<double> m1m2_offsetArray = new List<double>();
                    List<double> m1m2_wl_Array = new List<double>();
                    List<double> lp_current_list = new List<double>();
                    List<double> lp_wl_Array = new List<double>();
                    List<double> diff_wl_array = new List<double>();
                    #region 循环扫描 M1 M2 找目标波长
                    int countmax = this.TestRecipe.SerachLimit;

                    if (countmax < 5) countmax = 5;
                    if (countmax > 5000) countmax = 5000;
                    int FailCount = 0;

                    SerachDir = 0;
                    List<double> lstCurr = new List<double>();
                    List<double> lstwl = new List<double>();

                    for (int fori = 0; fori <= countmax; fori++)
                    {
                        this.Log_Global($"搜索第[{fori+1}]/[{countmax}]次");

                        if (fori >= countmax)
                        {
                            this.Log_Global($"搜索[{countmax-1}]次, 均无法找到目标波长");
                            break;
                        }

                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消 AlternativeQWLT");
                            return;
                        }
                        //找出上述波长数组中最接近在此波长区间（1548.0~1548.4）内的(m1, m2)点 ，

                        // 扫描到的波长， 大于目标， m1+val  m2-val
                        // 扫描到的波长， 小于目标， m1-val  m2+val

                        //(16.9398 +retry_step_mA, 8.4274 +retry_step_mA)
                        var M1start = tM1central + normal_scan_offset_mA;
                        var M1stop = tM1central - normal_scan_offset_mA;

                        var M2start = tM2central - normal_scan_offset_mA;
                        var M2stop = tM2central + normal_scan_offset_mA;



                        //若没法找到此区间 则需调整 mirror center(16.9398, 8.4274) 一个Mirror_retry_step_mA后重复此步骤
                        isAny_m1m2_wl_inRange = false;

                        //var M1start = M1central + normal_scan_offset_mA;
                        //var M1stop = M1central - normal_scan_offset_mA;
                        //var M2start = M2central - normal_scan_offset_mA;
                        //var M2stop = M2central + normal_scan_offset_mA;


                        var m1m2_sourceDelay_s = 0.01;
                        var m1m2_apertureTime_s = 0.005;

                        //建立扫描数组
                        double[] M1CurrentArray = M1start < M1stop ?
                            ArrayMath.CalculateArray(M1start, M1stop, normal_scan_step_mA) :
                            ArrayMath.CalculateArray(M1start, M1stop, -normal_scan_step_mA);//5 15

                        double[] M2CurrentArray = M2start < M2stop ?
                            ArrayMath.CalculateArray(M2start, M2stop, normal_scan_step_mA) :
                            ArrayMath.CalculateArray(M2start, M2stop, -normal_scan_step_mA);//15 5

                        m1m2_wl_Array.Clear();
                        m1m2_offsetArray.Clear();
                        for (int i = 0; i < Math.Min(M1CurrentArray.Length, M2CurrentArray.Length); i++)
                        {
                            m1m2_offsetArray.Add(Math.Round(normal_scan_offset_mA + i * normal_scan_step_mA, 3));
                        }

                        //5.按上述条件加给产品加电除mirror1,mirror2外， 对m1,m2进行对角线扫描并触发波长计，对对角线上每点对应的波长进行采集 得到(m1, m2)vs WL数组
                        int retest = 0; //20240718 波长计增加重测

                        if (true)
                        {
                            //string mirror2_trigger_signal_name = "";
                            //WavelengthAndPower waveandpower = new WavelengthAndPower();
                            //for (retest = 0; retest <= retestcount; retest++)
                            //{
                            //    if (retest == retestcount)
                            //    {
                            //        throw new Exception($"波长计获取结果数量[{retestcount}]次重试, 均与需求目标数量不等.");
                            //    }

                            //    FWM8612.RST();
                            //    FWM8612.SetTriggerSource(Source.EXTernal);
                            //    MIRROR1.Reset();
                            //    MIRROR2.Reset();


                            //    mirror2_trigger_signal_name = MIRROR2.BuildTermialName();
                            //    MIRROR2.SetDefaultTermialName(mirror2_trigger_signal_name);
                            //    S_6683H.TrigTerminalsStart(mirror2_trigger_signal_name);

                            //    MIRROR2.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)M1start, (float)M1stop, M1CurrentArray,
                            //      complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);
                            //    MIRROR1.SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning((float)M2start, (float)M2stop, M2CurrentArray,
                            //       complianceVoltage_V, this.TestRecipe.ApertureTime_s, true);


                            //    MIRROR1.TriggerOutputOn = true;
                            //    MIRROR2.TriggerOutputOn = true;


                            //    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(MIRROR2, new PXISourceMeter_4143[] { MIRROR1 });

                            //    FWM8612.EXTernalStart2();

                            //    //this.Log_Global($"开始Trigger扫描!");

                            //    Merged_PXIe_4143.Trigger(MIRROR2, new PXISourceMeter_4143[] { MIRROR1 });


                            //    if (FWM8612.FethEXTernalData(M1CurrentArray.Length, out waveandpower))//波长计
                            //    {
                            //        break;
                            //    }
                            //    else
                            //    {
                            //        this.Log_Global($"波长计获取结果数量[{waveandpower.Power.Count}]与需求目标数量[{M1CurrentArray.Length}]不等");
                            //        FWM8612.EXTernalStop();
                            //        S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);

                            //    }

                            //}

                            //this.Log_Global($"波长数据取回完成!");
                            //// var   result_mirroe1 = MIRROR1.Fetch_MeasureVals(Aggregate.Count, 100 * 1000.0);
                            //// var   result_mirroe2 = MIRROR2.Fetch_MeasureVals(Aggregate.Count, 100 * 1000.0);

                            //FWM8612.EXTernalStop();
                            //S_6683H.TrigTerminalsStop(mirror2_trigger_signal_name);

                            //MIRROR1.Reset();
                            //MIRROR2.Reset();

                            //for (int i = 0; i < waveandpower.Power.Count; i++)
                            //{
                            //    m1m2_wl_Array.Add(waveandpower.Wavelength[i]);
                            //}

                            MIRROR1.Reset();
                            MIRROR2.Reset();
                            FWM8612.RST();



                            List<double> MPD_Curr_list = new List<double>();
                            for (int i = 0; i < Math.Min(M1CurrentArray.Length, M2CurrentArray.Length); i++)
                            {
                                MIRROR1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(M1CurrentArray[i], complianceVoltage_V);
                                //Thread.Sleep(1);
                                MIRROR2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(M2CurrentArray[i], complianceVoltage_V);
                                Thread.Sleep(1);
                                var luckyWavelength = FWM8612.GetWavelenth();
                                m1m2_wl_Array.Add(luckyWavelength);
                            }
                            MIRROR1.Reset();
                            MIRROR2.Reset();
                        }
                        //找出上述波长数组中最接近在此波长区间（1548.0~1548.4）内的(m1, m2)点 ，若没法找到此区间 则需调整 mirror center(16.9398, 8.4274) 一个Mirror_retry_step_mA后重复此步骤


                        double last_chosen_val_gap = double.MaxValue;

                        int findedindex = -1;
               
                        //目标波长
                        double avg_wl_val = (lower_limit_wl_nm + upper_limit_wl_nm) / 2.0;

                        for (int index = 0; index < m1m2_wl_Array.Count; index++)
                        {
                            var wl_item = m1m2_wl_Array[index];  //波长

                            if(wl_item>0)
                            {
                                if (Math.Abs(avg_wl_val - wl_item) < last_chosen_val_gap)
                                {
                                    last_chosen_val_gap = Math.Abs(avg_wl_val - wl_item);
                                    findedindex = index;

                                }
                            }
                        }

                        if (findedindex == -1)
                        {
                            string msg = "";
                            if (m1m2_wl_Array.Count <= 0)
                            {
                                msg = "波长数据为空, 不可能的异常";
                            }
                            else
                            {
                                msg = "波长数据均无效[";
                                for (int index = 0; index < m1m2_wl_Array.Count; index++)
                                {
                                    var wl_item = m1m2_wl_Array[index];  //波长

                                    msg += wl_item.ToString() + ",";
                                }
                                msg += "]";
                            }
                            throw new Exception(msg);
                        }

                        double Proportion = 0.15;   //按照设定倍距离搜索运动

                        //在范围内跳出
                        if (JuniorMath.IsValueInLimitRange(m1m2_wl_Array[findedindex], lower_limit_wl_nm, upper_limit_wl_nm) == true)
                        {
                            isAny_m1m2_wl_inRange = true;
                            last_chosen_wl_val = m1m2_wl_Array[findedindex];
                            //last_chosen_m1m2_offset_val = offsetArray[index];
                            last_chosen_m1_val = M1CurrentArray[findedindex];
                            last_chosen_m2_val = M2CurrentArray[findedindex];
                            this.Log_Global($"M1 = [{last_chosen_m1_val}]mA M2 = [{last_chosen_m2_val}]mA");
                            this.Log_Global($"找到波长[{m1m2_wl_Array[findedindex]}]nm 目标范围[{lower_limit_wl_nm} - {upper_limit_wl_nm}]nm");
                            break;
                        }
                        //比范围小  // 扫描到的波长， 小于目标， m1-val  m2+val
                        else if (m1m2_wl_Array[findedindex] < lower_limit_wl_nm)
                        {
                            if (SerachDir != -1)
                            {
                                SerachDir = -1;  //负方向
                                lstCurr = new List<double>();
                                lstwl = new List<double>();
                            }

                            lstCurr.Add(tM1central);
                            lstwl.Add(m1m2_wl_Array[findedindex]);

                            if (lstCurr.Count > 2)
                            {
                                lstCurr.RemoveAt(0);
                                lstwl.RemoveAt(0);
                            }

                            int iC_M = 1;
                            if (lstCurr.Count == 2)
                            {
                                //计算目标电流
                                double TargetC = ((lstCurr[0] - lstCurr[1]) / (lstwl[0] - lstwl[1])) * (avg_wl_val - lstwl[1]) + lstCurr[1];
                                //计算倍数
                                double TargetC_M = (TargetC - lstCurr[1]) / retry_step_mA;

                                iC_M = (int)Math.Abs(TargetC_M * Proportion);//按照倍数距离运动
                                if (iC_M == 0) iC_M = 1; //至少运动1个
                            }

                            tM1central -= retry_step_mA * iC_M;
                            tM2central -= retry_step_mA * iC_M;



                            this.Log_Global($"负方向搜索[{iC_M}]倍,波长[{m1m2_wl_Array[findedindex]}]nm");
                            //if (m1m2_wl_Array[findedindex]==-1)
                            //    FailCount++;
                            //if (FailCount>5)
                            //{
                            //    this.Log_Global($"负方向搜索,连续次为[-1nm],退出不再搜索");
                            //}

                        }
                        //比范围小  // 扫描到的波长， 大于目标， m1+val  m2-val
                        else if (upper_limit_wl_nm < m1m2_wl_Array[findedindex])
                        {
                            if (SerachDir != 1)
                            {
                                SerachDir = 1;  //正方向
                                lstCurr = new List<double>();
                                lstwl = new List<double>();
                            }

                            lstCurr.Add(tM1central);
                            lstwl.Add(m1m2_wl_Array[findedindex]);

                            if (lstCurr.Count > 2)
                            {
                                lstCurr.RemoveAt(0);
                                lstwl.RemoveAt(0);
                            }

                            int iC_M = 1;
                            if (lstCurr.Count == 2)
                            {
                                //计算目标电流
                                double TargetC = ((lstCurr[0] - lstCurr[1]) / (lstwl[0] - lstwl[1])) * (avg_wl_val - lstwl[1]) + lstCurr[1];
                                //计算倍数
                                double TargetC_M = (TargetC - lstCurr[1]) / retry_step_mA;

                                iC_M = (int)Math.Abs(TargetC_M * Proportion); //按照倍数距离运动
                                if (iC_M == 0) iC_M = 1; //至少运动1个
                            }
                            tM1central += retry_step_mA * iC_M;
                            tM2central += retry_step_mA * iC_M;

                            this.Log_Global($"正方向搜索[{iC_M}]倍,波长[{m1m2_wl_Array[findedindex]}]nm");

                        }

                    }
                    #endregion

                    //找到了
                    if (isAny_m1m2_wl_inRange == true)
                    {
                        

                        //6.按上述(m1, m2)点作为条件扫描LaserPhase 对应的波长值, 得出 LaserPhase vs(WL-itu channel WL = 1546.917) Map,
                        //截取其中单调递减区间，并计算各区间斜率, 选择斜率为中间值的一段中点作为LaserPhase取值

                        this.Log_Global($"-----LP-----");

                        //给M1 M2 加电
                        MIRROR1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(last_chosen_m1_val, complianceVoltage_V);
                        Thread.Sleep(1);
                        MIRROR2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(last_chosen_m2_val, complianceVoltage_V);
                        Thread.Sleep(1);


                        this.RawDataMenu.MIRROR1_val = last_chosen_m1_val;
                        this.RawDataMenu.MIRROR2_val = last_chosen_m2_val;


                        #region LP
                        var RawData_LP = new RawData_AlternativeQWLT();
                        RawData_LP.TestStepStartTime = DateTime.Now;

                        var LParray = this.TestRecipe.LaserPhase_mA.Split(',');
                        if (LParray.Length != 3)
                        {
                            this.Log_Global("[LP_mA] Parameter error");
                            return;
                        }

                    
                        double[] LPCurrentarray = ArrayMath.CalculateArray(double.Parse(LParray[0]), double.Parse(LParray[2]), double.Parse(LParray[1]));

                        lp_current_list.AddRange(LPCurrentarray);

                        if (true)
                        {

                            for (int i = 0; i < LPCurrentarray.Length; i++)
                            {
                                LP.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(LPCurrentarray[i], complianceVoltage_V);
                                Thread.Sleep(1);
                                var luckyWavelength = FWM8612.GetWavelenth();
                                lp_wl_Array.Add(luckyWavelength);
                            }
                            LP.Reset();

                        }


                      
                        for (int i = 0; i < LPCurrentarray.Length; i++)
                        {
                            diff_wl_array.Add(lp_wl_Array[i] - itu_channel_wl_nm);

                            RawData_LP.Add(new RawDatumItem_AlternativeQWLT()
                            {
                                Section = "LP",
                                Current_mA_or_Mirror_Diagonal_Offset = lp_current_list[i],
                                //Current_mA_or_Mirror_Diagonal_Offset = lp_current_list[i] * 1000,
                                //LaserPhase vs(WL - itu channel WL = 1546.917)
                                Wavelength_nm = lp_wl_Array[i] - itu_channel_wl_nm,

                            });
                        }

                        //截取其中单调递减区间，并计算各区间斜率
                        //单调是怎么选择出来的
                        var wl_DecreasingRanges = QWLT_InternalMath.FindDecreasingRanges(diff_wl_array.ToArray());///{1,2,3}
                        var temp_slope_index = 0;
                        Dictionary<Range, double> decRngSlps = new Dictionary<Range, double>();
                        foreach (var item in wl_DecreasingRanges)
                        {
                            if (Math.Abs(item.Start - item.End) < 1)
                            {
                                continue;
                            }
                            List<double> temp_x_Range = new List<double>();
                            List<double> temp_y_Range = new List<double>();

                            for (int i = item.Start; i < item.End; i++)
                            {
                                temp_x_Range.Add(lp_current_list[i]);
                                temp_y_Range.Add(diff_wl_array[i]);
                            }
                            const int order = 1;
                            var coeff = PolyFitMath.PolynomialFit(temp_x_Range.ToArray(), temp_y_Range.ToArray(), order);

                            decRngSlps.Add(item, coeff.Coeffs[order]);
                        }

                        var sortedDictionary = decRngSlps.OrderBy(x => x.Value).ToList();

                        // 计算中间位置
                        int middleIndex = sortedDictionary.Count / 2;

                        // 取得中间的键值对
                        var middleElement = sortedDictionary[middleIndex];

                        var lp_chosen_start_mA = lp_current_list[middleElement.Key.Start];
                        var lp_chosen_stop_mA = lp_current_list[middleElement.Key.End];
                        var lp_chosen_val_mA = (lp_chosen_start_mA + lp_chosen_stop_mA) / 2.0;


                        RawData_LP.Section = "LP";
                        RawData_LP.Driver_mA = $"{double.Parse(LParray[0])}-{double.Parse(LParray[2])} step[{double.Parse(LParray[1])}]";
                        RawData_LP.resut = $"LP_value=[{lp_chosen_val_mA * 1000}]mA";
                        RawData_LP.TestStepEndTime = DateTime.Now;
                        RawData_LP.TestCostTimeSpan = RawData_LP.TestStepEndTime - RawData_LP.TestStepStartTime;
                        this.RawDataMenu.Add(RawData_LP);
                        this.RawDataMenu.LP_val = lp_chosen_val_mA;

                        this.Log_Global($"LP result = [{lp_chosen_val_mA  }]mA");
                        #endregion



                        //7.扫描phase 1 / phase 2 找出对应功率最大点的(p1, p2） ，输出最终结果   m1, m2, lp, p1, p2
                        //LP加电
                        LP.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(lp_chosen_val_mA, complianceVoltage_V);
                        Thread.Sleep(1);

                        var array_p1_p2_sweep_setting = this.TestRecipe.P1_P2_mA.Split(',');//0·20
                        if (array_p1_p2_sweep_setting.Length != 3)
                        {
                            this.Log_Global("[P1_P2_mA] Parameter error");
                            return;
                        }
                        double[] p1_p2_Currents_mA = ArrayMath.CalculateArray(double.Parse(array_p1_p2_sweep_setting[0]), double.Parse(array_p1_p2_sweep_setting[2]), double.Parse(array_p1_p2_sweep_setting[1]));


                        this.Log_Global($"-----PH_Max-----");

                        SwitchPD.TurnOn(true);
                        //OSwitch切换:
                        {
                            var och = Convert.ToByte(this.TestRecipe.LIVOpticalSwitchChannel);
                            if (OSwitch.SetCH(och) == false)
                            {
                                string msg = "光开关通道切换失败！";
                                this.Log_Global(msg);
                                throw new Exception(msg);
                            }
                        }

                        #region PH_Max
                        this.RawData = new RawData_AlternativeQWLT();
                        this.RawData.TestStepStartTime = DateTime.Now;
                        //记录搜索模式
                        this.RawData.SerachMode = this.TestRecipe.SerachMode.ToString();

                        //切换光开光到tap pd光路
                        var PD_K = this.TestRecipe.PD_K;
                        var PD_B = this.TestRecipe.PD_B;

                        float p1_p2_idle_val_mA = 0.0f;
                        float pdsourceVoltage_V = 0.0f;

                        PH2.Reset();
                        PH1.Reset();
                        PD.Reset();

                        PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(p1_p2_idle_val_mA, complianceVoltage_V);
                        PH1.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                        PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(pdsourceVoltage_V, (float)this.TestRecipe.PDComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                        PH1.TriggerOutputOn = true;
                        PD.TriggerOutputOn = true;

                        var slavers_Max = new PXISourceMeter_4143[] { PD };
                        Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH1, slavers_Max);
                        Merged_PXIe_4143.Trigger(PH1, slavers_Max);

                        //var PH1_Curr_Max = PH1.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;
                        var PD_P1_Curr_Max = PD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;

                        for (int i = 0; i < PD_P1_Curr_Max.Length; i++)
                        {
                            PD_P1_Curr_Max[i] = PD_P1_Curr_Max[i] * this.TestRecipe.PD_K + this.TestRecipe.PD_B;
                        }

                        PH2.Reset();
                        PH1.Reset();
                        PD.Reset();

                        PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(p1_p2_idle_val_mA, complianceVoltage_V);
                        PH2.SetupMaster_Sequence_SourceCurrent_SenseVoltage(float.Parse(array_p1_p2_sweep_setting[0]), float.Parse(array_p1_p2_sweep_setting[1]), float.Parse(array_p1_p2_sweep_setting[2]), this.TestRecipe.PCVoltage_V, sourceDelay_s, ApertureTime_s, true);
                        PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(pdsourceVoltage_V, (float)this.TestRecipe.PDComplianceCurrent_mA, p1_p2_Currents_mA.Length, sourceDelay_s, ApertureTime_s, true);

                        PH2.TriggerOutputOn = true;
                        PD.TriggerOutputOn = true;

                        Merged_PXIe_4143.ConfigureMultiChannelSynchronization(PH2, slavers_Max);
                        Merged_PXIe_4143.Trigger(PH2, slavers_Max);

                        var PD_P2_Curr_Max = PD.Fetch_MeasureVals(p1_p2_Currents_mA.Length, timeout_ms_fetchdata).CurrentMeasurements;

                        for (int i = 0; i < PD_P2_Curr_Max.Length; i++)
                        {
                            PD_P2_Curr_Max[i] = PD_P2_Curr_Max[i] * this.TestRecipe.PD_K + this.TestRecipe.PD_B;
                        }


                        List<double> phase_driving_Currents = new List<double>();
                        List<double> pd_reading_Currents  = new List<double>();
                        //p2
                        for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
                        {
                            phase_driving_Currents.Add(-p1_p2_Currents_mA[i]);
                            pd_reading_Currents.Add(PD_P2_Curr_Max[i] * 1000);
                            RawData.Add(new RawDatumItem_AlternativeQWLT()
                            {
                                Section = "PH_Max",
                                Current_mA_or_Mirror_Diagonal_Offset = -p1_p2_Currents_mA[i],
                                MPD_Current_mA = PD_P2_Curr_Max[i] * 1000,
                            });
                        }
                        //p1
                        for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
                        {
                            phase_driving_Currents.Add(p1_p2_Currents_mA[i]);
                            pd_reading_Currents.Add(PD_P1_Curr_Max[i] * 1000);
                            RawData.Add(new RawDatumItem_AlternativeQWLT()
                            {
                                Section = "PH_Max",
                                Current_mA_or_Mirror_Diagonal_Offset = p1_p2_Currents_mA[i],
                                MPD_Current_mA = PD_P1_Curr_Max[i] * 1000,
                            });
                        }
                        double max_PhaseValue = 0;
                        int max_mpd_index = 0;
                        int min_mpd_index = 0;

                        ArrayMath.GetMaxAndMinIndex(pd_reading_Currents.ToArray(), out max_mpd_index, out min_mpd_index);
                        max_PhaseValue = phase_driving_Currents[max_mpd_index];

                        string resut = string.Empty;
                        if (max_PhaseValue > 0 && max_PhaseValue != 0)
                        {
                            //    PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                            //    PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                            resut = $"PH1=[{Math.Abs(max_PhaseValue)}]mA & PH2=[0]mA";
                            this.RawData.PH_Max_1 = Math.Abs(max_PhaseValue);
                            this.RawData.PH_Max_2 = 0;
                            this.RawDataMenu.PH1_Max_val = Math.Abs(max_PhaseValue);
                            this.RawDataMenu.PH2_Max_val = 0;
                            this.Log_Global($"PH_Max 1st result = {resut}");
                        }
                        else if (max_PhaseValue < 0 && max_PhaseValue != 0)
                        {
                            //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(0, complianceVoltage_V);
                            //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                            resut = $"PH1=[0]mA & PH2=[{Math.Abs(max_PhaseValue)}]mA";
                            this.RawData.PH_Max_1 = 0;
                            this.RawData.PH_Max_2 = Math.Abs(max_PhaseValue);
                            this.RawDataMenu.PH1_Max_val = 0;
                            this.RawDataMenu.PH2_Max_val = Math.Abs(max_PhaseValue);

                            this.Log_Global($"PH_Max 1st result = {resut}");
                        }
                        else
                        {
                            //PH1.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                            //PH2.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(Math.Abs(max_PhaseValue), complianceVoltage_V);
                            resut = $"PH1=[{Math.Abs(max_PhaseValue)}]mA & PH2=[{Math.Abs(max_PhaseValue)}]mA";
                            this.RawData.PH_Max_1 = 0;
                            this.RawData.PH_Max_2 = Math.Abs(max_PhaseValue);
                            this.RawDataMenu.PH1_Max_val = 0;
                            this.RawDataMenu.PH2_Max_val = Math.Abs(max_PhaseValue);

                            this.Log_Global($"PH_Max 1st result = {resut}");
                        }


                        this.RawData.Section = "PH_Max";
                        this.RawData.Driver_mA = $"{double.Parse(array_p1_p2_sweep_setting[0])}-{double.Parse(array_p1_p2_sweep_setting[2])} step[{double.Parse(array_p1_p2_sweep_setting[1])}]";
                        this.RawData.resut = resut;
                        this.RawData.TestStepEndTime = DateTime.Now;
                        this.RawData.TestCostTimeSpan = this.RawData.TestStepEndTime - this.RawData.TestStepStartTime;
                        this.RawDataMenu.Add(RawData);

                        #endregion

                        #region 得到波长
                        {
                            //产品加电
                            GAIN.AssignmentMode_Current(chitem["Gain[mA]"], 2.5);
                            LP.AssignmentMode_Current(this.RawDataMenu.LP_val, 2.5);
                            MIRROR1.AssignmentMode_Current(this.RawDataMenu.MIRROR1_val, 2.5);
                            MIRROR2.AssignmentMode_Current(this.RawDataMenu.MIRROR2_val, 2.5);
                            PH1.AssignmentMode_Current(this.RawDataMenu.PH1_Max_val, 2.5);
                            PH2.AssignmentMode_Current(this.RawDataMenu.PH2_Max_val, 2.5);
                            SOA1.AssignmentMode_Current(chitem["SOA1[mA]"], 2.5);
                            SOA2.AssignmentMode_Current(chitem["SOA2[mA]"], 2.5);

                            //OSwitch切换:
                            {
                                var och = Convert.ToByte(this.TestRecipe.SPOpticalSwitchChannel);
                                if (OSwitch.SetCH(och) == false)
                                {
                                    string msg = "光开关通道切换失败！";
                                    this.Log_Global(msg);
                                    throw new Exception(msg);
                                }
                            }

                            this.RawDataMenu.Wavelength = FWM8612.GetWavelenth();
                        }
                        #endregion




                        //输出最终结果 m1, m2, lp, p1, p2

                        #region 打印

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string defaultFileName = string.Concat(@"AlternativeQWLT_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);
                        var finalFileName = $@"{path}\{defaultFileName}";

                        using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                        {
                            sw.WriteLine($"{MaskName}");// "DO721");
                            sw.WriteLine($"CH{this.TestRecipe.ITU_Channel} SMSR[dB]=****");
                            sw.WriteLine($"SerachMode={this.TestRecipe.SerachMode} [{lower_limit_wl_nm} - {upper_limit_wl_nm}]");
                            sw.WriteLine();
                            sw.WriteLine("Type,CH,Itu_WL,Itu_Freq,Gain[mA],SOA1[mA],SOA2[mA],Mirror1[mA],Mirror2[mA],L_Phase[mA],Phase1[mA],Phase2[mA],Wavelength[nm],Freq,Deviation[nm],SMSR[dB],Power[dBm],MZM1[V],MZM2[V],MidlineIndex");
                            sw.WriteLine($"OrgValue, {TestRecipe.ITU_Channel},{chitem["Itu_WL"]},{chitem["Itu_Freq"]}," +
                                $"{chitem["Gain[mA]"]},{chitem["SOA1[mA]"]},{chitem["SOA2[mA]"]}," +
                                $"{chitem["Mirror1[mA]"]},{chitem["Mirror2[mA]"]}," +
                                $"{chitem["L_Phase[mA]"]}," +
                                $"{chitem["Phase1[mA]"]},{chitem["Phase2[mA]"]}," +
                                $"{chitem["Wavelength[nm]"]}," +
                                $"{WaveLengthToFrequency(chitem["Wavelength[nm]"])}," +
                                $"{chitem["Deviation[nm]"]},{chitem["SMSR[dB]"]},{chitem["Power[dBm]"]}," +
                                $"{chitem["MZM1[V]"]},{chitem["MZM2[V]"]},{chitem["MidlineIndex"]}");

                            sw.WriteLine($"NewValue, {TestRecipe.ITU_Channel},{chitem["Itu_WL"]},{chitem["Itu_Freq"]}," +
                                $"{chitem["Gain[mA]"]},{chitem["SOA1[mA]"]},{chitem["SOA2[mA]"]}," +
                                $"{ this.RawDataMenu.MIRROR1_val},{ this.RawDataMenu.MIRROR2_val}," +
                                $"{ this.RawDataMenu.LP_val}," +
                                $"{this.RawDataMenu.PH1_Max_val},{this.RawDataMenu.PH2_Max_val}," +
                                $"{this.RawDataMenu.Wavelength}," +
                                $"{WaveLengthToFrequency(this.RawDataMenu.Wavelength)}," +
                                $"{chitem["Deviation[nm]"]},{chitem["SMSR[dB]"]},{chitem["Power[dBm]"]}," +
                                $"{chitem["MZM1[V]"]},{chitem["MZM2[V]"]},{chitem["MidlineIndex"]}");

                            sw.WriteLine();
                            sw.WriteLine($"MIRROR:");
                            sw.WriteLine($"current[mA],WL[nm]");
                            for (int i = 0; i < m1m2_wl_Array.Count; i++)
                            {
                                sw.WriteLine($"{m1m2_offsetArray[i]}, {m1m2_wl_Array[i] }");
                            }
                            sw.WriteLine();
                            sw.WriteLine($"LP:");
                            sw.WriteLine($"current[mA],WL_Deviation[nm]");
                            for (int i = 0; i < LPCurrentarray.Length; i++)
                            {
                                sw.WriteLine($"{lp_current_list[i] }, {diff_wl_array[i] }");
                            }
                            sw.WriteLine();
                            sw.WriteLine($"PH_Max:");
                            sw.WriteLine($"current[mA],POW[mW]");
                            for (int i = 0; i < phase_driving_Currents.Count; i++)
                            {
                                sw.WriteLine($"{phase_driving_Currents[i]}, {pd_reading_Currents[i]}");
                            }
                        
                        }
                        #endregion

                        if (token.IsCancellationRequested)
                        {
                            this.Log_Global($"用户取消 AlternativeQWLT");
                            return;
                        }

                        break;

                    }


                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消 AlternativeQWLT");
                        return;
                    }






                }

                //20241121 WHB增加图片存储功能
                string imagePath = Path.Combine(path, $@"..\AlternativeQWLT_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                imagePath = Path.GetFullPath(imagePath);
                ChartsSaver.SaveCharts(RawDataMenu, imagePath);
            }
            catch (Exception ex)
            {
                this.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                Merged_PXIe_4143.Reset();
                SwitchPD.TurnOn(false);
                this.Log_Global($"结束测试!");
            }
        }


        /// <summary>
        /// 检查用户是否取消测试
        /// </summary>
        /// <param name="token"></param>
        private void CheckCancellationRequested(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                Log_Global("用户取消测试");
                token.ThrowIfCancellationRequested();
                throw new OperationCanceledException();
            }
        }

        private double CreepGetPDCurrent_mA(Motor_LaserX_9078 Axis, int index)
        {
            var val = PD.ReadCurrent_A();

            val *= 1000;
            Log_Global($"Debug 光电流为[{val}]mA");

            return val;

        }

        private double CreepGetCurrent_mA(Motor_LaserX_9078 Axis, int index)
        {
            double current_PD_max = 0;
            List<double> lstcurrent = new List<double>();
            var sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(Axis, index); //当前挡位

            for (int i = 0; i < 10;)
            {
                //得到当前电流
                current_PD_max = Analog_LaserX_9078.GetCurrent_mA(Axis, index);



                if (current_PD_max > sense * 0.95)
                {
                    if (sense >= 1.8)
                    {
                        lstcurrent.Add(current_PD_max);
                        Log_Global($"已达最大电流档位");

                        break;
                    }

                    Analog_LaserX_9078.SetSenseCurrentRange_mA(Axis, index, sense * 2);
                    sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(Axis, index); //当前挡位
                }
                else
                {
                    i++;
                    lstcurrent.Add(current_PD_max);
                    Thread.Sleep(5);
                }

            }

            return lstcurrent.Average();
        }
        /// <summary>
        /// 在中心不变的情况下, 寻找一个能包住超限值的半径
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public double DataAnalyze_MinR(TrajResultItem result)
        {
            try
            {
                var ch = this.TestRecipe.Analog_CH - 1;
                List<double> pList = new List<double>();
                List<double> xList = new List<double>();
                List<double> yList = new List<double>();
                List<double> zList = new List<double>();

                foreach (var item in result.MotorPos_mm)
                {
                    if (item.Key.Name == "LNX")
                    {
                        xList = item.Value;
                    }
                    if (item.Key.Name == "LNY")
                    {
                        yList = item.Value;
                    }
                    if (item.Key.Name == "LNZ")
                    {
                        zList = item.Value;
                    }
                }

                //运动卡模拟量通道
                pList = result.Current_mA[ch];
                if (pList.Max() >= 2047)
                {
                    List<double> tpList = new List<double>();
                    List<double> txList = new List<double>();
                    List<double> tyList = new List<double>();
                    List<double> tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= 2047)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);
                        }
                    }

                    double x_range = txList.Max() - txList.Min();
                    double y_range = tyList.Max() - tyList.Min();
                    double z_range = tzList.Max() - tzList.Min();

                    if (x_range <= this.TestRecipe.Fine_Radius / 2 && y_range <= this.TestRecipe.Fine_Radius / 2 && z_range <= this.TestRecipe.Fine_Radius / 2)
                    {
                        double x_center = txList[0];
                        double y_center = tyList[0];
                        double z_center = tzList[0];

                        double x_Maxr = Math.Max(Math.Abs(txList.Max() - x_center), Math.Abs(txList.Min() - x_center));
                        double y_Maxr = Math.Max(Math.Abs(tyList.Max() - y_center), Math.Abs(tyList.Min() - y_center));
                        double z_Maxr = Math.Max(Math.Abs(tzList.Max() - z_center), Math.Abs(tzList.Min() - z_center));

                        double x_Minr = Math.Min(Math.Abs(txList.Max() - x_center), Math.Abs(txList.Min() - x_center));
                        double y_Minr = Math.Min(Math.Abs(tyList.Max() - y_center), Math.Abs(tyList.Min() - y_center));
                        double z_Minr = Math.Min(Math.Abs(tzList.Max() - z_center), Math.Abs(tzList.Min() - z_center));

                        //计算出三维空间半径
                        double Maxr = Math.Sqrt(Math.Pow(x_Maxr, 2) + Math.Pow(y_Maxr, 2) + Math.Pow(z_Maxr, 2));
                        double Minr = Math.Sqrt(Math.Pow(x_Minr, 2) + Math.Pow(y_Minr, 2) + Math.Pow(z_Minr, 2));

                        //最大三维半径 最大半径加一倍的光斑范围
                        return Maxr + (Maxr - Minr);
                    }
                    else
                    {
                        return this.TestRecipe.Fine_Radius;
                    }
                }

                return this.TestRecipe.Fine_Radius;
            }
            catch (Exception ex)
            {
                return this.TestRecipe.Fine_Radius;
            }
        }





        /// <summary>
        /// 写CSV文档
        /// </summary>
        /// <param name="LogDataMsg"></param>
        /// <param name="strb"></param>
        /// <param name="sw"></param>
        /// <param name="result"></param>
        private void WriteCSCVFile(string LogDataMsg, out StringBuilder strb, out StreamWriter sw, TrajResultItem result)
        {
            Log_Global($"原始数据:[{LogDataMsg}]");
            strb = PrintCSV(result);
            sw = new StreamWriter(LogDataMsg);
            sw.Write(strb.ToString());
            sw.Close(); strb.Clear();
        }
        public StringBuilder PrintCSV(TrajResultItem result)
        {
            StringBuilder sb = new StringBuilder();
            //try
            //{
            string str = "";
            {
                str = $"Id,";
                foreach (var item in result.MotorPos_mm)
                {
                    str += $"{item.Key.Name},";
                }
                foreach (var item in result.Voltage_mV)
                {
                    str += $"Ch{item.Key},";
                }
                foreach (var item in result.Current_mA)
                {
                    str += $"Ch{item.Key}_mA,";
                }
                sb.AppendLine(str);
            }

            int count = result.Id.Count;
            for (int j = 0; j < count; j++)
            {
                str = $"{result.Id[j]}_{result.DataIndex[j]},";
                foreach (var item in result.MotorPos_mm)
                {
                    str += $"{item.Value[j]},";
                }
                foreach (var item in result.Voltage_mV)
                {
                    str += $"{item.Value[j]},";
                }
                foreach (var item in result.Current_mA)
                {
                    str += $"{item.Value[j]},";
                }
                sb.AppendLine(str);
            }

            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}


            return sb;
        }
        /// <summary>
        /// 找到最大点位置
        /// </summary>
        /// <param name="lnx"></param>
        /// <param name="threeAxisList"></param>
        /// <param name="P1"></param>
        /// <param name="zeroScan"></param>
        /// <param name="maxList"></param>
        /// <param name="ch"></param>
        /// <param name="id"></param>
        /// <param name="retPoint"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private AxesPosition FindP1(List<Motor_LaserX_9078> threeAxisList,
                            Dictionary<int, PointResult> maxList,
                            int id,
                            Dictionary<AxesPosition, double> retPoint,
                            bool addlist)
        {
            //判断是否无光
            //this.JudgeThreshold_mW(retPoint);
            return MaxListAdd(threeAxisList, maxList, id, retPoint, addlist);
        }
        /// <summary>
        /// 与门限值判断
        /// </summary>
        /// <param name="retPoint"></param>
        private void JudgeThreshold_mW(Dictionary<AxesPosition, double> retPoint)
        {
            if (retPoint.First().Value <= TestRecipe.PowerThreshold_mA)
            {
                //while(true)
                //{
                //    Thread.Sleep(100);
                //}
                Log_Global($"{this.Name} 扫描范围内无光]");
                string str = $"{this.Name} 扫描范围内无光...";
                throw new Exception(str);
            }
        }

        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="eSize"></param>
        /// <param name="radius"></param>
        /// <param name="Position"></param>
        /// <param name="Plane"></param>
        /// <returns></returns>
        public TrajResultItem Run_Involute(eRunSize_Table eSize,
                                           AxesPosition Position,
                                           PmTrajSelectPlane Plane,
                                           TrajThresholdStop thresholdStop, //阈值停止
                                           CancellationToken token)
        {
            int rtn = 0;
            double RadiusSales = 1;       //比例
            double IntervalSales = 1;
            double Rough_R = 1;
            double Rough_Inv = 1;

            double Trajspeed = 1;

            //螺旋运动
            switch (eSize)
            {
                case eRunSize_Table.Fine_Double:   //精扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval* IntervalSales;
                        Trajspeed = TestRecipe.Fine_Trajspeed;
                    }
                    break;
            }

            return Run_Involute_Parameter(Rough_Inv, Rough_R, Rough_Inv, Trajspeed, Position, Plane, thresholdStop, token);
        }
        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="Rough_R">半径</param>
        /// <param name="Rough_Inv">间距</param>
        /// <param name="Trajspeed">速度</param>
        /// <param name="Position"></param>
        /// <param name="Plane"></param>
        /// <returns></returns>
        public TrajResultItem Run_Involute_Rough_ParameterR(eRunSize_Table eSize,
                                                            double Rough_R,
                                                            AxesPosition Position,
                                                            PmTrajSelectPlane Plane,
                                                            TrajThresholdStop thresholdStop, //阈值停止
                                                            CancellationToken token)
        {
            int rtn = 0;
            double RadiusSales = 1;       //比例
            double IntervalSales = 1;
            double Rough_Inv = 1;

            double Trajspeed = 1;

            //螺旋运动
            switch (eSize)
            {
                case eRunSize_Table.Rough:  //粗扫
                    {
                        RadiusSales = 2;
                        IntervalSales = 2;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Radius / 10;
                        Trajspeed = TestRecipe.Fine_Trajspeed;
                    }
                    break;
            }
            return Run_Involute_Parameter(Rough_Inv/2, Rough_R, Rough_Inv, Trajspeed, Position, Plane, thresholdStop, token);
        }
        /// <summary>
        /// 运行到指定点
        /// </summary>
        /// <param name="axisList"></param>
        /// <param name="targetPoint"></param>
        /// <param name="token"></param>
        public void MoveToAxesPosition(List<Motor_LaserX_9078> axisList, AxesPosition targetPoint, CancellationToken token)
        {
            foreach (var axis in axisList)
            {
                var pos = targetPoint.ItemCollection
                    .Where(kvp => kvp.AxisNo == axis.AxisNo.ToString())
                    .Select(kvp => kvp.Position)
                    .FirstOrDefault();
                axis.MoveToV3(pos, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Low);
            }
            foreach (var axis in axisList)
            {
                axis.WaitMotionDone();
            }
            Thread.Sleep(50);
        }
        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="Rough_R">半径</param>
        /// <param name="Rough_Inv">间距</param>
        /// <param name="Trajspeed">速度</param>
        /// <param name="Position"></param>
        /// <param name="Plane"></param>
        /// <returns></returns>
        public TrajResultItem Run_Involute_Parameter(
            double Rough_R_Inside,
            double Rough_R,
            double Rough_Inv,
            double Trajspeed,
            AxesPosition Position,
            PmTrajSelectPlane Plane,
            TrajThresholdStop thresholdStop, //阈值停止
            CancellationToken token)
        {
            //插补轴定义
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict = new Dictionary<PmTrajAxisType, MotorAxisBase>
            {
                { PmTrajAxisType.X_Dir, X2 },
                { PmTrajAxisType.Y_Dir, Y2 },
                { PmTrajAxisType.Z_Dir, Z2 }
            };

            Thread.Sleep(100);

            TrajResultItem result = new TrajResultItem();
            int rtn = 0;
            rtn = Parallel_2DCycleInvolute(axisDict,
                                           Position,
                                           Rough_R_Inside,
                                           Rough_R,
                                           Rough_Inv,
                                           Plane,
                                           true,
                                           Trajspeed,
                                           out result,
                                           thresholdStop,
                                           token);

            if (rtn != 0)
            {
                //异常返回;
            }

            return result;
        }
        /// <summary>
        /// 存储RawData并返回峰值点位
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool DataAnalyze(TrajResultItem result,
                                bool isRough,
                                out Dictionary<AxesPosition, double> AnalyzeResult)
        {
            const double Power_Threshold = 0.9;// 0.618;
            try
            {
                var ch = this.TestRecipe.Analog_CH - 1;

                List<double> pList = new List<double>();
                List<double> xList = new List<double>();
                List<double> yList = new List<double>();
                List<double> zList = new List<double>();

                List<double> tpList = new List<double>();
                List<double> txList = new List<double>();
                List<double> tyList = new List<double>();
                List<double> tzList = new List<double>();

                double tpSum = 0;   //求和
                double txSum = 0;   //加权求和
                double tySum = 0;
                double tzSum = 0;

                foreach (var item in result.MotorPos_mm)
                {
                    if (item.Key.Name == "LNX")
                    {
                        xList = item.Value;
                    }
                    if (item.Key.Name == "LNY")
                    {
                        yList = item.Value;
                    }
                    if (item.Key.Name == "LNZ")
                    {
                        zList = item.Value;
                    }
                }

                var actlnx = X2 as Motor_LaserX_9078;
                var actlny = Y2 as Motor_LaserX_9078;
                var actlnz = Z2 as Motor_LaserX_9078;
                var actList = new List<Motor_LaserX_9078>() { actlnx, actlny, actlnz };

                //运动卡模拟量通道, 取电流
                pList = result.Current_mA[ch];
                var sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, 0);
                if (pList.Max() >= sense * 0.95)
                {
                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    tpSum = 0;   //求和
                    txSum = 0;   //加权求和
                    tySum = 0;
                    tzSum = 0;

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= sense * 0.95)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);

                            tpSum += pList[i];
                            txSum += xList[i] * pList[i];
                            tySum += yList[i] * pList[i];
                            tzSum += zList[i] * pList[i];
                        }
                    }

                    double x_range = txList.Max() - txList.Min();
                    double y_range = tyList.Max() - tyList.Min();
                    double z_range = tzList.Max() - tzList.Min();

                    if (x_range <= this.TestRecipe.Fine_Radius * 1.5 && y_range <= this.TestRecipe.Fine_Radius * 1.5 && z_range <= this.TestRecipe.Fine_Radius * 1.5)
                    {
                        var tPmax = new AxesPosition();
                        foreach (var axisPos in t_Start_Pos)
                        {
                            var axis = actList.FirstOrDefault(item => item.AxisNo.ToString() == axisPos.AxisNo);
                            // 填入XYZ轴对应位置
                            tPmax.ItemCollection.Add(new AxisPosition()
                            {
                                Name = axis.Name,
                                CardNo = axis.CardNo.ToString(),
                                AxisNo = axis.AxisNo.ToString(),
                                Position = axis.Name == "LNX" ? txSum / tpSum :  //txList.Average() :
                                           axis.Name == "LNY" ? tySum / tpSum :  //tyList.Average() :
                                           axis.Name == "LNZ" ? tzSum / tpSum :  //tzList.Average() :
                                           axisPos.Position
                            });
                        }

                        Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                        //PD电流
                        tmaxPoint.Add(tPmax, sense);

                        AnalyzeResult = tmaxPoint;

                        //这里超过量程了, 需要跳挡
                        Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, sense * 2);

                        //这里到达最大量程
                        if (sense >= 1.8)
                        {
                            Log_Global($"已达最大电流档位，降低Gain电流10mA");

                            double tcurrent_mA = GAIN.ReadCurrent_A() * 1000.0;
                            GAIN.AssignmentMode_Current(tcurrent_mA - 10.0, 2.5);  //20240627 更换光开关后降低耦合电流
                        }

                        return true;
                    }
                    else
                    {
                        //20230224 面积中心做返回值
                        var pmax = pList.Max();
                        var pmin = pList.Min();

                        //使用黄金分割高度
                        var threshold_power = (pmax - pmin) * Power_Threshold + pmin;

                        tpList = new List<double>();
                        txList = new List<double>();
                        tyList = new List<double>();
                        tzList = new List<double>();

                        tpSum = 0;   //求和
                        txSum = 0;   //加权求和
                        tySum = 0;
                        tzSum = 0;

                        for (int i = 0; i < pList.Count; i++)
                        {
                            if (pList[i] >= threshold_power)
                            {
                                tpList.Add(pList[i]);
                                txList.Add(xList[i]);
                                tyList.Add(yList[i]);
                                tzList.Add(zList[i]);

                                tpSum += pList[i];
                                txSum += xList[i] * pList[i];
                                tySum += yList[i] * pList[i];
                                tzSum += zList[i] * pList[i];
                            }
                        }

                        var tPmax = new AxesPosition();
                        foreach (var axisPos in t_Start_Pos)
                        {
                            var axis = actList.FirstOrDefault(item => item.AxisNo.ToString() == axisPos.AxisNo);
                            // 填入XYZ轴对应位置
                            tPmax.ItemCollection.Add(new AxisPosition()
                            {
                                Name = axis.Name,
                                CardNo = axis.CardNo.ToString(),
                                AxisNo = axis.AxisNo.ToString(),
                                Position = axis.Name == "LNX" ? txSum / tpSum :  //txList.Average() :
                                           axis.Name == "LNY" ? tySum / tpSum :  //tyList.Average() :
                                           axis.Name == "LNZ" ? tzSum / tpSum :  //tzList.Average() :
                                           axisPos.Position
                            });
                        }

                        var maxIndex = GetMax(pList);

                        Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                        //PD电流
                        tmaxPoint.Add(tPmax, pList[maxIndex]);

                        AnalyzeResult = tmaxPoint;

                        //这里超过量程了, 需要跳挡
                        Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, sense * 2);


                        //这里到达最大量程
                        if (sense >= 1.8)
                        {
                            Log_Global($"已达最大电流档位，降低Gain电流10mA");

                            double tcurrent_mA = GAIN.ReadCurrent_A() * 1000.0;
                            GAIN.AssignmentMode_Current(tcurrent_mA - 10.0, 2.5);  //20240627 更换光开关后降低耦合电流
                        }

                        return false;
                    }
                }

                //20230224 面积中心做返回值
                {
                    var pmax = pList.Max();
                    var pmin = pList.Min();

                    //使用黄金分割高度
                    var threshold_power = (pmax - pmin) * Power_Threshold + pmin;

                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    tpSum = 0;   //求和
                    txSum = 0;   //加权求和
                    tySum = 0;
                    tzSum = 0;

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= threshold_power)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);

                            tpSum += pList[i];
                            txSum += xList[i] * pList[i];
                            tySum += yList[i] * pList[i];
                            tzSum += zList[i] * pList[i];
                        }
                    }

                    var tPmax = new AxesPosition();
                    foreach (var axisPos in t_Start_Pos)
                    {
                        var axis = actList.FirstOrDefault(item => item.AxisNo.ToString() == axisPos.AxisNo);
                        // 填入XYZ轴对应位置
                        tPmax.ItemCollection.Add(new AxisPosition()
                        {
                            Name = axis.Name,
                            CardNo = axis.CardNo.ToString(),
                            AxisNo = axis.AxisNo.ToString(),
                            Position = axis.Name == "LNX" ? txSum / tpSum :  //txList.Average() :
                                       axis.Name == "LNY" ? tySum / tpSum :  //tyList.Average() :
                                       axis.Name == "LNZ" ? tzSum / tpSum :  //tzList.Average() :
                                       axisPos.Position
                        });
                    }

                    var maxIndex = 0;
                    if (isRough)
                    {
                        maxIndex = GetMax_Rough(pList);
                    }
                    else
                    {
                        maxIndex = GetMax(pList);
                    }

                    Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                    //PD电流
                    tmaxPoint.Add(tPmax, pList[maxIndex]);

                    AnalyzeResult = tmaxPoint;

                    return true;
                }
            }
            catch (Exception ex)
            {
                AnalyzeResult = null;
                return false;
            }
        }
        public int GetMax_Rough(List<double> pList)
        {
            try
            {
                double[] countArr = new double[pList.Count];
                for (int i = 0; i < countArr.Length; i++)
                {
                    countArr[i] = i + 1;
                }
                double[] smoothArr = ArrayMath.CalculateSmoothedNthDerivate(countArr, pList.ToArray(), 1, 3, 7);
                Dictionary<int, double> maxDict = new Dictionary<int, double>();
                var top = pList.Select((value, index) => new { value, index })
                                    .OrderByDescending(item => item.value)
                                    .ThenByDescending(item => item.index)
                                    .Take(3)
                                    .ToArray();
                foreach (var item in top)
                {
                    maxDict.Add(item.index, item.value);
                }
                var halfHeight = (pList.Max() - pList.Min()) * 0.75 + pList.Min();
                Dictionary<int, double> finalDict = new Dictionary<int, double>();
                foreach (var item in maxDict)
                {
                    if (item.Key == 0)
                    {
                        if (pList[item.Key] - pList[item.Key + 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                    else if (item.Key == pList.Count - 1)
                    {
                        if (pList[item.Key] - pList[item.Key - 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        finalDict.Add(item.Key, item.Value);
                    }
                }

                if (finalDict.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return finalDict.Aggregate((m, n) => m.Value > n.Value ? m : n).Key;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// MaxList添加
        /// </summary>
        /// <param name="maxList"></param>
        /// <param name="id"></param>
        /// <param name="retPoint"></param>
        /// <returns></returns>
        private AxesPosition MaxListAdd(List<Motor_LaserX_9078> threeAxisList, Dictionary<int, PointResult> maxList, int id, Dictionary<AxesPosition, double> retPoint, bool addlist)
        {
            AxesPosition P1 = new AxesPosition();

            var maxPoint = new PointResult();
            maxPoint.ID = id;
            maxPoint.Position = new AxesPosition() { ItemCollection = retPoint.First().Key.ItemCollection };
            maxPoint.Power = retPoint.First().Value; //K_PD_mW;

            //拷贝出来坐标位置
            foreach (Motor_LaserX_9078 axis in threeAxisList)
            {
                var pos = Math.Round(retPoint.First().Key.ItemCollection.FirstOrDefault(item => item.AxisNo == axis.AxisNo.ToString()).Position, 6);

                P1.ItemCollection.Add(
                new AxisPosition()
                {
                    Name = axis.Name,
                    CardNo = axis.CardNo.ToString(),
                    AxisNo = axis.AxisNo.ToString(),
                    Position = pos
                });
            }

            if (addlist)
            {
                if (maxList.ContainsKey(id))
                {
                    maxList[id] = maxPoint;
                }
                else
                {
                    maxList.Add(id, maxPoint);
                }
            }
            //P1.ItemCollection = maxPoint2.Position.ItemCollection;
            return P1;
        }

        /// <summary>
        /// MaxList添加
        /// </summary>
        /// <param name="maxList"></param>
        /// <param name="id"></param>
        /// <param name="retPoint"></param>
        /// <returns></returns>
        private AxesPosition MaxListAdd(List<Motor_LaserX_9078> threeAxisList, Dictionary<int, PointResult> maxList, int id, AxesPosition retPoint, double power, bool addlist)
        {
            AxesPosition P1 = new AxesPosition();

            var maxPoint = new PointResult();
            maxPoint.ID = id;
            maxPoint.Position = new AxesPosition() { ItemCollection = retPoint.ItemCollection };
            maxPoint.Power = power; //K_PD_mW;

            //拷贝出来坐标位置
            foreach (Motor_LaserX_9078 axis in threeAxisList)
            {
                var pos = Math.Round(retPoint.ItemCollection.FirstOrDefault(item => item.AxisNo == axis.AxisNo.ToString()).Position, 6);

                P1.ItemCollection.Add(
                new AxisPosition()
                {
                    Name = axis.Name,
                    CardNo = axis.CardNo.ToString(),
                    AxisNo = axis.AxisNo.ToString(),
                    Position = pos
                });
            }

            if (addlist)
            {
                if (maxList.ContainsKey(id))
                {
                    maxList[id] = maxPoint;
                }
                else
                {
                    maxList.Add(id, maxPoint);
                }
            }
            //P1.ItemCollection = maxPoint2.Position.ItemCollection;
            return P1;
        }

        /// <summary>
        /// 求峰值index
        /// </summary>
        /// <param name="pList"></param>
        /// <returns></returns>
        public int GetMax(List<double> pList)
        {
            try
            {
                double[] countArr = new double[pList.Count];
                for (int i = 0; i < countArr.Length; i++)
                {
                    countArr[i] = i + 1;
                }
                double[] smoothArr = ArrayMath.CalculateSmoothedNthDerivate(countArr, pList.ToArray(), 1, 3, 7);
                Dictionary<int, double> maxDict = new Dictionary<int, double>();
                var top = pList.Select((value, index) => new { value, index })
                                    .OrderByDescending(item => item.value)
                                    .ThenByDescending(item => item.index)
                                    .Take(3)
                                    .ToArray();
                foreach (var item in top)
                {
                    maxDict.Add(item.index, item.value);
                }
                var halfHeight = (pList.Max() - pList.Min()) * 0.75 + pList.Min();
                Dictionary<int, double> finalDict = new Dictionary<int, double>();
                foreach (var item in maxDict)
                {
                    if (item.Key == 0)
                    {
                        if (pList[item.Key] - pList[item.Key + 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                    else if (item.Key == pList.Count - 1)
                    {
                        if (pList[item.Key] - pList[item.Key - 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        if (pList[item.Key] - pList[item.Key - 1] >= halfHeight ||
                        pList[item.Key] - pList[item.Key + 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                }

                if (finalDict.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return finalDict.Aggregate((m, n) => m.Value > n.Value ? m : n).Key;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //返回THz
        public static double WaveLengthToFrequency(double waveLength_nm)
        {
            double waveLength = waveLength_nm / 1e9;

            const double SpeedOfLight = 299792458.0; // 光速，单位是 m/s
            double frequency = SpeedOfLight / waveLength; // 单位是 1/m

            double frequency_THz = frequency / 1e12;
            return frequency_THz;
        }









    }
}