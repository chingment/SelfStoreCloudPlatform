using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Session
{
    public interface ISession
    {
        T Get<T>(string key) where T : class, new();
        void Set<T>(string key, T value) where T : class, new();
        void Postpone(string key);
        void Quit(string key);
    }
}
