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
    public class MerchantProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetMerchantGetDetails();
            var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == id).FirstOrDefault();
            if (sysMerchantUser == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            var merchantInfo = CurrentDb.MerchantInfo.Where(m => m.MerchantId == id).FirstOrDefault();

            if (merchantInfo == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }


            ret.Id = sysMerchantUser.Id ?? ""; ;
            ret.UserName = sysMerchantUser.UserName ?? "";
            ret.Name = merchantInfo.Name ?? "";
            ret.ContactAddress = merchantInfo.ContactAddress ?? "";
            ret.ContactName = merchantInfo.ContactName ?? "";
            ret.ContactPhone = merchantInfo.ContactPhone ?? "";

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string operater, RopMerchantAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var isExistsSysUser = CurrentDb.SysUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();

                if (isExistsSysUser != null)
                {
                    return new CustomJsonResult(ResultType.Failure, string.Format("该用户名（{0}）已经被使用", rop.UserName));
                }

                var sysMerchatUser = new SysMerchantUser();
                sysMerchatUser.Id = GuidUtil.New();
                sysMerchatUser.UserName = rop.UserName;
                sysMerchatUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                sysMerchatUser.SecurityStamp = Guid.NewGuid().ToString();
                sysMerchatUser.RegisterTime = this.DateTime;
                sysMerchatUser.Status = Enumeration.UserStatus.Normal;
                sysMerchatUser.BelongSite = Enumeration.BelongSite.Merchant;
                sysMerchatUser.CreateTime = this.DateTime;
                sysMerchatUser.Creator = operater;
                CurrentDb.SysMerchantUser.Add(sysMerchatUser);
                CurrentDb.SaveChanges();

                var merchantInfo = new MerchantInfo();
                merchantInfo.Id = sysMerchatUser.Id;
                merchantInfo.MerchantId = sysMerchatUser.Id;
                merchantInfo.Name = rop.Name;
                merchantInfo.ContactName = rop.ContactName;
                merchantInfo.ContactPhone = rop.ContactPhone;
                merchantInfo.ContactAddress = rop.ContactAddress;
                merchantInfo.ApiHost = "http://demo.api.term.17fanju.com";
                merchantInfo.ApiKey = "fanju";
                merchantInfo.ApiSecret = "7460e6512f1940f68c00fe1fdb2b7eb1";
                merchantInfo.PayTimeout = 120;
                merchantInfo.CreateTime = this.DateTime;
                merchantInfo.Creator = operater;
                CurrentDb.MerchantInfo.Add(merchantInfo);


                var sysRole = CurrentDb.SysRole.Where(m => m.BelongSite == Enumeration.BelongSite.Merchant && m.IsCanDelete == false).FirstOrDefault();
                if (sysRole == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "初始角色未指定");
                }


                var sysUserRole = new SysUserRole();
                sysUserRole.Id = GuidUtil.New();
                sysUserRole.RoleId = sysRole.Id;
                sysUserRole.UserId = sysMerchatUser.Id;
                sysUserRole.CreateTime = this.DateTime;
                sysUserRole.Creator = operater;
                sysUserRole.IsCanDelete = false;
                CurrentDb.SysUserRole.Add(sysUserRole);


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "新建成功");

            }

            return result;
        }

        public CustomJsonResult Edit(string operater, RopMerchantEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == rop.Id).FirstOrDefault();
                if (sysMerchantUser == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                var merchantInfo = CurrentDb.MerchantInfo.Where(m => m.MerchantId == rop.Id).FirstOrDefault();

                if (merchantInfo == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                merchantInfo.ContactName = rop.ContactName;
                merchantInfo.ContactPhone = rop.ContactPhone;
                merchantInfo.ContactAddress = rop.ContactAddress;
                merchantInfo.MendTime = this.DateTime;
                merchantInfo.Mender = operater;

                if (!string.IsNullOrEmpty(rop.Password))
                {
                    sysMerchantUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                    sysMerchantUser.MendTime = this.DateTime;
                    sysMerchantUser.Mender = operater;
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "保存成功");
            }

            return result;
        }

    }
}
