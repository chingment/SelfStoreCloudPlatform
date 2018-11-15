using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lumos.Web.Http
{
    public static class MonitorLog
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
            Task tk = LogAsync(filterContext.Request);
        }

        private static async Task LogAsync(HttpRequestMessage request, HttpResponseMessage response = null)
        {
            var sb = new StringBuilder();
            var myRequest = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request;
            sb.Append("Url: " + myRequest.RawUrl + Environment.NewLine);
            sb.Append("IP: " + Common.CommonUtil.GetIP() + Environment.NewLine);
            sb.Append("Method: " + myRequest.HttpMethod + Environment.NewLine);
            sb.Append("ContentType: " + myRequest.ContentType + Environment.NewLine);

            NameValueCollection headers = myRequest.Headers;


            sb.Append("Header.CurrentUserId: " + headers["CurrentUserId"] + Environment.NewLine);


            if (headers["key"] != null)
            {
                sb.Append("Header.key: " + headers["key"] + Environment.NewLine);
                sb.Append("Header.sign: " + headers["sign"] + Environment.NewLine);
                sb.Append("Header.version: " + headers["version"] + Environment.NewLine);
                sb.Append("Header.timestamp: " + headers["timestamp"] + Environment.NewLine);
            }

            if (myRequest.ContentType.IndexOf("application/json") > -1)
            {
                sb.Append("PostData: " + GetPostData(myRequest.InputStream) + Environment.NewLine);
            }
            if (response != null)
            {
                string content = await response.Content.ReadAsStringAsync();
                sb.Append("Response: " + content + Environment.NewLine);
            }

            LogUtil.Info(sb.ToString());
        }

        public static void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            Task tk = LogAsync(actionContext.Request, actionContext.Response);
        }
    }
}
