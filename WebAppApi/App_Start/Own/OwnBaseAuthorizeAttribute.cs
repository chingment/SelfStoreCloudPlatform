using System;
using System.Web;
using System.Collections.Specialized;
using System.Net;
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

namespace WebAppApi
{

    public class BaseAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string key = "_MonitorApiLog_";


        protected ILog Log
        {
            get
            {
                return LogManager.GetLogger(this.GetType());
            }
        }

        protected void SetTrackID()
        {
            if (ThreadContext.Properties["trackid"] == null)
                ThreadContext.Properties["trackid"] = DateTime.Now.TimeOfDay.TotalMilliseconds.ToString("00000000"); //Guid.NewGuid().ToString("N");
        }

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
                SetTrackID();
                Log.Info("调用API接口");
                DateTime requestTime = DateTime.Now;
                var request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
                var requestMethod = request.HttpMethod;

                string app_key = request.Headers["key"];
                string app_sign = request.Headers["sign"];
                string app_version = request.Headers["version"];
                string app_timestamp_s = request.Headers["timestamp"];

                if (app_version != null)
                {
                    Log.Info("app_version:" + app_version);
                }

                string app_data = null;
                if (requestMethod == "POST")
                {
                    //var s = HttpContext.Current.Request.Form["form-data"];
                    Stream stream = HttpContext.Current.Request.InputStream;
                    stream.Seek(0, SeekOrigin.Begin);
                    app_data = new StreamReader(stream).ReadToEnd();

                    Log.Info("app_data:" + app_data);

                    #region 过滤图片
                    if (app_data.LastIndexOf(",\"ImgData\":{") > -1)
                    {
                        //Log.Info("去掉图片之前的数据：" + app_data);
                        int x = app_data.LastIndexOf(",\"ImgData\":{");
                        app_data = app_data.Substring(0, x);
                        app_data += "}";
                        //Log.Info("去掉图片之后的数据：" + app_data);

                    }
                    else if (app_data.LastIndexOf(",\"imgData\":{") > -1)
                    {
                        // Log.Info("去掉图片之前的数据：" + app_data);
                        int x = app_data.LastIndexOf(",\"imgData\":{");
                        app_data = app_data.Substring(0, x);
                        app_data += "}";
                        //Log.Info("去掉图片之后的数据：" + app_data);
                    }

                    #endregion
                }
                else
                {
                    NameValueCollection queryForm = HttpContext.Current.Request.QueryString;
                    Dictionary<string, string> queryData = new Dictionary<string, string>();
                    for (int f = 0; f < queryForm.Count; f++)
                    {
                        string querykey = queryForm.Keys[f];
                        queryData.Add(querykey, queryForm[querykey]);
                    }
                    app_data = GetQueryData(queryData);
                }

                //记录请求的日志
                MonitorApiLog monitorApiLog = new MonitorApiLog();
                monitorApiLog.RequestTime = requestTime;
                monitorApiLog.RequestUrl = request.RawUrl;
                monitorApiLog.SignatureData = new SignatureData { Key = app_key, Sign = app_sign, TimeStamp = app_timestamp_s, Data = app_data };
                Log.Info(string.Format("API请求:{0}", monitorApiLog.ToString()));

                actionContext.ActionArguments[key] = monitorApiLog;

                //检查必要的参数
                if (app_key == null || app_sign == null || app_timestamp_s == null)
                {
                    APIResult result = new APIResult(ResultType.Failure, ResultCode.FailureSign, "缺少必要参数");
                    actionContext.Response = new APIResponse(result);
                    return;
                }

                //检查key是否在数据库中存在
                string app_secret = SysFactory.AppKeySecret.GetSecret(app_key);

                if (app_secret == null)
                {
                    APIResult result = new APIResult(ResultType.Failure, ResultCode.FailureSign, "应用程序Key,存在错误");
                    actionContext.Response = new APIResponse(result);
                    return;
                }

                long app_timestamp = long.Parse(app_timestamp_s);

                string signStr = Signature.Compute(app_key, app_secret, app_timestamp, app_data);

                //Log.Info("app_key:" + app_key);
                //Log.Info("app_secret:" + app_secret);
                //Log.Info("app_timestamp:" + app_timestamp);
                //Log.Info("app_data:" + app_data);
                //Log.Info("signStr:" + signStr);
                //Log.Info("app_sign:" + app_sign);


                if (Signature.IsRequestTimeout(app_timestamp))
                {
                    APIResult result = new APIResult(ResultType.Failure, ResultCode.FailureSign, "请求已超时");
                    actionContext.Response = new APIResponse(result);
                    return;
                }




                if (signStr != app_sign)
                {
                    Log.Warn("API签名错误");
                    APIResult result = new APIResult(ResultType.Failure, ResultCode.FailureSign, "签名错误");
                    actionContext.Response = new APIResponse(result);
                    return;
                }

                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("API错误:{0}", ex.Message), ex);
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
            Log.Info(string.Format("API响应:{0}", monitorApiLog.ToString()));
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