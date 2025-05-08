namespace SolveWare_TestComponents.Data
{
    public interface ITestPluginImportProfileBase
    {
        string CreateTime { get; set; }
        string LastModifyTime { get; set; }
        string Name { get; set; }
        bool Check(out string checkLog, params object[] args);
        void Clear();
        void Save(string filePath);
    }
}