using Lumos;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.Entity;
using Lumos.Mvc;
using System.Web.Http;


namespace WebAppApi.Controllers
{

    public class CartController : OwnApiBaseController
    {
        [HttpPost]
        public OwnApiHttpResponse Operate([FromBody]RopCartOperate rop)
        {
            IResult result = AppServiceFactory.Cart.Operate(this.CurrentUserId, this.CurrentUserId, rop);

            return new OwnApiHttpResponse(result);

        }

        public OwnApiHttpResponse GetPageData([FromUri]RupCartPageData rup)
        {
            var data = AppServiceFactory.Cart.GetPageData(this.CurrentUserId, this.CurrentUserId, rup.StoreId);

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", data);
        }
    }
}