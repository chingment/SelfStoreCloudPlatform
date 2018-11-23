using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class ReplenishStaffService : BaseProvider
    {
        public CustomJsonResult BindMobile(string operater, string clientId, RopReplenishStaffBindMobile rup)
        {
            if (!BizFactory.Sms.ReplenishStaffBindMobileCheckValidCode(rup.PhoneNumber, rup.VerifyCode))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "验证码不正确");
            }



            var sysUser = CurrentDb.SysUser.Where(m => m.Id == clientId).FirstOrDefault();


            sysUser.PhoneNumber = rup.PhoneNumber;

            CurrentDb.SaveChanges();

            var ret = new RetOperateResult();
            ret.Result = RetOperateResult.ResultType.Success;
            ret.Remarks = "";
            ret.Message = "操作成功";
            ret.IsComplete = true;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);
        }
    }
}
