using Lumos.DAL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class SysStaffUserService : BaseProvider
    {
        public CustomJsonResult GetInitDataByAddView(string pOperater)
        {
            var ret = new RetSysStaffUserGetInitDataByAddView();

            ret.Roles = ConvertToZTreeJson2(CurrentDb.SysRole.ToArray(), "id", "pid", "name", "role");

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult GetInitDataByEditView(string pOperater, string userId)
        {
            var ret = new RetSysStaffUserGetInitDataByEditView();
            var staffUser = CurrentDb.SysStaffUser.Where(m => m.Id == userId).FirstOrDefault();
            if (staffUser != null)
            {
                var isCheckedIds = CurrentDb.SysUserRole.Where(x => x.UserId == userId).Select(x => x.RoleId);

                ret.Roles = ConvertToZTreeJson2(CurrentDb.SysRole.ToArray(), "id", "pid", "name", "role", isCheckedIds.ToArray());

                ret.UserName = staffUser.UserName ?? ""; ;
                ret.FullName = staffUser.FullName ?? ""; ;
                ret.Email = staffUser.Email ?? ""; ;
                ret.PhoneNumber = staffUser.PhoneNumber ?? ""; ;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, RopSysStaffUserAdd rop)
        {
            var sysStaffUser = new SysStaffUser();
            sysStaffUser.Id = GuidUtil.New();
            sysStaffUser.UserName = string.Format("Up{0}", rop.UserName);
            sysStaffUser.FullName = rop.FullName;
            sysStaffUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
            sysStaffUser.Email = rop.Email;
            sysStaffUser.PhoneNumber = rop.PhoneNumber;
            sysStaffUser.Type = Enumeration.UserType.Staff;
            sysStaffUser.IsDelete = false;
            sysStaffUser.Status = Enumeration.UserStatus.Normal;
            return SysFactory.AuthorizeRelay.CreateUser<SysStaffUser>(pOperater, sysStaffUser, rop.RoleIds);
        }

        public CustomJsonResult Edit(string pOperater, RopSysStaffUserEdit rop)
        {
            var sysStaffUser = new SysStaffUser();
            sysStaffUser.Id = rop.UserId;

            if (!string.IsNullOrEmpty(rop.Password))
            {
                sysStaffUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
            }

            sysStaffUser.FullName = rop.FullName;
            sysStaffUser.Email = rop.Email;
            sysStaffUser.PhoneNumber = rop.PhoneNumber;


            return SysFactory.AuthorizeRelay.UpdateUser<SysStaffUser>(pOperater, sysStaffUser, rop.RoleIds);
        }


        public CustomJsonResult Edit(string pOperater, string[] userIds)
        {
            return SysFactory.AuthorizeRelay.DeleteUser(pOperater, userIds);
        }
    }
}
