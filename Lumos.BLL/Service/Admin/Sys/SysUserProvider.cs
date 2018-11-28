using System;
using System.Linq;

namespace Lumos.BLL.Service.Admin
{
    public class SysUserProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetSysUserGetDetails();
            var sysUser = CurrentDb.SysUser.Where(m => m.Id == id).FirstOrDefault();
            if (sysUser != null)
            {
                ret.UserName = sysUser.UserName ?? "";
                ret.FullName = sysUser.FullName ?? "";
                ret.Email = sysUser.Email ?? "";
                ret.PhoneNumber = sysUser.PhoneNumber ?? "";
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }


    }
}
