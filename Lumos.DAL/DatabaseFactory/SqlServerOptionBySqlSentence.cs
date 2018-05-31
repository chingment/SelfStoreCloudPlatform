using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{
    internal class SqlServerOptionBySqlSentence : IDBOptionBySqlSentence
    {

        /// <summary>
        ///检查记录是否存在
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public Boolean CheckDataExists(string strSQL)
        {
            using (SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {

                bool bFlag = false;
                SqlCommand SQLcmd = conn.CreateCommand();
                SQLcmd.CommandText = strSQL;
                SqlDataReader SQLreader = SQLcmd.ExecuteReader();
                SQLreader.Read();
                if (SQLreader.HasRows)
                {
                    bFlag = true;
                }
                else
                {
                    bFlag = false;
                }
                conn.Close();
                conn.Dispose();
                return bFlag;
            }
        }

        string IDBOption.ConnectionString
        {
            get;
            set;
        }


        SqlConnection dataContext;
        internal SqlServerOptionBySqlSentence()
        {
            ((IDBOptionBySqlSentence)this).ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            dataContext = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString);
        }


        IDbConnection IDBOption.Connection(string connectionString, DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    return new System.Data.SqlClient.SqlConnection(connectionString);
                default:
                    return null;
            }
        }


        /// <summary>
        /// 格式化字符串,符合SQL语句
        /// </summary>
        /// <param name="formatStr">需要格式化的字符串</param>
        /// <returns>字符串</returns>
        public static string FormatInSQL(string formatStr)
        {
            string rStr = formatStr;
            if (formatStr != null && formatStr != string.Empty)
            {
                rStr = rStr.Replace("'", "''");
                //rStr = rStr.Replace("\"", "\"\"");
            }
            return rStr;
        }


        #region IOption 成员

        /// <summary>
        /// 获取分页查询SQL语句
        /// </summary>
        /// <param name="pagingSql">需分页SQL</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页Size</param>
        /// <returns>分页语句</returns>
        string IDBOptionBySqlSentence.GetPagingQuery(string pagingSql, int currentPage, int pageSize)
        {
            //string permission = " ID IN(SELECT RecordID FROM fnGetDataPermission('" + UserContext.GetCurrentUserCode() + "','" + SystemContext.GetCurrentEntityCode() + "'))";
            //  string permission = " ID IN(SELECT RecordID FROM fnGetDataPermission('" + UserContext.GetCurrentUserCode() + "',''))";
            // pagingSql = pagingSql.Replace("#{DataPermission}", permission);
            string top = " TOP " + pageSize.ToString();
            pagingSql = pagingSql.Insert(pagingSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) + 6, top);
            if (currentPage != 1)
            {
                int where = pagingSql.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);
                int orderby = pagingSql.IndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
                int groupby = pagingSql.IndexOf("GROUP BY", StringComparison.OrdinalIgnoreCase);
                string identify;
                if (groupby == -1) //不存在Group By
                {
                    identify = " SnCode ";
                }
                else //存在Group By
                {
                    if (orderby < groupby) //Group By在Order By后
                    {
                        identify = pagingSql.Substring(groupby + 8).Split(',')[0] + " ";
                    }
                    else //Group By在Order By前
                    {
                        identify = pagingSql.Substring(groupby + 8, orderby - groupby - 9).Split(',')[0] + " ";
                    }
                }
                string childSql = identify + "NOT IN(SELECT TOP " + ((currentPage - 1) * pageSize).ToString() + identify
                    + pagingSql.Substring(pagingSql.IndexOf("FROM", StringComparison.OrdinalIgnoreCase)) + ")";
                int insertIndex;
                if (where != -1) //存在where
                {
                    insertIndex = where + 5;
                    childSql = childSql + " AND ";
                }
                else
                {
                    childSql = " WHERE" + childSql;
                    if (orderby == -1 && groupby == -1) //不存在Order By和Group By
                    {
                        insertIndex = pagingSql.Length;
                    }
                    else
                    {
                        if (orderby != -1 && groupby != -1) //同时存在Order By和Group By
                        {
                            insertIndex = orderby < groupby ? orderby : groupby;
                        }
                        else
                        {
                            insertIndex = orderby > groupby ? orderby : groupby;
                        }
                    }
                }
                pagingSql = pagingSql.Insert(insertIndex, childSql);
            }
            return pagingSql;
        }

        /// <summary>
        /// 查询影响行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数</param>
        /// <returns>影响行数</returns>
        int IDBOptionBySqlSentence.GetCount(string sql, DatabaseParameter[] values)
        {
            //string permission = " ID IN(SELECT RecordID FROM fnGetDataPermission('" + UserContext.GetCurrentUserCode() + "','" + SystemContext.GetCurrentEntityCode() + "'))";
            // string permission = " ID IN(SELECT RecordID FROM fnGetDataPermission('" + UserContext.GetCurrentUserCode() + "',''))";
            // sql = sql.Replace("#{DataPermission}", permission);
            string countSql = "SELECT COUNT(*) " + sql.Substring(sql.IndexOf("from", StringComparison.OrdinalIgnoreCase));
            int orderIndex = countSql.LastIndexOf("order", StringComparison.OrdinalIgnoreCase);
            if (orderIndex != -1)
            {
                countSql = countSql.Remove(orderIndex);
            }
            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(countSql, connection);
                if (values != null)
                {
                    SqlParameter[] databaseParameter = ParameterToDbParameter(values);
                    command.Parameters.AddRange(databaseParameter);
                }
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count;
            }
        }


        #region "执行简单的sql语句"

        int IDBOptionBySqlSentence.ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return rows;

                }
            }
        }
        int IDBOptionBySqlSentence.ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {

                    connection.Open();
                    cmd.CommandTimeout = Times;
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return rows;

                }
            }
        }
        int IDBOptionBySqlSentence.ExecuteSql(DatabaseParameter[] parameters, string storedProcName)
        {
            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                command.Dispose();
                connection.Close();
                return result;
            }
        }
        int IDBOptionBySqlSentence.ExecuteSql(string CommandText, DatabaseParameter[] Parameter)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, CommandText, Parameter);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Close();
                return val;
            }

        }
        int IDBOptionBySqlSentence.ExecuteSql(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Close();
                return val;
            }
        }

        #endregion

        #region ExecuteSqlTran操作
        bool IDBOptionBySqlSentence.ExecuteSqlTran(string[] sqlStrs)
        {
            bool check = true;
            SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString);
            SqlCommand cmd = new SqlCommand();
            using (conn)
            {
                conn.Open();
                System.Data.SqlClient.SqlTransaction trans = conn.BeginTransaction();
                using (trans)
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        foreach (string str in sqlStrs)
                        {
                            cmd.CommandText = str;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        check = false;
                        trans.Rollback();
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }
            return check;
        }
        bool IDBOptionBySqlSentence.ExecuteSqlTran(Hashtable SQLStringList)
        {
            bool val = true;
            using (SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {

                            string cmdText = myDE.Key.ToString().Split('|')[0];
                            DatabaseParameter[] cmdParms = (DatabaseParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, CommandType.Text, cmdText, cmdParms);
                            int valData = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        cmd.Dispose();
                        conn.Close();
                        val = false;
                    }
                }
            }
            return val;
        }

        bool IDBOptionBySqlSentence.ExecuteSqlTran(ArrayList SQLStringList)
        {
            bool val = true;
            using (SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {

                            string cmdText = myDE.Key.ToString().Split('|')[0];
                            DatabaseParameter[] cmdParms = (DatabaseParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, CommandType.Text, cmdText, cmdParms);
                            int valData = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        cmd.Dispose();
                        conn.Close();
                        val = false;
                    }
                }
            }
            return val;
        }

        bool IDBOptionBySqlSentence.ExecuteSqlTran(Dictionary<string, object> SQLStringList)
        {
            bool val = true;
            using (SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        foreach (KeyValuePair<string, object> entry in SQLStringList)
                        {
                            string cmdText = entry.Key.ToString().Split('|')[0];
                            DatabaseParameter[] cmdParms = (DatabaseParameter[])entry.Value;
                            PrepareCommand(cmd, conn, trans, CommandType.Text, cmdText, cmdParms);
                            int valData = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        cmd.Dispose();
                        conn.Close();
                        val = false;
                    }
                }
            }
            return val;
        }

        #endregion

        #region  ExecuteScalar操作
        object IDBOptionBySqlSentence.ExecuteScalar(string CommandText)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteScalar(CommandType.Text, CommandText, null);
        }

        object IDBOptionBySqlSentence.ExecuteScalar(CommandType cmdType, string CommandText, DatabaseParameter[] Parameter)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, CommandText, Parameter);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                cmd.Dispose();
                connection.Close();
                return val;
            }
        }

        object IDBOptionBySqlSentence.ExecuteScalar(string CommandText, DatabaseParameter[] Parameter)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                PrepareCommand(cmd, connection, null, CommandType.Text, CommandText, Parameter);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                cmd.Dispose();
                connection.Close();
                return val;
            }
        }

        object IDBOptionBySqlSentence.ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                cmd.Dispose();
                connection.Close();
                return val;
            }

        }


        #endregion

        #region ExecuteDataSet操作
        DataSet IDBOptionBySqlSentence.GetDataSet(String CommandText)
        {
            using (SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                DataSet dataSet = new DataSet();
                conn.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter(CommandText, conn);
                sqlDA.Fill(dataSet);
                conn.Close();
                return dataSet;
            }
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(String CommandText, DatabaseParameter[] Parameters)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(CommandType.Text, CommandText, Parameters);
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(string ConnectionString, String CommandText)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(ConnectionString, CommandType.Text, CommandText, null);
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        DataSet IDBOptionBySqlSentence.GetDataSetByProc(string ProName, DatabaseParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString);

            try
            {
                PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, ProName, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region 分页操作
        DataSet IDBOptionBySqlSentence.GetPageReocrdByProc(ref  QueryParam model)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@TableName", SqlDbType.VarChar,1000),
					new SqlParameter("@ReturnFields", SqlDbType.VarChar,8000),
					new SqlParameter("@PageSize", SqlDbType.Int,4),
					new SqlParameter("@PageIndex", SqlDbType.Int,4),
					new SqlParameter("@Where", SqlDbType.VarChar,8000),
                    new SqlParameter("@Orderfld", SqlDbType.VarChar,500),
                    new SqlParameter("@TotalRecord", SqlDbType.Int,4),
                    new SqlParameter("@TotalPage", SqlDbType.Int,4),
                    new SqlParameter("@Sql", SqlDbType.VarChar,8000)
            };
            parameters[0].Value = model.TableName;
            parameters[1].Value = model.ReturnFields;
            parameters[2].Value = model.PageSize;
            parameters[3].Value = model.PageIndex;
            if (model.Where == default(string) || model.Where.Trim() == "")
                parameters[4].Value = "1=1";
            else
                parameters[4].Value = model.Where;
            parameters[5].Value = model.Orderfld;
            parameters[6].Direction = ParameterDirection.Output;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Direction = ParameterDirection.Output;

            //调用分页存储过程
            SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString);
            DataSet ds = new DataSet();
            conn.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();

            SqlCommand command = new SqlCommand("PRO_LUMOS_STROPAGEPRO", conn);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
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


            sqlDA.SelectCommand = command;
            sqlDA.Fill(ds);
            conn.Close();
            string a = parameters[6].Value.ToString();
            model.TotalRecord = int.Parse(parameters[6].Value.ToString());
            model.TotalPage = int.Parse(parameters[7].Value.ToString());
            model.Sql = parameters[8].Value.ToString();
            return ds;
        }

        DataSet IDBOptionBySqlSentence.GetPageReocrdByProcLargeData(ref  QueryParam model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TabeName", SqlDbType.VarChar,1000),
					new SqlParameter("@Fields", SqlDbType.VarChar,8000),
					new SqlParameter("@pageNumber", SqlDbType.Int,4),
					new SqlParameter("@page", SqlDbType.Int,4),
					new SqlParameter("@SearchWhere", SqlDbType.VarChar,8000),
                    new SqlParameter("@OrderFields", SqlDbType.VarChar,500),
                    new SqlParameter("@TotalRecord", SqlDbType.Int,4),
            };
            parameters[0].Value = model.TableName;
            parameters[1].Value = model.ReturnFields;
            parameters[2].Value = model.PageSize;
            parameters[3].Value = model.PageIndex;
            if (model.Where == default(string) || model.Where.Trim() == "")
                parameters[4].Value = "1=1";
            else
                parameters[4].Value = model.Where;
            parameters[5].Value = model.Orderfld;
            parameters[6].Direction = ParameterDirection.Output;


            //调用分页存储过程
            SqlConnection conn = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString);
            DataSet ds = new DataSet();
            conn.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();

            SqlCommand command = new SqlCommand("im531_Page", conn);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
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


            sqlDA.SelectCommand = command;
            sqlDA.Fill(ds);
            conn.Close();
            string a = parameters[6].Value.ToString();
            model.TotalRecord = int.Parse(parameters[6].Value.ToString());
            return ds;
        }
        DataSet IDBOptionBySqlSentence.GetPageReocrdBySql(string sql, int pageIndex, int pageSize, out int record)
        {
            record = 0;
            return null;
        }
        #endregion


        static SqlParameter[] ParameterToDbParameter(DatabaseParameter[] values)
        {
            SqlParameter[] sqlParameter = new SqlParameter[values.Length];
            for (int i = 0; i < sqlParameter.Length; i++)
            {
                if (values[i].Direction == DatabaseParameterDirection.Output)
                {
                    if (values[i].DataType == DatabaseParameterDataType.Int)
                    {
                        SqlParameter sql1 = new SqlParameter();
                        sql1.ParameterName = "@" + values[i].Name;
                        sql1.Direction = ParameterDirection.Output;
                        sql1.SqlDbType = SqlDbType.Int;
                        sqlParameter[i] = sql1;
                    }
                    else if (values[i].DataType == DatabaseParameterDataType.Decimal)
                    {
                        SqlParameter sql1 = new SqlParameter();
                        sql1.ParameterName = "@" + values[i].Name;
                        sql1.Direction = ParameterDirection.Output;
                        sql1.SqlDbType = SqlDbType.Decimal;
                        sqlParameter[i] = sql1;
                    }
                    else if (values[i].DataType == DatabaseParameterDataType.Float)
                    {
                        SqlParameter sql1 = new SqlParameter();
                        sql1.ParameterName = "@" + values[i].Name;
                        sql1.Direction = ParameterDirection.Output;
                        sql1.SqlDbType = SqlDbType.Float;
                        sqlParameter[i] = sql1;
                    }
                    else
                    {
                        SqlParameter sql1 = new SqlParameter();
                        sql1.ParameterName = "@" + values[i].Name;
                        sql1.Direction = ParameterDirection.Output;
                        sql1.Value = "";
                        sqlParameter[i] = sql1;
                    }

                }
                else
                {
                    if (values[i].MaxLength != null)
                    {
                        sqlParameter[i] = new SqlParameter("@" + values[i].Name, SqlDbType.NVarChar, int.Parse(values[i].MaxLength.ToString()));
                        sqlParameter[i].Value = values[i].Value == null ? DBNull.Value : values[i].Value;
                    }
                    else
                    {
                        sqlParameter[i] = new SqlParameter("@" + values[i].Name, values[i].Value == null ? DBNull.Value : values[i].Value);
                    }
                }
            }
            return sqlParameter;
        }




        /// <summary>
        /// 提供一个SqlCommand对象的设置
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="conn">SqlConnection 对象</param>
        /// <param name="trans">SqlTransaction 对象</param>
        /// <param name="cmdType">CommandType 如存贮过程，T-SQL</param>
        /// <param name="cmdText">存贮过程名或查询串</param>
        /// <param name="cmdParms">命令中用到的参数集</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, DatabaseParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandTimeout = 300;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                SqlParameter[] databaseParameter = ParameterToDbParameter(cmdParms);
                cmd.Parameters.AddRange(databaseParameter);
            }
        }

        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, DatabaseParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, DatabaseParameter[] parameters)
        {

            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {

                SqlParameter[] databaseParameter = ParameterToDbParameter(parameters);
                command.Parameters.AddRange(databaseParameter);
            }

            return command;
        }





        object IDBOptionBySqlSentence.ExecuteSqlByProc(string ProName, DatabaseParameter[] cmdParms)
        {

            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(ProName, connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter[] databaseParameter = ParameterToDbParameter(cmdParms);
                command.Parameters.AddRange(databaseParameter);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return command.ExecuteNonQuery();

            }
        }

        Hashtable IDBOptionBySqlSentence.ExecuteSqlByProcWithOutPut(string ProName, DatabaseParameter[] cmdParms)
        {

            using (SqlConnection connection = new SqlConnection(((IDBOptionBySqlSentence)this).ConnectionString))
            {
                Hashtable ht = new Hashtable();
                connection.Open();
                SqlCommand command = new SqlCommand(ProName, connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter[] databaseParameter = ParameterToDbParameter(cmdParms);
                command.Parameters.AddRange(databaseParameter);
                command.ExecuteNonQuery();
                for (int i = 0; i < databaseParameter.Length; i++)
                {
                    if (databaseParameter[i].Direction == ParameterDirection.Output)
                    {
                        string pname = databaseParameter[i].ParameterName.Replace("@", "").ToString();
                        ht.Add(pname, command.Parameters[databaseParameter[i].ParameterName].Value.ToString());
                    }
                }
                command.Dispose();
                connection.Close();
                return ht;
            }
        }


        #endregion



    }
}
