using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestDatabase.Utility
{
    public class DBConnectionString
    {
        private static string _databaseType;
        private static string _connectionString;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DataBaseType DatabaseType
        {
            get
            {
                if (_databaseType == null)
                {
                    _databaseType = ConfigHelper.GetAppSettingsConfig("DatabaseType");
                }
                return (DataBaseType)Enum.Parse(typeof(DataBaseType), _databaseType.ToLower());
            }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    switch (DatabaseType)
                    {
                        case DataBaseType.mssql:
                            {
                                _connectionString = ConfigHelper.GetConnectionStringsConfig("MSSqlConnectionString");
                                break;
                            }
                        case DataBaseType.mysql:
                            {
                                _connectionString = ConfigHelper.GetConnectionStringsConfig("MysqlConnectionString");
                                break;
                            }
                    }

                    string ConStringEncrypt = ConfigHelper.GetAppSettingsConfig("ConStringEncrypt");
                    if (ConStringEncrypt == "true")
                    {
                        _connectionString = DESEncrypt.Decrypt(_connectionString);
                    }
                }
                return _connectionString;
            }
        }

        /// <summary>
        /// 获取连接字符串实例
        /// </summary>
        public DBConnectionString() { }

        /// <summary>
        /// 得到config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigHelper.GetConnectionStringsConfig(configName);
            string ConStringEncrypt = ConfigHelper.GetAppSettingsConfig("ConStringEncrypt");
            if (ConStringEncrypt == "true")
            {
                connectionString = DESEncrypt.Decrypt(connectionString);
            }
            return connectionString;
        }
    }
     
}
