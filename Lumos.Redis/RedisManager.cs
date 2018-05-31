using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using Newtonsoft;
using System.Configuration;
using StackExchange.Redis;

namespace Lumos.Redis
{
    /// <summary>
    /// 配置管理和初始化
    /// </summary>
    public class RedisManager
    {
        private static ConnectionMultiplexer _redis;
        private static object _locker = new object();

        public static ConnectionMultiplexer Manager
        {
            get
            {
                if (_redis == null)
                {
                    lock (_locker)
                    {
                        if (_redis != null) return _redis;

                        _redis = GetManager();
                        return _redis;
                    }
                }

                return _redis;
            }
        }

        private static ConnectionMultiplexer GetManager()
        {
            string _conn = ConfigurationManager.AppSettings["custom:RedisServer"];
            return ConnectionMultiplexer.Connect(_conn);
        }

        public static IDatabase Db
        {
            get
            {
                return RedisManager.Manager.GetDatabase();
            }
        }

    }
}
