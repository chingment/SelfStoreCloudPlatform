using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{
    public interface ILoggerProvider
    {
        void Debug(object s);
        void Debug(object s, System.Exception ex);

        void Info(object s);
        void Info(object s, System.Exception ex);

        void Warn(object s);
        void Warn(object s, System.Exception ex);

        void Error(object s);
        void Error(object s, System.Exception ex);

        void Fatal(object s);
        void Fatal(object s, System.Exception ex);

        void CallApi(object s);
        void CallApi(object s, System.Exception ex);

        void UserBehavior(object userid, string behavior);
    }
}
