using Lumos;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.Entity;
using Lumos.Mvc;
using System.Web.Http;
using WebAppApi.Models.Cart;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class CartController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse Operate(OperateParams model)
        {
            IResult result = AppServiceFactory.Cart.Operate(model.UserId, model.Operate, model.UserId, model.StoreId, model.List);

            return new APIResponse(result);

        }
    }
}