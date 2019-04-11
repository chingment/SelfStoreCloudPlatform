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
    public class AdProvider : BaseProvider
    {
        public CustomJsonResult AddRelease(string operater, string merchantId, RopAdAddRelease rop)
        {
           
            var adRelease = new AdRelease();

            adRelease.Id = GuidUtil.New();
            adRelease.AdSpaceId = rop.AdSpaceId;
            adRelease.MerchantId = merchantId;
            adRelease.Title = rop.Title;
            adRelease.Url = rop.Url;
            adRelease.Priority = 0;
            adRelease.Status = Enumeration.AdReleaseStatus.Normal;
            adRelease.Creator = operater;
            adRelease.CreateTime = DateTime.Now;
            CurrentDb.AdRelease.Add(adRelease);
            CurrentDb.SaveChanges();

            SdkFactory.PushService.UpdateMachineBanner(merchantId);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult DeleteRelease(string operater, string merchantId, string id)
        {
            var adRelease = CurrentDb.AdRelease.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (adRelease != null)
            {
                adRelease.Status = Enumeration.AdReleaseStatus.Deleted;
                adRelease.Mender = operater;
                adRelease.MendTime = DateTime.Now;
                CurrentDb.SaveChanges();

                SdkFactory.PushService.UpdateMachineBanner(merchantId);
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }
    }
}
