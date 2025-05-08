using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestDatabase.Interface
{
    public interface IDBFactory
    {
        /// <summary>
        /// 建立默认连接
        /// </summary>
        /// <returns>数据库连接</returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// 根据连接字符串建立Connection对象
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns>数据库连接对象</returns>
        IDbConnection CreateConnection(string connString);

        /// <summary>
        /// 建立Command对象
        /// </summary>
        /// <returns>Command对象</returns>
        IDbCommand CreateCommand();

        /// <summary>
        /// 建立DataAdapter对象
        /// </summary>
        /// <returns>DataAdapter对象</returns>
        IDbDataAdapter CreateDataAdapter();

        /// <summary>
        /// 根据Connection建立Transaction
        /// </summary>
        /// <param name="myDbConnection">Connection对象</param>
        /// <returns>Transaction对象</returns>
        IDbTransaction CreateTransaction(IDbConnection myDbConnection);

        /// <summary>
        /// 根据Command建立DataReader
        /// </summary>
        /// <param name="myDbCommand">Command对象</param>
        /// <returns>DataReader对象</returns>
        IDataReader CreateDataReader(IDbCommand myDbCommand);

        /// <summary>
        /// 获得连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        string GetConnectionString();

        string GetConnectionString(string connName);

        /// <summary>
        /// 返回执行Return Value
        /// </summary>
        /// <returns>参数对象</returns>
        IDbDataParameter GetReturnValuePara();

    }
}
