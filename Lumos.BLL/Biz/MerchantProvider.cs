using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class MerchantProvider : BaseProvider
    {
        public CustomJsonResult Add(int operater, SysMerchatUser sysMerchatUser, Merchant merchant)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var isExists = CurrentDb.SysUser.Where(m => m.UserName == sysMerchatUser.UserName).FirstOrDefault();

                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "账号已经存在");
                }

                sysMerchatUser.PasswordHash = PassWordHelper.HashPassword(sysMerchatUser.Password);
                sysMerchatUser.SecurityStamp = Guid.NewGuid().ToString();
                sysMerchatUser.RegisterTime = this.DateTime;
                sysMerchatUser.CreateTime = this.DateTime;
                sysMerchatUser.Creator = operater;
                sysMerchatUser.Status = Enumeration.UserStatus.Normal;
                sysMerchatUser.Type = Enumeration.UserType.Client;
                CurrentDb.SysMerchatUser.Add(sysMerchatUser);
                CurrentDb.SaveChanges();


                merchant.UserId = sysMerchatUser.Id;
                merchant.CreateTime = this.DateTime;
                merchant.Creator = operater;
                merchant.ApiHost = "http://api.17fanju.com";
                merchant.ApiKey = "fanju";
                merchant.ApiSecret = "7460e6512f1940f68c00fe1fdb2b7eb1";
                merchant.PayTimeout = 120;

                CurrentDb.Merchant.Add(merchant);
                CurrentDb.SaveChanges();
                merchant.Sn = SnUtil.Build(merchant.Id);
                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "新建成功");

            }

            return result;
        }

        public CustomJsonResult Edit(int operater, SysMerchatUser sysMerchatUser, Merchant merchant)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var l_merchant = CurrentDb.Merchant.Where(m => m.Id == merchant.Id).FirstOrDefault();
                l_merchant.ContactName = merchant.ContactName;
                l_merchant.ContactPhone = merchant.ContactPhone;
                l_merchant.ContactAddress = merchant.ContactAddress;
                l_merchant.LastUpdateTime = this.DateTime;
                l_merchant.Mender = operater;

                if (!string.IsNullOrEmpty(sysMerchatUser.Password))
                {
                    var l_sysMerchatUser = CurrentDb.SysUser.Where(m => m.Id == l_merchant.UserId && m.UserName == sysMerchatUser.UserName).FirstOrDefault();
                    if (l_sysMerchatUser != null)
                    {
                        l_sysMerchatUser.PasswordHash = PassWordHelper.HashPassword(sysMerchatUser.Password);
                        CurrentDb.SaveChanges();
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "保存成功");
            }

            return result;
        }
    }
}
