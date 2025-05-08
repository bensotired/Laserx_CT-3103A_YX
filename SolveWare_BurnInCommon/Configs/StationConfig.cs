using LX_BurnInSolution.Utilities;
using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public class StationConfig
    {
        string DefaultStationConfigFile = @"SystemConfig\StationConfigs.xml";
        static StationConfig _instance;
        static object _mutex = new object();
        public StationConfig()
        {
            this.BIPluginPackDlls = new List<string>();

            this.InstrumentChassisConfigs = new List<InstrumentChassisConfigItem>();
            this.AuxiliaryInstrumentConfigs = new List<InstrumentConfigItem>();
            this.InstrumentConfigs = new List<InstrumentConfigItem>();
            this.InstrumentRefreshGroupConfigs = new List<InstrumentRefreshGroupConfigItem>();
            this.BIChassisConfigs = new List<BIChassisConfigItem>();
            this.BIUnitConfigs = new List<BIUnitConfigItem>();
            this.RackIOConfigs = new DataBook<string, string>();
            this.SystemParams = new DataBook<string, string>();
            this.BIWorkerReflectiveConfigs = new List<BIWorkerConfigItem>();
        }
        public static StationConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new StationConfig();
                        }
                    }
                }
                return _instance;
            }
        }
        public void Load()
        {
            _instance = XmlHelper.DeserializeFile<StationConfig>(DefaultStationConfigFile);
        }
        public void Save()
        {
            XmlHelper.SerializeFile<StationConfig>(DefaultStationConfigFile, _instance);
        }
        public void Demo()
        {
            this.BIPluginPackDlls.Add("Hello.dll");
            this.InstrumentChassisConfigs.Add(new InstrumentChassisConfigItem()
            {
                ChassisType = "demoChassis",
                IsOnline = true,
                Name = "chas1",
                Resource = "100.100.10.10"
            });
            this.InstrumentChassisConfigs.Add(new InstrumentChassisConfigItem()
            {
                ChassisType = "demoChassis",
                IsOnline = true,
                Name = "chas2",
                Resource = "100.100.10.10"
            });
            this.InstrumentChassisConfigs.Add(new InstrumentChassisConfigItem()
            {
                ChassisType = "demoChassis",
                IsOnline = true,
                Name = "chas2Rack",
                Resource = "100.100.10.10"
            });
            this.AuxiliaryInstrumentConfigs.Add(new InstrumentConfigItem()
            {
                Address = "1",
                ChassisName = "chas2Rack",
                IsOnline = true,
                IsSimulation = false,
                Name = "inst1",

                InstrumentType = "smu"
            });
            this.InstrumentConfigs.Add(new InstrumentConfigItem()
            {
                Address = "1",
                ChassisName = "chas1",
                IsOnline = true,
                IsSimulation = false,
                Name = "inst1",/* Group = "grp1",*/
                InstrumentType = "smu"
            });
            this.InstrumentConfigs.Add(new InstrumentConfigItem()
            {
                Address = "2",
                ChassisName = "chas2",
                IsOnline = false,
                IsSimulation = true,
                Name = "inst1",
                //Group = "grp1",
                InstrumentType = "smu"
            });
            BIChassisConfigItem bic = new BIChassisConfigItem();
            bic.ChassisInstrumentList.Add("inst11", "inst12");
            var temp = new List<string>();
            temp.Add("1_1");
            temp.Add("1_2");
            bic.IsOnline = true;
            bic.Name = "demobic";
            bic.UnitControllerGroupList.Add("grp1", temp);
            this.BIChassisConfigs.Add(bic);
            this.BIChassisConfigs.Add(bic);
            BIUnitConfigItem biu = new BIUnitConfigItem();
            biu.Name = "1_1";
            biu.IsOnline = true;
            biu.Type = "demoT";
            biu.UnitInstrumentList.Add("uinst1", "uinst22");
            biu.UnitInstrumentList.Add("uinst3", "uinst44");
            biu.IoMap.Add("demoIO1", "10086");
            biu.IoMap.Add("demoIO2", "10086");
            DataBook<string, string> smap = new DataBook<string, string>();
            smap.Add("LD1", "1");
            smap.Add("LD2", "2");

            List<SectionItem> sitem = new List<SectionItem>();
            sitem.Add(new SectionItem() { Name = "LD1", InstChannel = 2, InstKey = "SMU_1" });
            sitem.Add(new SectionItem() { Name = "LD2", InstChannel = 2, InstKey = "SMU_2" });
            sitem.Add(new SectionItem() { Name = "EA1", InstChannel = 2, InstKey = "SMU_1" });
            biu.SectionMap.Add(1, sitem);
            biu.SectionMap.Add(2, sitem);
            InstrumentRefreshGroupConfigItem rgc = new InstrumentRefreshGroupConfigItem();
            rgc.Name = "inst";
            rgc.IsOnline = true;
            rgc.IsSimulation = false;
            rgc.RefreshList.Add("abc");
            rgc.RefreshList.Add("abc");
            rgc.RefreshList.Add("abc");
            this.InstrumentRefreshGroupConfigs.Add(rgc);
            this.InstrumentRefreshGroupConfigs.Add(rgc);
            this.BIUnitConfigs.Add(biu);
            this.BIUnitConfigs.Add(biu);
            this.RackIOConfigs.Add("valve", "10086");
            this.RackIOConfigs.Add("cda", "10087");
            this.SystemParams.Add("sys1", "10088");
            this.SystemParams.Add("sys2", "10089");
            XmlHelper.SerializeFile<StationConfig>(DefaultStationConfigFile, _instance);
        }
        /// <summary>
        /// 老化插件DLL名称列表
        /// </summary>
        public List<string> BIPluginPackDlls { get; set; }
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
        /// <summary>
        /// 硬件设备数据刷新组
        /// </summary>
        public List<InstrumentRefreshGroupConfigItem> InstrumentRefreshGroupConfigs { get; set; }
        /// <summary>
        /// 老化单元配置s
        /// </summary>
        public List<BIUnitConfigItem> BIUnitConfigs { get; set; }
        /// <summary>
        /// 老化抽屉配置
        /// </summary>
        public List<BIChassisConfigItem> BIChassisConfigs { get; set; }
        /// <summary>
        /// 老化组件映射配置
        /// </summary>
        public List<BIWorkerConfigItem> BIWorkerReflectiveConfigs { get; set; }
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