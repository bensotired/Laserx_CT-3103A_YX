using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Windows.Forms;
using SolveWare_TestDatabase.Interface;

namespace SolveWare_TestDatabase.Utility
{
    public class DataBaseHelper
    {
        #region "公用方法"

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>是否存在</returns>
        public static bool Exists(string strSql)
        {
            object obj = DataBaseHelper.GetSingle(strSql);
            int cmdResult = 0;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                cmdResult = 0;
            else
                cmdResult = int.Parse(obj.ToString());

            if (cmdResult == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>是否存在</returns>
        public static bool Exists(string strSql, params IDbDataParameter[] cmdParms)
        {
            object obj = DataBaseHelper.GetSingle(strSql, cmdParms);
            int cmdResult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                cmdResult = 0;
            else
                cmdResult = int.Parse(obj.ToString());

            if (cmdResult == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 获取分页SQL语句，默认row_number为关健字，所有表不允许使用该字段名
        /// </summary>
        /// <param name="recordCount">记录总数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="safeSql">SQL查询语句</param>
        /// <param name="orderField">排序字段，多个则用“,”隔开</param>
        /// <returns>分页SQL语句</returns>
        public static string CreatePagingSql(int recordCount, int pageSize, int pageIndex, string safeSql, string orderField)
        {
            DBFactory factory = new DBFactory();
            IPagingHelper myDBFactory = factory.CreatePagingInstance();

            return myDBFactory.CreatePagingSql(recordCount, pageSize, pageIndex, safeSql, orderField);
        }

        /// <summary>
        /// 获取记录总数SQL语句
        /// </summary>
        /// <param name="safeSql">SQL查询语句</param>
        /// <returns>记录总数SQL语句</returns>
        public static string CreateCountingSql(string safeSql)
        {
            DBFactory factory = new DBFactory();
            IPagingHelper myDBFactory = factory.CreatePagingInstance();
            return myDBFactory.CreateCountingSql(safeSql);
        }

        /// <summary>
        /// 创建SQL参数
        /// </summary>
        /// <param name="paraName">参数列表</param>
        /// <param name="sqlType">SQLType</param>
        /// <param name="size">大小</param>
        /// <returns>IDbDataParameter</returns>
        public static IDbDataParameter CreateDBParameter(string paraName, SqlDbType sqlType, int size)
        {
            DBFactory factory = new DBFactory();
            return factory.CreateDBParameter(paraName, sqlType, size);
        }

        /// <summary>
        /// 创建SQL参数
        /// </summary>
        /// <param name="paraName">参数列表</param>
        /// <param name="sqlType">SQLType</param>
        /// <returns>IDbDataParameter</returns>
        public static IDbDataParameter CreateDBParameter(string paraName, SqlDbType sqlType)
        {
            DBFactory factory = new DBFactory();
            return factory.CreateDBParameter(paraName, sqlType);
        }
        /// <summary>
        /// 创建SQL参数
        /// </summary>
        /// <param name="paraName">参数列表</param>
        /// <param name="sqlType">参数类型</param>
        /// <returns>IDbDataParameter</returns>
        public static IDbDataParameter CreateDBParameter(string paraName, string sqlType)
        {
            DBFactory factory = new DBFactory();
            return factory.CreateDBParameter(paraName, sqlType);
        }


        //DataBaseHelper.CreateDBParameter("?_PLCNo","varChar",100)

        public static IDbDataParameter CreateDBParameter(string paraName, string sqlType, int size)
        {
            DBFactory factory = new DBFactory();
            return factory.CreateDBParameter(paraName, sqlType, size);

        }
        #endregion

        #region "执行简单SQL语句"

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>查询结果（object)</returns>
        public static object GetSingle(string sql)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                            return null;
                        else
                            return obj;
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeout">执行SQL超时时间</param>
        /// <returns>查询结果（object)</returns>
        public static object GetSingle(string sql, int timeout)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.CommandTimeout = timeout;
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                            return null;
                        else
                            return obj;
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行简单SQL语句（Insert/Update/Delete)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteSql(string sql)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行简单SQL语句（Insert/Update/Delete)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeout">执行SQL超时时间</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteSql(string sql, int timeout)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.CommandTimeout = timeout;
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，实现事务
        /// </summary>
        /// <param name="sqlList">SQL语句列表</param>
        /// <returns>返回受影响行数</returns>
        public static int ExecuteSqlTran(List<String> sqlList)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                connection.Open();
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    cmd.Connection = connection;
                    using (IDbTransaction tx = myDBFactory.CreateTransaction(connection))
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            int count = 0;
                            for (int n = 0; n < sqlList.Count; n++)
                            {
                                string strsql = sqlList[n];
                                if (strsql.Trim().Length > 1)
                                {
                                    cmd.CommandText = strsql;
                                    count += cmd.ExecuteNonQuery();
                                }
                            }
                            tx.Commit();
                            return count;
                        }
                        catch
                        {
                            tx.Rollback();
                            return 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回数据集</returns>
        public static DataSet Query(string sql)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                DataSet ds = new DataSet();
                IDbDataAdapter adpter = null;
                try
                {
                    connection.Open();
                    using (IDbCommand cmd = myDBFactory.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Connection = connection;
                        adpter = myDBFactory.CreateDataAdapter();
                        adpter.SelectCommand = cmd;

                        adpter.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    GC.SuppressFinalize(adpter);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行SQL语句查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeout">执行SQL超时时间</param>
        /// <returns>返回数据集</returns>
        public static DataSet Query(string sql, int timeout)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                DataSet ds = new DataSet();
                IDbDataAdapter adpter = null;
                try
                {
                    connection.Open();
                    using (IDbCommand cmd = myDBFactory.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Connection = connection;
                        cmd.CommandTimeout = timeout;
                        adpter = myDBFactory.CreateDataAdapter();
                        adpter.SelectCommand = cmd;

                        adpter.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    GC.SuppressFinalize(adpter);
                }
                return ds;
            }
        }

        #endregion

        #region "执行带参数的"

        /// <summary>
        /// 执行带参数的SQL语句（Insert/Update/Delete)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteSql(string sql, params IDbDataParameter[] cmdParms)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sql, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行带参数的SQL语句（Insert/Update/Delete)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeout">SQL执行超时时间</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteSql(string sql, int timeout, params IDbDataParameter[] cmdParms)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    try
                    {
                        cmd.CommandTimeout = timeout;
                        PrepareCommand(cmd, connection, null, sql, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 批量执行SQL语句并实现事务
        /// </summary>
        /// <param name="sqlList">（Key为SQL语句，Value为参数列表）</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteSqlTran(Hashtable sqlList)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                connection.Open();
                using (IDbTransaction trans = myDBFactory.CreateTransaction(connection))
                {
                    IDbCommand cmd = myDBFactory.CreateCommand();
                    try
                    {
                        int count = 0;
                        //循环
                        foreach (DictionaryEntry myDE in sqlList)
                        {
                            string cmdText = myDE.Key.ToString();
                            IDbDataParameter[] cmdParms = (IDbDataParameter[])myDE.Value;
                            PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            count += val;
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();

                        return count;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string strSql, params IDbDataParameter[] cmdParms)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, strSql, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                            return null;
                        else
                            return obj;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="timeout">执行SQL语句超时时间</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string strSql, int timeout, params IDbDataParameter[] cmdParms)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    try
                    {
                        cmd.CommandTimeout = timeout;
                        PrepareCommand(cmd, connection, null, strSql, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                            return null;
                        else
                            return obj;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 执行带参数的SQL语句查询
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>数据集</returns>
        public static DataSet Query(string strSql, params IDbDataParameter[] cmdParms)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                using (IDbCommand cmd = myDBFactory.CreateCommand())
                {
                    PrepareCommand(cmd, connection, null, strSql, cmdParms);
                    IDbDataAdapter da = myDBFactory.CreateDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        GC.SuppressFinalize(da);
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// 准备参数
        /// </summary>
        /// <param name="cmd">SQL命令对象</param>
        /// <param name="conn">SQL连接对象</param>
        /// <param name="trans">SQL事务对象</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="cmdParms">查询参数</param>
        private static void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, string cmdText, IDbDataParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (IDbDataParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region "执行存储过程操作"
        /// <summary>
        /// 执行存储过程查询，返回数据集
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>数据集</returns>
        public static DataSet RunProcedure(string storedProcName, IDbDataParameter[] parameters)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                DataSet dataSet = new DataSet();
                IDbDataAdapter sqlDA = null;
                try
                {
                    connection.Open();
                    sqlDA = myDBFactory.CreateDataAdapter();
                    using (IDbCommand cmd = BuildQueryCommand(myDBFactory, connection, storedProcName, parameters))
                    {
                        sqlDA.SelectCommand = cmd;
                        sqlDA.Fill(dataSet);
                        connection.Close();
                    }
                    return dataSet;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                finally
                {
                    GC.SuppressFinalize(sqlDA);
                }
            }
        }

        /// <summary>
        /// 执行存储过程查询，返回数据集
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>数据集</returns>
        public static DataSet RunProcedure(string storedProcName, IDbDataParameter[] parameters, int timeout)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                DataSet dataSet = new DataSet();
                IDbDataAdapter sqlDA = null;
                try
                {
                    connection.Open();
                    sqlDA = myDBFactory.CreateDataAdapter();
                    using (IDbCommand cmd = BuildQueryCommand(myDBFactory, connection, storedProcName, parameters))
                    {
                        cmd.CommandTimeout = timeout;
                        sqlDA.SelectCommand = cmd;
                        sqlDA.Fill(dataSet);
                        connection.Close();
                    }

                    return dataSet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    GC.SuppressFinalize(sqlDA);
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="rowsAffected">影响行数</param>
        /// <returns>Return Value</returns>
        public static int RunProcedure(string storedProcName, IDbDataParameter[] parameters, out int rowsAffected)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                int result;
                connection.Open();

                using (IDbCommand command = BuildIntCommand(myDBFactory, connection, storedProcName, parameters))
                {
                    rowsAffected = command.ExecuteNonQuery();
                    IDbDataParameter p = (IDbDataParameter)command.Parameters["ReturnValue"];
                    result = (int)p.Value;
                }

                return result;
            }
        }

        /// <summary>
        /// 执行存储过程，返回受影响的行数
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>受影响行数</returns>
        public static int RunProcedureInt(string storedProcName, IDbDataParameter[] parameters)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                int result;
                connection.Open();

                using (IDbCommand command = BuildIntCommand(myDBFactory, connection, storedProcName, parameters))
                {
                    result = command.ExecuteNonQuery();
                }

                return result;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        public static void RunProcedureVoid(string storedProcName, IDbDataParameter[] parameters)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    IDbCommand command = BuildIntCommand(myDBFactory, connection, storedProcName, parameters);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProceName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回查询结果</returns>
        public static object RunProcedureReturnObj(string storedProceName, IDbDataParameter[] parameters)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    using (IDbCommand command = BuildIntCommand(myDBFactory, connection, storedProceName, parameters))
                    {
                        return command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>Return Value</returns>
        public static int GetReturnValueRunProcedure(string storedProcName, IDbDataParameter[] parameters)
        {
            DBFactory factory = new DBFactory();
            IDBFactory myDBFactory = factory.CreateInstance();
            using (IDbConnection connection = myDBFactory.CreateConnection(myDBFactory.GetConnectionString()))
            {
                try
                {
                    int result;
                    connection.Open();
                    using (IDbCommand command = BuildIntCommand(myDBFactory, connection, storedProcName, parameters))
                    {
                        command.ExecuteNonQuery();
                        IDbDataParameter p = (IDbDataParameter)command.Parameters["ReturnValue"];
                        result = (int)p.Value;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 构建查询SQL命令对象
        /// </summary>
        /// <param name="myDBFactory">工厂实体类</param>
        /// <param name="connection">SQL连接对象</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>SQL命令对象</returns>
        private static IDbCommand BuildQueryCommand(IDBFactory myDBFactory, IDbConnection connection, string storedProcName, IDbDataParameter[] parameters)
        {
            IDbCommand command = myDBFactory.CreateCommand();
            command.CommandText = storedProcName;
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = connection;


            if (parameters != null)
            {
                foreach (IDbDataParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
            }

            return command;
        }

        /// <summary>
        /// 构建执行动作SQL命令对象
        /// </summary>
        /// <param name="myDBFactory">工厂实体类</param>
        /// <param name="connection">SQL连接对象</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>SQL命令对象</returns>
        private static IDbCommand BuildIntCommand(IDBFactory myDBFactory, IDbConnection connection, string storedProcName, IDbDataParameter[] parameters)
        {
            IDbCommand command = myDBFactory.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcName;
            command.Connection = connection;

            if (parameters != null)
            {
                foreach (IDbDataParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
            }

            command.Parameters.Add(myDBFactory.GetReturnValuePara());
            return command;
        }

        public static int ExecuteNonQuery(IDBFactory myDBFactory, IDbTransaction trans, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = myDBFactory.CreateCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        #endregion
    }
}
