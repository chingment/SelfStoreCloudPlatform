using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lumos.Mvc
{
    public static class ApiMonitorLog
    {

        public static string GetPostData(Stream inputStream)
        {
            string s = "";

            if (inputStream == null)
                return s;

            try
            {
                Stream stream = inputStream;
                stream.Seek(0, SeekOrigin.Begin);
                s = new StreamReader(stream).ReadToEnd();
            }
            catch
            {
                s = "";
            }

            return s;
        }

        public static void OnActionExecuting(HttpActionContext filterContext)
        {
            var sb = new StringBuilder();
            var request = ((HttpContextWrapper)filterContext.Request.Properties["MS_HttpContext"]).Request;
            sb.Append("Method: " + request.HttpMethod + Environment.NewLine);
            sb.Append("Url: " + request.RawUrl + Environment.NewLine);
            sb.Append("PostData: " + GetPostData(request.InputStream) + Environment.NewLine);

            LogUtil.Info(sb.ToString());
        }


        private static async Task<string> LogAsync(HttpActionExecutedContext actionContext)
        {
            string content = await actionContext.Response.Content.ReadAsStringAsync();
            var sb = new StringBuilder();
            var request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
            //sb.Append("Method: " + request.HttpMethod + Environment.NewLine);
            //sb.Append("Url: " + request.RawUrl + Environment.NewLine);
            //sb.Append("PostData: " + GetPostData(request.InputStream) + Environment.NewLine);
            sb.Append("Response: " + content + Environment.NewLine);
            LogUtil.Info(sb.ToString());
            return content;
        }

        public static void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            var content = LogAsync(actionContext);
        }
    }
}
