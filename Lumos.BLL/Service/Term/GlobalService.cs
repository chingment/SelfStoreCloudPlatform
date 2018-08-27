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
        public CustomJsonResult DataSet(string operater, string userId, string merchantId, string machineId, DateTime? datetime)
        {
            CustomJsonResult result = new CustomJsonResult();
            var model = new DataSetModel();

            var machine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId && m.IsBind==true).FirstOrDefault();


            model.LogoImgUrl = machine.LogoImgUrl;
            model.BtnBuyImgUrl = machine.BtnBuyImgUrl;
            model.BtnPickImgUrl = machine.BtnPickImgUrl;


            model.Banners = TermServiceFactory.Machine.GetBanners(userId, userId, machineId);
            model.ProductKinds = TermServiceFactory.ProductKind.GetKinds(userId, userId, machineId);
            model.ProductSkus = TermServiceFactory.Machine.GetProductSkus(userId, userId, machineId);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }
    }
}
