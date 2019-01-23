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
            var merchant = CurrentDb.Merchant.Where(m => m.Id == id).FirstOrDefault();
            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == merchant.UserId).FirstOrDefault();

            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }


            ret.Id = merchant.Id ?? ""; ;
            ret.UserName = sysMerchantUser.UserName ?? "";
            ret.Name = merchant.Name ?? "";
            ret.ContactAddress = merchant.ContactAddress ?? "";
            ret.ContactName = merchant.ContactName ?? "";
            ret.ContactPhone = merchant.ContactPhone ?? "";
            ret.SimpleCode = merchant.SimpleCode;
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

                var isExistsSimpleCode = CurrentDb.Merchant.Where(m => m.SimpleCode == rop.SimpleCode).FirstOrDefault();

                if (isExistsSimpleCode != null)
                {
                    return new CustomJsonResult(ResultType.Failure, string.Format("该商户代码（{0}）已经被使用", rop.SimpleCode));
                }

                string merchantId = GuidUtil.New();
                var sysMerchatUser = new SysMerchantUser();
                sysMerchatUser.Id = GuidUtil.New();
                sysMerchatUser.UserName = string.Format("M{0}", rop.UserName);
                sysMerchatUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                sysMerchatUser.SecurityStamp = Guid.NewGuid().ToString();
                sysMerchatUser.RegisterTime = this.DateTime;
                sysMerchatUser.Status = Enumeration.UserStatus.Normal;
                sysMerchatUser.BelongSite = Enumeration.BelongSite.Merchant;
                sysMerchatUser.PositionId = Enumeration.SysPositionId.MerchantAdministrator;
                sysMerchatUser.CreateTime = this.DateTime;
                sysMerchatUser.Creator = operater;
                sysMerchatUser.MerchantId = merchantId;
                CurrentDb.SysMerchantUser.Add(sysMerchatUser);


                var merchant = new Merchant();
                merchant.Id = merchantId;
                merchant.UserId = sysMerchatUser.Id;
                merchant.Name = rop.Name;
                merchant.ContactName = rop.ContactName;
                merchant.ContactPhone = rop.ContactPhone;
                merchant.ContactAddress = rop.ContactAddress;
                merchant.ApiHost = "http://demo.api.term.17fanju.com";
                merchant.ApiKey = "fanju";
                merchant.ApiSecret = "7460e6512f1940f68c00fe1fdb2b7eb1";
                merchant.PayTimeout = 120;
                merchant.SimpleCode = rop.SimpleCode;
                merchant.CreateTime = this.DateTime;
                merchant.Creator = operater;
                CurrentDb.Merchant.Add(merchant);

                var sysRole = CurrentDb.SysRole.Where(m => m.BelongSite == Enumeration.BelongSite.Merchant && m.Dept == 0).FirstOrDefault();
                if (sysRole == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "初始角色未指定");
                }

                var productKind = new ProductKind();
                productKind.Id = GuidUtil.New();
                productKind.MerchantId = merchant.Id;
                productKind.Name = "我的品类";
                productKind.PId = GuidUtil.Empty();
                productKind.Dept = 0;
                productKind.Status = Enumeration.ProductKindStatus.Valid;
                productKind.Priority = 0;
                productKind.IsDelete = false;
                productKind.CreateTime = this.DateTime;
                productKind.Creator = operater;
                CurrentDb.ProductKind.Add(productKind);


                var productSubject = new ProductSubject();
                productSubject.Id = GuidUtil.New();
                productSubject.PId = GuidUtil.Empty();
                productSubject.MerchantId = merchant.Id;
                productSubject.Name = "我的栏目";
                productSubject.Status = Enumeration.ProductSubjectStatus.Valid;
                productSubject.Priority = 0;
                productSubject.IsDelete = false;
                productSubject.CreateTime = this.DateTime;
                productSubject.Creator = operater;
                CurrentDb.ProductSubject.Add(productSubject);


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

                var merchant = CurrentDb.Merchant.Where(m => m.Id == rop.Id).FirstOrDefault();

                if (merchant == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == merchant.UserId).FirstOrDefault();
                if (sysMerchantUser == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                merchant.ContactName = rop.ContactName;
                merchant.ContactPhone = rop.ContactPhone;
                merchant.ContactAddress = rop.ContactAddress;
                merchant.MendTime = this.DateTime;
                merchant.Mender = operater;

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
