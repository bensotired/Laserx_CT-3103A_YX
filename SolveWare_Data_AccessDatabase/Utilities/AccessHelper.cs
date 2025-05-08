//添加两个com组件引用
//Microsoft ADO Ext. 2.8 for DDL and Security
//Microsoft ActiveX Data Objects 2.8 Library

//ReadME:
//我们目前常用的Access数据库版本中， 有JET和ACE引擎。所以我们看到的后缀也有mdb和accdb之分

//JET引擎能访问 Office 97-Office 2003，但不能访问 Office 2007及以上的版本
//ACE 引擎是随 Office 2007 一起发布的数据库连接组件，既能访问 Office 2007及以后的版本，也能访问早期的 Office 97-Office 2003。

//JET引擎只有32位的版本
//ACE引擎有32位、64位的版本
//ACE64位的引擎可以连接32位和64位的access数据库
//一个操作系统只能安装一种位数的ACE，安装64位需要卸载32位
//64位的应用程序只能调用64位的引擎、32位的应用程序只能调用32位的引擎
//64位应用程序需要连接64位和32位access就必须用64位的ACE引擎

//数据库访问时注意：Microsoft.Jet.OLEDB.4.0（JET引擎）和Microsoft.ACE.OLEDB.12.0（ACE 引擎）。
//另外：Microsoft.ACE.OLEDB.12.0 能访问正在打开的 Excel 文件，而 Microsoft.Jet.OLEDB.4.0 则不允许

