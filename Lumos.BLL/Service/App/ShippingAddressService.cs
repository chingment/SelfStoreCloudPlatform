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
    public class ShippingAddressService : BaseProvider
    {





        public CustomJsonResult Edit(int operater, ShippingAddress shippingAddress)
        {
            CustomJsonResult result = new CustomJsonResult();


            if (shippingAddress.Id == 0)
            {
                shippingAddress.CreateTime = this.DateTime;
                shippingAddress.Creator = operater;
                CurrentDb.ShippingAddress.Add(shippingAddress);
                CurrentDb.SaveChanges();

            }
            else
            {

                var l_shippingAddress = CurrentDb.ShippingAddress.Where(m => m.Id == shippingAddress.Id).FirstOrDefault();

                l_shippingAddress.Receiver = shippingAddress.Receiver;
                l_shippingAddress.PhoneNumber = shippingAddress.PhoneNumber;
                l_shippingAddress.Area = shippingAddress.Area;
                l_shippingAddress.Address = shippingAddress.Address;
                l_shippingAddress.IsDefault = shippingAddress.IsDefault;
                CurrentDb.SaveChanges();
            }

            if (shippingAddress.IsDefault)
            {
                var list = CurrentDb.ShippingAddress.Where(m => m.UserId == shippingAddress.UserId).ToList();


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
