using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{
    public interface ITesterCoreInteration : ITesterCoreHandleUIAction, ITesterAssembly, ITesterDataBaseOperation
    {
        event SendMessageOutEventHandler SendOutFormCoreToGUIEvent;
        event SendMessageOutEventHandler SendOutFormCoreEvent;
        event SendMessageOutEventHandlerWithReturn SendOutFormCoreToGUIEventWithReturn;
        TestStationConfig StationConfig { get; }
        Dictionary<string, string> GetStationInstrumentSimpleInfos();
     
        object GetStationHardwareObject(string stationHardwareName);
        object GetAuxiliaryHardwareObject(string auxiHardwareName);
        Action<IMessage> SendToCore { get; set; }
        AccessPermissionLevel CurrentAPL { get; }
        string CurrentUserInfo { get; }
        string CurrentUserID { get; }
        void ShowLoginUI();
        void ShowUserManageUI();
        void RefreshLoginInfo();
        bool CanUserAccessDomain(AccessPermissionLevel requestApl);
        bool CanUserAccessResourceProviderUI(string appName);
        string PlatformVersionInfomation { get; }
        void LinkToCore(ITesterCoreLink coreLinkObjcet);
        void UnlinkFromCore(ITesterCoreLink coreLinkObjcet);
        bool TryAllocateResourceToPlugin(string name);
        //bool TryAllocateResourceToPlugin();
        bool TryAllocateResourceToPlatform();
        List<Type> GetAssignableTypesFromPreLoadDlls(Type baseType);
        Type GetTypeFromClassInPreLoadDlls(string className);
        List<Type> GetTypeFromClassInPreLoadDlls(List<string> className);
        bool PreLoadDllsContainsClass(string className);
        string[] GetTestPlugInKeys();
        ITesterAppPluginInteration GetTestPlugin(string apKey);
        object LoadTestRecipeInstance(string fileNameWithoutExtension);
        object LoadCalcRecipeInstance(string fileNameWithoutExtension);
       // object LoadInstanceFromFile_ByXmlRoot(string directoryPath, string fileNameWithoutExtension);
        Dictionary<string, string> GetLocalExecutorComboFiles();
        Dictionary<string, string> GetLocalTestProfileFiles();
        object LoadTestExecutorCombo(string configXmlFile);
        object LoadTestExecutorComboWithParams(string configXmlFile);
        object LoadTestProfile<TProfile>(string configXmlFile);
        object LoadTestProfileWithExtraTypes<TProfile>(string configXmlFile);
        GenernalResourceOwner ResourceOwner { get; }
        string ResourceOwnerName { get; }
        void RunTestExecutorDebugger(object eciObj);
        void RunTestExecutorDebugger(object eciObj , object tstParamObj);
        void RunTestExecutorCombo_ParamsEditor(object testExecutorComboWithParams);
        List<InstrMonitor> InstrumentsMonitors { get; }

        string CurrentProductName { get; }
        string CreateProductName { get; }
        bool CreateProductConfig(string prodName);
        bool SwitchProductConfig(string prodName);
        string Get_Create_ProductConfigFileFullPath(string constConfigFileName);
        string GetProductConfigFileFullPath(string constConfigFileName);
        string GetProductConfigFileDirectory();
        string[] GetProductConfigFolderNames();

        bool TryDisConnectAllInstruments();
        bool TryConnectAllInstruments();

        bool TryReleaseResourceBeforeClosing();
        string Get_Create_ProductConfigFileDirectory();
        void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true);
    }
}