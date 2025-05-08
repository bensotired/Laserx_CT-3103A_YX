using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{

    public interface ICoreInteration : ICoreRuntime//, ILogHandle, IExceptionHandle
    {
        bool IsMES_Enable { set; get; } 
        bool IsNeedAlertToExit();
        List<PermissionItem> UserList { get; }
        void SaveOperatorFile(List<PermissionItem> permissions);
        bool TryLogin(AccessPermissionLevel apl, string userId, string password);
        bool TryLogout();
        bool CanUserAccessDomain(AccessPermissionLevel requestApl);
        string SelectedUnitFullName { get; }
        //StationConfig StationConfig { get; }
        ICollection<string> ExecutableUnitNames { get; }
        //IBurnInPluginFactory PluginFactory { get; }
        //ITestStationManager TestStationManager { get; }
        Action<IMessage> SendToCore { get; set; }
        event SendMessageOutEventHandler SendOutFormCoreEvent;
        void GUIRunPopupUIInvokeAction(Action guiInvokeAction);
        //void GUIRunEngModeInvokeAction(object emGuiInvokeAction);
        bool CheckCombo(string comboName, string unitType, out string tips);
        bool CheckPlan(string planName, string destPlanType, out string tips);
        object CreateKnownStepComboObject(string comboName);
        object CreateKnownPlanObject(string planName);
        WorkerImportData CreateEmptyWorkerImportData(string unitName);
        bool UpdateWorkerImportData(WorkerImportData sourceWid, string planName, string comboName, UnitSlotsInfo unitSlots);
        bool ImportBI(WorkerImportData wid);
        bool ReImportBI(string unitName, string streamDataFile);
        bool StartBI(string unitName);
        bool StopBI(string unitName, string userId, AccessPermissionLevel apl);
        bool ResetUnit(string unitName);
        object GetEmUI(string unitName);
        object GetEmWorker(string unitName);
        void LinkToCore(ICoreLink coreLinkObjcet);
        void UnlinkFromCore(ICoreLink coreLinkObjcet);
        List<Type> GetAssignableTypesFromPreLoadDlls(Type baseType);
        List<Type> GetAssignableTypes(string sourceDllKey, Type baseType);
        List<string> PreLoadDllKeys { get; }
        object GetAuxiliaryInstrumentDict();
        IAppPluginInteration GetAppPluginInstance(string appKey);
    }
}