using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{
    public class DBOptionBySqlSentenceProvider : IDBOptionBySqlSentence
    {

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        IDBOptionBySqlSentence option;
        public DBOptionBySqlSentenceProvider()
        {

            option = new DBOptionFactory().CreateOptionBySqlSentence();
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
        /// 获取分页查询SQL语句
        /// </summary>
        /// <param name="pagingSql">需分页SQL</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页Size</param>
        /// <returns>分页语句</returns>
        string IDBOptionBySqlSentence.GetPagingQuery(string pagingSql, int currentPage, int pageSize)
        {
            return null;
        }

        /// <summary>
        /// 查询影响行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数</param>
        /// <returns>影响行数</returns>
        int IDBOptionBySqlSentence.GetCount(string sql, DatabaseParameter[] values)
        {
            return 0;
        }


        #region "执行简单的sql语句"

        int IDBOptionBySqlSentence.ExecuteSql(string SQLString)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSql(SQLString);
        }
        int IDBOptionBySqlSentence.ExecuteSqlByTime(string SQLString, int Times)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSqlByTime(SQLString, Times);
        }
        int IDBOptionBySqlSentence.ExecuteSql(DatabaseParameter[] parameters, string storedProcName)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSql(parameters, storedProcName);
        }
        int IDBOptionBySqlSentence.ExecuteSql(string CommandText, DatabaseParameter[] Parameter)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSql(CommandText, Parameter);

        }
        int IDBOptionBySqlSentence.ExecuteSql(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSql(connectionString, cmdType, cmdText, commandParameters);
        }
        #endregion

        #region ExecuteSqlTran操作
        bool IDBOptionBySqlSentence.ExecuteSqlTran(string[] sqlStrs)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSqlTran(sqlStrs);
        }
        bool IDBOptionBySqlSentence.ExecuteSqlTran(Hashtable SQLStringList)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSqlTran(SQLStringList);
        }
        bool IDBOptionBySqlSentence.ExecuteSqlTran(ArrayList SQLStringList)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSqlTran(SQLStringList);
        }

        bool IDBOptionBySqlSentence.ExecuteSqlTran(Dictionary<string, object> SQLStringList)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSqlTran(SQLStringList);
        }
        #endregion

        #region  ExecuteScalar操作
        object IDBOptionBySqlSentence.ExecuteScalar(string CommandText)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteScalar(CommandText);
        }

        object IDBOptionBySqlSentence.ExecuteScalar(CommandType cmdType, string CommandText, DatabaseParameter[] Parameter)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteScalar(cmdType, CommandText, Parameter);
        }

        object IDBOptionBySqlSentence.ExecuteScalar(string CommandText, DatabaseParameter[] Parameter)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteScalar(CommandText, Parameter);
        }

        object IDBOptionBySqlSentence.ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteScalar(connectionString, cmdType, cmdText, commandParameters);

        }


        #endregion


        #region ExecuteDataSet操作
        DataSet IDBOptionBySqlSentence.GetDataSet(String CommandText)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(CommandText);
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(String CommandText, DatabaseParameter[] Parameters)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(CommandText, Parameters);
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(string ConnectionString, String CommandText)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(ConnectionString, CommandText);
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(cmdType, cmdText, commandParameters);
        }
        DataSet IDBOptionBySqlSentence.GetDataSet(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(connectionString, cmdType, cmdText, commandParameters);
        }

        DataSet IDBOptionBySqlSentence.GetDataSetByProc(string ProName, DatabaseParameter[] cmdParms)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetDataSet(CommandType.StoredProcedure, ProName, cmdParms);
        }

        #endregion

        #region 分页操作
        DataSet IDBOptionBySqlSentence.GetPageReocrdByProc(ref  QueryParam model)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetPageReocrdByProc(ref model);
        }

        DataSet IDBOptionBySqlSentence.GetPageReocrdByProcLargeData(ref  QueryParam model)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetPageReocrdByProcLargeData(ref model);
        }



        DataSet IDBOptionBySqlSentence.GetPageReocrdBySql(string sql, int pageIndex, int pageSize, out int record)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.GetPageReocrdBySql(sql, pageIndex, pageSize, out record);
        }
        #endregion


        object IDBOptionBySqlSentence.ExecuteSqlByProc(string ProName, DatabaseParameter[] values)
        {
            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSqlByProc(ProName, values);
        }

        Hashtable IDBOptionBySqlSentence.ExecuteSqlByProcWithOutPut(string ProName, DatabaseParameter[] cmdParms)
        {

            IDBOptionBySqlSentence option = new DBOptionFactory().CreateOptionBySqlSentence();
            return option.ExecuteSqlByProcWithOutPut(ProName, cmdParms);
        }

    }
}
