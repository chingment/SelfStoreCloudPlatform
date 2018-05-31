using Lumos.Common;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAgent
{

    public class OwnNoResubmitAttribute : ActionFilterAttribute
    {
        private static readonly string HttpMehotdPost = "POST";
        private static readonly string prefix = "postFlag";
        private string nameWithRoute;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerContext = filterContext.Controller.ControllerContext;
            if (!controllerContext.IsChildAction)
            {
                var request = controllerContext.HttpContext.Request;
                if (request.Form["_UniqueSubmitIdentifier"] != null)
                {
                    string uniqueSubmitIdentifier = request.Form["_UniqueSubmitIdentifier"].ToString();
                    var session = controllerContext.HttpContext.Session;
                    bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();

                    nameWithRoute = uniqueSubmitIdentifier;

                    if (session[nameWithRoute] == null)
                    {
                        session[nameWithRoute] = uniqueSubmitIdentifier;
                    }
                    else
                    {

                        if (session[nameWithRoute].ToString() == uniqueSubmitIdentifier)
                        {
                            if (isAjaxRequest)
                            {
                                CustomJsonResult jsonResult = new CustomJsonResult(ResultType.Failure, "你已经提交了,请勿重复提交,关闭窗口或刷新后再次提交！");
                                //jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                                filterContext.Result = jsonResult;
                                filterContext.Result.ExecuteResult(filterContext);
                                filterContext.HttpContext.Response.End();
                            }
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private string GenerateUrlWithTimeStamp(string url)
        {
            return string.Format("{0}{1}timeStamp={2}", url, url.Contains("?") ? "&" : "?", (DateTime.Now - DateTime.Parse("2010/01/01")).Ticks);
        }

        private bool IsPost(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request.HttpMethod == HttpMehotdPost;
        }

        private string generateNameWithRoute(ControllerContext controllerContext)
        {
            StringBuilder sb = new StringBuilder(prefix);
            foreach (object routeValue in controllerContext.RouteData.Values.Values)
            {
                sb.AppendFormat("_{0}", routeValue);
            }
            return sb.ToString();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            CustomJsonResult result = JsonConvert.DeserializeObject<CustomJsonResult>(filterContext.Result.ToString());
            var controllerContext = filterContext.Controller.ControllerContext;
            var session = controllerContext.HttpContext.Session;
            if (result.Result != ResultType.Success && result.Result != ResultType.Exception)
            {
                session[nameWithRoute] = null;
            }
        }
    }
}