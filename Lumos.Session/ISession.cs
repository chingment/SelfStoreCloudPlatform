using System;


namespace Lumos.Session
{
    public interface ISession
    {
        T Get<T>(string key) where T : class, new();
        void Set<T>(string key, T value) where T : class, new();
        void Set<T>(string key, T value, TimeSpan expireIn) where T : class, new();
        void Postpone(string key);
        void Quit(string key);
    }
}
