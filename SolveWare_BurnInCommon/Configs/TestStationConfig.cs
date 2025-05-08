using LX_BurnInSolution.Utilities;
using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public class TestStationConfig
    {
        string DefaultStationConfigFile = @"SystemConfig\StationConfigs.xml";
        static TestStationConfig _instance;
        static object _mutex = new object();
        public TestStationConfig()
        {
            this.AppPluginPackDlls = new List<string>();
            this.InstrumentChassisConfigs = new List<InstrumentChassisConfigItem>();
            this.AuxiliaryInstrumentConfigs = new List<InstrumentConfigItem>();
            this.InstrumentConfigs = new List<InstrumentConfigItem>();
            this.InstrumentStatusMonitorConfigs = new List<InstrumentStatusMonitorItem>();
            this.AppPlugins = new List<AppPluginConfigItem>();
            this.TestPlugins = new List<TestPluginConfigItem>();
            this.RackIOConfigs = new DataBook<string, string>();
            this.SystemParams = new DataBook<string, string>();
        }
        public static TestStationConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new TestStationConfig();
                        }
                    }
                }
                return _instance;
            }
        }
        public void Load()
        {
            _instance = XmlHelper.DeserializeFile<TestStationConfig>(DefaultStationConfigFile);
        }
        public void Save()
        {
            XmlHelper.SerializeFile<TestStationConfig>(DefaultStationConfigFile, _instance);
        }
        /// <summary>
        /// 默认产品名称
        /// </summary>
        public string StartupProductName { get; set; }
        /// <summary>
        /// 老化插件DLL名称列表
        /// </summary>
        public List<string> AppPluginPackDlls { get; set; }
 
        /// <summary>
        /// 硬件通信底层配置
        /// </summary>
        public List<InstrumentChassisConfigItem> InstrumentChassisConfigs { get; set; }
        /// <summary>
        /// 辅助型硬件配置
        /// </summary>
        public List<InstrumentConfigItem> AuxiliaryInstrumentConfigs { get; set; }
        /// <summary>
        /// 主要硬件配置
        /// </summary>
        public List<InstrumentConfigItem> InstrumentConfigs { get; set; }
        public List<InstrumentStatusMonitorItem> InstrumentStatusMonitorConfigs { get; set; }
        public List<AppPluginConfigItem> AppPlugins { get; set; }
        public List<TestPluginConfigItem> TestPlugins { get; set; }
        public TestPluginConfigItem TestPluginConfigItem { get; set; }
   
        //public KeyPathsAndFiles KeyPathsAndFiles { get; set; }
        /// <summary>
        /// 机架IO控制配置 
        /// </summary>
        public DataBook<string, string> RackIOConfigs { get; set; }
        /// <summary>
        /// 系统配置参数
        /// </summary>
        public DataBook<string, string> SystemParams { get; set; }

        public string GetSystemParamsValue(string key)
        {
            if (this.SystemParams.Count > 0)
            {
                if (this.SystemParams.ContainsKey(key))
                {
                    return this.SystemParams[key];
                }
            }
            return string.Empty;
        }
        public void SetSystemParamsPath(string key, string val)
        {

            if (this.SystemParams.ContainsKey(key))
            {
                this.SystemParams[key] = val;
            }
            else
            {
                this.SystemParams.Add(key, val);
            }
            this.Save();
        }
    }
}