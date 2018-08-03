using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Newtonsoft.Json;
using Lumos.Common;

namespace Lumos.BLL
{

    public class ProductSkuProvider : BaseProvider
    {
        public CustomJsonResult Add(string operater, ProductSku productSku)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                productSku.Id = GuidUtil.New();
                productSku.Creator = operater;
                productSku.CreateTime = this.DateTime;

                CurrentDb.ProductSku.Add(productSku);
                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功");
            }

            return result;
        }
    }
}
