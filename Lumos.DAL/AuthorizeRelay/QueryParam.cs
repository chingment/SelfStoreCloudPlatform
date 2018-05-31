using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 存储过程QueryParam参数
    /// </summary>
    public class QueryParam
    {
        public QueryParam()
        {
            _tablename = "";
            _returnfields = "";
            _pagesize = 0;
            _pageindex = 0;
            _where = "";
            _orderfld = "";
            _totalrecord = 0;
            _totalpage = 0;
            _sql = "";
            _PrimaryKey = "";
        }

        private string _tablename;		//表名
        private string _returnfields;	//需要返回的列
        private int _pagesize;			// 每页记录数
        private int _pageindex;				//当前页码
        private string _where;	//查询条件
        private string _orderfld;			// 排序字段名最好为唯一主键
        private int _totalrecord;    //返回记录总数
        private int _totalpage;   //返回总页数
        private string _sql;//最后返回的SQL语句
        private string _PrimaryKey;
        private int _OrderType = 0;
        /// <summary>
        /// 排序类型 1:降序 其它为升序
        /// </summary>
        public int OrderType
        {
            get
            {
                return _OrderType;
            }
            set
            {
                _OrderType = value;
            }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string PrimaryKey
        {
            set { _PrimaryKey = value; }
            get { return _PrimaryKey; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }
        /// <summary>
        /// 需要返回的列
        /// </summary>
        public string ReturnFields
        {
            set { _returnfields = value; }
            get { return _returnfields; }
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            set { _pagesize = value; }
            get { return _pagesize; }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            set { _pageindex = value; }
            get { return _pageindex; }
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        public string Where
        {
            set { _where = value; }
            get { return _where; }
        }
        /// <summary>
        /// 排序字段名最好为唯一主键
        /// </summary>
        public string Orderfld
        {
            set { _orderfld = value; }
            get { return _orderfld; }
        }
        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int TotalRecord
        {
            set { _totalrecord = value; }
            get { return _totalrecord; }
        }
        /// <summary>
        /// 返回总页数
        /// </summary>
        public int TotalPage
        {
            set { _totalpage = value; }
            get { return _totalpage; }
        }
        /// <summary>
        /// 最后返回的当前页记录的SQL语句
        /// </summary>
        public string Sql
        {
            set { _sql = value; }
            get { return _sql; }
        }

        /// <summary>
        /// 最后返回的返回全部记录的SQL语句
        /// </summary>
        public string FullSql
        {
            get
            {
                string l_returnfields = " * ";
                if (_returnfields.Trim() != "")
                {
                    l_returnfields = _returnfields.Trim();
                }
                string l_where = "";
                if (_where.Trim() != "")
                {
                    l_where = " where " + _where + " ";
                }
                string l_order = "";
                if (_orderfld.Trim() != "")
                {
                    l_order = "  order by " + _orderfld.Trim() + " ";
                }
                return " select " + l_returnfields + " from " + _tablename + l_where + l_order;
            }
        }

    }
}
