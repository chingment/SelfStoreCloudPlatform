using System;
using System.Collections.Generic;


namespace Lumos.Redis
{
    public class RedisHashUtil
    {
        /// 判断某个数据是否已经被缓存
        /// </summary>
        public static bool Exist<T>(string hashId, string key)
        {
            return RedisManager.Db.HashExists(hashId, key);
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public static bool Set<T>(string hashId, string key, T t)
        {
            var value = Newtonsoft.Json.JsonConvert.SerializeObject(t);

            return RedisManager.Db.HashSet(hashId, key, value);
        }

        public static bool Set(string hashId, string key, object t)
        {
            var value = Newtonsoft.Json.JsonConvert.SerializeObject(t);

            return RedisManager.Db.HashSet(hashId, key, value);
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        public static bool Remove(string hashId, string key)
        {
            return RedisManager.Db.HashDelete(hashId, key);
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        public static bool Remove(string key)
        {
            return RedisManager.Db.KeyDelete(key);
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        public static T Get<T>(string hashId, string key)
        {
            string value = RedisManager.Db.HashGet(hashId, key);

            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        public static List<T> GetAll<T>(string hashId)
        {
            var result = new List<T>();
            var list = RedisManager.Db.HashValues(hashId);

            if (list != null && list.Length > 0)
            {

                foreach (var item in list)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item);
                    result.Add(obj);
                }
            }
            return result;
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        public static void SetExpire(string key, DateTime datetime)
        {
            RedisManager.Db.KeyExpire(key, datetime);
        }
    }
}
