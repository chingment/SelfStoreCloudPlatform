using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{
    public interface IDBOptionBySqlSentence : IDBOption
    {

        /// <summary>
        /// 获取分页查询SQL语句
        /// </summary>
        /// <param name="pagingSql">需分页SQL</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页Size</param>
        /// <returns>分页语句</returns>
        string GetPagingQuery(string pagingSql, int currentPage, int pageSize);

        /// <summary>
        /// 查询影响行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数</param>
        /// <returns>影响行数</returns>
        int GetCount(string sql, DatabaseParameter[] values);


        #region "执行简单的sql语句"

        int ExecuteSql(string SQLString);
        int ExecuteSqlByTime(string SQLString, int Times);
        int ExecuteSql(DatabaseParameter[] parameters, string storedProcName);
        int ExecuteSql(string CommandText, DatabaseParameter[] Parameter);
        int ExecuteSql(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters);
        object ExecuteSqlByProc(string ProName, DatabaseParameter[] values);
        Hashtable ExecuteSqlByProcWithOutPut(string ProName, DatabaseParameter[] values);
        #endregion

        #region ExecuteSqlTran操作
        bool ExecuteSqlTran(string[] sqlStrs);
        bool ExecuteSqlTran(Hashtable SQLStringList);
        bool ExecuteSqlTran(ArrayList SQLStringList);
        bool ExecuteSqlTran(Dictionary<string, object> SQLStringList);
        #endregion

        #region  ExecuteScalar操作
        object ExecuteScalar(string CommandText);

        object ExecuteScalar(CommandType cmdType, string CommandText, DatabaseParameter[] Parameter);

        object ExecuteScalar(string CommandText, DatabaseParameter[] Parameter);

        object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters);
        #endregion

        #region ExecuteDataSet操作
        DataSet GetDataSet(String CommandText);
        DataSet GetDataSet(String CommandText, DatabaseParameter[] Parameters);
        DataSet GetDataSet(string ConnectionString, String CommandText);
        DataSet GetDataSet(CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters);
        DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params DatabaseParameter[] commandParameters);
        DataSet GetDataSetByProc(string ProName, DatabaseParameter[] cmdParms);
        #endregion

        #region 分页操作
        DataSet GetPageReocrdByProc(ref  QueryParam model);
        DataSet GetPageReocrdByProcLargeData(ref  QueryParam model);
        DataSet GetPageReocrdBySql(string sql, int pageIndex, int pageSize, out int record);
        #endregion

    }
}
