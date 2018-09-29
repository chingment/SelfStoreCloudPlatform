using System;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Collections.Generic;
using Lumos.Mvc;
using System.Globalization;
using log4net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Lumos;
using Lumos.BLL;
using Lumos.Common;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Lumos.Session;

namespace WebAppApi
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string key = "_MonitorApiLog_";

        private DateTime GetDateTimeTolerance(long timestamp)
        {
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("1970-01-01 00:00:00")).AddSeconds(timestamp);
            var ts = DateTime.Now - dt;
            if (System.Math.Abs(ts.TotalMinutes) > 5)
            {
                dt = DateTime.Now;
            }
            return dt;
        }


        public static string GetQueryData(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return "";

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = HttpUtility.UrlEncode(dem.Current.Value, UTF8Encoding.UTF8).ToUpper();
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }

            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

            return s;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {

                LogUtil.Info("调用API接口");
                DateTime requestTime = DateTime.Now;
                var request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
                var requestMethod = request.HttpMethod;

                bool skipAuthorization = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
                if (skipAuthorization)
                {
                    return;
                }

                var accessToken = request.QueryString["accessToken"];

                if (string.IsNullOrEmpty(accessToken))
                {
                    APIResult result = new APIResult(ResultType.Failure, ResultCode.FailureSign, "accessToken不能为空");
                    actionContext.Response = new APIResponse(result);
                    return;
                }

                var userInfo = SSOUtil.GetUserInfo(accessToken);

                if (userInfo == null)
                {
                    APIResult result = new APIResult(ResultType.Failure, ResultCode.FailureSign, "accessToken 已经超时");
                    actionContext.Response = new APIResponse(result);
                    return;
                }

                string app_data = null;

                if (requestMethod == "POST")
                {
                    Stream stream = HttpContext.Current.Request.InputStream;
                    stream.Seek(0, SeekOrigin.Begin);
                    app_data = new StreamReader(stream).ReadToEnd();
                }

                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("API错误:{0}", ex.Message), ex);
                APIResult result = new APIResult(ResultType.Exception, ResultCode.Exception, "内部错误");
                actionContext.Response = new APIResponse(result);

                return;
            }

        }


        private async Task<string> GetResponseContentAsync(HttpActionExecutedContext actionContext, DateTime responseTime)
        {

            string content = await actionContext.Response.Content.ReadAsStringAsync();
            MonitorApiLog monitorApiLog = actionContext.ActionContext.ActionArguments[key] as MonitorApiLog;
            monitorApiLog.ResponseTime = responseTime;
            monitorApiLog.ResponseData = content;//form表单提交的数据
            LogUtil.Info(string.Format("API响应:{0}", monitorApiLog.ToString()));
            return content;
        }


        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            base.OnActionExecuted(actionContext);

            DateTime responseTime = DateTime.Now;
            var content = GetResponseContentAsync(actionContext, responseTime);

        }
    }
}