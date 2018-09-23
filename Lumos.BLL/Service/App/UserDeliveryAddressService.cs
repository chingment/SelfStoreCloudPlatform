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
        public CustomJsonResult Edit(string operater, string userId, RopUserDeliveryAddressEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var l_userDeliveryAddress = CurrentDb.UserDeliveryAddress.Where(m => m.Id == rop.Id).FirstOrDefault();
            if (string.IsNullOrEmpty(l_userDeliveryAddress.Id))
            {
                l_userDeliveryAddress = new UserDeliveryAddress();
                l_userDeliveryAddress.Id = GuidUtil.New();
                l_userDeliveryAddress.UserId = userId;
                l_userDeliveryAddress.Consignee = rop.Consignee;
                l_userDeliveryAddress.PhoneNumber = rop.PhoneNumber;
                l_userDeliveryAddress.AreaName = rop.AreaName;
                l_userDeliveryAddress.AreaCode = rop.AreaCode;
                l_userDeliveryAddress.Address = rop.Address;
                l_userDeliveryAddress.IsDefault = rop.IsDefault;
                l_userDeliveryAddress.CreateTime = this.DateTime;
                l_userDeliveryAddress.Creator = operater;
                CurrentDb.UserDeliveryAddress.Add(l_userDeliveryAddress);
                CurrentDb.SaveChanges();

            }
            else
            {
                l_userDeliveryAddress.Consignee = rop.Consignee;
                l_userDeliveryAddress.PhoneNumber = rop.PhoneNumber;
                l_userDeliveryAddress.AreaName = rop.AreaName;
                l_userDeliveryAddress.Address = rop.Address;
                l_userDeliveryAddress.IsDefault = rop.IsDefault;
                l_userDeliveryAddress.MendTime = this.DateTime;
                l_userDeliveryAddress.Creator = operater;
                CurrentDb.SaveChanges();
            }

            if (rop.IsDefault)
            {
                var list = CurrentDb.UserDeliveryAddress.Where(m => m.UserId == userId).ToList();


                foreach (var item in list)
                {
                    if (item.Id != rop.Id)
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
