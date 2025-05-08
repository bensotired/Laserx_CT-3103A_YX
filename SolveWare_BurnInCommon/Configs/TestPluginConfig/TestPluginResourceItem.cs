using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{

    public class TestPluginResourceItem
    {
        public TestPluginResourceItem()
        {
            this.ResourceItemList = new List<string>();
        }
        public string AppPluginName { get; set; }
        public ResourceItemType ItemType { get; set; }
        public List<string> ResourceItemList { get; set; }
    }

}