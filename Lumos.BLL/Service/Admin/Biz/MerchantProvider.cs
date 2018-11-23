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
        public CustomJsonResult GetDetails(string pOperater, string merchantId)
        {
            var ret = new RetMerchantGetDetails();
            var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == merchantId).FirstOrDefault();
            if (sysMerchantUser != null)
            {
                ret.MerchantId = sysMerchantUser.Id ?? ""; ;
                ret.UserName = sysMerchantUser.UserName ?? ""; ;
                ret.MerchantName = sysMerchantUser.MerchantName ?? ""; ;
                ret.ContactAddress = sysMerchantUser.ContactAddress ?? "";
                ret.ContactName = sysMerchantUser.ContactName ?? "";
                ret.ContactPhone = sysMerchantUser.ContactPhone ?? "";
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, RopMerchantAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var isExistsSysUser = CurrentDb.SysUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();

                if (isExistsSysUser != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "账号已经存在");
                }

                var sysMerchatUser = new SysMerchantUser();
                sysMerchatUser.Id = GuidUtil.New();
                sysMerchatUser.UserName = rop.UserName;
                sysMerchatUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                sysMerchatUser.MerchantName = rop.MerchantName;
                sysMerchatUser.ContactName = rop.ContactName;
                sysMerchatUser.ContactPhone = rop.ContactPhone;
                sysMerchatUser.ContactAddress = rop.ContactAddress;
                sysMerchatUser.SecurityStamp = Guid.NewGuid().ToString();
                sysMerchatUser.RegisterTime = this.DateTime;
                sysMerchatUser.CreateTime = this.DateTime;
                sysMerchatUser.Creator = pOperater;
                sysMerchatUser.Status = Enumeration.UserStatus.Normal;
                sysMerchatUser.BelongSite = Enumeration.BelongSite.Merchant;
                CurrentDb.SysMerchantUser.Add(sysMerchatUser);
                CurrentDb.SaveChanges();

                var merchantConfig = new MerchantConfig();
                merchantConfig.Id = GuidUtil.New();
                merchantConfig.MerchantId = sysMerchatUser.Id;
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

        public CustomJsonResult Edit(string pOperater, RopMerchantEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == rop.MerchantId).FirstOrDefault();
                sysMerchantUser.ContactName = rop.ContactName;
                sysMerchantUser.ContactPhone = rop.ContactPhone;
                sysMerchantUser.ContactAddress = rop.ContactAddress;
                sysMerchantUser.MendTime = this.DateTime;
                sysMerchantUser.Mender = pOperater;

                if (!string.IsNullOrEmpty(rop.Password))
                {
                    sysMerchantUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "保存成功");
            }

            return result;
        }


        public CustomJsonResult BindOnMachine(string pOperater, string pMerchantId, string pMachineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMachine = CurrentDb.Machine.Where(m => m.Id == pMachineId).FirstOrDefault();

                if (lMachine.IsUse)
                {
                    return new CustomJsonResult(ResultType.Failure, "该设备已经被绑定，未被解绑");
                }

                lMachine.IsUse = true;
                lMachine.MendTime = this.DateTime;
                lMachine.Mender = pOperater;

                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == pMerchantId && m.MachineId == pMachineId).FirstOrDefault();
                if (lMerchantMachine == null)
                {
                    lMerchantMachine = new MerchantMachine();
                    lMerchantMachine.Id = GuidUtil.New();
                    lMerchantMachine.MerchantId = pMerchantId;
                    lMerchantMachine.MachineId = pMachineId;
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.CreateTime = this.DateTime;
                    lMerchantMachine.Creator = pOperater;
                    CurrentDb.MerchantMachine.Add(lMerchantMachine);
                }
                else
                {
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.MendTime = this.DateTime;
                    lMerchantMachine.Mender = pOperater;
                }

                lMerchantMachine.LogoImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/LogoImg.png";
                lMerchantMachine.BtnBuyImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnBuyImg.png";
                lMerchantMachine.BtnPickImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnPickImg.png";

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "绑定成功");
            }

            return result;
        }

        public CustomJsonResult BindOffMachine(string pOperater, string pMerchantId, string pMachineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == pMerchantId && m.MachineId == pMachineId).FirstOrDefault();

                if (lMerchantMachine.IsBind == false)
                {

                    return new CustomJsonResult(ResultType.Failure, "该设备已经被解绑");
                }

                lMerchantMachine.IsBind = false;
                lMerchantMachine.MendTime = this.DateTime;
                lMerchantMachine.Mender = pOperater;

                var machine = CurrentDb.Machine.Where(m => m.Id == pMachineId).FirstOrDefault();

                machine.IsUse = false;
                machine.MendTime = this.DateTime;
                machine.Mender = pOperater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "解绑成功");
            }

            return result;
        }
    }
}
