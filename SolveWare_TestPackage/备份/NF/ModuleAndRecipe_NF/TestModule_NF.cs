using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolveWare_TestPackage
{   
    [SupportedCalculator
    (
        "TestCalculator_NF_Gain",
        "TestCalculator_NF_NF",
       // "TestCalculator_NF_Temperature",
        "TestCalculator_PrintNFTestParams",
        "TestCalculator_NF_TraceA_CenterWavelength_nm"
    )]


    [ConfigurableInstrument("OpticalSwitch", "OpticalSwitch_F", "用于切换产品前面光路(1*4切换器)")]
    [ConfigurableInstrument("OpticalSwitch", "OpticalSwitch_B", "用于切换产品后面光路(1*4切换器)")]
    //[ConfigurableInstrument("TLX1", "TLX1", "用于可调激光器控制")]   //用那单独的模块
    [ConfigurableInstrument("IOSA", "OSA", "用于光谱扫描")]
    [ConfigurableInstrument("Keithley2602B", "SourceMeter_2602B", "用于加电")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "用于获取载台当前温度")]

    public class TestModule_NF : TestModuleBase 
    {
        public TestModule_NF() : base() { }
        #region 以get属性获取测试模块运行所需资源  
        Keithley2602B SourceMeter_Master { get { return (Keithley2602B)this.ModuleResource["SourceMeter_2602B"]; } }
        MeerstetterTECController_1089 TEC { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }
        //Thorlabs_TLX1 TLX1 { get { return (Thorlabs_TLX1)this.ModuleResource["TLX1"]; } }//用那单独的模块
        OpticalSwitch OpticalSwitch_F { get { return (OpticalSwitch)this.ModuleResource["OpticalSwitch_F"]; } }
        OpticalSwitch OpticalSwitch_B { get { return (OpticalSwitch)this.ModuleResource["OpticalSwitch_B"]; } } 
        IOSA OSA_Master { get { return (IOSA)this.ModuleResource["OSA"]; } }
        #endregion
        TestRecipe_NF TestRecipe { get; set; } 
        RawData_NF RawData { get; set; }
        double DrivingCurrent_mA { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_NF);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_NF();

            return RawData;
        }
 
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_NF>(testRecipe);
        }
        //public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        //{
        //    this.DrivingCurrent_mA = 0.0;
        //    bool Findok = dutStreamData.SummaryDataCollection.ItemCollection.Exists(t => t.Name == this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString());
        //    if (this.TestRecipe.UseRefData_DrivingCurrent_mA == true && Findok)
        //    {
        //        var sdata = dutStreamData.SummaryDataCollection.GetSingleItem(this.TestRecipe.RefData_Name_DrivingCurrent_mA.ToString());
        //        DrivingCurrent_mA = Convert.ToDouble(sdata.Value);
        //        DrivingCurrent_mA += this.TestRecipe.RefData_DrivingCurrent_Offset_mA;
        //    }
        //    else
        //    {
        //        DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
        //    }
        //}
        public override void Run(CancellationToken token)
        {
            try
            {
                ////3. 设置TLS光源波长    //最开始设置
                ////1. GAIN/NF测试之前先完成背向光及前向光的耦合
                /// 

                ////2. 光开关将TLS光源切换到光谱仪
                ////4. 读取光谱并记录到Trace A
                //OSA.DisplayTrace(OsaTrace.TRA, true);
                //OSA.GetOpticalSpectrumTrace(true, OsaTrace.TRA, 5000);
                //OSA.FixTrace(OsaTrace.TRA, true);
                ////5. 切换光开关将光源切到产品后端入射Focuser
                ///
                /// 
                /// 
                ////6. 切换光开关将产品前向出光切到光谱仪
                ///   
                /// 加电---
                /// 
                ////7. 读取光谱并记录到Trace B
                //OSA.DisplayTrace(OsaTrace.TRB, true);
                //OSA.GetOpticalSpectrumTrace(true, OsaTrace.TRB, 5000);
                ////8. 执行EDFA-NF分析，并读取分析结果
                //string nfResults = OSA.CalculateGainNF();
                ////< ch num >,< center wl >,< input lvl >,< output lvl >,< ase lvl >, < resoln >, < gain >, < nf >,...
                //double gain = Convert.ToDouble(nfResults.Split(',')[6]);
                //double nf = Convert.ToDouble(nfResults.Split(',')[7]);
                //OSA.FixTrace(OsaTrace.TRA, false);
                //OSA.DisplayTrace(OsaTrace.TRB, false);

                //if (TLX1.IsOnline == false|| TLX1 == null) //用那单独的模块
                //{
                //    Log_Global($"仪器[{TLX1.Name}]状态为[{TLX1.IsOnline}]");
                //    return;
                //}
                if (OpticalSwitch_F.IsOnline == false || OpticalSwitch_F == null)
                {
                    Log_Global($"仪器[{OpticalSwitch_F.Name}]状态为[{OpticalSwitch_F.IsOnline}]");
                    return;
                }
                if (OpticalSwitch_B.IsOnline == false || OpticalSwitch_B == null)
                {
                    Log_Global($"仪器[{OpticalSwitch_B.Name}]状态为[{OpticalSwitch_B.IsOnline}]");
                    return;
                }
                if (OSA_Master.IsOnline == false || OSA_Master == null)
                {
                    Log_Global($"仪器[{OSA_Master.Name}]状态为[{OSA_Master.IsOnline}]");
                    return;
                }
                if (/*TEC.IsOnline == false ||*/ TEC == null)
                {
                    Log_Global($"仪器[{TEC.Name}]状态为[{TEC.IsOnline}]");
                    return;
                }
                this.Log_Global($"TestModule_NF 开始测试!");
                //if (TLX1.IsOnline) //用那单独的模块
                //{
                //    TLX1.Reset();
                //}
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Reset();                   
                }

                OSA_Master.CalibrationAutoZeroEnable = false;//20190321 新增 避免ZERO导致产品无数据
                OSA_Master.ResolutionBandwidth_nm = this.TestRecipe.OsaRbw_nm;
                OSA_Master.CenterWavelength_nm = this.TestRecipe.CenterWavelength_nm;
                OSA_Master.WavelengthSpan_nm = this.TestRecipe.WavelengthSpan_nm;
                //OSA_Master.TraceLength = this.TestRecipe.OsaTraceLength;
                OSA_Master.TraceLength_string = this.TestRecipe.OsaTraceLength_string;
                //OSA_Master.Sensitivity = YokogawaAQ6370SensitivityModes.Mid;
                OSA_Master.Sensitivity = this.TestRecipe.SensitivityModes;
                OSA_Master.SmsrModeDiff = this.TestRecipe.SmsrModeDiff_dB;
                OSA_Master.SMSRMask_nm = this.TestRecipe.SMSRMask_nm;

                //if (DrivingCurrent_mA == 0)
                {
                    DrivingCurrent_mA = this.TestRecipe.Default_DrivingCurrent_mA;
                }



                ////2. 光开关将TLS光源切换到光谱仪            
                OpticalSwitch_B.SetCH((byte)this.TestRecipe.TraceA_OpticalB_TLX1_ChannelNO);

                OpticalSwitch_F.SetCH((byte)this.TestRecipe.TraceA_OpticalF_SPECT_ChannelNO);
                ////3. 设置TLS光源波长
                //if (this.TestRecipe.Use_dBm0rmW) //用那单独的模块
                //{
                //    TLX1.SsetVOAOutModePower_dBm(this.TestRecipe.VOAAttenModePower_dBm);
                //}
                //else
                //{
                //    TLX1.SetVOAOutModePower_mW(this.TestRecipe.VOAAttenModePower_mW);
                //}

                ////4. 读取光谱并记录到Trace A
                (OSA_Master as OSA_AQ67370).DisplayTrace(OsaTrace.TRA, true);
                PowerWavelengthTrace trace_A = OSA_Master.GetOpticalSpectrumTrace(true, OsaTrace.TRA);
                (OSA_Master as OSA_AQ67370).FixTrace(OsaTrace.TRA, true);

                double TraceA_CenterWavelength_nm = OSA_Master.ReadCenterWL_nm();//Math.Round(OSA_Master.ReadCenterWL_nm(),2);
                RawData.TraceA_CenterWavelength_nm = TraceA_CenterWavelength_nm;

               // PowerWavelengthTrace trace_A = OSA_Master.GetOpticalSpectrumTrace(true, OsaTrace.TRA);
                //for (int i = 0; i < trace_A.Count; i++)
                //{
                //    RawData.Add(new RawDatumItem_NF()
                //    {
                //        TRA_Power = trace_A[i].Power_dBm,
                //        TRA_Wavelength_nm = trace_A[i].Wavelength_nm,
                //    });
                //}

                ////5. 切换光开关将光源切到产品后端入射Focuser              
                OpticalSwitch_B.SetCH((byte)this.TestRecipe.TraceB_OpticalB_TLX1_ChannelNO);              
                ////6. 切换光开关将产品前向出光切到光谱仪
                OpticalSwitch_F.SetCH((byte)this.TestRecipe.TraceB_OpticalF_SPECT_ChannelNO);

                //加电前等待
               // if (this.TestRecipe.BiasBeforeWait_ms > 0)
                {
                    this.Log_Global($"加电前等待 {this.TestRecipe.BiasBeforeWait_ms}ms.");
                    Thread.Sleep(this.TestRecipe.BiasBeforeWait_ms);
                }
                //这里要加电了
                SourceMeter_Master.SetupSMU_LD(DrivingCurrent_mA, this.TestRecipe.complianceVoltage_V); //默认2.5V

                //加电后等待至少500ms
                if (this.TestRecipe.BiasAfterWait_ms > 0)
                {
                    int waittime = 500;
                    waittime = Math.Max(waittime, this.TestRecipe.BiasAfterWait_ms);
                    this.Log_Global($"加电后等待 {waittime}ms.");
                    Thread.Sleep(waittime);
                }
                else
                {
                    Thread.Sleep(500);
                }

                //7. 读取光谱并记录到Trace B
                (OSA_Master as OSA_AQ67370).DisplayTrace(OsaTrace.TRB, true);
                PowerWavelengthTrace trace_B = OSA_Master.GetOpticalSpectrumTrace(true, OsaTrace.TRB);

               // PowerWavelengthTrace trace_B = OSA_Master.GetOpticalSpectrumTrace(true, OsaTrace.TRB);
                //if (trace_A.Count == trace_B.Count)
                //{
                //    for (int i = 0; i < trace_B.Count; i++)
                //    {
                //        RawData.Add(new RawDatumItem_NF()
                //        {
                //            TRA_Wavelength_nm = trace_A[i].Wavelength_nm,
                //            TRA_Power = trace_A[i].Power_dBm,
                //            TRB_Power = trace_B[i].Power_dBm,

                //            //TRB_Power = trace_B[i].Power_dBm,
                //            //TRB_Wavelength_nm = trace_B[i].Wavelength_nm,

                //        });
                //    }
                //}

                //8. 执行EDFA-NF分析，并读取分析结果
                string nfResults = (OSA_Master as OSA_AQ67370).CalculateGainNF(
                                                          this.TestRecipe.AALGo,
                                                          this.TestRecipe.FALGo,
                                                          this.TestRecipe.FARea ,
                                                          this.TestRecipe.IOFFset,
                                                          this.TestRecipe.IRANge,
                                                          this.TestRecipe.MARea,
                                                          this.TestRecipe.MDIFF ,
                                                          this.TestRecipe.OOFFset,
                                                          this.TestRecipe.PDISplay,
                                                          this.TestRecipe.TH ,
                                                          this.TestRecipe.RBWidth,
                                                          this.TestRecipe.SNOISE,
                                                          this.TestRecipe.SPOWer);

                //数据处理
                List<double> nfResults_list = new List<double>();
                double DataCount = 0;
                nfResults_list = this.ConvertStringArrayTodoubleList(nfResults.Split(','), out DataCount);

                //for (int i = 0; i < DataCount-1; i++)
                //{
                //    RawData.Add(new RawDatumItem_NF()
                //    {
                //        Wavelength_nm = nfResults_list[0 + i * 7] * 1e9,
                //        Input_Lvl_dBm = nfResults_list[1 + i * 7],
                //        Oupt_Lvl_dBm = nfResults_list[2 + i * 7],
                //        Ase_Lvl_dBm = nfResults_list[3 + i * 7],
                //        Resoln_nm = nfResults_list[4 + i * 7],
                //        Gain_dB = nfResults_list[5 + i * 7],
                //        NF_dB = nfResults_list[6 + i * 7],
                //    });
                //}
                //处理数据

                if (trace_A.Count == trace_B.Count)
                {
                    for (int i = 0; i < trace_B.Count; i++)
                    {
                        if (DataCount > i) //不知道他们两个实际会不会相等，要测试
                        {
                            RawData.Add(new RawDatumItem_NF()
                            {
                                TRA_Wavelength_nm = trace_A[i].Wavelength_nm,
                                TRA_Power = trace_A[i].Power_dBm,
                                TRB_Power = trace_B[i].Power_dBm,

                                //EmptyList = ",",

                                Wavelength_nm = nfResults_list[0 + i * 7] * 1e9,
                                Input_Lvl_dBm = nfResults_list[1 + i * 7],
                                Oupt_Lvl_dBm = nfResults_list[2 + i * 7],
                                Ase_Lvl_dBm = nfResults_list[3 + i * 7],
                                Resoln_nm = nfResults_list[4 + i * 7],
                                Gain_dB = nfResults_list[5 + i * 7],
                                NF_dB = nfResults_list[6 + i * 7],
                            });
                        }
                        else
                        {
                            RawData.Add(new RawDatumItem_NF()
                            {
                                TRA_Wavelength_nm = trace_A[i].Wavelength_nm,
                                TRA_Power = trace_A[i].Power_dBm,
                                TRB_Power = trace_B[i].Power_dBm,

                                //EmptyList = ",",

                                Wavelength_nm = 0,
                                Input_Lvl_dBm = 0,
                                Oupt_Lvl_dBm = 0,
                                Ase_Lvl_dBm = 0,
                                Resoln_nm = 0,
                                Gain_dB = 0,
                                NF_dB = 0,
                            });
                        }
                    }
                }

                //< ch num >,< center wl >,< input lvl >,< output lvl >,< ase lvl >, < resoln >, < gain >, < nf >,...
                //RawData.Gain = Convert.ToDouble(nfResults.Split(',')[6]);
                //RawData.NF = Convert.ToDouble(nfResults.Split(',')[7]);
                //RawData.DrivingCurrent_mA = DrivingCurrent_mA;

                (OSA_Master as OSA_AQ67370).FixTrace(OsaTrace.TRA, false);
                (OSA_Master as OSA_AQ67370).DisplayTrace(OsaTrace.TRB, false);


                //做完后必须要reset 带电操作会不好
                //if (TLX1.IsOnline) //用那单独的模块
                //{
                //    TLX1.Reset();                  
                //}
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Reset();
                }
                //获取温度
                RawData.NF_Temperature_degC = TEC.CurrentObjectTemperature;
                this.Log_Global("TestModule_NF 模块运行完成");
            }
            catch (Exception ex)
            {
                //if (TLX1.IsOnline) //用那单独的模块
                //{
                //    TLX1.Reset();
                //}
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Reset();
                }
                this._core.ReportException("光谱NF模块运行错误", ErrorCodes.Module_NF_Failed, ex);
            }
        }
        private List<double> ConvertStringArrayTodoubleList(string[] strArr,out double datacount) 
        {
            List<double> values = new List<double>();
            datacount = 0;
            try
            {               
                double _strvaluses = 0;
                if (double.TryParse(strArr[0], out _strvaluses))
                {
                    datacount=_strvaluses;
                }
                for (int i = 1; i < strArr.Length; i++)//从第一个开始，因为第一个是总数
                {
                    if (double.TryParse(strArr[i], out _strvaluses))
                    {
                        values.Add(_strvaluses);
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                return values;
                // throw;
            }
           
        }
    }
}
