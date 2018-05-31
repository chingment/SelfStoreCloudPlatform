using Lumos.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models;
using Lumos.Mvc;

namespace WebAgent
{
    #region 基类BaseController异常过滤器
    /// <summary>
    /// 控制方法异常扑捉,跳转页面,记录信息
    /// </summary>
    public class OwnExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {

            if (LogicalThreadContext.Properties["trackid"] == null)
            {
                if (HttpContext.Current.Session != null)
                {
                    LogicalThreadContext.Properties["trackid"] = HttpContext.Current.Session.SessionID;
                }
            }

            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            string controller = (string)filterContext.RouteData.Values["controller"];
            string action = (string)filterContext.RouteData.Values["action"];

            MessageBoxModel messageBox = new MessageBoxModel();
            messageBox.No = Guid.NewGuid().ToString();
            messageBox.Type = MessageBoxTip.Exception;
            messageBox.Title = "温馨提示";
            messageBox.Content = "[Own]抱歉,访问出错";
            messageBox.IsTop = false;
            messageBox.IsPopup = true;
            if (CommonUtils.CanViewErrorStackTrace())
            {
                messageBox.ErrorStackTrace = CommonUtils.ToHtml(filterContext.Exception.Message + "\r\n" + filterContext.Exception.StackTrace);
            }

            if (isAjaxRequest)
            {
                CustomJsonResult jsonResult = new CustomJsonResult(ResultType.Exception, ResultCode.Exception, messageBox.Title, messageBox);
                //jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = jsonResult;
                filterContext.Result.ExecuteResult(filterContext);
                filterContext.HttpContext.Response.End();
            }
            else
            {
                filterContext.Result = new ViewResult { ViewName = "MessageBox", MasterName = "_Layout", ViewData = new ViewDataDictionary { Model = messageBox } };
            }

            filterContext.ExceptionHandled = true;

            log.Error("发生异常错误[编号:" + messageBox.No + "]", filterContext.Exception);

        }


    }

    #endregion
}