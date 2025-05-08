namespace SolveWare_BurnInAppInterface
{
    public interface ITesterAppPluginUIModel:  IAccessResourceOwner 
    {
 
        void CreateMainUI();
        void DockUI();
        object GetUI();
        string LoadConfig();
        string LoadData();
        void PopUI();
        string SaveConfig();
        string SaveData();
    }
}