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
        public CustomJsonResult Add(string pOperater, SysMerchatUser pSysMerchatUser, Merchant pMerchant)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var isExistsSysUser = CurrentDb.SysUser.Where(m => m.UserName == pSysMerchatUser.UserName).FirstOrDefault();

                if (isExistsSysUser != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "账号已经存在");
                }
                pSysMerchatUser.Id = GuidUtil.New();
                pSysMerchatUser.PasswordHash = PassWordHelper.HashPassword(pSysMerchatUser.Password);
                pSysMerchatUser.SecurityStamp = Guid.NewGuid().ToString();
                pSysMerchatUser.RegisterTime = this.DateTime;
                pSysMerchatUser.CreateTime = this.DateTime;
                pSysMerchatUser.Creator = pOperater;
                pSysMerchatUser.Status = Enumeration.UserStatus.Normal;
                pSysMerchatUser.Type = Enumeration.UserType.Merchant;
                CurrentDb.SysMerchatUser.Add(pSysMerchatUser);
                CurrentDb.SaveChanges();

                pMerchant.Id = GuidUtil.New();
                pMerchant.UserId = pSysMerchatUser.Id;
                pMerchant.CreateTime = this.DateTime;
                pMerchant.Creator = pOperater;
                pMerchant.ApiHost = "http://api.17fanju.com";
                pMerchant.ApiKey = "fanju";
                pMerchant.ApiSecret = "7460e6512f1940f68c00fe1fdb2b7eb1";
                pMerchant.PayTimeout = 120;

                CurrentDb.Merchant.Add(pMerchant);
                CurrentDb.SaveChanges();
                //merchant.Sn = SnUtil.Build(merchant.Id);
                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "新建成功");

            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, SysMerchatUser pSysMerchatUser, Merchant pMerchant)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var lMerchant = CurrentDb.Merchant.Where(m => m.Id == pMerchant.Id).FirstOrDefault();
                lMerchant.ContactName = pMerchant.ContactName;
                lMerchant.ContactPhone = pMerchant.ContactPhone;
                lMerchant.ContactAddress = pMerchant.ContactAddress;
                lMerchant.MendTime = this.DateTime;
                lMerchant.Mender = pOperater;

                if (!string.IsNullOrEmpty(pSysMerchatUser.Password))
                {
                    var l_sysMerchatUser = CurrentDb.SysUser.Where(m => m.Id == lMerchant.UserId && m.UserName == pSysMerchatUser.UserName).FirstOrDefault();
                    if (l_sysMerchatUser != null)
                    {
                        l_sysMerchatUser.PasswordHash = PassWordHelper.HashPassword(pSysMerchatUser.Password);
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
