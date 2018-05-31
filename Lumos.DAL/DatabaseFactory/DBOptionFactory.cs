using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{
    /// <summary>
    /// 数据库工厂
    /// </summary>
    internal class DBOptionFactory
    {
        /// <summary>
        /// 创建数据库操作类
        /// </summary>
        /// <returns></returns>
        public IDBOptionBySqlSentence CreateOptionBySqlSentence()
        {

            IDBOptionBySqlSentence option = new SqlServerOptionBySqlSentence();


            return option;
        }

    }
}
