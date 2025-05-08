using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{

    public class AppPluginConfig
    {
        string DefaultAppPluginConfigFile = @"AppPluginConfig\AppPluginConfigs.xml";
        static AppPluginConfig _instance;
        static object _mutex = new object();
        public static AppPluginConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new AppPluginConfig();
                        }
                    }
                }
                return _instance;
            }
        }
        public AppPluginConfig()
        {
            this.AppPlugins = new List<AppPluginConfigItem>();
        }
        public List<AppPluginConfigItem> AppPlugins { get; set; }

 
        public void Load()
        {
            _instance = XmlHelper.DeserializeFile<AppPluginConfig>(DefaultAppPluginConfigFile);
        }
        public void Save()
        {
            _instance.AppPlugins.Add(new AppPluginConfigItem()
            {
                PluginName = "异常处理",
                PluginType = "SolveWare_BurnInException.dll",
                PluginDiscription = "",
            });

            XmlHelper.SerializeFile<AppPluginConfig>(DefaultAppPluginConfigFile, _instance);
        }
    }
}