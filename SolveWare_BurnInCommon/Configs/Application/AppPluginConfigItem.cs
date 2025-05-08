namespace SolveWare_BurnInCommon
{

    public class AppPluginConfigItem
    {
        public AppPluginConfigItem()
        {
            this.Plugin_PF_UIVisible = true;
            this.PluginEnable = true;
            this.PluginAccessLevel = 0;
        }

        public bool Plugin_PF_UIVisible { get; set; }
        public bool PluginEnable { get; set; }
        public int PluginAccessLevel { get; set; }
        public string PluginName { get; set; }
        public string PluginType { get; set; }
        public string PluginExecutor { get; set; }
        public string PluginConfigFile { get; set; }
        public string SlaverConfigFile { get; set; }
        public string PluginArguments { get; set; }
        public string PluginDiscription { get; set; }
    }
}