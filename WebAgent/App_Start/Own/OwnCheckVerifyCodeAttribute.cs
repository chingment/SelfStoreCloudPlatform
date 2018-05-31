using log4net;
using Lumos.Common;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;


namespace WebAgent
{
    public class CheckVerifyCodeAttribute : ActionFilterAttribute
    {
        public string SessionName { get; set; }

        public string VerifyCodeName { get; set; }

        public CheckVerifyCodeAttribute(string name)
        {
            this.SessionName = name;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            bool isCheck = true;
            var isCheckImageVerifyCode = filterContext.HttpContext.Request.Form["IsCheckVerifyCode"];
            if (isCheckImageVerifyCode != null)
            {
                Boolean.TryParse(isCheckImageVerifyCode.ToString(), out isCheck);
            }

            if (isCheck)
            {
                bool isFlag = true;
                if (string.IsNullOrEmpty(this.SessionName))
                {
                    isFlag = false;
                }
                else
                {
                    object sessionVerifyCode = filterContext.HttpContext.Session[this.SessionName];
                    if (sessionVerifyCode == null)
                    {
                        isFlag = false;
                    }
                    else
                    {
                        string verifyCode = null;
                        if (this.VerifyCodeName == null)
                        {
                            verifyCode = filterContext.HttpContext.Request.Form["txt_VerifyCode"];
                        }
                        else
                        {
                            verifyCode = filterContext.HttpContext.Request.Form[this.VerifyCodeName];
                        }
                        if (string.IsNullOrEmpty(verifyCode))
                        {
                            isFlag = false;
                        }
                        else
                        {
                            if (verifyCode.Trim().ToUpper() != sessionVerifyCode.ToString().Trim().ToUpper())
                            {
                                isFlag = false;
                            }
                        }
                    }
                }

                if (!isFlag)
                {
                    CustomJsonResult jsonResult = new CustomJsonResult(ResultType.Failure, ResultCode.FailureValidCode, "验证码不正确");
                   // jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    filterContext.Result = jsonResult;
                    filterContext.Result.ExecuteResult(filterContext);
                    filterContext.HttpContext.Response.End();
                }
            }

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result != null)
            {

                ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                log.Info(filterContext.Result.ToString());


                CustomJsonResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomJsonResult>(filterContext.Result.ToString());

                if (result.Result == ResultType.Success)
                {
                    if (!string.IsNullOrEmpty(this.SessionName))
                    {
                        filterContext.HttpContext.Session[this.SessionName] = null;
                    }
                }
            }
        }
    }
}