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
    public class MerchantMachineProvider : BaseProvider
    {
        public CustomJsonResult Bind(string operater, string merchantId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();

                if (machine.IsUse)
                {
                    return new CustomJsonResult(ResultType.Failure, "该设备已经被绑定，未被解绑");
                }

                machine.IsUse = true;
                machine.LastUpdateTime = this.DateTime;
                machine.Mender = operater;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantId).FirstOrDefault();
                var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId).FirstOrDefault();
                if (merchantMachine == null)
                {
                    merchantMachine = new MerchantMachine();
                    merchantMachine.Id = GuidUtil.New();
                    merchantMachine.UserId = merchant.UserId;
                    merchantMachine.MerchantId = merchantId;
                    merchantMachine.MachineId = machineId;
                    merchantMachine.Status = Enumeration.MerchantMachineStatus.Bind;
                    merchantMachine.CreateTime = this.DateTime;
                    merchantMachine.Creator = operater;
                    CurrentDb.MerchantMachine.Add(merchantMachine);
                }
                else
                {
                    merchantMachine.Status = Enumeration.MerchantMachineStatus.Bind;
                    merchantMachine.LastUpdateTime = this.DateTime;
                    merchantMachine.Mender = operater;
                }

                merchantMachine.LogoImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/LogoImg.png";
                merchantMachine.BtnBuyImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnBuyImg.png";
                merchantMachine.BtnPickImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnPickImg.png";

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "绑定成功");
            }

            return result;
        }

        public CustomJsonResult UnBind(string operater, string merchantId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId).FirstOrDefault();

                if (merchantMachine.Status == Enumeration.MerchantMachineStatus.Unbind)
                {

                    return new CustomJsonResult(ResultType.Failure, "该设备已经被解绑");
                }

                merchantMachine.Status = Enumeration.MerchantMachineStatus.Unbind;
                merchantMachine.LastUpdateTime = this.DateTime;
                merchantMachine.Mender = operater;

                var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();

                machine.IsUse = false;
                machine.LastUpdateTime = this.DateTime;
                machine.Mender = operater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "解绑成功");
            }

            return result;
        }
    }
}
