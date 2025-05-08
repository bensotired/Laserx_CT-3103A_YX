using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_Data_AccessDatabase.Base;
using SolveWare_Data_AccessDatabase.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SolveWare_Data_AccessDatabase.Business
{
    public class AccessDatabaseManager : TesterAppPluginUIModel, IAccessDatabaseManager
    {
        public string DataBaseFilePath = string.Empty;
        
        public string TableName = string.Empty;
        static AccessDatabaseManager _instance;
        static object _mutex = new object();
        public AccessDatabaseManager() { }

        public AccessDatabaseManager(string dbPath, string tableName)
        {
            this.DataBaseFilePath = dbPath;
            this.TableName = tableName;
        }
        public static AccessDatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccessDatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }
        public override void StartUp()
        {
            // base.StartUp();
            if (this._coreInteration.IsDataBaseEnable)
            {
                var FilePath = this._coreInteration.DataBaseConnectionString;
                var TableName = this._coreInteration.DataBaseTableName;
                string errMsg = string.Empty;
                var Dbase = CreateAccessDatabase(FilePath, out errMsg);
                if (Dbase == false)
                {
                    Log_Global("数据库创建失败！" + errMsg);
                    throw new Exception("数据库创建失败！" + errMsg);
                }
            }
        }
        public bool CreateAccessDatabase(string databaseFilePath, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                if (File.Exists(databaseFilePath))
                {
                    return true;
                }

                return AccessHelper.CreateAccessDb(databaseFilePath, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
                return false;
            }
        }

        public bool CreateAccessDataTable(string tableName, List<string> listColumns, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                AccessHelper.DATABASE = this.DataBaseFilePath;
                if (AccessHelper.TableExists(tableName))
                {
                    return true;
                }

                return AccessHelper.CreateAccessTable(tableName, listColumns, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
                return false;
            }
        }
        //public  void CreateAccessDataTable(string filePath, string tableName, params ADOX.Column[] colums )
        //{
        //    try
        //    {
        //        AccessHelper.CreateAccessTable(filePath, tableName, colums);
        //    }
        //    catch (Exception)
        //    {
                
        //    }
        //}

        public bool IsDataBaseConnected(string databaseFilePath, out string errMsg)// 检测数据库是否能连接成功
        {
            try
            {
                return AccessHelper.OpenConnect(databaseFilePath, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
                return false;
            }
        }

        public int SaveTestDataDut(string dbPath, string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            int row = 0;
            errMsg = string.Empty;
            string sqlCmd = string.Empty;
            try
            {
                AccessHelper.DATABASE = dbPath;
                if (AccessHelper.IsConnected == false)
                {
                    errMsg = $"数据库连接失败：{dbPath}";
                    return row;
                }
                if (AccessHelper.TableExists(tableName) == false)
                {
                    errMsg = $"数据库表不存在：{tableName}";
                    return row;
                }

                #region 判断表中是否存在相应的字段，如果没有创建该字段

                List<string> fields = AccessHelper.GetExcelTableColumn(tableName);

                foreach (string param in fieldVsValue.Keys)
                {
                    if (fields.Contains(param) == false)
                    {
                        if (param.Equals(ConstantParmaName.StartTime) || param.Equals(ConstantParmaName.EndTime))
                        {
                            sqlCmd = string.Format("ALTER TABLE [{0}] ADD COLUMN [{1}] DateTime NULL", tableName, param);
                        }
                        else
                        {
                            sqlCmd = string.Format("ALTER TABLE [{0}] ADD COLUMN [{1}] Varchar(200) NULL", tableName, param);
                        }
                        AccessHelper.ExecuteSql(sqlCmd);
                    }
                }
                #endregion

                StringBuilder paramStringBuilder = new StringBuilder(string.Format("INSERT INTO [{0}] (", tableName));
                StringBuilder valueStringBuilder = new StringBuilder("VALUES(");

                foreach (var item in fieldVsValue)
                {
                    paramStringBuilder.Append("[" + item.Key + "],");
                    valueStringBuilder.Append("'" + item.Value + "',"); 
                }

                string paramString = paramStringBuilder.ToString();
                paramString = paramString.Substring(0, paramString.Length - 1);
                string valueString = valueStringBuilder.ToString();
                valueString = valueString.Substring(0, valueString.Length - 1);
                string cmd = paramString + ") " + valueString + ");";

                sqlCmd = cmd;
                row = AccessHelper.ExecuteSql(cmd);//插入单个产品数据到表

            }
            catch (Exception ex)
            {
                errMsg = $"sql:{sqlCmd}-ex:{ex.Message}-{ex.StackTrace}";
                return row;
            }
            return row;
        }

        public int SaveTestDataDut(string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            int row = 0;
            errMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = $"数据库连接字符为空：{DataBaseFilePath}";
                    return row;
                }
                row = SaveTestDataDut(DataBaseFilePath, tableName, fieldVsValue, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
                return row;
            }
            return row;
        }

        public int SaveTestDataDut(Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            int row = 0;
            errMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = $"数据库连接字符为空：{DataBaseFilePath}";
                    return row;
                }

                if (string.IsNullOrEmpty(TableName))
                {
                    errMsg = $"表名为空：{TableName}";
                    return row;
                }
                row = SaveTestDataDut(DataBaseFilePath, TableName, fieldVsValue, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
                return row;
            }
            return row;
        }

        public int SaveTestDataDut(string sqlString, out string errMsg)
        {
            int row = 0;
            errMsg = string.Empty;
            try
            {
                row = AccessHelper.ExecuteSql(sqlString);
            }
            catch (Exception ex)
            {
                errMsg = $"sql:{sqlString}-ex:{ex.Message}-{ex.StackTrace}";
                return row;
            }
            return row;
        }

        public object SelectSpecificTestData(string dbPath, string tableName, string parmName, string conditionName, object conditionValue, out string errMsg)
        {
            object obj = null;
            errMsg = string.Empty;
            StringBuilder sql = new StringBuilder();
            try
            {
                AccessHelper.DATABASE = dbPath;
                if (AccessHelper.IsConnected == false)
                {
                    errMsg = $"数据库连接失败：{dbPath}";
                    return obj;
                }
                if (AccessHelper.TableExists(tableName) == false)
                {
                    errMsg = $"数据库表不存在：{tableName}";
                    return obj;
                }

                sql.Append($"SELECT {parmName} FROM {tableName} WHERE 1=1 ");

                if (conditionName.ToLower().Contains(ConstantParmaName.StartTime))
                {
                    sql.Append($" AND {conditionName}>=#{conditionValue}# ");//时间字段比较特殊
                }
                else if (conditionName.ToLower().Contains(ConstantParmaName.EndTime))
                {
                    sql.Append($" AND {conditionName}<=#{conditionValue}# ");
                }
                else
                {
                    sql.Append($" AND {conditionName}='{conditionValue}' ");
                }

                sql.Append($" ORDER BY ID DESC;");//自增长ID必须要，在创建表的时候指定

                obj = AccessHelper.GetSingle(sql.ToString());

            }
            catch (Exception ex)
            {
                errMsg = $"sql:{sql.ToString()}-ex:{ex.Message}-{ex.StackTrace}";
                return obj;
            }
            return obj;
        }

        public object SelectSpecificTestData(string tableName, string parmName, string conditionName, object conditionValue, out string errMsg)
        {
            object obj = null;
            errMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = "连接字符为空！";
                    return obj;
                }

                obj = SelectSpecificTestData(DataBaseFilePath, tableName, conditionName, conditionValue, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
                return obj;
            }
            return obj;
        }

        public object SelectSpecificTestData(string parmName, string conditionName, object conditionValue, out string errMsg)
        {
            object obj = null;
            errMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = "连接字符为空！";
                    return obj;
                }
                if (string.IsNullOrEmpty(TableName))
                {
                    errMsg = "表名为空！";
                    return obj;
                }
                obj = SelectSpecificTestData(DataBaseFilePath, TableName, parmName, conditionName, conditionValue, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
                return obj;
            }
            return obj;
        }

        public object SelectSpecificTestData(string sqlString, out string errMsg)
        {
            object obj = null;
            errMsg = string.Empty;
            try
            {
                obj = AccessHelper.GetSingle(sqlString, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"sql:{sqlString}-ex:{ex.Message}-{ex.StackTrace}";
                return obj;
            }
            return obj;
        }

        public DataTable SelectTestDataDut(string dbPath, string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            DataTable dt = new DataTable();
            errMsg = string.Empty;
            try
            {
                AccessHelper.DATABASE = dbPath;
                if (AccessHelper.IsConnected == false)
                {
                    errMsg = $"数据库连接失败：{dbPath}";
                    return dt;
                }
                if (AccessHelper.TableExists(tableName) == false)
                {
                    errMsg = $"表不存在：{tableName}";
                    return dt;
                }

                StringBuilder sql = new StringBuilder();
                sql.Append($"SELECT * FROM {tableName} WHERE 1=1 ");
                foreach (var parm in fieldVsValue)
                {
                    if (parm.Key.ToLower().Contains(ConstantParmaName.StartTime))
                    {
                        sql.Append($" AND {parm.Key}>=#{parm.Value}# ");//时间字段比较特殊
                    }
                    else if (parm.Key.ToLower().Contains(ConstantParmaName.EndTime))
                    {
                        sql.Append($" AND {parm.Key}<=#{parm.Value}# ");
                    }
                    else
                    {
                        sql.Append($" AND {parm.Key}='{parm.Value}' ");
                    }
                }
                sql.Append($" ORDER BY ID DESC;");//自增长ID必须要，在创建表的时候指定

                dt = AccessHelper.QueryDataTable(sql.ToString());

            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
            }
            return dt;
        }

        public DataTable SelectTestDataDut(string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            DataTable dt = new DataTable();
            errMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = "连接字符为空！";
                    return dt;
                }

                dt = SelectTestDataDut(DataBaseFilePath, tableName, fieldVsValue, out errMsg);

            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
            }
            return dt;
        }

        public DataTable SelectTestDataDut(Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            DataTable dt = new DataTable();
            errMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = "连接字符为空！";
                    return dt;
                }
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = "表名为空！";
                    return dt;
                }

                dt = SelectTestDataDut(DataBaseFilePath, TableName, fieldVsValue, out errMsg);

            }
            catch (Exception ex)
            {
                errMsg = $"ex:{ex.Message}-{ex.StackTrace}";
            }
            return dt;
        }

        public DataTable SelectTestDataDut(string sqlString, out string errMsg)
        {
            DataTable dt = new DataTable();
            errMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = "连接字符为空！";
                    return dt;
                }
                if (string.IsNullOrEmpty(DataBaseFilePath))
                {
                    errMsg = "表名为空！";
                    return dt;
                }

                dt = AccessHelper.ExecuteDataTable(DataBaseFilePath, sqlString, null);

            }
            catch (Exception ex)
            {
                errMsg = $"sql:{sqlString}-ex:{ex.Message}-{ex.StackTrace}";
            }
            return dt;
        }
        public  void NewDataBase(string filePath)
        {
            AccessHelper.DataBase(filePath);
        }
        public  void NewCreateTable(string filePath, string TableName, string[] ColumnName)
        {
            AccessHelper.CreateTable(filePath, TableName,ColumnName);
        }
        public void NewUpDataTable(string filePath, string TableName, string sqlHeader, string sqlValues, Dictionary<string,object> summarydata, string serialNumber, string startTime)
        {
            AccessHelper.UpDataTable(filePath, TableName, sqlHeader, sqlValues, summarydata, serialNumber, startTime);
        }
        public double DataBaseContrast(string filePath, string TableName, string columnName, double columnValue)
        {
            try
            {
                var diff = AccessHelper.DataContrast(filePath,TableName, columnName, columnValue);
                return diff;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string resourceOwnerName)
        {
            switch (currnetResourceOwner)
            {
                case GenernalResourceOwner.Platform:
                case GenernalResourceOwner.Plugin:
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
        }
        public const string ConfigFileName = @"DBFilter.xml";
          List<string> _DBLocalFilter { get;   set; } = new List<string>();
        public override void CreateMainUI()
        {
           // throw new NotImplementedException();
        }

        public override void ReinstallController()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(ConfigFileName);
                try
                {
                    _DBLocalFilter = XmlHelper.DeserializeFile<List<string>>(fullpath);
                    //AccessHelper.DBLocalFilter = _DBLocalFilter;
                }
                catch
                {
                    this._DBLocalFilter = new List<string>();
                    XmlHelper.SerializeFile(fullpath, _DBLocalFilter);
                }
               
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] DatabaseParamFilter 配置参数失败!");
            }
        }
        public override bool SwitchProductConfig()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath( ConfigFileName);
                try
                {
                    _DBLocalFilter= XmlHelper.DeserializeFile<List<string>>(fullpath);
                    //AccessHelper.DBLocalFilter = _DBLocalFilter;
                }
                catch
                {
                    this._DBLocalFilter = new  List<string>();
                
                    XmlHelper.SerializeFile(fullpath, _DBLocalFilter);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] DatabaseParamFilter 配置参数失败!");
            }
        }
        public override bool CreateProductConfig()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(  ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置DatabaseParamFilter文件为空!");
            }
            try
            {
                XmlHelper.SerializeFile(fullpath, _DBLocalFilter);
             
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] DatabaseParamFilter 配置参数失败!");
            }
        }
        
    }
}
