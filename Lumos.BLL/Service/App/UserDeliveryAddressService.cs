using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.App
{
    public class UserDeliveryAddressService : BaseProvider
    {
        public CustomJsonResult Edit(string operater, UserDeliveryAddress shippingAddress)
        {
            CustomJsonResult result = new CustomJsonResult();


            if (string.IsNullOrEmpty(shippingAddress.Id))
            {
                shippingAddress.CreateTime = this.DateTime;
                shippingAddress.Creator = operater;
                CurrentDb.UserDeliveryAddress.Add(shippingAddress);
                CurrentDb.SaveChanges();

            }
            else
            {

                var l_shippingAddress = CurrentDb.UserDeliveryAddress.Where(m => m.Id == shippingAddress.Id).FirstOrDefault();

                l_shippingAddress.Consignee = shippingAddress.Consignee;
                l_shippingAddress.PhoneNumber = shippingAddress.PhoneNumber;
                l_shippingAddress.AreaName = shippingAddress.AreaName;
                l_shippingAddress.Address = shippingAddress.Address;
                l_shippingAddress.IsDefault = shippingAddress.IsDefault;
                CurrentDb.SaveChanges();
            }

            if (shippingAddress.IsDefault)
            {
                var list = CurrentDb.UserDeliveryAddress.Where(m => m.UserId == shippingAddress.UserId).ToList();


                foreach (var item in list)
                {
                    if (item.Id != shippingAddress.Id)
                    {
                        item.IsDefault = false;
                        CurrentDb.SaveChanges();
                    }
                }
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
        }
    }
}
