using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class UserDeliveryAddressController : OwnBaseApiController
    {

        [HttpGet]
        public APIResponse My()
        {
            var query = (from o in CurrentDb.UserDeliveryAddress
                         where
                         o.UserId == this.CurrentUserId &&
                         o.IsDelete == false
                         select new { o.Id, o.Consignee, o.PhoneNumber, o.Address, o.AreaName, o.AreaCode, o.IsDefault, o.CreateTime }
              );



            query = query.OrderByDescending(r => r.CreateTime);

            var list = query.ToList();


            var model = new List<object>();


            foreach (var m in list)
            {

                model.Add(new
                {
                    m.Id,
                    m.Consignee,
                    m.PhoneNumber,
                    m.Address,
                    m.AreaName,
                    m.AreaCode,
                    m.IsDefault
                });
            }

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }

        [HttpPost]
        public APIResponse Edit(RopUserDeliveryAddressEdit rop)
        {
            var userDeliveryAddress = new UserDeliveryAddress();
            userDeliveryAddress.Id = rop.Id;
            userDeliveryAddress.UserId = this.CurrentUserId;
            userDeliveryAddress.PhoneNumber = rop.PhoneNumber;
            userDeliveryAddress.Consignee = rop.Consignee;
            userDeliveryAddress.AreaName = rop.AreaName;
            userDeliveryAddress.Address = rop.Address;
            userDeliveryAddress.IsDefault = rop.IsDefault;
            IResult result = AppServiceFactory.UserDeliveryAddress.Edit(this.CurrentUserId, userDeliveryAddress);
            return new APIResponse(result);
        }
    }
}