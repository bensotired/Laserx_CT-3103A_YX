using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public sealed class TesterAppPluginManager : TesterAppPluginModel

    {
        static TesterAppPluginManager _instance;
        static object _mutex = new object();

        Dictionary<string, ITesterAppPluginInteration> AppPlugins { get; set; }
        Dictionary<string, ITesterAppPluginInteration> TestPlugins { get; set; }
        public TesterAppPluginManager()
        {
            this.AppPlugins = new Dictionary<string, ITesterAppPluginInteration>();
            this.TestPlugins = new Dictionary<string, ITesterAppPluginInteration>(); 
        }
        public static TesterAppPluginManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new TesterAppPluginManager();
                        }
                    }
                }
                return _instance;
            }
        }
        public void Setup(TestStationConfig config)
        {
            SetupAppPlugin(config.AppPlugins);
            SetupTestPlugin(config.TestPlugins);
        }

        public void SetupTestPlugin(List<TestPluginConfigItem> testPluginConfigItems)
        {
            try
            {
                foreach (var tp in this.TestPlugins.Values)
                {
                    this._coreInteration.UnlinkFromCore(tp);
                }
                TestPlugins.Clear();
                foreach (var item in testPluginConfigItems)
                {
                    var pluginType = AssemblyManager.GetTypeFromClassInAppPluginDlls(item.PluginType);

                    var plugin = AssemblyManager.CreateInstance<ITesterAppPluginInteration>(item.PluginType, pluginType);
                    plugin.SetConfigItem(item);
                    this._coreInteration.LinkToCore(plugin);

                    plugin.StartUp();
                    
                    plugin.ReinstallController();
                    if (TestPlugins.ContainsKey(item.PluginName))
                    {
                        TestPlugins[item.PluginName] = plugin;
                    }
                    else
                    {
                        TestPlugins.Add(item.PluginName, plugin);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetupAppPlugin(List<AppPluginConfigItem> appPluginConfigs)
        {
            try
            {
                foreach (var plugin in this.AppPlugins.Values)
                {
                    this._coreInteration.UnlinkFromCore(plugin);
                }
                AppPlugins.Clear();
                foreach (var item in appPluginConfigs)
                {
                    var pluginType = AssemblyManager.GetTypeFromClassInAppPluginDlls(item.PluginType);

                    var plugin = AssemblyManager.CreateInstance<ITesterAppPluginInteration>(item.PluginType, pluginType);
                    plugin.SetConfigItem(item);
                    this._coreInteration.LinkToCore(plugin);

                    plugin.StartUp();
                    plugin.ReinstallController();
             
                 
                    if (AppPlugins.ContainsKey(item.PluginName))
                    {
                        AppPlugins[item.PluginName] = plugin;
                    }
                    else
                    {
                        AppPlugins.Add(item.PluginName, plugin);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string[] GetAppPlugInKeys()
        {
            string[] keys = new string[this.AppPlugins.Count];
            this.AppPlugins.Keys.CopyTo(keys, 0);
            return keys;
        }
        public ITesterAppPluginInteration GetAppPlugin(string apKey)
        {
            if (this.AppPlugins.ContainsKey(apKey))
            {
                return this.AppPlugins[apKey];
            }
            return default(ITesterAppPluginInteration);
        }
        public IEnumerable<ITesterAppPluginInteration> GetAppPlugins()
        {
            return this.AppPlugins.Values;
        }

        public string[] GetTestPlugInKeys()
        {
            string[] keys = new string[this.TestPlugins.Count];
            this.TestPlugins.Keys.CopyTo(keys, 0);
            return keys;
        }
        public ITesterAppPluginInteration GetTestPlugin(string apKey)
        {
            if (this.TestPlugins.ContainsKey(apKey))
            {
                return this.TestPlugins[apKey];
            }
            return default(ITesterAppPluginInteration);
        }
        public IEnumerable<ITesterAppPluginInteration> GetTestPlugins()
        {
            return this.TestPlugins.Values;
        }
        public override void Close()
        {
            try
            {
                 foreach (var app in this.AppPlugins.Values)
                {
                    app.Close();
                }
                foreach (var app in this.TestPlugins.Values)
                {
                    app.Close();
                }
                
              
            }
            catch (Exception ex)
            {
            }
        }
    }
}