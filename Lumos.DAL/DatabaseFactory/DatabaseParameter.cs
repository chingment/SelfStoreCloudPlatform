using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{
    public class DatabaseParameter
    {

        private string _Name = "";
        private object _Value = null;
        private DatabaseParameterDirection _Direction = DatabaseParameterDirection.Input;
        private DatabaseParameterDataType _DataType = DatabaseParameterDataType.Object;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        public int? MaxLength
        {
            get;
            set;
        }

        public DatabaseParameterDirection Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;
            }
        }

        public DatabaseParameterDataType DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
            }
        }


        /// <summary>
        /// 默认DatabaseParameterDirection为Input,不限制字符大小
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public DatabaseParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
            this.Direction = DatabaseParameterDirection.Input;
        }

        /// <summary>
        /// 默认DatabaseParameterDirection为Input,DatabaseParameterDataType为String
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="len">参数值最大字符数</param>
        public DatabaseParameter(string name, object value, int len)
        {
            this.Name = name;
            this.Value = value;
            this.Direction = DatabaseParameterDirection.Input;
            this.MaxLength = len;
        }

        /// <summary>
        /// 默认DatabaseParameterDirection为Input
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="datatype">参数类型</param>
        /// <param name="len">参数值最大字符数</param>
        public DatabaseParameter(string name, object value, DatabaseParameterDataType datatype, int len)
        {
            this.Name = name;
            this.Direction = DatabaseParameterDirection.Input;
            this.DataType = datatype;
            this.Value = value;
            this.MaxLength = len;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数输入输出类型</param>
        /// <param name="datatype">参数类型</param>
        /// <param name="len">参数值最大字符数</param>
        public DatabaseParameter(string name, object value, DatabaseParameterDirection direction, DatabaseParameterDataType datatype, int len)
        {
            this.Name = name;
            this.Direction = direction;
            this.DataType = datatype;
            this.Value = value;
            this.MaxLength = len;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="direction">参数输入输出类型</param>
        /// <param name="datatype">参数类型</param>
        public DatabaseParameter(string name, DatabaseParameterDirection direction, DatabaseParameterDataType datatype)
        {
            this.Name = name;
            this.Direction = direction;
            this.DataType = datatype;
        }
    }

    public enum DatabaseParameterDirection
    {

        // 摘要:
        //     参数是输入参数。
        Input = 1,
        //
        // 摘要:
        //     参数是输出参数。
        Output = 2,
        //
        // 摘要:
        //     参数既能输入，也能输出。
        InputOutput = 3,
        //
        // 摘要:
        //     参数表示诸如存储过程、内置函数或用户定义函数之类的操作的返回值。
        ReturnValue = 6,
    }
    public enum DatabaseParameterDataType
    {
        Object = 0,
        // 摘要:
        //     参数是输入参数。
        Int = 1,
        //
        // 摘要:
        //     参数是输出参数。
        String = 2,

        Decimal = 3,
        Float = 4,
    }
}
