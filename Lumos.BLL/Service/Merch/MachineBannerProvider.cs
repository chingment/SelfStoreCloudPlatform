using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class MachineBannerProvider : BaseProvider
    {
        public CustomJsonResult Add(string operater, string merchantId, RopMachineBannerAdd rop)
        {
            var machine = CurrentDb.Machine.Where(m => m.MerchantId == merchantId && m.Id == rop.MachineId).FirstOrDefault();

            var machineBanner = new MachineBanner();

            machineBanner.Id = GuidUtil.New();
            machineBanner.MerchantId = machine.MerchantId;
            machineBanner.StoreId = machine.StoreId;
            machineBanner.MachineId = rop.MachineId;
            machineBanner.Title = rop.Title;
            machineBanner.ImgUrl = rop.ImgUrl;
            machineBanner.Priority = 0;
            machineBanner.Status = Enumeration.MachineBannerStatus.Normal;
            machineBanner.Creator = operater;
            machineBanner.CreateTime = DateTime.Now;
            CurrentDb.MachineBanner.Add(machineBanner);
            CurrentDb.SaveChanges();

            SdkFactory.PushService.UpdateMachineBanner(machine.Id);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult Delete(string operater, string merchantId, string id)
        {
            var machineBanner = CurrentDb.MachineBanner.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (machineBanner != null)
            {
                machineBanner.Status = Enumeration.MachineBannerStatus.Deleted;
                machineBanner.Mender = operater;
                machineBanner.MendTime = DateTime.Now;
                CurrentDb.SaveChanges();

                SdkFactory.PushService.UpdateMachineBanner(machineBanner.MachineId);
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }
    }
}
