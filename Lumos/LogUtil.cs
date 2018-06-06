using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lumos
{
    public static class LogUtil
    {
        public static void SetTrackId()
        {
            string trackid = Guid.NewGuid().ToString();
            if (LogicalThreadContext.Properties["trackid"] == null)
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        trackid = HttpContext.Current.Session.SessionID;
                    }
                }
            }

            LogicalThreadContext.Properties["trackid"] = trackid;
        }

        private static ILog GetLog()
        {

            Type type = MethodBase.GetCurrentMethod().DeclaringType;

            var trace = new System.Diagnostics.StackTrace();

            //for (int i = 0; i < trace.FrameCount; i++)
            //{
            //    ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            //    System.Reflection.MethodBase mb2 = trace.GetFrame(i).GetMethod();
            //    if (mb2 != null)
            //    {
            //        if (mb2.DeclaringType != null)
            //        {
            //            log.Info(string.Format("[CALL STACK][{0}]: {1}.{2}", i, mb2.DeclaringType.FullName, mb2.Name));
            //        }
            //    }
            //}

            string name = type.Name;
            if (trace.FrameCount >= 3)
            {
                System.Reflection.MethodBase mb = trace.GetFrame(2).GetMethod();
                type = mb.DeclaringType;
                name = string.Format("{0}.{1}", mb.DeclaringType.FullName, mb.Name);
            }

            return log4net.LogManager.GetLogger(name);
        }

        public static void Info(string msg)
        {
            GetLog().Info(msg);
        }

        public static void Warn(string msg)
        {
            GetLog().Warn(msg);
        }

        public static void Error(string msg)
        {
            GetLog().Error(msg);
        }

        public static void Error(string msg, Exception ex)
        {
            GetLog().Error(msg, ex);
        }
    }
}
