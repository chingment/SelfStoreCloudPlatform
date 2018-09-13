using log4net;
using Lumos;
using Lumos.Common;
using Lumos.Mvc;
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebMobile
{
    #region 基类BaseController异常过滤器
    /// <summary>
    /// 控制方法异常扑捉,跳转页面,记录信息
    /// </summary>
    public class OwnExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _Message = "An exception error occurred";
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }


        private string _ErrorUrl = "~/Error.htm";
        public string ErrorUrl
        {
            get
            {
                return _ErrorUrl;
            }
            set
            {
                _ErrorUrl = value;
            }
        }


        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {

            LogUtil.SetTrackId();

            bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            string controller = (string)filterContext.RouteData.Values["controller"];
            string action = (string)filterContext.RouteData.Values["action"];

            MessageBoxModel messageBox = new MessageBoxModel();
            messageBox.No = Guid.NewGuid().ToString();
            messageBox.Type = MessageBoxTip.Exception;
            messageBox.Title = "抱歉,系统发生异常，如有需要请联系客服 020-82310186";
            messageBox.Content = "<a href=\"javascript:void(0)\" onclick=\"window.top.location.href='" + OwnWebSettingUtils.GetHomePage() + "'\">返回主页</a>";
            messageBox.IsTop = true;
            if (CommonUtils.CanViewErrorStackTrace())
            {
                // messageBox.ErrorStackTrace = CommonUtils.ToHtml(filterContext.Exception.Message + "\r\n" + filterContext.Exception.StackTrace);
            }

            //判断是否异步调用
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
                string masterName = "_Layout";


                filterContext.Result = new ViewResult { ViewName = "MessageBox", MasterName = masterName, ViewData = new ViewDataDictionary { Model = messageBox } };

            }


            filterContext.ExceptionHandled = true;

            log.Error("发生异常错误[编号:" + messageBox.No + "]", filterContext.Exception);


        }


    }

    #endregion
}