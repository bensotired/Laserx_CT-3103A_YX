using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    //0000 - 999  其他问题
    //1000 - 1999 马达
    //2000 - 2999 输入输出
    //3000 - 3999 视觉
    //4000 - 4999 硬件
    //5000 - 5999 测试



    public class ErrorCodes
    {
        #region 0 -999 自定义错误
        public const int NoError = 0;
        public const int ActionNotTaken = -1;
        public const int UserReqestStop = -2;
        public const int Unexecuted = -3;
        public const int NoStateActionAssign = -4;
        public const int OutOfBoundary = -5;
        public const int RunOutOfUnit = -6;
        public const int NoCurrentStateError = -7;
        public const int RunStateChainError = -8;
        public const int SaveExcelDataError = -9;
        public const int IndexGetCurrentPosError = -10;
        public const int IndexPreviousPosError = -11;
        public const int IndexNextPosError = -12;
        public const int StateActionFailed = -13;
        public const int AppConfigLoadedError = -14;
        public const int ActionParametersInCorrect = -15;
        public const int AssemblyNotFoundCreateObjectFailed = -16;
        public const int ClassNameNotFoundCreateObjectFailed = -17;
        public const int TrayDataNotFound = -18;
        public const int RowColInputNotCorrectForTrayIndex = -19;
        public const int TrayManagerConfigDataNotFound = -20;
        public const int FailToGetTrayPosition = -21;
        public const int OffsetManagerConfigDataNotFound = -22;
        public const int RelatedManagerNotFound = -23;
        public const int FailToIndexNext = -24;
        public const int FailToIndexPrevious = -25;
        public const int RelatedActionBaseNotFound = -26;
        public const int StageConfigNotFound = -27;
        public const int StationDataNotValid = -28;
        public const int NotActionData = -29;


        public const int TestEnteranceManagerConfigDataNotFound = -30;
        public const int TestProfileManagerConfigDataNotFound = -31;
        public const int PlatformUIActionFailed = -32;
        public const int PlatformUserFileLoadFailed = -33;
        public const int PlatformUserFileSaveFailed = -34;

        public const int CalPittch_Failed = -35;
        public const int Motion_RXoverRun_Failed = -36;
        #endregion

        #region 1000 - 1999 马达
        public const int NoMotorMovePosInfo = -1000;
        public const int MotionProfileIsNotFound = -1001;
        public const int MotorGoSafePosError = -1002;
        public const int MotorNotReachToPos = -1003;
        public const int MotorHomingError = -1004;
        public const int SameAxesForMotion = -1005;
        public const int MotorMoveError = -1006;
        public const int MotorMoveDoneWaitError = -1007;
        public const int MotorHomeDoneWaitError = -1008;
        public const int MotionManagerConfigLoadedError = -1009;
        public const int MotorNotFound = -1010;
        public const int MotionLimitActivated = -1011;
        public const int EStopTrigger = -1012;
        public const int MotorAlarmSignalActivated = -1013;
        public const int MotorMoveTimeOutError = -1014;
        public const int MotorIsProhibitedToMove = -1015;
        public const int SpeedFactorParametersSetTooHigh = -1016;
        public const int Sequence_MoveToAxesPositionFailed = -1017;
        public const int Parallel_MoveToAxesPositionFailed = -1018;
        
        #endregion

        #region 2000 - 2999 输入输出
        public const int IOConfigLoadedError = -2000;
        public const int NotTriggerIOPositionFlag = -2001;
        public const int NoSpinFrontModuleData = -2002;
        public const int NoSpinBackModuleData = -2003;
        public const int NoTECUpModuleData = -2004;
        public const int NoTECDownModuleData = -2005;
        public const int NoPinUpModuleData = -2006;
        public const int NoPinDownModuleData = -2007;
        public const int NoHolderRetreatData = -2008;
        public const int NoHolderPushData = -2009;
        public const int TrayHolderNotInProperPos = -2010;
        public const int InputIsNotDetect = -2011;
        public const int IOFunctionError = -2012;
        public const int OutPutIsNotFunctional = -2013;
        public const int SafeDoorIsOpen = -2014;//安全门处于打开状态
        public const int NoFixtureInPlace = -2015;//夹具未到位
        public const int CreateIOControllerFailed = -2016;
        public const int IOItemNotFoundError = -2017;
        public const int InPutNotTriggerProperly = -2018;
        public const int OutPutNotFound = -2019;
        public const int InPutNotFound = -2020;

        #endregion

        #region 3000 - 3999 视觉
        public const int VisionFailed = -3001;
        public const int MoveToCenterAfterVisionError = -3002;
        public const int OffsetActionDataNotExisited = -3003;
        public const int NotEnableFirstVision = -3004;
        public const int InspectionKitNotFound = -3005;
        public const int VisionManagerConfigDataNotFound = -3006;
        #endregion

        #region 4000 - 4999 硬件
        public const int SystemNotInitialized = -4000;
        public const int SerialPortConnectFailed = -4001;
        public const int InstrumentChassisNotFound = -4002;
        public const int CreateInstrumentFailed = -4003;
        public const int CreateInstrumentChassisFailed = -4004;
        public const int InstrumentChassisException = -4005;
        public const int InstrumentException = -4006;
        #endregion

        #region 5000 -5999 测试
        public const int TestPluginRuntimeUnexpectedError = -5000;

        public const int CreateTestModuleFailed = -5001;
        public const int CreateTestRecipeFailed = -5002;
        public const int CreateCalculatorFailed = -5003;
        public const int CreateCalculatorRecipeFailed = -5004;
        public const int CreateSpecificationFailed = -5005;
        public const int LoadTestExecutorConfigItemFailed = -5006;
        public const int LoadTestExecutorComboFailed = -5007;
        public const int LoadTestProfileFailed = -5008;
        public const int TestPluginRuntimeExpectedError = -5009;
        public const int JudgeDataError = -5010;
        public const int TestModuleRuntimeExceptionRaised = -5500;
        public const int UploadDataToDatabaseFailed = -5200;
        public const int TestPluginRuntimeKeyParamMonitorAbnormal = -5201;
        public const int TestPluginResetError = -5202;
        //计算参数往5100后面添加
        public const int Calc_LIV_Dse_ParamError = -5100;
        public const int Calc_LIV_Dseop_ParamError = -5101;
        public const int Calc_LIV_Dsex_ParamError = -5102;
        public const int Calc_LIV_Imax_ParamError = -5103;
        public const int Calc_LIV_Iop_ParamError = -5104;
        public const int Calc_LIV_Isat_ParamError = -5105;
        public const int Calc_LIV_Ith1_ParamError = -5106;
        public const int Calc_LIV_Ith2_ParamError = -5107;
        public const int Calc_LIV_Ith3_ParamError = -5108;
        public const int Calc_LIV_Kink_Current_ParamError = -5109;
        public const int Calc_LIV_Kink_OutRange_ParamError = -5110;
        public const int Calc_LIV_Kink_Percentage_ParamError = -5111;
        public const int Calc_LIV_Kink_Power_ParamError = -5112;
        public const int Calc_LIV_Kink1_ParamError = -5113;
        public const int Calc_LIV_Lierr_ParamError = -5114;
        public const int Calc_LIV_Lipo1_ParamError = -5115;
        public const int Calc_LIV_Lipo2_ParamError = -5116;
        public const int Calc_LIV_Pop_ParamError = -5117;
        public const int Calc_LIV_Pout_ParamError = -5118;
        public const int Calc_LIV_PowerCalibration_ParamError = -5119;
        public const int Calc_LIV_PowerMax_ParamError = -5120;
        public const int Calc_LIV_Pth2_ParamError = -5121;
        public const int Calc_LIV_Rd_ParamError = -5122;
        public const int Calc_LIV_Rs_ParamError = -5123;
        public const int Calc_LIV_Rs_2Point_ParamError = -5124;
        public const int Calc_LIV_Rs_I_ParamError = -5125;
        public const int Calc_LIV_Rs_V_ParamError = -5126;
        public const int Calc_LIV_SE_mWpermA_ParamError = -5127;
        public const int Calc_LIV_SE_mWpermW_ParamError = -5128;
        public const int Calc_LIV_SE_SEL_ParamError = -5129;
        public const int Calc_LIV_SE_ParamError = -5130;
        public const int Calc_LIV_SE2_ParamError = -5131;
        public const int Calc_LIV_SEf_ParamError = -5132;
        public const int Calc_LIV_SEmax_ParamError = -5133;
        public const int Calc_LIV_SEop_ParamError = -5134;
        public const int Calc_LIV_Vf_ParamError = -5135;
        public const int Calc_LIV_Vop_ParamError = -5136;
        public const int Calc_LIV_Vth2_ParamError = -5137;
        public const int Calc_LIV_Kink2_ParamError = -5138;
        public const int Calc_LIV_IR_ParamError = -5139;
        public const int Calc_LIV_Temperature_ParamError = -5140;
       // public const int Calc_LIV_DutyCycle_ParamError = -5140;
        
        //LIV测试参数
        public const int Calc_LIV_I_Start_mA_ParamError = -5141;
        public const int Calc_LIV_I_Stop_mA_ParamError = -5142;
        public const int Calc_LIV_I_Step_mA_ParamError = -5143;
        public const int Calc_LIV_PDComplianceCurrent_mA_ParamError = -5144;
        public const int Calc_LIV_Period_ms_ParamError = -5145;
        public const int Calc_LIV_pulseWidth_ms_ParamError = -5146;
        public const int Calc_LIV_SourceDelay_ms_ParamError = -5147;
        public const int Calc_LIV_SenseDelay_ms_ParamError = -5148;
        public const int Calc_LIV_NPLC_ms_ParamError = -5149;
        public const int Calc_LIV_PulsedMode_ParamError = -5150;
        public const int Calc_LIV_Factor_K_ParamError = -5149;
        public const int Calc_LIV_Factor_B_ParamError = -5150;
        public const int Calc_LIV_PrintLIVTestParams = -5151;
        public const int Calc_LIV_DutyCycle_ParamError = -5152;
        public const int Calc_LIV_Frequency_Hz_ParamError = -5153;
        public const int Calc_LIV_MaxPCE_ParamError = -5154;
        public const int Calc_LIV_SE_Ref_IOP_mWpermA_ParamError = -5155;
        public const int Calc_LIV_ResDiff_Ref_IOP_ParamError = -5156; 
        public const int Calc_LIV_PCE_Ref_IOP_ParamError = -5156;
        public const int Calc_LIV_Kink1_Percent_zh_ParamError = -5157;
        public const int Calc_LIV_Kink2_Percent_zh_ParamError = -5158;
        //光谱计算错误       
        public const int Calc_SPECT_PeakPower_ParamError = -5182;
        public const int Calc_SPECT_DrivingCurrent_mA_ParamError = -5183;
        public const int Calc_SPECT_PeakWavelength_mA_ParamError = -5184;
        public const int Calc_SPECT_SpectrumWidth_20db_ParamError = -5185;
        public const int Calc_SPECT_Temperature_ParamError = -5186;
        public const int Calc_SPECT_FWHM_ParamError = -5187;
        public const int Calc_SPECT_SMSR_ParamError = -5188;
        public const int Calc_SPECT_Wavelength_ParamError = -5189;
        public const int Calc_SPECT_SpectrumWidth_3db_ParamError = -5190;
        //FF
        public const int Calc_FF_AW_95Percent_ParamError = -5191;
        public const int Calc_FF_AW_e2_ParamError = -5192;
        public const int Calc_FF_AW_FWHM_ParamError = -5193;
        public const int Calc_FF_MaxPoint_ParamError = -5194;
        public const int Calc_FF_Centroid_ParamError = -5195;
       

        public const int Calc_FF_H_width_13p5_ParamError = -5192;
        public const int Calc_FF_H_width_50_ParamError = -5193;
        public const int Calc_FF_H_width_005_ParamError = -5194;
        public const int Calc_FF_V_width_13p5_ParamError = -5195;
        public const int Calc_FF_V_width_50_ParamError = -5196;
        public const int Calc_FF_V_width_005_ParamError = -5197;
        public const int Calc_FF_Temperature_ParamError = -5198;
        public const int Calc_FF_DrivingCurrent_mA_ParamError = -5199;
        public const int Calc_FF_MoveDistance_mm_ParamError = -5200;
        public const int Calc_FF_PrintFFTestParams = -5201;
        //PER
        public const int Calc_PER_Temperature_ParamError = -5201;
        public const int Calc_PER_NullA1_Deg_ParamError = -5202;
        public const int Calc_PER_PEA_ParamError = -5203;
        public const int Calc_PER_PEA1_Deg_ParamError = -5204;
        public const int Calc_PER_PER1_dB_ParamError = -5205;
        public const int Calc_PER_Power_NullA1_mW_ParamError = -5206;
        public const int Calc_PER_Power_PEA1_mW_ParamError = -5207;
        public const int Calc_PER_DrivingCurrent_mA_ParamError = -5208;
        public const int Calc_PER_Factor_K_ParamError = -5209;
        public const int Calc_PER_Factor_B_ParamError = -5210;


        //NanoTrakAlignment
        public const int Calc_NanoTrakAlignment_LX_AveragPower_mW_ParamError = -5230;
        public const int Calc_NanoTrakAverage_LX_AveragCurrent_mA_ParamError = -5231;
        public const int Calc_NanoTrakAlignment_RX_Power_mW_ParamError = -5232;
        public const int Calc_NanoTrakAverage_RX_Current_mA_ParamError = -5233;
        public const int Calc_NanoTrak_RX_Temperature_degC_ParamError = -5234;



        //NF
        public const int Calc_NF_NF_ParamError = -5245;
        public const int Calc_NF_Gain_ParamError = -5246;
        public const int Calc_NF_Temperature_ParamError = -5247;
        public const int Calc_NF_Testparameter_ParamError = -5248;
        public const int Calc_NF_TraceA_CenterWavelength_nm_ParamError = -5249;
        //TLX1
        public const int Calc_TLX1_Testparameter_ParamError = -5260;
        public const int Calc_TLX1_TLX1Power_ParamError = -5261;
        #endregion

        #region 6000 -6999 模块

        public const int Module_Coupling_Failed = -6001;
        public const int Module_DarkCurrent_Failed = -6002;
        public const int Module_FF_Failed = -6003;
        public const int Module_LIV_Failed = -6004;
        public const int Module_Motion_Failed = -6005;
        public const int Module_Response_Failed = -6006;
        public const int Module_TempControl_Failed = -6006;
        public const int Module_SPECT_Failed = -6007;
        public const int Module_PER_Failed = -6008;
        public const int Module_NanoTrakAlignment_LX_Failed = -6009;
        public const int Module_NanoTrakAlignment_RX_Failed = -6010;
        public const int Module_TLX1_Failed = -6011;
        public const int Module_OpticalSwitch_Failed = -6012;
        public const int Module_NF_Failed = -6007; 




        #endregion 6000 -6999 模块

        static Dictionary<int, string> ErrorMessageMap = new Dictionary<int, string>();

        public static void InitErrorMap()
        {
            Dictionary<int, string> m = ErrorMessageMap;
            m.Add(ActionNotTaken, "Action Not Taken | 行为任务尚未被执行");
            m.Add(UserReqestStop, "User Reqest Stop | 使用者停止机器");
            m.Add(NoStateActionAssign, "No State Action Assigned | 无行为任务指派");
            m.Add(Unexecuted, "Unexecuted | 未执行");
            m.Add(NoMotorMovePosInfo, "No Motor Move Pos Info | 无马达资讯");
            m.Add(SystemNotInitialized, "System Not Initialized Yet | 系统尚未初始化");
            m.Add(MotionProfileIsNotFound, "Motion Profile is not found | 马达Profile无物件");
            m.Add(MotorNotReachToPos, "Motor Not Reach to Command Pos | 马达没到达正确位置");
            m.Add(InputIsNotDetect, "Input is not detect | 输入无讯息");
            m.Add(MotorHomingError, "Homing Error | 复位失败");
            m.Add(SameAxesForMotion, "Same Axes for Motion | 无法同轴同时移动");
            m.Add(MotorMoveError, "Motor Move Error | 马达位移失败");
            m.Add(MotorMoveDoneWaitError, "Motor is not complete move | 马达未完成行程");
            m.Add(MotorHomeDoneWaitError, "Motor Homing is not successful! | 马达复位未完成");
            m.Add(OffsetActionDataNotExisited, "Offset Action Data Not Exisited | Offset Action Data 不存在");
            m.Add(NotTriggerIOPositionFlag, "Not Trigger IO Position Flag | 无触发触点停止");
            m.Add(NotEnableFirstVision, "Not Enable First Vision | 无启用 第1视觉");
            m.Add(IndexGetCurrentPosError, "Index To Get Current Pos Error | 索引当前位置发生错误");
            m.Add(IndexPreviousPosError, "Index Previous Pos Error | 索引前一个位置发生错误");
            m.Add(IndexNextPosError, "Index Next Pos Error | 索引后一个位置发生错误");
            m.Add(NoSpinFrontModuleData, "No Spin Front ModuleData | 没有 Spin Front Module Data 资料");
            m.Add(NoSpinBackModuleData, "No Spin Back ModuleData | 没有 Spin Back Module Data 资料");
            m.Add(NoTECUpModuleData, "No TEC Up ModuleData | 没有 TEC Up Module Data 资料");
            m.Add(NoTECDownModuleData, "No TEC Down ModuleData | 没有 TEC Down Module Data 资料");
            m.Add(NoPinUpModuleData, "No Pin Up ModuleData | 没有 Pin Up Module Data 资料");
            m.Add(NoPinDownModuleData, "No Pin Down ModuleData | 没有 Pin Down Module Data 资料");
            m.Add(NoHolderPushData, "No Holder Push ModuleData | 没有 Holder Push Module Data 资料");
            m.Add(NoHolderRetreatData, "No Holder Retreat ModuleData | 没有 Holder Retreat Module Data 资料");
            m.Add(IOConfigLoadedError, "IO Map Loaded Error | IO 输入输出 加载失败");
            m.Add(OutPutIsNotFunctional, "OutPut Is Not Functional | 输出无法正常操作");
            m.Add(SafeDoorIsOpen, "Safe Door Is Open | 安全门未关闭");
            m.Add(NoFixtureInPlace, "Fixture is Not In Place | 夹具未到位");
            m.Add(StateActionFailed, "State Action Failed | 行为任务执行失败");
            m.Add(AppConfigLoadedError, "App Config Loaded Error | 下载 APP Config失败");
            m.Add(MotionManagerConfigLoadedError, "Motion Manager Config Loaded Error | Motion Manager Config 下载失败");
            m.Add(MotorNotFound, "Motor Not Found | 没有马达物件");
            m.Add(CreateIOControllerFailed, "Craete IO Controller Failed | 创造 IO 控制器 失败");
            m.Add(ActionParametersInCorrect, "Action Parameters InCorrect | Action Type 参数不批配");
            m.Add(AssemblyNotFoundCreateObjectFailed, "Assembly Not Found Create Object Failed | DLL找不到，物件创造失败");
            m.Add(ClassNameNotFoundCreateObjectFailed, "Class Name Not Found Create Object Failed | Class找不到，物件创造失败");
            m.Add(IOItemNotFoundError, "IOItem Not Found Error | 无 IO 物件");
            m.Add(SerialPortConnectFailed, "Serial Port Connect Failed");
            m.Add(MotionLimitActivated, "Motion Limit Activated | 马达限位触发");
            m.Add(EStopTrigger, "EStop Trigger | Estop 触发");
            m.Add(MotorAlarmSignalActivated, "Motor Alarm Signal Activated | 马达警报触发");
            m.Add(InspectionKitNotFound, "Inspection Kit Not Found | 无视觉工具");
            m.Add(InPutNotTriggerProperly, "InPut Not Triggered Properly | 输入无正常触发");
            m.Add(OutPutNotFound, "OutPut Not Found | 无 OutPut 物件");
            m.Add(InPutNotFound, "InPut Not Found | 无 InPut 物件");
            m.Add(MotorMoveTimeOutError, "Motor Move Time Out Error | 马达移动超时");
            m.Add(MotorIsProhibitedToMove, "Motor Is Prohibited To Move | 马达禁止运行");
            m.Add(SpeedFactorParametersSetTooHigh, "Speed Factor Parameters Set Too High | 速度系数设定过高");
            m.Add(OffsetManagerConfigDataNotFound, "Offset Manager Config Data Not Found | 无 Offset Manager Config 资料");
            m.Add(TrayManagerConfigDataNotFound, "Tray Manager Config Data Not Found | 无 Tray Manager Config 资料");
            m.Add(VisionManagerConfigDataNotFound, "Vision Manager Config Data Not Found | 无 Vision Manager Config 资料");
            m.Add(TrayDataNotFound, "Tray Data Not Found | 无 Tray Data 物件");
            m.Add(RowColInputNotCorrectForTrayIndex, "Row Col Input Not Correct For Tray Index | Row Column 超出 index 范围");
            m.Add(FailToGetTrayPosition, "Fail To Get Tray Position | 无法计算Tray Position");
            m.Add(RelatedManagerNotFound, "Related Manager Not Found | 无相关 Manager 物件");
            m.Add(FailToIndexNext, "Fail To Index Next | 索引 下一个 失败");
            m.Add(FailToIndexPrevious, "Fail To Index Previous | 索引 上一个 失败");
            m.Add(RelatedActionBaseNotFound, "Related ActionBase Not Found | 无相关 ActionBase 物件");
            m.Add(StageConfigNotFound, "Stage Config Not Found | 无 Stage Config 物件");
            m.Add(StationDataNotValid, "Station Data Not Valid | 工作站资料不正确");

            m.Add(TestEnteranceManagerConfigDataNotFound, "TestEnterance Profile is not found | 测试入口无物件");
            m.Add(TestProfileManagerConfigDataNotFound, "TestProfile Profile is not found | 测试流程无物件");

            //// Process Error
            m.Add(OutOfBoundary, "Out Of Boundary | 超出界限");
            m.Add(RunOutOfUnit, "Run Out Of Unit | 无产品");
            m.Add(VisionFailed, "Vision Failed | 视觉失败");
            m.Add(IOFunctionError, "IO Function Error | IO输入输出 作业失败");
            m.Add(MotorGoSafePosError, "Motor Go Safe Pos Error | 马达回安全位置 失败");
            m.Add(NoCurrentStateError, "No Current State Error | 无行为物件");
            m.Add(RunStateChainError, "Run State Chain Error | 执行行为自动程序失败");
            m.Add(TrayHolderNotInProperPos, "Tray Holder Not In Proper Pos | Tray Holder 不在正确的位置上");



            //// Test Error
            //m.Add(TemperatureControlTestModuleError, "TempControlTest Error | 控温测试 失败");
            //m.Add(LineRTestModuleTestModuleError, "LineResistanceTest | 线电阻测试 失败");
            //m.Add(FarFieldTestModuleError, "FarFieldTest Error | 远场测试 失败");
            //m.Add(SaveExcelDataError, "SaveExcelData Error | 保存测试数据文件失败");
            //m.Add(NotExistTestModuleError, "NotExistTest Error | 不存在的测试项目");

            m.Add(Module_Coupling_Failed, "Coupling Failed | 耦合流程发生错误");
            m.Add(Module_DarkCurrent_Failed, "DarkCurrent Failed | 暗电流测试流程发生错误");
            m.Add(Module_FF_Failed, "FarField Failed | 远场发散角测试流程发生错误");
            m.Add(Module_LIV_Failed, "LIV Failed | LIV测试流程发生错误");
            m.Add(Module_SPECT_Failed, "SPECT Failed | 光谱测试流程发生错误");

        }

        public static string GetErrorDescription(int errorCode, string info1 = "",
                                                string info2 = "", string info3 = "")
        {
            string errorMsg = "Undefined";
            string fmsError = "";// FSMInnerErrorCode.GetErrorMessage(errorCode);
            if (fmsError != "")
                errorMsg = fmsError + " Code=" + errorCode.ToString();
            else if (ErrorMessageMap.ContainsKey(errorCode))
                errorMsg = ErrorMessageMap[errorCode];
            else
                errorMsg = errorMsg + " Code=" + errorCode.ToString();

            if (info1 != "")
                errorMsg += "<" + info1 + ">";
            if (info2 != "")
                errorMsg += "<" + info2 + ">";
            if (info3 != "")
                errorMsg += "\n<" + info3 + ">";
            return errorMsg;

        }
    }
}
