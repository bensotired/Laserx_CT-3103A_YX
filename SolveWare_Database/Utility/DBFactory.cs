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
    /// <summary>
    /// Factory类
    /// </summary>
    public sealed class DBFactory
    {
        private DataBaseType _databaseType = DBConnectionString.DatabaseType;

        /// <summary>
        /// DBFactory类构造函数
        /// </summary>
        public DBFactory()
        {
        }

        public DBFactory(DataBaseType databaseType)
        {
            _databaseType = databaseType;
        }


        /// <summary>
        /// 建立DBFactory类实例
        /// </summary>
        /// <returns>DBFactory类实例</returns>
        public IDBFactory CreateInstance()
        {
            IDBFactory abstractDbFactory = null;
            switch (_databaseType)
            {
                case DataBaseType.mssql:
                    //abstractDbFactory = new MSSQLFactory();
                    break;
                case DataBaseType.oracal:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.mysql:
                    //abstractDbFactory = new MySQLFactory();
                    break;
                case DataBaseType.odbc:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.oledb:
                    abstractDbFactory = null;
                    break;
                default:
                    throw new Exception("传入参数异常，请配置正确的DatabaseType!");
            }
            return abstractDbFactory;
        }



        /// <summary>
        /// 返回执行分页操作的接口对象
        /// </summary>
        /// <returns>分页操作的接口对象</returns>
        public IPagingHelper CreatePagingInstance()
        {
            IPagingHelper paginHelper = null;
            switch (_databaseType)
            {
                case DataBaseType.mssql:
                    //paginHelper = new MSSqlPagingHelper();
                    break;
                case DataBaseType.oracal:
                    paginHelper = null;
                    break;
                case DataBaseType.mysql:
                    paginHelper = null;
                    break;
                case DataBaseType.odbc:
                    paginHelper = null;
                    break;
                case DataBaseType.oledb:
                    paginHelper = null;
                    break;
                default:
                    throw new Exception("传入参数异常，请配置正确的DatabaseType!");
            }
            return paginHelper;
        }




        /// <summary>
        /// 返回执行的SQL参数
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="sqlType">SQL Type</param>
        /// <param name="size">大小</param>
        /// <returns>DB参数对象</returns>
        public IDbDataParameter CreateDBParameter(string paraName, SqlDbType sqlType, int size)
        {


            IDbDataParameter abstractDbFactory = null;
            switch (_databaseType)
            {
                case DataBaseType.mssql:
                    abstractDbFactory = new System.Data.SqlClient.SqlParameter(paraName, sqlType, size);
                    break;
                case DataBaseType.oracal:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.mysql:
                    abstractDbFactory = new MySql.Data.MySqlClient.MySqlParameter(paraName, sqlType);
                    break;
                case DataBaseType.odbc:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.oledb:
                    abstractDbFactory = null;
                    break;
                default:
                    throw new Exception("传入参数异常，请配置正确的DatabaseType!");
            }

            return abstractDbFactory;
        }

        /// <summary>
        /// 返回执行的SQL参数
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="sqlType">SQL Type</param>
        /// <returns>DB参数对象</returns>
        public IDbDataParameter CreateDBParameter(string paraName, SqlDbType sqlType)
        {
            IDbDataParameter abstractDbFactory = null;
            switch (_databaseType)
            {
                case DataBaseType.mssql:
                    abstractDbFactory = new System.Data.SqlClient.SqlParameter(paraName, sqlType);
                    break;
                case DataBaseType.oracal:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.mysql:
                    abstractDbFactory = new MySql.Data.MySqlClient.MySqlParameter(paraName, sqlType);
                    break;
                case DataBaseType.odbc:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.oledb:
                    abstractDbFactory = null;
                    break;
                default:
                    throw new Exception("传入参数异常，请配置正确的DatabaseType!");
            }

            return abstractDbFactory;
        }
        /// <summary>
        /// 返回执行的SQL参数
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="sqlType">参数类型</param>
        /// <returns>DB参数对象</returns>
        public IDbDataParameter CreateDBParameter(string paraName, string sqlType)
        {
            IDbDataParameter abstractDbFactory = null;
            switch (_databaseType)
            {
                case DataBaseType.mssql:
                    abstractDbFactory = new System.Data.SqlClient.SqlParameter(paraName, sqlType);
                    break;
                case DataBaseType.oracal:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.mysql:
                    abstractDbFactory = GetMysqlPara(paraName, sqlType);
                    break;
                case DataBaseType.odbc:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.oledb:
                    abstractDbFactory = null;
                    break;
                default:
                    throw new Exception("传入参数异常，请配置正确的DatabaseType!");
            }

            return abstractDbFactory;
        }

        public IDbDataParameter CreateDBParameter(string paraName, string sqlType, int size)
        {
            //  DataBaseHelper.CreateDBParameter("?_PLCNo",     "varChar",      100)

            IDbDataParameter abstractDbFactory = null;
            switch (_databaseType)
            {
                case DataBaseType.mssql:
                    abstractDbFactory = GetSqlPara(paraName, sqlType, size);
                    break;
                case DataBaseType.oracal:
                    //abstractDbFactory = GetOraclePara(paraName, sqlType);
                    break;
                case DataBaseType.mysql:
                    abstractDbFactory = GetMysqlPara(paraName, sqlType, size);
                    break;
                case DataBaseType.odbc:
                    abstractDbFactory = null;
                    break;
                case DataBaseType.oledb:
                    abstractDbFactory = GetOleDbPara(paraName, sqlType);
                    break;
                default:
                    throw new Exception("传入参数异常，请配置正确的DatabaseType!");
            }

            return abstractDbFactory;
        }

        private System.Data.SqlClient.SqlParameter GetSqlPara(string paraName, string dataType, int size)
        {
            switch (dataType.ToLower())
            {
                case "decimal":
                    return new System.Data.SqlClient.SqlParameter(paraName, System.Data.SqlDbType.Decimal, size);
                case "varchar":
                    return new System.Data.SqlClient.SqlParameter(paraName, System.Data.SqlDbType.VarChar, size);
                case "datetime":
                    return new System.Data.SqlClient.SqlParameter(paraName, System.Data.SqlDbType.DateTime);
                case "iamge":
                    return new System.Data.SqlClient.SqlParameter(paraName, System.Data.SqlDbType.Image);
                case "int":
                    return new System.Data.SqlClient.SqlParameter(paraName, System.Data.SqlDbType.Int);
                case "text":
                    return new System.Data.SqlClient.SqlParameter(paraName, System.Data.SqlDbType.NText);
                default:
                    return new System.Data.SqlClient.SqlParameter(paraName, System.Data.SqlDbType.VarChar);
            }
        }

        //private System.Data.OracleClient.OracleParameter GetOraclePara(string ParaName, string DataType)
        //{
        //    switch (DataType.ToLower())
        //    {
        //        case "decimal":
        //            return new System.Data.OracleClient.OracleParameter(ParaName, Oracle.ManagedDataAccess.Client.OracleDbType.Double);

        //        case "varchar":
        //            return new System.Data.OracleClient.OracleParameter(ParaName, System.Data.OracleClient.OracleType.VarChar);

        //        case "datetime":
        //            return new System.Data.OracleClient.OracleParameter(ParaName, System.Data.OracleClient.OracleType.DateTime);

        //        case "iamge":
        //            return new System.Data.OracleClient.OracleParameter(ParaName, System.Data.OracleClient.OracleType.BFile);

        //        case "int":
        //            return new System.Data.OracleClient.OracleParameter(ParaName, System.Data.OracleClient.OracleType.Int32);

        //        case "text":
        //            return new System.Data.OracleClient.OracleParameter(ParaName, System.Data.OracleClient.OracleType.LongVarChar);

        //        default:
        //            return new System.Data.OracleClient.OracleParameter(ParaName, System.Data.OracleClient.OracleType.VarChar);

        //    }
        //}

        private System.Data.OleDb.OleDbParameter GetOleDbPara(string ParaName, string DataType)
        {
            switch (DataType)
            {
                case "decimal":
                    return new System.Data.OleDb.OleDbParameter(ParaName, System.Data.DbType.Decimal);

                case "varchar":
                    return new System.Data.OleDb.OleDbParameter(ParaName, System.Data.DbType.String);

                case "datetime":
                    return new System.Data.OleDb.OleDbParameter(ParaName, System.Data.DbType.DateTime);

                case "iamge":
                    return new System.Data.OleDb.OleDbParameter(ParaName, System.Data.DbType.Binary);

                case "int":
                    return new System.Data.OleDb.OleDbParameter(ParaName, System.Data.DbType.Int32);

                case "text":
                    return new System.Data.OleDb.OleDbParameter(ParaName, System.Data.DbType.String);

                default:
                    return new System.Data.OleDb.OleDbParameter(ParaName, System.Data.DbType.String);

            }
        }

        private MySql.Data.MySqlClient.MySqlParameter GetMysqlPara(string ParaName, string DataType, int size)
        {
            switch (DataType.ToLower())
            {
                case "decimal":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Decimal, size);

                case "varchar":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.String, size);

                case "datetime":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.DateTime);

                case "iamge":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Binary);

                case "int":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Int32, size);

                case "text":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.String);

                case "float":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Float);

                default:
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.String);
            }
        }

        private MySql.Data.MySqlClient.MySqlParameter GetMysqlPara(string ParaName, string DataType)
        {
            switch (DataType.ToLower())
            {
                case "decimal":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Decimal);

                case "varchar":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.String);

                case "datetime":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.DateTime);

                case "iamge":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Binary);

                case "int":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Int32);

                case "text":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.String);

                case "float":
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.Float);

                default:
                    return new MySql.Data.MySqlClient.MySqlParameter(ParaName, MySqlDbType.String);
            }
        }
    }
    public enum DataBaseType
    {
        mssql,
        oracal,
        mysql,
        odbc,
        oledb
    }
}
