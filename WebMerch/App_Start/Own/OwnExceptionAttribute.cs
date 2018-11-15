using Lumos.Common;
using System;
using System.Web.Mvc;
using Lumos;
using Lumos.Web;

namespace WebMerch
{
    #region 基类BaseController异常过滤器
    /// <summary>
    /// 控制方法异常扑捉,跳转页面,记录信息
    /// </summary>
    public class OwnExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {

            bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            string controller = (string)filterContext.RouteData.Values["controller"];
            string action = (string)filterContext.RouteData.Values["action"];

            MessageBox messageBox = new MessageBox();
            messageBox.No = Guid.NewGuid().ToString();
            messageBox.Type = MessageBoxTip.Exception;
            messageBox.Title = "温馨提示";
            messageBox.Content = "[Own]抱歉,访问出错";
            messageBox.IsTop = false;
            messageBox.IsPopup = true;
            if (CommonUtil.CanViewErrorStackTrace())
            {
                messageBox.ErrorStackTrace = CommonUtil.ToHtml(filterContext.Exception.Message + "\r\n" + filterContext.Exception.StackTrace);
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

            LogUtil.Error("发生异常错误[编号:" + messageBox.No + "]", filterContext.Exception);

        }


    }

    #endregion
}