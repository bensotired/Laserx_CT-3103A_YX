using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface ICoreRuntime : ILogHandle, IExceptionHandle
    {
        string PlatformVersionInfomation
        {
            get; 
        }
        object GetHardWareChassisInstance(string unitName);
        void RunGroupAction(string unitName, Action<object> action, object actionArgs);
        bool IsGroupIdle(string unitName);
        bool IsAnyGroupMemberRunning(string unitName);
        bool ExistChassisManager(string unitName);
        StationConfig StationConfig { get; }
        string CurrentUserID { get; }
        AccessPermissionLevel CurrentAPL { get; }
        void UpdateWIDMesStatus(WorkerImportData wid);
    }
}