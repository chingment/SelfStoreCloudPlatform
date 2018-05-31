using log4net;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Lumos.Session.Redis
{
    /// <summary>
    /// 用户状态管理
    /// </summary>
    public class RedisSession : ISession
    {
        private ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string SessionSign = "SessionName";
        private const int TimeOut = 10080;
        private string _sessionId = "";
        private string _sessionKey = "";

        public string SessionId
        {

            get
            {
                return _sessionId;
            }
        }

        public string SessionKey
        {
            get
            {

                return _sessionKey;
            }
        }

        public RedisSession()
        {
            _sessionId = Guid.NewGuid().ToString();
            _sessionKey = string.Format("{0}:{1}", SessionSign, _sessionId);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key) where T : class, new()
        {
            return new RedisClient<T>().KGet(key);
        }

        public void Set<T>(string key,T obj) where T : class, new()
        {
            new RedisClient<T>().KSet(key, obj, new TimeSpan(0, TimeOut, 0));
        }

        public void Quit(string key)
        {
            new RedisClient<object>().KRemove(key);
        }

        public void Postpone(string key)
        {
            new RedisClient<object>().KSetEntryIn(key, new TimeSpan(0, TimeOut, 0));
        }
    }
}

