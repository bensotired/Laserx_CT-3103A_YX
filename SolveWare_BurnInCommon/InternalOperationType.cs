using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public enum InternalOperationType
    {
        UserRequest_SuspendSelectedCarrierBI,//老化暂停
        UserRequest_ContinueSelectedCarrierBI,//老化继续

        UserRequest_ExitApplication,
        UserRequest_LoadJob,
        UserRequest_StartJobTask,
        RefreshRackUnitStatus,
        /// <summary>
        /// for pump module burn-in,there are 4 unit in one chassis
        /// </summary>
        UserRequest_GetSelectedChassisDetails,
        UserRequest_GetSelectedCarrierDetails,
        UserRequest_PauseResumeSelectedCarrierBI,

        UserRequest_StartSelectedCarrierBI,
        UserRequest_LaunchSelectedUnitBI,
        UserRequest_StartSelectedUnitBI,
        UserRequest_CancelSelectedUnitBI,
        UserRequest_QuickStartSelectedCarrierBI, //快速启动
        UserRequest_StartAllCarrierBI,
        UserRequest_ResetSelectedCarrierBI,
        UserRequest_CloseAlarmOnce,        //当抽屉中产品有坏的情况下, 关闭一次报警
        UserRequest_ConfirmSelectedCarrierBIPlan,
        UserRequest_ReloadDataFile,
        UserRequest_ReloadJob,
        UserRequest_BigChassisStartCarrierBI,

        UserRequest_CalibCheck_Start,  //请求进入开始校准的检查模式
        UserRequest_Calib_Stop,         //请求进入开始校准的检查模式
        UserRequest_Calib_Reset,         //请求进入开始校准的检查模式
        CoreRequest_UpdateMonitorStatusInfo,
        CoreRequest_CreateMonitorStatusInfo,
        CoreRequest_UpdateTestPluginStatusInfo,
        CoreRequest_CreateTestPluginStatusInfo,
        CoreRequest_GUIRunUIInvokeAction,
        CoreRequest_GUIRunUIInvokeActionSYNC,
        CoreRequest_GUIRunUIInvokeFunc,
        //CoreRequest_GUIRunEngModeInvokeAction,
        CoreRequest_GUITestPluginMainPageDockingInvokeAction,
        CoreRequest_GUIMulti_TestPluginMainPageDockingInvokeAction,
        CoreRequest_GUIMessageBoardDockingInvokeAction,
        //CoreRequest_GUINanoTrakBoardDockingInvokeAction,
        CoreRequest_GUIStationBoardDockingInvokeAction,
        CoreRequest_GUITestFrameBoardDockingInvokeAction,
        CoreRequest_GUIChartBoardDockingInvokeAction,
        CoreRequest_GUIRackPanelDockingInvokeAction,
        CoreRequest_GUIDataSnapBoardDockingInvokeAction,
        Calibration_SingleSMU,              //未实现
        TestStation_Initialized,

        GeneralManager_Initialized,
        GeneralManager_EmergencyStopFlagChanged,

        RefreshInterationDataSource,
        RefreshUnitActiveStatus,
        RefreshUnitDataOverview,        //core -> gui 刷新当前选择的work的数据
        RefreshUnitDataOnce,
        RefreshCalibrationUnitData,     //core -> gui 刷新校准unit的数据

        ExceptionAlert,
        ExceptionCountChanged,
        Engineering_ConnectToChannel,
        Engineering_EnableInPostionBoardOutput,
        Engineering_DisableInPostionBoardOutput,
        Engineering_RefreshTestBoardOutputs,
        Engineering_EnableSmuBoardOutput,
        Engineering_DisableSmuBoardOutput,
        Engineering_EnableSmuBoardTCtrl,
        Engineering_DisableSmuBoardTCtrl,
        Engineering_RefreshSmuBoardOutputs,
        Engineering_SetUnitInternalTemperature,
        Engineering_SetUnitExternalTemperature,
        Engineering_EnableUnitInternalTemperatureControl,
        Engineering_EnableUnitExternalTemperatureControl,
        Engineering_EnableUnitDriving,
        BurnInPlanFilseChanged,
        BurnInJobFilseChanged,
        Debug_StartAllCarrierBI,
        UserRequest_ConvertBiDataFromXmlToExcel,

        //工程窗口用
        Engineering_SetSingleChannelOP,         //设定当前选择的某个通道
        Engineering_EnableAllChannelOP,         //打开当前选择的所有通道
        Engineering_DisableAllChannelOP,        //关闭当前选择的所有通道
        Engineering_DisableUnitChannelTctrl,    //关闭所有控温
        Engineering_EnableUnitChannelTctrl,     //打开所有控温
        Engineering_DisableSingleChannelTctrl,    //关闭当前选择的某个通道控温
        Engineering_EnableSingleChannelTctrl,     //打开当前选择的某个通道控温

        Engineering_SetIndicatorStatus,
        Engineering_SetCylinderStatus,
        Engineering_SetPanelLockStatus,
        Engineering_EnableUnitPanel,            //面板是否允许操作

        //校准用指令
        EngCalibration_SetSingleChannelOP,      //校准的工程操作
        EngCalibration_EnableAllChannelOP,
        EngCalibration_DisableAllChannelOP,
        EngCalibration_DisableUnitChannelTctrl,
        EngCalibration_EnableUnitChannelTctrl,
        EngCalibration_SetCylinderStatus,
        EngCalibration_SetIndicatorStatus,
        EngCalibration_SetPanelLockStatus,
        EngCalibration_EnableUnitPanel,            //面板是否允许操作

        TestStation_RackLeakageAlarmChanged,    //机架漏水报警
        TestStation_UnitLeakageAlarmChanged,    //抽屉漏水报警
        TestStation_UnitFanStopAlarmChanged,    //抽屉漏水报警
        TestStation_FanStopAlarmChanged,         //机架风扇停转报警
        TestStation_WaterUnstableAlarmChanged,   //水流量
        TestStation_WaterTemperatureOutOfRange,   //水流量
        TestStation_AirPressureOutOfRange,   //水流量
        //TestStation_WaterUnstableAlarm,
        TestStation_WaterValveChanged,          //水阀状态
        TestStation_AirPressureChanged,          //2022C 更新压缩空气传感器
        TestStation_AirPressureValueUpdate,          //2022C 更新压缩空气传感器
        TestStation_WaterTempValueUpdate,          //2022C 更新冷却水温度
        TestStation_RefreshRackInfo,
        TestStation_RackInfoStatusStripVisibleChanged,

        TestStation_InstrumentLog,
        ProductTypeFilseChanged,
        MESInteration_UpdateLauncherInfo,
        MESInteration_ImportOriginalWID,
        MESInteration_EnableChanged,
        MESInteration_UpdateWIDStatus,
        f1,
        f2,
        f3,
        f4,
        f5,
        f6,
        f7,
        f8,
        f9,
        f10,
        f11,
        f12,
        f13,
        f14,
        User_RequestShowAppPluginUI,
  
        User_GuiSelectedChassis,
        User_GuiSelectedUnit,
        User_GuiSelectedChassisAndUnit,     //用户在界面上选择了一个抽屉
        User_GuiSelectedCalibrationChassisAndUnit,     //20210223 用户在界面上选择了一个抽屉用于校准

        UserRequest_UpdateCharts,
        Engineering_SetUnitLDCurrent,
        Engineering_SetLDProtVolt,
        Engineering_SetTempLimit,
        Engineering_SetUnitTempControl,
        Engineering_EnablePanelButton,
        Engineering_FanPower,
        Engineering_Indicator,
        Engineering_Nitrogen,
        Engineering_SetUnitEAVoltage,
        Engineering_SetTempMonitorCoeff,
        HardParameter_TEC, //TEC 参数设定
        Engineering_ControlWarterValve,   //机架冷水阀门
        TestCoreInitialized,
        Engineering_SetSourceModeRangeProtection,

        Engineering_Laserx_CA6A_VoltageLimit, //20210219 镭神6A电源的电压上限设定(实际是设置明纬电源)
        Engineering_Laserx_CA6A_SMUProtect,   //20210221 镭神6A电源的开路和短路保护使能

        Engineering_Laserx_CA6A_GetVoltageLimit, //20210913 镭神6A电源的电压回读值(实际是明纬电源)

        Engineering_ReLoadLightPDCalibrationConfig,   //20210618 光PD校准系数重载


        HPL_BACKDOOR,

        DoCalibration,
        SwitchTestBoardChannel,
        Test,
        UserRequest_TurnOnlineChassis,
        UserRequest_TurnOfflineChassis,
        UserRequest_TurnOnSimnulation,
        //UserRequest_ForceTurnOnlineChassis,
        UserRequest_ForceTurnOfflineChassis, 
        UserRequest_TurnOnlineUnit,
        UserRequest_TurnOfflineUnit,
        UserRequest_PopupMessageBoard,
        UserRequest_PopupChartBoard,
        UserRequest_DockMessageBoard,
        UserRequest_DockChartBoard,
        UserRequest_PopupDataSnapBoard,
        UserRequest_DockDataSnapBoard,
        UserRequest_LoginStatusChanged,

        Layout_CreateOrDockTesterAppTabPage,
        Layout_HideTesterAppTabPage,

        NotifyUI_ProductConfigChanged,
    }
}