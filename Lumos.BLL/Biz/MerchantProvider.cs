using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class MerchantProvider : BaseProvider
    {
        public CustomJsonResult Add(int operater, Merchant merchant)
        {
            CustomJsonResult result = new CustomJsonResult();

            var l_posMachine = CurrentDb.Merchant.Where(m => m.Name == merchant.Name).FirstOrDefault();
            if (l_posMachine != null)
                return new CustomJsonResult(ResultType.Failure, "商户名称");


            merchant.CreateTime = this.DateTime;
            merchant.Creator = operater;


            CurrentDb.Merchant.Add(merchant);
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "保存成功");
        }

        public CustomJsonResult Edit(int operater, Merchant merchant)
        {
            CustomJsonResult result = new CustomJsonResult();

            var l_merchant = CurrentDb.Merchant.Where(m => m.Id == merchant.Id).FirstOrDefault();
            if (l_merchant == null)
                return new CustomJsonResult(ResultType.Failure, "不存在");

            l_merchant.LastUpdateTime = this.DateTime;
            l_merchant.Mender = operater;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "保存成功");
        }
    }
}
