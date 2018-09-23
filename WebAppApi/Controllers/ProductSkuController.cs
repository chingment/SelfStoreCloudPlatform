using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class ProductSkuController : OwnBaseApiController
    {

        [HttpGet]
        public APIResponse List(RupProductSkuList rup)
        {
            var model = AppServiceFactory.ProductSku.List(this.CurrentUserId, this.CurrentUserId, rup);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);

        }


        [HttpGet]
        public APIResponse Details(RupProductSkuDetails rup)
        {
            var model = BizFactory.ProductSku.GetModel(rup.SkuId);

            var sku = new SkuModel();

            sku.SkuId = model.Id;
            sku.SkuName = model.Name;
            sku.SalePrice = model.SalePrice.ToF2Price();
            sku.ShowPrice = model.ShowPrice.ToF2Price();
            sku.DetailsDes = model.DetailsDes;
            sku.BriefIntro = model.BriefInfo;
            sku.DispalyImgUrls = BizFactory.ProductSku.GetDispalyImgUrls(model.DispalyImgUrls);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = sku };

            return new APIResponse(result);
        }

    }
}