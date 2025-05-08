using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolveWare_TestDatabase.Utility
{
    /// <summary>
    /// 抓取Appsetting里面的值
    /// </summary>
    internal static class ConfigHelper
    { 

        //依据连接串名字connectionName返回数据连接字符串  
        public static string GetConnectionStringsConfig(string connectionName)
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            //参数1，模块；获取模块的路径 与 后面的字符串 拼接成一个完整的路劲
            string configPath = process.MainModule.FileName.Replace(process.ProcessName + ".exe", "SCADA.Utility.dll.config");
            ExeConfigurationFileMap configFileMap =
                new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configPath;
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            string connectionString =
            config.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString.ToString();
            return connectionString;
        }
        public static string GetAppSettingsConfig(string connectionName)
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            string configPath = process.MainModule.FileName.Replace(process.ProcessName + ".exe", "SCADA.Utility.dll.config");
            ExeConfigurationFileMap configFileMap =
            new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configPath;
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            string AppSettings =
            config.AppSettings.Settings[connectionName].Value.ToString();
            return AppSettings;
        }


        /////<summary> 
        /////更新连接字符串  
        /////</summary> 
        /////<param name="newName">连接字符串名称</param> 
        /////<param name="newConString">连接字符串内容</param> 
        /////<param name="newProviderName">数据提供程序名称</param> 
        //public static void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
        //{
        //    //指定config文件读取
        //    string file = System.Windows.Forms.Application.ExecutablePath;
        //    Configuration config = ConfigurationManager.OpenExeConfiguration(file);

        //    bool exist = false; //记录该连接串是否已经存在  
        //    //如果要更改的连接串已经存在  
        //    if (config.ConnectionStrings.ConnectionStrings[newName] != null)
        //    {
        //        exist = true;
        //    }
        //    // 如果连接串已存在，首先删除它  
        //    if (exist)
        //    {
        //        config.ConnectionStrings.ConnectionStrings.Remove(newName);
        //    }
        //    //新建一个连接字符串实例  
        //    ConnectionStringSettings mySettings =
        //        new ConnectionStringSettings(newName, newConString, newProviderName);
        //    // 将新的连接串添加到配置文件中.  
        //    config.ConnectionStrings.ConnectionStrings.Add(mySettings);
        //    // 保存对配置文件所作的更改  
        //    config.Save(ConfigurationSaveMode.Modified);
        //    // 强制重新载入配置文件的ConnectionStrings配置节  
        //    ConfigurationManager.RefreshSection("ConnectionStrings");
        //}



        /// <summary>
        /// 根据Key读取<add>元素的Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAddAppSettings(string key)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            string value = config.AppSettings.Settings[key].Value;
            return value;
        }

        /// <summary>
        /// 删除<add>元素
        /// </summary>
        /// <param name="key"></param>
        public static void AddAppSettings(string key)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            config.AppSettings.Settings.Remove(key);
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            ConfigurationManager.RefreshSection("appSettings");
        }


        /// <summary>
        /// 增加<add>元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSettings(string key, string value)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 修改appSettings键值对
        /// 写入<add>元素的Value
        /// </summary>
        /// <param name="key">元素</param>
        /// <param name="value">值</param>
        public static void ChangeAppSettings(string key, string value)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            config.AppSettings.Settings[key].Value = value;  //config.AppSettings.Settings["NetID"].Value=teNetID.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 更改ConnectionStrings
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newConString"></param>
        public static void ChangeConnectionStringsConfig(string newName, string newConString)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            config.ConnectionStrings.ConnectionStrings[newName].ConnectionString = newConString;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }


        /*需要注意的是：

        1、根据并不存在的Key值访问<add>元素，甚至使用remove()方法删除不存在的元素，都不会导致异常，前者会返回null。

        2、add已经存在的<add>元素也不会导致异常，而是concat了已有的Value和新的Value，用","分隔，例如："olldvalue,newvalue"。

        3、在项目进行编译后，在运行目录bin\Debuge文件下，将出现两个配置文件，一个名为“ProjectName.exe.config”，另一个名为“ProjectName.vshost.exe.config”。第一个文件为项目实际使用的配置文件，在程序运行中所做的更改都将被保存于此；第二个文件其实为原代码中“App.config”的同步文件，在程序运行中不会发生更改。

        4、特别注意大小写（XML文件是区分大小写的），例如appSettings配置节。

        5、可能有读者会想到，既然app.config是标准XML，当然也可以用操纵一般XML文件的方法来读写。这当然是可以的！只不过我认为这样就失去了VS提供app.config文件的意义了，还不如自己定义一个配置文件方便。*/

        ///<summary> 
        ///返回*.exe.config文件中appSettings配置节的value项  
        ///</summary> 
        ///<param name="strKey"></param> 
        ///<returns></returns> 
        public static string GetAppConfig(string strKey)
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == strKey)
                {
                    return config.AppSettings.Settings[strKey].Value.ToString();
                }
            }
            return null;
        }

        ///<summary>  
        ///在*.exe.config文件中appSettings配置节增加一对键值对  
        ///</summary>  
        ///<param name="newKey"></param>  
        ///<param name="newValue"></param>  
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            bool exist = false;
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == newKey)
                {
                    exist = true;
                }
            }
            if (exist)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        // 修改system.serviceModel下所有服务终结点的IP地址
        //public static void UpdateServiceModelConfig(string configPath, string serverIP)
        //{
        //    Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);
        //    ConfigurationSectionGroup sec = config.SectionGroups["system.serviceModel"];
        //    ServiceModelSectionGroup serviceModelSectionGroup = sec as ServiceModelSectionGroup;
        //    ClientSection clientSection = serviceModelSectionGroup.Client;
        //    foreach (ChannelEndpointElement item in clientSection.Endpoints)
        //    {
        //        string pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
        //        string address = item.Address.ToString();
        //        string replacement = string.Format("{0}", serverIP);
        //        address = Regex.Replace(address, pattern, replacement);
        //        item.Address = new Uri(address);
        //    }

        //    config.Save(ConfigurationSaveMode.Modified);
        //    ConfigurationManager.RefreshSection("system.serviceModel");
        //}

        // 修改applicationSettings中App.Properties.Settings中服务的IP地址
        public static void UpdateConfig(string configPath, string serverIP)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);
            ConfigurationSectionGroup sec = config.SectionGroups["applicationSettings"];
            ConfigurationSection configSection = sec.Sections["DataService.Properties.Settings"];
            ClientSettingsSection clientSettingsSection = configSection as ClientSettingsSection;
            if (clientSettingsSection != null)
            {
                SettingElement element1 = clientSettingsSection.Settings.Get("DataService_SystemManagerWS_SystemManagerWS");
                if (element1 != null)
                {
                    clientSettingsSection.Settings.Remove(element1);
                    string oldValue = element1.Value.ValueXml.InnerXml;
                    element1.Value.ValueXml.InnerXml = GetNewIP(oldValue, serverIP);
                    clientSettingsSection.Settings.Add(element1);
                }

                SettingElement element2 = clientSettingsSection.Settings.Get("DataService_EquipManagerWS_EquipManagerWS");
                if (element2 != null)
                {
                    clientSettingsSection.Settings.Remove(element2);
                    string oldValue = element2.Value.ValueXml.InnerXml;
                    element2.Value.ValueXml.InnerXml = GetNewIP(oldValue, serverIP);
                    clientSettingsSection.Settings.Add(element2);
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("applicationSettings");
        }

        private static string GetNewIP(string oldValue, string serverIP)
        {
            string pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
            string replacement = string.Format("{0}", serverIP);
            string newvalue = Regex.Replace(oldValue, pattern, replacement);
            return newvalue;
        }
    }
}
