using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class SysUserService : BaseProvider
    {
        public CustomJsonResult GetDetails(string pOperater, string userId)
        {
            var ret = new RetSysUserGetDetails();
            var sysUser = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();
            if (sysUser != null)
            {
                ret.UserName = sysUser.UserName ?? "";
                ret.FullName = sysUser.FullName ?? "";
                ret.Email = sysUser.Email ?? "";
                ret.PhoneNumber = sysUser.PhoneNumber ?? "";
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }
    }
}
