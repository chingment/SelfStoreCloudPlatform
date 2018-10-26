using StackExchange.Redis;
using System;


namespace Lumos.Redis
{
    /// <summary>
    /// 消息队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RedisMqObject<T> : RedisMqBaseObject
    {
        protected virtual bool IsTran { get; set; }

        protected virtual string MessageQueueKeyName { get; set; }

        /// <summary>
        /// 将指定的值插入到存储在键的列表尾部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        private void Push(string key, T t)
        {
            var redisManager = RedisManager.Manager;
            var lockdb = redisManager.GetDatabase(-1);
            var db = redisManager.GetDatabase();
            var keyInfo = AddSysCustomKey(key);
            if (IsTran)
            {
                var token = Environment.MachineName;
                if (lockdb.LockTake(keyInfo, token, TimeSpan.FromSeconds(20)))
                {
                    try
                    {
                        db.ListRightPush(keyInfo, ConvertJson(t));
                    }
                    finally
                    {
                        lockdb.LockRelease(keyInfo, token);
                    }
                }
            }
            else
            {
                db.ListRightPush(keyInfo, ConvertJson(t));
            }

        }
        /// <summary>
        /// 入栈
        /// </summary>
        /// <param name="t"></param>
        public void Push(T t)
        {
            Push(MessageQueueKeyName, t);
        }



        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            var keyInfo = AddSysCustomKey(MessageQueueKeyName);
            var redisManager = RedisManager.Manager;
            var lockdb = redisManager.GetDatabase(-1);
            var db = redisManager.GetDatabase();
            if (IsTran)
            {
                var token = Environment.MachineName;
                if (lockdb.LockTake(keyInfo, token, TimeSpan.FromSeconds(20)))
                {
                    try
                    {
                        var json = db.ListLeftPop(keyInfo);
                        if (json == default(RedisValue))
                        {
                            return default(T);
                        }
                        return ConvertObj<T>(json);
                    }
                    finally
                    {
                        lockdb.LockRelease(keyInfo, token);
                    }
                }
                return default(T);
            }
            else
            {
                var json = db.ListLeftPop(keyInfo);
                if (json == default(RedisValue))
                {
                    return default(T);
                }
                return ConvertObj<T>(json);
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            var redisManager = RedisManager.Manager;
            var db = redisManager.GetDatabase();
            var keyInfo = AddSysCustomKey(MessageQueueKeyName);
            var l = db.ListLength(keyInfo);

            return Convert.ToInt32(l);
        }

    }
}
