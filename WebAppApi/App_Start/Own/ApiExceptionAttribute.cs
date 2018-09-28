using log4net;
using Lumos;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http.Filters;

namespace WebAppApi
{


    public class APIExceptionAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            LogUtil.SetTrackId();
            log.Error("API调用出现异常", ex);
            //if (ex != null)
            //{
            //    if (ex.InnerException != null)
            //    {
            //        log.Error(ex.InnerException.Message);
            //        log.Error(ex.StackTrace);
            //    }
            //}

            APIResult result = new APIResult(ResultType.Exception, ResultCode.Exception, "程序发生异常");
            actionExecutedContext.Response = new APIResponse(result);

            base.OnException(actionExecutedContext);


            ////1.异常日志记录（正式项目里面一般是用log4net记录异常日志）
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "——" +
            //    actionExecutedContext.Exception.GetType().ToString() + "：" + actionExecutedContext.Exception.Message + "——堆栈信息：" +
            //    actionExecutedContext.Exception.StackTrace);

            ////2.返回调用方具体的异常信息

            //var httpResponseMessage = new HttpResponseMessage();
            //if (actionExecutedContext.Exception is NotImplementedException)
            //{
            //    httpResponseMessage.StatusCode = HttpStatusCode.NotImplemented;
            //}
            //else if (actionExecutedContext.Exception is TimeoutException)
            //{
            //    httpResponseMessage.StatusCode = HttpStatusCode.RequestTimeout;
            //}
            ////.....这里可以根据项目需要返回到客户端特定的状态码。如果找不到相应的异常，统一返回服务端错误500
            //else
            //{
            //    //,\"message\":\"程序发生异常\"
            //    httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
            //    httpResponseMessage.Content = new StringContent("{\"result\":3,\"code\":3000}", Encoding.GetEncoding("UTF-8"), "application/json");
            //}

            //actionExecutedContext.Response = httpResponseMessage;

            base.OnException(actionExecutedContext);

        }
    }

}