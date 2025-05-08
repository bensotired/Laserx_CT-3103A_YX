using MySql.Data.MySqlClient;
using SolveWare_TestDatabase.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestDatabase.Utility
{
    public class MySQLFactory : IDBFactory
    {
        /// <summary>
        /// MS SQL Server Factory
        /// </summary>
        public MySQLFactory() { }

        /// <summary>
        /// 建立默认连接
        /// </summary>
        /// <returns>数据库连接</returns>
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// 根据连接字符串建立Connection对象
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns>数据库连接对象</returns>
        public IDbConnection CreateConnection(string connString)
        {
            return new MySqlConnection(connString);
        }

        /// <summary>
        /// 建立Command对象
        /// </summary>
        /// <returns>Command对象</returns>
        public IDbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        /// <summary>
        /// 建立DataAdapter对象
        /// </summary>
        /// <returns>DataAdapter对象</returns>
        public IDbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        /// <summary>
        /// 根据Connection建立Transaction
        /// </summary>
        /// <param name="myDbConnection">Connection对象</param>
        /// <returns>Transaction对象</returns>
        public IDbTransaction CreateTransaction(IDbConnection myDbConnection)
        {
            return myDbConnection.BeginTransaction();
        }

        /// <summary>
        /// 根据Command建立DataReader
        /// </summary>
        /// <param name="myDbCommand">Command对象</param>
        /// <returns>DataReader对象</returns>
        public IDataReader CreateDataReader(IDbCommand myDbCommand)
        {
            return myDbCommand.ExecuteReader();
        }

        /// <summary>
        /// 获得连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        public string GetConnectionString()
        {
            return DBConnectionString.ConnectionString;
        }

        public string GetConnectionString(string connName)
        {
            return DBConnectionString.GetConnectionString(connName);
        }



        /// <summary>
        /// 返回执行Return Value
        /// </summary>
        /// <returns>参数对象</returns>
        public IDbDataParameter GetReturnValuePara()
        {
            return new MySqlParameter("ReturnValue",
                MySqlDbType.Int16, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null);
        }
    }
}
