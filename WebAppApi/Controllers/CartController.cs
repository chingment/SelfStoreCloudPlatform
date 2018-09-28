using Lumos;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.Entity;
using Lumos.Mvc;
using System.Web.Http;


namespace WebAppApi.Controllers
{

    public class CartController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse Operate([FromBody]RopCartOperate rop)
        {
            IResult result = AppServiceFactory.Cart.Operate(this.CurrentUserId, this.CurrentUserId, rop);

            return new APIResponse(result);

        }
    }
}