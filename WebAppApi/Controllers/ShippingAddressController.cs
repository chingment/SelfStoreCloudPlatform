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
using WebAppApi.Models.ShippingAddress;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class ShippingAddressController : OwnBaseApiController
    {

        [HttpGet]
        public APIResponse GetList(string userId)
        {
            var query = (from o in CurrentDb.UserDeliveryAddress
                         where
                         o.UserId == userId &&
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
        public APIResponse Edit(EditModel model)
        {
            var userDeliveryAddress = new UserDeliveryAddress();
            userDeliveryAddress.Id = model.Id;
            userDeliveryAddress.UserId = model.UserId;
            userDeliveryAddress.PhoneNumber = model.PhoneNumber;
            userDeliveryAddress.Consignee = model.Consignee;
            userDeliveryAddress.AreaName = model.AreaName;
            userDeliveryAddress.Address = model.Address;
            userDeliveryAddress.IsDefault = model.IsDefault;
            IResult result = AppServiceFactory.UserDeliveryAddress.Edit(model.UserId, userDeliveryAddress);
            return new APIResponse(result);
        }
    }
}