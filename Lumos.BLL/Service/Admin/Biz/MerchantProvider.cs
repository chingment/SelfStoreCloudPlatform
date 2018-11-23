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
            if (sysMerchantUser != null)
            {
                ret.Id = sysMerchantUser.Id ?? ""; ;
                ret.UserName = sysMerchantUser.UserName ?? "";

                var merchantInfo = CurrentDb.MerchantInfo.Where(m => m.MerchantId == id).FirstOrDefault();

                ret.MerchantName = merchantInfo.Name ?? "";
                ret.ContactAddress = merchantInfo.ContactAddress ?? "";
                ret.ContactName = merchantInfo.ContactName ?? "";
                ret.ContactPhone = merchantInfo.ContactPhone ?? "";
            }

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
                sysMerchatUser.CreateTime = this.DateTime;
                sysMerchatUser.Creator = operater;
                sysMerchatUser.Status = Enumeration.UserStatus.Normal;
                sysMerchatUser.BelongSite = Enumeration.BelongSite.Merchant;
                CurrentDb.SysMerchantUser.Add(sysMerchatUser);
                CurrentDb.SaveChanges();

                var merchantInfo = new MerchantInfo();
                merchantInfo.Id = sysMerchatUser.Id;
                merchantInfo.MerchantId = sysMerchatUser.Id;
                merchantInfo.Name = rop.MerchantName;
                merchantInfo.ContactName = rop.ContactName;
                merchantInfo.ContactPhone = rop.ContactPhone;
                merchantInfo.ContactAddress = rop.ContactAddress;
                merchantInfo.CreateTime = this.DateTime;
                merchantInfo.Creator = operater;
                merchantInfo.ApiHost = "http://demo.api.term.17fanju.com";
                merchantInfo.ApiKey = "fanju";
                merchantInfo.ApiSecret = "7460e6512f1940f68c00fe1fdb2b7eb1";
                merchantInfo.PayTimeout = 120;
                CurrentDb.MerchantInfo.Add(merchantInfo);
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
                var merchantInfo = CurrentDb.MerchantInfo.Where(m => m.Id == rop.Id).FirstOrDefault();

                merchantInfo.ContactName = rop.ContactName;
                merchantInfo.ContactPhone = rop.ContactPhone;
                merchantInfo.ContactAddress = rop.ContactAddress;


                merchantInfo.MendTime = this.DateTime;
                merchantInfo.Mender = operater;

                if (!string.IsNullOrEmpty(rop.Password))
                {
                    var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == rop.Id).FirstOrDefault();

                    sysMerchantUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);

                    CurrentDb.SaveChanges();
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "保存成功");
            }

            return result;
        }


        public CustomJsonResult BindOnMachine(string operater, string merchantId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMachine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();

                if (lMachine.IsUse)
                {
                    return new CustomJsonResult(ResultType.Failure, "该设备已经被绑定，未被解绑");
                }

                lMachine.IsUse = true;
                lMachine.MendTime = this.DateTime;
                lMachine.Mender = operater;

                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId).FirstOrDefault();
                if (lMerchantMachine == null)
                {
                    lMerchantMachine = new MerchantMachine();
                    lMerchantMachine.Id = GuidUtil.New();
                    lMerchantMachine.MerchantId = merchantId;
                    lMerchantMachine.MachineId = machineId;
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.CreateTime = this.DateTime;
                    lMerchantMachine.Creator = operater;
                    CurrentDb.MerchantMachine.Add(lMerchantMachine);
                }
                else
                {
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.MendTime = this.DateTime;
                    lMerchantMachine.Mender = operater;
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

        public CustomJsonResult BindOffMachine(string operater, string merchantId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId).FirstOrDefault();

                if (lMerchantMachine.IsBind == false)
                {

                    return new CustomJsonResult(ResultType.Failure, "该设备已经被解绑");
                }

                lMerchantMachine.IsBind = false;
                lMerchantMachine.MendTime = this.DateTime;
                lMerchantMachine.Mender = operater;

                var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();

                machine.IsUse = false;
                machine.MendTime = this.DateTime;
                machine.Mender = operater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "解绑成功");
            }

            return result;
        }
    }
}
