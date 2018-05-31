using Lumos.DAL;
using log4net;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;


namespace WebSSO
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //扩展
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngineExtension());
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            if (LogicalThreadContext.Properties["trackid"] == null)
            {
                if (this.Session != null)
                {
                    LogicalThreadContext.Properties["trackid"] = this.Session.SessionID;
                }
            }


            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            HttpApplication ap = sender as HttpApplication;
            System.Exception ex = ap.Server.GetLastError();


            var httpStatusCode = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误 

            switch (httpStatusCode)
            {
                case 404:
                    break;
                default:
                    log.Error("Application to catch an exception error", ex);
                    break;
            }
        }

    }

    public class RazorViewEngineExtension : RazorViewEngine
    {
        public RazorViewEngineExtension()
        {
            ////在RouteConfig.cs定义相应的路由
            string appPath = HttpRuntime.AppDomainAppPath;
            List<string> list = new List<string>();

            System.IO.DirectoryInfo viewDir = new System.IO.DirectoryInfo(appPath + "/Views");
            list.Add("~/Views/{1}/{0}.cshtml");
            for (int i = 0; i < viewDir.GetDirectories().Length; i++)
            {
                list.Add("~/Views/" + viewDir.GetDirectories()[i].Name + "/{0}.cshtml");
                list.Add("~/Views/" + viewDir.GetDirectories()[i].Name + "/{1}/{0}.cshtml");
            }

            ViewLocationFormats = list.ToArray();

        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            object a = base.ViewLocationFormats;
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }
    }

}
