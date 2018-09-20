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
        public APIResponse GetList(int userId)
        {
            var query = (from o in CurrentDb.ShippingAddress
                         where
                         o.UserId == userId &&
                         o.IsDelete == false
                         select new { o.Id, o.Receiver, o.PhoneNumber, o.Address, o.Area, o.AreaCode, o.IsDefault, o.CreateTime }
              );



            query = query.OrderByDescending(r => r.CreateTime);

            var list = query.ToList();


            var model = new List<object>();


            foreach (var m in list)
            {

                model.Add(new
                {
                    m.Id,
                    m.Receiver,
                    m.PhoneNumber,
                    m.Address,
                    m.Area,
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
            var shippingAddress = new ShippingAddress();
            shippingAddress.Id = model.Id;
            shippingAddress.UserId = model.UserId;
            shippingAddress.PhoneNumber = model.PhoneNumber;
            shippingAddress.Receiver = model.Receiver;
            shippingAddress.Area = model.Area;
            shippingAddress.Address = model.Address;
            shippingAddress.IsDefault = model.IsDefault;
            IResult result = AppServiceFactory.ShippingAddress.Edit(model.UserId, shippingAddress);
            return new APIResponse(result);
        }
    }
}