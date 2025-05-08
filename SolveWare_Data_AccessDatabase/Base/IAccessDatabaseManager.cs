using System.Collections.Generic;
using System.Data;

namespace SolveWare_Data_AccessDatabase.Base
{
    public interface IAccessDatabaseManager
    {
        /// <summary>
        /// 创建数据库文件
        /// </summary>
        /// <param name="databaseFilePath">指定数据库文件路径</param>
        /// <returns></returns>
        bool CreateAccessDatabase(string databaseFilePath, out string errMsg);

        /// <summary>
        /// 在指定数据库中创建指定数据表
        /// </summary>
        /// <param name="databaseFilePath">数据库地址</param>
        /// <param name="tableName">表名</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        bool CreateAccessDataTable(string tableName, List<string> listColumns, out string errMsg);
        //void CreateAccessDataTable(string filePath, string tableName, params ADOX.Column[] colums);

        /// <summary>
        /// 检测数据库是否能连接成功
        /// </summary>
        /// <param name="databaseFilePath">数据库地址</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        bool IsDataBaseConnected(string databaseFilePath, out string errMsg);

        /// <summary>
        /// 保存测试数据至数据库
        /// </summary>
        /// <param name="connectString">数据库连接字符</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="fieldVsValue">保存的字段对应的值，字典</param>
        /// <returns>保存成功的数据量</returns>
        int SaveTestDataDut(string connectString, string tableName, Dictionary<string, object> fieldVsValue, out string errMsg);
        int SaveTestDataDut(string tableName, Dictionary<string, object> fieldVsValue, out string errMsg);
        int SaveTestDataDut(Dictionary<string, object> fieldVsValue, out string errMsg);
        int SaveTestDataDut(string sqlString, out string errMsg);

        /// <summary>
        /// 查询多个条件下测试数据
        /// </summary>
        /// <param name="connectString">数据库连接字符</param>
        /// <param name="tableName">表名</param>
        /// <param name="fieldVsValue">需要查询的属性对应的值，字典</param>
        /// <returns></returns>
        DataTable SelectTestDataDut(string connectString, string tableName, Dictionary<string, object> fieldVsValue, out string errMsg);
        DataTable SelectTestDataDut(string tableName, Dictionary<string, object> fieldVsValue, out string errMsg);
        DataTable SelectTestDataDut(Dictionary<string, object> fieldVsValue, out string errMsg);
        DataTable SelectTestDataDut(string sqlString, out string errMsg);

        /// <summary>
        /// 查询单个条件下的测试数据
        /// </summary>
        /// <param name="connectString">数据库连接字符</param>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object SelectSpecificTestData(string connectString, string tableName, string paramName, string conditionName, object conditionValue, out string errMsg);
        object SelectSpecificTestData(string tableName, string paramName, string conditionName, object conditionValue, out string errMsg);
        object SelectSpecificTestData(string paramName, string conditionName, object conditionValue, out string errMsg);
        object SelectSpecificTestData(string sqlString, out string errMsg);


        /// <summary>
        /// 新写的建库--Irwin
        /// </summary>
        /// <param name="filePath">数据库地址</param>
        void NewDataBase(string filePath);
        /// <summary>
        /// 新写的建表--Irwin
        /// </summary>
        /// <param name="filePath">数据库地址</param>
        /// <param name="TableName">表名</param>
        void NewCreateTable(string filePath, string TableName, string[] ColumnName);
        /// <summary>
        /// 向表中写入数据（动态）--Irwin
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="TableName"></param>
        /// <param name="sqlHeader"></param>
        /// <param name="sqlValues"></param>
        /// <param name="summarydata"></param>
        //void NewUpDataTable(string filePath, string TableName, string sqlHeader, string sqlValues, SummaryDataCollection summarydata);
        //void NewUpDataTable(string filePath, string TableName, string sqlHeader, string sqlValues, SummaryDataCollection summarydata, string serialNumber, string startTime);
        void NewUpDataTable(string filePath, string TableName, string sqlHeader, string sqlValues, Dictionary<string, object> summarydata, string serialNumber, string startTime);
        /// <summary>
        /// 数据对比--Irwin
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="columnValue">对应的值</param>
        /// <returns></returns>
        //double DataBaseContrast(string TableName, string columnName, double columnValue);
        double DataBaseContrast(string filePath, string TableName, string columnName, double columnValue);
    }
}
