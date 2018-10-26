using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace WebAppApi.Controllers
{

    public class GlobalController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse DataSet([FromUri]RupGlobalDataSet rup)
        {
            var ret = new RetGobalDataSet();
            ret.Index = AppServiceFactory.Index.GetPageData(this.CurrentUserId, this.CurrentUserId, rup.StoreId);
            ret.ProductKind = AppServiceFactory.ProductKind.GetPageData(this.CurrentUserId, this.CurrentUserId, rup.StoreId);
            ret.Cart = AppServiceFactory.Cart.GetPageData(this.CurrentUserId, this.CurrentUserId, rup.StoreId);
            ret.Personal = AppServiceFactory.Personal.GetPageData(this.CurrentUserId, this.CurrentUserId, rup.StoreId);
            OwnApiHttpResult result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = ret };
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse AccessToken([FromUri]RupGlobalAccessToken rup)
        {
            //var resultModel = new DataSetResultModel();
            //resultModel.Index = AppServiceFactory.Index.GetData(userId, userId, storeId);
            //resultModel.ProductKind = AppServiceFactory.ProductKind.GetKinds(userId, userId, storeId);
            //resultModel.Cart = AppServiceFactory.Cart.GetData(userId, userId, storeId);
            //resultModel.Personal = AppServiceFactory.Personal.GetData(userId, userId, storeId);
            //APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = resultModel };
            //return new APIResponse(result);
            return null;
        }

    }
}