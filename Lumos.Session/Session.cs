using Lumos.Session.Redis;
using System;

namespace Lumos.Session
{
    public class Session : ISession
    {
        ISession _session = new RedisSession();

        public T Get<T>(string key) where T : class, new()
        {

            return _session.Get<T>(key);
        }
        public void Set<T>(string key, T obj) where T : class, new()
        {
            _session.Set<T>(key, obj);
        }
        public void Set<T>(string key, T obj, TimeSpan expireIn) where T : class, new()
        {
            _session.Set<T>(key, obj, expireIn);
        }

        public void Postpone(string key)
        {
            _session.Postpone(key);
        }

        public void Quit(string key)
        {
            _session.Quit(key);
        }
    }
}
