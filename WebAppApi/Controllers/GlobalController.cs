using Lumos.BLL;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models;
namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class GlobalController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse DataSet(int userId, int merchantId, int posMachineId, DateTime? datetime)
        {

            //var model = new DataSetModel();

            //model.Index = ServiceFactory.Index.GetData(userId);
            //model.ProductKind = ServiceFactory.Product.GetKinds();
            //model.Cart = ServiceFactory.Cart.GetData(userId);
            //model.Personal = ServiceFactory.Personal.GetData(userId);
            //APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };
            //return new APIResponse(result);

            return null;
        }
    }
}