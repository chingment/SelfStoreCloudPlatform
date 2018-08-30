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
        public CustomJsonResult DataSet(string pOperater, string pUserId, string pMerchantId, string pMachineId, DateTime? pDatetime)
        {
            CustomJsonResult result = new CustomJsonResult();
            var model = new DataSetModel();

            var machine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == pMerchantId && m.MachineId == pMachineId && m.IsBind == true).FirstOrDefault();


            model.LogoImgUrl = machine.LogoImgUrl;
            model.BtnBuyImgUrl = machine.BtnBuyImgUrl;
            model.BtnPickImgUrl = machine.BtnPickImgUrl;


            model.Banners = TermServiceFactory.Machine.GetBanners(pUserId, pUserId, pMachineId);
            model.ProductKinds = TermServiceFactory.ProductKind.GetKinds(pUserId, pUserId, pMachineId);
            model.ProductSkus = TermServiceFactory.Machine.GetProductSkus(pUserId, pUserId, pMachineId);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }
    }
}