using ADOX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace SolveWare_Data_AccessDatabase.Utilities
{
    public abstract class AccessHelper
    {
        public static List<string> DBLocalFilter { get; set; }
        public static OleDbConnection m_conn = new OleDbConnection();
        
        public static string DATABASE = string.Empty;//@"D:\product.mdb";//"D:\AccessDB\LX_DB.mdb";// AppDomain.CurrentDomain.BaseDirectory + "rate.accdb";

        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.    
        public static string connectionString = "连接字符串";//Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\product.mdb

        //up 20220621 增加开关控制两种数据库访问方式
        public static bool IsUseConnectType1 = true;
        private static string ProviderMode1 = "Provider=Microsoft.ACE.OLEDB.12.0;";
        private static string ProviderMode2 = "Provider=Microsoft.Jet.OLEDB.4.0;";
        public const string ConfigFileName = @"DBFilter.xml";
        public static bool IsConnected
        {
            get
            {
                try
                {
                    if (m_conn?.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        OpenConnect();

                        return m_conn.State == ConnectionState.Open;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool OpenConnect()
        {
            if (m_conn == null || m_conn.State != ConnectionState.Open)
            {
                m_conn = new OleDbConnection();
                //string connStr = "Provider = Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;Jet OLEDB:Database Password = '" 
                //    + PassWord + "';User ID = " + UserName + ";Data Source = " + strPath;

                //Microsoft.Jet.OLEDB.4.0是连接access2003等数据库使用的。
                //connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;";//"Provider = Microsoft.ACE.OLEDB.12.0;";

                //Microsoft.ACE.OLEDB.12.0是连接access2007之后的数据库使用的
                if (IsUseConnectType1)
                {
                    connectionString = $"{ProviderMode1}Persist Security Info=true;";
                }
                else
                {
                    connectionString = $"{ProviderMode2}Persist Security Info=true;";
                }

                connectionString += "Data Source = " + DATABASE + ";";
                //connstr += "User ID=Admin;Password=laserx;";
                //connstr += "Connect Timeout=30;";
                m_conn.ConnectionString = connectionString;

                m_conn.Open();
            }

            return m_conn.State == ConnectionState.Open;
        }
        public static bool OpenConnect(string dataBasePath,out string msg)
        {
            msg = string.Empty;
            try
            {
                if (m_conn == null || m_conn.State != ConnectionState.Open)
                {
                    m_conn = new OleDbConnection();
                    //string connStr = "Provider = Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;Jet OLEDB:Database Password = '" 
                    //    + PassWord + "';User ID = " + UserName + ";Data Source = " + strPath;

                    //Microsoft.Jet.OLEDB.4.0是连接access2003等数据库使用的。
                    //connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=true;";//"Provider = Microsoft.ACE.OLEDB.12.0;";

                    //Microsoft.ACE.OLEDB.12.0是连接access2007之后的数据库使用的
                    if (IsUseConnectType1)
                    {
                        connectionString = $"{ProviderMode1}Persist Security Info=true;";
                    }
                    else
                    {
                        connectionString = $"{ProviderMode2}Persist Security Info=true;";
                    }

                    connectionString += "Data Source = " + dataBasePath + ";";
                    //connstr += "User ID=Admin;Password=laserx;";
                    //connstr += "Connect Timeout=30;";
                    m_conn.ConnectionString = connectionString;

                    m_conn.Open();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
            }

            return m_conn.State == ConnectionState.Open;
        }

        public static DataTable QueryDataTable(string strsql)
        {
            DataTable dt = null;
            OleDbCommand cmd = null;
            OleDbDataAdapter ad = null;
            try
            {
                OpenConnect();

                lock (dt = new DataTable())
                {
                    cmd = new OleDbCommand(strsql, m_conn);
                    ad = new OleDbDataAdapter((OleDbCommand)cmd);
                    dt.Clear();
                    ad.Fill(dt);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Dispose();
            }
            return dt;
        }

        public static bool TableExists(string tableName)
        {
            OpenConnect();

            bool bExist = false;
            try
            {
                using (DataTable dt = m_conn.GetSchema("Tables"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (string.Equals(tableName, dr[2].ToString()))
                        {
                            bExist = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

            return bExist;
        }

        public static int ExecteNonQueryText(string sql, params OleDbParameter[] ps)
        {
            int rowCount = -1;
            OpenConnect();
            try
            {

                OleDbCommand cmd = new OleDbCommand();
                //lock ( cmd = new OleDbCommand())
                {
                    cmd.Connection = m_conn;
                    cmd.CommandText = sql;
                    foreach (OleDbParameter p in ps)
                    {
                        cmd.Parameters.Add(p);
                    }
                    rowCount = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
            return rowCount;
        }

        public static DataTable ExecuteDataTable(string connecttionString, string sql, params OleDbParameter[] pms)
        {
            OpenConnect();

            DataTable dt = new DataTable();
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(sql, m_conn.ConnectionString))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.CommandTimeout = 0;
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }

        public static int ExecteNonQuery(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandTimeout = 0;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {

                //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                //清空SqlCommand中的参数列表
                cmd.Parameters.Clear();
                return val;
            }
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] cmdParms)
        {
            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            cmd.CommandTimeout = 0;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 创建access数据库
        /// </summary>
        /// <param name="filePath">数据库文件的全路径，如 D:\\NewDb.mdb</param>
        public static bool CreateAccessDb(string filePath, out string msg)
        {
            ADOX.Catalog catalog = new Catalog();
            msg = string.Empty;
            var fPath = Path.GetFullPath(filePath);
            if (Directory.Exists(Path.GetDirectoryName(fPath)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fPath));
            }

            if (!File.Exists(fPath))
            {
                try
                {

                    if (IsUseConnectType1)
                    {
                        catalog.Create($"{ProviderMode1}Data Source=" + fPath + ";Jet OLEDB:Engine Type=5");
                    }
                    else
                    {
                        catalog.Create($"{ProviderMode2}Data Source=" + fPath + ";Jet OLEDB:Engine Type=5");
                    }
                }
                catch (Exception ex)
                {
                    msg = $"filePath:{fPath}-CreateAccessDb ex:{ex.Message}-{ex.StackTrace} [{fPath}].";
                    return false;
                }
            }


            //if (!File.Exists(filePath))
            //{
            //    try
            //    {
            //        //Microsoft.ACE.OLEDB.12.0是连接access2007之后的数据库使用的
            //        if (IsUseConnectType1)
            //        {
            //            catalog.Create($"{ProviderMode1}Data Source=" + filePath + ";Jet OLEDB:Engine Type=5");
            //        }
            //        else
            //        {
            //            catalog.Create($"{ProviderMode2}Data Source=" + filePath + ";Jet OLEDB:Engine Type=5");
            //        }
            //        //Microsoft.Jet.OLEDB.4.0是连接access2003等数据库使用的。
            //        //catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Jet OLEDB:Engine Type=5");
            //    }
            //    catch (Exception ex)
            //    {
            //        msg = $"filePath:{filePath}-CreateAccessDb ex:{ex.Message}-{ex.StackTrace} [{filePath}].";
            //        return false;
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 在access数据库中创建表
        /// </summary>
        /// <param name="filePath">数据库表文件全路径如D:\\NewDb.mdb 没有则创建 </param>
        /// <param name="tableName">表名</param>
        /// <param name="colums">ADOX.Column对象数组</param>
        //public static void CreateAccessTable(string filePath, string tableName, params ADOX.Column[] colums)
        //{
        //    ADOX.Catalog catalog = new Catalog();
        //    //数据库文件不存在则创建
        //    if (!File.Exists(filePath))
        //    {
        //        try
        //        {
        //            if (IsUseConnectType1)
        //            {
        //                catalog.Create($"{ProviderMode1}Data Source=" + filePath);
        //            }
        //            else
        //            {
        //                catalog.Create($"{ProviderMode2}Data Source=" + filePath);
        //            }
        //            //catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Jet OLEDB:Engine Type=5");
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    ADODB.Connection cn = new ADODB.Connection();
        //    cn.Open("Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" + filePath, null, null, -1);
        //    catalog.ActiveConnection = cn;
        //    ADOX.Table table = new ADOX.Table();
        //    table.Name = tableName;
        //    foreach (var column in colums)
        //    {
        //        table.Columns.Append(column);
        //    }
        //    //column.ParentCatalog = catalog;
        //    //column.Properties["AutoIncrement"].Value = true; //设置自动增长
        //    //table.Keys.Append("FirstTablePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null); //定义主键
        //    catalog.Tables.Append(table);
        //    cn.Close();
        //}
        /// <summary>
        /// 在access数据库中创建表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="listColumns">列名集合</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CreateAccessTable(string tableName,List<string> listColumns,out string msg)
        {
            string sqlCmd = string.Format("CREATE TABLE [{0}] ([ID] AUTOINCREMENT(1,1),", tableName);
            msg = string.Empty;
            try
            {
                foreach (string param in listColumns)
                {
                    if (param.Contains("Theta") || param.Contains("PD_Reading"))
                    {
                        sqlCmd += string.Format("[{0}] Memo,", param);//字段类型支持最长65535
                    }
                    else if (param.Contains("Time"))
                    {
                        sqlCmd += string.Format("[{0}] DateTime null,", param);
                    }
                    else
                    { 
                        sqlCmd += string.Format("[{0}] varchar(200) NULL,", param);
                    }
                }
                sqlCmd = sqlCmd.Trim(',');
                sqlCmd += ")";
                ExecteNonQueryText(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                msg = $"sql:{sqlCmd}-ex:{ex.Message}-{ex.StackTrace}";
                return false;
            }
        }

        /// <summary>
        /// 获取Access数据库中指定表的所有列
        /// </summary>
        public static List<string> GetExcelTableColumn(string tableName)
        {
            //获取表名
            string tblName = tableName.Trim();
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(tblName))
            {
                return list;
            }

            try
            {
                OpenConnect();

                //获取表中的所有列信息
                DataTable schemaTable = m_conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tblName, null });

                //获取到列名称
                foreach (DataRow row in schemaTable.Rows)
                {
                    list.Add(row["COLUMN_NAME"].ToString());
                }
            }
            catch (Exception exc)
            {
                //PublicMethod.MessageError("加载Access文件过程发生异常，请重试！");
            }
            finally
            {
                Dispose();
            }

            return list;
        }
        /// <summary>
        /// 按表中顺序获取表中所有列
        /// </summary>
        /// <param name="tablenaem">表名</param>
        /// <returns></returns>
        private List<string> GetTableColumn_Ordered(string tablenaem)
        {
            string tblName = tablenaem.Trim();
            List<string> listColumn = new List<string>();
            try
            {
                OpenConnect();
                string sql = "SELECT * FROM " + tblName;
                OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, m_conn);
                DataSet dataSet = new DataSet();
                thisAdapter.Fill(dataSet, tblName);
                foreach (DataTable item in dataSet.Tables)
                {
                    foreach (DataColumn column in dataSet.Tables[item.TableName].Columns)
                    {
                        listColumn.Add(column.ColumnName);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Dispose();
            }

            return listColumn;
        }
        public static List<string> GetDBAllTable()
        {
            List<string> list = new List<string>();
            try
            {
                OpenConnect();

                //获取表中的所有列信息
                DataTable schemaTable = m_conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                //获取到列名称
                foreach (DataRow row in schemaTable.Rows)
                {
                    list.Add(row["TABLE_NAME"].ToString());
                }
            }
            catch (Exception exc)
            {
            }
            finally
            {
                Dispose();
            }

            return list;
        }


        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (OleDbException Ex)
                    {
                        connection.Close();
                        throw new Exception(Ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>    
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.OleDb.OleDbException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand(SQLString, connection);
                System.Data.OleDb.OleDbParameter myParameter = new System.Data.OleDb.OleDbParameter("@content", OleDbType.VarChar);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OleDb.OleDbException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                System.Data.OleDb.OleDbParameter myParameter = new System.Data.OleDb.OleDbParameter("@fs", OleDbType.Binary);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OleDb.OleDbException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString,out string msg)
        {
            msg = string.Empty;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OleDb.OleDbException ex)
                    {
                        msg = $"ex:{ex.Message}-{ex.StackTrace}";
                        connection.Close();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string strSQL)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                try
                {
                    connection.Open();
                    OleDbDataReader myReader = cmd.ExecuteReader();
                    return myReader;
                }
                catch (System.Data.OleDb.OleDbException e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet QueryDataSet(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }


        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            OleDbParameter[] cmdParms = (OleDbParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string SQLString, params OleDbParameter[] cmdParms)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OleDbDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.OleDb.OleDbException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
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
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        #region 操作Access
        //private static string DataPath = @"D:\test.mdb";
        //private static string TableName = "NewTable";
        //private static string conStr = @"Provider = Microsoft.ACE.OLEDB.12.0;Data Source=D:\test.mdb;";
        public static void DataBase(string filePath)
        {
            ADOX.Catalog catalog = new Catalog();
            
            if (!File.Exists(filePath))
            {
                //catalog.Create(@"Provider = Microsoft.Jet.OLEDB.4.0;Data Source=D:\test.mdb;Jet OLEDB:Engine Type = 5");
                catalog.Create($"Provider = Microsoft.ACE.OLEDB.12.0;Data Source={filePath}");
            }

        }
        public static void CreateTable(string filePath,string TableName,string[] ColumnName)
        {
            string conStr = $"Provider = Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";
            //string conStr = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\test.mdb";
            OleDbConnection conn = new OleDbConnection(conStr);
            conn.Open();
            bool bExist = false;
            using (DataTable dt = conn.GetSchema("Tables"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.Equals(TableName, dr[2].ToString()))
                    {
                        bExist = true;
                        break;
                    }
                }
            }
            if (!bExist)
            {
                //string dbstr = $"CREATE TABLE {TableName}(Batch varchar(255) ,FixtureID varchar(255),Station varchar(255),Operator varchar(255),PartNumber varchar(255),SerialNumber varchar(255),CarrierID varchar(255),WorkOrder varchar(255),FailureCode varchar(255),Posittion varchar(255),IsActive varchar(255))";
                string dbstr = $"CREATE TABLE {TableName}([ID] AUTOINCREMENT(1,1),";
                for (int i = 0; i < ColumnName.Length; i++)
                {
                    if (ColumnName[i].Contains("Time"))
                    {
                        dbstr += $"[{ColumnName[i]}] DateTime NULL,";
                    }
                    else
                    {
                        dbstr += $"[{ColumnName[i]}] varchar(255),";
                    }
                        
                }
                dbstr = dbstr.Trim(',');
                dbstr += " )";
                OleDbCommand oleDbCom = new OleDbCommand(dbstr, conn);

                oleDbCom.ExecuteNonQuery();

            }
            conn.Close();
        }
        public static void UpDataTable(string filePath, string TableName, string sqlHeader, string sqlValues, Dictionary<string,object> summarydata, string SerialNumber, string StartTime)
        {
            string[] namekt = sqlHeader.Split(",".ToCharArray());
            string[] valuekt = sqlValues.Split(",".ToCharArray());


            //bool isPass = false;
            string InSql = string.Empty;
            //string ColName = string.Empty;
            string conSt = $"Provider = Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";
            OleDbConnection conn = new OleDbConnection(conSt);
            conn.Open();
            foreach (var kvp in summarydata)
            {
                string sqlAlter = string.Empty;
       
                var summaryName = kvp.Key;
                var summaryVal = kvp.Value;
                object[] oa = { null, null, TableName, summaryName };
                DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, oa);
                if (schemaTable.Rows.Count == 0)
                {

                    if (summaryName.Contains("Time"))
                    {
                        sqlAlter = $"ALTER TABLE {TableName} ADD COLUMN [{summaryName}] DateTime NULL";
                    }
                    else
                    {
                        sqlAlter = $"ALTER TABLE {TableName} ADD COLUMN [{summaryName}] varchar(255)";
                    }

                    OleDbCommand oleDbAlter = new OleDbCommand(sqlAlter, conn);
                    oleDbAlter.ExecuteNonQuery();
                }
            }
            String sqlSE = $"SELECT * from {TableName}  WHERE [SerialNumber] = '{SerialNumber}'  AND [CreateStartTime] = #{StartTime}#";
            OleDbCommand oleDbSE = new OleDbCommand(sqlSE, conn);
            OleDbDataReader sdr = oleDbSE.ExecuteReader();
            if (sdr.HasRows)
            {
                foreach (var kvp in summarydata)
                {
                    var summaryName = kvp.Key;
                    var summaryVal = kvp.Value;
                    var sqlUPDate = $"UPDATE {TableName} SET [{summaryName}] = '{summaryVal}' WHERE [SerialNumber] = '{SerialNumber}'  AND [CreateStartTime] = #{StartTime}#";
                    OleDbCommand oleDbUPDate = new OleDbCommand(sqlUPDate, conn);
                    oleDbUPDate.ExecuteNonQuery();
                }
                for (int i = 0; i < namekt.Length; i++)
                {
                    var sqlUPDate = $"UPDATE {TableName} SET [{namekt[i]}] = {valuekt[i]} WHERE [SerialNumber] = '{SerialNumber}'  AND [CreateStartTime] = #{StartTime}#";
                    OleDbCommand oleDbUPDate = new OleDbCommand(sqlUPDate, conn);
                    oleDbUPDate.ExecuteNonQuery();
                }
                conn.Close();
            }
            else
            {
                string str = string.Empty;
                string Sql = string.Empty;
                foreach (var kvp in summarydata)
                {
                    var summaryName = kvp.Key;
                    var summaryVal = kvp.Value;
                    str += "," + "[" + summaryName + "]";
                    Sql += " ," + "'" + summaryVal + "'";
                }
                InSql = $" INSERT INTO {TableName} ({sqlHeader}{str}) VALUES (";
                InSql += sqlValues;
                InSql += Sql;
                InSql += " )";
                OleDbCommand oleDbInsert = new OleDbCommand(InSql, conn);
                oleDbInsert.ExecuteNonQuery();
                conn.Close();
            }
        }
       

        public static double DataContrast(string filePath, string TableName, string columnName, double columnValue)
        {
            try
            {
                DataTable dt = new DataTable();
                string conSt = $"Provider = Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";
                //string conSt = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = D:\test.mdb";
                OleDbConnection conn = new OleDbConnection(conSt);
                conn.Open();

                string sql = $"SELECT [{columnName}] FROM {TableName}";
                string beforeSummary = null;
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataReader dbDataReader = cmd.ExecuteReader();
                while (dbDataReader.Read())
                {
                    if (dbDataReader[columnName].ToString() != "")
                    {
                        beforeSummary = dbDataReader[columnName].ToString();
                        break;
                    }
                }

                var beforeSummaryValue = double.Parse(beforeSummary);
                dbDataReader.Close();
                conn.Close();
                return columnValue - beforeSummaryValue;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
        public static void Dispose()
        {
            m_conn.Close();
            m_conn.Dispose();
            m_conn = null;
        }
    }

}
