using System;
using System.Linq;

namespace Lumos.BLL.Service.Admin
{
    public class SysUserProvider : BaseProvider
    {
        public string GetFullName(string pId)
        {
            if (pId == null)
                return "";

            string fullName = "";
            var user = CurrentDb.SysUser.Where(m => m.Id == pId).FirstOrDefault();
            if (user != null)
            {
                fullName = user.FullName;
            }

            return fullName;
        }

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
