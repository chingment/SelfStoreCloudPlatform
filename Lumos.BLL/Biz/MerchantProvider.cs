using Lumos.DAL;
using Lumos.Entity;
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
        public CustomJsonResult Add(string pOperater, SysMerchantUser pSysMerchatUser)
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
                pSysMerchatUser.SecurityStamp = Guid.NewGuid().ToString();
                pSysMerchatUser.RegisterTime = this.DateTime;
                pSysMerchatUser.CreateTime = this.DateTime;
                pSysMerchatUser.Creator = pOperater;
                pSysMerchatUser.Status = Enumeration.UserStatus.Normal;
                pSysMerchatUser.Type = Enumeration.UserType.Merchant;
                CurrentDb.SysMerchantUser.Add(pSysMerchatUser);
                CurrentDb.SaveChanges();

                var merchantConfig = new MerchantConfig();
                merchantConfig.Id = GuidUtil.New();
                merchantConfig.MerchantId = pSysMerchatUser.Id;
                merchantConfig.CreateTime = this.DateTime;
                merchantConfig.Creator = pOperater;
                merchantConfig.ApiHost = "http://demo.api.term.17fanju.com";
                merchantConfig.ApiKey = "fanju";
                merchantConfig.ApiSecret = "7460e6512f1940f68c00fe1fdb2b7eb1";
                merchantConfig.PayTimeout = 120;
                CurrentDb.MerchantConfig.Add(merchantConfig);
                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "新建成功");

            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, SysMerchantUser pSysMerchatUser)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var lSysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == pSysMerchatUser.Id).FirstOrDefault();
                lSysMerchantUser.ContactName = pSysMerchatUser.ContactName;
                lSysMerchantUser.ContactPhone = pSysMerchatUser.ContactPhone;
                lSysMerchantUser.ContactAddress = pSysMerchatUser.ContactAddress;
                lSysMerchantUser.MendTime = this.DateTime;
                lSysMerchantUser.Mender = pOperater;

                //if (!string.IsNullOrEmpty(pSysMerchatUser.Password))
                //{
                //    lSysMerchantUser.PasswordHash = PassWordHelper.HashPassword(pSysMerchatUser.Password);
                //}

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "保存成功");
            }

            return result;
        }



    }
}
