using log4net;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Redis
{

    public static class RedisClient
    {

        public static T Get<T>(string key) where T : class
        {
            var client = new RedisClient<T>();

            return client.KGet(key);
        }

        public static T HashGet<T>(string key, string value) where T : class
        {
            string val = RedisManager.Db.HashGet(key, value);
            if (string.IsNullOrEmpty(val))
                return null;

            return JsonConvert.DeserializeObject<T>(val);
        }

        public static bool HashSet<T>(string key, string hasField, T value) where T : class
        {
            string v = JsonConvert.SerializeObject(value);
            bool isFlag = RedisManager.Db.HashSet(key, hasField, v, When.Always);

            return isFlag;
        }

        public static bool Set<T>(string key, T value, TimeSpan expireIn) where T : class
        {
            var client = new RedisClient<T>();

            return client.KSet(key, value, expireIn);
        }
    }



    public class RedisClient<T> where T : class
    {

        private ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        IDatabase client = RedisManager.Manager.GetDatabase();
        /// <summary>  
        /// Session管理  
        /// </summary>  
        public RedisClient()
        {

        }

        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                return RedisManager.Manager;
            }
        }

        public bool KSet(string key, T value, TimeSpan expireIn, When when = When.Always)
        {
            bool isFlag = false;
            try
            {
                if (typeof(T) == typeof(string))
                {
                    isFlag = client.StringSet(key, value.ToString(), expireIn);
                }
                else
                {
                    isFlag = client.StringSet(key, JsonConvert.SerializeObject(value), expireIn, when);
                }

            }
            catch (Exception ex)
            {
                log.Error("RedisClient Error(KSet):" + ex.Message);
            }
            return isFlag;
        }

        public T KGet(string key)
        {
            try
            {
                T fEntity = null;
                string fSessionValue = client.StringGet(key);

                if (!string.IsNullOrEmpty(fSessionValue))
                {
                    fEntity = JsonConvert.DeserializeObject<T>(fSessionValue);
                }
                return fEntity;
            }
            catch (Exception ex)
            {
                log.Error("RedisClient Error(KGet):" + ex.Message);
            }
            return default(T);
        }

        public string KGetString(string key)
        {
            try
            {
                string fSessionValue = client.StringGet(key);

                return fSessionValue;
            }
            catch (Exception ex)
            {
                log.Error("RedisClient Error(KGet):" + ex.Message);
            }
            return null;
        }


        public void KSetEntryIn(string key, TimeSpan expiresTime)
        {
            try
            {
                client.KeyExpire(key, expiresTime);
            }
            catch (Exception ex)
            {
                log.Error("RedisClient Error(KSetEntryIn):" + ex.Message);
            }
        }

        public void KRemove(string key)
        {
            try
            {
                client.KeyExpire(key, new TimeSpan(0, -1, 0));
            }
            catch (Exception ex)
            {
                log.Error("RedisClient Error(KSetEntryIn):" + ex.Message);
            }
        }
    }
}
