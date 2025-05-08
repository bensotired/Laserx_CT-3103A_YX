using SolveWare_BurnInCommon;

namespace SolveWare_BurnInAppInterface
{
    /// <summary>
    /// 测试ying
    /// </summary>
    public interface ITesterAppPluginInteration : ITesterAppLogHandle, ITesterAppExceptionHandle, ITesterAppPluginUIModel, ITesterCoreLink, IAccessPermissionLevel
    {
        string Name { get; }
        string VersionInfo { get; }
        bool IsPluginEnable { get; }

        bool IsPlugin_PF_UIVisible { get; }

        void StartUp();
        void SetConfigItem(AppPluginConfigItem configItem);
        void Dev();
        void ReinstallController();
        bool SwitchProductConfig();
        bool CreateProductConfig();
        void Close();
        void RefreshOnceUI();
    }
}