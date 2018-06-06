using Lumos.BLL.Service.Term.Models;
using Lumos.BLL.Service.Term.Models.Global;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class GlobalService : BaseProvider
    {
        public CustomJsonResult DataSet(int operater, int merchantId, int machineId, DateTime? datetime)
        {
            CustomJsonResult result = new CustomJsonResult();
            var model = new DataSetModel();

            var machine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId && m.Status == Entity.Enumeration.MerchantMachineStatus.Bind).FirstOrDefault();


            model.LogoImgUrl = machine.LogoImgUrl;
            model.BtnBuyImgUrl = machine.BtnBuyImgUrl;
            model.BtnPickImgUrl = machine.BtnPickImgUrl;

            #region banner
            var banner = CurrentDb.MachineBanner.Where(m => m.MerchantId == merchantId && m.MachineId == machineId && m.Status == Entity.Enumeration.MachineBannerStatus.Normal).OrderByDescending(m => m.Priority).ToList();

            foreach (var item in banner)
            {
                model.Banner.Add(new BannerModel { ImgUrl = item.ImgUrl });
            }
            #endregion banner


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }
    }
}
