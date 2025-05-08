using SolveWare_BurnInCommon;
using SolveWare_Data_AccessDatabase.Business;
using SolveWare_TestComponents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class TesterCore
    {
        public string DataBaseConnectionString
        {
            get
            {
                var dbConn = this.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.DATA_BASE_CONNECTION_STRING);
                return dbConn;
            }
        }

        public bool IsDataBaseEnable
        {
            get
            {
                bool isDataEnable = false;
                var isDataEnableValue = this.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.IS_DATA_BASE_ENABLE);
                if (string.IsNullOrEmpty(isDataEnableValue))
                {
                    return isDataEnable;
                }
                else
                {
                    isDataEnable = Convert.ToBoolean(isDataEnableValue);
                }
                return isDataEnable;
            }
        }
        public string DataBaseTableName
        {
            get
            {
                return this.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.DATA_BASE_TABLENAME);
            }
        }
        public bool CreateAccessDatabase(string databaseFilePath, out string errMsg)
        {
            return AccessDatabaseManager.Instance.CreateAccessDatabase(databaseFilePath, out errMsg);
        }

        public bool CreateAccessDataTable(string tableName, List<string> listColumns, out string errMsg)
        {
            return AccessDatabaseManager.Instance.CreateAccessDataTable(tableName, listColumns, out errMsg);
        }
        //public void CreateAccessDTable(string filePath, string tableName, params ADOX.Column[] colums)
        //{
        //    AccessDatabaseManager.Instance.CreateAccessDataTable(filePath, tableName, colums);
        //}
        public bool IsDataBaseConnected(string databaseFilePath, out string errMsg)
        {
            return AccessDatabaseManager.Instance.IsDataBaseConnected(databaseFilePath, out errMsg);
        }

        public int SaveTestDataDut(string connectString, string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SaveTestDataDut(connectString, tableName, fieldVsValue, out errMsg);
        }

        public int SaveTestDataDut(string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SaveTestDataDut(tableName, fieldVsValue, out errMsg);
        }

        public int SaveTestDataDut(Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SaveTestDataDut(fieldVsValue, out errMsg);
        }

        public int SaveTestDataDut(string sqlString, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SaveTestDataDut(sqlString, out errMsg);
        }

        public DataTable SelectTestDataDut(string connectString, string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SelectTestDataDut(connectString, tableName, fieldVsValue, out errMsg);
        }

        public DataTable SelectTestDataDut(string tableName, Dictionary<string, object> fieldVsValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SelectTestDataDut(tableName, fieldVsValue, out errMsg);
        }

        public DataTable SelectTestDataDut(Dictionary<string, object> fieldVsValue, out string errMsg)
        {

            return AccessDatabaseManager.Instance.SelectTestDataDut(fieldVsValue, out errMsg);
        }

        public DataTable SelectTestDataDut(string sqlString, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SelectTestDataDut(sqlString, out errMsg);
        }

        public object SelectSpecificTestData(string connectString, string tableName, string paramName, string conditionName, object conditionValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SelectSpecificTestData(connectString, tableName, paramName, conditionName, conditionValue, out errMsg);
        }

        public object SelectSpecificTestData(string tableName, string paramName, string conditionName, object conditionValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SelectSpecificTestData(tableName, paramName, conditionName, conditionValue, out errMsg);
        }

        public object SelectSpecificTestData(string paramName, string conditionName, object conditionValue, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SelectSpecificTestData(paramName, conditionName, conditionValue, out errMsg);
        }

        public object SelectSpecificTestData(string sqlString, out string errMsg)
        {
            return AccessDatabaseManager.Instance.SelectSpecificTestData(sqlString, out errMsg);
        }

        //public double DataBaseContrast(string TableName, string columnName, double columnValue)
        //{
        //    return AccessDatabaseManager.Instance.DataBaseContrast(TableName, columnName, columnValue);
        //}
        public double DataBaseContrast(string filePath, string TableName, string columnName, double columnValue)
        {
            return AccessDatabaseManager.Instance.DataBaseContrast(filePath, TableName, columnName, columnValue);
        }
        public void NewCreateTable(string filePath, string TableName, string[] ColumnName)
        {
            try
            {
                AccessDatabaseManager.Instance.NewCreateTable(filePath, TableName, ColumnName);
            }
            catch (Exception)
            {

            }
        }

        public void NewDataBase(string filePath)
        {
            try
            {
                AccessDatabaseManager.Instance.NewDataBase(filePath);
            }
            catch (Exception)
            {

            }
        }

        public void NewUpDataTable(string filePath, string TableName, string sqlHeader, string sqlValues, Dictionary<string, object> summarydata, string serialNumber, string startTime)
        {
            try
            {
                AccessDatabaseManager.Instance.NewUpDataTable(filePath, TableName, sqlHeader, sqlValues, summarydata, serialNumber, startTime);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
    }
}