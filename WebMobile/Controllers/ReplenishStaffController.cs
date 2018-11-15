using Lumos;
using Lumos.BLL;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.AppMobile;
using Lumos.BLL.Service.Merch;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMobile.Controllers
{
    public class ReplenishStaffController : OwnBaseController
    {
        public ActionResult BindMobile()
        {
            return View();
        }

        public CustomJsonResult BindMobileSendValidCode(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return new CustomJsonResult(ResultType.Failure, "该手机号不能为空");


            var sysClientUser = CurrentDb.SysClientUser.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
            if (sysClientUser != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.AlreadyExist, "该手机号已被使用");
            }
            var result = BizFactory.Sms.ReplenishStaffBindMobile(this.CurrentUserId, phoneNumber);
            return result;
        }

        public CustomJsonResult CheckBindMobile()
        {
            var ret = new RetOperateResult();


            var sysUser = CurrentDb.SysUser.Where(m => m.Id == this.CurrentUserId).FirstOrDefault();

            if (string.IsNullOrEmpty(sysUser.PhoneNumber))
            {
                ret.Result = RetOperateResult.ResultType.Failure;
                ret.Remarks = "";
                ret.Message = "未绑定手机";
                ret.IsComplete = true;

                return new CustomJsonResult(ResultType.Success, ResultCode.NoBind, "未绑定手机", ret);
            }
            else
            {
                ret.Result = RetOperateResult.ResultType.Success;
                ret.Remarks = "";
                ret.Message = string.Format("已绑定手机尾号({0})", CommonUtil.GetPhoneNumberTail(sysUser.PhoneNumber));
                ret.IsComplete = true;

                return new CustomJsonResult(ResultType.Success, ResultCode.HasBind, "已绑定手机", ret);
            }

        }

        [HttpPost]
        public CustomJsonResult BindMobile(Lumos.BLL.Service.AppMobile.RopReplenishStaffBindMobile rop)
        {
            return AppServiceFactory.ReplenishStaff.BindMobile(this.CurrentUserId, this.CurrentUserId, rop);
        }


    }
}