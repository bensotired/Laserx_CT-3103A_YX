using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{

    public class TestPluginConfigItem : AppPluginConfigItem
    {
        public List<string> History { get; set; }
        public List<TestPluginResourceItem> ResourcePluginItems { get; set; }
        public List<TestExecutorUnitConfig> TestExecutorUnitConfigs { get; set; }
        public string PluginResourceProviderType { get; set; } 
        public TestPluginConfigItem() 
        {
            this.History = new List<string>();
            this.ResourcePluginItems = new List<TestPluginResourceItem>();
            this.TestExecutorUnitConfigs = new List<TestExecutorUnitConfig>();
        }
    }
}