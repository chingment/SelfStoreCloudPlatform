using Lumos.DAL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Admin
{
    public class SysAdminUserProvider : BaseProvider
    {

        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetSysAdminUserGetDetails();


            var sysAdminUser = CurrentDb.SysAdminUser.Where(m => m.Id == id).FirstOrDefault();
            if (sysAdminUser != null)
            {
                ret.UserName = sysAdminUser.UserName ?? ""; ;
                ret.FullName = sysAdminUser.FullName ?? ""; ;
                ret.Email = sysAdminUser.Email ?? ""; ;
                ret.PhoneNumber = sysAdminUser.PhoneNumber ?? "";
                ret.PositionId = sysAdminUser.PositionId;
                ret.OrganizationId = sysAdminUser.OrganizationId;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult Add(string operater, RopSysAdminUserAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            var isExistUserName = CurrentDb.SysUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();
            if (isExistUserName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("该用户名（{0}）已被使用", rop.UserName));
            }

            using (TransactionScope ts = new TransactionScope())
            {
                var sysAdminUser = new SysAdminUser();
                sysAdminUser.Id = GuidUtil.New();
                sysAdminUser.UserName = string.Format("Up{0}", rop.UserName);
                sysAdminUser.FullName = rop.FullName;
                sysAdminUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                sysAdminUser.Email = rop.Email;
                sysAdminUser.PhoneNumber = rop.PhoneNumber;
                sysAdminUser.BelongSite = Enumeration.BelongSite.Admin;
                sysAdminUser.IsDelete = false;
                sysAdminUser.IsCanDelete = true;
                sysAdminUser.Status = Enumeration.UserStatus.Normal;
                sysAdminUser.RegisterTime = DateTime.Now;
                sysAdminUser.Status = Enumeration.UserStatus.Normal;
                sysAdminUser.SecurityStamp = Guid.NewGuid().ToString().Replace("-", "");
                sysAdminUser.PositionId = rop.PositionId;
                sysAdminUser.OrganizationId = rop.OrganizationId;
                sysAdminUser.Creator = operater;
                sysAdminUser.CreateTime = DateTime.Now;

                CurrentDb.SysAdminUser.Add(sysAdminUser);
                CurrentDb.SaveChanges();


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult Edit(string operater, RopSysAdminUserEdit rop)
        {

            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysAdminUser = CurrentDb.SysAdminUser.Where(m => m.Id == rop.Id).FirstOrDefault();
                if (!string.IsNullOrEmpty(rop.Password))
                {
                    sysAdminUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                }
                sysAdminUser.FullName = rop.FullName;
                sysAdminUser.Email = rop.Email;
                sysAdminUser.PhoneNumber = rop.PhoneNumber;
                sysAdminUser.MendTime = DateTime.Now;
                sysAdminUser.Mender = operater;
                sysAdminUser.PositionId = rop.PositionId;
                sysAdminUser.OrganizationId = rop.OrganizationId;
                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

            }
            return result;


        }


        public CustomJsonResult Delete(string operater, string[] ids)
        {
            if (ids == null)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到用户");

            if (ids.Length <= 0)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到用户");


            foreach (string userId in ids)
            {
                SysUser user = CurrentDb.SysUser.Find(userId);

                if (!user.IsCanDelete)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("不允许删除用户（{0}）", user.UserName));
                }

                user.IsDelete = true;
                user.Mender = operater;
                user.MendTime = DateTime.Now;


                var userRoles = CurrentDb.SysUserRole.Where(m => m.UserId == userId).ToList();
                foreach (var userRole in userRoles)
                {
                    CurrentDb.SysUserRole.Remove(userRole);
                }

                CurrentDb.SaveChanges();
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }
    }
}
